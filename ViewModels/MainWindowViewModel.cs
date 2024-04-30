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
using Microsoft.Win32;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace PhoneBook.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly DataContext _context;
        private ObservableCollection<AbonentViewModel> _abonents;
        private string _searchNumber;
        private readonly ObservableCollection<AbonentViewModel> _allAbonents;
        private ObservableCollection<AbonentViewModel> _filteredAbonents;
        private string _searchText;
        public event EventHandler<ObservableCollection<AbonentViewModel>> FilteredAbonentsUpdated;

        public ObservableCollection<AbonentViewModel> Abonents
        {
            get => _abonents;
            set => SetProperty(ref _abonents, value);
        }

        public string SearchNumber
        {
            get => _searchNumber;
            set => SetProperty(ref _searchNumber, value);
        }

        public Command SearchCommand => new Command(SearchAbonents);
        public Command ExportCsvCommand => new Command(ExportCsv);
        public Command ShowStreetsCommand => new Command(ShowStreets);

        public MainWindowViewModel(DataContext context)
        {
           
            _context = context;
            LoadAbonents();
            _allAbonents = new ObservableCollection<AbonentViewModel>(GetAbonents().Select(s => new AbonentViewModel(s)));
            _filteredAbonents = new ObservableCollection<AbonentViewModel>(_allAbonents);
            SearchCommandUpdate = new RelayCommand(SearchImpl);
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

        public RelayCommand SearchCommandUpdate { get; }

        private void SearchImpl()
        {
            var searchText = SearchText?.ToLowerInvariant();

            _filteredAbonents.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                foreach (var ab in _allAbonents)
                {
                    _filteredAbonents.Add(ab);
                }

                return;
            }

            var filteredAbonents = _allAbonents.Where(a =>
               (a.MobilePhoneNumber != null && a.MobilePhoneNumber.ToLowerInvariant().Contains(searchText)) ||
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
                //FilteredAbonentsUpdated = new EventHandler().Invoke(this, _filteredAbonents);
                FilteredAbonentsUpdated(this, _filteredAbonents);
              
            }
        }

        private void LoadAbonents()
        {
            //var abonents = _context.Abonents

            //    .ToList();
            var abonents = _context.GetList<Abonent>().ToList();
            var adrs= _context.GetList<Address>().ToList();
            var streets= _context.GetList<Street>().ToList();
            var phones = _context.GetList<PhoneNumber>().ToList();
            for (int i = 0; i < abonents.Count(); i++)
            {
                abonents[i].Address = adrs.Where(a => a.Id == abonents[i].AddressId).FirstOrDefault();
                abonents[i].Address.Street = streets.Where(s => s.Id == abonents[i].Address.StreetId).FirstOrDefault();
                abonents[i].PhoneNumbers = phones.Where(p => p.AbonentId == abonents[i].Id).ToList();
            }
            Abonents = new ObservableCollection<AbonentViewModel>(
                abonents.Select(a => new AbonentViewModel(a))
            );
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

        private void SearchAbonents()
        {
            //var filteredAbonents = GetAbonents()
            //    //.Include(a => a.Address)
            //    //.Include(a => a.PhoneNumbers)
            //    .Where(a => a.PhoneNumbers.Any(p => p.Number.Contains(SearchNumber)))
            //    .ToList();

            //Abonents = new ObservableCollection<AbonentViewModel>(
            //    filteredAbonents.Select(a => new AbonentViewModel(a))
            //);
            new SearchNumberWindow(_context,this).Show();
        }



        private void ExportCsv()
        {

            // 1. Подготовка данных для экспорта
            var _viewModel = new MainWindowViewModel(_context);
            var data = _viewModel.Abonents.Select(abonent => new
            {
                FullName = abonent.FullName,
                StreetName = abonent.StreetName,
                HouseNumber = abonent.HouseNumber,
                HomePhoneNumber = abonent.HomePhoneNumber,
                WorkPhoneNumber = abonent.WorkPhoneNumber,
                MobilePhoneNumber = abonent.MobilePhoneNumber
            }).ToList();

            // 2. Создание объекта StringBuilder для хранения CSV-строк
            var csvBuilder = new StringBuilder();

            // 3. Добавление заголовков столбцов
            csvBuilder.AppendLine("ФИО;Улица;Дом;Домашний;Рабочий;Мобильный");

            // 4. Добавление строк данных
            foreach (var row in data)
            {
                // Итерация по значениям в строке
                foreach (var property in row.GetType().GetProperties())
                {
                    var value = property.GetValue(row);
                    csvBuilder.Append($"{value};");
                }

                // Удаление последней запятой (из-за последнего свойства)
                csvBuilder.Remove(csvBuilder.Length - 1, 1);

                // Добавление перехода на новую строку
                csvBuilder.AppendLine();
            }

            // 5. Сохранение CSV-строки в файл
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.FileName = "report.csv";

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var stream = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.UTF8))
                {

                    stream.WriteLine(csvBuilder.ToString());
                }
                MessageBox.Show("Данные успешно экспортированы в CSV-файл.", "Экспорт CSV", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ShowStreets()
        {
            new StreetsWindow(_context).Show();
            // TODO: Implement ShowStreets method to display streets and subscriber counts
        }
    }
}
