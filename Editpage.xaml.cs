using System;
using System.Collections.Generic;
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

namespace DemoEx
{
    /// <summary>
    /// Логика взаимодействия для Editpage.xaml
    /// </summary>
    public partial class Editpage : Page
    {
        private int Id;
        private Order _currentOrder = new Order();
        private demoexEntities2 db = demoexEntities2.GetContext();

        public Editpage(int id, int orderId)
        {
            
            InitializeComponent();
            Id = id;
            
            _currentOrder = db.Order.FirstOrDefault(o => o.ID_order == orderId);

            if (_currentOrder == null)
            {
                MessageBox.Show("Не удалось загрузить заявку для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Загрузка связанных данных для выпадающих списков
            Status.ItemsSource = db.Status.ToList();
            Type.ItemsSource = db.Fault_type.ToList();
            ComboIsp.ItemsSource = db.User.Where(_user => _user.Role.Name_role == "Исполнитель").ToList();

            DatePicker1.SelectedDate = _currentOrder.Data_end;

            ComboIsp.SelectedItem = _currentOrder.User;
            Type.SelectedItem = db.Fault_type.FirstOrDefault(_faultType => _faultType.ID == _currentOrder.Fault_type_id);
            Status.SelectedItem = db.Status.FirstOrDefault(_Status => _Status.ID == _currentOrder.Status_id);

            // Установка выбранной заявки в качестве контекста данных для формы
            DataContext = _currentOrder;

            Status.SelectionChanged += Status_SelectionChanged;
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Status.SelectedItem is Status selectedStatus && selectedStatus.Name_status == "Выполнено")
            {

                DatePicker1.SelectedDate = DateTime.Now;
            }
            else
            {
                DatePicker1.SelectedDate = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var editingOrder = db.Order.Where(i => i.ID_order == _currentOrder.ID_order).FirstOrDefault();
            editingOrder.Executive_id = ((User)ComboIsp.SelectedItem).ID;
            editingOrder.Fault_type_id = ((Fault_type)Type.SelectedItem).ID;
            editingOrder.Status_id = ((Status)Status.SelectedItem).ID;
            editingOrder.Problem_description = EditProblem.Text;
            editingOrder.Data_end = DatePicker1.SelectedDate;
            db.SaveChanges();
            NavigationService.Navigate(new Gridmanagerpage(Id));

        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите завершить редактирование без сохранения?", "Вы уверены, что хотите вернуться?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new Gridmanagerpage(Id));
            }

        }
    }
}

    

