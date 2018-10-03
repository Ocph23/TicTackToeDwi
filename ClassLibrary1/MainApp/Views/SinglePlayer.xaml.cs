using GreedyLib;
using MainApp.Models;
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
    /// Interaction logic for SinglePlayer.xaml
    /// </summary>
    public partial class SinglePlayer : Window
    {
        public SinglePlayer()
        {
            InitializeComponent();
            DataContext = new SinglePlayerViewModel();
        }

       
    }


    public class SinglePlayerViewModel:BaseViewModel
    {
        public CommandHandler SinglePlayerScoreCommand { get; }
        public CommandHandler PlayCommand { get; }
        public CommandHandler NewPlayerCommand { get; }
        public List<PlayerModel> Source { get; set; }
        public CollectionView SourceView { get; }

        public SinglePlayerViewModel()
        {
            SinglePlayerScoreCommand = new CommandHandler { ExecuteAction=SinglePlayerScoreAction };
            PlayCommand = new CommandHandler { CanExecuteAction=x=>SelectedItem!=null, ExecuteAction = PlayAction };
            NewPlayerCommand = new CommandHandler { ExecuteAction = NewPlayerAction };
            Source = new List<PlayerModel>();
            SourceView = (CollectionView)CollectionViewSource.GetDefaultView(Source);
            LoadAsync();
        }

        private void LoadAsync()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.Players.Select();
                Source.Clear();
                foreach(var item in result)
                {
                    Source.Add(item);
                }

                SourceView.Refresh();
            }
        }

        private void NewPlayerAction(object obj)
        {
            var form = new Views.AddNewPlayer();
            var vm = new AddNewPlayerViewModel() { WindowClose = form.Close };
            form.DataContext = vm;
            form.ShowDialog();

            if (vm.Model != null)
                Source.Add(vm.Model);

            SourceView.Refresh();
        }

        private void PlayAction(object obj)
        {
            var form = new MainWindow(SelectedItem);
            form.ShowDialog();
        }



        private PlayerModel _SelectedItem;

        public PlayerModel SelectedItem
        {
            get { return _SelectedItem; }
            set { SetProperty(ref _SelectedItem ,value); }
        }


        private void SinglePlayerScoreAction(object obj)
        {
            var form = new Views.SinglePlayerScore();
            form.ShowDialog();
        }
    }
}
