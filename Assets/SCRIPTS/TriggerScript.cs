using UnityEngine;
using System.Collections;

public class TriggerScript : MonoBehaviour {

    PlayerController playerControllerRef;

    public enum TriggerType
    {
        startRace,
        startDrift,
        endDrfit,
        launch
    }
    public TriggerType _type;

    public enum TurnDirection
    {
        right,
        left
    }
    public TurnDirection _turnDirection;
    public GameObject _endTrigger;
    [SerializeField] public GameObject _nextTrigger;
    public int _cornerMagnitude;
    public int _launchAngle;
    public float _launchForceMagnitude;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // change the move direction
            playerControllerRef = other.gameObject.GetComponentInParent<PlayerController>();
            if (_type == TriggerType.startDrift)
            {
                bool tDir = false;
                if (_turnDirection == TurnDirection.left) { tDir = false; }
                else if (_turnDirection == TurnDirection.right) { tDir = true; }
                else Debug.Log("No Turn Direction Set.");
                playerControllerRef.SetCorner(_cornerMagnitude, tDir);
                //change the end trigger's turn magnitude to cancel the right amount of turn when you hit it
                if (_endTrigger != null)
                    _endTrigger.GetComponent<TriggerScript>()._cornerMagnitude = _cornerMagnitude;
            }
            else if (_type == TriggerType.endDrfit)
            {
                playerControllerRef.EndCorner(_cornerMagnitude, _nextTrigger.transform.position);
            }
            else if (_type == TriggerType.launch)
            {
                playerControllerRef.SetLaunch(_launchForceMagnitude, _launchAngle);
            }
        }

        //turn the character to it's side a bit
        //start the drifting animation
    }

    void OnTriggerStay(Collider other)
    {
        if (_type == TriggerType.endDrfit || _type == TriggerType.startRace)
        {
            if (other.tag == "Player")
            {
                if (_nextTrigger != null)
                {
                    Vector3 pos = new Vector3(_nextTrigger.transform.position.x, other.gameObject.transform.position.y, _nextTrigger.transform.position.z);
                    Vector3 dir = (pos - other.gameObject.transform.position).normalized;
                    Debug.DrawRay(other.gameObject.transform.position, dir);
                    Quaternion lookRotation = Quaternion.LookRotation(dir);
                    other.gameObject.transform.rotation = Quaternion.Slerp( other.gameObject.transform.rotation, lookRotation, 10f * Time.deltaTime);
                }
                else
                {
                    Debug.LogWarning("You forgot to set the next Trigger!");
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        switch (_type)
        {
            case TriggerType.startRace:
                Gizmos.color = Color.blue;
                break;
            case TriggerType.startDrift:
                Gizmos.color = Color.green;
                break;
            case TriggerType.endDrfit:
                Gizmos.color = Color.red;
                break;
            case TriggerType.launch:
                Gizmos.color = Color.yellow;
                break;
            default:
                Debug.LogWarning("Trigger Box has not been set to a type.");
                break;
        }
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        //_wireColored = false;
    }

}
