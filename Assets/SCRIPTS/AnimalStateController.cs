using UnityEngine;
using System.Collections;

public class AnimalStateController : MonoBehaviour {

    public enum animalStates
    {
        stopped = 0,
        slow1 = 1,
        slow2 = 2,
        slow3 = 3,
        travelling = 4,
        super = 5,
        complete = 6,
        crashed = 7
    }
    public static animalStates curentState;

	// Use this for initialization
	void Start () {
        curentState = animalStates.travelling;
	}

    //So I can pass in an int if I want
    public void ChangeState(int newState) {
        animalStates newStateCast = (animalStates)newState;
        ChangeState(newStateCast);
    }

    public void ChangeState(animalStates newState) {
        switch (newState) {
            case animalStates.stopped:
                curentState = animalStates.stopped;
                Debug.Log("State is Stopped");
                //What happens when stopped
                //Wait a second and change to slow 1
                break;
            case animalStates.slow1:
                curentState = animalStates.slow1;
                Debug.Log("State is slow1");
                //What happens when slow1
                //accelerate each frame
                //when speed hits xchange to slow 2
                break;
            case animalStates.slow2:
                curentState = animalStates.slow2;
                Debug.Log("State is slow2");
                //What happens when slow2
                //accelerate each frame
                //change to slow 3
                break;
            case animalStates.slow3:
                curentState = animalStates.slow3;
                Debug.Log("State is slow3");
                //What happens when slow3
                //accelerate each frame
                //change to travelling
                break;
            case animalStates.travelling:
                curentState = animalStates.travelling;
                Debug.Log("State is travelling");
                //What happens when travelling
                //accelerate each frame
                //when speed hits x change to super
                break;
            case animalStates.super:
                curentState = animalStates.super;
                Debug.Log("State is super");
                //What happens when super
                //when speed hits x
                break;
            case animalStates.complete:
                curentState = animalStates.complete;
                Debug.Log("State is Complete");
                //What happens when completed
                break;
            default:
                //Default behaviour
                break;
        }
    }
}
