using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Junkrat_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _mineNumberText;
    [SerializeField] private Image _mineRechargeOuterRing;
    [SerializeField] private Image _mineRechargeCenter;

    [SerializeField] private Color _minesFullRingColor;
    [SerializeField] private Color _minesNotFullRingColor;
    
    [SerializeField] private Color _minesFullTextColor;
    [SerializeField] private Color _minesNotFullTextColor;

    private Controller_Junkrat _playerJunkrat;
    
    private void Start()
    {
        _playerJunkrat = FindObjectOfType<Controller_Junkrat>();
    }

    private void Update()
    {
        float mineRechargePercent = _playerJunkrat.GetMineRechargePercent();
        int numAvailableMines = _playerJunkrat.GetNumAvailableMines();

        if (numAvailableMines == 2)
        {
            EnableFullMines();
        }
        else
        {
            DisableFullMines();
        }

        SetRadialPercent(mineRechargePercent);
        SetMineAmountText(numAvailableMines);
    }

    void SetRadialPercent(float perc)
    {
        float currentPerc = Mathf.Clamp(perc, 0f, 1f);
        _mineRechargeOuterRing.fillAmount = (1 - currentPerc);
    }

    void SetMineAmountText(int amount)
    {
        int mineAmount = Mathf.Clamp(amount, 0, 2);

        _mineNumberText.text = mineAmount.ToString();
    }

    void EnableFullMines()
    {
        // Enable center image
        _mineRechargeCenter.enabled = true;
        
        // Change outer ring color to white
        _mineRechargeOuterRing.color = _minesFullRingColor;

        // Change number text color to grey
        _mineNumberText.color = _minesFullTextColor;
    }

    void DisableFullMines()
    {
        // Disable center image
        _mineRechargeCenter.enabled = false;
        
        // Change outer ring color to yellow during recharge
        _mineRechargeOuterRing.color = _minesNotFullRingColor;
        
        // Change number text color to white
        _mineNumberText.color = _minesNotFullTextColor;
    }
}
