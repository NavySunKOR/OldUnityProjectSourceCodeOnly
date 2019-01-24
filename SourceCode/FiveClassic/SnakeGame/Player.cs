using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EightClassics.SnakeGame
{
    [System.Serializable]
    public class PlayerTail
    {
        public int posX;
        public int posY;
    }
    [System.Serializable]
    public class Player{

        public int headPosX;
        public int headPosY;
        public int moveAxisX = 1;
        public int moveAxisY;
        public List<PlayerTail> tails;
        private int score = 0;
      

        public void InitReferences(int hX,int hY)
        {
            headPosX = hX;
            headPosY = hY;
            tails = new List<PlayerTail>();
        }

        public void AddScore(int sco)
        {
            AddLength();
            score += sco;
        }

        public void AddLength()
        {
            PlayerTail tail = new PlayerTail();
            tail.posX = headPosX;
            tail.posY = headPosY;
            tails.Add(tail);
        }
        
        public void MoveHead(int x, int y)
        {
            headPosX = x;
            headPosY = y;
        }

        public int GetScore()
        {
            return score;
        }

        // moveRest of tails
        
    }
}
