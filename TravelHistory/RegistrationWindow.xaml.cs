using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace TravelHistory
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        DBClass dataBase = new DBClass();
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void Login_window_open(object sender, MouseButtonEventArgs e)//открытие окна входа 
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Registrationpage_close_button_click(object sender, RoutedEventArgs e)//закрытие окна регистрации
        {
            Application.Current.Shutdown();
        }

        private Boolean checkUser()//проверка логина
        {
           string loginUser = registrationpage_login_textbox.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select login from Users where login = '{loginUser}';";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                return true;
            }else
            {
                return false;
            }
        }

        public Boolean CheckEmail()//проверка почты
        {
            string email = registrationpage_email_textbox.Text;
            if (email.Contains("@") && email.Length > 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean CheckPass()//проверка пароля на спец.символы
        {
            string password = registrationpage_password_textbox.Password;
            if(password.Length > 5 
                && password.Any(char.IsLetter) 
                && password.Any(char.IsDigit) 
                && password.Any(char.IsPunctuation) 
                && password.Any(char.IsLower)
                && password.Any(char.IsUpper))
            {
                return true;
            } else 
            { 
                return false; 
            }
        }

        private void register_button_Click(object sender, RoutedEventArgs e)//регистрация с проверками
        {
            string firstname = registrationpage_firstname_textbox.Text;
            string lastname = registrationpage_lastname_textbox.Text;
            string loginUser = registrationpage_login_textbox.Text;
            string passUser = registrationpage_password_textbox.Password;
            string confpass = registrationpage_confpass_textbox.Password;
            string email = registrationpage_email_textbox.Text;

            if (registrationpage_firstname_textbox.Text == "" || registrationpage_lastname_textbox.Text == "" ||
                registrationpage_login_textbox.Text == "" || registrationpage_password_textbox.Password == "" ||
                registrationpage_confpass_textbox.Password == "" || registrationpage_email_textbox.Text == "")
            {
                MessageBox.Show("Поля пустые!", "Регистрация не завершена");
            } else if (checkUser())
            {
                MessageBox.Show("Аккаунт с таким логином уже существует!");
            } else if (!(registrationpage_password_textbox.Password == registrationpage_confpass_textbox.Password))
            {
                MessageBox.Show("Пароли не совпадают!\nВведите снова", "Регистрация не завершена");
                registrationpage_password_textbox.Password = "";
                registrationpage_confpass_textbox.Password = "";
                registrationpage_password_textbox.Focus();
            } else if (!CheckPass()) {
                MessageBox.Show("Пароль может содержать только символы a-z, A-Z, нижний и верхний регистр, знаки пунктуации, цифры\nВведите снова", "Регистрация не завершена");
                registrationpage_password_textbox.Password = "";
                registrationpage_confpass_textbox.Password = "";
                registrationpage_password_textbox.Focus();
            }
            else if (!CheckEmail())
            {
                MessageBox.Show("Почта должна содержать @ и не менее 5 символов!");
                registrationpage_email_textbox.Text = "";
                registrationpage_email_textbox.Focus();
            }
            else
            {
                dataBase.openConnection();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();

                string querystring = $"insert into Users (firstname, lastname, login, password, email) values ('{firstname}', '{lastname}', '{loginUser}', '{passUser}', '{email}');";
                SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count < 1)
                {
                    MessageBox.Show("Регистрация прошла успешно!");
                }
                else
                {
                    MessageBox.Show("Ошибка");
                }

                MainWindow loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }

            dataBase.closeConnection();
        }
    }
}
