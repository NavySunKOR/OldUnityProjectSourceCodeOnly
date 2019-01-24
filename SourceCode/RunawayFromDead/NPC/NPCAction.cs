using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RunawayFromDead {
	public class NPCAction : MonoBehaviour {

        public LayerMask player;
        [Tooltip("Unit in seconds.")]
        public float searchDelay;
        private NavMeshAgent agent;
        private NPCStats stats;
        private NPCEvents events;
        private float searchTimer;
        private float attackTimer;
        private Transform playerTr;
        private SphereCollider[] punches;

        private void Awake()
        {
            stats = GetComponent<NPCStats>();
            events = GetComponent<NPCEvents>();
            agent = GetComponent<NavMeshAgent>();
            playerTr = GameObject.FindGameObjectWithTag("Player").transform;
            punches = GetComponentsInChildren<SphereCollider>();
        }

        private void OnEnable () {
            events.NPCTrackingEvent += Tracking;
            events.NPCAttackingEvent += Attacking;
            events.NPCTrackingAnimationEvent += NotAttacking;
            events.NPCLostTrackingAnimationEvent += NotAttacking;

        }

		private void OnDisable () {
            events.NPCTrackingEvent -= Tracking;
            events.NPCAttackingEvent -= Attacking;
            events.NPCTrackingAnimationEvent -= NotAttacking;
            events.NPCLostTrackingAnimationEvent -= NotAttacking;
        }
        
		private void Update () {
            if (events.CallNPCisAliveEvent())
            {
                if (Time.time - searchTimer > searchDelay)
                {
                    searchTimer = Time.time;
                    Collider[] colliders = Physics.OverlapSphere(transform.position, stats.trackingRange, player);
                    if (colliders.Length > 0)
                        events.CallNPCTrackingEvent(colliders[0].transform);
                    else
                        events.CallNPCLostTrackingAnimationEvent();
                }

                if (Vector3.Distance(playerTr.position, transform.position) < (agent.stoppingDistance + .5f)
                     && Time.time - attackTimer > stats.attackingDelay)
                {

                    attackTimer = Time.time;
                    events.CallNPCAttackingEvent();
                }
            }
		}

        private void Tracking(Transform transform)
        {
            agent.SetDestination(transform.position);
        }

        private void Attacking()
        {
            foreach(SphereCollider collider in punches)
            {
                collider.enabled = true;
            }
        }

        private void NotAttacking()
        {
            foreach (SphereCollider collider in punches)
            {
                collider.enabled = false;
            }
        }
        


	}
}
