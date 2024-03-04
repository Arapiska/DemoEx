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
    /// Логика взаимодействия для Static.xaml
    /// </summary>
    public partial class Static : Page
    {
        private int Id;
        public Static()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                using (var context = new demoexEntities2())
                {
                    int completeOrdersCount = context.Order.Count(o => o.Status_id == 3);
                    labelCompletedRequestsCount.Content = $"Количество выполненных заявок: {completeOrdersCount}";

                    double averageCompletionTime = CalculateAverageCompletionTime(context.Order);
                    labelAverageExecutionTime.Content = $"Среднее время выполнения: {averageCompletionTime} часов";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при загрузке статистики: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private double CalculateAverageCompletionTime(IQueryable<Order> orderList)
        {
            var completedOrders = orderList
                .Where(o => o.Status_id == 3 && o.Data_add != null && o.Data_end != null)
                .ToList();

            if (completedOrders.Any())
            {
                double totalMinutes = completedOrders
                    .Sum(o => (o.Data_end - o.Data_add).Value.TotalMinutes);
                double averageMinutes = totalMinutes / completedOrders.Count;
                return Math.Round(averageMinutes / 60.0, 2); // Преобразуем в часы
            }
            else
            {
                return 0;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Gridmanagerpage(Id));
        }
    }
}
