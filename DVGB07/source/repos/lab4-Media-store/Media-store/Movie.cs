using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Media_store
{
    internal class Movie : Item
    {
        public override string Type => "Movie";
        public string Format { get; set; }
        public int? Length { get; set; }

        public Movie(int PID, string Name, double Price, int Stock, string Format, int Length) : base(PID, Name, Price, Stock)
        {
            this.Format = Format;
            this.Length = Length;
        }
        public Movie(int PID, string Name, double Price, int Stock, string Format) : base(PID, Name, Price, Stock) {
            this.Format = Format;
            this.Length = null;
        }
        // What if only Length is applied?
        //public Movie(int PID, string Name, double Price, string Length) : base(PID, Name, Price) {
        //    this.Length = Length;
        //}
        public Movie(int PID, string Name, double Price, int Stock) : base(PID, Name, Price, Stock) {
            this.Format = null;
            this.Length = null;
        }
    }
}
