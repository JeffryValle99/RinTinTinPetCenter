using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoVeterinaria
{
    public partial class addCliente : Form
    {
        public addCliente()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            
            // Validaciones
            if (txtNombre.Text.Trim().Length == 0)
            {
                MessageBox.Show("Nombre debe ir lleno");
                return; // No continua
            }
            if (txtApellido.Text.Trim().Length == 0)
            {
                MessageBox.Show("Apellido debe ir lleno");
                return; // No continua
            }
            //if (!long.TryParse(txtTelefono.Text, out long Telefono))
            //{
            //    MessageBox.Show("Telefono debe ir lleno");
            //    return; // No continua
            //}
            if (txtCorreo.Text.Trim().Length == 0)
            {
                MessageBox.Show("Correo debe ir lleno");
                return; // No continua
            }
            if (txtDireccion.Text.Trim().Length == 0)
            {
                MessageBox.Show("Existencias debe ir lleno");
                return; // No continua
            }
            // 
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
