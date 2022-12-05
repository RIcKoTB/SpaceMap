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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SpaceMap
{
    /// <summary>
    /// Логика взаимодействия для MGMarsWindow.xaml
    /// </summary>
    public partial class MGMarsWindow : Window
    {
        private const string pathToData = @"Data";

        DispatcherTimer gameTimer = new DispatcherTimer(); 
        List<Ellipse> removeThis = new List<Ellipse>(); 
       
        int spawnRate = 60; 
        int currentRate; 
        int health = 350; 
        int posX;
        int posY; 
        int score = 0;

        double growthRate = 0.6;

        Random rand = new Random(); 

       
        MediaPlayer playClickSound = new MediaPlayer();
        MediaPlayer playerPopSound = new MediaPlayer();

       
        Uri ClickedSound;
        Uri PoppedSound;

        
        Brush brush;
        public MGMarsWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameLoop; 
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); 
            gameTimer.Start(); 

            currentRate = spawnRate; 

            ClickedSound = new Uri("pack://siteoforigin:,,,/Sound/clickedpop.mp3");
            PoppedSound = new Uri("pack://siteoforigin:,,,/Sound/pop.mp3");
        }

        private void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
          
            currentRate -= 2;

           
            if (currentRate < 1)
            {
                currentRate = spawnRate;

                posX = rand.Next(15, 700);
                posY = rand.Next(50, 350);

                brush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(1, 255), (byte)rand.Next(1, 255), (byte)rand.Next(1, 255)));

              
                Ellipse circle = new Ellipse
                {

                    Tag = "circle",
                    Height = 10,
                    Width = 10,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = brush

                };

                Canvas.SetLeft(circle, posX);
                Canvas.SetTop(circle, posY);
               
                MyCanvas.Children.Add(circle);
            }

            foreach (var x in MyCanvas.Children.OfType<Ellipse>())
            {
               
                x.Height += growthRate; 
                x.Width += growthRate; 
                x.RenderTransformOrigin = new Point(0.5, 0.5);

                if (x.Width > 70)
                {
                    removeThis.Add(x);
                    health -= 15; 
                    playerPopSound.Open(PoppedSound); 
                    playerPopSound.Play(); 

                }

            }
            
            if (health > 1)
            {
                healthBar.Width = health;
            }
            else
            {
                GameOverFunction();
            }

            foreach (Ellipse i in removeThis)
            {
                MyCanvas.Children.Remove(i); 
            }

            
            if (score > 5)
            {
                spawnRate = 25;
            }

            if (score > 20)
            {
                spawnRate = 15;
                growthRate = 1.5;
            }

            if(score > 30)
            {
                gameTimer.Stop();
                Win();
            }


        }
        private void Win()
        {
            string[] tmpStringArray = File.ReadAllLines(pathToData + "\\Users\\" + AuthorizationWindow.UserName + "\\" + "Info.txt");

            tmpStringArray[3] += "1"; // Запис 1 як пройденого рівня в файл для перевірки на пройденість

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
        private void CanvasClicking(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Ellipse)
            {
                Ellipse circle = (Ellipse)e.OriginalSource;

                MyCanvas.Children.Remove(circle);

                score++;

                playClickSound.Open(ClickedSound);
                playClickSound.Play();

            }
        }

        private void GameOverFunction()
        {

            gameTimer.Stop(); 

        
            MessageBox.Show("Ви програли!");

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

            foreach (var y in MyCanvas.Children.OfType<Ellipse>())
            {
               
                removeThis.Add(y);
            }
           
            foreach (Ellipse i in removeThis)
            {
                MyCanvas.Children.Remove(i);
            }

           
            growthRate = .6;
            spawnRate = 60;
            score = 0;
            currentRate = 5;
            health = 350;
            removeThis.Clear();
            gameTimer.Start();
        }
    }
}
