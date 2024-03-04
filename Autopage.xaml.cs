using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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

namespace DemoEx
{
    /// <summary>
    /// Логика взаимодействия для Autopage.xaml
    /// </summary>
    public partial class Autopage : Page
    {
        public Autopage()
        {
            InitializeComponent();
        }

        public int CheckUser(string login)
        {
            demoexEntities2 db = demoexEntities2.GetContext();
            User user = db.User.FirstOrDefault(_user => _user.Login.Equals(login));
            if (user != null)
            {
                return user.ID;
            }
            throw new Exception("Учетной записи с таким логином не существует");

        }

        
        private void Button_Aut(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = Txtlogin.Text;
                string password = Txtpassword.Password;
                int userId = CheckUser(login);
                demoexEntities2 db = demoexEntities2.GetContext();
                User user = db.User.FirstOrDefault(_user => _user.Login.Equals(login));
                if (user == null)
                {
                    MessageBox.Show("Пользователя с таким логином не существует", "Некорректные данные", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.Password != password) 
                {
                    MessageBox.Show("Неверный пароль", "Некорректные данные", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                switch (user.ID_role)
                {
                    case 1:
                        NavigationService.Navigate(new Griduser(user.ID_role));
                        break;
                    case 2:
                        NavigationService.Navigate(new Gridmanagerpage(user.ID_role));
                        break;
                    case 3:
                        NavigationService.Navigate(new GridIsppage(user.ID_role));
                        break;
                }
                Txtlogin.Clear();
                Txtpassword.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Некорректные данные", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
