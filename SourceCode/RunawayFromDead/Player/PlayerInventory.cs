using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RunawayFromDead {
	public class PlayerInventory : MonoBehaviour {
        public Item[] inventory;
        public GameObject[] weapons;
        private Player player;
        [SerializeField]
        private int currentWeaponAmmoIndex = 0;
        [SerializeField]
        private int itemStoreIndex = 0;

        private void OnEnable()
        {
            player = GetComponent<Player>();
            player.PlayerCanReloadEvent += CheckAmmoAmount;
            player.PlayerCanPickupItemEvent += CanPickupItem;
            player.PlayerPickupItemEvent += PickupItem;
            player.PlayerUseItemEvent += UseItem;
            player.PlayerDiscardItemEvent += DiscardItem;
            player.PlayerAddWeaponReloadEvent += AddReloadEvent;
            player.PlayerRemoveWeaponReloadEvent += RemoveReloadEvent;
        }

        private void OnDisable()
        {
            player.PlayerCanReloadEvent -= CheckAmmoAmount;
            player.PlayerCanPickupItemEvent -= CanPickupItem;
            player.PlayerPickupItemEvent -= PickupItem;
            player.PlayerUseItemEvent -= UseItem;
            player.PlayerDiscardItemEvent -= DiscardItem;
            player.PlayerAddWeaponReloadEvent -= AddReloadEvent;
            player.PlayerRemoveWeaponReloadEvent -= RemoveReloadEvent;
        }


        private void PickupItem(Transform tr)
        {
            Debug.Log("Pick up in.");
            //check is already exist and put in
            ItemPickup itemPickup = tr.GetComponent<ItemPickup>();
            for (int i = 0; i < itemStoreIndex + 1; i++)
            {
                if (inventory[i].itemType == itemPickup.itemInfo.itemType && inventory[i].itemName == itemPickup.itemInfo.itemName)
                {
                    if (inventory[i].amount < inventory[i].maximumAmount)
                    {
                        inventory[i].amount += itemPickup.suppliedAmount;
                        //if maximum capacity reached, then store next.
                        if(inventory[i].amount > inventory[i].maximumAmount)
                        {
                            if(!(i + 1).Equals(inventory.Length))
                            {
                                inventory[i + 1].itemName = itemPickup.itemInfo.itemName;
                                inventory[i + 1].itemType = itemPickup.itemInfo.itemType;
                                inventory[i + 1].weaponType = itemPickup.itemInfo.weaponType;
                                inventory[i + 1].maximumAmount = itemPickup.itemInfo.maximumAmount;
                                inventory[i + 1].amount = inventory[i].amount - inventory[i].maximumAmount;
                                inventory[i].amount = inventory[i].maximumAmount;
                                ++itemStoreIndex;
                                Debug.Log("if maximum capacity reached, then store next.");
                                Destroy(tr.gameObject);
                                return; 
                            }
                            //If there's no more space, then leftovers will be discarded;
                            Debug.Log("If there's no more space, then leftovers will be discarded");
                            Destroy(tr.gameObject);
                            return;
                        }

                        //nothing wrong. so moving on.
                        Debug.Log("nothing wrong. so moving on.");
                        Destroy(tr.gameObject);
                        return;
                    }
                }
            }

            //else
            if((itemStoreIndex + 1) < inventory.Length) { 
                inventory[itemStoreIndex + 1] = new Item();
                inventory[itemStoreIndex + 1].itemName = itemPickup.itemInfo.itemName;
                inventory[itemStoreIndex + 1].itemType = itemPickup.itemInfo.itemType;
                inventory[itemStoreIndex + 1].weaponType = itemPickup.itemInfo.weaponType;
                inventory[itemStoreIndex + 1].maximumAmount = itemPickup.itemInfo.maximumAmount;
                inventory[itemStoreIndex + 1].amount = itemPickup.suppliedAmount;
                ++itemStoreIndex;
                Destroy(tr.gameObject);
            }
            else
            {
                Debug.Log("Can not pick up item. Need to do something on this.");
            }
        }
        
        private void EquipItem(int index)
        {
            WeaponType weaponType = inventory[index].weaponType;
            switch (weaponType) {
                case WeaponType.hg: weapons[0].SetActive(true); break;
                case WeaponType.sg: weapons[1].SetActive(true); break;
                case WeaponType.ar: weapons[2].SetActive(true); break;
                default: break;
            }

            currentWeaponAmmoIndex = index;

        }

        private void UnequipWeapon()
        {
            WeaponType weapon = inventory[currentWeaponAmmoIndex].weaponType;
            switch (weapon)
            {
                case WeaponType.hg: weapons[0].SetActive(false); break;
                case WeaponType.sg: weapons[1].SetActive(false); break;
                case WeaponType.ar: weapons[2].SetActive(false); break;
                default: break;
            }

        }

        //Global
        private void DiscardItem(int index, int amount)
        {
            inventory[index].amount -= amount;
            if(inventory[index].amount <= 0)
                inventory[index].ResetSlot();
        }

        //Global
        private void UseItem(int index,int amount)
        {
            ItemType itemType = inventory[index].itemType;
            switch (itemType) {
                case ItemType.ammo: break;
                case ItemType.heal: player.CallPlayerRefillHealthEvent(100); break;
                case ItemType.key: break;
                case ItemType.weapon: UnequipWeapon(); EquipItem(index); break;
                default: break;
            }

        }
        
        //Global
        private bool CheckAmmoAmount(int amount)
        {

            for (int i = itemStoreIndex; i >= 0; i--)
            {
                Debug.Log(inventory[i].itemType + "  " + inventory[currentWeaponAmmoIndex].weaponType);
                if (inventory[i].itemType == ItemType.ammo && inventory[i].weaponType == inventory[currentWeaponAmmoIndex].weaponType)
                {
                    Debug.Log("Check pass , " ) ;
                    return true;
                }
            }
            Debug.Log("Check nopass");
            return false;
        }

        //TODO : need argument
        private bool CanPickupItem(Transform tr)
        {
            //item space available check
            ItemPickup itemPickup = tr.GetComponent<ItemPickup>();
            if(itemStoreIndex + 1 < inventory.Length)
            {
                if (inventory[itemStoreIndex + 1].itemName.Equals(""))
                {
                    return true;
                }
                else
                {
                    for (int i = 0; i < itemStoreIndex + 1; i++)
                    {
                        if (inventory[i].itemType == itemPickup.itemInfo.itemType && inventory[i].itemName == itemPickup.itemInfo.itemName)
                        {
                            if (inventory[i].amount < inventory[i].maximumAmount)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        //Global
        private void ReloadAmmo(int amount)
        {
            WeaponType currentEquiped = inventory[currentWeaponAmmoIndex].weaponType;
            Debug.Log("WeaponType : " + currentEquiped);
            for(int i = itemStoreIndex ; i >= 0 ; i--)
            {
                Debug.Log("Checking..." + " " + inventory[i].itemType + " " + inventory[i].weaponType);
                if(inventory[i].itemType == ItemType.ammo && inventory[i].weaponType == currentEquiped)
                {
                    inventory[i].amount -= amount;
                    if(inventory[i].amount <= 0)
                    {
                        inventory[i].ResetSlot();
                    }
                    break;
                }
            }
        }

        private void AddReloadEvent(WeaponEvent eventDot)
        {
            Debug.Log("Successful");
            eventDot.WeaponReloadEvent += ReloadAmmo;
        }

        private void RemoveReloadEvent(WeaponEvent eventDot)
        {
            Debug.Log("Removed Successful");
            eventDot.WeaponReloadEvent -= ReloadAmmo;
        }






    }
}
