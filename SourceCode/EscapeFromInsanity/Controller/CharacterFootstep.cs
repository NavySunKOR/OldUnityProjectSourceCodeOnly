using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Controller {
	public class CharacterFootstep : MonoBehaviour {
        public AudioClip footStepSound;
        public float walkInterval;
        public float aimWalkInterval;
        public float runInterval;
        private float walkTimer;
        private float runTimer;
        private float aimWalkTimer;
        private PlayerStat stat;
        private AudioSource source;

        private void Awake()
        {
            stat = GetComponent<PlayerStatus>().stat;
            source = GetComponent<AudioSource>();
            source.clip = footStepSound;
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if(horizontal * horizontal > Mathf.Epsilon || vertical * vertical > Mathf.Epsilon)
            {
                if (stat.isRunning)
                {
                    if (Time.time - runTimer >= runInterval)
                    {
                        runTimer = Time.time;
                        source.Play();
                    }
                }
                else if(stat.aim)
                {
                    if (Time.time - aimWalkTimer >= aimWalkInterval)
                    {
                        aimWalkTimer = Time.time;
                        source.Play();
                    }
                }
                else
                {
                    if (Time.time - walkTimer >= walkInterval)
                    {
                        walkTimer = Time.time;
                        source.Play();
                    }
                }
            }
        }

    }
}
