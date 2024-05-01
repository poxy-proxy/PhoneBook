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
using Microsoft.Win32;
using System.Configuration;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DataContext _context;
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            _context = new DataContext(ConfigurationManager.ConnectionStrings["PBDBConnection"].ConnectionString); 
            _viewModel = new MainWindowViewModel(_context);
            _viewModel.FilteredAbonentsUpdated += OnFilteredAbonentsUpdated;
            InitializeComponent();

            DataContext = _viewModel;
            
        }

      

        private void SearchCommand_Executed(object sender, RoutedEventArgs e)
        {
            var searchWindow = new SearchNumberWindow(_context,_viewModel);
            searchWindow.Show();
        }

        private void OnFilteredAbonentsUpdated(object sender, ObservableCollection<AbonentViewModel> filteredAbonents)
        {
          
            tablePhoneBook.ItemsSource = filteredAbonents;
         
        }
    }
}
