using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    Transform targetObject; //current

    public Transform targetObject1; // LadyWithTheErmine
    public Transform targetObject2; // Girl with a Pearl Earring
    public Transform targetObject3; // MonaLisa
    public Transform targetObject4; // StarryNight
    public Transform targetObject5; // TheKiss
    public Transform targetObject6; // AmericanGothic
    public Transform targetObject7; // WhistlersMother
    public Transform targetObject8; // TheYoungLadiesOfAvignon

    int numberDetectedPaintings = 0;
    public float maxAngle = 10f;

    bool timeConditionMet = false;
    float detectionStartTime = 0f;
    public float requiredTime = 3f; // 3 seconds

    public TMP_Text paintingTitle;

    void Start()
    {
        numberDetectedPaintings = 0;
        paintingTitle.text = string.Empty;
    }

    void Update()
    {
        switch(numberDetectedPaintings)
        {
            case 0: targetObject = targetObject1; paintingTitle.text = "Lady with an Ermine";  break;
            case 1: targetObject = targetObject2; paintingTitle.text = "Girl with a Pearl Earring";  break;
            case 2: targetObject = targetObject3; paintingTitle.text = "Mona Lisa";  break;
            case 3: targetObject = targetObject4; paintingTitle.text = "The Starry Night";  break;
            case 4: targetObject = targetObject5; paintingTitle.text = "The Kiss";  break;
            case 5: targetObject = targetObject6; paintingTitle.text = "American Gothic";  break;
            case 6: targetObject = targetObject7; paintingTitle.text = "Whistler's Mother";  break;
            case 7: targetObject = targetObject8; paintingTitle.text = "The Ladies of Avignon";  break;      
        }


        //check if camera is looking at the object
        Vector3 directionToTarget = targetObject.position - Camera.main.transform.position;
        float angle = Vector3.Angle(directionToTarget, Camera.main.transform.forward);

        Debug.DrawLine(transform.position, targetObject.position, Color.blue, 2.5f);
        Debug.DrawLine(Camera.main.transform.forward * 100, transform.position, Color.red, 2.5f);

        if (angle < maxAngle)
        {
            //Debug.Log("Object is detected");

            if (!timeConditionMet)
            {
                detectionStartTime = Time.time;
                timeConditionMet = true;
            }
            if (Time.time - detectionStartTime >= requiredTime)
            {
                if(numberDetectedPaintings < 8)
                {
                    numberDetectedPaintings++;
                    timeConditionMet = false;
                }
                else if(numberDetectedPaintings == 8)
                {
                    Debug.Log("Ukoñczono level");
                }

            }

        }
        else
        {
            //Debug.Log("Object is not detected");
            timeConditionMet = false;
        }

       // Debug.Log("Liczba wykrytych obrazów: " + numberDetectedPaintings);
    }
}
