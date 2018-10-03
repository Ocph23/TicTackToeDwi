using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchAndBoundLib
{
   public class BranchAndBoundHelper
    {
        internal static List<Position> GetFreePosition(int[,] board, int lenght)
        {
            List<Position> list = new List<Position>();

            for (var i = 0; i < lenght; i++)
            {
                for (var j = 0; j < lenght; j++)
                {
                    if (board[i, j] == 0)
                        list.Add(new Position(i, j));
                }
            }

            return list;
        }
    }
}
