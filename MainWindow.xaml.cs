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
            _context = new DataContext("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\vs projects\\PhoneBook\\PhoneBookDB.mdf\";Integrated Security=True"); // Replace with your connection string
            _viewModel = new MainWindowViewModel(_context);
            _viewModel.FilteredAbonentsUpdated += OnFilteredAbonentsUpdated;
            InitializeComponent();

            DataContext = _viewModel;
            
        }

        private void ExportCsvCommand_Executed(object sender, RoutedEventArgs e)
        {
            // 1. Подготовка данных для экспорта
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
            csvBuilder.AppendLine("ФИО,Улица,Дом,Домашний,Рабочий,Мобильный");

            // 4. Добавление строк данных
            foreach (var row in data)
            {
                csvBuilder.AppendLine($"{row.FullName},{row.StreetName},{row.HouseNumber},{row.HomePhoneNumber},{row.WorkPhoneNumber},{row.MobilePhoneNumber}");
            }

            // 5. Сохранение CSV-строки в файл
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.FileName = "abonents.csv";

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var stream = new StreamWriter(saveFileDialog.FileName))
                {
                    stream.Write(csvBuilder.ToString());
                }

                MessageBox.Show("Данные успешно экспортированы в CSV-файл.", "Экспорт CSV", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchCommand_Executed(object sender, RoutedEventArgs e)
        {
            var searchWindow = new SearchNumberWindow(_context,_viewModel);
            searchWindow.Show();
        }

        private void OnFilteredAbonentsUpdated(object sender, ObservableCollection<AbonentViewModel> filteredAbonents)
        {
            // Update the data source of your table control with 'filteredAbonents'
            tablePhoneBook.ItemsSource = filteredAbonents;
         
        }
    }
}
