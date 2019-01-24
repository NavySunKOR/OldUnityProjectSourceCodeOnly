using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class PlayerStats : MonoBehaviour {
        
        public float walkSpeed = 10f;
        public float runSpeed = 30f;
        [Range(0,100)]
        public int health = 100;
        [Range(0, 100)]
        public float stamina = 100f;

        private Player player;
        private FirstPersonController fpsController;
        private FirstPersonCamera fpsCamera;
        private bool isDead = false;

        private void OnEnable () {
            player = GetComponent<Player>();
            player.PlayerTookHitEvent += TookDamage;
            player.PlayerRefillHealthEvent += RefillHealth;
            fpsController = GetComponent<FirstPersonController>();
            fpsController.walkSpeed = walkSpeed;
            fpsController.runSpeed = runSpeed;
            fpsCamera = GetComponent<FirstPersonCamera>();
            player.PlayerGameOverEvent += Die;
            player.PlayerIsDeadEvent += IsPlayerDied;
            player.PlayerControllerOnOffEvent += ActDeactControllers;
        }

		private void OnDisable () {
            player.PlayerTookHitEvent -= TookDamage;
        }

		private void Update () {
            if (!player.CallPlayerIsDeadEvent())
            {
                CheckRunState();
            }
        }

        private void TookDamage(int damage)
        {
            health -= damage;
            if(health <= 0)
            {
                player.CallPlayerGameOverEvent();
            }
        }

        private void Die()
        {
            isDead = true;
        }

        private void RefillHealth(int amount)
        {
            health += amount;
            if(health > 100)
            {
                health = 100;
            }
        }


        //This is not included with event because it should be run independly.
        private void CheckRunState()
        {
            if (fpsController.isRun)
            {
                stamina -= 2 * Time.deltaTime;
            }
            else
            {
                if(stamina < 100)
                    stamina += 1 * Time.deltaTime;
            }

            if(stamina <= 0)
            {
                fpsController.canRun = false;
            }
            else
            {
                fpsController.canRun = true;
            }
        }

        private bool IsPlayerDied()
        {
            return isDead;
        }

        private void ActDeactControllers()
        {
            fpsCamera.enabled = !fpsCamera.enabled;
            fpsController.enabled = !fpsController.enabled;
        }

	}
}
