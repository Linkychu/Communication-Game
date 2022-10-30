using System;
using UnityEngine;
using AIStateManager;
using Player;

namespace Data.AI
{

   
    
    public abstract class State: MonoBehaviour
    {
        protected EnemyRandomPathFinding EnemyAi;


        private void Start()
        {
            gameObject.tag = transform.parent.tag;
            if (gameObject.CompareTag("Enemy"))
            {
                EnemyAi = GetComponentInParent<EnemyRandomPathFinding>();
            }
        }

        public abstract State RunCurrentState();
    }
}