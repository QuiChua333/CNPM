using System.Windows.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelManagement.ViewModel.AdminVM.HistoryManagementVM
{
    public class HistoryManagementVM : BaseVM
    {
        public ICommand FirstLoadCM { get; set; }
        public HistoryManagementVM()
        {
            FirstLoadCM = new RelayCommand<System.Windows.Controls.Page>((p) => { return true; }, (p) =>
            {

            });

        }
    }
}
