using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class RentalContractDetailDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public string RentalContractId { get; set; }
        public string RentalContractDetailId { get; set; }
        public string CustomerTypeId { get; set; }
        private string _CustomerName;
        public string CustomerName
        {
            get { return _CustomerName; }
            set
            {
                this._CustomerName = value;
                this.NotifyPropertyChanged("CustomerName");
            }
        }
        private int _STT;
        public int STT
        {
            get { return _STT; }
            set
            {
                this._STT = value;
                this.NotifyPropertyChanged("STT");
            }
        }

        private string _CCCD;
        public string CCCD
        {
            get { return _CCCD; }
            set
            {
                this._CCCD = value;
                this.NotifyPropertyChanged("CCCD");
            }
        }
        private string _Address;
        public string Address
        {
            get { return _Address; }
            set
            {
                this._Address = value;
                this.NotifyPropertyChanged("Address");
            }
        }
        private string _CustomerType;
        public string CustomerType
        {
            get { return _CustomerType; }
            set
            {
                this._CustomerType = value;
                this.NotifyPropertyChanged("CustomerType");
            }
        }
    }
}
