using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerOneMovement: MonoBehaviour
{
    private Player1 player;
    private Rigidbody rb;
    
    [Range(0, 100)]
    public float speed = 6;
    
    [Range(0, 100)]
    public float sprintSpeed = 12;
    
    public Camera Camera;
    

    public bool isSprinting;

    [HideInInspector]
    public Vector3 moveVelocityRef;

    Vector3 moveDir = Vector3.zero;

    public LayerMask GroundMask;

    public bool canUseCamera;

    public bool canMove = true;

    private CameraController cameraController;
    
    private void Awake()
    {
        player = new Player1();

        cameraController = Camera.GetComponent<CameraController>();
        cameraController.playerControllerInput = player.Camera.Look;
        
        player.Locomotion.Sprint.performed += context => isSprinting = isGrounded();
        player.Locomotion.Sprint.canceled += context => isSprinting = false;
       
        
    }

    
    
    private void OnEnable()
    {
       ControlStatus(true);
    }
    
    private void OnDisable()
    {
        ControlStatus(false);
    }

    private void ControlStatus(bool enable)
    {
        if (enable)
        {
            player.Locomotion.Enable();
            player.Camera.Look.Enable();
        }

        else
        {
            player.Locomotion.Disable();    
            player.Camera.Look.Disable();
        }
    }

    public bool isGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f, GroundMask))
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraController.EnableCameraInput();
       
    }


    public void EnableCameraInput()
    {
        cameraController.EnableCameraInput();
    }
    public void DisableCameraInput()
    {
        cameraController.DisableCameraInput();
    }
    private void FixedUpdate()
    {
        if(!canMove && isGrounded())
            return;
        Movement();


    }

    void Movement()
    {
        float MovementSpeed = isSprinting ? sprintSpeed : speed;
        
        
        moveDir += GetCameraRight(Camera) * (player.Locomotion.Movement.ReadValue<Vector2>().x * MovementSpeed);
        moveDir += GetCameraForward(Camera) * (player.Locomotion.Movement.ReadValue<Vector2>().y * MovementSpeed);

        
        
        moveVelocityRef = moveDir;
        rb.AddForce(moveDir, ForceMode.Acceleration);
        moveDir = Vector3.zero;
       // moveVelocityRef = moveDir;
        LookAt();

    }
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (player.Locomotion.Movement.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        else
        {
            rb.angularVelocity = Vector3.zero;;
        }
    }
    private Vector3 GetCameraForward(Camera playerCam)
    {
        Vector3 forward = playerCam.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCam)
    {
        Vector3 right = playerCam.transform.right;
        right.y = 0;
        return right.normalized;
    }
    private void Update()
    {
        

       
    }

    

}


    // Update is called once per frame
    

