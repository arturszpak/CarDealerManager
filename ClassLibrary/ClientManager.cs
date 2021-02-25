using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSemestralny
{
    public class ClientManager
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PESEL { get; set; }
        public string NIP { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string ZIPCODE { get; set; }

        public ClientManager(TB_CLIENT client)
        {
            ID = client.ID_CLIENT;
            Name = client.NAME;
            Surname = client.SURNAME;
            PESEL = client.PESEL;
            NIP = client.NIP.ToString() ?? "";
            StreetNumber = client.TB_ADDRESS.STREET_NUMBER;
            City = client.TB_ADDRESS.CITY;
            ZIPCODE = client.TB_ADDRESS.ZIP_CODE;
        }
    }

    public class ClientService
    {
        readonly CarDealerManagementDBEntities db = new CarDealerManagementDBEntities();

        public List<TB_ADDRESS> GetAddresses() => db.TB_ADDRESS.ToList();

        public List<TB_CLIENT> GetList()
        {
            return db.TB_CLIENT
                .Include("TB_ADDRESS")
                .ToList();
        }
    }

}
