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
    /// Логика взаимодействия для MGVeneraWindow.xaml
    /// </summary>
    public partial class MGVeneraWindow : Window
    {
        private const string pathToData = @"Data";

        ImageBrush backgroundImage = new ImageBrush();
        ImageBrush ghostSprite = new ImageBrush(); 
        ImageBrush aimImage = new ImageBrush();

        DispatcherTimer DummyMoveTimer = new DispatcherTimer(); 
        DispatcherTimer showGhostTimer = new DispatcherTimer(); 

        int topCount = 0; 
        int bottomCount = 0;

        int score; 
        int miss;

        List<int> topLocations; 
        List<int> bottomLocations; 

        List<Rectangle> removeThis = new List<Rectangle>(); 

        Random rand = new Random();

        public MGVeneraWindow()
        {
            InitializeComponent();

            this.Cursor = Cursors.None; 

            backgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/background2.png"));
            MyCanvas.Background = backgroundImage;

            scopeImage.Source = new BitmapImage(new Uri("pack://application:,,,/Photo/sniper-aim.png"));

            ghostSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/ghost.png"));

            DummyMoveTimer.Tick += DummyMoveTick;
            DummyMoveTimer.Interval = TimeSpan.FromMilliseconds(rand.Next(800, 2000));
            DummyMoveTimer.Start();

            showGhostTimer.Tick += ghostAnimation;
            showGhostTimer.Interval = TimeSpan.FromMilliseconds(20);
            showGhostTimer.Start();


            topLocations = new List<int> { 270, 540, 23, 270, 540, 23 };

            bottomLocations = new List<int> { 128, 678, 138, 128, 678, 138 };
        }

        private void ShootDummy(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                Rectangle activeRec = (Rectangle)e.OriginalSource; 

                MyCanvas.Children.Remove(activeRec); 

                score++; // add 1 to the score

                if ((string)activeRec.Tag == "top")
                {
                    topCount--;
                }
                else if ((string)activeRec.Tag == "bottom")
                {
                    bottomCount--;
                }

                Rectangle ghostRec = new Rectangle
                {
                    Width = 60,
                    Height = 100,
                    Fill = ghostSprite,
                    Tag = "ghost"
                };

                Canvas.SetLeft(ghostRec, Mouse.GetPosition(MyCanvas).X - 40); 
                Canvas.SetTop(ghostRec, Mouse.GetPosition(MyCanvas).Y - 60); 

                MyCanvas.Children.Add(ghostRec); 
            }

            if(score > 15)
            {
                Win();
            }
        }

        private void Win()
        {
            string[] tmpStringArray = File.ReadAllLines(pathToData + "\\Users\\" + AuthorizationWindow.UserName + "\\" + "Info.txt");

            tmpStringArray[8] += "1"; // Запис 1 як пройденого рівня в файл для перевірки на пройденість

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

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point position = e.GetPosition(this);
            double pX = position.X;
            double pY = position.Y;

            Canvas.SetLeft(scopeImage, pX - (scopeImage.Width / 2));
            Canvas.SetTop(scopeImage, pY - (scopeImage.Height / 2));
        }

        private void ghostAnimation(object sender, EventArgs e)
        {
            scoreText.Content = "Scored: " + score;
            missText.Content = "Missed: " + miss;

            if (miss == 10)
            {
                MessageBox.Show("Ви програли!");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "ghost")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 5);

                    if (Canvas.GetTop(x) < -180)
                    {
                        removeThis.Add(x);

                    }
                }
            }

            foreach (Rectangle y in removeThis)
            {
                MyCanvas.Children.Remove(y);
            }
        }

        private void DummyMoveTick(object sender, EventArgs e)
        {
            removeThis.Clear(); 

            foreach (var i in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)i.Tag == "top" || (string)i.Tag == "bottom")
                {
                    removeThis.Add(i);

                    topCount--; 
                    bottomCount--; 
                    miss++; 
                }
            }

            if (topCount < 3)
            {
                ShowDummies(topLocations[rand.Next(0, 5)], 35, rand.Next(1, 4), "top");
                topCount++; 
            }


            if (bottomCount < 3)
            {
                ShowDummies(bottomLocations[rand.Next(0, 5)], 230, rand.Next(1, 4), "bottom");
                bottomCount++; 
            }
        }

        private void ShowDummies(int x, int y, int skin, string tag)
        {

            ImageBrush dummyBackground = new ImageBrush();

            switch (skin)
            {
                case 1:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/dummy01.png"));
                    break;
                case 2:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/dummy02.png"));
                    break;
                case 3:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/dummy03.png"));
                    break;
                case 4:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/dummy04.png"));
                    break;
            }

            Rectangle newRec = new Rectangle
            {
                Tag = tag,
                Width = 80,
                Height = 155,
                Fill = dummyBackground
            };

            Canvas.SetTop(newRec, y); 
            Canvas.SetLeft(newRec, x); 

            MyCanvas.Children.Add(newRec); 
        }
    }
}
