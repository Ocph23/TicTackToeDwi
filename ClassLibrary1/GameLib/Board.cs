using BranchAndBoundLib;
using GreedyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
    public delegate void delComputerStep(Player player, Position position);
    public delegate void delPlayerWin(Player player);
    public delegate void delGameDraw();
    public class Board:BaseNotify, IBoard
    {
        private Player _player1;
        public IMachine Engine { get; set; }
        public event delComputerStep ComputerOnPlaying;
        public event delPlayerWin OnPlayerWin;
        public event delGameDraw GameIsDraw;
      
        public void StartNewGame(int lenght)
        {
            BoardMatrix = lenght;
            Engine = Machine.CreateGreedy(BoardMatrix);
            Player1.Pions.Clear();
            Player2.Pions.Clear();
            Player1.IsPlay = true;
        }

        public Task Play(Player player, Pion pion, Position newPosition)
        {
            var board = GetBoardArray();
            bool draw = IsDraw(board);
            pion.SetPosition(newPosition);


            if(PlayerWin(player))
            {
                GameOver = true;
                OnPlayerWin?.Invoke(player);
            }else if(draw)
            {
                GameOver = true;
                GameIsDraw?.Invoke();
            }else if (Engine != null)
            {
                Engine.SetNewBoard(board);
            }
          
            SwichPlayer();

            return Task.FromResult(0);
        }

        public bool PlayerWin(Player playera)
        {
            int player = playera.Id;
            var board = GetBoardArray();
            int row = 0;
            int col = 0;
            int lright = 0;
            int rleft = 0;
            int z = BoardMatrix - 1;
            bool isWin = false;
            for (int i = 0; i < BoardMatrix; i++)
            {
                row = 0;
                col = 0;


                for (int j = 0; j < BoardMatrix; j++)
                {
                    if (board[i, j] == player)
                        row++;

                    if (board[j, i] == player)
                        col++;

                }

                if (board[i, i] == player)
                    lright++;

                if (board[i, z] == player)
                    rleft++;

                if (row == BoardMatrix || col == BoardMatrix || lright == BoardMatrix || rleft == BoardMatrix)
                {
                    isWin = true;
                    break;
                }
                z--;
            }

            return isWin;

        }

        private bool IsDraw(int[,] board)
        {
           bool draw = true;
           foreach(var item in board)
            {
                if(item<=0)
                {
                    draw = false;
                    break;
                }
            }
            if (draw)
                GameOver = true;
            return draw;
        }

        private int[,] GetBoardArray()
        {
            var result = new int[BoardMatrix, BoardMatrix];
           foreach(var item in Player1.Pions)
            {
                result[item.Position.Row, item.Position.Column] = 1;
            }

            foreach (var item in Player2.Pions)
            {
                result[item.Position.Row, item.Position.Column] = 2;
            }
            return result;
        }

        public Task SetPlayer(Player one, Player two)
        {
            Player1 = one;
            one.Id = 1;
            Player2 = two;
            two.Id = 2;

       
            return Task.FromResult(0);
        }

        public Task SwichPlayer()
        {


            if (Player1.IsPlay)
            {
                Player1.IsPlay = false;
                Player2.IsPlay = true;
               
            }
            else
            {
                Player1.IsPlay = true;
                Player2.IsPlay = false;
            }

            if (Player2.IsPlay && Player2.PlayerType == PlayerType.Computer)
            {
                ComputerPlaying(Player2);
            }

            return Task.FromResult(0);
        }

        public Tuple<string,Position,TimeSpan> GetBestPosition(EngineType engine)
        {
            Stopwatch sw = Stopwatch.StartNew();

            if (engine == EngineType.Greedy)
            {
                var result = Engine.GetSolution(this.GetBoardArray(), Player1.Id);
                var foPosition = Engine.GetBestPosition(Player1.Id);
                sw.Stop();
                ComputerOnPlaying?.Invoke(Player1, new Position(foPosition.Item1, foPosition.Item2));
                return new Tuple<string, Position,TimeSpan>(result, new Position(foPosition.Item1, foPosition.Item2),sw.Elapsed);
            }else
            {
                
                IMachine machine = Machine.CreateBranchAndBound(BoardMatrix);
                var result = Engine.GetSolution(this.GetBoardArray(), Player1.Id);
                var foPosition = Engine.GetBestPosition(Player1.Id);
                sw.Stop();
                ComputerOnPlaying?.Invoke(Player1, new Position(foPosition.Item1, foPosition.Item2));
                return new Tuple<string, Position, TimeSpan>(result, new Position(foPosition.Item1, foPosition.Item2), sw.Elapsed);
            }
        }

        public Task ComputerPlaying(Player player2)
        {
            Engine.SetNewBoard(this.GetBoardArray());
            var foPosition = Engine.GetBestPosition(Player2.Id);
            if(foPosition!=null)
                ComputerOnPlaying?.Invoke(player2, new Position(foPosition.Item1, foPosition.Item2));
            return Task.FromResult(0);
        }

        public Task<Player> GetPlayer(int Id)
        {
            if (Id == 1)
                return Task.FromResult(Player1);
            else
                return Task.FromResult(Player2);
        }

        public Task<Player> GetPlayerIsPlay()
        {
            if(Player1.Pions.Count<=0)
                return Task.FromResult(Player1);
            else if (Player1.IsPlay)
                return Task.FromResult(Player1);
            else if (Player2.IsPlay)
                return Task.FromResult(Player2);
            return null;
        }

        public Task<int> GetBoardCount()
        {
            return Task.FromResult(BoardMatrix);
        }

      //  private List<Pion> board =new List<Pion>();
        public Player Player1 {
            get { return _player1; }
            set { SetProperty(ref _player1, value); }
        }
        public Player Player2 { get; private set; }
        public int BoardMatrix { get; private set; }
        public Pion SelectedPion { get; set;  }
        public bool GameOver { get; private set; }
        
    }


    public interface IMachine
    {
        bool PlayerWin(Player player);
        void SetNewBoard(int[,] board);
        Tuple<int, int> GetBestPosition(int playerId);
        string GetSolution(int[,] v, int playerId);
        StringBuilder Report { get; set; }
    }


    public class Machine
    {
        public static IMachine CreateGreedy(int boardLenght)
        {
            return new GreedyMachine(boardLenght);
        }

        public static IMachine CreateBranchAndBound(int boardLenght)
        {
            return new BranchAndBoundMachine(boardLenght);
        }

        public static Board CreateNewGame()
        {
            return new Board();
        }

    }


    public class GreedyMachine : IMachine
    {
        private Greedy machine;

        public GreedyMachine(int boardLenght)
        {
            machine = new Greedy(boardLenght);
        }

        public string Result { get ; set ; }
        public StringBuilder Report { get; set; }

        public Tuple<int,int> GetBestPosition(int playerId)
        {
            return machine.GetPositionFromFOSelected();
        }

       
        public string GetSolution(int[,] v, int playerId)
        {
           var report= machine.GetSolution(v);
            return report;
        }

        public bool PlayerWin(Player player)
        {
            return machine.PlayerWin(player.Id);
        }

        public void SetNewBoard(int[,] board)
        {
            machine.SetNewBoard(board);
        }

       
    }


    public class BranchAndBoundMachine : IMachine
    {
        private int boardLenght;
        private BranchAndBound machine;

        public BranchAndBoundMachine(int boardLenght)
        {
            this.boardLenght = boardLenght;
            machine = new BranchAndBound(boardLenght);
        }

        public string Result { get; set; }
        public StringBuilder Report { get; set; }

        public Tuple<int, int> GetBestPosition(int playerId)
        {
            return machine.GetBestPosition(playerId);
        }

        public string GetSolution(int[,] v, int playerId)
        {
            throw new NotImplementedException();
        }

        public bool PlayerWin(Player player)
        {
            return machine.PlayerWin(player.Id);
        }

        public void SetNewBoard(int[,] board)
        {
            machine.SetNewBoard(board);
        }
    }
}
