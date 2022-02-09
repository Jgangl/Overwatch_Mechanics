using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junkrat_Grenade : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damageRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionDelay;
    [SerializeField] private GameObject _explosionVFX;
    
    private Rigidbody _rb;
    private Camera _mainCam;

    private bool timerStarted = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
    }

    private void Start()
    {
        
    }

    public void SetInitialVelocity(Vector3 initalDirection)
    {
        _rb.AddForce(initalDirection * _speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.tag == "Player") return;

        if (collision.gameObject.tag == "Enemy")
        {
            Explode();
        }
        else
        {
            StartExplosionTimer();
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
        }
        
        GameObject explosionVFX = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        Destroy(explosionVFX, 5f);
        // Mine particles
        // Mine sound
        Destroy(gameObject);
    }

    private void StartExplosionTimer()
    {
        if (timerStarted) return;
        
        timerStarted = true;
        StartCoroutine(ExplosionTimer(_explosionDelay));
    }

    IEnumerator ExplosionTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Explode();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}
