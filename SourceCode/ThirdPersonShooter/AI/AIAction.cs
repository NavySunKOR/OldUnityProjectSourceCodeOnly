using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ThirdPersonShooter
{
	public class AIAction : MonoBehaviour {
        
        public LayerMask player;
        public AIStatus status;
        private NavMeshAgent agent;
        private RaycastHit hit;
        private AIAnimatorController anim;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<AIAnimatorController>();
            agent.speed = status.movementSpeed;
        }

        private void Update()
        {
            if(status.health <= 0)
            {
                status.die = true;
            }

            if (!status.die)
            {
                Collider[] colls = Physics.OverlapSphere(transform.position, status.detectionRange, player);
                if (colls.Length > 0)
                {
                    status.tracking = true;
                    foreach (Collider coll in colls)
                    {
                        agent.SetDestination(coll.transform.position);
                        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(coll.transform.position - transform.position),3f * Time.deltaTime);
                        if (Vector3.Distance(coll.transform.position, transform.position) <= status.attackRange)
                        {
                            status.tracking = false;
                            agent.isStopped = true;
                            Attack();
                        }
                        else
                        {
                            status.tracking = true;
                            agent.isStopped = false;
                            agent.SetDestination(coll.transform.position);
                        }
                    }
                }
                else
                {
                    status.tracking = false;
                }
            }
            else
            {
                status.tracking = false;
                agent.isStopped = true;
                Invoke("DisableObject", 6f);
            }
            
            
        }

        private void TookHit(int damage)
        {
            status.health -= damage;
        }

        private void Attack()
        {
           if(Time.time - status.attackTimer > status.attackInterval)
           {
                status.attackTimer = Time.time;
               anim.SendMessage("Attack");
               Ray ray = new Ray(transform.position, transform.forward);
               if (Physics.Raycast(ray, out hit, status.attackRange, player))
               {
                   hit.transform.SendMessage("TookHit", status.damage);
               }
            }
            
        }

        private void DisableObject()
        {
            gameObject.SetActive(false);
        }




    }
}
