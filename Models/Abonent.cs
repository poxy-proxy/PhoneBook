using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    public class Abonent
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public string FullName { get; set; }
        public Address Address { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }

        public Abonent()
        {
            Address = new Address();
            PhoneNumbers = new List<PhoneNumber>();
        }
    }
}
