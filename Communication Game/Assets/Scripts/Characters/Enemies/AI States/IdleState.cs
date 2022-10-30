using AIStateManager;
using UnityEngine;
using static EnemyRandomPathFinding;

namespace Data.AI
{
    public class IdleState : State
    {
        public ChaseState chaseState;
        public bool canSeeTarget;
        


        public override State RunCurrentState()
        {
            
            
            if (canSeeTarget)
            {
                EnemyAi.SwitchState(enemyState.chase);
                return chaseState;
            }

            else
            {
                LookForTarget();
                return this;
            }
           
        }


        void LookForTarget()
        {
            
            if (gameObject.CompareTag("Enemy") && EnemyAi != null)
            {
                EnemyAi.SwitchState(enemyState.patrol);
                
                canSeeTarget = EnemyAi.IsPlayerVisible();
                    
            }
            
        }
    }
}