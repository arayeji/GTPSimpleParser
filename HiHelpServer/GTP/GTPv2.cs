using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LTEParser.EtherPacket;
using static LTEParser.IE;
using static System.Net.WebRequestMethods;
using System.Net;
using System.IO;

namespace LTEParser
{
    public class GTPv2
    {
        public uint TEID = 0;
        public uint messageLength = 0;
        public uint sequenceNumber = 0;
        public int Spare = 0;
        public MessageType messageType;
        public List<IE> IEs = new List<IE>();

       

        [JsonConverter(typeof(StringEnumConverter))]
        public enum MessageType
        {
            Node_Alive_Request = 4,
            Node_Alive_Response = 5,
            Release_Access_Bearers_Request = 170,
            Release_Access_Bearers_Response = 171,
            Downlink_Data_Notification_Failure_Indication = 70,
            Modify_Bearers_Request = 34,
            Create_Session_Request = 32,
            Create_Session_Response = 33,
            Downlink_Data_Notification = 176,
            Downlink_Data_Notification_Acknowledgement = 177,
        }

        public GTPv2(MemoryStream ms,int Len)
        {
            byte[] mt = new byte[1];
            ms.Read(mt, 0, 1);
            messageType = (GTPv2.MessageType)Convert.ToInt32(mt[0]);

            byte[] pLen = new byte[2];
            ms.Read(pLen, 0, 2);
            messageLength = Convert.ToUInt32(pLen.ToInt());

            byte[] teid = new byte[4];
            ms.Read(teid, 0, 4);
            TEID = teid.ToInt();

            byte[] seq = new byte[3];
            ms.Read(seq, 0, 3);
            sequenceNumber = seq.ToInt();

            Spare = ms.ReadByte();

            while (ms.Position < Len - 1)
            {
                try
                {
                    IE avp = new IE();
                    try
                    {
                        avp.ieType = (IEType)ms.ReadByte();
                    }
                    catch (Exception)
                    {
                        avp.ieType = IEType.UNKNOWN;
                    }

                    int avplen = (int)(uint)(((ms.ReadByte() & 0xff) << 8) | (ms.ReadByte() & 0xff));
                    byte[] avpbt = new byte[avplen];
                    ms.ReadByte();
                    ms.Read(avpbt, 0, avplen);
                    switch (avp.ieType)
                    {
                        case IEType.APN:

                            avp.Value = Encoding.UTF8.GetString(avpbt);
                            break;
                        case IEType.PDN_Address_Allocation:

                            string s = avpbt.ToString();
                            byte[] bt = s.Remove(0, 2).ToByteArray();
                            avp.Value = new IPAddress(bt).ToString();

                            break;
                        case IEType.IMSI:


                            avp.Value = avpbt.IEArrayToString();

                            break;
                        case IEType.MSISDN:
                            avp.Value = avpbt.IEArrayToString();
                            break;

                        default:
                            avp.Value = avpbt.ToString();
                            break;
                    }

                    IEs.Add(avp);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
