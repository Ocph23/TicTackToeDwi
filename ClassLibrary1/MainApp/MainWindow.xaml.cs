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
        private string guid = Guid.NewGuid().ToString();
        public MainWindow(PlayerModel player)
        {
            InitializeComponent();
            vm = new AppTime(boardView, guid) { WindowClose=Close};
            vm.OnUpdateConsole += Vm_OnUpdateConsole;
            DataContext =vm;
            vm.GameMode = GameMode.SinglePlayer;
            vm.SetPlayer(player);
            vm.SelectedPlayer = new PlayerModel() {Id=player.Id,Name=player.Name, Score=player.Score };
            board33.IsChecked = true;
        }

        public MainWindow(string player1, string player2)
        {
            InitializeComponent();
            vm = new AppTime(boardView, guid) { WindowClose = Close }; ;
            vm.OnUpdateConsole += Vm_OnUpdateConsole;
            DataContext = vm;
            vm.GameMode = GameMode.MultiPlayer;
            vm.SetPlayer(player1, player2);
            board33.IsChecked = true;
        }

        private void Vm_OnUpdateConsole(string message)
        {
            console.AppendText(message);
            console.ScrollToEnd();
        }

        private void boardView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            vm.PreviewMouseLeftButtonDown(e.ClickCount > 0);
        }
       

        private void ulang_Click(object sender, RoutedEventArgs e)
        {
            console.Document.Blocks.Clear();
           // vm.StartCommand.Execute(null);
        }
        

        private void board33_Checked(object sender, RoutedEventArgs e)
        {
            rowHeader.RowDefinitions.Clear();
            colomHeader.ColumnDefinitions.Clear();
            vm.BoardLength = 3;
            for(int i =0; i<vm.BoardLength;i++)
            {
                rowHeader.RowDefinitions.Add(new RowDefinition());
                colomHeader.ColumnDefinitions.Add(new ColumnDefinition());
                var item = new Label() {  Content = "V" + (i+1), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment = VerticalAlignment.Center };
                item.Foreground = new BrushConverter().ConvertFromString("#FF26A3D3") as SolidColorBrush;
                Grid.SetColumn(item, i);
                this.colomHeader.Children.Add(item);
                var item1 = new Label() { Content = "H" + (i+1), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment = VerticalAlignment.Center };
                Grid.SetRow(item1, i);
                this.rowHeader.Children.Add(item1);
            }

            console.Document.Blocks.Clear();
            vm.LoadBoard(vm.BoardLength);
        }

        private void board44_Checked(object sender, RoutedEventArgs e)
        {
            rowHeader.RowDefinitions.Clear();
            colomHeader.ColumnDefinitions.Clear();
            console.Document.Blocks.Clear();
            vm.BoardLength = 4;
            for (int i = 0; i < vm.BoardLength; i++)
            {
                rowHeader.RowDefinitions.Add(new RowDefinition());
                colomHeader.ColumnDefinitions.Add(new ColumnDefinition());
                var item = new Label() { Content = "V" + (i + 1), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment = VerticalAlignment.Center };
                item.Foreground = new BrushConverter().ConvertFromString("#FF26A3D3") as SolidColorBrush;
                Grid.SetColumn(item, i);
                this.colomHeader.Children.Add(item);
                var item1 = new Label() { Content = "H" + (i + 1), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment = VerticalAlignment.Center };
                Grid.SetRow(item1, i);
                this.rowHeader.Children.Add(item1);
            }
            vm.LoadBoard(vm.BoardLength);
        }

        private async void btnGreedy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var capture = new Models.CaptureGreedyModel();
                capture.Guid = guid;
                capture.Board = vm.BoardLength;
                string fileGuid = Guid.NewGuid().ToString();

                if (chkCapture.IsChecked==true)
                {
                     capture.awal= await CapurePictureAsync(true, capture, fileGuid);
                }
                var result = vm.BoardGame.GetBestPosition(EngineType.Greedy);
                console.AppendText("Greedy \r" + result.Item1);
                console.AppendText("Posisi : " + result.Item2.Row + "," + result.Item2.Column + "\r\n\r\n");
                vm.CurrentTime = result.Item3;
                console.ScrollToEnd();
                if (chkCapture.IsChecked == true)
                {
                    capture.Time = vm.CurrentTime;
                    CapurePictureAsync(false, capture, fileGuid);
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private async Task<string> CapurePictureAsync(bool isBefore,  ICapture capture, string fileGuid)
        {
            await Task.Delay(1000);
            System.Drawing.Image b = null;
            System.Drawing.Size sz = new System.Drawing.Size((int)ActualWidth, (int)ActualHeight);
            System.Drawing.Point loc = new System.Drawing.Point((int)Left+300, (int)Top+50);
            using (b = new System.Drawing.Bitmap(505, 580))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b))
                {
                    g.CopyFromScreen(loc, new System.Drawing.Point(0, 0), sz);
                }

                System.Drawing.Image x = new System.Drawing.Bitmap(b);

                ImageBrush myBrush = new ImageBrush();
                string file = string.Empty;
                if (isBefore)
                    file = $"awal-{fileGuid}.jpeg";
                else
                    file = $"akhir-{fileGuid}.jpeg";

                x.Save(AppDomain.CurrentDomain.BaseDirectory + "Captures\\" +file, System.Drawing.Imaging.ImageFormat.Png);

                if(!isBefore)
                {
                    capture.akhir = file;
                    capture.Save();
                    chkCapture.IsChecked = false;
                }
                return file;
            }
        }

        private async void btnBB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var capture = new Models.CaptureBandBModel();
                capture.Guid = guid;
                capture.Board = vm.BoardLength;
                string fileGuid = Guid.NewGuid().ToString();
                if (chkCapture.IsChecked == true)
                {
                    capture.awal = await CapurePictureAsync(true, capture, fileGuid);
                }
                var result = vm.BoardGame.GetBestPosition(EngineType.BranchAndBound);
                console.AppendText("Branch & Bound \r" + result.Item1);
                console.AppendText("Posisi : " + result.Item2.Row + "," + result.Item2.Column + "\r\n\r\n");
                vm.CurrentTime = result.Item3;
                console.ScrollToEnd();

                if (chkCapture.IsChecked == true)
                {
                    capture.Time = vm.CurrentTime;
                    CapurePictureAsync(false, capture, fileGuid);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

    }

    public class AppTime:Ocph.DAL.BaseNotify
    {
        public event delegateUpdateConsole OnUpdateConsole;
        public Board BoardGame { get; set; }
        public List<PionView> Pions { get; } = new List<PionView>();

        private string guid;
        private Grid boardView;
        public PlayerModel SelectedPlayer { get; set; }
        public GameMode GameMode { get; set; }
        public int BoardLength { get; set; }

        private bool caprureBoard;

        public bool IsCaptureBoard
        {
            get {
                if (GameCount > 0)
                    caprureBoard= false;
                else
                    caprureBoard = true;
                return caprureBoard; }
            set
            {
                SetProperty(ref caprureBoard, value);
            }
           
        }


        public AppTime(Grid board, string guid)
        {
            this.guid = guid;
            boardView = board;
            BoardGame = Machine.CreateNewGame();
            BoardGame.OnPlayerWin += Game_OnPlayerWin;
            BoardGame.ComputerOnPlaying += Game_ComputerOnPlaying;
            BoardGame.GameIsDraw += BoardGame_GameIsDraw;

            GreedyCommand = new CommandHandler { CanExecuteAction = x => Mulai, ExecuteAction = GreedyCommandAction };
            BranchCommand = new CommandHandler { CanExecuteAction = x => Mulai, ExecuteAction = GreedyCommandAction };

            StartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = StartCommandAction };
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
                if (GameMode == GameMode.SinglePlayer && SelectedPlayer!=null && SelectedPlayer.Score == null)
                {
                    SelectedPlayer.Score = new ScoreModel
                    {
                        ComputerWin = Player2Win,
                        PlayerWin = Player1Win,
                        Draw = Draw,
                        PlayerId = SelectedPlayer.Id.Value,
                        Time = PlayTime
                    };


                    SelectedPlayer.SaveScore(board3,board4);
                }
                WindowClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private ScoreModel board3;

        private ScoreModel board4;

        
                    
        private void Game_OnPlayerWin(Player player)
        {
            Stop();
            Mulai = false;
            SetScore(player,"WIN");
            MessageBox.Show(player.Name + " Win");

        }

        private void SetScore(Player player, string v)
        {
            if ((board3 == null || board4 == null)&&SelectedPlayer!=null)
            {
                board3 = new ScoreModel() { PlayerId = SelectedPlayer.Id.Value, Board = 3, GuidData=guid };
                board4 = new ScoreModel() { PlayerId = SelectedPlayer.Id.Value, Board = 4 , GuidData = guid };
            }

            if (SelectedPlayer==null)
            {
                board3 = new ScoreModel() {  Board = 3 };
                board4 = new ScoreModel() { Board = 4 };
            }

            if (v == "WIN")
            {
                if (BoardLength == 3)
                {
                    if (player.Id == 1)
                    {
                        Player1Win++;
                        board3.PlayerWin++;
                    }
                    else
                    {
                        Player2Win++;
                        board3.ComputerWin++;
                    }
                    board3.Time += PlayTime;
                }
                else
                {
                    if (player.Id == 1)
                    {
                        Player1Win++;
                        board4.PlayerWin++;
                    }
                    else
                    {
                        Player2Win++;
                        board4.ComputerWin++;
                    }
                    board4.Time += PlayTime;
                }

            }
            else
            {
                Draw++;
                if (BoardLength == 3)
                {
                    board3.Draw++;
                    board3.Time += PlayTime;
                }
                else
                {
                    board4.Draw++;
                    board4.Time += PlayTime;
                }
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

            
            if(GameCount>0 && Mulai==true)
            {
                this.RestartCommand.Execute(null);
            }
            else
            {
                GameCount++;
                Mulai = true;
                LoadBoard(BoardLength);
               
            }
          
        }

        internal async void SetPlayer(string player1, string player2)
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
            SetScore(null, "Draw");
        }

        //Event Action
        private void BoardGame_OnChangeConsole(string text)
        {
            OnUpdateConsole?.Invoke(text);
        }



      
        private async void Game_ComputerOnPlaying(Player player, Position position)
        {
            if (FreePosition(position) && Mulai)
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
            if(Mulai && Pions.Count<=0)
                AppStopwatch = Stopwatch.StartNew();

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



        private void Caprure()
        {
          
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
            set {
                IsCaptureBoard = false;
                SetProperty(ref _gameCount, value);
                
            }
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
