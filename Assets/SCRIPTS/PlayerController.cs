using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    AnimalStateController animalStateControllerRef;
    RagdollController ragdollRef;
    SpeedometerScript speedo;
    public Transform animal;

    float speed = 0f;
    float maxSpeed = 50f;
    int _gear;

    float _boostAmount = 0.01f;

    //How much the player acclerates at each gear
    float[] accelAmounts = new float[] {
        1f,//0.1f
        0.5f,//0.05f
        0.1f,//0.03f
        0.02f,
        0.015f,
        0.005f };
    //The speeds where the gears change and the acceleration slows.
    float[] speedThresholds = new float[] {
        0.5f,
        5f,
        10f,
        20f,
        40f };

    //Character controller values
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    float rotSpeed = 10f; // rotate speed in degrees/second

    //This will come from each trigger (for cornering)
    bool cornering = false;
    int _turnDirValue; //-1 is left and  +1 is right
    Vector3 turnAmount;
    float _cornerMagnitude = 0f;
    float _cornerMagFactor = 0.001f;
    float driftTurnSpeed = 0.1f;//how quickly it goes to it's drift rotation (moving object not animation body)
    float _launchMagnitude = 0f;
    int _launchAngle = 0; //An int from 0 - 90 degrees

    //UI Stuff
    float uiUpdateRate = 0.25f;//in seconds

    //TODO: Create startPLayer function when we want to have some sort of menu.

    void Awake()
    {
        controller = GetComponent<CharacterController>(); //NOTE: Does this screw things up?
        animalStateControllerRef = gameObject.GetComponent<AnimalStateController>();
        ragdollRef = gameObject.GetComponentInChildren<RagdollController>();
    }

    void Start()
    {
        StartCoroutine("UpdateUI");
    }

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            //moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 1);//1 //the input will turn the player but the speed of the turn will increase with speed
            if (cornering)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 1);
                turnAmount = transform.rotation.eulerAngles;
                //Debug.Log("turnAmount.y:" + turnAmount.y + " = _cornerMagnitude:" + _cornerMagnitude + " * _turnDirValue:" + _turnDirValue + " * speed:" + speed + " * _cornerMagFactor:" + _cornerMagFactor);
                turnAmount.y = _cornerMagnitude * _turnDirValue * speed * _cornerMagFactor;
                transform.Rotate(turnAmount);
                
            }
            else
            {
                moveDirection = new Vector3(0, 0, 1);
            }
            moveDirection = transform.TransformDirection(moveDirection);
            GearChanger();
            Accelerate(_gear);
        }
        moveDirection *= speed;
        //transform.Rotate(0, Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime, 0);
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            SpeedBoost();
        }
    }

    void Accelerate(int gear)
    {
        switch (gear)
        {

            case 0:
                break;

            case 1:
                if (speed <= maxSpeed)
                    speed += accelAmounts[0];
                break;

            case 2:
                if (speed <= maxSpeed)
                    speed += accelAmounts[1];
                break;

            case 3:
                if (speed <= maxSpeed)
                    speed += accelAmounts[2];
                break;

            case 4:
                if (speed <= maxSpeed)
                    speed += accelAmounts[3];
                break;

            case 5:
                if (speed <= maxSpeed)
                    speed += accelAmounts[4];
                break;

            case 6:
                if (speed <= maxSpeed)
                    speed += accelAmounts[5];
                break;

            default:
                Debug.LogWarning("That gear doesn't exist!");
                break;

        }

       

    }

    void GearChanger()//This will also handle state
    {
        //This first part will change the state depending on the current Speed
        if (speed >= 0 && speed < speedThresholds[0])
        {
            if (AnimalStateController.curentState != AnimalStateController.animalStates.stopped)
            {
                animalStateControllerRef.ChangeState(0);//0 is stopped
                _gear = 1;
            }
        }
        else if (speed > speedThresholds[0] && speed < speedThresholds[1])
        {
            if (AnimalStateController.curentState != AnimalStateController.animalStates.slow1)
            {
                animalStateControllerRef.ChangeState(1);//1 is slow1
                _gear = 2;
            }
        }
        else if (speed > speedThresholds[1] && speed < speedThresholds[2])
        {
            if (AnimalStateController.curentState != AnimalStateController.animalStates.slow2)
            {
                animalStateControllerRef.ChangeState(2);
                _gear = 3;
            }
        }
        else if (speed > speedThresholds[2] && speed < speedThresholds[3])
        {
            if (AnimalStateController.curentState != AnimalStateController.animalStates.slow3)
            {
                animalStateControllerRef.ChangeState(3);
                _gear = 4;
            }
        }
        else if (speed > speedThresholds[3] && speed < speedThresholds[4])
        {
            if (AnimalStateController.curentState != AnimalStateController.animalStates.travelling)
            {
                animalStateControllerRef.ChangeState(4);
                _gear = 5;
            }
        }
        else if (speed > speedThresholds[4] && speed <= maxSpeed)
        {
            if (AnimalStateController.curentState != AnimalStateController.animalStates.super)
            {
                animalStateControllerRef.ChangeState(5);
                _gear = 6;
            }
        }
        else
        {
            Debug.LogWarning("The speed somehow is outside range???");
        }
    }

    public void SlowPlayer(float slowAmount)
    {
        if (controller.isGrounded)
        {
            speed -= slowAmount;
            if (speed <= 0)
                speed = 0;
        }
    }

    public void SlipPlayer(float slipTime)
    {

    }

    public void SpeedBoost ()
    {
        Debug.Log("Speed Boost: " + _boostAmount);
        speed += _boostAmount;
    }

    public void CollisionLaunch(Collision collision)
    {
        animalStateControllerRef.ChangeState(7);
        speed = 0;
        _gear = 0;
        ragdollRef.ActivateRagdoll();
        ragdollRef.SmallCollisionLaunch();

        //TODO: Put more control on the colllision
    }

    public void SetCorner(int cornerMag, bool turnDirection)
    {
        cornering = true;
        //Debug.Log("Corner hit -- cornerMag = " + cornerMag);

        if (turnDirection == false)
            _turnDirValue = -1;
        else if (turnDirection == true)
            _turnDirValue = +1;

        _cornerMagnitude = cornerMag;
    }

    public void EndCorner(int cornerMagnitude, Vector3 nextTrigger)
    {
        cornerMagnitude -= cornerMagnitude;
        _turnDirValue = -1;//Default to left
        cornering = false;
    }

    public void DriftAnimal()
    {
        //twist magnatude should be relative to the speed
        //
        

        //float tiltAroundZ = Input.GetAxis("Horizontal") * rotTiltAngle;
        //Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);
        //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotSmooth);
    }

    public void SetLaunch(float launchMag, int angle)
    {
        _launchMagnitude = launchMag;
        _launchAngle = angle;
    }

    public void FinalLaunch()
    {
        animalStateControllerRef.ChangeState(7);
        //speed = 0;
        //_gear = 0;
        ragdollRef.ActivateRagdoll();
        ragdollRef.FinalLaunch();
        animalStateControllerRef.ChangeState(7);
    }

    //UI Stuff
    IEnumerator UpdateUI()
    {
        SpeedometerScript.tachO.UpdateText(speed);
        yield return new WaitForSeconds(uiUpdateRate);
        StartCoroutine("UpdateUI"); 
    }

}