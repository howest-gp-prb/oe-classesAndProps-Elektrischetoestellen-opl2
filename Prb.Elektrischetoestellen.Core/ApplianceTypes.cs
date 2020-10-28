using System;
using System.Collections.Generic;
using System.Text;

namespace Prb.Elektrischetoestellen.Core
{
    public class ApplianceTypes
    {
        public static List<string> GetApplianceTypes()
        {
            List<string> applianceTypes = new List<string>();
            applianceTypes.Add("Vaatwasser");
            applianceTypes.Add("Oven");
            applianceTypes.Add("Fornuis");
            applianceTypes.Add("Koelkast");
            applianceTypes.Add("Diepvries");
            applianceTypes.Add("Boiler");
            applianceTypes.Add("TV");
            return applianceTypes;

        }
    }
}
