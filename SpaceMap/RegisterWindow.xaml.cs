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

namespace SpaceMap
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private const string pathToData = @"Data";

        public static string UserName = ""; 
        public static string UserPassword = ""; 

        private void btnLoginWay_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(pathToData + "\\Users"))
            {
                Directory.CreateDirectory(pathToData + "\\Users");
            }

            UserName = txtbUserName.Text;

            if (UserName.Length == 0) // Перевірка на пусте поле
            {
                MessageBox.Show("Логін не може бути пустий");
                return;
            }

            UserPassword = pswbPassword.Password;

            if (UserPassword.Length == 0) // Перевірка на пусте поле
            {
                MessageBox.Show("Пароль не може бути пустий");
                return;
            }


            if (Directory.Exists(pathToData + "\\Users\\" + UserName))
            {
                MessageBox.Show("Користувач вже інсує");
                return;
            }

            Directory.CreateDirectory(pathToData + "\\Users\\" + UserName);

            string hashOfPassword = BCrypt.Net.BCrypt.HashPassword(UserPassword);

            File.WriteAllText(pathToData + "\\Users\\" + UserName + "\\Info.txt", hashOfPassword + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n");
            MessageBox.Show("Реєстрація успішна!");

            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }
    }
}
