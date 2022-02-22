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

    private MeshRenderer _meshRenderer;
    private Collider _col;
    private Rigidbody _rb;

    private bool _wallRotated = false;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _col = GetComponentInChildren<Collider>();
        _rb = GetComponentInChildren<Rigidbody>();

        DisableWall();
    }

    public void StartBuildingWall()
    {
        transform.localScale = new Vector3(transform.localScale.x, _wallMinScale, transform.localScale.z);
        _col.enabled = true;
        //_rb.isKinematic = false;
        //_rb.useGravity = false;
        StartCoroutine(BuildWallCoroutine());
    }

    IEnumerator BuildWallCoroutine()
    {
        float progress = 0;

        float localYScale = _wallMinScale;
     
        while(progress <= 1)
        {
            localYScale = Mathf.Lerp(_wallMinScale, _wallMaxScale, progress);
            //transform.localScale = Vector3.Lerp(, FinalScale, progress);
            transform.localScale = new Vector3(transform.localScale.x, localYScale, transform.localScale.z);
            progress += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(WallTimerCoroutine());
    }

    IEnumerator WallTimerCoroutine()
    {
        yield return new WaitForSeconds(_wallTimeoutTime);
        StopWallBuild();
    }

    public void StopWallBuild()
    {
        _meshRenderer.enabled = false;
        _col.enabled = false;
    }

    public void StartGhostBuild()
    {
        _meshRenderer.enabled = true;
        _col.enabled = false;

        // Default wall to not rotated
        _wallRotated = false;
        
        transform.localScale = new Vector3(transform.localScale.x, _wallMinScale, transform.localScale.z);
        
        // Show Particles where wall will show up
    }

    public void EnableWall()
    {
        _meshRenderer.enabled = true;
        _col.enabled = false;
    }

    public void DisableWall()
    {
        _meshRenderer.enabled = false;
        _col.enabled = false;
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
}
