using System;
using System.Collections.Generic;
using System.Text;

namespace SharedApp
{
   public class Position
    {
        public Position(int row, int column )
        {
            Row = row;
            Column = column;

        }

        public int Row { get; set; }
        public int Column { get; set; }
    }
}
