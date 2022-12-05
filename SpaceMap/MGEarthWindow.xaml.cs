using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace SpaceMap
{
    /// <summary>
    /// Логика взаимодействия для MGEarthWindow.xaml
    /// </summary>
    public partial class MGEarthWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        private const string pathToData = @"Data";

        bool goLeft, goRight, goDown, goUp; 
        bool noLeft, noRight, noDown, noUp; 

        int speed = 8; // player speed

        Rect pacmanHitBox; 

        int ghostSpeed = 10; // ghost image speed
        int ghostMoveStep = 160; 
        int currentGhostStep; 
        int score = 0; // score keeping integer


        public MGEarthWindow()
        {
            InitializeComponent();
            GameSetUp();
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            // this is the key down event

            if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false; 

                goLeft = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2); 
            }

            if (e.Key == Key.Right && noRight == false)
            {
                noLeft = noUp = noDown = false;
                goLeft = goUp = goDown = false; 

                goRight = true; 

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2); 

            }

            if (e.Key == Key.Up && noUp == false)
            {
                noRight = noDown = noLeft = false; 
                goRight = goDown = goLeft = false; 

                goUp = true; 

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2); 
            }

            if (e.Key == Key.Down && noDown == false)
            {
                noUp = noLeft = noRight = false; 
                goUp = goLeft = goRight = false;

                goDown = true; 

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2); 
            }
        }

        private void GameSetUp()
        {
            MyCanvas.Focus(); 

            gameTimer.Tick += GameLoop; 
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); 
            gameTimer.Start(); 
            currentGhostStep = ghostMoveStep; 
            
            ImageBrush pacmanImage = new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/pacman.jpg"));
            pacman.Fill = pacmanImage;

            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/red.jpg"));
            redGuy.Fill = redGhost;

            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/orange.jpg"));
            orangeGuy.Fill = orangeGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/pink.jpg"));
            pinkGuy.Fill = pinkGhost;

        }

        private void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score; // show the scoreo to the txtscore label. 


            if (goRight)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }
            if (goLeft)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }
            if (goUp)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            if (goDown)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }
            // end the movement 


            // restrict the movement
            if (goDown && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height)
            {
                noDown = true;
                goDown = false;
            }
            if (goUp && Canvas.GetTop(pacman) < 1)
            {
                noUp = true;
                goUp = false;
            }
            if (goLeft && Canvas.GetLeft(pacman) - 10 < 1)
            {
                noLeft = true;
                goLeft = false;
            }
            if (goRight && Canvas.GetLeft(pacman) + 70 > Application.Current.MainWindow.Width)
            {
                noRight = true;
                goRight = false;
            }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height); 

            
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
               
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); 

              
                if ((string)x.Tag == "wall")
                {
                   
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }
                   
                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }
                    
                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }
                    
                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }
                }

                if ((string)x.Tag == "coin")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        // add 1 to the score
                        score++;
                    }
                }

                // if any rectangle has the tag ghost inside of it
                if ((string)x.Tag == "ghost")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        GameOver("Ghosts got you, click ok to play again");
                    }

                    if (x.Name.ToString() == "orangeGuy")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);

                    }
                    else
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }

                    currentGhostStep--;

                    if (currentGhostStep < 1)
                    {
                        currentGhostStep = ghostMoveStep;
                        ghostSpeed = -ghostSpeed;
                    }
                }
            }

            // if the player collected 85 coins in the game

            if (score == 85)
            {
                gameTimer.Stop();
                Win();
            }
        }

        private void Win()
        {
            string[] tmpStringArray = File.ReadAllLines(pathToData + "\\Users\\" + AuthorizationWindow.UserName + "\\" + "Info.txt");

            tmpStringArray[1] += "1"; // Запис 1 як пройденого рівня в файл для перевірки на пройденість

            File.WriteAllText(pathToData + "\\Users\\" + AuthorizationWindow.UserName + "\\" + "Info.txt", string.Empty);

            for (int i = 0; i <= 8; i++)
            {
                File.AppendAllText(pathToData + "\\Users\\" + AuthorizationWindow.UserName + "\\" + "Info.txt", tmpStringArray[i] + "\n");
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

            MessageBox.Show("Ви виграли!");
        }

        private void GameOver(string message)
        {
            gameTimer.Stop(); 
            MessageBox.Show("Ви програли");
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
