using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    

    private int isWalkingHash;

    private int isRunningHash;

    private bool isSprinting;

    private PlayerManager _playerManager;
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        _playerManager = GetComponentInParent<PlayerManager>();

    }
    
    

    // Update is called once per frame
    void Update()
    {
        WalkAnimation();
    }

    private void LateUpdate()
    {
       
    }

    private void WalkAnimation()
    {
        switch (_playerManager.playerState)
        {
            
            case PlayerState.Player1:
            {
                bool successful = TryGetComponent(out PlayerOneMovement P1);
                if (successful)
                {
                    isSprinting = P1.isSprinting;

                    if (P1.moveVelocityRef != Vector3.zero)
                    {
                        animator.SetBool(isWalkingHash, !isSprinting);
                        animator.SetBool(isRunningHash, isSprinting);
                    }

                    else
                    {
                        animator.SetBool(isWalkingHash, false);
                        animator.SetBool(isRunningHash, false);
                    }
                }
                
               
            }

                break;

            case PlayerState.Player2:
            {
                bool successful = TryGetComponent(out PlayerTwoMovement P2);
                if (successful)
                {
                    isSprinting = P2.isSprinting;

                    if (P2.moveVelocityRef != Vector3.zero)
                    {
                        animator.SetBool(isWalkingHash, !isSprinting);
                        animator.SetBool(isRunningHash, isSprinting);
                    }

                    else
                    {
                        animator.SetBool(isWalkingHash, false);
                        animator.SetBool(isRunningHash, false);
                    }
                }

                break;
            }
        }
    }

    public void AttackAnimation(int id)
    {
        switch (id)
        {
            case 1:
                animator.SetTrigger(AttackHash);
                break;
            
                
        }
      
    }
}
