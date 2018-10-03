using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreedyLib
{
   public class GreedyHelper
    {
        internal static List<Position> GetFreePosition(int[,] board,int lenght)
        {
            List<Position> list = new List<Position>();

            for (var i= 0; i < lenght; i++)
            {
                for (var j = 0; j < lenght; j++)
                {
                    if (board[i, j] == 0)
                        list.Add(new Position(i, j));
                }
            }

            return list;
        }


        internal static List<Position> GetFreePositionRightToleft(int[,] board, int lenght)
        {
            List<Position> list = new List<Position>();
            int j = lenght - 1;
            for (var i = 0; i < lenght; i++)
            {
                if (board[i, j] == 0)
                    list.Add(new Position(i, j));
                j--;
            }
            return list;
        }
    }
}
