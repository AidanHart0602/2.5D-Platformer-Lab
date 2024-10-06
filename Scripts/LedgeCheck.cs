using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    [SerializeField]
    private Vector3 _handPos, _bodyPos;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LedgeChecker")
        {
            Player player = other.transform.parent.GetComponent<Player>();
            player.LedgeGrabbed(_handPos, this);
        }
    }

    public Vector3 StandUpProgress()
    {
        return _bodyPos;
    }
}
