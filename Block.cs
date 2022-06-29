

using System.Collections.Generic;

namespace Tetirs
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffSet { get; }
        public abstract int Id { get; }
        public int roationState;
        private Position offSet;
        public Block()
        {
            offSet=new Position(StartOffSet.Row,StartOffSet.Column);
        }
        public IEnumerable<Position> TilePostions()
        {

                foreach (Position P in Tiles[roationState])
                {
                    yield return new Position(P.Row + offSet.Row, P.Column + offSet.Column);

                }
            

        }
        public void RotateCW()
        {
            roationState= (roationState+1)%Tiles.Length;
        }
        public void RotateCCW()
        {
            if (roationState == 0)
            {
                roationState = Tiles.Length-1;
            }
            else
            {
                roationState--;
            }

            }
            public void Move(int row,int columns)
        {
            offSet.Row += row;
            offSet.Column += columns;

        }
        public void Rest()
        {
            roationState=0;
            offSet.Row=StartOffSet.Row;
            offSet.Column = StartOffSet.Column;

        }

    }
}
