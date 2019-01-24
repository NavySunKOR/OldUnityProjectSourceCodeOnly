using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Animation;

namespace AI {

    [System.Serializable]
    public class AIStat
    {
        public int health;
        public bool tracking;
        public bool die;
        public float movementSpeed;
        public float rpm;
        public float attackInterval;
        public float attackTimer;
        public float detectRange;

    }

    [System.Serializable]
    public class SoundClips
    {
        public AudioClip[] zombieIdle;
        public AudioClip[] zombieAttack;
        public AudioClip[] zombieTookHit;
        public AudioClip zombieDie;
    }


    public class AIStatus : MonoBehaviour {
        public AIStat stat;
        public LayerMask playerLayer;
        public SoundClips sound;
        public Collider[] punch;
        private AIAnimatorController animController;
        private Transform player;
        private NavMeshAgent agent;
        private Collider coll;
        private AudioSource source;


        private void Awake()
        {
            stat.attackInterval = 60.0f / stat.rpm;
            animController = GetComponent<AIAnimatorController>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            agent.speed = stat.movementSpeed;
            coll = GetComponent<Collider>();
            source = GetComponent<AudioSource>();

        }


        private void Update()
        {
            if(stat.health <= 0 && !stat.die)
            {
                stat.die = true;
                coll.enabled = false;
                agent.isStopped = true;
                foreach (Collider coll in punch)
                {
                    coll.enabled = false;
                }
                source.PlayOneShot(sound.zombieDie);
                Destroy(gameObject, 4f);
            }
            else if(!stat.die)
            {
                if (!source.isPlaying)
                {
                    int idx = Random.Range(0, sound.zombieIdle.Length);
                    source.clip = sound.zombieIdle[idx];
                    source.Play();
                }
                if (Physics.CheckSphere(transform.position, stat.detectRange, playerLayer))
                {
                    stat.tracking = true;
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                    if(Vector3.Distance(transform.position,player.position) <= 1.5f && Time.time - stat.attackTimer >= stat.attackInterval)
                    {
                        stat.attackTimer = Time.time;
                        foreach(Collider coll in punch)
                        {
                            coll.enabled = true;
                        }
                        int idx = Random.Range(0, sound.zombieAttack.Length);
                        source.clip = sound.zombieAttack[idx];
                        source.Play();
                        agent.isStopped = true;
                        animController.Attack();
                    }
                    else
                    {
                        agent.isStopped = false;
                    }
                }
                else
                {
                    stat.tracking = false;
                    agent.isStopped = true;
                }
            }
        }

        private void TookHit(int damage)
        {
            int idx = Random.Range(0, sound.zombieTookHit.Length);
            source.clip = sound.zombieTookHit[idx];
            source.Play();
            stat.health -= damage;
            animController.TookHit();
            StartCoroutine(PauseForSecond(2f));
        }

        IEnumerator PauseForSecond(float time)
        {
            float speed = agent.speed;
            agent.speed = 0f;
            yield return new WaitForSeconds(time);
            agent.speed = speed;

        }

    }
}
