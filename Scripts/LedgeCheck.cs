using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    [SerializeField]
    private Vector3 _handPos = new Vector3(.57f, 69.87f, 123.4f);
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LedgeChecker")
        {
            Player player = other.transform.parent.GetComponent<Player>();
            player.LedgeGrabbed(_handPos);
        }
    }
}
