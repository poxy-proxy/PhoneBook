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
            _streets = LoadStreets();
            //_street = street;
            //Name = street.Name;
        }

        private ObservableCollection<StreetViewModel> LoadStreets()
        {
          

            var streets = new ObservableCollection<Street>(context.GetList<Street>().ToList());
            var abonents = context.GetList<Abonent>().ToList();
            var adrs = context.GetList<Address>().ToList();
            for (int i = 0; i < abonents.Count(); i++)
            {
                abonents[i].Address = adrs.Where(a => a.Id == abonents[i].AddressId).FirstOrDefault();

            }
            var streetsViewModel = new ObservableCollection<StreetViewModel>(
                streets.Select(a => new StreetViewModel(a))
            );

            for (int i = 0; i < streetsViewModel.Count; i++)
            {
                foreach (var abonent in abonents)
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
}
