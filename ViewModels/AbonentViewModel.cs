using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using PhoneBook.Models;
using PhoneBook.DB;
using System.IO;

namespace PhoneBook.ViewModels
{
    public class AbonentViewModel : ViewModelBase
    {
        private readonly Abonent _abonent;

        public string FullName => _abonent.FullName;
        public string StreetName => _abonent.Address.Street.Name;
        public string HouseNumber => _abonent.Address.HouseNumber;
        public string HomePhoneNumber => GetPhoneNumberByType("Home");
        public string WorkPhoneNumber => GetPhoneNumberByType("Work");
        public string MobilePhoneNumber => GetPhoneNumberByType("Mobile");

        private string GetPhoneNumberByType(string type)
        {
            return _abonent.PhoneNumbers.FirstOrDefault(p => p.Type == type)?.Number;
        }

        public AbonentViewModel(Abonent abonent)
        {
            _abonent = abonent;
        }
    }
    }
