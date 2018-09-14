using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreedyLib
{
   public class Greedy
    {
        public Greedy(int[,] board,int boardLenght)
        {
            this.Board = board;
            BoardLengt= boardLenght;
            tableStrategy = new TableStrategy(boardLenght);
            TableView = new List<Item>();
            LoadAsync();
        }

        private void LoadAsync()
        {
            GetBaris(ref TableView);
            GetColumn(ref TableView);
            ToRightDiagonal(ref TableView);
            ToLeftDiagonal(ref TableView);
            this.HS = TableView.Where(O => O.Value > 0).ToList();
            FSK = new List<Item>();
            int value = 0;
            foreach (var item in HS.OrderByDescending(O => O.Value).ToList())
            {
                if (item.Value >= value)
                {
                    value = item.Value;
                    FSK.Add(item);
                }
                else
                    break;
            }

            FO = FSK.FirstOrDefault();
        }

        private void GetBaris(ref List<Item> baris)
        {
            for(var i=0;i<BoardLengt;i++)
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
                    baris.Add(new Item { Name="H"+(i+1), Value=result.Value });
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
                    columns.Add(new Item { Name = "V" + (j+1), Value = result.Value });
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
                columns.Add(new Item { Name = "D1", Value = result.Value });

        }

        private void ToLeftDiagonal(ref List<Item> columns)
        {
            var pemain = 0;
            var com = 0;

            int k = 0;
            for (var n = BoardLengt-1; n >=0; n--)
            {
               
                if (Board[k, n] == 1)
                    pemain++;

                if (Board[k, n] == 2)
                    com++;

                k++;

              
            }
            var result = tableStrategy.GetPrioritas(pemain, com);
            if (result != null)
                columns.Add(new Item { Name = "D2", Value = result.Value });
        }

        public int[,] Board { get; }
        public int BoardLengt { get; }
        public List<Item> HS { get; private set; }
        public List<Item> FSK { get; private set; }
        public Item FO { get; private set; }

        private TableStrategy tableStrategy;
        public List<Item> TableView;


    }



    public class TableStrategy
    {
        private List<Strategy> list;

        public TableStrategy(int board)
        {
            this.list = new List<Strategy>();
            if (board==3)
            {
              
                list.Add(new Strategy { Player1 = 0, Player2 = 0, Value = 0 });
                list.Add(new Strategy { Player1 = 1, Player2 = 0, Value = 10 });
                list.Add(new Strategy { Player1 = 2, Player2 = 0, Value = 100 });
                list.Add(new Strategy { Player1 = 1, Player2 = 1, Value = 1 });
                list.Add(new Strategy { Player1 = 0, Player2 = 1, Value = 20 });
                list.Add(new Strategy { Player1 = 0, Player2 = 2, Value = 200 });
            }
            else
            {
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
            }
        }


        public Strategy GetPrioritas(int p1, int p2)
        {
            return list.Where(O => O.Player1 == p1 && O.Player2 == p2).FirstOrDefault();
        }



    }

    public class Item
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class Strategy
    {
        public int Player1 { get; set; }
        public int Player2 { get; set; }
        public int Value { get; set; }
    }
    


}
