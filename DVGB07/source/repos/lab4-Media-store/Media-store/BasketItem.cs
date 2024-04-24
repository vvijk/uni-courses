using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_store {
    public class BasketItem {
        public Item Item { get; set; }

        private int _quantity;
        public int Quantity{
            get { return _quantity; }
            set {
                if (_quantity != value){
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }
        public string ItemName => Item?.Name;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
