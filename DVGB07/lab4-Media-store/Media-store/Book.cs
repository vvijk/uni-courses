using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Store;

namespace Media_store
{
    public class Book : Item
    {
        public override string Type => "Book";
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Format { get; set; }
        public string Language { get; set; }

        public Book(int PID, string Name, double Price, int Stock, string Author, string Genre, string Format, string Language) : base(PID, Name, Price, Stock)
        {
            this.Author = Author;
            this.Genre = Genre;
            this.Format = Format;
            this.Language = Language;
        }
    }
}
