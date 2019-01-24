using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class NPCAttackTrigger : MonoBehaviour {
        private SphereCollider punch;
        private int damage;

        private void Awake()
        {
            punch = GetComponent<SphereCollider>();
            damage = GetComponentInParent<NPCStats>().damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                other.transform.GetComponent<Player>().CallPlayerTookHitEvent(damage);
                punch.enabled = false;
            }
        }
    }
}
