﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sacristan.Utils;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [SerializeField]
    private Text followInstructionsText;

    [SerializeField]
    private Text savedNicholases;

    [SerializeField]
    private Text nicholasSavedText;

    private GameCrosshairUI _crosshairUI;

    private bool isPunBeingDisplayed = false;

    public GameCrosshairUI CrosshairUI
    {
        get
        {
            if (_crosshairUI == null) _crosshairUI = GetComponentInChildren<GameCrosshairUI>();
            return _crosshairUI;
        }
    }

    private void Start()
    {
        ShowFollowInstructions(false);
        GameManager.Instance.OnNicholasSaved += GameManager_OnNicholasSaved;
        GameManager.Instance.OnNicholasDied += Instance_OnNicholasDied;
        UpdateUI();
    }

    public void ShowFollowInstructions(bool flag)
    {
        followInstructionsText.enabled = flag;
    }

    private void Instance_OnNicholasDied(Nicholas nicholasSaved)
    {
        UpdateUI();
    }

    private void GameManager_OnNicholasSaved(Nicholas savedNicholas)
    {
        UpdateUI();
        TriggerNicholasSavedText();
    }

    private void TriggerNicholasSavedText()
    {
        nicholasSavedText.gameObject.SetActive(true);

        int cleverPunIndex = Random.Range(0, CleverPuns.NicholasSavedPuns.Length);
        nicholasSavedText.text = CleverPuns.NicholasSavedPuns[cleverPunIndex];

        if(isPunBeingDisplayed) StopCoroutine(DisablePun());
        StartCoroutine(DisablePun());
    }

    private IEnumerator DisablePun()
    {
        isPunBeingDisplayed = true;
        yield return new WaitForSeconds(3f);
        nicholasSavedText.gameObject.SetActive(false);
        isPunBeingDisplayed = false;
    }

    private void UpdateUI()
    {
        savedNicholases.text = string.Format("{0}/{1}", GameManager.Instance.NicholasesSaved, GameManager.Instance.NicholasesTotal);
    }
}
