using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;


namespace MiPOSCSharpMySQL.Formularios
{
    public partial class FormVentas : Form
    {
        public FormVentas()
        {
            InitializeComponent();

            txtIdCliente.ReadOnly = true;
            txtNombreCliente.ReadOnly = true;
            txtApPaterno.ReadOnly = true;
            txtApMaterno.ReadOnly = true;

            txtIdProducto.ReadOnly = true;
            txtNombreProducto.ReadOnly = true;
            txtPrecio.ReadOnly = true;
            txtStock.ReadOnly = true;
            txtPrecioVentaFinal.ReadOnly = true;

            Controlador.ControladorVenta objetoVenta = new Controlador.ControladorVenta();
            objetoVenta.MostrarUltimaFactura(lbUltimaFactura);

            //Controlador.ControladorProducto objetoProducto = new Controlador.ControladorProducto();
            //Controlador.ControladorCliente objetoCliente = new Controlador.ControladorCliente();
            //objetoProducto.MostrarProductos(dgvProducto);
            //objetoCliente.MostrarClientes(dgvCliente);
        }


        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label13_Click(object sender, EventArgs e)
        {

        }
        private void label12_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label7_Click_1(object sender, EventArgs e)
        {

        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void label10_Click(object sender, EventArgs e)
        {

        }
        private void dgvCarrito_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void txtPrecioVentaFinal_TextChanged(object sender, EventArgs e)
        {

        }
        private void dgvCliente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void txtApPaterno_TextChanged(object sender, EventArgs e)
        {

        }


        /*----------------------------------------------------------------------------------------------------------------------*/


        private void txtBuscarCliente_TextChanged(object sender, EventArgs e)
        {
            Controlador.ControladorVenta objetoVenta = new Controlador.ControladorVenta();
            objetoVenta.BuscarClientes(txtBuscarCliente, dgvCliente);
        }

        private void txtBuscarProducto_TextChanged(object sender, EventArgs e)
        {
            Controlador.ControladorVenta objetoVenta = new Controlador.ControladorVenta();
            objetoVenta.BuscarProductos(txtBuscarProducto ,dgvProducto);

        }

        private void dgvProducto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Controlador.ControladorVenta objetoVenta = new Controlador.ControladorVenta();
            objetoVenta.SeleccionarProductoVenta(dgvProducto, txtIdProducto, txtNombreProducto, txtPrecio, txtStock, txtPrecioVentaFinal);
        }

        private void dgvCliente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Controlador.ControladorVenta objetoVenta = new Controlador.ControladorVenta();
            objetoVenta.SeleccionarClienteVenta(dgvCliente, txtIdCliente, txtNombreCliente, txtApPaterno, txtApMaterno);
        }

        private void btnHabilitar_Click(object sender, EventArgs e)
        {
            txtPrecioVentaFinal.ReadOnly = false;
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            txtPrecioVentaFinal.ReadOnly = true;
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            Controlador.ControladorVenta objetoVenta = new Controlador.ControladorVenta();
            objetoVenta.PasarProductosVenta(dgvCarrito, txtIdProducto, txtNombreProducto, txtPrecioVentaFinal, txtStockVenta, txtStock);
            objetoVenta.CalcularTotal(dgvCarrito, lbIva,lbTotal);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Controlador.ControladorVenta objetoVenta = new Controlador.ControladorVenta();
            objetoVenta.EliminarSeleccion(dgvCarrito);
        }

        private async void btnFacturar_Click(object sender, EventArgs e)
        {
            //Controlador.ControladorVenta objetoVenta = new Controlador.ControladorVenta();
            //objetoVenta.CrearFactura(txtIdCliente);

            //objetoVenta.RealizarVentaV2(dgvCarrito,idFactura:0); //OJO

            //objetoVenta.LimpiarCamposVenta(txtBuscarCliente, dgvCliente,txtBuscarProducto,dgvProducto,txtIdCliente,txtNombreCliente, txtApPaterno, txtApMaterno, 
            //                               txtIdProducto, txtNombreProducto, txtPrecio, txtStock, txtPrecioVentaFinal, txtStockVenta, dgvCarrito, lbIva, lbTotal);
            //objetoVenta.MostrarUltimaFactura(lbUltimaFactura);

            Controlador.ControladorReporte objetoReporte = new Controlador.ControladorReporte();
            Controlador.ControladorVenta objetoVenta = new Controlador.ControladorVenta();

            // Creamos la factura y obtenemos su ID
            long idFactura = objetoVenta.CrearFacturaV2(txtIdCliente);

            // Si hubo un error, terminamos
            if (idFactura == -1) return;

            // Registramos los productos vendidos
            objetoVenta.RealizarVentaV2(dgvCarrito, idFactura);


            // Limpiamos la pantalla
            objetoVenta.LimpiarCamposVenta(txtBuscarCliente, dgvCliente, txtBuscarProducto, dgvProducto, txtIdCliente, txtNombreCliente,
                                            txtApPaterno, txtApMaterno, txtIdProducto, txtNombreProducto, txtPrecio, txtStock,
                                            txtPrecioVentaFinal, txtStockVenta, dgvCarrito, lbIva, lbTotal);

            // Mostramos la última factura generada
            objetoVenta.MostrarUltimaFactura(lbUltimaFactura);
 
            //Esperar que todos los datos se guarden correctamente
            await Task.Delay(1000);

            // Imprimimos la factura
            if (long.TryParse(lbUltimaFactura.Text, out long ultimaFactura))
            {
                objetoReporte.ImprimirFactura(ultimaFactura); // Ahora acepta long directamente
            }
            else
            {
                MessageBox.Show("Error al obtener el número de la última factura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }

    }
}
