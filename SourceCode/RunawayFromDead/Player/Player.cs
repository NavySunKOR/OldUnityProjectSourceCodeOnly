using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class Player : MonoBehaviour {

        public delegate void PlayerTookHit(int damage);
        public delegate void PlayerPickupItem(Transform transform);
        public delegate void PlayerOpenInventory();
        public delegate void PlayerGameOver();
        public delegate void PlayerOpenMenu();
        public delegate void PlayerRefillHealth(int amount);
        public delegate void PlayerReloadAmmo(int amount);
        public delegate bool PlayerCanReload(int amount);
        public delegate void PlayerDiscardItem(int index,int amount);
        public delegate bool PlayerCanPickupItem(Transform tr);//TODO : AddArgument;
        public delegate void PlayerUseItem(int index, int amount);
        public delegate void PlayerShowItemPickupUI(string name);
        public delegate void PlayerOpenDoorUI();
        public delegate void PlayerOpenDoor();
        public delegate void PlayerOpenDoorFailed();
        public delegate void PlayerClearEventMessage();
        public delegate void PlayerAddWeaponReload(WeaponEvent eventDot);
        public delegate void PlayerRemoveWeaponReload(WeaponEvent eventDot);
        public delegate bool PlayerIsDead();
        public delegate void PlayerControllerOnOff();


        public PlayerTookHit PlayerTookHitEvent;
        public PlayerPickupItem PlayerPickupItemEvent;
        public PlayerOpenInventory PlayerOpenInventoryEvent;
        public PlayerGameOver PlayerGameOverEvent;
        public PlayerOpenMenu PlayerOpenMenuEvent;
        public PlayerRefillHealth PlayerRefillHealthEvent;
        public PlayerCanReload PlayerCanReloadEvent;
        public PlayerDiscardItem PlayerDiscardItemEvent;
        public PlayerCanPickupItem PlayerCanPickupItemEvent;
        public PlayerUseItem PlayerUseItemEvent;
        public PlayerShowItemPickupUI PlayerShowItemPickupUIEvent;
        public PlayerOpenDoorUI PlayerOpenDoorUIEvent;
        public PlayerClearEventMessage PlayerClearEventMessageEvent;
        public PlayerOpenDoorFailed PlayerOpenDoorFailedEvent;
        public PlayerAddWeaponReload PlayerAddWeaponReloadEvent;
        public PlayerRemoveWeaponReload PlayerRemoveWeaponReloadEvent;
        public PlayerIsDead PlayerIsDeadEvent;
        public PlayerControllerOnOff PlayerControllerOnOffEvent;


        private void OnEnable()
        {
            Time.timeScale = 1f;
            PlayerGameOverEvent += TogglePause;
            PlayerOpenInventoryEvent += TogglePause;
            PlayerOpenMenuEvent += TogglePause;
        }

        private void OnDisable()
        {
            PlayerGameOverEvent -= TogglePause;
            PlayerOpenInventoryEvent -= TogglePause;
            PlayerOpenMenuEvent -= TogglePause;
        }

        public void CallPlayerTookHitEvent(int damage)
        {
            PlayerTookHitEvent(damage);
        }

        public void CallPlayerPickupItemEvent(Transform tr)
        {
            if (PlayerCanPickupItemEvent(tr))
            {
                PlayerPickupItemEvent(tr);
            }
        }

        public void CallPlayerOpenInventoryEvent()
        {
            PlayerOpenInventoryEvent();
        }

        public void CallPlayerGameOverEvent()
        {
            PlayerGameOverEvent();
        }

        public void CallPlayerOpenMenuEvent()
        {
            PlayerOpenMenuEvent();
        }

        public void CallPlayerRefillHealthEvent(int amount)
        {
            PlayerRefillHealthEvent(amount);
        }

        public bool CallPlayerCanReloadEvent(int amount)
        {
            return PlayerCanReloadEvent(amount);
        }

        public void CallPlayerDiscardItemEvent(int index,int amount)
        {
            PlayerDiscardItemEvent(index, amount);
        }

        public void CallPlayerUseItemEvent(int index,int amount)
        {
            PlayerUseItemEvent(index, amount);
        }

        public void CallPlayerShowItemPickupUIEvent(string itemName)
        {
            PlayerShowItemPickupUIEvent(itemName);
        }

        public void CallPlayerOpenDoorUIEvent()
        {
            PlayerOpenDoorUIEvent();
        }

        public void CallPlayerClearEventMessageEvent()
        {
            PlayerClearEventMessageEvent();
        }

        public void CallPlayerOpenDoorFailedEvent()
        {
            PlayerOpenDoorFailedEvent();
        }

        public void CallPlayerAddWeaponReloadEvent(WeaponEvent eventDot)
        {
            PlayerAddWeaponReloadEvent(eventDot);
        }

        public void CallPlayerRemoveWeaponReloadEvent(WeaponEvent eventDot)
        {
            PlayerRemoveWeaponReloadEvent(eventDot);
        }

        //pause action. it doesn't check input.
        private void TogglePause()
        {
            Time.timeScale = (Time.timeScale > 0) ? 0 : 1;
        }

        public bool CallPlayerIsDeadEvent()
        {
            return PlayerIsDeadEvent();
        }

        public void CallPlayerControllerOnOffEvent()
        {
            PlayerControllerOnOffEvent();
        }
    }
}
