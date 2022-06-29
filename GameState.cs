namespace Tetirs
{
    public class GameState
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get=> currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Rest();
                
                for(int i=0; i < 2; i++) {
                    currentBlock.Move(1, 0);
                    if (!BlockFit())
                {
                    currentBlock.Move(-1, 0);

                }
                }
            }
        }
        public GameGrid GameGrid { get; }
        public BLockQueue BLockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; } 
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public GameState()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            GameGrid = new GameGrid(22, 10);
            BLockQueue = new BLockQueue();
            currentBlock = BLockQueue.GetAndUpdate();
            CanHold = true;
        }
        private bool BlockFit()
        {
            foreach (Position p in currentBlock.TilePostions())
            {
                if (!GameGrid.IsEmpty(p.Row,p.Column))
                {
                    return false;

                }
            }
            return true;
        }
        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }
            if(currentBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BLockQueue.GetAndUpdate();
            }
            else
            {
                Block temp = currentBlock;

                currentBlock = BLockQueue.GetAndUpdate();
                HeldBlock = temp;

            }
            CanHold = false;
        }
        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();
            if (!BlockFit())
            {
                currentBlock.RotateCCW();
            }
        }
        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();
            if (!BlockFit())
            {
                currentBlock.RotateCW();
            }
        }
        public void MoveBlockLeft()
        {
            currentBlock.Move(0,-1);
            if (!BlockFit())
            {
                CurrentBlock.Move(0,1);
            }

        }
        public void MoveBlockRight()
        {
            currentBlock.Move(0, 1);
            if (!BlockFit())
            {
                CurrentBlock.Move(0, -1);
            }

        }
        private bool ISGameOver()
        {
            return !(GameGrid.IsRowEmpty(0)&&GameGrid.IsRowEmpty(1));
        }
        private void PlaceBlock()
        {
            foreach(Position p in currentBlock.TilePostions())
            {
                GameGrid[p.Row, p.Column] = currentBlock.Id;
            }
               Score+=GameGrid.ClearFullRows();
            if (ISGameOver())
            {
                GameOver = true;

            }
            else
            {
                CurrentBlock = BLockQueue.GetAndUpdate();
                CanHold = true;
            }
        }
        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);
            if (!BlockFit())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }

        }
        private int TileDropDistance(Position P)
        {
            int drop = 0;
            while (GameGrid.IsEmpty(P.Row+drop+1,P.Column))
            {
                drop++;

            }
            return drop;
        }
        public int BloCkdropDistance()
        {
            int drop = GameGrid.Rows;


            foreach (Position p in currentBlock.TilePostions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));

            }
            return drop;

        }


        public void DropBLock()
        {
            currentBlock.Move(BloCkdropDistance(),0);
            PlaceBlock();
        }
    }
}
