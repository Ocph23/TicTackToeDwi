using SharedApp;
using System;
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
            pion.SetPosition(newPosition);
            this.SwichPlayer();
            return Task.FromResult(0);
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

        private List<Pion> board;

        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public static int MaxPion { get; private set; }
        public Pion SelectedPion { get; set;  }

        internal Board(int barisColumn)
        {
            board = new List<Pion>();
          
        }
    }
}
