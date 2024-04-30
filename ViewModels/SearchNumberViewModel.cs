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
    public class SearchNumberViewModel : ViewModelBase
    {
        private readonly DataContext _context;
        private readonly ObservableCollection<AbonentViewModel> _allAbonents;
        private ObservableCollection<AbonentViewModel> _filteredAbonents;
        private string _searchText;
        public event EventHandler<ObservableCollection<AbonentViewModel>> FilteredAbonentsUpdated;

        public SearchNumberViewModel(DataContext context)
        {
            _context = context;
            _allAbonents = new ObservableCollection<AbonentViewModel>(GetAbonents().Select(s => new AbonentViewModel(s)));
            _filteredAbonents = new ObservableCollection<AbonentViewModel>(_allAbonents);
            SearchCommand = new RelayCommand(SearchImpl);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        public ObservableCollection<AbonentViewModel> FilteredAbonents => _filteredAbonents;

        public RelayCommand SearchCommand { get; }

        private void SearchImpl()
        {
            var searchText = SearchText?.ToLowerInvariant();

            _filteredAbonents.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                foreach(var ab in _allAbonents)
                {
                    _filteredAbonents.Add(ab);
                }

                return;
            }

            var filteredAbonents = _allAbonents.Where(a =>
               (a.MobilePhoneNumber!=null && a.MobilePhoneNumber.ToLowerInvariant().Contains(searchText)) ||
                (a.HomePhoneNumber != null && a.HomePhoneNumber.ToLowerInvariant().Contains(searchText)) ||
                (a.WorkPhoneNumber != null && a.WorkPhoneNumber.ToLowerInvariant().Contains(searchText))
            );

            if (filteredAbonents.Count() == 0)
            {
                MessageBox.Show("Нет абонентов, удовлетворяющих критерию поиска.", "Поиск по номеру",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                foreach (var fa in filteredAbonents)
                {
                    _filteredAbonents.Add(fa);
                }

                FilteredAbonentsUpdated?.Invoke(this, _filteredAbonents);
            }
        }

        private List<Abonent> GetAbonents()
        {
            var abonents = _context.GetList<Abonent>().ToList();
            var adrs = _context.GetList<Address>().ToList();
            var streets = _context.GetList<Street>().ToList();
            var phones = _context.GetList<PhoneNumber>().ToList();
            for (int i = 0; i < abonents.Count(); i++)
            {
                abonents[i].Address = adrs.Where(a => a.Id == abonents[i].AddressId).FirstOrDefault();
                abonents[i].Address.Street = streets.Where(s => s.Id == abonents[i].Address.StreetId).FirstOrDefault();
                abonents[i].PhoneNumbers = phones.Where(p => p.AbonentId == abonents[i].Id).ToList();
            }
            return abonents;
        }
    }
}
