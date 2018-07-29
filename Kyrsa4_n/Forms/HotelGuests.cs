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
    public partial class HotelGuests : Form
    {
        private string[] guestData;

        public string[] GuestData
        {
            get { return guestData;}
        }

        public void LoadGuestsFromXml()
        {
            int nodeCount = 0;
            DataGridView Dgv = dataGridView1;
            XmlDocument xDoc = new XmlDocument();

            xDoc.Load("XMLs/Guests.xml");           

            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
            {
                if (Dgv.Rows.Count < xDoc.DocumentElement.ChildNodes.Count)
                {
                    Dgv.Rows.Add();
                }

                Dgv[0, nodeCount].Value = node["Room"].InnerText;
                Dgv[1, nodeCount].Value = node["Class"].InnerText;
                Dgv[2, nodeCount].Value = node["Name"].InnerText;
                Dgv[3, nodeCount].Value = node["Surname"].InnerText;
                Dgv[4, nodeCount].Value = node["DateOfBirth"].InnerText;
                Dgv[5, nodeCount].Value = node["CheckInDate"].InnerText;
                Dgv[6, nodeCount].Value = node["CheckOutDate"].InnerText;
                Dgv[7, nodeCount].Value = node["ResidingCost"].InnerText;

                nodeCount++;
            }

        }

        public HotelGuests()
        {
            InitializeComponent();
            guestData = new string[10];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<GuestReg>().Count() < 1)
            {
                GuestReg guestReg = new GuestReg();
                guestReg.ShowDialog(this);
            }
            else
            {
                Application.OpenForms.OfType<GuestReg>().First().Focus();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ChangeDepartureDate>().Count() < 1)
            {
                ChangeDepartureDate earlyDeparture = new ChangeDepartureDate();
                earlyDeparture.ShowDialog(this);
            }
            else
            {
                Application.OpenForms.OfType<ChangeDepartureDate>().First().Focus();
            }
        }

        private void HotelGuests_Load(object sender, EventArgs e)
        {

            DataGridView Dgv = dataGridView1;

            Dgv.Columns.Add("1", "Номер");
            Dgv.Columns.Add("2", "Класс");
            Dgv.Columns.Add("3", "Имя");
            Dgv.Columns.Add("4", "Фамилия");
            Dgv.Columns.Add("5", "Дата рождения");
            Dgv.Columns.Add("6", "Дата заселения");
            Dgv.Columns.Add("7", "Дата отъезда");
            Dgv.Columns.Add("8", "Стоимость проживания");

            LoadGuestsFromXml();
            button3.Enabled = false;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {         
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    DataGridViewCellCollection Cells = dataGridView1.SelectedRows[0].Cells;
                    button3.Enabled = true;

                    for (int i = 0; i < dataGridView1.SelectedRows[0].Cells.Count; i++)
                    {
                        if (Cells[i].Value != null )
                        {
                            guestData[i] = dataGridView1.SelectedRows[0].Cells[i].Value.ToString();
                        }
                    }
                }
            }           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlDocument xRoomsDoc = new XmlDocument();

            xDoc.Load("XMLs/Guests.xml");

            foreach(XmlNode node in xDoc.DocumentElement)
            {
                if(node["Room"].InnerText == guestData[0])
                {
                    if(node["Name"].InnerText == guestData[2])
                    {
                        if(node["Surname"].InnerText == guestData[3])
                        {
                            if(node["DateOfBirth"].InnerText == guestData[4])
                            {
                                xDoc.DocumentElement.RemoveChild(node);
                                
                            }
                        }
                    }
                }
            }           

            xDoc.Save("XMLs/Guests.xml");

            xRoomsDoc.Load("XMLs/Rooms.xml");

            foreach (XmlNode node in xRoomsDoc.DocumentElement)
            {
                if (node["Number"].InnerText == guestData[0])
                {
                    if (node["Class"].InnerText == guestData[1])
                    {
                        node["Status"].InnerText = "Доступен";
                    }
                }
            }

            xRoomsDoc.Save("XMLs/Rooms.xml");

            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

            LoadGuestsFromXml();
        }
    }
}
