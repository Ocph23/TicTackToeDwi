using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BranchAndBoundLib
{
    public delegate void delBoundUpdateConsole(string message);
    public class BranchAndBound
    {
        public event delBoundUpdateConsole OnUpdateConsole;
        public BranchAndBound( int boardLenght)
        {
            BoardLengt = boardLenght;
            GoaldStates = new GoaldState();
            TableView = new List<Item>();
        }


        public void SetNewBoard(int[,] board)
        {
            this.Board = board;
        }

        public BoardItem GetSelectedBoard()
        {
            return new BoardItem(Board, BoardLengt);
        }
       
      
        public int[,] Board { get; private set; }
        public int BoardLengt { get; }
        public GoaldState GoaldStates { get; }
        private List<Item> TableView { get; }

        public bool PlayerWin(int player)
        {
            int row = 0;
            int col = 0;
            int lright = 0;
            int rleft = 0;
            int z = BoardLengt - 1;
            bool isWin = false;
            for (int i = 0; i < BoardLengt; i++)
            {
                row = 0;
                col = 0;


                for (int j = 0; j < BoardLengt; j++)
                {
                    if (Board[i, j] == player)
                        row++;

                    if (Board[j, i] == player)
                        col++;

                }

                if (Board[i, i] == player)
                    lright++;

                if (Board[i, z] == player)
                    rleft++;

                if (row == BoardLengt || col == BoardLengt || lright == BoardLengt || rleft == BoardLengt)
                {
                    isWin = true;
                    break;
                }
                z--;
            }

            return isWin;
        }

        public Tuple<int, int> GetBestPosition(int playerId)
        {
            var selectedBoard = GetSelectedBoard();
            var resul = selectedBoard.GetChildFor(playerId);
            foreach(var itemSelected in resul)
            {
                itemSelected.CalculateFor(playerId);
                UpdateConsole(itemSelected);
            }

            //Cek if Win Position
            BoardItem item = resul.Where(O => O.PlayerWinStep).FirstOrDefault();
            if(item != null)
                return new Tuple<int, int>(item.Position.Row, item.Position.Column);

            item = resul.Where(O => O.OpponenWinStep).FirstOrDefault();
            if (item != null)
                return new Tuple<int, int>(item.Position.Row, item.Position.Column);

            item = FindBestOption(resul);

            if(item!=null)
                return new Tuple<int, int>(item.Position.Row, item.Position.Column);
            return null;
        }

        private void UpdateConsole(BoardItem itemSelected)
        {
            StringBuilder sb = new StringBuilder();
       
            for (var i = 0; i<itemSelected.BoardLengt;i++)
            {
                sb.AppendLine();
                sb.Append("|");
                for (var j = 0; j < itemSelected.BoardLengt; j++)
                {
                    var result = itemSelected.Board[i, j];
                    if (result == 1)
                        sb.Append(" o |");
                    else if (result == 2)
                        sb.Append(" x |");
                    else
                        
                        sb.Append(" - |");
                }
       
            }
            sb.AppendLine();
            sb.AppendLine(string.Format("GN={0}-{1}", itemSelected.OpponentChance, itemSelected.PlayerChance));
            sb.AppendLine("CN=" + itemSelected.CN);
            sb.AppendLine();
            OnUpdateConsole?.Invoke(sb.ToString());
        }

        private BoardItem FindBestOption(List<BoardItem> resul)
        {
            List<BoardItem> bestOptions = new List<BoardItem>();
            bool isFirst = true;
            int i = 0;
            foreach (var data in resul.OrderBy(O => O.CN))
            {
                if (isFirst)
                {
                    i = data.CN;
                    isFirst = false;
                    bestOptions.Add(data);
                }
                else if (i == data.CN)
                {
                    bestOptions.Add(data);
                }
                else
                {
                    break;
                }
            }


            return bestOptions.OrderByDescending(O => O.PlayerChance).FirstOrDefault();
        }
    }

    public class BoardItem
    {
        public int[,] Board { get; }
        public int BoardLengt { get; }
        public int FN { get; private set; }
        public int CN { get; private set; }
        public bool PlayerWinStep { get; private set; }
        internal Position Position { get; private set; }
        public bool OpponenWinStep { get; private set; }
        public int PlayerChance { get; private set; }
        public int OpponentChance { get; private set; }

        public BoardItem(int[,] board,int lengt)
        {
            Board = board;
            BoardLengt = lengt;
        }


        public void CalculateFor(int playerId)
        {
            int MaxId = playerId;
            int MinId = 0;
            switch (playerId)
            {
                case 1:
                    MinId = 2;
                    break;
                case 2:
                    MinId = 1;
                    break;
                default:
                    break;
            }

            PlayerChance = KemunginanMenang(MaxId);
            OpponentChance = KemunginanMenang(MinId);
            FN = OpponentChance - PlayerChance;
            CN = CountOf(MinId)+FN;
            PlayerWinStep = PlayerWin(MaxId, Board);
            int[,] newBoar = (int[,])Board.Clone();
            newBoar[Position.Row, Position.Column] = MinId;
            OpponenWinStep = PlayerWin(MinId, newBoar);


        }

       

        public int KemunginanMenang(int id)
        {
            int menang = 0;
            for (var i = 0; i < BoardLengt; i++)
            {
                int hor = 0;
                int ver = 0;
                for (var j = 0; j < BoardLengt; j++)
                {
                    if (Board[i, j] == 0 || Board[i, j] == id)
                        hor++;
                    if (Board[j, i] == 0 || Board[j, i] == id)
                        ver++;
                }

                if (hor==BoardLengt)
                    menang++;
                if (ver==BoardLengt)
                    menang++;

            }

            //leftToright
            int m = BoardLengt - 1; ;
            int h = 0;
            int v = 0;
            for (var i = 0; i < BoardLengt; i++)
            {
                if (Board[i, i] == 0 || Board[i, i] == id)
                    h++;

                if (Board[i, m] == 0 || Board[i, m] == id)
                    v++;
                m--;
            }

            if (h==BoardLengt)
                menang++;
            if (v == BoardLengt)
                menang++;

            return menang;
        }

        public List<Tuple<int,int>> GetRuangKosong()
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            for (var i = 0; i < BoardLengt; i++)
            {
                for (var j = 0; j < BoardLengt; j++)
                {
                    if (Board[i, j] == 0)
                        list.Add(Tuple.Create(i,j));
                }
            }
            return list;
        }

        public List<BoardItem> GetChildFor(int Id)
        {
            List<BoardItem> items = new List<BoardItem>();
            foreach(var item in GetRuangKosong())
            {
                int[,] newBoard =(int[,]) Board.Clone();
                newBoard[item.Item1, item.Item2]=Id;
                var board = new BoardItem(newBoard,BoardLengt);
                board.SetSelectedPosition(item.Item1, item.Item2);
                items.Add(board);
            }
            return items;
        }

        private void SetSelectedPosition(int item1, int item2)
        {
            Position = new Position(item1, item2);
        }

        public int CountOf(int v)
        {
            int result = 0;
            foreach(var item in Board)
            {
                if(item==v)
                {
                    result++;
                }
            }

            return result;
        }


        public bool PlayerWin(int player,int[,] board)
        {
            int row = 0;
            int col = 0;
            int lright = 0;
            int rleft = 0;
            int z = BoardLengt - 1;
            bool isWin = false;
            for (int i = 0; i < BoardLengt; i++)
            {
                row = 0;
                col = 0;


                for (int j = 0; j < BoardLengt; j++)
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

                if (row == BoardLengt || col == BoardLengt || lright == BoardLengt || rleft == BoardLengt)
                {
                    isWin = true;
                    break;
                }
                z--;
            }

            return isWin;
        }

    }


}
