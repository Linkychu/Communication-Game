using System;
using System.Collections;
using AIStateManager;
using Characters;
using UnityEngine;
using static EnemyRandomPathFinding;

namespace Data.AI
{
    public class AttackState : State
    {
        private bool canAttack = true;
        public float CoolDownDuration = 1f;
        public Moves effect;
        public Moves[] effects;
        public CharacterClass charClass;
        public Vector3 offset;
        public ChaseState chaseState;
        public IdleState idleState;

        public float effectTime = 1f;
        private float time;

        private void Awake()
        {
            idleState = transform.parent.GetComponentInChildren<IdleState>();
        }

        public override State RunCurrentState()
        {
            
            switch (gameObject.tag)
            {
                case "Enemy":
                    if (!EnemyAi.IsPlayerInRange() || EnemyAi.target == null)
                    {
                        EnemyAi.SwitchState(enemyState.chase);
                        return chaseState;
                    }
                    
                    else if (EnemyAi.target != null)
                    {
                        StartCoroutine(Attack());
                        return this;
                    }
                    
                    break;

            }

            return chaseState;
        }

        IEnumerator Attack()
        {
            if (canAttack)
            {
                Moves effectRef;
                AttackEffect effectObj;
                switch (gameObject.tag)
                {
                    case "Enemy":
                        EnemyAi.SwitchState(enemyState.attack);
                        charClass = EnemyAi.gameObject.GetComponent<CharacterClass>();
                        if ((charClass.values.myStats.currentMana < effect.manaCost))
                        {
                            yield return null;
                        }

                        charClass.values.myStats.currentMana -= effect.manaCost;
                        CoolDownDuration = effectTime + effect.lifetime;
                        effectRef = effect;
                        effectRef.Spawn(EnemyAi.target.position + transform.forward, transform, transform.rotation.eulerAngles, false);
                        effectRef.setUser(charClass);
                        effectObj = effectRef.effect;

                        break;

                }
                
                StartCoroutine(CoolDownTimer());
            }

            else
            {
                yield return new WaitUntil(() => canAttack);
            }
        }
        
        IEnumerator CoolDownTimer()
        {
            canAttack = false;
            if (gameObject.CompareTag("Ally"))
            {
                yield return new WaitForSeconds(CoolDownDuration);

            }

            else
            {
                yield return new WaitForSeconds(CoolDownDuration);

            }
          
           
            canAttack = true;


        }
    }
}