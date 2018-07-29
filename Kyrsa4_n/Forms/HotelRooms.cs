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
    public partial class HotelRooms : Form
    {
        public HotelRooms()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoadRoomsFromXml()
        {
            int nodeCount = 0;
            DataGridView Dgv = dataGridView1;
            XmlDocument xRoomsDoc = new XmlDocument();

            xRoomsDoc.Load("XMLs/Rooms.xml");

            foreach (XmlNode node in xRoomsDoc.DocumentElement)
            {
                if (Dgv.Rows.Count < xRoomsDoc.DocumentElement.ChildNodes.Count)
                {
                    Dgv.Rows.Add();
                }

                Dgv[0, nodeCount].Value = node["Number"].InnerText;
                Dgv[1, nodeCount].Value = node["NumberOfPlaces"].InnerText;
                Dgv[2, nodeCount].Value = node["Class"].InnerText;
                Dgv[3, nodeCount].Value = node["Status"].InnerText;

                nodeCount++;
            }
        }

        private void HotelRooms_Load(object sender, EventArgs e)
        {
            DataGridView Dgv = dataGridView1;

            Dgv.Columns.Add("1", "Номер");
            Dgv.Columns.Add("2", "Кол-во мест");
            Dgv.Columns.Add("3", "Класс");
            Dgv.Columns.Add("4", "Статус");

            LoadRoomsFromXml();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<NewRoom>().Count() < 1)
            {
                NewRoom newRoom = new NewRoom();
                newRoom.ShowDialog(this);
            }
            else
            {
                Application.OpenForms.OfType<NewRoom>().First().Focus();
            }
        }
    }
}
