using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonShooter
{
	public class PlayerStatus : MonoBehaviour {

        public int health;

        private void Update()
        {
            //GameOver;   
        }

        public void TookHit(int damage)
        {
            health -= damage;
        }


	}
}
