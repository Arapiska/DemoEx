using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Логика взаимодействия для Addpage.xaml
    /// </summary>
    public partial class Addpage : Page
    {
        
        public int Id;
        public Addpage(int id)
        {
            InitializeComponent();
            Id = id;
            
            Equipment.ItemsSource = demoexEntities2.GetContext().Gear.ToList();
            Type.ItemsSource = demoexEntities2.GetContext().Fault_type.ToList();
        }
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Problem.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните поле 'Опишите проблему'", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DB db = new DB();

                var EquipmentName = (Equipment.SelectedItem as Gear).Name_Gear;
                var FaultTypeName = (Type.SelectedItem as Fault_type).Fault_name;

                if (!string.IsNullOrWhiteSpace(EquipmentName) && !string.IsNullOrWhiteSpace(FaultTypeName))
                {
                    var selectedEquipment = db.GetContext().Gear.FirstOrDefault(g => g.Name_Gear == EquipmentName);
                    var selectedFaultType = db.GetContext().Fault_type.FirstOrDefault(t => t.Fault_name == FaultTypeName);

                    if (selectedEquipment != null && selectedFaultType != null)
                    {
                        Order order = new Order
                        {
                            ID_order = db.getLastID() + 1,
                            Gear_id = selectedEquipment.ID,
                            Fault_type_id = selectedFaultType.ID,
                            ID_User = Id,
                            Executive_id = null,
                            Executive_comment = null,
                            Problem_description = Problem.Text,
                            Data_add = DateTime.Now,
                            Data_end = null,
                            Status_id = 1
                        };

                        db.addToTable(order);
                        NavigationService.Navigate(new Griduser(Id));
                    }
                    else
                    {
                        MessageBox.Show("Выбранные элементы не найдены в базе данных.");
                    }
                }
                else
                {
                    MessageBox.Show("Выберите оборудование и тип неисправности.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                MessageBox.Show($"Exception: {ex.Message}");
            }
                 
        }

        private void Button_Back(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите завершить заполнение заявки без сохранения?", "Вы уверены, что хотите вернуться?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new Griduser(Id));
            }

        }

    }
}
