using GameLib;
using SharedApp;
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

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBoard game;

        public List<PionView> Pions { get; }

        public MainWindow()
        {
            InitializeComponent();
            this.Pions = new List<PionView>();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           
            game = Board.CreateNewGame(3);

            for(var i=0;i<3;i++)
            {
                boardView.ColumnDefinitions.Add(new ColumnDefinition());
                boardView.RowDefinitions.Add(new RowDefinition());
            }


           await game.SetPlayer(new Player("Player1", SharedApp.PlayerType.Human, SharedApp.PlayerPionType.Circle),
                new Player("Player2", SharedApp.PlayerType.Human, SharedApp.PlayerPionType.Cross));

           await game.SwichPlayer();


        }

        private void PionView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (game.SelectedPion == null)
            {
                var pView = (sender as PionView);
                game.SelectedPion =pView.PionModel;
                pView.Background = new SolidColorBrush(Colors.Green);
            }
     
            else
                game.SelectedPion = null;
        }

        private async void boardView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

           

            Player player = await game.GetPlayerIsPlay();
            Position newPosition = GetNewPosition(Mouse.GetPosition(boardView));




            if(FreePosition(newPosition) && player!=null && player.Pions.Count< await game.GetBoardCount() )
            {
                var pion = new PionView(await player.CreatePion(),player);
                pion.PionModel.Position = newPosition;
                pion.OnSelected += Pion_OnSelected;
                Pions.Add(pion);
                boardView.Children.Add(pion);
                await game.Play(player, pion.PionModel, newPosition);
            }

            




            if (e.ClickCount > 0 && game.SelectedPion!=null && (game.SelectedPion.Position.Row!=newPosition.Row||game.SelectedPion.Position.Column!=newPosition.Column )) // for double-click, remove this condition if only want single click
            {


                // row and col now correspond Grid's RowDefinition and ColumnDefinition mouse was 
                // over when double clicked!
                var pion = Pions.Where(O => O.PionModel.Name == game.SelectedPion.Name &&O.PionModel.PionType==player.PionType).FirstOrDefault();
                 await  game.Play(player, pion.PionModel,newPosition);
                game.SelectedPion = null;
     //           pion.Background = new SolidColorBrush(Colors.Red);
            }

            gv.ItemsSource = game.GetGreedy();
            gvHs.ItemsSource = game.GetHs();
            gvFSK.ItemsSource = game.GetFSK();
            fo.Content = game.GetFO();

        }

        private void Pion_OnSelected(PionView pion)
        {
            game.SelectedPion = pion.PionModel;
        }

        private bool FreePosition(Position newPosition)
        {
            var result = Pions.Where(O => O.PionModel.Position.Column == newPosition.Column && O.PionModel.Position.Row == newPosition.Row).FirstOrDefault();
            if (result == null)
                return true;
            else
                return false;
        }

        private Position GetNewPosition(Point point)
        {
          

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // calc row mouse was over
            foreach (var rowDefinition in boardView.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // calc col mouse was over
            foreach (var columnDefinition in boardView.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

            return new Position(row, col);
        }
    }

    

    
}
