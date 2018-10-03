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
using MainApp.Models;

namespace MainApp.Views
{
    /// <summary>
    /// Interaction logic for AddNewPlayer.xaml
    /// </summary>
    public partial class AddNewPlayer : Window
    {
        public AddNewPlayer()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


    public class AddNewPlayerViewModel : BaseViewModel
    {
        public AddNewPlayerViewModel()
        {
            SaveCommand = new CommandHandler { CanExecuteAction = x => !string.IsNullOrEmpty(PlayerName), ExecuteAction = SaveCommandAction };
            CancelCommand = new CommandHandler {  ExecuteAction = CancelCommandAction };
        }

        private void CancelCommandAction(object obj)
        {
            Model = null;
            WindowClose();
        }

        private void SaveCommandAction(object obj)
        {
            try
            {
                var model = new Models.PlayerModel { Name = PlayerName };
                if (model.SaveChange())
                {
                    Model = model;
                    WindowClose();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string playerName;

        public string PlayerName
        {
            get { return playerName; }
            set {SetProperty(ref playerName ,value); }
        }

        public CommandHandler SaveCommand { get; }
        public CommandHandler CancelCommand { get; }
        public PlayerModel Model { get; private set; }
        public Action WindowClose { get; internal set; }
    }
}
