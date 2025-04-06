using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProyectoVeterinaria
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";

            try
            {
                SqlConnection con = new SqlConnection(cnx);
                con.Open();
                MessageBox.Show("Conexión Existosa");


                con.Close();

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnfrmControl_Click(object sender, EventArgs e)
        {
            Control frmControl = new Control();
            frmControl.Show();
        }
    }
}
