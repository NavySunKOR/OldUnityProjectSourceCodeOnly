using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ThirdPersonShooter
{
	public class PlayerAnimatorController : MonoBehaviour, IActionable
    {

        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();

        }

        private void Update()
        {
            anim.SetFloat("HorizontalMove", PlayerStat.horizontalMove);
            anim.SetFloat("VerticalMove", PlayerStat.verticalMove);
            anim.SetBool("Crouch", PlayerStat.crouch);
            anim.SetBool("Aim", PlayerStat.aim);
            anim.SetBool("Run", PlayerStat.run);
            anim.SetBool("Cover", PlayerStat.cover);
            anim.SetInteger("CoverMovement", PlayerStat.coverMovement);
            anim.SetBool("PistolWield", PlayerStat.pistolWield);
            anim.SetBool("RifleWield", PlayerStat.rifleWield);
        }

        public void PistolWield()
        {
            PlayerStat.pistolWield = true;
            PlayerStat.rifleWield = false;
        }

        public void RifleWield()
        {
            PlayerStat.pistolWield = false;
            PlayerStat.rifleWield = true;
        }
        
        public void Reload()
        {
            if(PlayerStat.pistolWield || PlayerStat.rifleWield)
                anim.SetTrigger("Reloading");
        }

    }
}
