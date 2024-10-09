using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private Transform _pointA, _pointB;
    [SerializeField]
    private float _speed;
    private bool _direction = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_direction == true)
        {
            GoingDown();
        }

        if(_direction == false)
        {
            GoingUp();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Entered Elevator");
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
    private void GoingUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, _pointA.position, _speed * Time.deltaTime);
    }

    private void GoingDown() 
    {
        transform.position = Vector3.MoveTowards(transform.position, _pointB.position, _speed *Time.deltaTime);
    }


    IEnumerator ChangeDirection()
    {
        while(1 == 1)
        {
            _direction = true;
            yield return new WaitForSeconds(5.0f);
            _direction = false;
            yield return new WaitForSeconds(5.0f);
        }
    }
}
