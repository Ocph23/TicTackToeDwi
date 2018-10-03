using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreedyLib
{
    internal class Item
    {
        private string _name;

        public string Name
        {
            get { return _name + Id; }
            set
            {
                _name = value;
            }
        }


        public int Value { get; internal set; }
        public ItemType ItemType { get; internal set; }
        public int Id { get; internal set; }
    }


    internal enum ItemType
    {
        Column,Row,DiagonalToRight, DiagonalToLeft
    }
}
