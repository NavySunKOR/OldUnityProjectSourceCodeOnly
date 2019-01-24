using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace RunawayFromDead {
	public class PlayerUIController : MonoBehaviour {

        public GameObject inventoryPanel;
        public GameObject menuPanel;
        public GameObject gameOverPanel;
        public Text eventMessage;
        //1,2:Handgun ; 3,4:M4A1 ; 5,6:Shotgun;
        public GameObject[] uiObjects;
        public Transform inventoryUIParent;
        public Button discardButton;
        public Image[] healthImages;
        public Text healthText;
        private Player player;
        private PlayerStats playerStats;
        private bool isDiscarding;

        private void OnEnable()
        {
            player = GetComponent<Player>();
            playerStats = GetComponent<PlayerStats>();
            ToggleCursor();
            player.PlayerOpenInventoryEvent += OpenInventory;
            player.PlayerOpenInventoryEvent += ToggleCursor;
            player.PlayerGameOverEvent += OpenGameOver;
            player.PlayerGameOverEvent += ToggleCursor;
            player.PlayerOpenMenuEvent += OpenMenu;
            player.PlayerOpenMenuEvent += ToggleCursor;
            player.PlayerOpenInventoryEvent += UpdateItemList;
            player.PlayerOpenDoorUIEvent += OpenDoorUI;
            player.PlayerShowItemPickupUIEvent += ShowItemPickupUI;
            player.PlayerClearEventMessageEvent += ClearEventMessage;
        }

        private void OnDisable()
        {
            player.PlayerOpenInventoryEvent -= OpenInventory;
            player.PlayerOpenInventoryEvent -= ToggleCursor;
            player.PlayerGameOverEvent -= OpenGameOver;
            player.PlayerGameOverEvent -= ToggleCursor;
            player.PlayerOpenMenuEvent -= OpenMenu;
            player.PlayerOpenMenuEvent -= ToggleCursor;
            player.PlayerOpenInventoryEvent -= UpdateItemList;
            player.PlayerOpenDoorUIEvent -= OpenDoorUI;
            player.PlayerShowItemPickupUIEvent -= ShowItemPickupUI;
            player.PlayerClearEventMessageEvent -= ClearEventMessage;
        }

        private void Update()
        {
            if (!player.CallPlayerIsDeadEvent())
            {
                CheckInput();
            }

            UpdatingHealthBar();
        }

        private void CheckInput()
        {
            if (InputManager.GetKeyDown(InputNames.openInventory))
            {
                player.CallPlayerOpenInventoryEvent();
            }

            if (InputManager.GetKeyDown(InputNames.pause))
            {
                player.CallPlayerOpenMenuEvent();
            }
        }

        private void OpenInventory()
        {
            player.CallPlayerControllerOnOffEvent();
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }

        private void OpenMenu()
        {
            player.CallPlayerControllerOnOffEvent();
            menuPanel.SetActive(!menuPanel.activeSelf);
        }

        private void OpenGameOver()
        {
            player.CallPlayerControllerOnOffEvent();
            menuPanel.SetActive(!menuPanel.activeSelf);
            gameOverPanel.SetActive(true);
        }

        private void ToggleCursor()
        {
            Debug.Log(Cursor.lockState + " / " + Cursor.visible);
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = (Cursor.visible == false) ? true : false;
        }


        private void UpdateItemList()
        { 
            foreach (Transform child in inventoryUIParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            Item[] items = GetComponentInParent<PlayerInventory>().inventory;

            for (int i = 0; i < items.Length; i++)
            {
                if (!PropertyName.IsNullOrEmpty(items[i].itemName))
                {
                    GameObject uiObj;
                    switch (items[i].itemType)
                    {

                        case ItemType.ammo:
                            uiObj = Instantiate(uiObjects[GetAmmoInfo(items[i])]) as GameObject;
                            uiObj.transform.SetParent(inventoryUIParent, false);
                            uiObj.GetComponentInChildren<Text>().text = items[i].amount.ToString();
                            int temp = i;
                            int dropAmount = 0;
                            switch (items[i].weaponType)
                            {
                                case WeaponType.ar: dropAmount = 10; break;
                                case WeaponType.sg: dropAmount = 3; break;
                                case WeaponType.hg: dropAmount = 10; break;
                                default: break;
                            }

                            uiObj.GetComponent<Button>().onClick.AddListener(delegate { if (isDiscarding) player.CallPlayerDiscardItemEvent(temp, dropAmount); else player.CallPlayerUseItemEvent(temp, 1); });
                            break;
                        case ItemType.weapon:
                            uiObj = Instantiate(uiObjects[GetWeaponInfo(items[i])]) as GameObject;
                            uiObj.transform.SetParent(inventoryUIParent, false);
                            uiObj.GetComponentInChildren<Text>().text = "1";
                            int tempWeapon = i;
                            uiObj.GetComponent<Button>().onClick.AddListener(delegate { if (isDiscarding) player.CallPlayerDiscardItemEvent(tempWeapon,1); else player.CallPlayerUseItemEvent(tempWeapon, 1); });
                            break;
                        default:
                            uiObj = Instantiate(uiObjects[GetItemInfo()]) as GameObject;
                            uiObj.transform.SetParent(inventoryUIParent, false);
                            uiObj.GetComponentInChildren<Text>().text = items[i].amount.ToString();
                            int tempItem = i;
                            uiObj.GetComponent<Button>().onClick.AddListener(delegate { if (isDiscarding) player.CallPlayerDiscardItemEvent(tempItem,1); else player.CallPlayerUseItemEvent(tempItem, 1); });
                            break;
                    }
                }
            }
        }

        private int GetWeaponInfo(Item item)
        {
            switch (item.weaponType)
            {
                case WeaponType.hg : return 0;
                case WeaponType.ar : return 2; 
                case WeaponType.sg : return 4;
                default: return 7; 
            }
        }

        private int GetAmmoInfo(Item item)
        {
            switch (item.weaponType)
            {
                case WeaponType.hg: return 1;
                case WeaponType.ar: return 3;
                case WeaponType.sg: return 5;
                default: return 7;
            }
        }

        private int GetItemInfo()
        {
            //HealthItem
            return 6;
        }

        public void SetDiscard()
        {
            isDiscarding = !isDiscarding;
            discardButton.image.color = (isDiscarding) ? Color.red : Color.white;
        }

        private void ShowItemPickupUI(string itemName)
        {
            eventMessage.text = itemName + "을 획득하려면 E키를 누르십시오.";
        }

        private void OpenDoorUI()
        {
            eventMessage.text = "E키를 누르십시오.";
        }

        private void ClearEventMessage()
        {
            eventMessage.text = "";
        }

        private void UpdatingHealthBar()
        {
            if(playerStats.health > 60)
            {
                SetHealthGreen();
            }
            else if(playerStats.health > 30)
            {
                SetHealthYellow();
            }
            else
            {
                SetHealthRed();
            }

            healthText.text = playerStats.health.ToString();
        }

        private void SetHealthGreen()
        {
            for (int i = 0; i < healthImages.Length; i++)
            {
                healthImages[i].color = Color.green;
            }
            healthText.color = Color.green;
        }

        private void SetHealthYellow()
        {
            for (int i = 0; i < healthImages.Length; i++)
            {
                healthImages[i].color = Color.yellow;
            }
            healthText.color = Color.yellow;
        }

        private void SetHealthRed()
        {
            for (int i = 0; i < healthImages.Length; i++)
            {
                healthImages[i].color = Color.red;
            }
            healthText.color = Color.red;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Show");
        }

        public void ExitToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
    }
}
