using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private float _speed = 5, _gravity = .3f, _jump = 15;
    private Vector3 _direction, _velocity;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if(_controller == null)
        {
            Debug.Log("Controller is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (_controller.isGrounded == true)
        {
            _direction = new Vector3(0, 0, horizontalInput);
            _velocity = _direction * _speed;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jumping");
                _velocity.y += _jump;
            }
     
        }
        _velocity.y -= _gravity;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
