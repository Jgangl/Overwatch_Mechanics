using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mei_Wall : MonoBehaviour
{
    [SerializeField] private float _wallMaxScale;
    [SerializeField] private float _wallMinScale;
    [SerializeField] private float _wallBuildingTime;

    private MeshRenderer _meshRenderer;
    private Collider _col;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _col = GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBuildingWall()
    {
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
        
        //transform.localScale = FinalScale;
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
}
