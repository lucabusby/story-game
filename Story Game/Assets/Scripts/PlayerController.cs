using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private Vector3 _direction;
    private Vector3 _moveDir;
    private CharacterController _characterController;
    private float _currentVelocity;
    private float _gravity = -9.81f;
    private float _velocity;
    private float _speedRamp = 0.0f;
    public Transform cam;
    private float sprint = 1.0f;
    private Vector3 hitNormal;
    private bool isSliding;
    private float sprintInput;
    [SerializeField] private float slideFriction = 0.3f;

    [SerializeField] private float gravityMultiplier = 1.0f;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] public float speed;
    [SerializeField] private float jumpPower;


    private void Awake(){
        Cursor.lockState = CursorLockMode.Locked;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update(){
        SpeedRamp();
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
    }

    private void SpeedRamp(){
        //eases out player movement
        if (_input.sqrMagnitude >= 1 && _speedRamp <= 1.0f) {
            _speedRamp = (_speedRamp * 1.005f) + 0.001f;
            if (_speedRamp >= 0.99f){
                _speedRamp = 1.0f;
            }
        } else if (_input.sqrMagnitude == 0 && _speedRamp >= 0.01f) {
            _speedRamp = _speedRamp * 0.995f;
            if (_speedRamp <= 0.01f){
                _speedRamp = 0.0f;
            }
        }

        //eases out sprinting
        if (sprintInput == 1 && sprint <= 1.5f){
            sprint = sprint * 1.0003f;
            if (sprint >= 1.49f){
                sprint = 1.5f;
            }
        } else if(sprintInput == 0 && sprint >= 1.01f) {
            sprint = sprint * 0.999f;
            if (sprint <= 1.01f){
                sprint = 1f;
            }
        }

        //checks if player is sliding
        isSliding = (Vector3.Angle (Vector3.up, hitNormal) <= _characterController.slopeLimit);
    }

    private void ApplyRotation(){
        // sets rotation of direction to where the player is looking
        // check for _speedRamp == 0 but it returns to origin
        if (_input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

        _moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
 
    }

    private void ApplyGravity(){
        //checks if player is grounded otherwise pulls player down
        if (IsGrounded() && _velocity <= 0){
            _velocity = -1.0f;
        } else {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
        _direction.y = _velocity;
        _moveDir.y = _velocity;
    }

    public void ApplyMovement(){
        //movement is applied & checks if player is sliding
        if (!isSliding) {
            _moveDir.x += (1f - hitNormal.y) * hitNormal.x * (1f - slideFriction);
            _moveDir.z += (1f - hitNormal.y) * hitNormal.z * (1f - slideFriction);
        }

        _moveDir.x = _moveDir.x * _speedRamp;
        _moveDir.z = _moveDir.z * _speedRamp;

        _characterController.Move(_moveDir * (speed * sprint) * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context){
        //gets input values for W A S D
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }

    public void Jump(InputAction.CallbackContext context){
        //adds jumping when on ground
        if (!context.started) return;
        if (!IsGrounded()) return;
        _velocity += jumpPower;
    }

    public void Sprint(InputAction.CallbackContext context){
        //adds sprinting
        if (!IsGrounded()) return;
        sprintInput = context.ReadValue<float>();
    }

    public void Crouch(InputAction.CallbackContext context){
        var crouchInput = context.ReadValue<float>();
    }

    void OnControllerColliderHit (ControllerColliderHit hit) {
        hitNormal = hit.normal;
    }

    private bool IsGrounded() => _characterController.isGrounded;
}