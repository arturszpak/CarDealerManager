using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSemestralny
{
    public class TransactionManager
    {
        public int ID { get; set; }
        public string Client { get; set; }
        public string Car { get; set; }
        public DateTime DateOfTransaction { get; set; }

        public TransactionManager(TB_TRANSACTIONS transaction)
        {
            string clientBuilder = $"{transaction.TB_CLIENT.NAME}, " +
                                   $"{transaction.TB_CLIENT.SURNAME}, " +
                                   $"pesel: {transaction.TB_CLIENT.PESEL}, " +
                                   $"nip: {transaction.TB_CLIENT.NIP}, " +
                                   $"Address: {transaction.TB_CLIENT.TB_ADDRESS.STREET_NUMBER}," +
                                   $" {transaction.TB_CLIENT.TB_ADDRESS.CITY}," +
                                   $" {transaction.TB_CLIENT.TB_ADDRESS.ZIP_CODE}";

            string carBuilder = $"{transaction.TB_CAR.TB_CAR_MODEL.CAR_MODEL}, " +
                                  $"color: {transaction.TB_CAR.TB_CAR_COLOR.COLOR}, " +
                                  $"condition: {transaction.TB_CAR.TB_CAR_CONDITION.CONDITION}, " +
                                  $"country: {transaction.TB_CAR.TB_CAR_COUNTRY.COUNTRY}";

            this.ID = transaction.ID_TRANSACTION;
           
            this.Client = clientBuilder;
            this.Car = carBuilder;
            this.DateOfTransaction = transaction.TRANSACTION_DATE;
        }
    }
    public class TransactionService
    {
        CarDealerManagementDBEntities context = new CarDealerManagementDBEntities();
        public List<TB_CAR_MODEL> GetModels() => context.TB_CAR_MODEL.ToList();
        public List<TB_CAR_COLOR> GetColors() => context.TB_CAR_COLOR.ToList();
        public List<TB_CAR_COUNTRY> GetCountry() => context.TB_CAR_COUNTRY.ToList();
        public List<TB_CAR_CONDITION> GetCondition() => context.TB_CAR_CONDITION.ToList();

        public List<TB_CAR> GetCars()
        {
            return context.TB_CAR
                .Include("TB_CAR_MODEL")
                .Include("TB_CAR_COLOR")
                .Include("TB_CAR_CONDITION")
                .Include("TB_CAR_COUNTRY")
                .ToList();
        }
        public List<TB_CLIENT> GetClients() => context.TB_CLIENT.ToList();

        public List<TB_TRANSACTIONS> GetList()
        {
            return context.TB_TRANSACTIONS
                .Include("TB_CLIENT")
                .Include("TB_CAR")
                .ToList();
        }
    }
}
