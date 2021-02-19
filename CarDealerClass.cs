using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ProjektSemestralny
{
    class CarDealerClass
    {
        public int ID_CAR { get; set; }
        //public int ID_CAR_MODEL { get; set; }
        public string CAR_MODEL { get; set; }

        //public int ID_CAR_COLOR { get; set; }
        public string CAR_COLOR { get; set; }
        public string CAR_CONDITION { get; set; }
        public string CAR_COUNTRY { get; set; }

        //public int ID_CAR_CONDITION { get; set; }

        //public int ID_CAR_COUNTRY { get; set; }
        public decimal CAR_PRICE { get; set; }

        public CarDealerClass(TB_CAR car)
        {
            ID_CAR = car.ID_CAR;

            //ID_CAR_MODEL = (int)car.ID_CAR_MODEL;
            CAR_MODEL = car.TB_CAR_MODEL.CAR_MODEL;

            //ID_CAR_COLOR = (int)car.ID_CAR_COLOR;
            CAR_COLOR = car.TB_CAR_COLOR.COLOR;
            CAR_CONDITION = car.TB_CAR_CONDITION.CONDITION;
            CAR_COUNTRY = car.TB_CAR_COUNTRY.COUNTRY;

            //ID_CAR_CONDITION = (int)car.ID_CAR_CONDITION;
            //ID_CAR_COUNTRY = (int)car.ID_CAR_COUNTRY;
            CAR_PRICE = (int)car.CAR_PRICE;
        }
    }

    public class CarService
    {
        CarDealerManagementDBEntities context = new CarDealerManagementDBEntities();
 

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


