using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EightClassics.ArkanoidGame {
    struct Position
    {
       public float posX;
       public float posY;
    }
	public class GameManager : MonoBehaviour {

        public GameObject blockObject;
        public GameObject barObject;
        public GameObject ballObject;

        public float sizeX;
        public float sizeY;
        public int blockCount;

        public float barMovementSpeed;


        private bool gameStart;

        //block field
        private GameObject[] blockInstances;

        //ball field
        private GameObject ballInstance;
        private Position ballDir;

        //bar field
        private GameObject barInstance;

        private void Awake()
        {
            blockInstances = new GameObject[blockCount];
            ballDir = new Position();
            float startPosX = sizeX * 0.1f;
            float startPosY = sizeY * 0.6f;

            int colPerRow = Mathf.RoundToInt((sizeX * 0.8f) / 2.5f);

            int count = 0;
            for(int i = 0; i < blockInstances.Length; i++)
            {
                blockInstances[i] = Instantiate(blockObject, new Vector3(startPosX, startPosY, 0), Quaternion.identity);
                count++;
                startPosX += 2.5f;
                //index - 1
                if(i != 0 && count % colPerRow == 0)
                {
                    startPosY += 2.5f;
                    startPosX = sizeX * 0.1f;
                }
            }

            barInstance = Instantiate(barObject, new Vector3(sizeX / 2, sizeY * 0.05f, 0), Quaternion.identity);
            ballInstance = Instantiate(ballObject, new Vector3(sizeX / 2, (sizeY * 0.05f) + 0.5f, 0), Quaternion.identity);


        }

        private void Update()
        {

            CheckInputs();
            BallMovement();


        }

        private void CheckInputs()
        {
            if (!gameStart && Input.GetKeyDown(KeyCode.Space))
            {
                ballDir.posX = 0.5f;
                ballDir.posY = 0.5f;
                gameStart = true;
            }

            if (gameStart)
            {
                if (Input.GetKey(KeyCode.RightArrow) && IsBarInside())
                {
                    barInstance.transform.position += new Vector3(barMovementSpeed * Time.deltaTime, 0, 0);
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && IsBarInside())
                {
                    barInstance.transform.position -= new Vector3(barMovementSpeed * Time.deltaTime, 0, 0);
                }
            }
        }

        private void TurnXDir()
        {
            //someday, add angle dirs...
            ballDir.posX = -ballDir.posX;
        }

        private void TurnYDir()
        {
            ballDir.posY = -ballDir.posY;
        }

        private bool IsBarInside()
        {
            if (barInstance.transform.position.x >= 1f && barInstance.transform.position.x <= sizeX - 1f)
            {
                return true;
            }
            else
            {
                if(barInstance.transform.position.x < 1f)
                {
                    barInstance.transform.position += new Vector3(1f, 0, 0);
                }
                else
                {
                    barInstance.transform.position -= new Vector3(1f, 0, 0);
                }
            }
            return true;
        }

        private void BallMovement()
        {
            ballInstance.transform.position += new Vector3(ballDir.posX, ballDir.posY, 0);
            if (ballInstance.transform.position.x <= 0.5f || ballInstance.transform.position.x >= sizeX - 0.5f)
            {
                TurnXDir();
            }

            if (ballInstance.transform.position.y >= sizeY - 0.5f || ballInstance.transform.position.y <= 0.5f)
            {
                TurnYDir();
            }

            //Colide Check
            IsColideWithObject();
        }

        private void IsColideWithObject()
        {
            //is colide with bar
            Vector3 ballPos = ballInstance.transform.position;
            Vector3 barPos = barInstance.transform.position;

            //or you could ballPos - barPos, add x,y , squareRoot. bar's haf length is 1 and balls
            if (Vector3.Distance(ballPos, barPos) < 1.11f && ballPos.y > barPos.y+0.25f)
            {
                if(ballPos.x == barPos.x)
                {
                    TurnYDir();
                }
                else
                {
                    TurnXDir();
                    TurnYDir();
                }
                return;
            }
            else
            {
                //is colide with blocks?
                for(int i = 0; i < blockInstances.Length; i++)
                {
                    if(blockInstances[i] != null)
                    {
                        Vector3 blockPos = blockInstances[i].transform.position;
                        if(Vector3.Distance(ballPos, blockPos) < 1.11f)
                        {
                            Destroy(blockInstances[i]);
                            blockInstances[i] = null;
                            TurnXDir();
                            TurnYDir();
                            return;
                        }
                    }
                }
            }
        }

        



    }
}
