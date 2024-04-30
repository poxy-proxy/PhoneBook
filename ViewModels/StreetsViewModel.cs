using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using PhoneBook.Models;
using PhoneBook.DB;

namespace PhoneBook.ViewModels
{
    public class StreetViewModel : ViewModelBase
    {
        public Street _street { get; set; }

        public string Name => _street.Name;

        public int CountAbonents { get; set; } = 0;

        public StreetViewModel(Street street)
        {
            _street = street;
        }
    }
}
