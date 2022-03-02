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

    private bool _ghostWallBuilding = false;
    private bool _wallBuilt = false;
    
    private void Start()
    {
        _playerMei = FindObjectOfType<Controller_Mei>();
        _meiWall = FindObjectOfType<Mei_Wall>();
        _meiWall.OnWallTimeout += OnWallTimeout;
        
        EnableAvailableWall();
    }

    private void Update()
    {
        float wallRechargePercent = _playerMei.GetWallRechargePercent();
        float wallTimeRemaining = _playerMei.GetWallRechargeTimeRemaining();

        if (Mathf.Approximately(wallRechargePercent, 0f))
        {
            EnableAvailableWall();
        }
        else
        {
            DisableAvailableWall();
        }

        SetFillPercent(wallRechargePercent);
        SetTimeRemainingText(wallTimeRemaining);
    }

    private void OnWallTimeout()
    {
        // Enable cooldown text and fill percent
        
        // Move icon to original position
    }
    
    private void OnStartWallBuild()
    {
        // Enable selected color
        
        // Move icon down and to the right slightly
    }
    
    private void OnStartGhostWallBuild()
    {
        // Enable selected color
        
        // Move icon down and to the right slightly
    }
    
    private void OnStopGhostWallBuild()
    {
        // Enable basic color
        
        // Move icon to original position
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
        _iceWallInnerIcon.gameObject.SetActive(false);

        _iceWallOuterIcon.color = _iceWallBasicOuterIconColor;

        _iceWallAbilityIcon.color = _iceWallAbilityBasicColor;
        
        _iceWallCooldownText.gameObject.SetActive(false);
    }

    void DisableAvailableWall()
    {
        // Change outer icon color
        _iceWallOuterIcon.color = _iceWallSelectedOuterIconColor;
        
        // Change inner icon color
        _iceWallInnerIcon.gameObject.SetActive(true);
        
        // Change ability icon color
        _iceWallAbilityIcon.color = _iceWallAbilitySelectedColor;

        // Enable cooldown text
        _iceWallCooldownText.gameObject.SetActive(true);
    }
}
