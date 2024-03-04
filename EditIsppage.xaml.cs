using System;
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
    /// Логика взаимодействия для EditIsppage.xaml
    /// </summary>
    public partial class EditIsppage : Page
    {
        private int Id;
        private Order _currentOrder = new Order();
        private demoexEntities2 db = demoexEntities2.GetContext();
        public EditIsppage(int id, int orderId)
        {
            InitializeComponent();
            Id = id;
            _currentOrder = db.Order.FirstOrDefault(o => o.ID_order == orderId);

            if (_currentOrder == null)
            {
                MessageBox.Show("Не удалось загрузить заявку для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Status.ItemsSource = db.Status.ToList();            
            Status.SelectedItem = db.Status.FirstOrDefault(_Status => _Status.ID == _currentOrder.Status_id);

            // Установка выбранной заявки в качестве контекста данных для формы
            DataContext = _currentOrder;

            Status.ItemsSource = db.Status.ToList();


            Status.SelectedItem = db.Status.FirstOrDefault(_Status => _Status.ID == _currentOrder.Status_id);
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите завершить редактирование без сохранения?", "Вы уверены, что хотите вернуться?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
               NavigationService.Navigate(new GridIsppage(Id));
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var editingOrder = db.Order.Where(i => i.ID_order == _currentOrder.ID_order).FirstOrDefault(); 
            editingOrder.Executive_comment = Comm.Text;
            editingOrder.Status_id = ((Status)Status.SelectedItem).ID;
            
            if (editingOrder.Status_id == 3)
            {
                editingOrder.Data_end = DateTime.Now;
            }
            db.SaveChanges();
            NavigationService.Navigate(new GridIsppage(Id));

        }

    }
}
