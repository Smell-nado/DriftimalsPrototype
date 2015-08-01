using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class SpeedController : MonoBehaviour {
    
    float _currentSpeed = 0f;
    float _turnSpeed = 8f;
    float _boostSpeed = 2f;
    float _maxSpeed = 30f;
    float _accelAmount = 0.1f;
    float _objectDecelAmount = 0.33f;
    float rotateSpeed = 1.0F;
    //public float gravity = 1.0F;
    private Vector3 moveDirection = Vector3.zero;

    //All the same size for debug
    float smallObstacleReduction = 30f;
    float medObstacleReduction = 30f;
    float largeObstacleReduction = 30f;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Current Speed: " + _currentSpeed);
            _currentSpeed += _boostSpeed;
        }

        //TODO make turn a rotation rather than a slide

        CharacterController controller = GetComponent<CharacterController>();
        Accelerate();
        moveDirection = new Vector3(-_currentSpeed, 0, Input.GetAxis("Horizontal") * _turnSpeed);
        moveDirection = transform.TransformDirection(moveDirection);
        controller.Move(moveDirection * Time.deltaTime);

    }

    public void Accelerate() {
        if (_currentSpeed <= _maxSpeed)
            _currentSpeed += _accelAmount;
    }

    public void HitObject(int obstacleSize) {
        switch(obstacleSize) {
            case 0:
                _currentSpeed -= smallObstacleReduction;
                if (_currentSpeed <= 0)
                    _currentSpeed = 0;
                break;
            case 1:
                _currentSpeed -= medObstacleReduction;
                if (_currentSpeed <= 0)
                    _currentSpeed = 0;
                break;
            case 2:
                _currentSpeed -= largeObstacleReduction;
                if (_currentSpeed <= 0)
                    _currentSpeed = 0;
                break;
            default:
                Debug.LogWarning("There is no obstacle that size!");
                break;

        }
    }
}
