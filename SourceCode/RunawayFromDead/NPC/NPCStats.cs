using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RunawayFromDead {
    [RequireComponent(typeof(NavMeshAgent))]
	public class NPCStats : MonoBehaviour {

        public int health;
        public int damage;
        [Tooltip("Unit in seconds")]
        public float attackingDelay;
        public float trackingSpeed;
        public float trackingRange;
        private NPCEvents events;
        private NavMeshAgent navMeshAgent;
        private NPCAction action;
        private bool isAlive = true;
        private bool isDied; // stop call method twice


		private void OnEnable() { 
            events = GetComponent<NPCEvents>();
            events.NPCTookHitEvent += TookHit;
            events.NPCDieEvent += Die;
            events.NPCisAliveEvent += IsAlive;
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = trackingSpeed;
        }

		private void OnDisable () {
            events.NPCTookHitEvent -= TookHit;
            events.NPCDieEvent -= Die;
            events.NPCisAliveEvent -= IsAlive;
        }
       
		private void Update () {
            if(health <= 0)
            {
                isAlive = false;
            }
            if (!events.NPCisAliveEvent() && !isDied)
            {
                events.CallNPCDieEvent();
            }
		}


        private void TookHit(int damage)
        {
            health -= damage;
        }

        private void Die()
        {
            isDied = true;
            GetComponent<NavMeshAgent>().Stop();
            Destroy(gameObject, 3f);
        }

        private bool IsAlive()
        {
            return isAlive;
        }

    }
}
