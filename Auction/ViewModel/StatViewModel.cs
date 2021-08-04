using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.ViewModel
{
    class StatViewModel: BaseViewModel
    {
        private int _totalPrice;
        public int TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        private int _bidCount;
        public int BidCount
        {
            get => _bidCount;
            set
            {
                _bidCount = value;
                OnPropertyChanged(nameof(BidCount));
            }
        }

        private int _soldCount;
        public int SoldCount
        {
            get => _soldCount;
            set
            {
                _soldCount = value;
                OnPropertyChanged(nameof(SoldCount));
            }
        }

        private int _clientCount;
        public int ClientCount
        {
            get => _clientCount;
            set
            {
                _clientCount = value;
                OnPropertyChanged(nameof(ClientCount));
            }
        }
        public StatViewModel() {}
        public StatViewModel(int totalPrice, int bidCount, int soldCount, int clientCount)
        {
            _totalPrice = totalPrice;
            _bidCount = bidCount;
            _soldCount = soldCount;
            _clientCount = clientCount;
        }
    }
}
