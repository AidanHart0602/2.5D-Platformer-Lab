using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]
    private Transform _pointA, _pointB;
    private Transform _currentPoint;
    // Start is called before the first frame update
    void Start()
    {
        _currentPoint = _pointA;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentPoint.position, 3 * Time.deltaTime);

        if(transform.position == _pointA.position)
        {
            _currentPoint = _pointB;
        }

        if (transform.position == _pointB.position)
        {
            _currentPoint = _pointA;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
