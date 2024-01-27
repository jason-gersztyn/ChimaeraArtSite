using System;
using System.Windows.Forms;

namespace Chimaera.Body
{
    public partial class SeriesSelect : Form
    {
        public SeriesSelect()
        {
            InitializeComponent();
        }

        private void createSeriesButton_Click(object sender, EventArgs e)
        {
            var createSeries = new SeriesCreate();
            createSeries.Show();
        }

        private void selectSeriesButton_Click(object sender, EventArgs e)
        {

        }
    }
}