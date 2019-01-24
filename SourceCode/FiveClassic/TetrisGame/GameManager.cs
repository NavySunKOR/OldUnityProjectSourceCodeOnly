using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EightClassics.TetrisGame;

namespace EightClassics.TetrisGame
{
	public class GameManager : MonoBehaviour {

        public GameObject graphicalObject;
        public GameObject blockObject;
        public GameObject remainObject;
        public Transform backgroundParent;
        public Transform remainParent;
        public int mapSizeX;
        public int mapSizeY;
        public float gameTimerInterval;

        private float timer;
        [SerializeField]
        private Block currentBlock;
        private Block nextBlock; // if current is finished....
        private Position blockFallStartPos;

        private List<GameObject> blockPool;
        private Map map;




        private void Awake()
        {
            //Spawn, GameInitRender,MapSetting....
            blockPool = new List<GameObject>();
            blockFallStartPos = new Position(Mathf.RoundToInt(mapSizeX / 2), 1);
            map = new Map(mapSizeX, mapSizeY, blockFallStartPos);
            currentBlock = new Block(blockFallStartPos);
            nextBlock = new Block(blockFallStartPos);
            int selector = Mathf.RoundToInt(Random.Range(0, 7));
            currentBlock.SetBlock((BlockType)selector);
            selector = Mathf.RoundToInt(Random.Range(0, 7));
            nextBlock.SetBlock((BlockType)selector);
            GameInitRender();
        }

        private void Update()
        {
            InputLoop();
            GamePositionUpdate();
            GameLoop();
            RenderUpdate();
        }

