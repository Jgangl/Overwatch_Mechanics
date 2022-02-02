using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Controller_Junkrat : MonoBehaviour
{
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private Transform _grenadeSpawnPoint;
    [SerializeField] private GameObject _minePrefab;

    [SerializeField] private int maxNumMines;

    [SerializeField] private float _mass = 3f;

    private CharacterController _charController;
    
    private Queue<Junkrat_Mine> _currentMines = new Queue<Junkrat_Mine>();
    private Vector3 impact;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (impact.magnitude > 0.2) 
            _charController.Move(impact * Time.deltaTime);
        
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 4*Time.deltaTime);
    }

    public void AddImpact(Vector3 direction, float force)
    {
        direction.Normalize();

        // Reflect down force on the ground
        if (direction.y < 0)
            direction.y = -direction.y;

        impact += direction.normalized * force / _mass;
    }

    void Fire()
    {
        GameObject grenade = Instantiate(_grenadePrefab, _grenadeSpawnPoint.position, _grenadeSpawnPoint.rotation);
    }

    void ThrowMine()
    {
        GameObject mineObj = Instantiate(_minePrefab, _grenadeSpawnPoint.position, _grenadeSpawnPoint.rotation);
        Junkrat_Mine mine = mineObj.GetComponentInChildren<Junkrat_Mine>();
        AddMine(mine);
    }

    void ExplodeCurrentMines()
    {
        for (int i = 0; i < _currentMines.Count; i++)
        {
            Junkrat_Mine mineToRemove = _currentMines.Dequeue();
            RemoveMine(mineToRemove);
        }
    }

    private void AddMine(Junkrat_Mine newMine)
    {
        int numMines = _currentMines.Count;

        if (numMines < 2)
        {
            _currentMines.Enqueue(newMine);
        }
        else if (numMines == 2)
        {
            Junkrat_Mine mineToRemove = _currentMines.Dequeue();
            RemoveMine(mineToRemove);
            
            _currentMines.Enqueue(newMine);
        }
    }

    private void RemoveMine(Junkrat_Mine mineToRemove)
    {
        mineToRemove.Explode();
    }

    public void OnLeftMouseAbility()
    {
        Fire();
    }
    public void OnRightMouseAbility()
    {
        ExplodeCurrentMines();
    }
    public void OnShift_Ability()
    {
        ThrowMine();
    }
    public void OnE_Ability()
    {
        //sprint = newSprintState;
    }
}
