using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mei_Wall : MonoBehaviour
{
    [SerializeField] private float _wallMaxScale;
    [SerializeField] private float _wallMinScale;
    [SerializeField] private float _wallBuildingTime;
    [SerializeField] private float _wallTimeoutTime = 2f;

    [Space(5)]
    
    [Header("Effects")] 
    [SerializeField] private GameObject _breakParticles;
    [SerializeField] private Vector3 _breakParticlesOffset;
    [SerializeField] private ParticleSystem[] _frostParticles;
    [SerializeField] private ParticleSystem[] _ghostSnowParticles;

    [Space(5)]
    
    [Header("Audio")]
    //[SerializeField] private AudioClip _iceWallStartBuildClip;
    //[SerializeField] private AudioClip _iceWallStartPreBuildClip;
    //[SerializeField] private AudioClip _iceWallBreakClip;
    //[SerializeField] private AudioClip _iceWallPreBreakClip;
    //[SerializeField] private AudioClip _iceWallRotateClip;
    //[SerializeField] private AudioClip _iceWallWindClip;
    //[SerializeField] private AudioClip _iceWallErrorClip;
    
    [Space(5)]
    
    [SerializeField] private AudioSource _iceWallStartBuildSource;
    [SerializeField] private AudioSource _iceWallStartPreBuildSource;
    [SerializeField] private AudioSource _iceWallBreakSource;
    [SerializeField] private AudioSource _iceWallPreBreakSource;
    [SerializeField] private AudioSource _iceWallRotateSource;
    [SerializeField] private AudioSource _iceWallWindSource;

    private MeshRenderer[] _meshRenderers;
    private Collider[] _colliders;
    private Rigidbody _rb;

    private bool _wallRotated = false;

    private bool _wallBuilt = false;

    public delegate void WallTimeout();
    public event WallTimeout OnWallTimeout;
    public delegate void StartWallBuild();
    public event StartWallBuild OnStartWallBuild;
    public delegate void StartGhostWallBuild();
    public event StartGhostWallBuild OnStartGhostWallBuild;
    public delegate void StopGhostWallBuild();
    public event StopGhostWallBuild OnStopGhostWallBuild;

    private void Awake()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _colliders = GetComponentsInChildren<Collider>();
        _rb = GetComponentInChildren<Rigidbody>();

        DisableWall();
    }

    private void Start()
    {
        EnableFrostParticles(false);
        EnableGhostParticles(false);
    }

    public void StartBuildingWall()
    {
        _wallBuilt = true;
        
        EnableRenderers(true);
        
        transform.localScale = new Vector3(transform.localScale.x, _wallMinScale, transform.localScale.z);
        EnableColliders(true);
        //_rb.isKinematic = false;
        //_rb.useGravity = false;
        StartCoroutine(BuildWallCoroutine());
        
        OnStartWallBuild?.Invoke();
        
        _iceWallWindSource.Stop();
        _iceWallStartBuildSource.Play();
    }

    IEnumerator BuildWallCoroutine()
    {
        EnableFrostParticles(true);
        float progress = 0;
        float currentBuildingTime = 0f;

        float localYScale = _wallMinScale;
     
        while(currentBuildingTime <= _wallBuildingTime)
        {
            // Map to wall scale progress from 0 to 1

            progress = map(currentBuildingTime, 0f, _wallBuildingTime, 0f, 1f);
            
            localYScale = Mathf.Lerp(_wallMinScale, _wallMaxScale, progress);
            //transform.localScale = Vector3.Lerp(, FinalScale, progress);
            transform.localScale = new Vector3(transform.localScale.x, localYScale, transform.localScale.z);
            currentBuildingTime += Time.deltaTime;
            yield return null;
        }

        //EnableFrostParticles(true);
        
        StartCoroutine(WallTimerCoroutine());
    }
    
    float map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    IEnumerator WallTimerCoroutine()
    {
        yield return new WaitForSeconds(_wallTimeoutTime);
        
        if (_wallBuilt)
            DestroyWall();
    }

    public void StopGhostBuild()
    {
        EnableGhostParticles(false);
        
        //EnableRenderers(false);
        EnableColliders(false);
        
        OnStopGhostWallBuild?.Invoke();
        
        _iceWallStartPreBuildSource.Stop();
        _iceWallWindSource.Stop();
    }

    public void StartGhostBuild()
    {
        EnableGhostParticles(true);
        
        //EnableRenderers(true);
        EnableColliders(false);

        // Default wall to not rotated
        _wallRotated = false;
        
        transform.localScale = new Vector3(transform.localScale.x, _wallMinScale, transform.localScale.z);
        
        OnStartGhostWallBuild?.Invoke();
        
        _iceWallStartPreBuildSource.Play();
        _iceWallWindSource.Play();
        
        // Show Particles where wall will show up
    }

    public void DestroyWall()
    {
        _wallBuilt = false;
        
        DisableWall();

        EnableFrostParticles(false);
        EnableGhostParticles(false);
        
        OnWallTimeout?.Invoke();

        Vector3 particlesRotation = _breakParticles.transform.rotation.eulerAngles;
        
        Vector3 particleRotation = new Vector3(particlesRotation.x, particlesRotation.y, transform.rotation.eulerAngles.y);

        GameObject wallBreakParticles = Instantiate(_breakParticles, transform.position + _breakParticlesOffset, Quaternion.Euler(particleRotation));
        Destroy(wallBreakParticles, 5f);
        
        _iceWallBreakSource.Play();
    }

    public void EnableWall()
    {
        EnableRenderers(true);
        EnableColliders(false);
    }

    public void DisableWall()
    {
        EnableRenderers(false);
        EnableColliders(false);
    }

    public void RotateWall()
    {
        _wallRotated = !_wallRotated;
        _iceWallRotateSource.Stop();
        _iceWallRotateSource.Play();
    }

    public void SetWallPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetWallRotation(float yRot)
    {
        if (_wallRotated)
            yRot += 90f;
        
        Vector3 currentWallRotation = transform.rotation.eulerAngles;
        Vector3 newWallRotation = new Vector3(currentWallRotation.x, yRot, currentWallRotation.z);
        
        transform.rotation = Quaternion.Euler(newWallRotation);
    }

    public void EnableRenderers(bool enabled)
    {
        foreach (MeshRenderer renderer in _meshRenderers)
        {
            renderer.enabled = enabled;
        }
    }
    
    public void EnableColliders(bool enabled)
    {
        foreach (Collider collider in _colliders)
        {
            collider.enabled = enabled;
        }
    }

    private void EnableFrostParticles(bool enabled)
    {
        foreach (ParticleSystem particle in _frostParticles)
        {
            if (enabled)
                particle.Play();
            else
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void EnableGhostParticles(bool enabled)
    {
        foreach (ParticleSystem particle in _ghostSnowParticles)
        {
            if (enabled)
                particle.Play();
            else
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
