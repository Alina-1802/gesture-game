using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class UI : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject levelCompletionPanel;
    public GameObject gameplayDescriptionPanel;
    public GameObject paintingNamePanel;
    public GameObject levelLostPanel;

    float descriptionPanelStartTime = 0;
    float descriptionPanelDurationTime = 10;

    public TMP_Text timerText;

    bool isGameActive = false;


    public bool IsGameActive()
    { 
        return isGameActive; 
    }

    public bool IsLevelStarted()
    {
        if(Time.time - descriptionPanelStartTime > descriptionPanelDurationTime)
        {
            return true;
        }

        return false;
    }

    void Start()
    {
        gameplayDescriptionPanel.SetActive(true);
        paintingNamePanel.SetActive(false);
        levelCompletionPanel.SetActive(false);
        levelLostPanel.SetActive(false);

        timerText.enabled = false;
        timerText.text = string.Empty;
    }


    void Update()
    {
        bool isDataReceived = mainCamera.GetComponent<CameraController>().IsDataReceived();

        if(!isDataReceived)
        {
            descriptionPanelStartTime = Time.time;
        }

        //game started
        if (IsLevelStarted())
        {
            isGameActive = true;
            gameplayDescriptionPanel.SetActive(false);
            paintingNamePanel.SetActive(true);

            //show timer
            timerText.enabled = true;
            float levelTimeLeft = mainCamera.GetComponent<Gameplay>().GetLevelTimeLeft();
            string timeLeftString = levelTimeLeft.ToString("F2");
            timerText.text = timeLeftString;
        }

        bool isLevelCompleted = mainCamera.GetComponent<Gameplay>().IsLevelCompleted();
        bool isTimeOver = mainCamera.GetComponent<Gameplay>().IsTimeOver();

        if (isLevelCompleted) 
        {
            //won game
            levelCompletionPanel.SetActive(true);
            paintingNamePanel.SetActive(false);
            isGameActive = false;
        }
        else if(isTimeOver)
        {
            //lost game
            levelLostPanel.SetActive(true);
            paintingNamePanel.SetActive(false);
            isGameActive = false;
        }
    }
}
