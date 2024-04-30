using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int StreetId { get; set; }

        public Street Street { get; set; }
        public string HouseNumber { get; set; }

        public Address()
        {
            Street = new Street();
        }
    }
}
