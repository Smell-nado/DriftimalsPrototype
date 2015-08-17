using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollController : MonoBehaviour
{
    //SpeedController speedControllerRef;
    PlayerController playerRef;
    Rigidbody rb;

    //For rotating
    public float rotSmooth = 2.0F;
    public float rotTiltAngle = 30.0F;

    float smallCollisionForce = 4000f;
    float finalCollisionForce = 4000f;

    List<CharacterJointDisabler> _jointDisablers;
    bool clicked = false;
    private void Awake()
    {
        //speedControllerRef = gameObject.GetComponentInParent<SpeedController>();
        playerRef = gameObject.GetComponentInParent<PlayerController>();
        CharacterJoint[] characterJoints = transform.GetComponentsInChildren<CharacterJoint>();
        rb = gameObject.GetComponent<Rigidbody>();

        _jointDisablers = new List<CharacterJointDisabler>();
        for (int i = 0; i < characterJoints.Length; ++i)
        {
            characterJoints[i].collider.enabled = false;
            characterJoints[i].rigidbody.isKinematic = true;

            CharacterJointDisabler disabler = characterJoints[i].gameObject.AddComponent<CharacterJointDisabler>();
            disabler.CopyValuesAndDestroyJoint(characterJoints[i]);
            _jointDisablers.Add(disabler);
        }
    }

    public void SmallCollisionLaunch()
    {
        //Can we get a random dir?
        Vector3 forceDir = new Vector3(0f, 1f, 1f);
        Vector3 forceAmount = forceDir * smallCollisionForce;
        rb.AddForce(forceAmount);
        //Debug.Log("Launched at x:" + forceAmount.x + " y:" + forceAmount.y + " z:" + forceAmount.z);
    }

    public void FinalLaunch()
    {
        //Can we get a random dir?
        Vector3 forceDir = new Vector3(0f, 1f, 1f);
        Vector3 forceAmount = forceDir * finalCollisionForce;
        rb.AddForce(forceAmount);
        //Debug.Log("Launched at x:" + forceAmount.x + " y:" + forceAmount.y + " z:" + forceAmount.z);
    }

    public void ActivateRagdoll()
    {
        rigidbody.constraints = RigidbodyConstraints.None;

        for (int i = 0; i < _jointDisablers.Count; ++i)
        {
            if (_jointDisablers[i] != null)
            {
                _jointDisablers[i].collider.enabled = true;
                _jointDisablers[i].rigidbody.isKinematic = false;

                _jointDisablers[i].CreateJointAndDestoryThis();
            }
            
        }
    }

    //Need a deActivate Ragdoll

}


public class CharacterJointDisabler : MonoBehaviour
{
    private Rigidbody ConnectedBody;
    private Vector3 Anchor;
    private Vector3 Axis;
    private bool AutoConfigureConnectedAnchor;
    private Vector3 ConnectedAnchor;
    private Vector3 SwingAxis;
    private SoftJointLimit LowTwistLimit;
    private SoftJointLimit HighTwistLimit;
    private SoftJointLimit Swing1Limit;
    private SoftJointLimit Swing2Limit;
    private float BreakForce;
    private float BreakTorque;
    private bool EnableCollision;

    public void CopyValuesAndDestroyJoint(CharacterJoint characterJoint)
    {
        ConnectedBody = characterJoint.connectedBody;
        Anchor = characterJoint.anchor;
        Axis = characterJoint.axis;
        AutoConfigureConnectedAnchor = characterJoint.autoConfigureConnectedAnchor;
        ConnectedAnchor = characterJoint.connectedAnchor;
        SwingAxis = characterJoint.swingAxis;
        LowTwistLimit = characterJoint.lowTwistLimit;
        HighTwistLimit = characterJoint.highTwistLimit;
        Swing1Limit = characterJoint.swing1Limit;
        Swing2Limit = characterJoint.swing2Limit;
        BreakForce = characterJoint.breakForce;
        BreakTorque = characterJoint.breakTorque;
        EnableCollision = characterJoint.enableCollision;

        Destroy(characterJoint);
    }

    public void CreateJointAndDestoryThis()
    {
        CharacterJoint characterJoint = gameObject.AddComponent<CharacterJoint>();

        characterJoint.connectedBody = ConnectedBody;
        characterJoint.anchor = Anchor;
        characterJoint.axis = Axis;
        characterJoint.autoConfigureConnectedAnchor = AutoConfigureConnectedAnchor;
        characterJoint.connectedAnchor = ConnectedAnchor;
        characterJoint.swingAxis = SwingAxis;
        characterJoint.lowTwistLimit = LowTwistLimit;
        characterJoint.highTwistLimit = HighTwistLimit;
        characterJoint.swing1Limit = Swing1Limit;
        characterJoint.swing2Limit = Swing2Limit;
        characterJoint.breakForce = BreakForce;
        characterJoint.breakTorque = BreakTorque;
        characterJoint.enableCollision = EnableCollision;

        Destroy(this);
    }


}
