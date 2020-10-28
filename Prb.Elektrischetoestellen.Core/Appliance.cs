using System;
using System.Collections.Generic;
using System.Text;

namespace Prb.Elektrischetoestellen.Core
{
    //public enum ApplianceTypes
    //{
    //    Vaatwasser, Oven, Fornuis, Koelkast, Diepvries, Boiler, TV 
    //}
    public class Appliance
    {
        private string applianceType;
        private string applianceBrand;
        private int applianceConsumption;
        private decimal appliancePrice;

        public string ApplianceType
        {
            get { return applianceType; }
            set
            {
                value = value.Trim();
                if (value == "")
                {
                    throw new Exception("Je dient een type op te geven !");
                }
                else
                {
                    applianceType = value;
                }
            }
        }

        public string ApplianceBrand
        {
            get { return applianceBrand; }
            set 
            {
                value = value.Trim();
                if (value == "")
                {
                    throw new Exception("Je dient een merk op te geven !");
                }
                else
                {
                    applianceBrand = value;
                }
            }
        }

        public int ApplianceConsumption
        {
            get { return applianceConsumption; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Je dient een verbruik op te geven !");
                }
                else
                {
                    applianceConsumption = value;
                }
            }
        }


        public decimal AppliancePrice
        {
            get { return appliancePrice; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Je dient een correcte prijs op te geven (gebruik kommas, geen punten voor decimalen) !");
                }
                else
                {
                    appliancePrice = value;
                }
            }
        }
        public Appliance()
        {

        }
        public Appliance(string applianceType, string applianceBrand, int applianceConsumption, decimal appliancePrice)
        {
            // we vullen de props zelf, niet de private velden, zodat de setters uitgevoerd worden
            ApplianceType = applianceType;
            ApplianceBrand = applianceBrand;
            ApplianceConsumption = applianceConsumption;
            AppliancePrice = appliancePrice;
        }
        public override string ToString()
        {
            return $"{applianceType} - {applianceBrand}";
        }



    }
}
