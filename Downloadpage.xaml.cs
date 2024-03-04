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
using System.Windows.Threading;

namespace DemoEx
{
    /// <summary>
    /// Логика взаимодействия для Downloadpage.xaml
    /// </summary>
    public partial class Downloadpage : Page
    {
        private DispatcherTimer timer;
        public Downloadpage()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
            timer.Stop();
            
            NavigationService navService = NavigationService.GetNavigationService(this);
            navService?.Navigate(new Autopage());
        }
    }
}
