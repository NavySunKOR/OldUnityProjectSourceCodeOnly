using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class NPCEvents : MonoBehaviour {

        public delegate void NPCTracking(Transform tr);
        public delegate void NPCAttacking();
        public delegate void NPCTookHit(int damage);
        public delegate void NPCDie();
        public delegate bool NPCisAlive();

        public delegate void NPCTrackingAnimation();
        public delegate void NPCLostTrackingAnimation();
        public delegate void NPCDieAnimation();
        public delegate void NPCAttackAnimation();

        public NPCTracking NPCTrackingEvent;
        public NPCAttacking NPCAttackingEvent;
        public NPCTookHit NPCTookHitEvent;
        public NPCDie NPCDieEvent;
        public NPCisAlive NPCisAliveEvent;

        public NPCTrackingAnimation NPCTrackingAnimationEvent;
        public NPCLostTrackingAnimation NPCLostTrackingAnimationEvent;
        public NPCDieAnimation NPCDieAnimationEvent;
        public NPCAttackAnimation NPCAttackAnimationEvent;



        /*
         * tracking, attactking, took hit, die.
         */

        public void CallNPCTrackingEvent(Transform tr)
        {
            NPCTrackingAnimationEvent();
            NPCTrackingEvent(tr);
        }

        public void CallNPCLostTrackingAnimationEvent()
        {
            NPCLostTrackingAnimationEvent();
        }

        public void CallNPCAttackingEvent()
        {
            NPCAttackAnimationEvent();
            NPCAttackingEvent();
        }

        public void CallNPCTookHitEvent(int damage)
        {
            NPCTookHitEvent(damage);
        }

        public void CallNPCDieEvent()
        {
            NPCDieAnimationEvent();
            NPCDieEvent();
        }

        public bool CallNPCisAliveEvent()
        {
            return NPCisAliveEvent();
        }


    }
}
