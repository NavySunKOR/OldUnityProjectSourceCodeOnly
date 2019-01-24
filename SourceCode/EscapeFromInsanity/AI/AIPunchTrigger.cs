using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
	public class AIPunchTrigger : MonoBehaviour {

        public int damage;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.SendMessage("TookHit", damage);
                transform.GetComponent<Collider>().enabled = false;
            }
        }

    }
}
