using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _checkGround;
    [SerializeField] private LayerMask _groundMask;
    [Header("Settings")]
    [SerializeField] private float _checkRadiusSprehe = 0.2f;
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _jumpHeight = 1f;
    [SerializeField] private float _speedRun = 7;
    [SerializeField] private float _gravity = -14f;
    // Start is called before the first frame update
    [Range(0,100)]
    [SerializeField] private float sensivity = 50f;

    float rotationX;
    bool isGrounded;
    Vector3 velocity;
    Vector3 move;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
        Velocity();
    }
    private void Rotate()

    {
        float mouseX = Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        float mouseY= Input.GetAxis("Mouse Y")* sensivity * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -90 , 90f);

        rotationX -= mouseY;
        _cameraTransform.localRotation = Quaternion.Euler(rotationX, 0 , 0);
        transform.Rotate(Vector3.up * mouseX);
    }
    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        move = transform.forward * moveY + transform.right * moveX;
        if (Input.GetKey(KeyCode.LeftShift) && (moveX != 0 || moveY != 0))
        {
            characterController.Move(move * _speedRun * Time.deltaTime);
        }
        else
        {
            characterController.Move(move * _speed * Time.deltaTime);
        }
    }  
     private void Velocity()
    {
        isGrounded = Physics.CheckSphere(_checkGround.position, _checkRadiusSprehe, _groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += Time.deltaTime * _gravity;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}

