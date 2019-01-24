using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EightClassics.PongGame
{
	public class GameManager : MonoBehaviour {

        //map
        public GameObject playerObject;
        public GameObject ballObject;
        public float fieldSizeX;
        public float fieldSizeY;
        public float moveSpeed;
        public int playerLength;
        public float ballSpeed;



        //players
        //0 : Player1 , 1 : Player2
        private int oppositeTurn = 0;
        private GameObject[] playerOneGraphic;
        private GameObject[] playerTwoGraphic;

        //upper : last index of , under : first index of
        private Vector3 playerOneUpperPos;
        private Vector3 playerOneUnderPos;

        private Vector3 playerTwoUpperPos;
        private Vector3 playerTwoUnderPos;
        

        private float playerOneStartPosX;
        private float playerOneStartPosY;

        private float playerTwoStartPosX;
        private float playerTwoStartPosY;

        //ball
        private GameObject ballInstance;
        private float ballDirX;
        private float ballDirY;
        private float ballPosX;
        private float ballPosY;
        private float ballIngameSpeed;



        private void Awake()
        {
            InitProp();
            InitRender();
            UpdateLastPosOfPlayerOne();
            UpdateLastPosOfPlayerTwo();

        }

        private void Update()
        {
            InputCheckFirstPlayer();
            InputCheckSecondPlayer();
            PositionReminder();
            UpdateLastPosOfPlayerOne();
            UpdateLastPosOfPlayerTwo();
            BallMovement();
            BallDetection();

        }

        private void InputCheckFirstPlayer()
        {
             if (Input.GetKey(KeyCode.W))
             {
                foreach (GameObject prop in playerOneGraphic)
                {
                    prop.transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                }
             }
             else if (Input.GetKey(KeyCode.S))
             {
                  foreach (GameObject prop in playerOneGraphic)
                  {
                      prop.transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
                  }
             }
        }
        private void InputCheckSecondPlayer()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                 foreach (GameObject prop in playerTwoGraphic)
                 {
                    prop.transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                 }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                 foreach (GameObject prop in playerTwoGraphic)
                 {
                     prop.transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
                 }
            }
        }

        private void InitProp()
        {
            playerOneGraphic = new GameObject[playerLength];
            playerTwoGraphic = new GameObject[playerLength];

            playerOneStartPosX = 1f;
            playerTwoStartPosX = fieldSizeX - 1f;

            playerOneStartPosY = fieldSizeY / 2f;
            playerTwoStartPosY = fieldSizeY / 2f;

            ballPosX = fieldSizeX / 2f;
            ballPosY = fieldSizeY / 2f;

            ballDirX = (Random.Range(-1f, 1f) > 0) ? 1f : -1f;
            ballDirY = Random.Range(-1f, 1.1f);
            ballIngameSpeed = ballSpeed; ;

        }

        private void BallSet()
        {
            ballPosX = fieldSizeX / 2f;
            ballPosY = fieldSizeY / 2f;
            ballDirX = (Random.Range(-1f,1f) > 0)? 1f : -1f ;
            ballDirY = Random.Range(-1f, 1.1f);
            ballIngameSpeed = ballSpeed;

            ballInstance.transform.position = new Vector2(ballPosX, ballPosY);
        }

        private void InitRender()
        {
            // players spawn

            for (int i = 0; i < playerLength; i++)
            {
                playerOneGraphic[i] = Instantiate(playerObject, new Vector2(playerOneStartPosX, playerOneStartPosY + i), Quaternion.identity);
                playerTwoGraphic[i] = Instantiate(playerObject, new Vector2(playerTwoStartPosX, playerTwoStartPosY + i), Quaternion.identity);
            }


            //ball spawn
            ballInstance = Instantiate(ballObject, new Vector2(ballPosX, ballPosY), Quaternion.identity);
        }

        private void UpdateLastPosOfPlayerOne()
        {
            playerOneUpperPos = playerOneGraphic[playerOneGraphic.Length - 1].transform.position;
            playerOneUnderPos = playerOneGraphic[0].transform.position;
        }

        private void UpdateLastPosOfPlayerTwo()
        {
            playerTwoUpperPos = playerTwoGraphic[playerTwoGraphic.Length - 1].transform.position;
            playerTwoUnderPos = playerTwoGraphic[0].transform.position;
        }

        private void PositionReminder()
        {
            if (playerOneUnderPos.y < 0 || playerOneUpperPos.y > fieldSizeY)
            {
                if(playerOneUnderPos.y < 0)
                {
                    foreach (GameObject prop in playerOneGraphic)
                    {
                        prop.transform.position += new Vector3(0, 1f, 0);
                    }
                }
                else
                {
                    foreach (GameObject prop in playerOneGraphic)
                    {
                        prop.transform.position -= new Vector3(0, 1f, 0);
                    }
                }
            }
            
            if (playerTwoUnderPos.y < 0 || playerTwoUpperPos.y > fieldSizeY)
            {
                if(playerTwoUnderPos.y < 0)
                {
                    foreach (GameObject prop in playerTwoGraphic)
                    {
                        prop.transform.position += new Vector3(0, 1f, 0);
                    }
                }
                else
                {
                    foreach (GameObject prop in playerTwoGraphic)
                    {
                        prop.transform.position -= new Vector3(0, 1f, 0);
                    }
                }
            }
        }

        private void BallMovement()
        {
            ballInstance.transform.position += new Vector3(ballDirX * ballIngameSpeed * Time.deltaTime, ballDirY * ballIngameSpeed * Time.deltaTime, 0);
        }

        private void BallDetection()
        {
            //player two bounce check
            if (ballInstance.transform.position.x > playerTwoUnderPos.x - 1f
                && ballInstance.transform.position.y > playerTwoUnderPos.y
                && ballInstance.transform.position.y < playerTwoUpperPos.y)
            {
                ballDirX *= -1f;
                ballDirY *= (ballInstance.transform.position.y - ((playerTwoUnderPos.y + playerTwoUpperPos.y) / 2) > 0f)? -1f : 1f;
                ballIngameSpeed += 1f;
                BallMovement();
            }
            //player one bounce check;
            else if (ballInstance.transform.position.x < playerOneUnderPos.x + 1f
                && ballInstance.transform.position.y > playerOneUnderPos.y
                && ballInstance.transform.position.y < playerOneUpperPos.y)
            {
                ballDirX *= -1f;
                ballDirY *= (ballInstance.transform.position.y - ((playerOneUnderPos.y + playerOneUpperPos.y) / 2) > 0f) ? -1f : 1f;
                ballIngameSpeed += 1f;
                BallMovement();
            }
            //upper boundary
            else if (ballInstance.transform.position.y < fieldSizeY 
                && ballInstance.transform.position.y > fieldSizeY - 1f)
            {
                ballDirY *= -1f;
                ballIngameSpeed += 2f;
                BallMovement();
            }
            //under boundary
            else if (ballInstance.transform.position.y > 0 
                && ballInstance.transform.position.y < 1f)
            {
                ballDirY *= -1f;
                ballIngameSpeed += 2f;
                BallMovement();
            }


            //Gets score
            //player2 win.입사각
            if(ballInstance.transform.position.x < 0)
            {
                BallSet();
            }
            //player1 win.
            else if(ballInstance.transform.position.x > fieldSizeX)
            {
                BallSet();
            }


        }



    }
}
