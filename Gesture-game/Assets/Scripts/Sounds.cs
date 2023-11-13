using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isObjectDetected = mainCamera.GetComponent<Gameplay>().IsObjectDetected();
        bool isObjectDetectedRequiredTime = mainCamera.GetComponent<Gameplay>().IsObjectDetectedRequiredTime();

        if (isObjectDetected && isObjectDetectedRequiredTime) //to poprawic
        {
            GetComponentInChildren<AudioSource>().Play();
        }
    }
}
