using GameLib;
using GreedyLib;
using MainApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public delegate void delegateUpdateConsole (string message);
    public partial class MainWindow : Window
    {
        private AppTime vm;

        public MainWindow(PlayerModel player)
        {
            InitializeComponent();
            vm = new AppTime(boardView) { WindowClose=Close};
            vm.SetPlayer(player);
            vm.OnUpdateConsole += Vm_OnUpdateConsole;
            DataContext =vm;
            vm.GameMode = GameMode.SinglePlayer;
            board33.IsChecked = true;
        }

        public MainWindow(string player1, string player2)
        {
            InitializeComponent();
            vm = new AppTime(boardView);
            vm.OnUpdateConsole += Vm_OnUpdateConsole;
            DataContext = vm;
            vm.GameMode = GameMode.MultiPlayer;
            board33.IsChecked = true;
        }

        private void Vm_OnUpdateConsole(string message)
        {
            console.AppendText(message);
            console.ScrollToEnd();
        }

        //private void PionView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (vm.BoardGame.SelectedPion == null)
        //    {
        //        var pView = (sender as PionView);
        //        vm.BoardGame.SelectedPion =pView.PionModel;
        //        pView.Background = new SolidColorBrush(Colors.Green);
        //    }
        //    else
        //        vm.BoardGame.SelectedPion = null;
        //}

        private void boardView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            vm.PreviewMouseLeftButtonDown(e.ClickCount > 0);
        }
       

        private void ulang_Click(object sender, RoutedEventArgs e)
        {
            console.Document.Blocks.Clear();
            vm.StartCommand.Execute(null);
        }
        

        private void board33_Checked(object sender, RoutedEventArgs e)
        {
            vm.BoardLength = 3;
            console.Document.Blocks.Clear();
            vm.LoadBoard(vm.BoardLength);
        }

        private void board44_Checked(object sender, RoutedEventArgs e)
        {
            console.Document.Blocks.Clear();
            vm.BoardLength = 4;
            vm.LoadBoard(vm.BoardLength);
        }

        private void btnGreedy_Click(object sender, RoutedEventArgs e)
        {
          var result=  vm.BoardGame.GetBestPosition(EngineType.Greedy);
            console.AppendText(result.Item1);
            console.AppendText("Best Position : " + result.Item2.Row + "," + result.Item2.Column);
            vm.CurrentTime = result.Item3;
            console.ScrollToEnd();
        }

        private void btnBB_Click(object sender, RoutedEventArgs e)
        {
            vm.BoardGame.GetBestPosition(EngineType.BranchAndBound);
        }

      
    }

    public class AppTime:Ocph.DAL.BaseNotify
    {
        public event delegateUpdateConsole OnUpdateConsole;
        public Board BoardGame { get; set; }
        public List<PionView> Pions { get; } = new List<PionView>();
        private Grid boardView;
        public PlayerModel SelectedPlayer { get; }
        public GameMode GameMode { get; set; }
        public int BoardLength { get; set; }

        public AppTime(Grid board)
        {
            boardView = board;
            BoardGame = Machine.CreateNewGame();
            BoardGame.OnPlayerWin += Game_OnPlayerWin;
            BoardGame.ComputerOnPlaying += Game_ComputerOnPlaying;
            BoardGame.GameIsDraw += BoardGame_GameIsDraw;

            GreedyCommand = new CommandHandler { CanExecuteAction = x => Mulai, ExecuteAction = GreedyCommandAction };
            BranchCommand = new CommandHandler { CanExecuteAction = x => Mulai, ExecuteAction = GreedyCommandAction };

            StartCommand = new CommandHandler { CanExecuteAction = x => !Mulai, ExecuteAction = StartCommandAction };
            RestartCommand = new CommandHandler { CanExecuteAction = x => Mulai, ExecuteAction = ResetCommandAction };
            CloseCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CloseCommandAction };
            UpdateTime();
        }

        private async void CloseCommandAction(object obj)
        {
            try
            {
                Stop();
                await Task.Delay(1);
                if (GameMode == GameMode.SinglePlayer && SelectedPlayer.Score == null)
                {
                    SelectedPlayer.Score = new ScoreModel
                    {
                        ComputerWin = Player2Win,
                        PlayerWin = Player1Win,
                        Draw = Draw,
                        PlayerId = SelectedPlayer.Id.Value,
                        Time = PlayTime
                    };
                    SelectedPlayer.SaveChange();
                }
                WindowClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetCommandAction(object obj)
        {
            Stop();
            LoadBoard(BoardLength);
            StartCommand.Execute(null);
        }

        public void Stop()
        {
            Mulai = false;
            if (AppStopwatch != null && AppStopwatch.IsRunning)
                AppStopwatch.Stop();
        }

        private void StartCommandAction(object obj)
        {
            GameCount++;
            Mulai = true;
            LoadBoard(BoardLength);
            AppStopwatch = Stopwatch.StartNew();
        }

        private async void SetPlayer(string player1, string player2)
        {
            await BoardGame.SetPlayer(new Player(player1, PlayerType.Human, PlayerPionType.Cross),
                  new Player(player2, PlayerType.Human, PlayerPionType.Circle));
        }

        internal async void SetPlayer(PlayerModel player)
        {
            await BoardGame.SetPlayer(new Player(player.Name, PlayerType.Human, PlayerPionType.Cross),
                 new Player("Computer", PlayerType.Computer, PlayerPionType.Circle));
        }

        public void LoadBoard(int board)
        {
            Pions.Clear();
            boardView.Children.Clear();
            boardView.ColumnDefinitions.Clear();
            boardView.RowDefinitions.Clear();
            BoardGame.StartNewGame(board);
            for (var i = 0; i < board; i++)
            {
                boardView.ColumnDefinitions.Add(new ColumnDefinition());
                boardView.RowDefinitions.Add(new RowDefinition());
            }

            for (var i = 0; i < board; i++)
            {
                for (var j = 0; j < board; j++)
                {
                    var back = new Border { BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Colors.Gray) };
                    Grid.SetRow(back, i);
                    Grid.SetColumn(back, j);
                    boardView.Children.Add(back);

                }
            }
         //   await BoardGame.SwichPlayer();
        }

        private void BoardGame_GameIsDraw()
        {
            Stop();
            Mulai = false;
            MessageBox.Show("GAME DRAW");
            Draw++;
        }

        //Event Action
        private void BoardGame_OnChangeConsole(string text)
        {
            OnUpdateConsole?.Invoke(text);
        }

        private void Game_OnPlayerWin(Player player)
        {
            Stop();
            Mulai = false;
            if (player.Id == 1)
                Player1Win++;
            else
                Player2Win++;
            MessageBox.Show("Player " + player.Id + " Win");

        }

        private async void Game_ComputerOnPlaying(Player player, Position position)
        {
            if (FreePosition(position))
            {
                var pion = new PionView(await player.CreatePion(), player);
                pion.PionModel.Position = position;
                Pions.Add(pion);
                boardView.Children.Add(pion);
                await BoardGame.Play(player, pion.PionModel, position);
            }
        }
        private bool FreePosition(Position newPosition)
        {
            var result = Pions.Where(O => O.PionModel.Position.Column == newPosition.Column && O.PionModel.Position.Row == newPosition.Row).FirstOrDefault();
            if (result == null)
                return true;
            else
                return false;
        }

        public async void PreviewMouseLeftButtonDown(bool countGraderThen0)
        {
            if (Mulai)
            {
                Player player = await BoardGame.GetPlayerIsPlay();
                Position newPosition = GetNewPosition(Mouse.GetPosition(boardView));

                if (FreePosition(newPosition) && player != null)
                {
                    var pion = new PionView(await player.CreatePion(), player);
                    pion.PionModel.Position = newPosition;
                    Pions.Add(pion);
                    boardView.Children.Add(pion);
                    await BoardGame.Play(player, pion.PionModel, newPosition);
                }

                if(countGraderThen0 && BoardGame.SelectedPion != null && (BoardGame.SelectedPion.Position.Row != newPosition.Row ||
                BoardGame.SelectedPion.Position.Column != newPosition.Column)) // for double-click, remove this condition if only want single click
                {
                    // row and col now correspond Grid's RowDefinition and ColumnDefinition mouse was 
                    // over when double clicked!
                    var pion = Pions.Where(O => O.PionModel.Name == BoardGame.SelectedPion.Name && O.PionModel.PionType == player.PionType).FirstOrDefault();
                    await BoardGame.Play(player, pion.PionModel, newPosition);
                    BoardGame.SelectedPion = null;
                    //           pion.Background = new SolidColorBrush(Colors.Red);
                }




            }
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

        private void GreedyCommandAction(object obj)
        {
            if (EngineType == EngineType.Greedy)
                EngineType = EngineType.BranchAndBound;
            else
                EngineType = EngineType.Greedy;
        }
        


        private void UpdateTime()
        {
            Task.Run(() =>
            {
              //  CurrentTime = DateTime.Now;
                if (AppStopwatch != null && AppStopwatch.IsRunning)
                    PlayTime = AppStopwatch.Elapsed;
                Task.Delay(1000);
                UpdateTime();
            });
        }

       

        private TimeSpan _currentTime;
        private TimeSpan _time;

        public TimeSpan CurrentTime
        {
            get { return _currentTime; }
            set {
                SetProperty(ref _currentTime, value);
            }
        }


        public TimeSpan PlayTime
        {
            get { return _time;}
            set
            {
                SetProperty(ref _time, value);
            }
        }


        private bool mulai;

        public bool Mulai
        {
            get { return mulai; }
            set { SetProperty(ref mulai ,value); }
        }

        private int _draw;

        public int Draw
        {
            get { return _draw; }
            set {SetProperty(ref _draw , value); }
        }

        private int player1;
        private int player2;
        private int _gameCount;

        public int Player1Win
        {
            get { return player1; }
            set { SetProperty(ref player1, value); }
        }

        public int Player2Win
        {
            get { return player2; }
            set { SetProperty(ref player2, value); }
        }


        public int GameCount
        {
            get { return _gameCount; }
            set { SetProperty(ref _gameCount, value); }
        }

        private EngineType engineType;

        public EngineType EngineType
        {
            get { return engineType; }
            set {SetProperty(ref engineType ,value); }
        }



        private Stopwatch AppStopwatch { get; set; }
        public CommandHandler MulaiCommand { get; private set; }
        public CommandHandler UlangCommand { get; private set; }
        public CommandHandler GreedyCommand { get; set; }
        public CommandHandler BranchCommand { get; set; }
        public CommandHandler StartCommand { get; }
        public CommandHandler RestartCommand { get; }
        public CommandHandler CloseCommand { get; }
        public Action WindowClose { get; internal set; }
    }


}
