using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
    public enum ItemType
    {
        heal,weapon,ammo,key
    }
    public enum WeaponType
    {
        hg,sub,ar,sg
    }

    [System.Serializable]
    public class Item {
        public string itemName;
        public ItemType itemType;
        public WeaponType weaponType; // If itemType is ammo....
        public int amount;
        public int maximumAmount;

        public void ResetSlot()
        {
            itemName = "";
            itemType = ItemType.heal;
            weaponType = WeaponType.hg;
            amount = 0;
            maximumAmount = 0;
        }
	}

    [System.Serializable]
    public class WeaponMagazine
    {
        public WeaponType weaponType;
        public int ammoAmount;
        public int maximumAmount;
    }
}
