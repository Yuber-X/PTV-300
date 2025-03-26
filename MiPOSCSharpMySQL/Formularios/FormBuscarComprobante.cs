using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiPOSCSharpMySQL.Formularios
{
    public partial class FormBuscarComprobante : Form
    {
        public FormBuscarComprobante()
        {
            InitializeComponent();
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Controlador.ControladorReporte objetoReporte = new Controlador.ControladorReporte();
            objetoReporte.MostrarDatosFactura(txtnumerofactura, lblnumerofactura, lblfechaventa, lblnombres, lblappaterno, lblapmaterno);
            objetoReporte.MostrarVentaFactura(txtnumerofactura,dgvproducto,lbliva,lbltotal);
        }
    }
}
