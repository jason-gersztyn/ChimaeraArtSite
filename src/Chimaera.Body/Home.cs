using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chimaera.Body
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void shopButton_Click(object sender, EventArgs e)
        {
            var seriesForm = new SeriesSelect();
            seriesForm.Show();
        }

        private void comicButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under construction");
        }
    }
}
