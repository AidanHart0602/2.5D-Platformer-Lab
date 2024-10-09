using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _model;
    private CharacterController _controller;
    [SerializeField] private float _speed = 5, _gravity = 0.3f, _jump = 15;
    private Vector3 direction, velocity;
    [SerializeField]
    private Animator _anim;
    private bool _ledgeGrabbed;
    private LedgeCheck _ledge;
    private bool _rolling = false;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovementCalc();
        if (_ledgeGrabbed == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _anim.SetTrigger("Climb");
                StartCoroutine(StandUpStart());
            }
        }
    }

    private void MovementCalc()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (_controller.isGrounded == true)
        {
            _anim.SetBool("Jump", false);
            direction = new Vector3(0, 0, horizontalInput);
            velocity = direction * _speed;

            _anim.SetFloat("Speed", Mathf.Abs(horizontalInput));

            if (horizontalInput != 0)
            {
                Vector3 DirectionFace = transform.localEulerAngles;
                DirectionFace.y = direction.z > 0 ? 0 : 180;
                _model.transform.localEulerAngles = DirectionFace;
            }

            if (_rolling == false)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    velocity.x = 0;
                    _anim.SetTrigger("Roll");
                    _rolling = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _anim.SetBool("Jump", true);
                Debug.Log("Jumping");
                velocity.y += _jump;
            }
        }
        velocity.y -= _gravity;
        _controller.Move(velocity * Time.deltaTime);
    }

    public void LedgeGrabbed(Vector3 handPos, LedgeCheck CurrentLedge)
    {
        _ledge = CurrentLedge;
        _controller.enabled = false;
        _ledgeGrabbed = true;
        _anim.SetBool("LedgeGrab", true);
        _anim.SetFloat("Speed", 0);
        _anim.SetBool("Jump", false);
        transform.position = handPos;
    }



    IEnumerator StandUpStart()
    {
        _ledgeGrabbed = false;
        yield return new WaitForSeconds(5.1f);
        StandUpFinish();
    }
    public void StandUpFinish()
    {
        transform.position = _ledge.StandUpProgress();
        _anim.SetBool("LedgeGrab", false);
        _controller.enabled = true;
    }

    public void RollingComplete()
    {
        if (transform.rotation.y == 0)
        {
            _controller.Move(new Vector3(0, 0, 12.5f));
            _rolling = false;
        }
        else
        {
            _controller.Move(new Vector3(0, 0, -12.5f));
            _rolling = false;
        }
    }
}
