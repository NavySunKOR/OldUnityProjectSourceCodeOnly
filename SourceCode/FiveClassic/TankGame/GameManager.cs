using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EightClassics.TankGame {

    struct Position
    {
        public int posX;
        public int posY;
    }


	public class GameManager : MonoBehaviour {
        //switchable
        //graphics
        public GameObject barrierObject;
        public GameObject playerOneObject;
        public GameObject playerTwoObject;
        public GameObject fireRateItemObject;
        public GameObject velocityItemObject;
        public GameObject projectileObject;
        //attributes.
        public int sizeX;
        public int sizeY;
        public float defaultProjectileVelocity;
        public float defaultRPM;


        //constants
        private const int NONE = 0;
        private const int PLAYER1 = 1;
        private const int PLAYER2 = 2;
        private const int BARRIER = 3;
        private const int FIRERATE_ITEM = 4;
        private const int VELOCITY_ITEM = 5;

        //stage
        [SerializeField]
        private int[,] map;


        //Graphics
        private GameObject[,] objectPool;

        private GameObject playerOneInstance;
        private GameObject playerTwoInstance;

        //player
        private Position pOnePos;
        private Position pTwoPos;
        private Position pOneDir;
        private Position pTwoDir;

        private float timer;
        private float timeInterval = 0.1f;
        private float pOneProjectileVelocity;
        private float pTwoProjectileVelocity;
        private float pOneRPM;
        private float pTwoRPM;
        private float pOneFireInterval;
        private float pTwoFireInterval;
        private float pOneFireTimer;
        private float pTwoFireTimer;


        private List<GameObject> playerOneProjectile;
        private List<Position> playerOneProjectileDir;
        private List<GameObject> playerTwoProjectile;
        private List<Position> playerTwoProjectileDir;


        private void Awake()
        {
            InitMap();
            InitPlayer();
            InitItem();
            InitGraphic();
        }


        private void InitMap()
        {
            map = new int[sizeX, sizeY];
            objectPool = new GameObject[sizeX, sizeY];

            //init with zero.
            for(int x = 0; x < sizeX; x++)
            {
                for(int y = 0; y < sizeY; y++)
                {
                    map[x, y] = NONE;
                }
            }

            //set barriers
            int barrierX = 0;
            for (int y = 0; y < sizeY; y++)
            {
                map[barrierX, y] = BARRIER;
            }
            barrierX = sizeX - 1;
            for (int y = 0; y < sizeY; y++)
            {
                map[barrierX, y] = BARRIER;
            }

            int barrierY = 0;
            for (int x = 0; x < sizeX; x++)
            {
                map[x, barrierY] = BARRIER;
            }

            barrierY = sizeY - 1;
            for (int x = 0; x < sizeX; x++)
            {
                map[x, barrierY] = BARRIER;
            }

            for (int x = 1; x < sizeX - 1; x++)
            {
                for (int y = 1; y < sizeY - 1; y++)
                {
                    if(map[x-1, y] != BARRIER 
                        && map[x + 1, y] != BARRIER
                        && map[x, y+1] != BARRIER
                        && map[x, y-1] != BARRIER)
                    {
                        int seed = Random.Range(1, 11);
                        if (seed % 4 == 0)
                        {
                            map[x, y] = BARRIER;
                        }
                    }
                }
            }
        }
        
        private void InitPlayer()
        {
            int x = Random.Range(1, sizeX - 1);
            int y = Random.Range(1, sizeY - 1);
            while (map[x,y] == NONE)
            {
                x = Random.Range(1, sizeX - 1);
                y = Random.Range(1, sizeY - 1);
            }

            map[x, y] = PLAYER1;

            x = Random.Range(1, sizeX - 1);
            y = Random.Range(1, sizeY - 1);

            while (map[x, y] == NONE)
            {
                x = Random.Range(1, sizeX - 1);
                y = Random.Range(1, sizeY - 1);
            }

            map[x, y] = PLAYER2;

            playerOneProjectile = new List<GameObject>();
            playerTwoProjectile = new List<GameObject>();
            playerOneProjectileDir = new List<Position>();
            playerTwoProjectileDir = new List<Position>();
            pOnePos = new Position();
            pTwoPos = new Position();
            pOneDir = new Position();
            pTwoDir = new Position();
            pOneProjectileVelocity = defaultProjectileVelocity;
            pTwoProjectileVelocity = defaultProjectileVelocity;
            pOneRPM = defaultRPM;
            pTwoRPM = defaultRPM;
            pOneFireInterval = 60f / pOneRPM;
            pTwoFireInterval = 60f / pTwoRPM;
            pOneFireTimer = 0f;
            pTwoFireTimer = 0f;

        }

        private void InitItem()
        {
            int x = Random.Range(1, sizeX - 1);
            int y = Random.Range(1, sizeY - 1);
            while (map[x, y] == NONE)
            {
                x = Random.Range(1, sizeX - 1);
                y = Random.Range(1, sizeY - 1);
            }

            map[x, y] = FIRERATE_ITEM;

            x = Random.Range(1, sizeX - 1);
            y = Random.Range(1, sizeY - 1);

            while (map[x, y] == NONE)
            {
                x = Random.Range(1, sizeX - 1);
                y = Random.Range(1, sizeY - 1);
            }

            map[x, y] = VELOCITY_ITEM;
        }


        private void InitGraphic()
        {
            for(int x = 0; x < sizeX; x++)
            {
                for(int y = 0; y < sizeY; y++)
                {
                    if(map[x,y] == BARRIER)
                        objectPool[x, y] = Instantiate(barrierObject, new Vector3(x, y, 0), Quaternion.identity);
                    else if(map[x, y] == PLAYER1)
                    {
                        objectPool[x, y] = Instantiate(playerOneObject, new Vector3(x, y, 0), Quaternion.identity);
                        playerOneInstance = objectPool[x, y];
                        pOnePos.posX = x;
                        pOnePos.posY = y;
                    }
                    else if (map[x, y] == PLAYER2)
                    {
                        objectPool[x, y] = Instantiate(playerTwoObject, new Vector3(x, y, 0), Quaternion.identity);
                        playerTwoInstance = objectPool[x, y];
                        pTwoPos.posX = x;
                        pTwoPos.posY = y;
                    }
                    else if (map[x, y] == FIRERATE_ITEM)
                        objectPool[x, y] = Instantiate(fireRateItemObject, new Vector3(x, y, 0), Quaternion.identity);
                    else if (map[x, y] == VELOCITY_ITEM)
                        objectPool[x, y] = Instantiate(velocityItemObject, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }

        private void Update()
        {
            SetPosition();
            //could use fixed update, but for project settings, this time used timer.
            if(Time.time - timer > timeInterval)
            {
                timer = Time.time;
                MovementCheck();
                
            }

            FireCheckPlayerOne();
            FireCheckPlayerTwo();
            ItemCheckPlayerOne();
            ItemCheckPlayerTwo();
        }

        private void SetPosition()
        {
            Vector3 pOne = playerOneInstance.transform.position;
            Vector3 pTwo = playerTwoInstance.transform.position;
            int pOneX = Mathf.RoundToInt(pOne.x);
            int pOneY = Mathf.RoundToInt(pOne.y);

            int pTwoX = Mathf.RoundToInt(pTwo.x);
            int pTwoY = Mathf.RoundToInt(pTwo.y);

            if (pOneX != pOnePos.posX || pOneY != pOnePos.posY)
            {
                map[pOneX, pOneY] = PLAYER1;
                map[pOnePos.posX, pOnePos.posY] = NONE;
                objectPool[pOneX, pOneY] = playerOneInstance;
                objectPool[pOnePos.posX, pOnePos.posY] = null;
                pOnePos.posX = pOneX;
                pOnePos.posY = pOneY;


            }

            if (pTwoX != pTwoPos.posX || pTwoY != pTwoPos.posY)
            {
                map[pTwoX, pTwoY] = PLAYER2;
                map[pTwoPos.posX, pTwoPos.posY] = NONE;
                objectPool[pTwoX, pTwoY] = playerTwoInstance;
                objectPool[pTwoPos.posX, pTwoPos.posY] = null;
                pTwoPos.posX = pTwoX;
                pTwoPos.posY = pTwoY;
            }

        }

        private void MovementCheck()
        {
            //Player1
            if (Input.GetKey(KeyCode.W))
            {
                pOneDir.posX = 0;
                pOneDir.posY = 1;
                if (pOnePos.posY < sizeY - 1 && !IsCollide(KeyCode.W))
                {
                    playerOneInstance.transform.position += new Vector3(0, 1f, 0);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                pOneDir.posX = 0;
                pOneDir.posY = -1;
                if (pOnePos.posY > 0 && !IsCollide(KeyCode.S))
                {
                    playerOneInstance.transform.position -= new Vector3(0, 1f, 0);
                }
            }
            else if(Input.GetKey(KeyCode.A))
            {
                pOneDir.posX = -1;
                pOneDir.posY = 0;
                if (pOnePos.posX > 0 && !IsCollide(KeyCode.A))
                {
                    playerOneInstance.transform.position -= new Vector3(1f, 0, 0);
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                pOneDir.posX = 1;
                pOneDir.posY = 0;
                if (pOnePos.posX < sizeX - 1 && !IsCollide(KeyCode.D))
                {
                    playerOneInstance.transform.position += new Vector3(1f, 0, 0);
                }
            }

            //Player2
            if (Input.GetKey(KeyCode.UpArrow))
            {
                pTwoDir.posX = 0;
                pTwoDir.posY = 1;
                if (pTwoPos.posY < sizeY - 1 && !IsCollide(KeyCode.UpArrow))
                {
                    playerTwoInstance.transform.position += new Vector3(0, 1f, 0);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                pTwoDir.posX = 0;
                pTwoDir.posY = -1;
                if (pTwoPos.posY > 0 && !IsCollide(KeyCode.DownArrow))
                {
                    playerTwoInstance.transform.position -= new Vector3(0, 1f, 0);
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                pTwoDir.posX = -1;
                pTwoDir.posY = 0;
                if (pTwoPos.posX > 0 && !IsCollide(KeyCode.LeftArrow))
                {
                    playerTwoInstance.transform.position -= new Vector3(1f, 0, 0);
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                pTwoDir.posX = 1;
                pTwoDir.posY = 0;
                if (pTwoPos.posX < sizeX - 1 && !IsCollide(KeyCode.RightArrow))
                {
                    playerTwoInstance.transform.position += new Vector3(1f, 0, 0);
                }
            }
        }

        private bool IsCollide(KeyCode keyCode)
        {
            switch (keyCode)
            {
                //PlayerOne
                case KeyCode.W:
                    if (map[pOnePos.posX, pOnePos.posY + 1] == BARRIER || map[pOnePos.posX, pOnePos.posY + 1] == PLAYER2)
                        return true;
                    else
                        return false;
                case KeyCode.S:
                    if (map[pOnePos.posX, pOnePos.posY - 1] == BARRIER || map[pOnePos.posX, pOnePos.posY - 1] == PLAYER2)
                        return true;
                    else
                        return false;
                case KeyCode.A:
                    if (map[pOnePos.posX-1, pOnePos.posY] == BARRIER || map[pOnePos.posX-1, pOnePos.posY] == PLAYER2)
                        return true;
                    else
                        return false;
                case KeyCode.D:
                    if (map[pOnePos.posX + 1, pOnePos.posY] == BARRIER || map[pOnePos.posX + 1, pOnePos.posY] == PLAYER2)
                        return true;
                    else
                        return false;
                //PlayerTwo
                case KeyCode.UpArrow:
                    if (map[pTwoPos.posX, pTwoPos.posY + 1] == BARRIER || map[pTwoPos.posX, pTwoPos.posY + 1] == PLAYER1)
                        return true;
                    else
                        return false;

                case KeyCode.DownArrow:
                    if (map[pTwoPos.posX, pTwoPos.posY - 1] == BARRIER || map[pTwoPos.posX, pTwoPos.posY - 1] == PLAYER1)
                        return true;
                    else
                        return false;
                case KeyCode.LeftArrow:
                    if (map[pTwoPos.posX - 1, pTwoPos.posY] == BARRIER || map[pTwoPos.posX - 1, pTwoPos.posY] == PLAYER1)
                        return true;
                    else
                        return false;
                case KeyCode.RightArrow:
                    if (map[pTwoPos.posX + 1, pTwoPos.posY] == BARRIER || map[pTwoPos.posX + 1, pTwoPos.posY] == PLAYER1)
                        return true;
                    else
                        return false;
                default: return false;
            }

        }

        private void FireCheckPlayerOne()
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time - pOneFireTimer > pOneFireInterval)
            {
                playerOneProjectile.Add(Instantiate(projectileObject, new Vector3(pOnePos.posX + pOneDir.posX, pOnePos.posY + pOneDir.posY, 0), Quaternion.identity));
                playerOneProjectileDir.Add(pOneDir);
            }

            if(playerOneProjectile.Count > 0)
            {
                for(int i = 0; i < playerOneProjectile.Count; i++)
                {
                    //block or player hit detection.
                    int projectileX = Mathf.RoundToInt(playerOneProjectile[i].transform.position.x);
                    int projectileY = Mathf.RoundToInt(playerOneProjectile[i].transform.position.y);

                    if (projectileX < 0 || projectileX > sizeX - 1 || projectileY < 0 || projectileY > sizeY -1)
                    {
                        Destroy(playerOneProjectile[i]);
                        playerOneProjectile.Remove(playerOneProjectile[i]);
                        playerOneProjectileDir.Remove(playerOneProjectileDir[i]);
                        continue;
                    }
                    else if (map[projectileX, projectileY] == PLAYER2)
                    {
                        playerTwoInstance.SetActive(false);
                        Destroy(playerOneProjectile[i]);
                        playerOneProjectile.Remove(playerOneProjectile[i]);
                        playerOneProjectileDir.Remove(playerOneProjectileDir[i]);
                        continue;
                        //player one win.
                    }
                    else if (map[projectileX, projectileY] == BARRIER)
                    {
                        map[projectileX, projectileY] = NONE;
                        Destroy(objectPool[projectileX, projectileY]);
                        objectPool[projectileX, projectileY] = null;
                        Destroy(playerOneProjectile[i]);
                        playerOneProjectile.Remove(playerOneProjectile[i]);
                        playerOneProjectileDir.Remove(playerOneProjectileDir[i]);
                        continue;
                    }

                    //addForce
                    playerOneProjectile[i].transform.position += new Vector3(playerOneProjectileDir[i].posX * pOneProjectileVelocity * Time.deltaTime , playerOneProjectileDir[i].posY * pOneProjectileVelocity * Time.deltaTime, 0);
                }
            }
        }

        private void FireCheckPlayerTwo()
        {
            if (Input.GetKeyDown(KeyCode.Return) && Time.time - pTwoFireTimer > pTwoFireInterval)
            {
                playerTwoProjectile.Add(Instantiate(projectileObject, new Vector3(pTwoPos.posX + pTwoDir.posX, pTwoPos.posY + pTwoDir.posY, 0), Quaternion.identity));
                playerTwoProjectileDir.Add(pTwoDir);
            }

            if (playerTwoProjectile.Count > 0)
            {
                for (int i = 0; i < playerTwoProjectile.Count; i++)
                {
                    //block or player hit detection.
                    int projectileX = Mathf.RoundToInt(playerTwoProjectile[i].transform.position.x);
                    int projectileY = Mathf.RoundToInt(playerTwoProjectile[i].transform.position.y);

                    if (projectileX < 0 || projectileX > sizeX - 1 || projectileY < 0 || projectileY > sizeY - 1)
                    {
                        Destroy(playerTwoProjectile[i]);
                        playerTwoProjectile.Remove(playerTwoProjectile[i]);
                        playerTwoProjectileDir.Remove(playerTwoProjectileDir[i]);
                        continue;
                    }
                    else if (map[projectileX, projectileY] == PLAYER1)
                    {
                        playerOneInstance.SetActive(false);
                        Destroy(playerTwoProjectile[i]);
                        playerTwoProjectile.Remove(playerTwoProjectile[i]);
                        playerTwoProjectileDir.Remove(playerTwoProjectileDir[i]);
                        continue;
                        //player two win.
                    }
                    else if (map[projectileX, projectileY] == BARRIER)
                    {
                        map[projectileX, projectileY] = NONE;
                        Destroy(objectPool[projectileX, projectileY]);
                        objectPool[projectileX, projectileY] = null;
                        Destroy(playerTwoProjectile[i]);
                        playerTwoProjectile.Remove(playerTwoProjectile[i]);
                        playerTwoProjectileDir.Remove(playerTwoProjectileDir[i]);
                        continue;
                    }

                    //addForce
                    playerTwoProjectile[i].transform.position += new Vector3(playerTwoProjectileDir[i].posX * pTwoProjectileVelocity * Time.deltaTime, playerTwoProjectileDir[i].posY * pTwoProjectileVelocity * Time.deltaTime, 0);
                }
            }
        }

        private void ItemCheckPlayerOne()
        {
            if (map[pOnePos.posX + pOneDir.posX,pOnePos.posY + pOneDir.posY] == FIRERATE_ITEM)
            {
                pOneRPM += 20f;
                pOneFireInterval = 60f / pOneRPM;
                map[pOnePos.posX + pOneDir.posX, pOnePos.posY + pOneDir.posY] = NONE;
                Destroy(objectPool[pOnePos.posX + pOneDir.posX, pOnePos.posY + pOneDir.posY]);
                objectPool[pOnePos.posX + pOneDir.posX, pOnePos.posY + pOneDir.posY] = null;
            }
            else if(map[pOnePos.posX + pOneDir.posX, pOnePos.posY + pOneDir.posY] == VELOCITY_ITEM)
            {
                pOneProjectileVelocity += 5f;
                map[pOnePos.posX + pOneDir.posX, pOnePos.posY + pOneDir.posY] = NONE;
                Destroy(objectPool[pOnePos.posX + pOneDir.posX, pOnePos.posY + pOneDir.posY]);
                objectPool[pOnePos.posX + pOneDir.posX, pOnePos.posY + pOneDir.posY] = null;
            }
        }

        private void ItemCheckPlayerTwo()
        {
            if (map[pTwoPos.posX + pTwoDir.posX, pTwoPos.posY +pTwoDir.posY] == FIRERATE_ITEM)
            {
                pTwoRPM += 20f;
                pTwoFireInterval = 60f / pTwoRPM;
                map[pTwoPos.posX + pTwoDir.posX, pTwoPos.posY + pTwoDir.posY] = NONE;
                Destroy(objectPool[pTwoPos.posX + pTwoDir.posX, pTwoPos.posY + pTwoDir.posY]);
                objectPool[pTwoPos.posX + pTwoDir.posX, pTwoPos.posY + pTwoDir.posY] = null;
            }
            else if (map[pTwoPos.posX + pTwoDir.posX, pTwoPos.posY + pTwoDir.posY] == VELOCITY_ITEM)
            {
                pTwoProjectileVelocity += 5f;
                map[pTwoPos.posX + pTwoDir.posX, pTwoPos.posY + pTwoDir.posY] = NONE;
                Destroy(objectPool[pTwoPos.posX + pTwoDir.posX, pTwoPos.posY + pTwoDir.posY]);
                objectPool[pTwoPos.posX + pTwoDir.posX, pTwoPos.posY + pTwoDir.posY] = null;
            }
        }
    }
}
