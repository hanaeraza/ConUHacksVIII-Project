using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GripSwitcher : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint rightHand;
    [SerializeField] TwoBoneIKConstraint leftHand;

    [SerializeField] RigBuilder rigBuilder;

    public void UpdateGrip(GameObject rightGrip, GameObject leftGrip) {
        if (rightGrip != null) {
            rightHand.data.target = rightGrip.transform;
        }
        else {
            rightHand.data.target = null;
        }

        if (leftGrip != null) {
            leftHand.data.target = leftGrip.transform;
        }
        else {
            leftHand.data.target = null;
        }

        rigBuilder.Build();
    }
}
