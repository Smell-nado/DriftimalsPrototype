using UnityEngine;
using System.Collections;

public class TimingController : MonoBehaviour {

    bool drifting = false;
    Vector3 firstPos;
    Vector3 tapPos;
    float tapDelta;

    float accuracyRatio;

    float boxWidth;

	// Use this for initialization
	void Start () {
        //TODO THIS IS WRONG...gets the set size compared with the actual size
        boxWidth = transform.localScale.z;
        Debug.Log("Local Scale in Z is = " + boxWidth);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //get the position
            firstPos = other.transform.position;
            Debug.Log("Point of contact: " + firstPos);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!drifting)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    tapPos = other.transform.position;
                    tapDelta = Vector3.Distance(tapPos, firstPos);
                    Debug.Log("Distance = " + tapDelta);

                    if (tapDelta < boxWidth)
                    {
                        drifting = true;
                        //get the accuracy
                        accuracyRatio = GetRatio(tapDelta);
                        Debug.Log("Accuracy Ratio = " + accuracyRatio);
                    }
                    else
                    {
                        //not ready to be registered as an attempt
                    }
                }
            }
        }
    }

    float GetRatio(float deltaPos) //0 - 1 // 0.5 is perfect
    {
        return deltaPos / boxWidth;
    }

    void SetAccuracyResult(float ratio)
    {
        if (ratio > 0f && ratio < 0.25f)
        {
            //It's too early
        }
        if (ratio > 0.25f && ratio < 0.40f)
        {
            //Good
        }
        if (ratio > 0.40f && ratio < 0.48f)
        {
            //Great
        }
        if (ratio > 0.48f && ratio < 0.52f)
        {
            //Prefect
        }
        if (ratio > 0f && ratio < 0.25f)
        {
            //TODO
        }
    }
}
