using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

namespace Animation {
	public class AIAnimatorController : MonoBehaviour, AIActor
    {
        private AIStat status;
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            status = GetComponent<AIStatus>().stat;
        }

        private void Update()
        {
            if (status.die)
            {
                anim.SetBool("die",true);
            }
            else
            {
                anim.SetBool("tracking", (status.tracking) ? true : false);
            }
        }

        public void Attack()
        {
            anim.SetBool("attack",true);
        }

        public void TookHit()
        {
            anim.SetTrigger("hit");
        }




    }
}
