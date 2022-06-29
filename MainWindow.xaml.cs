using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Tetirs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImage = new ImageSource[] {
        new BitmapImage(new Uri("Assest/TIleEmpty.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/TileCyan.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/TileBlue.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/TileOrange.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/TileYellow.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/TileGreen.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/TilePurple.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/TileRed.png",UriKind.Relative))

        };
        private readonly ImageSource[] BlockImage = new ImageSource[] {
        new BitmapImage(new Uri("Assest/Block-Empty.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/Block-I.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/Block-J.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/Block-L.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/Block-O.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/Block-S.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/Block-T.png",UriKind.Relative)),
        new BitmapImage(new Uri("Assest/Block-Z.png",UriKind.Relative))

        };
        private readonly Image[,] imageControls;
        private GameState gameState = new GameState();
        private int maxDelay = 1000;
        private int mindelay = 75;
        private int delayDecrease = 75;
        



        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }
        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellsize = 25;
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellsize,
                        Height = cellsize
                    };
                    Canvas.SetTop(imageControl, (r - 2) * cellsize+10);
                    Canvas.SetLeft(imageControl, c * cellsize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;

                }
            }
            return imageControls;


        }

        private void DrawGrid(GameGrid grid)
        {
            for(int r = 0; r < grid.Rows; r++)
            {
                for(int c=0;c< grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImage[id];
                }
            }
        }
        private void DrawBlock(Block block)
        {

            foreach(Position p in block.TilePostions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;

                imageControls[p.Row, p.Column].Source = tileImage[block.Id];
            }
        }
        private void DrawNextBlock(BLockQueue bLockQueue)
        {
            Block next = bLockQueue.NextBlock;
            NextImage.Source = BlockImage[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if(heldBlock == null)
            {
                HoldImage.Source = BlockImage[0];

            }
            else
            {
                HoldImage.Source = BlockImage[heldBlock.Id];
            }
        }
        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BloCkdropDistance();
            foreach(Position p in block.TilePostions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity=0.25;
                imageControls[p.Row + dropDistance, p.Column].Source=tileImage[block.Id];
            }

        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BLockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            
            
            ScoreText.Text = $"Score:{gameState.Score}";
        }
        private async Task GameLoop()
        {
            int delay = Math.Max(mindelay, maxDelay - (gameState.Score * delayDecrease));
            Draw(gameState);
            while (!gameState.GameOver)
            {
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text= $"Score:{gameState.Score}";

        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBLock();
                    break;

                default:
                    return;
            }
            Draw(gameState);
        }


       

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();

        }

        
        private async void PlayAagin_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();

        }
    }
}
