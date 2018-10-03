using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib
{
    public class Position
    {
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; set; }
        public int Column { get; set; }

        public void SetPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
