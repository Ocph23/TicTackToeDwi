using SharedApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
   public interface IBoard
    {
        Pion SelectedPion { get; set; }
        Task Play(Player player, Pion pion, Position newPosition);
        Task SetPlayer(Player one, Player two);
        Task SwichPlayer();
        Task<Player> GetPlayer(int Id);
        Task<Player> GetPlayerIsPlay();
    }
}
