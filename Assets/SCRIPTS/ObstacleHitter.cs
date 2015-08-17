using UnityEngine;
using System.Collections;

public class ObstacleHitter : MonoBehaviour {
    //Attach this script to each obstacle and set what it does
    PlayerController playerRef;

    //NOTE: Types of wheel or skates can handle the two types of puddles differently
    public enum ObstacleType
    {
        stickPuddle,
        slipPuddle,
        smallObject,
        largeObject,
        noSlideGround
    }
    public ObstacleType obstacleType;

    float _stickPuddleReduc = 30f;
    float _slipPuddleTime = 2f;
    //Should we have stop times for each object?

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Obstacle hit of type " + obstacleType);
            playerRef = other.GetComponent<PlayerController>();
            if (playerRef != null)
                HitObject(obstacleType);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Player" || col.transform.tag == "Animal")
        {
            foreach (ContactPoint contact in col.contacts)
            {
                playerRef = col.transform.GetComponentInParent<PlayerController>();
                Debug.Log(playerRef);
                if (playerRef != null)
                {
                    HitObject(obstacleType, col);
                }
            }
        }
    }

    public void HitObject(ObstacleType oType)
    {
        switch (oType)
        {
            case ObstacleType.stickPuddle:
                playerRef.SlowPlayer(_stickPuddleReduc);
                break;
            case ObstacleType.slipPuddle:
                playerRef.SlipPlayer(_slipPuddleTime);
                break;
            case ObstacleType.smallObject:
                Debug.LogError("Use the function that takes a Collision as a parameter.");
                break;
            case ObstacleType.largeObject:
                //
                break;
            case ObstacleType.noSlideGround:
                //                                                                                    
                break;
            default:
                Debug.LogWarning("There is no obstacle of that type!");
                break;
        }
    }

    public void HitObject(ObstacleType oType, Collision collision)
    {
        switch (oType)
        {
            case ObstacleType.stickPuddle:
                playerRef.SlowPlayer(_stickPuddleReduc);
                break;
            case ObstacleType.slipPuddle:
                playerRef.SlipPlayer(_slipPuddleTime);
                break;
            case ObstacleType.smallObject:
                playerRef.CollisionLaunch(collision);
                break;
            case ObstacleType.largeObject:
                //
                break;
            case ObstacleType.noSlideGround:
                //                                                                                    
                break;
            default:
                Debug.LogWarning("There is no obstacle of that type!");
                break;
        }
    }



}
