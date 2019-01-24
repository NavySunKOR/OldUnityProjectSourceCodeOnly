using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    [System.Serializable]
    public class PlayerStat
    {
        public bool die;
        public int health;
        public float horizontal;
        public float vertical;
        public bool aim;
        public bool isRunning;
    }

	public class PlayerStatus : MonoBehaviour {
        public PlayerStat stat;
        public GameObject flashlight;
        public GameObject bloodEffect;
        public Transform bloodEffectPos;

        private void Update()
        {
            if(stat.health <= 0)
            {
                stat.die = true;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                flashlight.SetActive(!flashlight.activeSelf);
            }
        }

        public void TookHit(int damage)
        {
            stat.health -= damage;
            BloodSplat();
        }

        void BloodSplat()
        {
            GameObject go = Instantiate(bloodEffect, bloodEffectPos);
            go.transform.position = bloodEffectPos.position;
            Destroy(go, 1.5f);
        }
        

    }
}
