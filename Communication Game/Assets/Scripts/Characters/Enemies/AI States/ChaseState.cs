using UnityEngine;
using static EnemyRandomPathFinding;
using AIStateManager;


namespace Data.AI
{
    
   
    public class ChaseState : State
    {
        public AttackState attackState;
        public IdleState IdleState => transform.parent.GetComponentInChildren<IdleState>();
        public override State RunCurrentState()
        {
            if (isInAttackRange())
            {
              
                EnemyAi.SwitchState(enemyState.attack);
                return attackState;
            }

            else
            {
                switch (gameObject.tag)
                {
                    case "Enemy":
                        if (EnemyAi.target != null)
                        {
                            EnemyAi.SwitchState(enemyState.chase);
                        }

                        else
                        {
                            IdleState.canSeeTarget = false;
                            return IdleState;
                            
                        }

                        break;
                }
                return this;
            }
            
        }

        public bool isInAttackRange()
        {
            switch (gameObject.tag)
            {
                case "Enemy" when EnemyAi != null:
                    return EnemyAi.IsPlayerInRange();
                default:
                    return false;
            }
        }
    }
}