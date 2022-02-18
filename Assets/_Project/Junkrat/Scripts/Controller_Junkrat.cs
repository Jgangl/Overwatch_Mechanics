using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Controller_Junkrat : MonoBehaviour
{
    [SerializeField] private float _grenadeFireRate = 1.5f;
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private Transform _grenadeSpawnPoint;
    [SerializeField] private Transform _mineSpawnPoint;
    [SerializeField] private GameObject _minePrefab;

    [SerializeField] private int _maxNumMines = 2;
    [SerializeField] private float _mineRechargeTime = 5f;
    private int _numAvailableMines;
    private float _currentMineRechargeTime;
    private float _currentMineRechargePercent;

    [SerializeField] private float _mass = 3f;

    private CharacterController _charController;

    private Junkrat_Mine _currentMine;
    private Vector3 _impact;
    private bool _allowGrenadeFiring = true;
    private bool _canThrowMine = true;

    private Coroutine _firstMineRechargeTimer;
    private Coroutine _secondMineRechargeTimer;

    private Camera _gunCamera;
    private Camera _mainCamera;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _numAvailableMines = _maxNumMines;
        _gunCamera = GameObject.FindGameObjectWithTag("GunCamera").GetComponent<Camera>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_impact.magnitude > 0.2) 
            _charController.Move(_impact * Time.deltaTime);

        // consumes the impact energy each cycle:
        _impact = Vector3.Lerp(_impact, Vector3.zero, 4*Time.deltaTime);
    }

    public void AddImpact(Vector3 direction, float force)
    {
        direction.Normalize();

        // Reflect down force on the ground
        if (direction.y < 0)
            direction.y = -direction.y;

        _impact += direction.normalized * force / _mass;
    }

    IEnumerator FireTimer()
    {
        _allowGrenadeFiring = false;
        yield return new WaitForSeconds( 1 / _grenadeFireRate);
        _allowGrenadeFiring = true;
    }

    void Fire()
    {
        if (!_allowGrenadeFiring) return;
        
        // Calculate grenade direction
        
        // Ray out from camera
        //Vector3 targetPoint = _gunCamera.transform.forward * 5;
        //Vector3 targetPoint2 = _mainCamera.transform.forward * 10;
        //Debug.DrawLine(_gunCamera.transform.position, targetPoint, Color.red, 2f);
        //Debug.DrawLine(_mainCamera.transform.position, targetPoint2, Color.yellow, 1f);
        //Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * 100f, Color.red, 1f);
        //Debug.DrawRay(_gunCamera.transform.position, _gunCamera.transform.forward, Color.yellow, 1f);

        //Vector3 grenadeDirection = (_grenadeSpawnPoint.position - targetPoint).normalized;
        //Debug.DrawRay(_grenadeSpawnPoint.position, grenadeDirection * 5f, Color.red, 1f);
        
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 targetPoint = ray.GetPoint(10f);
        targetPoint.y += 1;

        Vector3 grenadeDirection = (targetPoint - _grenadeSpawnPoint.position).normalized;
        Debug.DrawRay(_grenadeSpawnPoint.position, grenadeDirection * 5f, Color.red, 1f);

        GameObject grenade = Instantiate(_grenadePrefab, _grenadeSpawnPoint.position, _grenadeSpawnPoint.rotation);
        grenade.GetComponentInChildren<Junkrat_Grenade>().SetInitialVelocity(grenadeDirection);
        StartCoroutine(FireTimer());
        
        SoundManager_Junkrat.Instance.PlayGrenadeShoot();
    }

    void ThrowMine()
    {
        if (!_canThrowMine || _numAvailableMines == 0) return;
        
        // Destroy previous mine if it exists
        if (_currentMine != null)
        {
            Destroy(_currentMine.gameObject);
        }

        _numAvailableMines -= 1;

        if (_numAvailableMines == 1)
        {
            // Start recharge timer only if available mines is 1
            Coroutine mineRechargeTimer = StartCoroutine(MineRechargeTimer());
        }

        GameObject mineObj = Instantiate(_minePrefab, _mineSpawnPoint.position, _mineSpawnPoint.rotation);
        _currentMine = mineObj.GetComponentInChildren<Junkrat_Mine>();

        // Play mine throw sound
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
        Fire();
    }
    public void OnRightMouseAbility()
    {
        ExplodeCurrentMine();
    }
    public void OnShift_Ability()
    {
        ThrowMine();
    }
    public void OnE_Ability()
    {
        //sprint = newSprintState;
    }

    IEnumerator MineRechargeTimer()
    {
        _currentMineRechargeTime = _mineRechargeTime;
        while (_currentMineRechargeTime > 0f)
        {
            _currentMineRechargeTime -= Time.deltaTime;
            _currentMineRechargePercent = _currentMineRechargeTime / _mineRechargeTime;
            yield return null;
        }

        _numAvailableMines += 1;
        
        print("Num available mines after recharging: " + _numAvailableMines);
        
        // Check num available mines to see if we need to recharge again
        if (_numAvailableMines == 1)
        {
            StartCoroutine(MineRechargeTimer());
        }
    }

    public float GetMineRechargePercent()
    {
        return _currentMineRechargePercent;
    }
    
    public int GetNumAvailableMines()
    {
        return _numAvailableMines;
    }
}
