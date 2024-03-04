﻿using System;
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
    /// Логика взаимодействия для GridIsppage.xaml
    /// </summary>
    public partial class GridIsppage : Page
    {
        private int Id;
        public GridIsppage(int id)
        {
            InitializeComponent();
            Id = id;
            IspGrid.ItemsSource = demoexEntities2.GetContext().Order.Where(_order => _order.Executive_id == Id).ToList();

        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchText = searchBox.Text.ToLower();
                bool hasMatches = false;

                // Сначала сбросим все стили перед новым поиском
                foreach (var item in IspGrid.Items)
                {
                    DataGridRow row = (DataGridRow)IspGrid.ItemContainerGenerator.ContainerFromItem(item);

                    if (row != null)
                    {
                        foreach (DataGridColumn column in IspGrid.Columns)
                        {
                            if (column is DataGridTextColumn)
                            {
                                var cellContent = column.GetCellContent(row);

                                if (cellContent != null)
                                {
                                    // Удалить стили из ячейки
                                    ((TextBlock)cellContent).ClearValue(TextBlock.BackgroundProperty);

                                    string cellText = ((TextBlock)cellContent).Text.ToLower();

                                    if (cellText.Contains(searchText))
                                    {
                                        // Подсветить текст совпадения (изменить цвет текста)
                                        ((TextBlock)cellContent).Background = Brushes.GreenYellow;
                                        hasMatches = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (!hasMatches)
                {
                    MessageBox.Show("Совпадений не найдено.", "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из учётной записи?", "Выйти из аккаунта?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new Autopage());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Получение выбранной заявки
            Order selectedOrder = IspGrid.SelectedItem as Order;

            if (selectedOrder != null)
            {
                // Переход на экран редактирования с передачей Id заявки
                NavigationService.Navigate(new EditIsppage(Id, selectedOrder.ID_order));
            }
            else
            {
                MessageBox.Show("Выберите заявку для редактирования.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                demoexEntities2.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                IspGrid.ItemsSource = demoexEntities2.GetContext().Order.ToList();
            }
        }
    }
}
