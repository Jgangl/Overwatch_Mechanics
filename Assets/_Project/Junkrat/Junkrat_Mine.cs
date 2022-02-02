using System;
using UnityEngine;

public class Junkrat_Mine : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damageRadius;
    [SerializeField] private float _explosionForce;
    private Rigidbody _rb;
    private Collider _collider;

    private Camera _mainCam;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _mainCam = Camera.main;
    }

    private void Start()
    {
        _rb.AddForce(_mainCam.transform.forward * _speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.tag == "Player") return;

        transform.parent = collision.transform;
        _rb.isKinematic = true;

        ContactPoint hitPoint = collision.contacts[0];

        transform.up = hitPoint.normal;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            transform.position = hit.point;
            transform.position += transform.TransformDirection (new Vector3(0f, _collider.bounds.extents.y, 0f));
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
        // Grenade particles
        // Grenade sound
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}
