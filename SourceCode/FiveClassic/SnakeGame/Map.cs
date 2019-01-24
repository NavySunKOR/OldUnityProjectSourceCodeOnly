using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EightClassics.SnakeGame
{
    [System.Serializable]
	public class Map {
        public static int playerId = 2;
        public static int itemId = 3;
        public int[,] map;

        private int sizeX;
        private int sizeY;

        public void SetMap(int x, int y)
        {
            sizeX = x;
            sizeY = y;
            map = new int[sizeX, sizeY];
        }

    }
}
