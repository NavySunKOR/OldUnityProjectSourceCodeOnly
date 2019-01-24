using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class WeaponAnimation : MonoBehaviour {

        public AudioSource audioSource;
        public AudioClip fireSound;
        public AudioClip reloadSound;
        private WeaponEvent weaponEvent;
        private Animator anim;

		private void OnEnable () {
            audioSource = GetComponent<AudioSource>();
            weaponEvent = GetComponent<WeaponEvent>();
            weaponEvent.WeaponFireEvent += PlayFireAnimation;
            weaponEvent.WeaponAimEvent += PlayAimAnimation;
            weaponEvent.WeaponReloadEvent += PlayReloadAnimation;
            anim = GetComponent<Animator>();

        }

		private void OnDisable () {
            weaponEvent.WeaponFireEvent -= PlayFireAnimation;
            weaponEvent.WeaponAimEvent -= PlayAimAnimation;
            weaponEvent.WeaponReloadEvent -= PlayReloadAnimation;
        }
        
        private void PlayFireAnimation()
        {
            anim.SetTrigger("fire");
        }

        private void PlayAimAnimation()
        {

        }

        private void PlayReloadAnimation(int noUse)
        {
            anim.SetTrigger("reload");
        }
	}
}
