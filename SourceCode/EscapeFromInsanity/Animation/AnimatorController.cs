using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Animation {
    [RequireComponent(typeof(Animator))]
	public class AnimatorController : MonoBehaviour, Actor
    {

        public enum WeaponWield {none,pistolWield,rifleWield};
        private WeaponWield wield;
        private PlayerStat stat;
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            stat = GetComponent<PlayerStatus>().stat;
        }

        private void Update()
        {
            anim.SetFloat("Horizontal",stat.horizontal);
            anim.SetFloat("Vertical",stat.vertical);
            anim.SetBool("Aim",stat.aim);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                wield = WeaponWield.pistolWield;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                wield = WeaponWield.rifleWield;
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                wield = WeaponWield.none;
            }


            anim.SetBool("PistolWield", (wield == WeaponWield.pistolWield) ? true : false);
            anim.SetBool("RifleWield", (wield == WeaponWield.rifleWield) ? true : false);
            
        }

        public void Reload()
        {
            anim.SetTrigger("Reload");
        }

        public void Die()
        {

        }

    }
}
