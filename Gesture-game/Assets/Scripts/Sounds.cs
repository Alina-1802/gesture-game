using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject canvas;

    bool isObjectDetected;
    bool isGameActive;
    bool isRequiredTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isObjectDetected = mainCamera.GetComponent<Gameplay>().IsObjectDetected();
        isGameActive = canvas.GetComponent<UI>().IsGameActive();
        isRequiredTime = mainCamera.GetComponent<Gameplay>().IsDetectionRequiredTime();
        Debug.Log(isRequiredTime);


        if (isGameActive && isObjectDetected && isRequiredTime)
        {
            GetComponentInChildren<AudioSource>().Play();
        } 
    }
}
