using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class PlayerItemPickup : MonoBehaviour {

        public LayerMask item;
        public float detectionRange;
        private RaycastHit hit;
        private Player player;
        private Transform tr;
        private GlobalEventManager ge;

		private void OnEnable () {
            player = GetComponent<Player>();
            tr = Camera.main.transform;
            ge = GameObject.FindGameObjectWithTag("GlobalEventManager").GetComponent<GlobalEventManager>();
        }
        
		private void Update () {
            if (!player.CallPlayerIsDeadEvent())
            {
                CheckItemPickup();
            }
        }

        private void CheckItemPickup()
        {
            Debug.DrawLine(transform.position, transform.forward * detectionRange, Color.green);
            if (Physics.Raycast(tr.position, tr.forward, out hit, detectionRange, item))
            {
                if(hit.transform.GetComponent<Door>() != null)
                {
                    player.CallPlayerOpenDoorUIEvent();
                    if (InputManager.GetKeyDown(InputNames.use))
                    {
                        hit.transform.GetComponent<Door>().OpenDoor();
                    }
                }
                else if(hit.transform.GetComponent<ItemPickup>() != null)
                {
                    string itemName = hit.transform.GetComponent<ItemPickup>().itemInfo.itemName;
                    player.CallPlayerShowItemPickupUIEvent(itemName);
                    if (InputManager.GetKeyDown(InputNames.use))
                    {
                        player.CallPlayerPickupItemEvent(hit.transform);
                    }
                }
                else if (hit.transform.GetComponent<KeyPickups>() != null)
                {
                    player.CallPlayerShowItemPickupUIEvent(hit.transform.GetComponent<KeyPickups>().keyName);
                    if (InputManager.GetKeyDown(InputNames.use))
                    {
                        ge.AddKey(hit.transform.GetComponent<KeyPickups>().keyName);
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                player.CallPlayerClearEventMessageEvent();
            }
        }
        
    }
}