        private void InputLoop()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentBlock.MovePosition(1,0);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentBlock.MovePosition(-1,0);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentBlock.MovePosition(0,1);
            }
            //Get Input and block horizontal movement.
        }

        private void GamePositionUpdate()
        {
            for(int i = 0; i < map.mapGrid.GetLength(0); i++)
            {
                for(int j=0; j<map.mapGrid.GetLength(1); j++)
                {
                    if(map.mapGrid[i, j] != 2)
                    {
                        map.mapGrid[i, j] = 0;
                    }
                }
            }    
            foreach (Position pos in currentBlock.blocks)
            {
                map.mapGrid[pos.posX, pos.posY] = 1;
            }
        }

        private void GameLoop()
        {
            if(Time.time - timer > gameTimerInterval)
            {
                timer = Time.time;
                //Block Vertical Movement in period(We could use FixedUpdate, but in this time, time check system will be used for any environment.
                //Check blocks under the existence.
                bool canMove = true;
                foreach(Position pos in currentBlock.blocks)
                {
                    if(pos.posY + 1 >= mapSizeY)
                    {
                        canMove = false;
                        //switch to nextBlock.
                        foreach (Position position in currentBlock.blocks)
                        {
                            map.mapGrid[position.posX, position.posY] = 2;
                        }
                        NewRenderRemaining(currentBlock.blocks);
                        currentBlock = nextBlock;
                        int selector = Mathf.RoundToInt(Random.Range(0, 7));
                        nextBlock.SetBlock((BlockType)selector);
                    }
                    else if (map.mapGrid[pos.posX, pos.posY + 1] == 2)
                    {
                        canMove = false;
                        //switch to nextBlock.
                        foreach (Position position in currentBlock.blocks)
                        {
                            map.mapGrid[position.posX,position.posY] = 2 ;
                        }
                        NewRenderRemaining(currentBlock.blocks);
                        currentBlock = nextBlock;
                        int selector = Mathf.RoundToInt(Random.Range(0, 7));
                        nextBlock.SetBlock((BlockType)selector);
                    }
                }

                if (canMove)
                {
                    foreach (Position position in currentBlock.blocks)
                    {
                        position.posY += 1;
                    }
                }

                //check lines 
                CheckLineStates();
            }
        }

        private void CheckLineStates()
        {
            for (int y = 0; y < map.mapGrid.GetLength(1); y++)
            {
                int filed = 0;
                for (int x = 0; x < map.mapGrid.GetLength(0); x++)
                {
                    if (map.mapGrid[x, y] == 2)
                    {
                        filed += 1;
                    }
                }

                if (filed == map.mapGrid.GetLength(0))
                {
                    DeleteLine(y);
                }
            }
        }

        private void DeleteLine(int yPos)
        {
            for(int y = yPos; y > 0; y--)
            {
                for (int x = 0; x < map.mapGrid.GetLength(0); x++)
                {
                    map.mapGrid[x, y] = map.mapGrid[x, y - 1];
                    RefreshRemainRender();
                }
            }
        }

        private void RefreshRemainRender()
        {
            //나중 훗날에 오래 걸리는 현상을 제거할것.
            for (int i = 0; i < remainParent.transform.childCount; i++)
            {
                GameObject child = remainParent.transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(false);
            }

            for (int x = 0; x < map.mapGrid.GetLength(0); x++)
            {
                for(int y = 0; y < map.mapGrid.GetLength(1); y++)
                {
                    if(map.mapGrid[x,y] == 2)
                    {
                        GameObject go = Instantiate(remainObject, new Vector3(x, -y, 1), Quaternion.identity);
                        go.transform.SetParent(remainParent);
                    }
                }
            }
        }

        private void RenderUpdate()
        {
            //Block movement realtime render.
            for(int i = 0; i < currentBlock.blocks.Length; i++)
            {
                blockPool[i].transform.position = new Vector3(currentBlock.blocks[i].posX, -currentBlock.blocks[i].posY, 1);
            }
        }

        private void NewRenderRemaining(Position[] positions)
        {
            foreach(Position pos in positions)
            {
                GameObject go =  Instantiate(remainObject, new Vector3(pos.posX, -pos.posY, 1), Quaternion.identity);
                go.transform.SetParent(remainParent);
            }
        }

        /*
        private void NewRenderRemaining(List<Position> positions)
        {
            foreach (Position pos in positions)
            {
                GameObject go = Instantiate(remainObject, new Vector3(pos.posX, -pos.posY, 1), Quaternion.identity);
                go.transform.SetParent(remainParent);
            }
        }

        private void NewSpecificRenderRemaining(int x, int y)
        {
            GameObject go = Instantiate(remainObject, new Vector3(x, -y, 1), Quaternion.identity);
            go.transform.SetParent(remainParent);
        }
        */

        /*
        LEGACY : in case for use again.....
        private void RemoveRenderRemaining(int y) 
        {
            //destroy olds.
            for (int x = 0; x < map.mapGrid.GetLength(0); x++)
            {
                // 나중에 인스턴스 체크를 해보자.
               Position pos = remainPoolKeyHolder[x, y];
               GameObject obj = remainPool[pos];
               remainPoolKeyHolder[x, y] = null;
               remainPool.Remove(pos);
               Destroy(obj);
            }
        }

        private void RemoveRenderRemaining(List<Position> positions)
        {
            //destroy olds.
            foreach (Position po in positions)
            {
                // 나중에 인스턴스 체크를 해보자.
                Position pos = remainPoolKeyHolder[po.posX, po.posY];
                GameObject obj = remainPool[pos];
                remainPoolKeyHolder[po.posX, po.posY] = null;
                remainPool.Remove(pos);
                Destroy(obj);
            }
        }

        private void RemoveSpecificRenderRemaining(int x, int y)
        {
            Position pos = remainPoolKeyHolder[x, y];
            GameObject obj = remainPool[pos];
            remainPoolKeyHolder[x, y] = null;
            remainPool.Remove(pos);
            Destroy(obj);
        }
        */

        private void GameInitRender()
        {
            //background graphic

            //block graphic
            for (int i = 0; i < currentBlock.blocks.Length; i++)
            {
                blockPool.Add(Instantiate(blockObject, new Vector3(currentBlock.blocks[i].posX, -currentBlock.blocks[i].posY, 1), Quaternion.identity));
            }
            for(int i =0; i <map.mapGrid.GetLength(0); i++)
            {
                for(int j=0; j<map.mapGrid.GetLength(1); j++)
                {
                    //Position y is must be inversed....
                    GameObject obj = Instantiate(graphicalObject, new Vector3(i, -j, 2), Quaternion.identity);
                    obj.transform.SetParent(backgroundParent);
                }
            }
        }

	}
}
