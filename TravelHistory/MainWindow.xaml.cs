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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TravelHistory
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DBClass dataBase = new DBClass();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Registration_window_open(object sender, RoutedEventArgs e)//закрытие окна по кнопке
        {
            RegistrationWindow RegistrationWindow = new RegistrationWindow();
            RegistrationWindow.Show();
            this.Close();
        }

        private void Loginpage_close_button_click(object sender, RoutedEventArgs e)//переход на окно регистрации
        {
            Application.Current.Shutdown();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            string loginUser = loginpage_login_textbox.Text;
            string passwordUser = loginpage_password_textbox.Password;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select id_user, login, password from Users where login = '{loginUser}' and password = '{passwordUser}';";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if(table.Rows.Count == 1)
            {
                MessageBox.Show("Welcome!");
                loginpage_login_textbox.Text = "";
                loginpage_password_textbox.Password = "";
            }
            else
            {
                this.Close();
                MessageBox.Show("None!");
                RegistrationWindow regist = new RegistrationWindow();
                regist.Show();
            }
        }

        private void AccountClick(object sender, MouseButtonEventArgs e)
        {
            Account account = new Account();
            account.Show();
            this.Close();
        }
    }
}
