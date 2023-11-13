using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject levelCompletionPanel;
    public GameObject gameplayDescriptionPanel;
    public GameObject paintingNamePanel;

    //temp
    float startTime = 0;
    float timePeroid = 10;

    void Start()
    {
        gameplayDescriptionPanel.SetActive(true);
        paintingNamePanel.SetActive(false);
        levelCompletionPanel.SetActive(false);
    }


    void Update()
    {
        bool isDataReceived = mainCamera.GetComponent<CameraController>().IsDataReceived();

        if(!isDataReceived)
        {
            startTime = Time.time;
        }


        if (Time.time - startTime > timePeroid)
        {
            gameplayDescriptionPanel.SetActive(false);
            paintingNamePanel.SetActive(true);
        }

        bool isLevelCompleted = mainCamera.GetComponent<Gameplay>().IsLevelCompleted();

        if(isLevelCompleted) 
        {
            levelCompletionPanel.SetActive(true);
            paintingNamePanel.SetActive(false);
        }
    }
}
