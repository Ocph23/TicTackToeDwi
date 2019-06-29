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
using System.Windows.Shapes;

namespace MainApp.Views
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();

            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory+"Captures");
        }

        private void single_Click(object sender, RoutedEventArgs e)
        {

            var form = new Views.SinglePlayer();
            form.ShowDialog();
        }

        private void multi_Click(object sender, RoutedEventArgs e)
        {
            var form = new Views.MultiPlayer();
            form.ShowDialog();
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            var form = new Views.HelpView();
            form.ShowDialog();
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            var form = new Views.AboutView();
            form.ShowDialog();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
