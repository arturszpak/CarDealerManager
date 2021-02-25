using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSemestralny
{
    class CarDealerClass
    {

        public int ID_CAR { get; set; }
        public string CAR_MODEL { get; set; }
        public string CAR_COLOR { get; set; }
        public string CAR_CONDITION { get; set; }
        public string CAR_COUNTRY { get; set; }
        public int CAR_PRICE { get; set; }

        public CarDealerClass(TB_CAR car)
        {
            ID_CAR = car.ID_CAR;
            CAR_MODEL = car.TB_CAR_MODEL.CAR_MODEL;
            CAR_COLOR = car.TB_CAR_COLOR.COLOR;
            CAR_CONDITION = car.TB_CAR_CONDITION.CONDITION;
            CAR_COUNTRY = car.TB_CAR_COUNTRY.COUNTRY;
            CAR_PRICE = (int)car.CAR_PRICE;
        }
    }

    public class CarService
    {
        readonly CarDealerManagementDBEntities context = new CarDealerManagementDBEntities();

        public List<TB_CAR_MODEL> GetModels() => context.TB_CAR_MODEL.ToList();
        public List<TB_CAR_COLOR> GetColors() => context.TB_CAR_COLOR.ToList();
        public List<TB_CAR_COUNTRY> GetCountry() => context.TB_CAR_COUNTRY.ToList();
        public List<TB_CAR_CONDITION> GetCondition() => context.TB_CAR_CONDITION.ToList();

        public List<TB_CAR> GetList()
        {
            return context.TB_CAR
                .Include("TB_CAR_COLOR")
                .Include("TB_CAR_CONDITION")
                .Include("TB_CAR_MODEL")
                .Include("TB_CAR_COUNTRY")
                .ToList();
        }
    }
}


