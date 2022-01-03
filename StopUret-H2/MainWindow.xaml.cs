using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace StopUret_H2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer;
        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer(60, false);
            timer.TimeUpdate += UpdateTimerDisplay;
        }

        private void UpdateTimerDisplay(string time)
        {
            Dispatcher.Invoke(() =>
            {
                timeLeftDisplay.Content = time;
            });
            
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = false;
        }

        private void StartTimer_Click(object sender, RoutedEventArgs e)
        {
            if (!timer.isStarted && timer.FullTime > 0)
            {
                timer.Start();
            }
            else if (timer.isStopped && timer.FullTime > 0)
            {
                timer.Start();
            }
        }

        private void StopTimer_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void SetTimer_Click(object sender, RoutedEventArgs e)
        {
            if (timer.isStopped || !timer.isStarted)
            {
                timer.SetTime(timeHours.Value, timeMinutes.Value, timeSeconds.Value);
            } else
            {
                MessageBox.Show("You need to stop the timer, to set new timer");
            }
        }
    }
}
