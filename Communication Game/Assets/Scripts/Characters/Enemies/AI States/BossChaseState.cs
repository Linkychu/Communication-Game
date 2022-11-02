using Player;
using UnityEngine;

namespace Characters.Enemies.AI_States
{
    public class BossChaseState : StateMachineBehaviour
    {
        private BossAI ai;
        private static readonly int Attack = Animator.StringToHash("Attack");
        

        private float attackTime = 0;
        private float attackTimeLimit = 3f;
        private int index;
        private Transform target;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (ai == null)
            {
                ai = animator.GetComponent<BossAI>();
                if (PlayerStateManager.instance == null)
                {
                    index = 0;
                    target = GameObject.FindObjectOfType<PlayerClass>().transform;

                }

                else
                {
                    index = UnityEngine.Random.Range(0, PlayerStateManager.instance.alivePlayers.Count);
                    target = PlayerStateManager.instance.alivePlayers[index].transform;
                }

            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.ResetTrigger(Attack);
           


        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if (attackTime < attackTimeLimit)
            {
                if (Vector3.Distance(ai.transform.position, target.position) <= ai.AttackRange)
                {

                    attackTime += Time.deltaTime;
                    ai.SetTarget(target);
                    animator.SetTrigger(Attack);

                    
                }

                else
                {
                    ai.agent.destination = target.position;
                }
                
            }

            else
            {
                animator.Play("Robo_Idle");
            }



        }
    }
}
