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
    public partial class FormReportePorFechas : Form
    {
        public FormReportePorFechas()
        {
            InitializeComponent();
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Controlador.ControladorReporte objetoReporte = new Controlador.ControladorReporte();
            objetoReporte.MostrarVentaPorFecha(dtpDesde, dtpHasta,dgvventas,lbltotal);
        }
    }
}
