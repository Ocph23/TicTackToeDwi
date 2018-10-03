using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreedyLib
{
    public class Greedy
    {
        public Greedy(int boardLenght)
        {
            BoardLengt = boardLenght;
            TableView = new List<Item>();
            tableStrategy = new TableStrategy(boardLenght);
        }

        public void SetNewBoard(int[,] board)
        {
            Board = board;
            TableView.Clear();
            LoadAsync();
        }


        public string GetSolution(int[,] board)
        {
            SetNewBoard(board);
            return Report;
        }

        StringBuilder sb = new StringBuilder();
        private void LoadAsync()
        {
            sb.Clear();
            GetBaris(ref TableView);
            GetColumn(ref TableView);
            ToRightDiagonal(ref TableView);
            ToLeftDiagonal(ref TableView);
            this.HS = TableView.Where(O => O.Value > 0).ToList();
            FSK = new List<Item>();
            int value = 0;
     
            sb.AppendLine();
            sb.AppendLine("FSK");
            sb.AppendLine("Nama        Nilai");
            sb.AppendLine("-----------------");
            foreach (var item in HS.OrderByDescending(O => O.Value).ToList())
            {
                if (item.Value >= value)
                {
                    value = item.Value;
                    FSK.Add(item);

                    sb.AppendFormat("{0}        {1}", item.Name, item.Value);
                    sb.AppendLine();
                }
                else
                    break;
            }

            if (FSK.Count > 1)
            {
                FO = GetBestPosition(FSK);
            }
            else
                FO = FSK.FirstOrDefault();
           if(FO!=null)
            {
            
                sb.AppendLine();
                sb.AppendFormat("FO : {0} -{1}", FO.Name, FO.Value);
                sb.AppendLine();
                sb.AppendLine();
            }

            Report = sb.ToString();
        }

        private void GetBaris(ref List<Item> baris)
        {
            for (var i = 0; i < BoardLengt; i++)
            {
                var pemain = 0;
                var com = 0;
                for (var j = 0; j < BoardLengt; j++)
                {
                    if (Board[i, j] == 1)
                        pemain++;

                    if (Board[i, j] == 2)
                        com++;
                }

                var result = tableStrategy.GetPrioritas(pemain, com);
                if (result != null)
                    baris.Add(new Item { Name = "H", ItemType = ItemType.Row, Id = (i + 1), Value = result.Value });
            }
        }

        private void GetColumn(ref List<Item> columns)
        {
            for (var j = 0; j < BoardLengt; j++)
            {
                var pemain = 0;
                var com = 0;
                for (var i = 0; i < BoardLengt; i++)
                {
                    if (Board[i, j] == 1)
                        pemain++;

                    if (Board[i, j] == 2)
                        com++;
                }

                var result = tableStrategy.GetPrioritas(pemain, com);
                if (result != null)
                    columns.Add(new Item { ItemType = ItemType.Column, Name = "V", Id = (j + 1), Value = result.Value });
            }
        }

        private void ToRightDiagonal(ref List<Item> columns)
        {
            var pemain = 0;
            var com = 0;
            for (var n = 0; n < BoardLengt; n++)
            {

                if (Board[n, n] == 1)
                    pemain++;

                if (Board[n, n] == 2)
                    com++;


            }

            var result = tableStrategy.GetPrioritas(pemain, com);
            if (result != null)
                columns.Add(new Item { ItemType = ItemType.DiagonalToRight, Name = "D", Id = 1, Value = result.Value });

        }

        private void ToLeftDiagonal(ref List<Item> columns)
        {
            var pemain = 0;
            var com = 0;

            int k = 0;
            for (var n = BoardLengt - 1; n >= 0; n--)
            {

                if (Board[k, n] == 1)
                    pemain++;

                if (Board[k, n] == 2)
                    com++;

                k++;


            }
            var result = tableStrategy.GetPrioritas(pemain, com);
            if (result != null)
                columns.Add(new Item { ItemType = ItemType.DiagonalToLeft, Id = 2, Name = "D", Value = result.Value });
        }

        public IEnumerable GetGreedy()
        {
            return TableView;
        }

        public Tuple<int, int> GetPositionFromFOSelected()
        {
            Position pos = new Position(0, 0);
            var datas = GreedyHelper.GetFreePosition(Board, BoardLengt);
            if (FO != null && datas != null && datas.Count > 0)
            {

                switch (FO.ItemType)
                {
                    case ItemType.Column:
                        pos = datas.Where(O => O.Column == FO.Id - 1).FirstOrDefault();
                        break;

                    case ItemType.Row:
                        pos = datas.Where(O => O.Row == FO.Id - 1).FirstOrDefault();
                        break;
                    case ItemType.DiagonalToRight:
                        pos = datas.Where(O => O.Column == O.Row).FirstOrDefault();
                        break;
                    case ItemType.DiagonalToLeft:
                        pos = GreedyHelper.GetFreePositionRightToleft(Board, BoardLengt).FirstOrDefault();
                        break;
                    default:
                        break;
                }
                if (pos != null)
                    return new Tuple<int, int>(pos.Row, pos.Column);

            }
            return null;
        }

        internal Item GetBestPosition(List<Item> bestlist)
        {
            var diagonal = bestlist.Where(O => O.ItemType == ItemType.DiagonalToLeft || O.ItemType == ItemType.DiagonalToRight).FirstOrDefault();
            if (diagonal != null)
            {
                return diagonal;
            }
            else
            {
                Item[] newArray = bestlist.Where(O => O.ItemType != ItemType.DiagonalToLeft && O.ItemType != ItemType.DiagonalToRight).ToArray();

                return newArray[0];
            }
        }

        public IEnumerable GetHs()
        {
            return HS;
        }

        public IEnumerable GetFSK()
        {
            return this.FSK;
        }

        public string GetFO()
        {
            if (FO != null)
            {
                return FO.Name;
            }

            else
                return string.Empty;
        }

        public int[,] Board { get; set; }

        public int BoardLengt { get; }

        private List<Item> HS { get; set; }

        private List<Item> FSK { get; set; }

        private Item FO { get; set; }
        public string Report { get; private set; }

        private TableStrategy tableStrategy;

        private List<Item> TableView;

        public bool PlayerWin(int player)
        {
            int row = 0;
            int col = 0;
            int lright = 0;
            int rleft = 0;
            int z = BoardLengt-1;
            bool isWin = false;
            for(int i=0;i<BoardLengt;i++)
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

                if (row == BoardLengt||col==BoardLengt || lright==BoardLengt|| rleft==BoardLengt)
                {
                    isWin = true;
                    break;
                }
                z--;
            }

            return isWin;

        }

    }
    public class TableStrategy
    {
        private List<Strategy> list;

        public TableStrategy(int board)
        {
            this.list = new List<Strategy>();
            switch (board)
            {
                case 3:
                    list.Add(new Strategy { Player1 = 0, Player2 = 0, Value = 0 });
                    list.Add(new Strategy { Player1 = 1, Player2 = 0, Value = 10 });
                    list.Add(new Strategy { Player1 = 2, Player2 = 0, Value = 100 });
                    list.Add(new Strategy { Player1 = 1, Player2 = 1, Value = 1 });
                    list.Add(new Strategy { Player1 = 0, Player2 = 1, Value = 20 });
                    list.Add(new Strategy { Player1 = 0, Player2 = 2, Value = 200 });
                    break;
                case 4:
                    list.Add(new Strategy { Player1 = 0, Player2 = 0, Value = 0 });
                    list.Add(new Strategy { Player1 = 1, Player2 = 0, Value = 10 });
                    list.Add(new Strategy { Player1 = 2, Player2 = 0, Value = 100 });
                    list.Add(new Strategy { Player1 = 3, Player2 = 0, Value = 1000 });

                    list.Add(new Strategy { Player1 = 0, Player2 = 1, Value = 20 });
                    list.Add(new Strategy { Player1 = 0, Player2 = 2, Value = 200 });
                    list.Add(new Strategy { Player1 = 0, Player2 = 3, Value = 2000 });

                    list.Add(new Strategy { Player1 = 1, Player2 = 1, Value = 1 });
                    list.Add(new Strategy { Player1 = 1, Player2 = 2, Value = 1 });
                    list.Add(new Strategy { Player1 = 2, Player2 = 1, Value = 1 });
                    break;

                case 5:
                    list.Add(new Strategy { Player1 = 0, Player2 = 0, Value = 0 });
                    list.Add(new Strategy { Player1 = 1, Player2 = 0, Value = 10 });
                    list.Add(new Strategy { Player1 = 2, Player2 = 0, Value = 100 });
                    list.Add(new Strategy { Player1 = 3, Player2 = 0, Value = 1000 });
                    list.Add(new Strategy { Player1 = 4, Player2 = 0, Value = 10000 });

                    list.Add(new Strategy { Player1 = 0, Player2 = 1, Value = 20 });
                    list.Add(new Strategy { Player1 = 0, Player2 = 2, Value = 200 });
                    list.Add(new Strategy { Player1 = 0, Player2 = 3, Value = 2000 });
                    list.Add(new Strategy { Player1 = 0, Player2 = 4, Value = 20000 });

                    list.Add(new Strategy { Player1 = 1, Player2 = 1, Value = 1 });
                    list.Add(new Strategy { Player1 = 1, Player2 = 2, Value = 1 });
                    list.Add(new Strategy { Player1 = 1, Player2 = 3, Value = 1 });
                    list.Add(new Strategy { Player1 = 2, Player2 =1, Value = 1 });
                    list.Add(new Strategy { Player1 = 2, Player2 = 2, Value = 1 });
                    list.Add(new Strategy { Player1 = 2, Player2 = 3, Value = 1 });
                    list.Add(new Strategy { Player1 = 3, Player2 = 1, Value = 1 });
                    list.Add(new Strategy { Player1 = 3, Player2 = 2, Value = 1 });
                    list.Add(new Strategy { Player1 = 3, Player2 = 3, Value = 1 });
                    break;
                default:
                    break;
            }
           
        }

        public Strategy GetPrioritas(int p1, int p2)
        {
            return list.Where(O => O.Player1 == p1 && O.Player2 == p2).FirstOrDefault();
        }

    }

    

}
