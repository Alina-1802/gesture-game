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

    public GameObject UIPanels;

    private int numberDetectedPaintings = 0;
    public float maxAngle = 10f;

    private bool timeConditionMet = false;
    private float detectionStartTime;
    public float requiredTime = 3f; // 3 seconds

    public TMP_Text paintingTitle;

    float levelTimeLeft = 60f;
    bool stopTimer = false;

    public float GetLevelTimeLeft()
    {
        return levelTimeLeft;
    }

    public bool IsLevelStarted()
    {
        bool isLevelStarted = UIPanels.GetComponent<UI>().IsLevelStarted();
        return isLevelStarted;
    }

    public bool IsFaceScriptConnected()
    {
        bool isFaceScriptConnected = GetComponent<CameraController>().IsDataReceived();
        return isFaceScriptConnected;
    }

    public bool IsLevelOver()
    {
        if (IsLevelCompleted() || IsTimeOver())
        {
            return true;
        }

        return false;
    }

    public bool IsTimeOver()
    {
        if(levelTimeLeft <= 0)
        {
            return true;
        }

        return false;

    }

    public bool IsLevelCompleted()
    {
        if(numberDetectedPaintings == 8 && levelTimeLeft > 0)
        {
            return true;
        }

        return false;
    }

    public bool IsObjectDetected()
    {
        Vector3 directionToTarget = targetObject.position - Camera.main.transform.position;
        float angle = Vector3.Angle(directionToTarget, Camera.main.transform.forward);

        if (angle < maxAngle)
        {
            return true;
        }

        return false;
    }

    public bool IsDetectionRequiredTime()
    {
        if (Time.time - detectionStartTime >= requiredTime)
        {
            return true;
        }

        return false;
    }

    void SetCurrentTargetObject()
    {
        switch (numberDetectedPaintings)
        {
            case 0: targetObject = targetObject1; paintingTitle.text = "Lady with an Ermine"; break;
            case 1: targetObject = targetObject2; paintingTitle.text = "Girl with a Pearl Earring"; break;
            case 2: targetObject = targetObject3; paintingTitle.text = "Mona Lisa"; break;
            case 3: targetObject = targetObject4; paintingTitle.text = "The Starry Night"; break;
            case 4: targetObject = targetObject5; paintingTitle.text = "The Kiss"; break;
            case 5: targetObject = targetObject6; paintingTitle.text = "American Gothic"; break;
            case 6: targetObject = targetObject7; paintingTitle.text = "Whistler's Mother"; break;
            case 7: targetObject = targetObject8; paintingTitle.text = "The Ladies of Avignon"; break;
        }
    }

    void Start()
    {
        numberDetectedPaintings = 0;
        paintingTitle.text = string.Empty;
        SetCurrentTargetObject();
        detectionStartTime = Time.time;
    }

    void Update()
    {
        //connect python script

        //load level description

        //start level
        if(IsFaceScriptConnected() && IsLevelStarted())
        {
            SetCurrentTargetObject();

            //level timer
            if(!stopTimer)
            {
                levelTimeLeft -= Time.deltaTime;
                //Debug.Log(levelTimeLeft);
            }

            if (IsTimeOver())
            {
                //Debug.Log("Koniec czasu!");
                levelTimeLeft = 0;
                stopTimer = true;
            }

            //Debug.DrawLine(transform.position, targetObject.position, Color.blue, 2.5f);
            //Debug.DrawLine(Camera.main.transform.forward * 100, transform.position, Color.red, 2.5f);

            //check if painting is detected
            if (IsObjectDetected())
            {
                if (!timeConditionMet)
                {
                    detectionStartTime = Time.time;
                    timeConditionMet = true;
                }

                if (IsDetectionRequiredTime())
                {
                    if (!IsLevelCompleted())
                    {
                        numberDetectedPaintings++;
                        timeConditionMet = false;
                        //Debug.Log("Painting detected!");
                    }
                    if(IsLevelCompleted())
                    {
                        //Debug.Log("You won!");
                        stopTimer = true;
                    }
                }
            }
            else
            {
                timeConditionMet = false;
            }
        }

        
    }
}
