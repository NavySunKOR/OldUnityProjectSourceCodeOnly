using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class WeaponEvent : MonoBehaviour {
        public delegate void WeaponReload(int amount);
        public delegate void WeaponAim();
        public delegate void WeaponFire();

        public WeaponReload WeaponReloadEvent;
        public WeaponAim WeaponAimEvent;
        public WeaponFire WeaponFireEvent;
        private Player player;

        private void OnEnable()
        {
            player = GetComponentInParent<Player>();
            player.CallPlayerAddWeaponReloadEvent(this);
        }

        private void OnDisable()
        {
            player.CallPlayerRemoveWeaponReloadEvent(this);
        }

        public void CallWeaponReloadEvent(int amount)
        {
            if(player.CallPlayerCanReloadEvent(amount))
                WeaponReloadEvent(amount);
        }

        public void CallWeaponAimEvent()
        {
            WeaponAimEvent();
        }

        public void CallWeaponFireEvent()
        {
            WeaponFireEvent();
        }

    }
}
