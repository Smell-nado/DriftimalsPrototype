using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollController : MonoBehaviour
{
    SpeedController speedControllerRef;
    Rigidbody rb;

    List<CharacterJointDisabler> _jointDisablers;
    bool clicked = false;
    private void Awake()
    {
        speedControllerRef = gameObject.GetComponentInParent<SpeedController>();
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

    void Update()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    if (!clicked)
        //    {
        //        ActivateRagdoll();
        //        LaunchAnimal();
        //        clicked = true;
        //    }

        //}
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            Debug.Log("Obstacle Hit");
            speedControllerRef.HitObject(1);
            LaunchAnimal();
            ActivateRagdoll();
        }
    }

    void LaunchAnimal()
    {
        
        Vector3 forceDir = new Vector3(0f, 1f, 0f);
        Vector3 forceAmount = forceDir * 5000f;
        rb.AddForce(forceAmount);
        Debug.Log("Launched at x:" + forceAmount.x + " y:" + forceAmount.y + " z:" + forceAmount.z);
    }

    public void ActivateRagdoll()
    {
        rigidbody.constraints = RigidbodyConstraints.None;

        for (int i = 0; i < _jointDisablers.Count; ++i)
        {
            _jointDisablers[i].collider.enabled = true;
            _jointDisablers[i].rigidbody.isKinematic = false;

            _jointDisablers[i].CreateJointAndDestoryThis();
        }
    }
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
