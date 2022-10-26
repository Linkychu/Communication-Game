using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour, AxisState.IInputAxisProvider
{
    
    
   
    [HideInInspector]
    public InputAction playerControllerInput;
    private float x;

    private bool canUseCamera;
    // Start is called before the first frame update
    private void Awake()
    {

        Cursor.lockState = CursorLockMode.Confined;
    }

  

    public float GetAxisValue(int axis)
    {
        if (canUseCamera)
        {
            switch (axis) 
                
            {
                case 0: return playerControllerInput.ReadValue<Vector2>().x;
                case 1: return playerControllerInput.ReadValue<Vector2>().y;
                case 2: return playerControllerInput.ReadValue<float>();
                
            }
            
        }
        
        return 0;
        
    }

    private void OnEnable()
    {
        //cameraControls.Mouse.MouseLook.performed += GetInput;
        
    }

    private void OnDisable()
    {
        
    }

    void Start()
    {
      
    }

    public void EnableCameraInput()
    {
        canUseCamera = true;
    }

    public void DisableCameraInput()
    {
        canUseCamera = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
