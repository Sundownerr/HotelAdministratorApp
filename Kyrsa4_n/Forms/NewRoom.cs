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

namespace Kyrsa4_n
{
    public partial class NewRoom : Form
    {
        public NewRoom()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool haveEmptyFields = textBox1.Text == "" || numericUpDown1.Text == "" || numericUpDown2.Text == "";

            if (haveEmptyFields)
            {
                MessageBox.Show("Некоторые поля не заполнены");
            }
            else
            {
                XmlDocument xDoc = new XmlDocument();

                xDoc.Load("XMLs/Rooms.xml");

                XmlElement rootElement = xDoc.CreateElement("Room");
                XmlElement numberElement = xDoc.CreateElement("Number");
                XmlElement numberOfPlacesElement = xDoc.CreateElement("NumberOfPlaces");
                XmlElement classElement = xDoc.CreateElement("Class");
                XmlElement statusElement = xDoc.CreateElement("Status");

                numberElement.InnerText = textBox1.Text;
                classElement.InnerText = numericUpDown1.Value.ToString();
                numberOfPlacesElement.InnerText = numericUpDown2.Value.ToString();
                statusElement.InnerText = "Доступен";

                xDoc.DocumentElement.AppendChild(rootElement);
                rootElement.AppendChild(numberElement);
                rootElement.AppendChild(numberOfPlacesElement);
                rootElement.AppendChild(classElement);
                rootElement.AppendChild(statusElement);

                xDoc.Save("XMLs/Rooms.xml");

                Application.OpenForms.OfType<HotelRooms>().First().LoadRoomsFromXml();

                this.Close();
            }
        }
    }
}
