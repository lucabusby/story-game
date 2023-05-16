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
    public Transform cam;
    private float sprint = 1.0f;

    [SerializeField] private float gravityMultiplier = 1.0f;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;


    private void Awake(){
        _characterController = GetComponent<CharacterController>();
    }

    private void Update(){
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
    }

    private void ApplyGravity(){

        if (IsGrounded() && _velocity <= 0){
            _velocity = -1.0f;
        } else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
        _moveDir.y = _velocity;
    }

    private void ApplyRotation(){

        if (_input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

        _moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
 
    }

    public void ApplyMovement(){
        _characterController.Move(_moveDir * (speed * sprint) * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context){
        _input = context.ReadValue<Vector2>();
        _moveDir = new Vector3(_input.x, 0.0f, _input.y);
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }

    public void Jump(InputAction.CallbackContext context){
        if (!context.started) return;
        if (!IsGrounded()) return;
        _velocity += jumpPower;
    }

    public void Sprint(InputAction.CallbackContext context){
        if (!IsGrounded()) return;
        var sprintInput = context.ReadValue<float>();
        if (sprintInput == 1){
            sprint = 1.6f;
        } else{sprint = 1.0f;}
    }

    private bool IsGrounded() => _characterController.isGrounded;
}