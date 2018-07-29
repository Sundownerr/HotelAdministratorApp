using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Kyrsa4_n
{
    public partial class ChangeDepartureDate : Form
    {
        private string[] selectedGuest;
        private double residingCost;

        public ChangeDepartureDate()
        {
            InitializeComponent();
        }

        private void EarlyDeparture_Load(object sender, EventArgs e)
        {
            selectedGuest = Application.OpenForms.OfType<HotelGuests>().First().GuestData;
            
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            dateTimePicker2.CustomFormat = "dd/MM/yyyy";
            dateTimePicker3.CustomFormat = "dd/MM/yyyy";
            dateTimePicker4.CustomFormat = "dd/MM/yyyy";

            nameTb.Text = selectedGuest[2];
            surnameTb.Text = selectedGuest[3];
            dateTimePicker1.Value = DateTime.Parse(selectedGuest[4]);
            dateTimePicker2.Value = DateTime.Parse(selectedGuest[5]);
            dateTimePicker3.Value = DateTime.Parse(selectedGuest[6]);

            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            dateTimePicker3.Enabled = false;

            dateTimePicker4.MinDate = dateTimePicker2.Value;

            residingCostTb.Text = ((dateTimePicker3.Value - dateTimePicker2.Value).TotalDays * 100 * int.Parse(selectedGuest[1])).ToString() + " р.";

           

            button1.Enabled = false;
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("XMLs/Guests.xml");

            foreach (XmlNode node in xDoc.DocumentElement)
            {
                if (node["Room"].InnerText == selectedGuest[0])
                {
                    if (node["Name"].InnerText == selectedGuest[2])
                    {
                        if (node["Surname"].InnerText == selectedGuest[3])
                        {
                            node["CheckOutDate"].InnerText = dateTimePicker4.Text;
                            node["ResidingCost"].InnerText = Convert.ToInt32(residingCost).ToString() + " р.";
                        }
                    }
                }
            }

            int dateChange = DateTime.Compare(dateTimePicker3.Value, dateTimePicker4.Value);
            string messageText;

            if (dateChange <= 0)
            {
                messageText = "delay";
            }
            else
            {
                messageText = "early";
            }


            xDoc.Save("XMLs/Guests.xml");

            Application.OpenForms.OfType<HotelGuests>().First().LoadGuestsFromXml();

            string receiptFileName = @"Receipt\" + DateTime.Today.ToShortDateString() + "_" + selectedGuest[2] + "_" + selectedGuest[3] + "_" + messageText + ".txt";           
          
            StreamWriter receipt = new StreamWriter(File.Create(receiptFileName));

            receipt.WriteLine("Квитанция");
            receipt.WriteLine();
            receipt.WriteLine("за оплату проживание в номере " + selectedGuest[0] + " класса " + selectedGuest[1]);
            receipt.WriteLine();
            receipt.WriteLine();
            receipt.WriteLine("Постоялец:   " + selectedGuest[2] + " " + selectedGuest[3] + " " + selectedGuest[4]);
            receipt.WriteLine();
            receipt.WriteLine("К оплате:    " + Convert.ToInt32(residingCost).ToString() + " р.");

            receipt.Close();
            
            this.Close();
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            residingCost = (dateTimePicker4.Value - dateTimePicker2.Value).TotalDays * 100 * int.Parse(selectedGuest[1]);
            residingCostTb.Text = Convert.ToInt32(residingCost).ToString() + " р.";
        }
    }
}
