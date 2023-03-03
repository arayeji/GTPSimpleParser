using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace LTEParser
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        EtherPacket pac = new EtherPacket();
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach(Control ctrl in tableLayoutPanel2.Controls)
            {
                if(ctrl.GetType()== typeof(PictureBox))
                {
                    ((PictureBox)ctrl).SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            DequeuePacket.Start();
          
            pac.RecPacket();
          
        }
 
        private void DequeuePacket_Tick(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            
            for (int x = 0; x < 100 && EtherPacket.Packets.Count>0; x++)
            {
                try
                {
                    sb.Append(JsonConvert.SerializeObject(EtherPacket.Packets.Dequeue())+"\r\n");
                }
                catch (Exception ex)
                {
                    EtherPacket.Recs.Enqueue(ex.ToString());
                }
               
            }
            GTPLog.AppendText(sb.ToString());

            StringBuilder sb1 = new StringBuilder();
            for (int x = 0; x < 100 && EtherPacket.Recs.Count > 0; x++)
            {
                try
                {
                    sb1.Append(JsonConvert.SerializeObject(EtherPacket.Recs.Dequeue()) + "\r\n");
                }
                catch (Exception ex)
                {
                    EtherPacket.Recs.Enqueue(ex.ToString());
                }

            }
            GTPLog.AppendText(sb1.ToString());
        }

        
        private void Copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GTPLog.Text);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            pac.Stop();
        }
    }
}
