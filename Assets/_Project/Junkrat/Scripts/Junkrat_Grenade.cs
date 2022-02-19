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
    [SerializeField] private AudioClip _explosionAudioClip;
    [SerializeField] private AudioClip _fuseAudioClip;
    [SerializeField] private AudioClip _targetHitAudioClip;
    [SerializeField] private AudioSource _explosionAudioSource;
    [SerializeField] private AudioSource _fuseAudioSource;
    [SerializeField] private AudioSource _targetHitAudioSource;
    
    private Rigidbody _rb;
    private Collider _col;
    private MeshRenderer _meshRenderer;
    private Camera _mainCam;

    private bool timerStarted = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _mainCam = Camera.main;

        PlayFuseClip();
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
            PlayTargetHitClip();
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
        Destroy(explosionVFX, 2f);

        StopAllCoroutines();
        
        DisableObject();
        
        // Mine sound
        PlayExplosionClip();
        
        Destroy(gameObject.transform.parent.gameObject, 1f);
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

    private void PlayExplosionClip()
    {
        _fuseAudioSource.Stop();
        
        _explosionAudioSource.Stop();
        _explosionAudioSource.clip = _explosionAudioClip;
        _explosionAudioSource.Play();
    }

    private void DisableObject()
    {
        _meshRenderer.enabled = false;
        _col.enabled = false;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
        Transform glowParticle = transform.Find("Glow_Particle");
        if (glowParticle != null)
            glowParticle.gameObject.SetActive(false);
    }
    
    private void PlayFuseClip()
    {
        _fuseAudioSource.Stop();
        _fuseAudioSource.clip = _fuseAudioClip;
        _fuseAudioSource.Play();
    }
    
    private void PlayTargetHitClip()
    {
        _targetHitAudioSource.Stop();
        _targetHitAudioSource.clip = _targetHitAudioClip;
        _targetHitAudioSource.Play();
    }
}
