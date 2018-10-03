using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchAndBoundLib
{

    public class GoaldState
    {
        bool IsGoal(List<Position> list, int boardLengt)
        {
            //horizontal
            var first = list.FirstOrDefault();
            if (list.Count == list.Where(O => O.Row == first.Row).Count())
                return true;

            //vertical
            if (list.Count == list.Where(O => O.Column == first.Column).Count())
                return true;

            //leftTorigtDiagonal
            if (list.Where(O => O.Row == O.Column).Count() == list.Count)
                return true;

            //rightToleft
            int count = 0;
            int j = list.Count - 1;
            for (var i = 0; i < list.Count; i++)
            {
                var result = list.Where(O => O.Row == i && O.Column == j).FirstOrDefault();
                if (result != null)
                {
                    count++;
                    j--;
                }
                else
                {
                    break;
                }
            }

            if (count == list.Count)
                return true;



            return false;
        }

    }
}
