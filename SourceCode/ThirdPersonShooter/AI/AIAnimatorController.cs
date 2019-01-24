using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonShooter
{
	public class AIAnimatorController : MonoBehaviour {
        private AIAction aiAction;
        private Animator anim;

        private void Awake()
        {
            aiAction = GetComponent<AIAction>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            anim.SetBool("Tracking", aiAction.status.tracking);
            anim.SetBool("Die", aiAction.status.die);
           
        }

        private void Attack()
        {
            anim.SetTrigger("Attack");
        }

    }
}
