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
using System.Windows.Controls;

namespace PhoneBook
{
    public partial class SearchNumberWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public SearchNumberWindow(DataContext context,MainWindowViewModel viewModel)//SearchNumberViewModel viewModel)
        {
            InitializeComponent();
            //_viewModel = viewModel;
            DataContext = context;
            _viewModel = viewModel;
            searchButton.Command = _viewModel.SearchCommandUpdate;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.SearchText = searchTextBox.Text;
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = "";
            _viewModel.SearchCommandUpdate.Execute(sender);
        }
    }
}
