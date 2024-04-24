using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Media_store {
    public class Item {
        public int PID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public virtual string Type { get; } // virutal == declaring that it is desired to be overridden.
        public int Stock { get; set; }
        public Item(int PID, string Name, double Price, int Stock) {
            this.PID = PID;
            this.Name = Name;
            this.Price = Price;
            this.Stock = Stock;
        }
        public void UpdatePrice(double newPrice) {
            Price = newPrice;
        }
        public void UpdateStock(int newStock) {
            Stock = newStock;
        }
    }
}
