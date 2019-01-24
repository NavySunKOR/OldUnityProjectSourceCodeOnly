using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EightClassics.TetrisGame {
    public enum BlockType
    {
        Straight = 0 , // ㅡ 자
        LType, //L자
        LTypeR,
        Curved, //ㄹ자
        CurvedR,
        Square, // 사각형
        TType, // T 형

    }

    [System.Serializable]
    public class Position
    {
        public int posX;
        public int posY;

        public Position()
        {
            posX = 0;
            posY = 0;
        }

        public Position(int x, int y)
        {
            posX = x;
            posY = y;
        }
    }
    [System.Serializable]
    public class Block {

        public Position[] blocks;
        //left edge of all blocks.
        public Position position;
        private BlockType blockType;

        public Block(Position startWith)
        {
            //블록의 개수는 4개
            blocks = new Position[4];
            position = startWith;
        }

        public void SetBlock(BlockType type)
        {
            switch (type)
            {
                case BlockType.Straight: CreateStraight(); blockType = type;  break;
                case BlockType.LType: CreateLType(); blockType = type; break;
                case BlockType.LTypeR: CreateLTypeR(); blockType = type; break;
                case BlockType.Curved: CreateCurved(); blockType = type; break;
                case BlockType.CurvedR: CreateCurvedR(); blockType = type; break;
                case BlockType.Square: CreateSquare(); blockType = type; break;
                case BlockType.TType: CreateTType(); blockType = type; break;
                default: break;
            }
        }

        private void CreateStraight()
        {
            blocks[0] = new Position(position.posX, position.posY);
            blocks[1] = new Position(position.posX+1, position.posY);
            blocks[2] = new Position(position.posX+2, position.posY);
            blocks[3] = new Position(position.posX+3, position.posY);
        }

        private void CreateLType()
        {
            blocks[0] = new Position(position.posX, position.posY);
            blocks[1] = new Position(position.posX + 1, position.posY);
            blocks[2] = new Position(position.posX, position.posY + 1);
            blocks[3] = new Position(position.posX, position.posY + 2);
        }

        private void CreateLTypeR()
        {
            blocks[0] = new Position(position.posX, position.posY);
            blocks[1] = new Position(position.posX + 1, position.posY);
            blocks[2] = new Position(position.posX + 1, position.posY + 1);
            blocks[3] = new Position(position.posX + 1, position.posY + 2);
        }

        private void CreateCurved()
        {
            blocks[0] = new Position(position.posX , position.posY + 1);
            blocks[1] = new Position(position.posX + 1, position.posY + 1);
            blocks[2] = new Position(position.posX + 1, position.posY);
            blocks[3] = new Position(position.posX + 2, position.posY);
        }

        private void CreateCurvedR()
        {
            blocks[0] = new Position(position.posX, position.posY);
            blocks[1] = new Position(position.posX + 1, position.posY);
            blocks[2] = new Position(position.posX + 1, position.posY + 1);
            blocks[3] = new Position(position.posX + 2, position.posY + 1);
        }

        private void CreateSquare()
        {
            blocks[0] = new Position(position.posX, position.posY);
            blocks[1] = new Position(position.posX + 1, position.posY);
            blocks[2] = new Position(position.posX, position.posY + 1);
            blocks[3] = new Position(position.posX + 1, position.posY + 1);
        }

        private void CreateTType()
        {
            blocks[0] = new Position(position.posX, position.posY);
            blocks[1] = new Position(position.posX + 1, position.posY);
            blocks[2] = new Position(position.posX + 2, position.posY);
            blocks[3] = new Position(position.posX + 1, position.posY + 1);
        }

        public void MovePosition(int x , int y)
        {
            for(int i= 0; i < blocks.Length; i++)
            {
                blocks[i].posX += x;
                blocks[i].posY += y;
            }
        }

        public void RotateRight()
        {

        }

        public void RotateLeft()
        {

        }


    }
}
