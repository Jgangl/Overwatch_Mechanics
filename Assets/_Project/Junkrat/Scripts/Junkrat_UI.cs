using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Junkrat_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _mineNumberText;
    [SerializeField] private Image _mineRechargeImage;

    private Controller_Junkrat _playerJunkrat;
    
    private void Start()
    {
        _playerJunkrat = FindObjectOfType<Controller_Junkrat>();
    }

    private void Update()
    {
        float mineRechargePercent = _playerJunkrat.GetMineRechargePercent();
        int numAvailableMines = _playerJunkrat.GetNumAvailableMines();

        SetRadialPercent(mineRechargePercent);
        SetMineAmountText(numAvailableMines);

    }

    void SetRadialPercent(float perc)
    {
        float currentPerc = Mathf.Clamp(perc, 0f, 1f);
        _mineRechargeImage.fillAmount = currentPerc;
    }

    void SetMineAmountText(int amount)
    {
        int mineAmount = Mathf.Clamp(amount, 0, 2);

        _mineNumberText.text = mineAmount.ToString();
    }
}
