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
using PhoneBook.ViewModels;

namespace PhoneBook
{
    /// <summary>
    /// Логика взаимодействия для StreetsWindow.xaml
    /// </summary>
    public partial class StreetsWindow : Window
    {
        private readonly DataContext _context;
        private ObservableCollection<StreetWindowViewModel> _streets;
        private StreetWindowViewModel _selectedStreet;

        public StreetsWindow(DataContext context)
        {
            _context = context;
            _streets = new ObservableCollection<StreetWindowViewModel>();

            InitializeComponent();
            var svm = new StreetWindowViewModel(_context);
            DataContext = svm;
           // LoadStreets();
        }

        //private void LoadStreets()
        //{
        //    var streets = _context.GetList<Street>().Select(s => new StreetViewModel(s)).ToList();
        //    _streets.Clear();
        //    foreach (var street in streets)
        //    {
        //        _streets.Add(street);
        //    }
           
        //}

        //private void AddButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var streetEditWindow = new StreetEditWindow(_context, null);
        //    streetEditWindow.ShowDialog();

        //    if (streetEditWindow.DialogResult == true)
        //    {
        //        LoadStreets();
        //    }
        //}

        //private void EditButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_selectedStreet == null)
        //    {
        //        return;
        //    }

        //    var streetEditWindow = new StreetEditWindow(_context, _selectedStreet);
        //    streetEditWindow.ShowDialog();

        //    if (streetEditWindow.DialogResult == true)
        //    {
        //        LoadStreets();
        //    }
        //}

        //private void DeleteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_selectedStreet == null)
        //    {
        //        return;
        //    }

        //    var result = MessageBox.Show($"Удалить улицу {_selectedStreet.Name}?", "Удаление улицы",
        //        MessageBoxButton.YesNo, MessageBoxImage.Question);

        //    if (result == MessageBoxResult.Yes)
        //    {
        //        _context.Streets.Delete(_selectedStreet.Street);
        //        _context.SaveChanges();

        //        LoadStreets();
        //    }
        //}

        //private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    _selectedStreet = (StreetViewModel)e.SelectedItems.FirstOrDefault();
        //    editButton.IsEnabled = _selectedStreet != null;
        //    deleteButton.IsEnabled = _selectedStreet != null;
        //}
    }

    public class StreetWindowViewModel : ViewModelBase
    {
      
        private readonly DataContext context;
        private ObservableCollection<StreetViewModel> _streets;
 

        public ObservableCollection<StreetViewModel> Streets
        {
            get => _streets;
            set => SetProperty(ref _streets, value);
        }

        public StreetWindowViewModel(DataContext _context)
        {
            context = _context;
            _streets=LoadStreets();
            //_street = street;
            //Name = street.Name;
        }

        private ObservableCollection<StreetViewModel> LoadStreets()
        {
            //var abonents = _context.Abonents

            //    .ToList();
            
            var streets = new ObservableCollection<Street> (context.GetList<Street>().ToList());
            var abonents = context.GetList<Abonent>().ToList();
            var adrs = context.GetList<Address>().ToList();
            for (int i = 0; i < abonents.Count(); i++)
            {
                abonents[i].Address = adrs.Where(a => a.Id == abonents[i].AddressId).FirstOrDefault();
              
            }
            var streetsViewModel= new ObservableCollection<StreetViewModel>(
                streets.Select(a => new StreetViewModel(a))
            );

            for(int i = 0; i < streetsViewModel.Count; i++)
            {
                foreach(var abonent in abonents)
                {
                    if (abonent.Address.StreetId == streetsViewModel[i]._street.Id)
                    {
                        streetsViewModel[i].CountAbonents++;
                    }
                }
              
            }

             return streetsViewModel;

        }
    }

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


