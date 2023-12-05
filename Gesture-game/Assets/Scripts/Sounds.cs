using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isObjectDetected = mainCamera.GetComponent<Gameplay>().IsObjectDetected();
        bool isGameActive = canvas.GetComponent<UI>().IsGameActive();
        bool isRequiredTime = mainCamera.GetComponent<Gameplay>().IsDetectionRequiredTime();

        if (isObjectDetected && isGameActive && isRequiredTime)
        {
            GetComponentInChildren<AudioSource>().Play();
        } 
    }
}
