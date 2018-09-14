using GreedyLib;
using SharedApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
    public class Board:IBoard
    {
        public static IBoard  CreateNewGame(int boardCunt)
        {
            MaxPion = boardCunt;
            return new Board(boardCunt);
        }

        public Task Play(Player player, Pion pion, Position newPosition)
        {
            bool canmove = true;
            if(SelectedPion!=null && SelectedPion.Name==pion.Name)
            {
                if(pion.Position.Row==newPosition.Row || pion.Position.Column == newPosition.Column || (pion.Position.Column==1 && pion.Position.Row== 1))
                {

                    if (canmove && pion.Position.Row > newPosition.Row && pion.Position.Row - 1 > newPosition.Row)
                        canmove = false;

                    if (canmove && pion.Position.Row < newPosition.Row && pion.Position.Row + 1 < newPosition.Row)
                        canmove = false;

                    if (canmove && pion.Position.Column > newPosition.Column && pion.Position.Column - 1 > newPosition.Column)
                        canmove = false;

                    if (canmove && pion.Position.Column < newPosition.Column && pion.Position.Column + 1 < newPosition.Column)
                        canmove = false;

                    if (canmove && pion.Position.Row == newPosition.Column && pion.Position.Column == newPosition.Row)
                        canmove = false;

                }
                else if( newPosition.Column!=1 || newPosition.Row!=1)
                {
                    canmove = false;
                }

            }

            if(canmove)
            {
                pion.SetPosition(newPosition);
                this.SwichPlayer();

                var board = GetBoardArray();
                this.GreedyView = new Greedy(board,MaxPion);


            }
            return Task.FromResult(0);
        }

        private int[,] GetBoardArray()
        {
            var result = new int[MaxPion, MaxPion];
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
            if (Player1.IsPlay)
                return Task.FromResult(Player1);
            else if (Player2.IsPlay)
                return Task.FromResult(Player2);
            return null;
        }

        public Task<int> GetBoardCount()
        {
            return Task.FromResult(MaxPion);
        }

        public IEnumerable GetGreedy()
        {
            return this.GreedyView.TableView;
        }

        public IEnumerable GetHs()
        {
            return this.GreedyView.HS;
        }

        public IEnumerable GetFSK()
        {
            return this.GreedyView.FSK;
        }

        public string GetFO()
        {
            return this.GreedyView.FO.Name;
        }

       

        private List<Pion> board;

        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public static int MaxPion { get; private set; }
        public Pion SelectedPion { get; set;  }
        public Greedy GreedyView { get; private set; }

        internal Board(int barisColumn)
        {
            board = new List<Pion>();
          
        }
    }
}
