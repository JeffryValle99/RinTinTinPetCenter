using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProyectoVeterinaria
{
    public partial class Control : Form
    {
        public Control()
        {
            InitializeComponent();
        }

        public void GenerarCorreo()
        {
            string nombre = txtNombreEmpleado.Text.Trim().ToLower();
            string apellido = txtApellidoEmpleado.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(apellido))
            {
                // Crear el correo combinando nombre y apellido
                string correo = $"{nombre}.{apellido}@vetclinic.com";
                txtCorreoEmpleado.Text = correo;
            }
            else
            {   // Si algún campo está vacío, limpiar el textbox de correo
                txtCorreoEmpleado.Text = string.Empty;
            }
        }

        private void frmClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gridClientes_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        public void CargarInformacion(string load, DataGridView datagrid)
        {
            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                SqlConnection con = new SqlConnection(cnx);
                SqlDataAdapter data = new SqlDataAdapter(load, con);
                DataTable tabla = new DataTable();
                data.Fill(tabla);
                datagrid.DataSource = tabla;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Control_Load(object sender, EventArgs e)
        {
            CargarInformacion("spSelectCliente", gridClientes);
            CargarInformacion("spSelectMascota", gridMascotas);
            CargarInformacion("spSelectArticulo", gridArticulos);
            CargarInformacion("spSelectEmpleado", gridEmpleados);
            CargarInformacion("spSelectFactura", gridFactura);
            CargarInformacion("spSelectCompra", gridCompra);
            CargarInformacion("spSelectPago", gridPagos);
            CargarInformacion("spSelectCita", gridCitas);
            CargarInformacion("spSelectFichaRecepcion", gridFichas);
            controllers(gridClientes);
            controllers(gridMascotas);
            controllers(gridArticulos);
            controllers(gridEmpleados);
            controllers(gridFactura);
            controllers(gridCompra);
            controllers(gridPagos);
            controllers(gridCitas);
            controllers(gridFichas);
            //ComboBoxs
            CargarDatosCliente();
            CargarDatosEspecie();
            CargarDatosProveedor();
            CargarDatosEspecialidad();
            CargarDatosArea();
            CargarDatosCuenta();
            CargarDatosCompra();
            CargarDatosMascotaCita();
            CargarDatosMascotaFicha();
            CargarDatosEmpleado();
            CargarDatosCita();
            combo();
        }




        public void controllers(DataGridView grid)
        {
            try
            {
                grid.ReadOnly = true;
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
                grid.AllowUserToResizeColumns = false;
                grid.AllowUserToOrderColumns = false;
                grid.AllowUserToResizeRows = false;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                foreach (DataGridViewColumn column in grid.Columns)
                {
                    column.FillWeight = column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            addCliente agregar = new addCliente();
            agregar.ShowDialog();

            if (!agregar.IsDisposed)
            {
                string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
                try
                {
                    SqlConnection con = new SqlConnection(cnx);
                    SqlCommand cmd = new SqlCommand("spInsertarCliente", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre", agregar.txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Apellido", agregar.txtApellido.Text);
                    cmd.Parameters.AddWithValue("@Telefono", agregar.txtTelefono.Text);
                    cmd.Parameters.AddWithValue("@Correo", agregar.txtCorreo.Text);
                    cmd.Parameters.AddWithValue("@Direccion", agregar.txtDireccion.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    CargarInformacion("spSelectCliente", gridClientes); // Actualiza DataGridView
                    agregar.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {


        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (gridClientes.CurrentRow != null)
            {
                DataGridViewRow fila = gridClientes.CurrentRow;
                addCliente update = new addCliente();

                string cliente = fila.Cells["ClienteID"].Value.ToString();
                update.txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                update.txtApellido.Text = fila.Cells["Apellido"].Value.ToString();
                update.txtTelefono.Text = fila.Cells["Telefono"].Value.ToString();
                update.txtCorreo.Text = fila.Cells["Correo"].Value.ToString();
                update.txtDireccion.Text = fila.Cells["Direccion"].Value.ToString();
                update.btnAgregar.Text = "Actualizar";
                update.ShowDialog();

                if (!update.IsDisposed)
                {
                    try
                    {

                        // Mostrar diálogo de confirmación
                        DialogResult resultado = MessageBox.Show("¿Estás seguro de querer actualizar este registro?", "Confirmación",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        // Si el usuario selecciona "No", salir del método
                        if (resultado == DialogResult.No) return;

                        string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";

                        SqlConnection con = new SqlConnection(cnx);
                        con.Open();

                        int clienteID = Convert.ToInt16(cliente);
                        SqlCommand cmd = new SqlCommand("spUpdateCliente", con);

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                        cmd.Parameters.AddWithValue("@Nombre", update.txtNombre.Text);
                        cmd.Parameters.AddWithValue("@Apellido", update.txtApellido.Text);
                        cmd.Parameters.AddWithValue("@Telefono", int.Parse(update.txtTelefono.Text));
                        cmd.Parameters.AddWithValue("@Correo", update.txtCorreo.Text);
                        cmd.Parameters.AddWithValue("@Direccion", update.txtDireccion.Text);

                        //Abrir la conexión
                        cmd.ExecuteNonQuery();
                        con.Close(); // Cerrar la conexión

                        update.Dispose();
                        this.DialogResult = DialogResult.OK;
                        CargarInformacion("spSelectCliente", gridClientes);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (gridClientes.CurrentRow != null)
            {
                DataGridViewRow fila = gridClientes.CurrentRow;
                addCliente delete = new addCliente();
                string cliente = fila.Cells["ClienteID"].Value.ToString();
                int clienteID = Convert.ToInt16(cliente);
                delete.btnAgregar.Text = "Eliminar";
                // Mostrar diálogo de confirmación
                DialogResult resultado = MessageBox.Show(
                "¿Estás seguro de querer eliminar el registro?",
                "Confirmación de Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

                // Si el usuario selecciona "No", salir
                if (resultado == DialogResult.No)
                {
                    return;
                }

                try
                {
                    string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
                    SqlConnection con = new SqlConnection(cnx);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spDeleteCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                    cmd.ExecuteNonQuery();
                    con.Close(); // Cerrar la conexión

                    delete.Dispose();
                    this.DialogResult = DialogResult.OK;
                    CargarInformacion("spSelectCliente", gridClientes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void CargarDatosCliente()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectCliente", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dtCliente = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dtCliente);
                cmbCliente.DataSource = dtCliente;
                cmbCliente.DisplayMember = "Nombre";
                cmbCliente.ValueMember = "ClienteID";
                cmbClienteFactura.DataSource = dtCliente;
                cmbClienteFactura.DisplayMember = "Nombre";
                cmbClienteFactura.ValueMember = "ClienteID";

                cnx.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void CargarDatosEspecie()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectEspecie", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                cmbEspecie.DataSource = dt;
                cmbEspecie.DisplayMember = "Nombre";
                cmbEspecie.ValueMember = "EspecieID";

                cnx.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public void CargarDatosProveedor()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectProveedor", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                cmbProveedor.DataSource = dt;
                cmbProveedor.DisplayMember = "Nombre";
                cmbProveedor.ValueMember = "ProveedorID";

                cmbProveedorCompra.DataSource = dt;
                cmbProveedorCompra.DisplayMember = "Nombre";
                cmbProveedorCompra.ValueMember = "ProveedorID";

                cnx.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        public void CargarDatosEspecialidad()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectEspecialidad", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                cmbEspecialidadEmpleado.DataSource = dt;
                cmbEspecialidadEmpleado.DisplayMember = "Nombre";
                cmbEspecialidadEmpleado.ValueMember = "EspecialidadID";
                cnx.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
        public void CargarDatosArea()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectArea", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                cmbAreaEmpleado.DataSource = dt;
                cmbAreaEmpleado.DisplayMember = "Nombre";
                cmbAreaEmpleado.ValueMember = "AreaID";
                cnx.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        public void CargarDatosCuenta()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectCuenta", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                cmbCuentaFactura.DataSource = dt;
                cmbCuentaFactura.DisplayMember = "Banco";
                cmbCuentaFactura.ValueMember = "CuentaID";

                cmbCuentaPago.DataSource = dt;
                cmbCuentaPago.DisplayMember = "Banco";
                cmbCuentaPago.ValueMember = "CuentaID";

                cnx.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        public void CargarDatosCompra()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectCompra", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                cmbCompraPago.DataSource = dt;
                cmbCompraPago.DisplayMember = "Total";
                cmbCompraPago.ValueMember = "CompraID";
                cnx.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        public void CargarDatosMascotaCita()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectMascota", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                cmbMascotaCita.DataSource = dt;
                cmbMascotaCita.DisplayMember = "Nombre";
                cmbMascotaCita.ValueMember = "MascotaID";

                cnx.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
        public void CargarDatosMascotaFicha()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectMascota", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                cmbMascotaFicha.DataSource = dt;
                cmbMascotaFicha.DisplayMember = "Nombre";
                cmbMascotaFicha.ValueMember = "MascotaID";
                cnx.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        public void CargarDatosEmpleado()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectEmpleado", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                cmbEmpleadoFicha.DataSource = dt;
                cmbEmpleadoFicha.DisplayMember = "Nombre";
                cmbEmpleadoFicha.ValueMember = "EmpleadoID";

                cnx.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        public void CargarDatosCita()
        {
            string con = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            SqlConnection cnx = new SqlConnection(con);
            try
            {
                cnx.Open();
                SqlCommand command = new SqlCommand("spSelectCita", cnx);
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                cmbCitaFicha.DataSource = dt;
                cmbCitaFicha.DisplayMember = "Motivo";
                cmbCitaFicha.ValueMember = "CitaID";

                cnx.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        public void combo()
        {
            // Sexo
            cmbSexo.Items.Add("Macho");
            cmbSexo.Items.Add("Hembra");
            // Factura - Estado
            cmbEstadoFactura.Items.Add("Pagada");
            cmbEstadoFactura.Items.Add("Pendiente");
            // Factura - Metodo de Pago
            cmbMPFactura.Items.Add("Efectivo");
            cmbMPFactura.Items.Add("Tarjeta");
            cmbMPFactura.Items.Add("Transferencia");
            // Compra - Estado
            cmbEstadoCompra.Items.Add("Completado");
            cmbEstadoCompra.Items.Add("Pendiente");
            // Pago - Metodo de Pago
            cmbMPPago.Items.Add("Efectivo");
            cmbMPPago.Items.Add("Tarjeta");
            cmbMPPago.Items.Add("Transferencia");
            // Cita y Ficha Recepcion
            cmbEstadoCita.Items.Add("Pendiente");
            cmbEstadoCita.Items.Add("Completada");

            cmbEstadoFicha.Items.Add("Pendiente");
            cmbEstadoFicha.Items.Add("Atendido");

            cmbTipoFicha.Items.Add("Emergencia");
            cmbTipoFicha.Items.Add("Atencion normal");
            cmbTipoFicha.Items.Add("Cita");

        }


        private void btnAgregarMascota_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (txtNombreMascota.Text.Trim().Length == 0) { errorProvider1.SetError(txtNombreMascota, "El nombre es obligatorio"); return; };
                if (cmbCliente.Text.Length == 0) { errorProvider1.SetError(cmbCliente, "El cliente es obligatorio"); return; };
                if (cmbEspecie.Text.Length == 0) { errorProvider1.SetError(cmbEspecie, "La Especie es obligatoria"); return; };
                if (cmbSexo.Text.Length == 0) { errorProvider1.SetError(cmbSexo, "El sexo es obligatorio"); return; };
                if (txtRazaMascota.Text.Trim().Length == 0) { errorProvider1.SetError(txtRazaMascota, "La Raza es obligatoria"); return; };
                if (txtFechaMascota.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaMascota, "La Fecha es obligatoria"); return; };
                errorProvider1.Clear();


                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spInsertarMascota", con);

                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@Nombre", txtNombreMascota.Text);
                cmd.Parameters.AddWithValue("@EspecieID", cmbEspecie.SelectedValue);
                cmd.Parameters.AddWithValue("@ClienteID", cmbCliente.SelectedValue);
                cmd.Parameters.AddWithValue("@FechaNacimiento", txtFechaMascota.Text);
                cmd.Parameters.AddWithValue("@Sexo", cmbSexo.Text);
                cmd.Parameters.AddWithValue("@Raza", txtRazaMascota.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtNombreMascota.Text = "";
                txtFechaMascota.Text = "";
                txtRazaMascota.Text = "";


                CargarInformacion("spSelectMascota", gridMascotas); // Actualiza DataGridView

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtNombreMascota_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnActualizarMascota_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (txtNombreMascota.Text.Trim().Length == 0) { errorProvider1.SetError(txtNombreMascota, "El nombre es obligatorio"); return; };
                if (cmbCliente.Text.Length == 0) { errorProvider1.SetError(cmbCliente, "El cliente es obligatorio"); return; };
                if (cmbEspecie.Text.Length == 0) { errorProvider1.SetError(cmbEspecie, "La Especie es obligatoria"); return; };
                if (cmbSexo.Text.Length == 0) { errorProvider1.SetError(cmbSexo, "El sexo es obligatorio"); return; };
                if (txtRazaMascota.Text.Trim().Length == 0) { errorProvider1.SetError(txtRazaMascota, "La Raza es obligatoria"); return; };
                if (txtFechaMascota.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaMascota, "La Fecha es obligatoria"); return; };
                errorProvider1.Clear();

                DataGridViewRow fila = gridMascotas.CurrentRow;
                int mascota = Convert.ToInt16(fila.Cells["MascotaID"].Value.ToString());


                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spUpdateMascota", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", txtNombreMascota.Text);
                cmd.Parameters.AddWithValue("@EspecieID", cmbEspecie.SelectedValue);
                cmd.Parameters.AddWithValue("@ClienteID", cmbCliente.SelectedValue);
                cmd.Parameters.AddWithValue("@FechaNacimiento", txtFechaMascota.Text);
                cmd.Parameters.AddWithValue("@Sexo", cmbSexo.Text);
                cmd.Parameters.AddWithValue("@Raza", txtRazaMascota.Text);
                cmd.Parameters.AddWithValue("@MascotaID", mascota);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtNombreMascota.Text = "";
                txtFechaMascota.Text = "";
                txtRazaMascota.Text = "";


                CargarInformacion("spSelectMascota", gridMascotas); // Actualiza DataGridView

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridMascotas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow filaSeleccionada = gridMascotas.Rows[e.RowIndex];
                txtNombreMascota.Text = filaSeleccionada.Cells["Nombre"].Value?.ToString();
                cmbCliente.SelectedValue = filaSeleccionada.Cells["ClienteID"].Value?.ToString();
                cmbEspecie.SelectedValue = filaSeleccionada.Cells["EspecieID"].Value?.ToString();
                cmbSexo.SelectedItem = filaSeleccionada.Cells["Sexo"].Value?.ToString();
                txtRazaMascota.Text = filaSeleccionada.Cells["Raza"].Value?.ToString();
                txtFechaMascota.Text = filaSeleccionada.Cells["FechaNacimiento"].Value?.ToString();
            }
        }

        private void gridMascotas_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEliminarMascota_Click(object sender, EventArgs e)
        {
            if (gridMascotas.CurrentRow != null)
            {
                DataGridViewRow fila = gridMascotas.CurrentRow;
                int mascota = Convert.ToInt16(fila.Cells["MascotaID"].Value.ToString());
                // Mostrar diálogo de confirmación
                DialogResult resultado = MessageBox.Show("¿Estás seguro de querer eliminar el registro?", "Confirmación de Eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.No) { return; }
                try
                {
                    string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
                    SqlConnection con = new SqlConnection(cnx);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spDeleteMascota", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mascota", mascota);
                    cmd.ExecuteNonQuery();
                    con.Close(); // Cerrar la conexión

                    this.DialogResult = DialogResult.OK;
                    CargarInformacion("spSelectMascota", gridMascotas);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (txtNombreArticulo.Text.Trim().Length == 0) { errorProvider1.SetError(txtNombreArticulo, "El nombre es obligatorio"); return; };
                if (Double.Parse(txtPrecioArticulo.Text) <= 0) { errorProvider1.SetError(txtPrecioArticulo, "El Precio debe ser mayor a 0"); return; };
                if (cmbProveedor.Text.Length == 0) { errorProvider1.SetError(cmbProveedor, "El proveedor es obligatorio"); return; };
                if (txtDescripcionArticulo.Text.Trim().Length == 0) { errorProvider1.SetError(txtDescripcionArticulo, "La descripcion es obligatoria"); return; };
                errorProvider1.Clear();


                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spInsertarArticulo", con);

                cmd.CommandType = CommandType.StoredProcedure;

                double precio = Convert.ToDouble(txtPrecioArticulo.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreArticulo.Text);
                cmd.Parameters.AddWithValue("@Descripcion", txtDescripcionArticulo.Text);
                cmd.Parameters.AddWithValue("@Precio", precio);
                cmd.Parameters.AddWithValue("@ProveedorID", cmbProveedor.SelectedValue);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtNombreArticulo.Text = "";
                txtDescripcionArticulo.Text = "";
                txtPrecioArticulo.Text = "";


                CargarInformacion("spSelectArticulo", gridArticulos); // Actualiza DataGridView

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActualizarArticulo_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (txtNombreArticulo.Text.Trim().Length == 0) { errorProvider1.SetError(txtNombreArticulo, "El nombre es obligatorio"); return; };
                if (Double.Parse(txtPrecioArticulo.Text) <= 0) { errorProvider1.SetError(txtPrecioArticulo, "El Precio debe ser mayor a 0"); return; };
                if (cmbProveedor.Text.Length == 0) { errorProvider1.SetError(cmbProveedor, "El proveedor es obligatorio"); return; };
                if (txtDescripcionArticulo.Text.Trim().Length == 0) { errorProvider1.SetError(txtDescripcionArticulo, "La descripcion es obligatoria"); return; };
                errorProvider1.Clear();

                DataGridViewRow fila = gridArticulos.CurrentRow;
                int articulo = Convert.ToInt16(fila.Cells["ArticuloID"].Value.ToString());

                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spUpdateArticulo", con);
                cmd.CommandType = CommandType.StoredProcedure;

                double precio = Convert.ToDouble(txtPrecioArticulo.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreArticulo.Text);
                cmd.Parameters.AddWithValue("@Descripcion", txtDescripcionArticulo.Text);
                cmd.Parameters.AddWithValue("@Precio", precio);
                cmd.Parameters.AddWithValue("@ProveedorID", cmbProveedor.SelectedValue);
                cmd.Parameters.AddWithValue("@ArticuloID", articulo);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtNombreArticulo.Text = "";
                txtPrecioArticulo.Text = "0.00";
                txtDescripcionArticulo.Text = "";


                CargarInformacion("spSelectArticulo", gridArticulos); // Actualiza DataGridView

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridMascotas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gridArticulos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow filaSeleccionada = gridArticulos.Rows[e.RowIndex];
                txtNombreArticulo.Text = filaSeleccionada.Cells["Nombre"].Value?.ToString();
                txtPrecioArticulo.Text = filaSeleccionada.Cells["Precio"].Value?.ToString();
                cmbProveedor.SelectedValue = filaSeleccionada.Cells["ProveedorID"].Value?.ToString();
                txtDescripcionArticulo.Text = filaSeleccionada.Cells["Descripcion"].Value?.ToString();
            }
        }

        private void btnEliminarArticulo_Click(object sender, EventArgs e)
        {
            if (gridArticulos.CurrentRow != null)
            {
                DataGridViewRow fila = gridArticulos.CurrentRow;
                int articulo = Convert.ToInt16(fila.Cells["ArticuloID"].Value.ToString());
                // Mostrar diálogo de confirmación
                DialogResult resultado = MessageBox.Show("¿Estás seguro de querer eliminar el registro?", "Confirmación de Eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.No) { return; }
                try
                {
                    string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
                    SqlConnection con = new SqlConnection(cnx);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spDeleteArticulo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ArticuloID", articulo);
                    cmd.ExecuteNonQuery();
                    con.Close(); // Cerrar la conexión
                    this.DialogResult = DialogResult.OK;
                    CargarInformacion("spSelectArticulo", gridArticulos);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void gridEmpleados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow filaSeleccionada = gridEmpleados.Rows[e.RowIndex];
                txtNombreEmpleado.Text = filaSeleccionada.Cells["Nombre"].Value?.ToString();
                txtApellidoEmpleado.Text = filaSeleccionada.Cells["Apellido"].Value?.ToString();
                txtTelefonoEmpleado.Text = filaSeleccionada.Cells["Telefono"].Value?.ToString();
                txtCorreoEmpleado.Text = filaSeleccionada.Cells["Correo"].Value?.ToString();
                txtDireccionEmpleado.Text = filaSeleccionada.Cells["Direccion"].Value?.ToString();
                cmbEspecialidadEmpleado.SelectedValue = filaSeleccionada.Cells["EspecialidadID"].Value?.ToString();
                cmbAreaEmpleado.SelectedValue = filaSeleccionada.Cells["AreaID"].Value?.ToString();
            }
        }

        private void btnAgregarEmpleado_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (txtNombreEmpleado.Text.Trim().Length == 0) { errorProvider1.SetError(txtNombreMascota, "El nombre es obligatorio"); return; };
                if (txtApellidoEmpleado.Text.Trim().Length == 0) { errorProvider1.SetError(txtRazaMascota, "El apellido es obligatorio"); return; };
                if (txtTelefonoEmpleado.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaMascota, "El telefon es obligatorio"); return; };
                if (txtCorreoEmpleado.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaMascota, "El correo es obligatorio"); return; };
                if (txtDireccionEmpleado.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaMascota, "La direccion es obligatoria"); return; };
                if (cmbEspecialidadEmpleado.Text.Length == 0) { errorProvider1.SetError(cmbCliente, "La especialidad es obligatoria"); return; };
                if (cmbAreaEmpleado.Text.Length == 0) { errorProvider1.SetError(cmbEspecie, "El area es obligatoria"); return; };
                errorProvider1.Clear();


                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spInsertarEmpleado", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", txtNombreEmpleado.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellidoEmpleado.Text);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefonoEmpleado.Text);
                cmd.Parameters.AddWithValue("@Correo", txtCorreoEmpleado.Text);
                cmd.Parameters.AddWithValue("@Direccion", txtDireccionEmpleado.Text);
                cmd.Parameters.AddWithValue("@EspecialidadID", cmbEspecialidadEmpleado.SelectedValue);
                cmd.Parameters.AddWithValue("@AreaID", cmbAreaEmpleado.SelectedValue);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtNombreEmpleado.Text = "";
                txtApellidoEmpleado.Text = "";
                txtTelefonoEmpleado.Text = "";
                txtCorreoEmpleado.Text = "";
                txtDireccionEmpleado.Text = "";

                CargarInformacion("spSelectEmpleado", gridEmpleados); // Actualiza DataGridView

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnActualizarEmpleado_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (txtNombreEmpleado.Text.Trim().Length == 0) { errorProvider1.SetError(txtNombreEmpleado, "El nombre es obligatorio"); return; };
                if (txtApellidoEmpleado.Text.Trim().Length == 0) { errorProvider1.SetError(txtApellidoEmpleado, "El apellido es obligatorio"); return; };
                if (txtTelefonoEmpleado.Text.Trim().Length == 0) { errorProvider1.SetError(txtTelefonoEmpleado, "El telefono es obligatorio"); return; };
                if (txtCorreoEmpleado.Text.Trim().Length == 0) { errorProvider1.SetError(txtCorreoEmpleado, "El correo es obligatorio"); return; };
                if (txtDireccionEmpleado.Text.Trim().Length == 0) { errorProvider1.SetError(txtDireccionEmpleado, "La direccion es obligatoria"); return; };
                if (cmbEspecialidadEmpleado.Text.Length == 0) { errorProvider1.SetError(cmbEspecialidadEmpleado, "La especialidad es obligatoria"); return; };
                if (cmbAreaEmpleado.Text.Length == 0) { errorProvider1.SetError(cmbAreaEmpleado, "El area es obligatoria"); return; };
                errorProvider1.Clear();

                DataGridViewRow fila = gridEmpleados.CurrentRow;
                int empleado = Convert.ToInt16(fila.Cells["EmpleadoID"].Value.ToString());


                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spUpdateEmpleado", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", txtNombreEmpleado.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellidoEmpleado.Text);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefonoEmpleado.Text);
                cmd.Parameters.AddWithValue("@Correo", txtCorreoEmpleado.Text);
                cmd.Parameters.AddWithValue("@Direccion", txtDireccionEmpleado.Text);
                cmd.Parameters.AddWithValue("@EspecialidadID", cmbEspecialidadEmpleado.SelectedValue);
                cmd.Parameters.AddWithValue("@AreaID", cmbAreaEmpleado.SelectedValue);
                cmd.Parameters.AddWithValue("@EmpleadoID", empleado);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtNombreEmpleado.Text = "";
                txtApellidoEmpleado.Text = "";
                txtTelefonoEmpleado.Text = "";
                txtCorreoEmpleado.Text = "";
                txtDireccionEmpleado.Text = "";


                CargarInformacion("spSelectEmpleado", gridEmpleados); // Actualiza DataGridView

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnEliminarEmpleado_Click(object sender, EventArgs e)
        {
            if (gridEmpleados.CurrentRow != null)
            {
                DataGridViewRow fila = gridEmpleados.CurrentRow;
                int empleado = Convert.ToInt16(fila.Cells["EmpleadoID"].Value.ToString());
                // Mostrar diálogo de confirmación
                DialogResult resultado = MessageBox.Show("¿Estás seguro de querer eliminar el registro?", "Confirmación de Eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.No) { return; }
                try
                {
                    string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
                    SqlConnection con = new SqlConnection(cnx);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spDeleteEmpleado", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpleadoID", empleado);
                    cmd.ExecuteNonQuery();
                    con.Close(); // Cerrar la conexión

                    this.DialogResult = DialogResult.OK;
                    CargarInformacion("spSelectEmpleado", gridEmpleados);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtNombreEmpleado_TextChanged(object sender, EventArgs e)
        {
            GenerarCorreo();
        }

        private void txtApellidoEmpleado_TextChanged(object sender, EventArgs e)
        {
            GenerarCorreo();
        }

        private void btnAgregarFactura_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (cmbCuentaFactura.Text.Length == 0) { errorProvider1.SetError(cmbCuentaFactura, "La cuenta es obligatorio"); return; };
                if (cmbClienteFactura.Text.Length == 0) { errorProvider1.SetError(cmbClienteFactura, "El cliente es obligatoria"); return; };
                if (cmbMPFactura.Text.Length == 0) { errorProvider1.SetError(cmbMPFactura, "El metodo de pago es obligatorio"); return; };
                if (cmbEstadoFactura.Text.Length == 0) { errorProvider1.SetError(cmbEstadoFactura, "El estado es obligatorio"); return; };
                if (Double.Parse(txtSubtotalFactura.Text) <= 0) { errorProvider1.SetError(txtSubtotalFactura, "El Subtotal debe ser mayor a 0"); return; };
                errorProvider1.Clear();


                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spInsertarFactura", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClienteID", cmbClienteFactura.SelectedValue);
                cmd.Parameters.AddWithValue("@Subtotal", Double.Parse(txtSubtotalFactura.Text));
                cmd.Parameters.AddWithValue("@Estado", cmbEstadoFactura.SelectedItem);
                cmd.Parameters.AddWithValue("@MetodoPago", cmbMPFactura.SelectedItem);
                cmd.Parameters.AddWithValue("@CuentaID", cmbCuentaFactura.SelectedValue);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                txtSubtotalFactura.Text = "0.00";
                CargarInformacion("spSelectFactura", gridFactura); // Actualiza DataGridView

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnActualizarFactura_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (cmbCuentaFactura.Text.Length == 0) { errorProvider1.SetError(cmbCuentaFactura, "La cuenta es obligatorio"); return; };
                if (cmbClienteFactura.Text.Length == 0) { errorProvider1.SetError(cmbClienteFactura, "El cliente es obligatoria"); return; };
                if (cmbMPFactura.Text.Length == 0) { errorProvider1.SetError(cmbMPFactura, "El metodo de pago es obligatorio"); return; };
                if (cmbEstadoFactura.Text.Length == 0) { errorProvider1.SetError(cmbEstadoFactura, "El estado es obligatorio"); return; };
                if (Double.Parse(txtSubtotalFactura.Text) <= 0) { errorProvider1.SetError(txtSubtotalFactura, "El Subtotal debe ser mayor a 0"); return; };
                errorProvider1.Clear();

                DataGridViewRow fila = gridFactura.CurrentRow;
                int factura = Convert.ToInt16(fila.Cells["FacturaID"].Value.ToString());
                double subtotal = Convert.ToDouble(txtSubtotalFactura.Text);

                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spUpdateFactura", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClienteID", cmbClienteFactura.SelectedValue);
                cmd.Parameters.AddWithValue("@Subtotal", subtotal);
                cmd.Parameters.AddWithValue("@Estado", cmbEstadoFactura.SelectedItem);
                cmd.Parameters.AddWithValue("@MetodoPago", cmbMPFactura.SelectedItem);
                cmd.Parameters.AddWithValue("@CuentaID", cmbCuentaFactura.SelectedValue);
                cmd.Parameters.AddWithValue("@FacturaID", factura);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                txtSubtotalFactura.Text = "0.00";
                CargarInformacion("spSelectFactura", gridFactura); // Actualiza DataGridView

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnEliminarFactura_Click(object sender, EventArgs e)
        {
            if (gridFactura.CurrentRow != null)
            {
                DataGridViewRow fila = gridFactura.CurrentRow;
                int factura = Convert.ToInt16(fila.Cells["FacturaID"].Value.ToString());
                // Mostrar diálogo de confirmación
                DialogResult resultado = MessageBox.Show("¿Estás seguro de querer eliminar el registro?", "Confirmación de Eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.No) { return; }
                try
                {
                    string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
                    SqlConnection con = new SqlConnection(cnx);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spDeleteFactura", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FacturaID", factura);
                    cmd.ExecuteNonQuery();
                    con.Close(); // Cerrar la conexión

                    this.DialogResult = DialogResult.OK;
                    CargarInformacion("spSelectFactura", gridFactura);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void gridFactura_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow filaSeleccionada = gridFactura.Rows[e.RowIndex];
                cmbClienteFactura.SelectedValue = filaSeleccionada.Cells["ClienteID"].Value?.ToString();
                if (filaSeleccionada.Cells["CuentaID"].Value?.ToString() == "")
                {
                    cmbCuentaFactura.SelectedValue = "";
                } else
                {
                    cmbCuentaFactura.SelectedValue = filaSeleccionada.Cells["CuentaID"].Value?.ToString();
                }
                cmbMPFactura.SelectedItem = filaSeleccionada.Cells["MetodoPago"].Value?.ToString();
                txtSubtotalFactura.Text = filaSeleccionada.Cells["Subtotal"].Value?.ToString();
                cmbEstadoFactura.SelectedItem = filaSeleccionada.Cells["Estado"].Value?.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregarCompra_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (cmbProveedorCompra.Text.Length == 0) { errorProvider1.SetError(cmbProveedorCompra, "El proveedor es obligatorio"); return; };
                if (Double.Parse(txtTotalCompra.Text) <= 0) { errorProvider1.SetError(txtTotalCompra, "El total debe ser mayor a 0"); return; };
                if (cmbEstadoCompra.Text.Length == 0) { errorProvider1.SetError(cmbEstadoCompra, "El estado es obligatorio"); return; };
                errorProvider1.Clear();

                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spInsertarCompra", con);

                cmd.CommandType = CommandType.StoredProcedure;

                double total = Convert.ToDouble(txtTotalCompra.Text);
                cmd.Parameters.AddWithValue("@ProveedorID", cmbProveedorCompra.SelectedValue);
                cmd.Parameters.AddWithValue("@Total", total);
                cmd.Parameters.AddWithValue("@Estado", cmbEstadoCompra.SelectedItem);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtTotalCompra.Text = "0.00";
                CargarInformacion("spSelectCompra", gridCompra); // Actualiza DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActualizarCompra_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (cmbProveedorCompra.Text.Length == 0) { errorProvider1.SetError(cmbProveedorCompra, "El proveedor es obligatorio"); return; };
                if (Double.Parse(txtTotalCompra.Text) <= 0) { errorProvider1.SetError(txtTotalCompra, "El total debe ser mayor a 0"); return; };
                if (cmbEstadoCompra.Text.Length == 0) { errorProvider1.SetError(cmbEstadoCompra, "El estado es obligatorio"); return; };
                errorProvider1.Clear();

                DataGridViewRow fila = gridCompra.CurrentRow;
                int compra = Convert.ToInt16(fila.Cells["CompraID"].Value.ToString());

                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spUpdateCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;

                double total = Convert.ToDouble(txtTotalCompra.Text);
                cmd.Parameters.AddWithValue("@ProveedorID", cmbProveedorCompra.SelectedValue);
                cmd.Parameters.AddWithValue("@Total", total);
                cmd.Parameters.AddWithValue("@Estado", cmbEstadoCompra.SelectedItem);
                cmd.Parameters.AddWithValue("@CompraID", compra);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtTotalCompra.Text = "0.00";
                CargarInformacion("spSelectCompra", gridCompra); // Actualiza DataGridView
            }
            catch (Exception ex){ MessageBox.Show(ex.Message); }
        }

        private void btnEliminarCompra_Click(object sender, EventArgs e)
        {
            if (gridCompra.CurrentRow != null)
            {
                DataGridViewRow fila = gridCompra.CurrentRow;
                int compra = Convert.ToInt16(fila.Cells["CompraID"].Value.ToString());
                // Mostrar diálogo de confirmación
                DialogResult resultado = MessageBox.Show("¿Estás seguro de querer eliminar el registro?", "Confirmación de Eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.No) { return; }
                try
                {
                    string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
                    SqlConnection con = new SqlConnection(cnx);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spDeleteCompra", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompraID", compra);
                    cmd.ExecuteNonQuery();
                    con.Close(); // Cerrar la conexión
                    this.DialogResult = DialogResult.OK;
                    CargarInformacion("spSelectCompra", gridCompra);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void gridCompra_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow filaSeleccionada = gridCompra.Rows[e.RowIndex];
                cmbProveedorCompra.SelectedValue = filaSeleccionada.Cells["ProveedorID"].Value?.ToString();
                txtTotalCompra.Text = filaSeleccionada.Cells["Total"].Value?.ToString();
                cmbEstadoCompra.SelectedItem = filaSeleccionada.Cells["Estado"].Value?.ToString();
            }
        }

        private void gridPagos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow filaSeleccionada = gridPagos.Rows[e.RowIndex];
                cmbCompraPago.SelectedValue = filaSeleccionada.Cells["CompraID"].Value?.ToString();
                cmbMPPago.SelectedItem = filaSeleccionada.Cells["MetodoPago"].Value?.ToString();
                txtMontoPago.Text = filaSeleccionada.Cells["Monto"].Value?.ToString();
                cmbCuentaPago.SelectedValue = filaSeleccionada.Cells["CuentaID"].Value?.ToString();
                txtDescripcionPago.Text = filaSeleccionada.Cells["Descripcion"].Value?.ToString();
            }
        }

        private void btnAgregarPago_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (cmbCompraPago.Text.Length == 0) { errorProvider1.SetError(cmbCompraPago, "La compra es obligatorio"); return; };
                if (cmbMPPago.Text.Length == 0) { errorProvider1.SetError(cmbMPPago, "El metodo de pago es obligatorio"); return; };
                if (txtMontoPago.Text.Trim().Length == 0) { errorProvider1.SetError(txtMontoPago, "El monto es obligatorio"); return; };
                if (cmbCuentaPago.Text.Length == 0) { errorProvider1.SetError(cmbCuentaPago, "La cuenta es obligatoria"); return; };
                if (txtDescripcionPago.Text.Trim().Length == 0) { errorProvider1.SetError(txtDescripcionPago, "La descripcion es obligatoria"); return; };
                errorProvider1.Clear();

                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spInsertarPago", con);
                cmd.CommandType = CommandType.StoredProcedure;

                double monto = Convert.ToDouble(txtMontoPago.Text);
                cmd.Parameters.AddWithValue("@CompraID", cmbCompraPago.SelectedValue);
                cmd.Parameters.AddWithValue("@Descripcion", txtDescripcionPago.Text);
                cmd.Parameters.AddWithValue("@Monto", monto);
                cmd.Parameters.AddWithValue("@MetodoPago", cmbMPPago.SelectedItem);
                cmd.Parameters.AddWithValue("@CuentaID", cmbCuentaPago.SelectedValue);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtMontoPago.Text = "";
                txtDescripcionPago.Text = "";
                CargarInformacion("spSelectPago", gridPagos); // Actualiza DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActualizarPago_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (cmbCompraPago.Text.Length == 0) { errorProvider1.SetError(cmbCompraPago, "La compra es obligatorio"); return; };
                if (cmbMPPago.Text.Length == 0) { errorProvider1.SetError(cmbMPPago, "El metodo de pago es obligatorio"); return; };
                if (txtMontoPago.Text.Trim().Length == 0) { errorProvider1.SetError(txtMontoPago, "El monto es obligatorio"); return; };
                if (cmbCuentaPago.Text.Length == 0) { errorProvider1.SetError(cmbCuentaPago, "La cuenta es obligatoria"); return; };
                if (txtDescripcionPago.Text.Trim().Length == 0) { errorProvider1.SetError(txtDescripcionPago, "La descripcion es obligatoria"); return; };
                errorProvider1.Clear();

                DataGridViewRow fila = gridPagos.CurrentRow;
                int pago = Convert.ToInt16(fila.Cells["PagoID"].Value.ToString());

                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spUpdatePago", con);
                cmd.CommandType = CommandType.StoredProcedure;

                double monto = Convert.ToDouble(txtMontoPago.Text);
                cmd.Parameters.AddWithValue("@CompraID", cmbCompraPago.SelectedValue);
                cmd.Parameters.AddWithValue("@Descripcion", txtDescripcionPago.Text);
                cmd.Parameters.AddWithValue("@Monto", monto);
                cmd.Parameters.AddWithValue("@MetodoPago", cmbMPPago.SelectedItem);
                cmd.Parameters.AddWithValue("@CuentaID", cmbCuentaPago.SelectedValue);
                cmd.Parameters.AddWithValue("@PagoID", pago);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                CargarInformacion("spSelectPago", gridPagos); // Actualiza DataGridView
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnEliminarPago_Click(object sender, EventArgs e)
        {
            if (gridPagos.CurrentRow != null)
            {
                DataGridViewRow fila = gridPagos.CurrentRow;
                int pago = Convert.ToInt16(fila.Cells["PagoID"].Value.ToString());
                // Mostrar diálogo de confirmación
                DialogResult resultado = MessageBox.Show("¿Estás seguro de querer eliminar el registro?", "Confirmación de Eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.No) { return; }
                try
                {
                    string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
                    SqlConnection con = new SqlConnection(cnx);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spDeletePago", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PagoID", pago);
                    cmd.ExecuteNonQuery();
                    con.Close(); // Cerrar la conexión
                    this.DialogResult = DialogResult.OK;
                    CargarInformacion("spSelectPago", gridPagos);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnAgregarCita_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (cmbMascotaCita.Text.Length == 0) { errorProvider1.SetError(cmbMascotaCita, "La mascota es obligatorio"); return; };
                if (txtFechaCita.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaCita, "La fecha es obligatoria"); return; };
                if (cmbEstadoCita.Text.Length == 0) { errorProvider1.SetError(cmbEstadoCita, "El estado de la cita es obligatorio"); return; };
                if (txtMotivoCita.Text.Trim().Length == 0) { errorProvider1.SetError(txtMotivoCita, "El motivo es obligatorio"); return; };
                errorProvider1.Clear();

                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spInsertarCita", con);
                cmd.CommandType = CommandType.StoredProcedure;

                //double monto = Convert.ToDouble(txtMontoPago.Text);

                cmd.Parameters.AddWithValue("@MascotaID", cmbMascotaCita.SelectedValue);
                cmd.Parameters.AddWithValue("@Fecha", txtFechaCita.Text);
                cmd.Parameters.AddWithValue("@Motivo", txtMotivoCita.Text);
                cmd.Parameters.AddWithValue("@Estado", cmbEstadoCita.SelectedItem);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtFechaCita.Text = "";
                txtMotivoCita.Text = "";
                CargarInformacion("spSelectCita", gridCitas); // Actualiza DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAgregarFicha_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (cmbMascotaFicha.Text.Length == 0) { errorProvider1.SetError(cmbMascotaCita, "La mascota es obligatorio"); return; };
                if (txtMotivoFicha.Text.Trim().Length == 0) { errorProvider1.SetError(txtMotivoCita, "El motivo es obligatorio"); return; };
                if (txtObservacionesFicha.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaCita, "Las observaciones son obligatorias"); return; };
                if (cmbEmpleadoFicha.Text.Length == 0) { errorProvider1.SetError(cmbMascotaCita, "El empleado es obligatorio"); return; };
                if (txtContactoFicha.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaCita, "El contacto es obligatorio"); return; };
                if (txtTelefonoFicha.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaCita, "El telefono es oblitgatorio"); return; };
                if (cmbTipoFicha.Text.Length == 0) { errorProvider1.SetError(cmbEstadoCita, "El tipo es obligatorio"); return; };
                if (cmbEstadoFicha.Text.Length == 0) { errorProvider1.SetError(cmbEstadoCita, "El estado es obligatorio"); return; };
                if (cmbCitaFicha.Text.Length == 0) { errorProvider1.SetError(cmbEstadoCita, "La cita es obligatoria"); return; };
                errorProvider1.Clear();

                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spInsertarFichaRecepcion", con);
                cmd.CommandType = CommandType.StoredProcedure;

                //double monto = Convert.ToDouble(txtMontoPago.Text);

                cmd.Parameters.AddWithValue("@MascotaID", cmbMascotaFicha.SelectedValue);
                cmd.Parameters.AddWithValue("@Motivo", txtMotivoFicha.Text);
                cmd.Parameters.AddWithValue("@Observaciones", txtObservacionesFicha.Text);
                cmd.Parameters.AddWithValue("@EmpleadoID", cmbEmpleadoFicha.SelectedValue);
                cmd.Parameters.AddWithValue("@Contacto", txtContactoFicha.Text);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefonoFicha.Text);
                cmd.Parameters.AddWithValue("@Tipo", cmbTipoFicha.SelectedItem);
                cmd.Parameters.AddWithValue("@Estado", cmbEstadoFicha.SelectedItem);
                cmd.Parameters.AddWithValue("@CitaID", cmbCitaFicha.SelectedValue);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                txtMotivoFicha.Text = "";
                txtObservacionesFicha.Text = "";
                txtContactoFicha.Text = "";
                txtTelefonoFicha.Text = "";

                CargarInformacion("spSelectFichaRecepcion", gridFichas); // Actualiza DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActualizarCita_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (cmbMascotaCita.Text.Length == 0) { errorProvider1.SetError(cmbMascotaCita, "La mascota es obligatorio"); return; };
                if (txtFechaCita.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaCita, "La fecha es obligatoria"); return; };
                if (cmbEstadoCita.Text.Length == 0) { errorProvider1.SetError(cmbEstadoCita, "El estado de la cita es obligatorio"); return; };
                if (txtMotivoCita.Text.Trim().Length == 0) { errorProvider1.SetError(txtMotivoCita, "El motivo es obligatorio"); return; };
                errorProvider1.Clear();

                DataGridViewRow fila = gridCitas.CurrentRow;
                int cita = Convert.ToInt16(fila.Cells["CitaID"].Value.ToString());

                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spUpdateCita", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MascotaID", cmbMascotaCita.SelectedValue);
                cmd.Parameters.AddWithValue("@Fecha", txtFechaCita.Text);
                cmd.Parameters.AddWithValue("@Motivo", txtMotivoCita.Text);
                cmd.Parameters.AddWithValue("@Estado", cmbEstadoCita.SelectedItem);
                cmd.Parameters.AddWithValue("@CitaID", cita);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                CargarInformacion("spSelectCita", gridCitas); // Actualiza DataGridView

                txtFechaCita.Text = "";
                txtMotivoFicha.Text = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnActualizarFicha_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
            try
            {
                if (cmbMascotaFicha.Text.Length == 0) { errorProvider1.SetError(cmbMascotaCita, "La mascota es obligatorio"); return; };
                if (txtMotivoFicha.Text.Trim().Length == 0) { errorProvider1.SetError(txtMotivoCita, "El motivo es obligatorio"); return; };
                if (txtObservacionesFicha.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaCita, "Las observaciones son obligatorias"); return; };
                if (cmbEmpleadoFicha.Text.Length == 0) { errorProvider1.SetError(cmbMascotaCita, "El empleado es obligatorio"); return; };
                if (txtContactoFicha.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaCita, "El contacto es obligatorio"); return; };
                if (txtTelefonoFicha.Text.Trim().Length == 0) { errorProvider1.SetError(txtFechaCita, "El telefono es oblitgatorio"); return; };
                if (cmbTipoFicha.Text.Length == 0) { errorProvider1.SetError(cmbEstadoCita, "El tipo es obligatorio"); return; };
                if (cmbEstadoFicha.Text.Length == 0) { errorProvider1.SetError(cmbEstadoCita, "El estado es obligatorio"); return; };
                if (cmbCitaFicha.Text.Length == 0) { errorProvider1.SetError(cmbEstadoCita, "La cita es obligatoria"); return; };
                errorProvider1.Clear();

                DataGridViewRow fila = gridFichas.CurrentRow;
                int ficha = Convert.ToInt16(fila.Cells["FichaID"].Value.ToString());

                SqlConnection con = new SqlConnection(cnx);
                SqlCommand cmd = new SqlCommand("spUpdateFichaRecepcion", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MascotaID", cmbMascotaFicha.SelectedValue);
                cmd.Parameters.AddWithValue("@Motivo", txtMotivoFicha.Text);
                cmd.Parameters.AddWithValue("@Observaciones", txtObservacionesFicha.Text);
                cmd.Parameters.AddWithValue("@EmpleadoID", cmbEmpleadoFicha.SelectedValue);
                cmd.Parameters.AddWithValue("@Contacto", txtContactoFicha.Text);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefonoFicha.Text);
                cmd.Parameters.AddWithValue("@Tipo", cmbTipoFicha.SelectedItem);
                cmd.Parameters.AddWithValue("@Estado", cmbEstadoFicha.SelectedItem);
                cmd.Parameters.AddWithValue("@CitaID", cmbCitaFicha.SelectedValue);
                cmd.Parameters.AddWithValue("@FichaID", ficha);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                CargarInformacion("spSelectFichaRecepcion", gridFichas); // Actualiza DataGridView

                txtMotivoFicha.Text = "";
                txtObservacionesFicha.Text = "";
                txtContactoFicha.Text = "";
                txtTelefonoFicha.Text = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void btnEliminarCita_Click(object sender, EventArgs e)
        {
            if (gridCitas.CurrentRow != null)
            {
                DataGridViewRow fila = gridCitas.CurrentRow;
                int cita = Convert.ToInt16(fila.Cells["CitaID"].Value.ToString());
                // Mostrar diálogo de confirmación
                DialogResult resultado = MessageBox.Show("¿Estás seguro de querer eliminar el registro?", "Confirmación de Eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.No) { return; }
                try
                {
                    string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
                    SqlConnection con = new SqlConnection(cnx);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spDeleteCita", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CitaID", cita);
                    cmd.ExecuteNonQuery();
                    con.Close(); // Cerrar la conexión
                    this.DialogResult = DialogResult.OK;
                    CargarInformacion("spSelectCita", gridCitas);

                    txtFechaCita.Text = "";
                    txtMotivoFicha.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnEliminarFicha_Click(object sender, EventArgs e)
        {
            if (gridFichas.CurrentRow != null)
            {
                DataGridViewRow fila = gridFichas.CurrentRow;
                int ficha = Convert.ToInt16(fila.Cells["FichaID"].Value.ToString());
                // Mostrar diálogo de confirmación
                DialogResult resultado = MessageBox.Show("¿Estás seguro de querer eliminar el registro?", "Confirmación de Eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.No) { return; }
                try
                {
                    string cnx = "Server=3.128.144.165;Database=DB20212000761;User Id=jeffry.valle;Password=JV20212000761;";
                    SqlConnection con = new SqlConnection(cnx);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spDeleteFichaRecepcion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FichaID", ficha);
                    cmd.ExecuteNonQuery();
                    con.Close(); // Cerrar la conexión
                    this.DialogResult = DialogResult.OK;
                    CargarInformacion("spSelectFichaRecepcion", gridFichas);

                    txtMotivoFicha.Text = "";
                    txtObservacionesFicha.Text = "";
                    txtContactoFicha.Text = "";
                    txtTelefonoFicha.Text = "";

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void gridCitas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow filaSeleccionada = gridCitas.Rows[e.RowIndex];
                cmbMascotaCita.SelectedValue = filaSeleccionada.Cells["MascotaID"].Value?.ToString();
                txtFechaCita.Text = filaSeleccionada.Cells["Fecha"].Value?.ToString();
                txtMotivoCita.Text = filaSeleccionada.Cells["Motivo"].Value?.ToString();
                cmbEstadoCita.SelectedItem = filaSeleccionada.Cells["Estado"].Value?.ToString();
            }
        }
        private void gridFichas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = gridFichas.Rows[e.RowIndex];
                cmbMascotaFicha.SelectedValue = fila.Cells["MascotaID"].Value?.ToString();
                txtMotivoFicha.Text = fila.Cells["Motivo"].Value?.ToString();
                txtObservacionesFicha.Text = fila.Cells["Observaciones"].Value?.ToString();
                cmbEmpleadoFicha.SelectedValue = fila.Cells["EmpleadoID"].Value?.ToString();
                txtContactoFicha.Text = fila.Cells["Contacto"].Value?.ToString();
                txtTelefonoFicha.Text = fila.Cells["Telefono"].Value?.ToString();
                cmbTipoFicha.SelectedItem = fila.Cells["Tipo"].Value?.ToString();
                cmbEstadoFicha.SelectedItem = fila.Cells["Estado"].Value?.ToString();
                cmbCitaFicha.SelectedValue = fila.Cells["CitaID"].Value?.ToString();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label49_Click(object sender, EventArgs e)
        {

        }

        private void label50_Click(object sender, EventArgs e)
        {

        }

        private void label48_Click(object sender, EventArgs e)
        {

        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void label42_Click(object sender, EventArgs e)
        {

        }

        private void label44_Click(object sender, EventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void label46_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
