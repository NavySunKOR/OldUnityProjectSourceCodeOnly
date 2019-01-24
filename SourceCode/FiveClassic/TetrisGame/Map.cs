using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EightClassics.TetrisGame
{
	public class Map{
        //0 - 30
        public int[,] mapGrid;
        public Position blockFallStartPos;

        public Map(int sizeX, int sizeY, Position blockStartPos)
        {
            mapGrid = new int[sizeX, sizeY];
            blockFallStartPos = blockStartPos;

            for(int i = 0; i < mapGrid.GetLength(0); i++)
            {
                for(int j = 0; j < mapGrid.GetLength(1); j++)
                {
                    mapGrid[i, j] = 0;
                }
            }
        }
    }
}
