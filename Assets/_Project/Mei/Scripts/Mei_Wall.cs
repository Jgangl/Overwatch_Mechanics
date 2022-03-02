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

    private MeshRenderer[] _meshRenderers;
    private Collider[] _colliders;
    private Rigidbody _rb;

    private bool _wallRotated = false;

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

    public void StartBuildingWall()
    {
        transform.localScale = new Vector3(transform.localScale.x, _wallMinScale, transform.localScale.z);
        EnableColliders(true);
        //_rb.isKinematic = false;
        //_rb.useGravity = false;
        StartCoroutine(BuildWallCoroutine());
    }

    IEnumerator BuildWallCoroutine()
    {
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

        StartCoroutine(WallTimerCoroutine());
    }
    
    float map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    IEnumerator WallTimerCoroutine()
    {
        yield return new WaitForSeconds(_wallTimeoutTime);
        
        DestroyWall();
    }

    public void StopWallBuild()
    {
        EnableRenderers(false);
        EnableColliders(false);
    }

    public void StartGhostBuild()
    {
        EnableRenderers(true);
        EnableColliders(false);

        // Default wall to not rotated
        _wallRotated = false;
        
        transform.localScale = new Vector3(transform.localScale.x, _wallMinScale, transform.localScale.z);
        
        // Show Particles where wall will show up
    }

    public void DestroyWall()
    {
        StopWallBuild();

        OnWallTimeout?.Invoke();
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
}
