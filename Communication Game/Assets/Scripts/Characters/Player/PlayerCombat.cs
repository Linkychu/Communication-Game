using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using UnityEngine.InputSystem;


public class PlayerCombat : MonoBehaviour
{
    private PlayerClass myClass;

    private Player1 player1Input;
    private Player2 player2Input;

    public Moves LightAttack;

    public Moves HeavyAttack;

    public Vector3 offsetL;
    public Vector3 offsetH;
    
    private AnimationController _animator;

    private AttackEffect CurrentMove;
    // Start is called before the first frame update

    private void OnEnable()
    {
        player1Input.Combat.Enable();
    }

    private void OnDisable()
    {
        player1Input.Combat.Disable();
    }

    private void Awake()
    {
        player1Input = new Player1();
        player1Input.Combat.FirstAttack.performed += FirstAttackInput;
        player1Input.Combat.HeavyAttack.performed += HeavyAttackInput;
    }

    public void FirstAttackInput(InputAction.CallbackContext context)
    {
        if (CurrentMove != null)
        {
            Destroy(CurrentMove.gameObject);
        }
        
        LightAttack.Spawn((transform.position + offsetL), transform, transform.rotation.eulerAngles);
        CurrentMove = LightAttack.effect;
        LightAttack.setUser(myClass);
        _animator.AttackAnimation(1);
    }
    private void HeavyAttackInput(InputAction.CallbackContext context)
    {
        if (CurrentMove != null)
        {
            Destroy(CurrentMove.gameObject);
        }
        
        HeavyAttack.Spawn(transform.position + offsetH + transform.forward, transform, transform.rotation.eulerAngles);
        CurrentMove = HeavyAttack.effect;
        HeavyAttack.setUser(myClass);
    }

    

    void Start()
    {
       
        myClass = GetComponent<PlayerClass>();
        _animator = GetComponent<AnimationController>();
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
