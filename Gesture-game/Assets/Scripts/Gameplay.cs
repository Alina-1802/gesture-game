using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    Transform targetObject; //current

    public Transform targetObject1; // AmericanGothic
    public Transform targetObject2; // Sunflowers
    public Transform targetObject3; // MonaLisa
    public Transform targetObject4; // StarryNight
    public Transform targetObject5; // TheKiss
    public Transform targetObject6; // LadyWithTheErmine
    public Transform targetObject7; // WhistlersMother
    public Transform targetObject8; // TheYoungLadiesOfAvignon

    float[] flagArray;
    int numberDetectedPaintings;
    public float maxAngle = 10f;

    bool timeConditionMet = false;
    float detectionStartTime = 0f;
    public float requiredTime = 3f; // 3 seconds

    // Start is called before the first frame update
    void Start()
    {
        flagArray = new float[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        numberDetectedPaintings = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(numberDetectedPaintings)
        {
            case 0: targetObject = targetObject1; break;
            case 1: targetObject = targetObject2; break;
            case 2: targetObject = targetObject3; break;
            case 3: targetObject = targetObject4; break;
            case 4: targetObject = targetObject5; break;
            case 5: targetObject = targetObject6; break;
            case 6: targetObject = targetObject7; break;
            case 7: targetObject = targetObject8; break;      
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
                flagArray[numberDetectedPaintings] = 1;
                numberDetectedPaintings++;
                timeConditionMet = false;
            }

        }
        else
        {
            //Debug.Log("Object is not detected");
            timeConditionMet = false;
        }

        Debug.Log("american: " + flagArray[0]);
        Debug.Log("sunflowers: " + flagArray[1]);
        Debug.Log("mona lisa: " + flagArray[2]);
    }
}
