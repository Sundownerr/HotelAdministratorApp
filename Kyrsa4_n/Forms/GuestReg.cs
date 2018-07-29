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
    public partial class GuestReg : Form
    {
        private double residingCost;
        private char[] separators; 
        private string[] selectedRoomNumber;

        public GuestReg()
        {
            InitializeComponent();
        }

        public string GuestName
        {
            get { return nameTb.Text; }
        }

        public string GuestSurname
        {
            get { return surnameTb.Text; }
        }

        public string GuestDateOfBirth
        {
            get { return dateTimePicker1.Text; }
        }

        public string GuestRoomNumber
        {
            get { return listBox1.Text; }
        }      

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string calculateResidingCost()
        {
            if (listBox1.SelectedItem != null)
            {
                residingCost = (dateTimePicker3.Value - dateTimePicker2.Value).TotalDays * 100 * int.Parse(selectedRoomNumber[2]);
                return Convert.ToInt32(residingCost).ToString() + " р.";
            }
            else
            {
                return "-";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool haveEmptyFields, haveIncorrectDates;
                      
            haveIncorrectDates = DateTime.Compare(dateTimePicker2.Value, dateTimePicker3.Value) > 0;
            haveEmptyFields = nameTb.Text == "" || surnameTb.Text == "" || listBox1.SelectedItem == null;
           
            if (haveEmptyFields || haveIncorrectDates)
            {
                MessageBox.Show("Имеются незаполненные поля или неправильно указаны даты!");
            }
            else
            {
                XmlDocument xDoc = new XmlDocument();
                XmlDocument xRoomsDoc = new XmlDocument();

                xDoc.Load("XMLs/Guests.xml");

                XmlElement rootElement = xDoc.CreateElement("Guest");
                XmlElement roomElement = xDoc.CreateElement("Room");
                XmlElement classElement = xDoc.CreateElement("Class");
                XmlElement nameElement = xDoc.CreateElement("Name");
                XmlElement surnameElement = xDoc.CreateElement("Surname");
                XmlElement dateOfBirthElement = xDoc.CreateElement("DateOfBirth");
                XmlElement checkInDateElement = xDoc.CreateElement("CheckInDate");
                XmlElement checkOutDateElement = xDoc.CreateElement("CheckOutDate");
                XmlElement residingCostElement = xDoc.CreateElement("ResidingCost");

                roomElement.InnerText = selectedRoomNumber[0];
                classElement.InnerText = selectedRoomNumber[2];
                nameElement.InnerText = nameTb.Text;
                surnameElement.InnerText = surnameTb.Text;
                dateOfBirthElement.InnerText = dateTimePicker1.Text;
                checkInDateElement.InnerText = dateTimePicker2.Text;
                checkOutDateElement.InnerText = dateTimePicker3.Text;
                residingCostElement.InnerText = residingCostTb.Text;

                xDoc.DocumentElement.AppendChild(rootElement);
                rootElement.AppendChild(roomElement);
                rootElement.AppendChild(classElement);
                rootElement.AppendChild(nameElement);
                rootElement.AppendChild(surnameElement);
                rootElement.AppendChild(dateOfBirthElement);
                rootElement.AppendChild(checkInDateElement);
                rootElement.AppendChild(checkOutDateElement);
                rootElement.AppendChild(residingCostElement);

                xDoc.Save("XMLs/Guests.xml");

                xRoomsDoc.Load("XMLs/Rooms.xml");                                            

                foreach (XmlNode node in xRoomsDoc.DocumentElement)
                {
                    if (node["Status"].InnerText == "Доступен")
                    {
                        if(node["Number"].InnerText == selectedRoomNumber[0])
                        {
                            node["Status"].InnerText = "Недоступен";
                            break;
                        }
                    }
                }

                xRoomsDoc.Save("XMLs/Rooms.xml");

                if (Application.OpenForms.OfType<HotelGuests>().Count() > 0)
                {
                    Application.OpenForms.OfType<HotelGuests>().First().LoadGuestsFromXml();
                }

                string receiptFileName = @"Receipt\" + DateTime.Today.ToShortDateString() + "_" + nameTb.Text + "_" + surnameTb.Text + ".txt";

                StreamWriter receipt = new StreamWriter(File.Create(receiptFileName));

                receipt.WriteLine("Квитанция");
                receipt.WriteLine();
                receipt.WriteLine("за оплату проживание в номере " + selectedRoomNumber[0] + " класса " + selectedRoomNumber[2]);
                receipt.WriteLine();
                receipt.WriteLine();
                receipt.WriteLine("Постоялец:   " + nameTb.Text + " " + surnameTb.Text + " " + dateTimePicker1.Text);
                receipt.WriteLine();
                receipt.WriteLine("К оплате:    " + residingCostTb.Text);

                receipt.Close();

                this.Close();
            }
        }

        private void GuestReg_Load(object sender, EventArgs e)
        {
            separators = new char[2];
            separators[0] = ',';
            separators[1] = ' ';
            XmlDocument xRoomsDoc = new XmlDocument();
            StringBuilder roomInfo = new StringBuilder();

            residingCostTb.Text = "";
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            dateTimePicker2.CustomFormat = "dd/MM/yyyy";
            dateTimePicker3.CustomFormat = "dd/MM/yyyy";

            dateTimePicker2.MinDate = DateTime.Today;
            dateTimePicker3.MinDate = dateTimePicker2.Value.AddDays(1);

            xRoomsDoc.Load("XMLs/Rooms.xml");        

            foreach (XmlNode node in xRoomsDoc.DocumentElement)
            {
                if (node["Status"].InnerText == "Доступен")
                {
                    roomInfo.Append(int.Parse(node["Number"].InnerText) + ", " + int.Parse(node["Class"].InnerText) + " кл., " + int.Parse(node["NumberOfPlaces"].InnerText) + " мест");

                    listBox1.Items.Add(roomInfo.ToString());

                    roomInfo.Remove(0, roomInfo.Length);
                }
            }          
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            residingCostTb.Text = calculateResidingCost();
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            residingCostTb.Text = calculateResidingCost();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {           
            selectedRoomNumber = listBox1.Text.Split(separators);
            residingCostTb.Text = calculateResidingCost();
        }
    }
}
