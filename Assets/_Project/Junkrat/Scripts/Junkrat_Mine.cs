using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Junkrat_Mine : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damageRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private Vector3 _startTorque;
    [SerializeField] private GameObject _explosionVFX;
    private Rigidbody _rb;
    private Collider[] _colliders;
    //private Collider _collider;

    private Camera _mainCam;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
        //_collider = GetComponent<Collider>();
        _mainCam = Camera.main;
    }

    private void Start()
    {
        _rb.AddForce(_mainCam.transform.forward * _speed);
        _rb.AddRelativeTorque(_startTorque);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.tag == "Player" || collision.gameObject.layer != LayerMask.NameToLayer("Environment")) return;
        
        _rb.isKinematic = true;

        ContactPoint hitPoint = collision.contacts[0];

        transform.up = hitPoint.normal;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.up, 1f);

        List<RaycastHit> hitsSorted = hits.OrderBy(
                                x => Vector2.Distance(transform.position,x.point)
                            ).ToList();
        
        foreach (RaycastHit hit in hitsSorted)
        {
            if (IsSelfCollider(hit.collider)) continue;

            transform.position = hit.point;
            break;
        }
    }

    public void Explode()
    {
        // Cause spherical force to gameobjects near
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        Physics.OverlapSphereNonAlloc(transform.position, _damageRadius, hitColliders);
        foreach (Collider hitCollider in hitColliders)
        {
            if (!hitCollider || hitCollider.tag == "Grenade" || hitCollider.tag == "Mine") continue;
            
            Rigidbody hitRb = hitCollider.GetComponent<Rigidbody>();

            if (hitRb && hitRb != _rb)
            {
                hitRb.AddExplosionForce(_explosionForce, transform.position, _damageRadius);
            }
            else
            {
                Controller_Junkrat junkController = hitCollider.GetComponent<Controller_Junkrat>();
                if (junkController)
                {
                    Vector3 explosionDirection = hitCollider.transform.position - transform.position;
                    junkController.AddImpact(explosionDirection, _explosionForce);
                }
            }
        }

        GameObject explosionVFX = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        Destroy(explosionVFX, 5f);
        // Grenade particles
        // Grenade sound
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }

    private bool IsSelfCollider(Collider col)
    {
        foreach (Collider colSelf in _colliders)
        {
            if (col == colSelf)
                return true;
        }

        return false;
    }
}
