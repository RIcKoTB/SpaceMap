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
    /// Логика взаимодействия для MGUpiterWindow.xaml
    /// </summary>
    public partial class MGUpiterWindow : Window
    {
        int maxItem = 5; 
        int currentItems = 0;

        Random r = new Random();

        int score = 0;
        int missed = 0;

        DispatcherTimer GameTimer = new DispatcherTimer(); 
        List<Rectangle> itemstoremove = new List<Rectangle>(); 

        ImageBrush playerImage = new ImageBrush(); 
        ImageBrush backgroundImage = new ImageBrush();
        private const string pathToData = @"Data";

        public MGUpiterWindow()
        {
            InitializeComponent();

            GameTimer.Tick += dropItems;
            GameTimer.Interval = TimeSpan.FromMilliseconds(20);
            GameTimer.Start();

            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/netLeft.png"));
            backgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/background.jpg"));

            player1.Fill = playerImage;
            myCanvas.Background = backgroundImage;
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point position = e.GetPosition(this);
            double pX = position.X;
            double pY = position.Y;

            Canvas.SetLeft(player1, pX - 10);

            if (Canvas.GetLeft(player1) < 260)
            {
                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/netLeft.png"));
            }

            if (Canvas.GetLeft(player1) > 260)
            {
                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/netRight.png"));
            }
        }


        private void dropItems(object sender, EventArgs e)
        {
            ScoreText.Content = "Score: " + score; 
            missedText.Content = "Missed: " + missed; 

            if (currentItems < maxItem)
            {

                makePresents(); 
                currentItems++; 
                itemstoremove.Clear();
            }

            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "drops")
                {
                    int dropThis = r.Next(8, 20);

                    Canvas.SetTop(x, Canvas.GetTop(x) + dropThis);

                    Rect items = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    Rect playerObject = new Rect(Canvas.GetLeft(player1), Canvas.GetTop(player1), player1.Width, player1.Height);

                    if (playerObject.IntersectsWith(items))
                    {
                        itemstoremove.Add(x);

                        currentItems--;

                        score++;
                    }
                    else if (Canvas.GetTop(x) > 700)
                    {
                        itemstoremove.Add(x);

                        currentItems--;

                        missed++;
                    }

                }
            }

            foreach (Rectangle y in itemstoremove)
            {
                myCanvas.Children.Remove(y);
            }

            if (missed > 6)
            {
                GameTimer.Stop();

                MessageBox.Show("Ви програли!");

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }

            if (score > 20)
            {
                Win();
                GameTimer.Stop();
            }
        }

        private void Win()
        {
            string[] tmpStringArray = File.ReadAllLines(pathToData + "\\Users\\" + AuthorizationWindow.UserName + "\\" + "Info.txt");

            tmpStringArray[7] += "1"; // Запис 1 як пройденого рівня в файл для перевірки на пройденість

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

        private void makePresents()
        {
            ImageBrush presents = new ImageBrush(); 

            int i = r.Next(1, 6); 

            switch (i)
            {
                case 1:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/present_01.png"));
                    break;
                case 2:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/present_02.png"));
                    break;
                case 3:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/present_03.png"));
                    break;
                case 4:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/present_04.png"));
                    break;
                case 5:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/present_05.png"));
                    break;
                case 6:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Photo/present_06.png"));
                    break;
            }

            Rectangle newRec = new Rectangle
            {
                Tag = "drops",
                Width = 50,
                Height = 50,
                Fill = presents,
            };

            Canvas.SetTop(newRec, r.Next(60, 150) * -1);
            Canvas.SetLeft(newRec, r.Next(10, 450));

            myCanvas.Children.Add(newRec);
        }


    }
}
