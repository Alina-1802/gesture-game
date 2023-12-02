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

    //gameplayDescriptionPanel time
    float descriptionPanelStartTime = 0;
    float descriptionPanelDurationTime = 10;

    public TMP_Text timerText;

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
            levelCompletionPanel.SetActive(true);
            paintingNamePanel.SetActive(false);
        }
        else if(isTimeOver)
        {
            //handle lost game
            //Debug.Log("You lost!");
        }
    }
}
