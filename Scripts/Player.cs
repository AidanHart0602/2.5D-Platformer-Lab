using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _model;
    private CharacterController _controller;
    [SerializeField] private float _speed = 5, _gravity = 0.3f, _jump = 15;
    private Vector3 _direction, _velocity;
    [SerializeField]
    private Animator _anim;
    private bool _ledgeGrabbing = false;
    private bool _hanging = false;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (_controller.isGrounded == true)
        {
            _direction = new Vector3(0, 0, horizontalInput);
            _velocity = _direction * _speed;

            _anim.SetFloat("Speed", Mathf.Abs(horizontalInput));

            if (horizontalInput != 0)
            {
                Vector3 DirectionFace = transform.localEulerAngles;
                DirectionFace.y = _direction.z > 0 ? 0 : 180;
                _model.transform.localEulerAngles = DirectionFace;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_hanging == false && _ledgeGrabbing == false)
                {
                    _anim.SetTrigger("Jump");
                    Debug.Log("Jumping");
                    _velocity.y += _jump;
                }

                if(_hanging == true)
                {
             //       StartCoroutine(ClimbUp());
           //         _hanging = false;
                }
            }
          
        }
        _velocity.y -= _gravity;
        _controller.Move(_velocity * Time.deltaTime);
    }
   /* IEnumerator ClimbUp()
    {
        _anim.SetTrigger("Climb");
        //Move position of player on top of the platform
        yield return new WaitForSeconds(3);
        _ledgeGrabbing = false;
        _controller.enabled = true;
    }*/

    public void LedgeGrabbed(Vector3 handPos)
    {
        _controller.enabled = false;
        _anim.SetTrigger("LedgeGrab");
        _anim.SetFloat("Speed", 0);
        _ledgeGrabbing = true;
        _hanging = true;
        transform.position = handPos;
    }
}
