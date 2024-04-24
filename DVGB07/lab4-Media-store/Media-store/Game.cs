using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Media_store
{
    internal class Game : Item
    {
        public override string Type => "Game";
        public string Platform { get; set; }

        public Game(int PID, string Name, double Price, int Stock, string Platform) : base(PID, Name, Price, Stock)
        {
            this.Platform = Platform;
        }
    }
}

