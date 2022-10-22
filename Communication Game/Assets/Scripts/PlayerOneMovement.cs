using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerOneMovement: MonoBehaviour
{
    private Player1 player;
    private Rigidbody rb;
    
    [Range(0, 100)]
    public float speed = 6;

  

    private void Awake()
    {
        player = new Player1();
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
            player.PlayerControls.Enable();
        }

        else
        {
            player.PlayerControls.Disable();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        float x = player.PlayerControls.Movement.ReadValue<Vector2>().x;
        float z = player.PlayerControls.Movement.ReadValue<Vector2>().y;


        Vector3 moveDir = (x * transform.right) + (z  * transform.forward);

        rb.AddForce(moveDir * speed, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
