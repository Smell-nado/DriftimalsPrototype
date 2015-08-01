using UnityEngine;
using System.Collections;

public class AnimalStateController : MonoBehaviour {
    SpeedController speedControllerRef;

    public enum animalStates
    {
        stopped = 0,
        slow1 = 1,
        slow2 = 2,
        slow3 = 3,
        travelling = 4,
        super = 5,
        complete = 6
    }
    public animalStates curentState;

	// Use this for initialization
	void Start () {
        speedControllerRef = gameObject.GetComponent<SpeedController>();
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
                //What happens when stopped
                //Wait a second and change to slow 1
                break;
            case animalStates.slow1:
                //What happens when slow1
                //accelerate each frame
                //when speed hits xchange to slow 2
                break;
            case animalStates.slow2:
                //What happens when slow2
                //accelerate each frame
                //change to slow 3
                break;
            case animalStates.slow3:
                //What happens when slow3
                //accelerate each frame
                //change to travelling
                break;
            case animalStates.travelling:
                //What happens when travelling
                //accelerate each frame
                //when speed hits x change to super
                break;
            case animalStates.super:
                //What happens when super
                //when speed hits x
                break;
            case animalStates.complete:
                //What happens when completed
                break;
            default:
                //Default behaviour
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        //Acclerate an amount
        //if 
	
	}
}
