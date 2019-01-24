using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EightClassics.SnakeGame {
	public class GameManager : MonoBehaviour {

        public GameObject playerGraphicalObject;
        public GameObject backgrounGraphicalObject;
        public GameObject itemGraphicalObject;
        public Text gameOverMessage;
        public int mapSizeX;
        public int mapSizeY;
        public int playerMovingSpeed;
        public float checkInterval;


        private bool isGameOn = true;

        private List<GameObject> playerInstantObjects;
        private GameObject itemInstantObject;

        [SerializeField]
        private Player player;
        [SerializeField]
        private Map map;
        [SerializeField]
        private Item item;
        private float timer;

        private void Awake()
        {
            player = new Player();
            map = new Map();
            item = new Item();
            playerInstantObjects = new List<GameObject>();
            player.InitReferences((int)mapSizeX/2, (int)mapSizeY/2);
            map.SetMap(mapSizeX, mapSizeY);
            SpawnItem();
            GameInitRender();

        }

        private void Update()
        {
            if (isGameOn)
            {
                InputCheck();
                GameLoop();
                GameGraphicRender();
            }
            else
            {
                gameOverMessage.text = "Game Over!";
            }
        }

        private void InputCheck()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && player.moveAxisX != -1)
            {
                player.moveAxisX = 1;
                player.moveAxisY = 0;
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow) && player.moveAxisX != 1)
            {
                player.moveAxisX = -1;
                player.moveAxisY = 0;
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow) && player.moveAxisY != -1)
            {
                player.moveAxisX = 0;
                player.moveAxisY = 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && player.moveAxisY != 1)
            {
                player.moveAxisX = 0;
                player.moveAxisY = -1;
            }

        }

        // position update and movement in game logic.
        private void GameLoop()
        {
            if(Time.time - timer > checkInterval)
            {
                timer = Time.time;

                //기존에 있던 장소를 변경 시 업데이트 하는 로직을 손볼 필요 있음.
                CheckOutOfBoundGameOver();

                for (int i = player.tails.Count - 1 ; i >= 0; i--)
                {
                    if(i == 0)
                    {
                        map.map[player.tails[i].posX, player.tails[i].posY] = 0;
                        player.tails[i].posX = player.headPosX;
                        player.tails[i].posY = player.headPosY;
                    }
                    else
                    {
                        map.map[player.tails[i].posX, player.tails[i].posY] = 0;
                        player.tails[i].posX = player.tails[i - 1].posX;
                        player.tails[i].posY = player.tails[i - 1].posY;
                    }
                }
                //머리만 움직이므로 
                if (player.tails.Count <= 0)
                    map.map[player.headPosX, player.headPosY] = 0;

                player.headPosX += player.moveAxisX;
                player.headPosY += player.moveAxisY;

                CheckOutOfBoundGameOver();

                if (map.map[player.headPosX, player.headPosY] == Map.itemId)
                {
                    player.AddScore(100);
                    SpawnItem();
                    GameObject playerInstance = Instantiate(playerGraphicalObject, new Vector3(player.tails[player.tails.Count - 1].posX, player.tails[player.tails.Count - 1].posY, 1), Quaternion.identity);
                    playerInstantObjects.Add(playerInstance);
                    PositionUpdate();
                }
                else if (map.map[player.headPosX, player.headPosY] == Map.playerId)
                {
                    isGameOn = false;
                }
                else
                {
                    PositionUpdate();
                }


            }

        }

        private void SpawnItem()
        {
            item.posX = Random.Range(0, map.map.GetLength(0));
            item.posY = Random.Range(0, map.map.GetLength(1));

            //플레이어가 있을 만한 장소에 숫자를 제외하는것이 일반적이나, 해당 내용을 제외하였다.
            map.map[item.posX, item.posY] = Map.itemId;
        }

        private void CheckOutOfBoundGameOver()
        {
            if (player.headPosX < 0 || player.headPosX >= map.map.GetLength(0))
            {
                isGameOn = false;
            }
            else if (player.headPosY < 0 || player.headPosY >= map.map.GetLength(1))
            {
                isGameOn = false;
            }
        }

        private void PositionUpdate()
        {
            map.map[player.headPosX, player.headPosY] = Map.playerId;
            for (int i = 0; i < player.tails.Count; i++)
            {
                map.map[player.tails[i].posX, player.tails[i].posY] = Map.playerId;
            }
        }

        private void GameInitRender()
        {
            if(map.map != null)
            {
                //bg
                for (int x = 0; x < map.map.GetLength(0); x++)
                {
                    for(int y = 0; y < map.map.GetLength(1); y++)
                    {
                        Instantiate(backgrounGraphicalObject, new Vector3(x, y,0),Quaternion.identity);
                    }
                }

                //player head
                playerInstantObjects.Add(Instantiate(playerGraphicalObject, new Vector3(player.headPosX, player.headPosY, 1), Quaternion.identity) as GameObject);

                //item
                itemInstantObject = Instantiate(itemGraphicalObject, new Vector3(item.posX, item.posY, 1), Quaternion.identity) as GameObject;
            }
        }

        // position update and movement in game graphic.
        private void GameGraphicRender()
        {
            playerInstantObjects[0].transform.position = new Vector3(player.headPosX, player.headPosY, 1);
            for(int i = 1; i < playerInstantObjects.Count; i++)
            {
                playerInstantObjects[i].transform.position = new Vector3(player.tails[i-1].posX, player.tails[i-1].posY, 1);
            }

            itemInstantObject.transform.position = new Vector3(item.posX, item.posY, 1);
        }

    }
}
