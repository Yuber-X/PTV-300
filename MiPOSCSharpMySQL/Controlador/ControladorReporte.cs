using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MiPOSCSharpMySQL.Controlador
{
    internal class ControladorReporte
    {
        public void MostrarDatosFactura(TextBox numeroFactura, Label numeroFacturaEncontrado, Label fechaFacturaEncontrado,
                                        Label nombreClienteEncontrado, Label appaternoEncontrado, Label apmaternoEncontrado) 
        {

            Configuracion.CConexion objetoConexion = new Configuracion.CConexion();

            string consulta = "select factura.idfactura, factura.fechaFactura, cliente.nombres, cliente.appaterno, cliente.appmaterno from factura INNER JOIN cliente ON cliente.idCliente = factura.fkcliente WHERE factura.idFactura = @idfactura;";

            try
            {
                MySqlConnection conexion = objetoConexion.estableceConexion();

                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@idFactura", long.Parse(numeroFactura.Text));

                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    numeroFacturaEncontrado.Text = reader["idfactura"].ToString();
                    fechaFacturaEncontrado.Text = DateTime.Parse(reader["fechaFactura"].ToString()).ToString("dd-MM-yyyy");
                    nombreClienteEncontrado.Text = reader["nombres"].ToString();
                    appaternoEncontrado.Text = reader["appaterno"].ToString();
                    apmaternoEncontrado.Text = reader["appmaterno"].ToString();
                }
                else
                {
                    numeroFacturaEncontrado.Text = "-------";
                    fechaFacturaEncontrado.Text = "-------";
                    nombreClienteEncontrado.Text = "-------";
                    appaternoEncontrado.Text = "-------";
                    apmaternoEncontrado.Text = "-------";

                    MessageBox.Show("No se encontró la factura");
                }
            }
            catch (Exception e)
            {

                MessageBox.Show("Error al Mostrar Factura: " + e.ToString());

            }
            finally
            {
                objetoConexion.CerrarConexion();
            }
        }
        public void MostrarVentaFactura(TextBox numeroFactura, DataGridView tablaTotalProductos, Label iva, Label total)
        {

            Configuracion.CConexion objetoConexion = new Configuracion.CConexion();


            DataTable modelo = new DataTable();

            modelo.Columns.Add("N.Producto", typeof(string));
            modelo.Columns.Add("Cantidad", typeof(string));
            modelo.Columns.Add("PrecioVenta", typeof(double));
            modelo.Columns.Add("Subtotal", typeof(int));

            NumberFormatInfo formato = new NumberFormatInfo();
            formato.NumberDecimalDigits = 2;

            try
            {
                string sql = "select producto.nombre, detalle.cantidad, detalle.precioventa from detalle INNER JOIN factura ON factura.idfactura = detalle.fkfactura INNER JOIN producto ON producto.idproducto = detalle.fkproducto WHERE factura.idfactura = @idfactura;";
                
                MySqlConnection conexion = objetoConexion.estableceConexion();

                MySqlCommand comando = new MySqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@idfactura", int.Parse(numeroFactura.Text));

                MySqlDataReader rs = comando.ExecuteReader();

                double totalFactura = 0.0;
                double valorIVA = 0.18;

                while (rs.Read())
                {
                    string nombreProducto = rs["nombre"].ToString();
                    int cantidad = rs.GetInt32("cantidad");
                    double precioVenta = rs.GetDouble("precioVenta");

                    double subtotal = cantidad * precioVenta; 

                    totalFactura += subtotal;

                    modelo.Rows.Add(nombreProducto, cantidad, precioVenta, subtotal);
                }

                double totalIVA = totalFactura * valorIVA;

                iva.Text = totalIVA.ToString("N", formato);
                total.Text = totalFactura.ToString("N", formato);

            }
            catch (Exception e)
            {
                MessageBox.Show ("Error al mostrar Datos: " + e.ToString());
            }
            finally
            {
                objetoConexion.CerrarConexion();
            }
        }

        public void MostrarVentaPorFecha(DateTimePicker desde, DateTimePicker hasta, DataGridView tablaVenta, Label totalGenaral)
        {

            Configuracion.CConexion objetoConexion = new Configuracion.CConexion();


            DataTable modelo = new DataTable();

            modelo.Columns.Add("Id.Factura", typeof(long));
            modelo.Columns.Add("FechaFactura", typeof(DateTime));
            modelo.Columns.Add("N.Producto", typeof(string));
            modelo.Columns.Add("Cantidad", typeof(int));
            modelo.Columns.Add("PrecioVenta", typeof(double));
            modelo.Columns.Add("Subtotal", typeof(double));


            tablaVenta.DataSource = modelo;

            // \r\n

            NumberFormatInfo formato = new NumberFormatInfo();
            formato.NumberDecimalDigits = 2;

            try
            {
                string sql = "SELECT factura.idfactura, factura.fechaFactura, producto.nombre, detalle.cantidad, detalle.precioventa FROM detalle INNER JOIN factura ON factura.idfactura = detalle.fkfactura INNER JOIN producto ON producto.idproducto = detalle.fkproducto WHERE factura.fechaFactura BETWEEN @fechadesde AND @fechahasta;";

                MySqlConnection conexion = objetoConexion.estableceConexion();

                // Verificar conexión
                if (conexion.State == ConnectionState.Closed)
                {
                    MessageBox.Show("Error: La conexión no se estableció correctamente.");
                    return;
                }

                MySqlCommand comando = new MySqlCommand(sql, conexion);

                comando.Parameters.AddWithValue("@fechadesde", desde.Value.ToString("yyyy-MM-dd"));
                comando.Parameters.AddWithValue("@fechahasta", hasta.Value.ToString("yyyy-MM-dd"));


                MySqlDataReader rs = comando.ExecuteReader();

                double totalFactura = 0.0;
 
                while (rs.Read())
                {

                    long idFactura = rs.GetInt64("idfactura");
                    DateTime fechaFactura = rs.GetDateTime("fechaFactura");
                    string nombreProducto = rs.GetString("nombre");
                    int cantidad = rs.GetInt32("cantidad");
                    double precioVenta = rs.GetDouble("precioVenta");
                    double subtotal = cantidad * precioVenta;

                    totalFactura += subtotal;

                    modelo.Rows.Add(idFactura, fechaFactura,nombreProducto, cantidad, precioVenta, subtotal);
                }

                rs.Close();

                tablaVenta.DataSource = null;
                tablaVenta.DataSource = modelo;
                tablaVenta.Refresh();

                totalGenaral.Text = totalFactura.ToString("N", formato);

            }
            catch (Exception e)
            {
                MessageBox.Show("Error al mostrar Datos: " + e.ToString());
            }
            finally
            {
                objetoConexion.CerrarConexion();
            }
        }

    }
}
