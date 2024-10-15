using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    private CharacterController _controller;
    [SerializeField] private float _speed = 5, _gravity = 0.3f, _jump = 15, _ladderSpeed = 3;
    private Vector3 _direction, _velocity;
    [SerializeField]
    private float _rollDistance;
    [SerializeField] private Animator _anim;
    private bool _ledgeGrabbed;
    private LedgeCheck _ledge;
    private bool _rolling = false;
    private bool _ladderClimb;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (_rolling == false)
        {
            MovementCalc();
        }

        if(_ladderClimb == true)
        {
            LadderMovement();
        }

        if (_rolling == false && Input.GetKeyDown(KeyCode.LeftShift))
        {
            _rolling = true;
            StartCoroutine(Rolling());

        }

        if (_ledgeGrabbed == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _anim.SetTrigger("Climb");
                StartCoroutine(StandUpStart());
            }
        }
    }
    private void LadderMovement()
    {
        float VerticalInput = Input.GetAxis("Vertical");
        float HorizontalInput = Input.GetAxis("Horizontal");
        if(VerticalInput != 0)
        { 
            if (transform.position.y < 10)
            {
                _direction = new Vector3(0, VerticalInput, 0);
                _velocity = _direction * _ladderSpeed;
                _controller.Move(_velocity * Time.deltaTime);
            }
        }
        if(HorizontalInput != 0)
        {
            _direction = new Vector3(0, HorizontalInput, 0);
            _velocity = _direction * _speed;
        }
    }
    private void MovementCalc()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (_controller.isGrounded == true)
        {
            _anim.SetBool("Jump", false);
            _direction = new Vector3(0, 0, horizontalInput);
            _velocity = _direction * _speed;

            _anim.SetFloat("Speed", Mathf.Abs(horizontalInput));

            //Turning if statement
            if (horizontalInput != 0)
            {
                Vector3 DirectionFace = transform.localEulerAngles;
                DirectionFace.y = _direction.z > 0 ? 0 : 180;
                _model.transform.localEulerAngles = DirectionFace;
                
                if(DirectionFace.y == 180)
                {
                    _rollDistance = -9;
                }

                else if(DirectionFace.y == 0)
                {
                    _rollDistance = 9;
                }
            }

            //Jumping if statement
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _anim.SetBool("Jump", true);
                Debug.Log("Jumping");
                _velocity.y += _jump;
            }
        }
        if(_ladderClimb == false)
        {
            _velocity.y -= _gravity * Time.deltaTime;
        }
        
        _controller.Move(_velocity * Time.deltaTime);   
    }
    IEnumerator Rolling()
    {
        _anim.SetTrigger("Roll");
        yield return new WaitForSeconds(2.34f);
        _controller.Move(new Vector3(0, 0, _rollDistance));
        _rolling = false;
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
    private void ClimbLadder()
    {
        _ladderClimb = true;
        _anim.SetFloat("Speed", 0);
        _anim.SetBool("LadderClimb", true);
    }

    private void ExitLadder()
    {
        _ladderClimb = false;
        _anim.SetBool("LadderClimb", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ladder")
        {
            _ladderClimb = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        if (other.tag == "Ladder")
        {
            if(VerticalInput != 0)
            {
                ClimbLadder();
            }
            if(HorizontalInput != 0)
            {
                ExitLadder();
            }
        }
    }
}
