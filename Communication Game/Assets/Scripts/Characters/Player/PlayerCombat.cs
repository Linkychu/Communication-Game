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

    public PlayerState user;
    // Start is called before the first frame update

    private void OnEnable()
    {
        switch (user)
        {
            case PlayerState.Player1:
                player1Input.Combat.Enable();
                break;
            case PlayerState.Player2:
                player2Input.Combat.Enable();
                break;
        }
      
    }

    private void OnDisable()
    {
        switch (user)
        {
            case PlayerState.Player1:
                player1Input.Combat.Disable();
                break;
            case PlayerState.Player2:
                player2Input.Combat.Disable();
                break;
        }
    }

    private void Awake()
    {
        switch (user)
        {
            case PlayerState.Player1:
                player1Input = new Player1();
                player1Input.Combat.FirstAttack.performed += FirstAttackInput;
                player1Input.Combat.HeavyAttack.performed += HeavyAttackInput;
                break;
            case PlayerState.Player2:
                player2Input = new Player2();
                player2Input.Combat.FirstAttack.performed += FirstAttackInput;
                player2Input.Combat.HeavyAttack.performed += HeavyAttackInput;
                break;
        }
    }

    public void FirstAttackInput(InputAction.CallbackContext context)
    {
        if (CurrentMove != null)
        {
            Destroy(CurrentMove.gameObject);
        }
        
        if(myClass.values.myStats.currentMana < LightAttack.manaCost)
            return;
        _animator.AttackAnimation(1);
        myClass.UseMana(LightAttack.manaCost);
        LightAttack.Spawn( offsetL, transform, transform.rotation.eulerAngles, true);
        LightAttack.setUser(myClass);
        CurrentMove = LightAttack.effect;
      
       
    }
    private void HeavyAttackInput(InputAction.CallbackContext context)
    {
        if (CurrentMove != null)
        {
            Destroy(CurrentMove.gameObject);
        }
        
        if(myClass.values.myStats.currentMana < HeavyAttack.manaCost)
            return;
        _animator.AttackAnimation(1);
        myClass.UseMana(HeavyAttack.manaCost);
        HeavyAttack.Spawn(offsetH , transform, transform.rotation.eulerAngles, true);
        HeavyAttack.setUser(myClass);
        CurrentMove = HeavyAttack.effect;
      
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
