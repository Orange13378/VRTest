using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    internal Rigidbody RBody;
    internal void Start()
    {
        this.RBody = this.gameObject.GetComponent<Rigidbody>();
        this.RBody.isKinematic = false;
        int childcount = this.transform.childCount;

        for (int i = 0; i < childcount; i++)
        {
            Transform t = this.transform.GetChild(i);
            t.gameObject.AddComponent<HingeJoint>();
            HingeJoint hinge = t.gameObject.GetComponent<HingeJoint>();
            hinge.connectedBody = i == 0 ? this.RBody : this.transform.GetChild(i - 1).GetComponent<Rigidbody>();
            hinge.useSpring = true;
            hinge.enableCollision = true;
        }
    }

}
