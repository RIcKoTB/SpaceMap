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

namespace SpaceMap
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string pathToData = @"Data";

        string[] tmpStringArray = File.ReadAllLines(pathToData + "\\Users\\" + AuthorizationWindow.UserName + "\\" + "Info.txt");

        public MainWindow()
        {
            InitializeComponent();
            UserNameText();
            PlanetsOpacity();
        }

        private void UserNameText()
        {
            txtUserName.Text = AuthorizationWindow.UserName;
        }

        private void PlanetsOpacity()
        {
            if (tmpStringArray[1] == "1")
            {
                btnEarth.Opacity = 1;
            }

            if (tmpStringArray[2] == "1")
            {
                btnSaturn.Opacity = 1;
            }

            if (tmpStringArray[3] == "1")
            {
                btnMars.Opacity = 1;
            }

            if (tmpStringArray[4] == "1")
            {
                btnNeptun.Opacity = 1;
            }

            if (tmpStringArray[5] == "1")
            {
                btnMerkury.Opacity = 1;
            }

            if (tmpStringArray[6] == "1")
            {
                btnUran.Opacity = 1;
            }

            if (tmpStringArray[7] == "1")
            {
                btnUpiter.Opacity = 1;
            }

            if (tmpStringArray[8] == "1")
            {
                btnVenera.Opacity = 1;
            }

        }

        private void btnNeptun_Click(object sender, RoutedEventArgs e)
        {
            if (tmpStringArray[4] != "1")
            {
                MGNeptunWindow mGNeptunWindow = new MGNeptunWindow();
                mGNeptunWindow.Show();
                this.Close();
            }
        }

        private void btnUpiter_Click(object sender, RoutedEventArgs e)
        {
            if (tmpStringArray[7] != "1")
            {
                MGUpiterWindow mGUpiterWindow = new MGUpiterWindow();
                mGUpiterWindow.Show();
                this.Close();
            }
        }

        private void btnEarth_Click(object sender, RoutedEventArgs e)
        {
            if(tmpStringArray[1] != "1")
            {
                MGEarthWindow mGEarthWindow = new MGEarthWindow();
                mGEarthWindow.Show();
                this.Close();
            }
        }

        private void btnUran_Click(object sender, RoutedEventArgs e)
        {
            if (tmpStringArray[6] != "1")
            {
                MGUranWindow mGUranWindow = new MGUranWindow();
                mGUranWindow.Show();
                this.Close();
            }
        }

        private void btnSaturn_Click(object sender, RoutedEventArgs e)
        {
            if (tmpStringArray[2] != "1")
            {
                MGSaturnWindow MGSaturnWindow = new MGSaturnWindow();
                MGSaturnWindow.Show();
                this.Close();
            }
        }

        private void btnMars_Click(object sender, RoutedEventArgs e)
        {

            if (tmpStringArray[3] != "1")
            {
                MGMarsWindow mGMarsWindow = new MGMarsWindow();
                mGMarsWindow.Show();
                this.Close();
            }
        }

        private void btnVenera_Click(object sender, RoutedEventArgs e)
        {
            if (tmpStringArray[8] != "1")
            {
                MGVeneraWindow mGVeneraWindow = new MGVeneraWindow();
                mGVeneraWindow.Show();
                this.Close();
            }
        }

        private void btnMerkury_Click(object sender, RoutedEventArgs e)
        {
            if (tmpStringArray[5] != "1")
            {
                MGMerkuryWindow mGMerkuryWindow = new MGMerkuryWindow();
                mGMerkuryWindow.Show();
                this.Close();
            }
        }

        private void btnLoggout_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}
