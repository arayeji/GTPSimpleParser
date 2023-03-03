using System;
using System.Collections.Generic;
using SharpPcap; 
using System.IO;

namespace LTEParser
{
    class EtherPacket
    {
        static public Queue<GTPv2> Packets = new Queue<GTPv2>();
        static public Queue<string> Recs = new Queue<string>();
         
      
        void EtherPacket_OnPacketArrival(object sender, PacketCapture e)
        {
            try
            {
                MemoryStream ms = new MemoryStream(e.Data.ToArray());
                ms.Seek(42, SeekOrigin.Begin);
                if (ms.ReadByte() == 72)
                {
                    GTPv2 gtp = new GTPv2(ms, e.Data.Length);
                    Packets.Enqueue(gtp);
                }
                else
                {
                  //  Recs.Enqueue(ByteArrayToString(e.Packet.Data));
                }
               
            }
            catch (Exception ex)
            {
                Recs.Enqueue(ex.ToString());
            }
        }
      
     

         
        CaptureDeviceList devlist = CaptureDeviceList.Instance;
        List<ICaptureDevice> Devices = new List<ICaptureDevice>();
        public void RecPacket()
        {
            
            for (int i = 0; i < devlist.Count; i++)
            {

                try
                {

                    {
                        SharpPcap.ICaptureDevice devcap = devlist[i];

                        devcap.Open(DeviceModes.Promiscuous);
                        {
                            if (devcap.MacAddress.ToString().ToUpper() == "005056816ED1")
                            {
                                Devices.Add(devcap);

                                devcap.OnPacketArrival += EtherPacket_OnPacketArrival;
                                devcap.Filter = "udp";
                                devcap.StartCapture();
                            }
                            else
                                devcap.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                   
                }
            }
        }


        public void Stop()
        {
            foreach(ICaptureDevice dev in Devices)
            {
                dev.StopCapture();
                dev.Close();
            }
        }
    }
}
