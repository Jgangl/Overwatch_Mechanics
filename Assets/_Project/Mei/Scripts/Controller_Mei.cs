using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Controller_Mei : MonoBehaviour
{
    [SerializeField] private float _primaryFireRate = 1f;
    //[SerializeField] private GameObject _grenadePrefab;
    //[SerializeField] private Transform _grenadeSpawnPoint;
    //[SerializeField] private Transform _mineSpawnPoint;
    //[SerializeField] private GameObject _minePrefab;

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
    [SerializeField] private float _wallDistance = 10f;
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
        _meiWall.DisableWall();
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
                    _meiWall.transform.position = hit.point;
                    _meiWall.EnableWall();
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

    void Fire()
    {
        if (!_allowPrimaryFiring) return;

        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        //Vector3 targetPoint = ray.GetPoint(10f);
        //targetPoint.y += 1;

        //Vector3 grenadeDirection = (targetPoint - _grenadeSpawnPoint.position).normalized;
        //Debug.DrawRay(_grenadeSpawnPoint.position, grenadeDirection * 5f, Color.red, 1f);

        //GameObject grenade = Instantiate(_grenadePrefab, _grenadeSpawnPoint.position, _grenadeSpawnPoint.rotation);
        //grenade.GetComponentInChildren<Junkrat_Grenade>().SetInitialVelocity(grenadeDirection);
        StartCoroutine(FireTimer());
        
        //SoundManager_Junkrat.Instance.PlayGrenadeShoot();
    }

    void TryBuildWall()
    {
        if (!_canBuildWall) return;
        //{
            // Rotate wall 
        //};

        _buildingWall = true;
        
        

        // Build wall

        //GameObject mineObj = Instantiate(_minePrefab, _mineSpawnPoint.position, _mineSpawnPoint.rotation);
        //_currentMine = mineObj.GetComponentInChildren<Junkrat_Mine>();

        // Play mine throw sound
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
            _meiWall.DisableWall();
    }

    void ExplodeCurrentMine()
    {
        if (_currentMine)
        {
            _currentMine.Explode();
            _currentMine = null;
        }
    }

    public void OnLeftMouseAbility()
    {
        if (_buildingWall)
            BuildWall();
        else
            Fire();
    }
    public void OnRightMouseAbility()
    {
        Debug.Log("Right mosue");
        CancelWallBuild();
    }
    public void OnShift_Ability()
    {
        //ThrowMine();
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
