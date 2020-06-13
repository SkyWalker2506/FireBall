using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public HingeJoint hJoint;
    public LineRenderer lRenderer;

    public void FindRelativeJointPos(Vector3 blockPosition)
    {
        blockPosition = new Vector3(blockPosition.x, blockPosition.y - 5, blockPosition.z);
        hJoint.anchor = (blockPosition - transform.position);
        lRenderer.SetPosition(1, hJoint.anchor);
        lRenderer.enabled = true;
    }
}
