using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kyrsa4_n
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        //  Exit button

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        //  Open hotel rooms button

        private void button1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<HotelRooms>().Count() < 1)
            {              
                HotelRooms hotelRooms = new HotelRooms();
                hotelRooms.Show();
            } 
            else
            {
                Application.OpenForms.OfType<HotelRooms>().First().Focus();
            }
        }

        //  Open hotel guests button

        private void button2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<HotelGuests>().Count() < 1)
            {
                HotelGuests hotelGuests = new HotelGuests();
                hotelGuests.Show();
            }
            else
            {
                Application.OpenForms.OfType<HotelGuests>().First().Focus();
            }
        }

        //  Open guest registration button

        private void button3_Click(object sender, EventArgs e)
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
    }
}
