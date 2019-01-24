using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class NPCAnimation : MonoBehaviour {
        private Animator animator;
        private NPCEvents events;



        private void Awake()
        {
            animator = GetComponent<Animator>();
            events = GetComponent<NPCEvents>();
        }
        private void OnEnable () {
            events.NPCTrackingAnimationEvent += Tracking;
            events.NPCAttackAnimationEvent += Attack;
            events.NPCDieAnimationEvent += Die;
            events.NPCLostTrackingAnimationEvent += LostTracking;
        }
		private void OnDisable () {
            events.NPCTrackingAnimationEvent -= Tracking;
            events.NPCAttackAnimationEvent -= Attack;
            events.NPCDieAnimationEvent -= Die;
            events.NPCLostTrackingAnimationEvent -= LostTracking;
        }
        private void Tracking()
        {
            animator.SetBool("tracking", true);
        }
        private void LostTracking()
        {
            animator.SetBool("tracking", false);
        }
        private void Die()
        {
            animator.SetTrigger("die");
        }
        private void Attack()
        {
            Debug.Log("Attacking!");
            animator.CrossFade("Attack",0.2f);
        }
	}
}
