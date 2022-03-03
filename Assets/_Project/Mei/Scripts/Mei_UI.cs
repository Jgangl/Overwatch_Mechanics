using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Mei_UI : MonoBehaviour
{
    [SerializeField] private Color _iceWallBasicOuterIconColor;
    [SerializeField] private Color _iceWallSelectedOuterIconColor;
    [SerializeField] private Color _iceWallInnerIconColor;
    [SerializeField] private Color _iceWallAbilityBasicColor;
    [SerializeField] private Color _iceWallAbilitySelectedColor;

    [SerializeField] private Image _iceWallOuterIcon;
    [SerializeField] private Image _iceWallInnerIcon;
    [SerializeField] private Image _iceWallAbilityIcon;
    [SerializeField] private TextMeshProUGUI _iceWallCooldownText;

    private Controller_Mei _playerMei;
    private Mei_Wall _meiWall;
    private Animator _animator;

    private bool _ghostWallBuilding = false;
    private bool _wallBuilt = false;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _playerMei = FindObjectOfType<Controller_Mei>();
        _playerMei.OnWallCooldownFinished += OnWallCooldownFinished;
        
        _meiWall = FindObjectOfType<Mei_Wall>();
        _meiWall.OnWallTimeout += OnWallTimeout;
        _meiWall.OnStartWallBuild += OnStartWallBuild;
        _meiWall.OnStartGhostWallBuild += OnStartGhostWallBuild;
        _meiWall.OnStopGhostWallBuild += OnStopGhostWallBuild;
        
        SetDefaultState();
    }

    private void Update()
    {
        float wallRechargePercent = _playerMei.GetWallRechargePercent();
        float wallTimeRemaining = _playerMei.GetWallRechargeTimeRemaining();
/*
        if (Mathf.Approximately(wallRechargePercent, 0f))
        {
            EnableAvailableWall();
        }
        else
        {
            DisableAvailableWall();
        }
*/

        // Update Inner Icon Fill with wall cooldown percent
        SetFillPercent(wallRechargePercent);
        
        // Update Cooldown remaining text
        SetTimeRemainingText(wallTimeRemaining);
    }

    private void OnWallTimeout()
    {
        /*
        // Enable cooldown text and fill percent
        EnableCooldownText(true);
        
        // Enable inner icon for fill cooldown
        EnableInnerIcon(true);
        */
        
        SetCooldownState();
        
        // Move icon to original position
        _animator.SetBool("Selected", false);
    }
    
    private void OnStartWallBuild()
    {
        // No additional functionality needed yet
    }
    
    private void OnStartGhostWallBuild()
    {
        /*
        // Enable selected color
        ChangeOuterIconColor(_iceWallSelectedOuterIconColor);
        
        // Change ability icon color
        ChangeAbilityIconColor(_iceWallAbilitySelectedColor);
        
        */
        
        SetSelectedState();
        
        // Move icon down and to the right slightly
        _animator.SetBool("Selected", true);
    }
    
    private void OnStopGhostWallBuild()
    {
        /*
        // Enable outer icon basic color
        ChangeOuterIconColor(_iceWallBasicOuterIconColor);
        
        // Enable ability icon basic color
        ChangeAbilityIconColor(_iceWallAbilityBasicColor);
        
        */
        
        SetDefaultState();
        
        // Move icon to original position
        _animator.SetBool("Selected", false);
    }

    private void OnWallCooldownFinished()
    {
        SetDefaultState();
        
        /*
        // Enable basic color
        ChangeOuterIconColor(_iceWallBasicOuterIconColor);
        
        // Disable inner icon
        EnableInnerIcon(false);
        
        // Change Ability icon to basic color
        ChangeAbilityIconColor(_iceWallAbilityBasicColor);
        
        // Disable cooldown text
        EnableCooldownText(false);
        */
    }

    void SetFillPercent(float perc)
    {
        float currentPerc = Mathf.Clamp(perc, 0f, 1f);
        _iceWallInnerIcon.fillAmount = currentPerc;
    }

    void SetTimeRemainingText(float seconds)
    {
        _iceWallCooldownText.text = seconds.ToString("F0");
    }

    void EnableAvailableWall()
    {
        //_iceWallInnerIcon.gameObject.SetActive(false);

        //_iceWallOuterIcon.color = _iceWallBasicOuterIconColor;

        //_iceWallAbilityIcon.color = _iceWallAbilityBasicColor;
        
        //_iceWallCooldownText.gameObject.SetActive(false);
    }

    void DisableAvailableWall()
    {
        // Change outer icon color
        //ChangeOuterIconColor(_iceWallSelectedOuterIconColor);
        
        // Change inner icon color
        //_iceWallInnerIcon.gameObject.SetActive(true);
        
        // Change ability icon color
        //ChangeAbilityIconColor(_iceWallAbilitySelectedColor);

        // Enable cooldown text
        //EnableCooldownText(true);
    }

    void SetDefaultState()
    {
        ChangeOuterIconColor(_iceWallBasicOuterIconColor);
        
        ChangeAbilityIconColor(_iceWallAbilityBasicColor);
        
        EnableCooldownText(false);
        
        EnableInnerIcon(false);
    }
    
    void SetSelectedState()
    {
        ChangeOuterIconColor(_iceWallSelectedOuterIconColor);
        
        ChangeAbilityIconColor(_iceWallAbilitySelectedColor);
        
        EnableCooldownText(false);
        
        EnableInnerIcon(false);
    }
    
    void SetCooldownState()
    {
        ChangeOuterIconColor(_iceWallSelectedOuterIconColor);
        
        ChangeAbilityIconColor(_iceWallAbilitySelectedColor);
        
        EnableCooldownText(true);
        
        EnableInnerIcon(true);
    }

    void ChangeOuterIconColor(Color newColor)
    {
        _iceWallOuterIcon.color = newColor;
    }

    void EnableInnerIcon(bool enabled)
    {
        _iceWallInnerIcon.gameObject.SetActive(enabled);
    }

    void ChangeAbilityIconColor(Color newColor)
    {
        _iceWallAbilityIcon.color = newColor;
    }

    void EnableCooldownText(bool enabled)
    {
        _iceWallCooldownText.gameObject.SetActive(enabled);
    }
    
    
}
