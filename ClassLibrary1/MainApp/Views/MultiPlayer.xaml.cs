using Ocph.DAL;
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
    /// Interaction logic for MultiPlayer.xaml
    /// </summary>
    public partial class MultiPlayer : Window
    {
        public MultiPlayer()
        {
            InitializeComponent();
            DataContext = new MultiPlayerViewModel() { WindowClose=Close};
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


    public class MultiPlayerViewModel:BaseNotify
    {
        public MultiPlayerViewModel()
        {
            OKCommand = new CommandHandler { CanExecuteAction = OKValidate, ExecuteAction = OKCommandaction };
        }

        private bool OKValidate(object obj)
        {
            if (!string.IsNullOrEmpty(Player1) && !string.IsNullOrEmpty(Player2))
                return true;
            else
                return false;
        }

        private void OKCommandaction(object obj)
        {
            var form = new MainWindow(Player1, Player2);
            form.Show();
            WindowClose();
        }

        private string player1;

        public string Player1
        {
            get { return player1; }
            set {SetProperty(ref player1 ,value); }
        }


        private string player2;

        public string Player2
        {
            get { return player2; }
            set {SetProperty(ref player2 ,value); }
        }

        public CommandHandler OKCommand { get; }
        public Action WindowClose { get; internal set; }
    }
}
