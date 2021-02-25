using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSemestralny
{
    public class AddressClass
    {
        public int ID { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string  ZIP { get; set; }

        public AddressClass(TB_ADDRESS adres)
        {
            this.ID = adres.ID_ADDRESS;
            this.Street = adres.STREET_NUMBER;
            this.City = adres.CITY;
            this.ZIP = adres.ZIP_CODE;
        }
    }

    public class AddressService
    {
        CarDealerManagementDBEntities context = new CarDealerManagementDBEntities();
        public List<TB_ADDRESS> GetList()
        {
            return context.TB_ADDRESS.ToList();
        }
    }
}
