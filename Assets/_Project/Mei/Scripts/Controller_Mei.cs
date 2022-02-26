using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller_Mei : MonoBehaviour
{
    [SerializeField] private float _primaryFireRate = 1f;
    [SerializeField] private ParticleSystem _primaryParticleSystem;

    [SerializeField] private float _wallRechargeTime = 5f;
    private float _currentWallRechargeTime;
    private float _currentWallRechargePercent;

    [SerializeField] private float _mass = 3f;

    private CharacterController _charController;

    private Junkrat_Mine _currentMine;
    private Vector3 _impact;
    private bool _allowPrimaryFiring = true;
    private bool _canBuildWall = true;
    private bool _buildingWall = false;
    private bool _cancelWallBuild = false;

    [SerializeField] private Mei_Wall _wallPrefab;
    [SerializeField] private float _wallDistance = 30f;
    [SerializeField] private LayerMask _wallMask;
    private Mei_Wall _meiWall;

    private Coroutine _wallRechargeTimer;

    private Camera _gunCamera;
    private Camera _mainCamera;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _gunCamera = GameObject.FindGameObjectWithTag("GunCamera").GetComponent<Camera>();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _meiWall = Instantiate(_wallPrefab, Vector3.zero, Quaternion.identity);
        
        StopFirePrimary();
    }

    private void Update()
    {
        if (_buildingWall)
        {
            Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _wallDistance, _wallMask))
            {
                if (_wallPrefab)
                {
                    _meiWall.SetWallPosition(hit.point);
                    _meiWall.SetWallRotation(transform.rotation.eulerAngles.y);
                }
            }
        }
    }

    IEnumerator FireTimer()
    {
        _allowPrimaryFiring = false;
        yield return new WaitForSeconds( 1 / _primaryFireRate);
        _allowPrimaryFiring = true;
    }

    void StartFirePrimary()
    {
        _primaryParticleSystem.Play();

        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        //Vector3 targetPoint = ray.GetPoint(10f);
        //targetPoint.y += 1;

        //Vector3 grenadeDirection = (targetPoint - _grenadeSpawnPoint.position).normalized;
        //Debug.DrawRay(_grenadeSpawnPoint.position, grenadeDirection * 5f, Color.red, 1f);
    }
    
    void StopFirePrimary()
    {
        //Debug.Log("Stop primary fire");
        if (_primaryParticleSystem.isPlaying)
        {
            
            _primaryParticleSystem.Stop();
        }
    }
    
    void FireSecondary()
    {
    }

    void TryBuildWall()
    {
        if (!_buildingWall)
        {
            _buildingWall = true;
            _meiWall.StartGhostBuild();
        }
        else
        {
            _meiWall.RotateWall();
        }
    }

    void BuildWall()
    {
        _buildingWall = false;
        
        _meiWall.StartBuildingWall();
        //StartCoroutine(WallRechargeTimer());
    }

    void CancelWallBuild()
    {
        if (_buildingWall)
        {
            _meiWall.StopWallBuild();
            _buildingWall = false;
        }
    }

    public void OnLeftMouseAbility(InputValue value)
    {
        if (value.isPressed)
        {
            if (_buildingWall)
                BuildWall();
            else
                StartFirePrimary();
        }
        else
        {
            StopFirePrimary();
        }
    }
    public void OnRightMouseAbility(InputValue value)
    {
        if (_buildingWall)
            CancelWallBuild();
        else
            FireSecondary();
    }
    public void OnShift_Ability()
    {
        
    }
    public void OnE_Ability()
    {
        TryBuildWall();
    }

    IEnumerator WallRechargeTimer()
    {
        _currentWallRechargeTime = _wallRechargeTime;
        while (_currentWallRechargeTime > 0f)
        {
            _currentWallRechargeTime -= Time.deltaTime;
            _currentWallRechargePercent = _currentWallRechargeTime / _wallRechargeTime;
            yield return null;
        }
    }

    public float GetWallRechargePercent()
    {
        return _currentWallRechargePercent;
    }
}
