using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    private CharacterController _controller;
    [SerializeField] private float _speed = 5, _gravity = 0.3f, _jump = 15, _ladderSpeed = 1.5f;
    private Vector3 _direction, _velocity;
    [SerializeField]
    private float _rollDistance;
    [SerializeField] private Animator _anim;
    private bool _ledgeGrabbed;
    private LedgeCheck _ledge;
    private bool _rolling = false;
    [SerializeField]
    private bool _ladderActive = false, _startLadderClimb = false;
    private Vector3 _directionFace;
    [SerializeField]
    private Vector3 _ladderPos, _finalLadderPos;
    void Start()
    {
        _directionFace = transform.localEulerAngles;
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (_rolling == false)
        {
            MovementCalc();
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
    private void MovementCalc()
    {
        if (_ladderActive == true)
        { 
            if (Input.GetKeyDown(KeyCode.W)) 
            {
                _controller.enabled = false;
                _startLadderClimb = true;
                transform.position = _ladderPos;
                _directionFace.y = 0;
                _model.transform.eulerAngles = _directionFace;
                _ladderActive = false;
            }
        }

        if (_startLadderClimb == true)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            float verticalInput = Input.GetAxis("Vertical");

            if (Input.GetKey("w") && transform.position.y < 7.5f)
            {
                _anim.SetBool("LadderClimb", true);
                transform.position += Vector3.up * _ladderSpeed * Time.deltaTime;
            }

            else if (Input.GetKey("s") && transform.position.y > 0.6f)
            {
                transform.position += Vector3.down * _ladderSpeed * Time.deltaTime;
            }

            else if (transform.position.y > 7.5f)
            {
                _anim.SetTrigger("ClimbUp");
           
                _startLadderClimb = false;
                StartCoroutine(LadderClimbUp());
            }

            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("Exiting");
                _anim.SetBool("LadderClimb", false);
                _startLadderClimb = false;
                _controller.enabled = true;
            }
        }


        if (_startLadderClimb == false)
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
                    _directionFace.y = _direction.z > 0 ? 0 : 180;
                    _model.transform.localEulerAngles = _directionFace;

                    if (_directionFace.y == 180)
                    {
                        _rollDistance = -9;
                    }

                    else if (_directionFace.y == 0)
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

            _velocity.y -= _gravity * Time.deltaTime;

            _controller.Move(_velocity * Time.deltaTime);
        }

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
    IEnumerator LadderClimbUp()
    {
        yield return new WaitForSeconds(4f);
        _anim.SetBool("LadderClimb", false);
        transform.position = _finalLadderPos;
        _controller.enabled = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ladder")
        {
            Debug.Log("Detected Ladder");
            if(_startLadderClimb == false)
            {
                _ladderActive = true;
            }       
        }
    }
}
