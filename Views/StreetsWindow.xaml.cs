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

    }
}


