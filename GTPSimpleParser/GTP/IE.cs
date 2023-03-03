using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTEParser
{
    public class IE
    {
        public IEType ieType;
        public string Value;

        [JsonConverter(typeof(StringEnumConverter))]
        public enum IEType
        {
            IMSI = 1,
            Recovery = 3,
            Cause = 2,
            MSISDN = 76,
            User_Location_Info = 86,
            Serving_Network = 83,
            RAT_Type = 82,
            Indication = 77,
            Fully_Qualified_Tunnel_Endpoint_Identifier = 87,
            APN = 71,
            Selection_Mode = 128,
            PDN_Type = 99,
            PDN_Address_Allocation = 79,
            APN_Restriction = 127,
            AMBR = 72,
            Protocol_Congiguration_Option = 78,
            Bearer_Context = 93,
            UE_Time_Zone = 114,
            UNKNOWN = 0

        }
    }

}
