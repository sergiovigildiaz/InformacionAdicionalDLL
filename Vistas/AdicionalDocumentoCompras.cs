using a3ERPActiveX;
using InformacionAdicional.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using static Telerik.WinControls.NativeMethods;

namespace InformacionAdicional.Vistas
{
    public partial class AdicionalDocumentoCompras : RadForm
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        private IntPtr parentHwnd;

        public IntPtr ParentHwnd
        {
            get { return parentHwnd; }
            set { this.parentHwnd = value; }
        }

        private string iDoc;

        public string IDoc
        {
            get { return iDoc; }
        }

        private string tipo;
        private string conexion;
        // tipo documento
        private string doc;
        private Enlace enlace;

        //private Func<string, DataSet> ObtenerEmailsDesdeDocumento;
        //private delegate string ImprimirDocVenta();
        //private delegate void MostrarTotalesDocVenta();
        //private ImprimirDocVenta imprimirDocVenta;
        //private MostrarTotalesDocVenta mostrarTotalesDocVenta;
        //private Func<string, IList<string>> ObtenerFicherosVinculadosDoc;

        public AdicionalDocumentoCompras(string conexion, string doc, Enlace enlace, string tipo)
        {
            InitializeComponent();

            this.conexion = conexion;
            this.doc = doc;
            this.enlace = enlace;
            this.tipo = tipo;
        }

        
        private void adicionalDoc_Shown(object sender, EventArgs e)
        {
            RECT rct;

            // Tipo documento
            var documento = this.doc.ToUpper();
            
            // Ruta del archivo que dice si se muestra o no el documento
            string rutaArchivo = Path.GetFullPath("..\\..\\MostrarDocumentos.txt");

            // El archivo existe
            if (File.Exists(rutaArchivo))
            {
                // Lee todas las líneas del archivo de
                var lineas = File.ReadAllLines(rutaArchivo);

                for (int i= 1; i < 9;i++)
                {
                    var linea = lineas[i];

                    // Divide la línea ya que antes del = está el nombre del documento y a la derecha el boolean de si se debe de mostrar o no
                    var partes = linea.Split('=');

                    if (partes.Length == 2)
                    {
                        string tipoDoc = partes[0].Trim();
                        string mostrar = partes[1].Trim();

                        // Si el tipo de documento reflejado en el .txt es el mismo que el del documento activo actualmente y la segunda parte es true, se debe de mostrar
                        if ((tipoDoc.Equals(documento, StringComparison.OrdinalIgnoreCase)) && (Boolean.Parse(mostrar)))
                        {
                            var p = new Point(810, 25);
                            this.Location = p;
                        } else if ((tipoDoc.Equals(this.doc.ToUpper(), StringComparison.OrdinalIgnoreCase)) && (Boolean.Parse(mostrar) == false)) // Si es el mismo tipo de documento pero el boolean es false, no se debe de mostrar
                        {
                            this.Visible = false;

                            var p = new Point(2000, 2000);
                            this.Location = p;
                        }
                    }
                }
            }

        }

        public void setIDoc(string iDoc, string doc)
        {
            this.iDoc = iDoc;
            if (!string.IsNullOrEmpty(doc) && !string.IsNullOrEmpty(iDoc))
            {
                if (doc.Equals("FACTC") || doc.Equals("FACTCLAY") || doc.Equals("FC"))
                {
                    MostrarTotalesFacturaCompra();
                }
                else if (doc.Equals("ALBAC") || doc.Equals("ALBACLAY") || doc.Equals("AC"))
                {
                    MostrarTotalesAlbaranCompra();
                }
                else if (doc.Equals("PEDIC") || doc.Equals("PEDICLAY") || doc.Equals("PC"))
                {
                    MostrarTotalesPedidosCompra();
                }
                else if (doc.Equals("FACTV") || doc.Equals("FV"))
                {
                    MostrarTotalesFacturaVenta();
                }
                else if (doc.Equals("ALBAV") || doc.Equals("AV"))
                {
                    MostrarTotalesAlbaranVenta();
                }
                else if (doc.Equals("PEDIV") || doc.Equals("PV"))
                {
                    MostrarTotalesPedidosVenta();
                }
                else if (doc.Equals("OFERV") || doc.Equals("OV"))
                {
                    MostrarTotalesOfertasVenta();
                }
                else if (doc.Equals("OFERC") || doc.Equals("OC"))
                {
                    MostrarTotalesOfertasCompra();
                }
            }
        }

        public void setValoresAntes(string iDoc, string basemoneda, string total, string iva, string ret)
        {
            this.iDoc = iDoc;
            tbBases.Text = basemoneda;
            tbIva.Text = iva;
            tbTotal.Text = total;
            txtRet.Text = ret.ToString();
        }

        private void MostrarTotalesFacturaVenta()
        {
            // Inicializa los campos de texto con "0" para mostrar que aún no hay totales calculados
            tbBases.Text = "0"; // Total de bases imponibles
            tbIva.Text = "0";   // Total de IVA
            tbTotal.Text = "0"; // Total general
            txtRet.Text = "0";  // Total de retenciones

            // Llama a la base de datos para obtener los detalles de la factura de venta
            // usando el ID de documento (IDoc) convertido a un entero
            var ds = PersistenceFactory.Queries(conexion).ObtenerFacturaVentaFromId(int.Parse(this.IDoc));

            // Verifica si hay filas en el conjunto de resultados
            if (ds.Tables[0].Rows.Count > 0)
            {
                // Inicializa variables para acumular los totales
                double totalBases = 0;
                double totalIva = 0;
                double totalRet = 0;
                double total = 0;

                // Recorre cada fila en la tabla de resultados
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    // Declara variables para almacenar los valores temporales
                    double bases, iva, ret;

                    // Convierte los valores a double
                    double.TryParse(Convert.ToString(r["BASEMONEDA"]), out bases);
                    double.TryParse(Convert.ToString(r["TOTIVA"]), out iva);
                    double.TryParse(Convert.ToString(r["TOTIRPFMONEDA"]), out ret);
                    double.TryParse(Convert.ToString(r["TOTMONEDA"]), out total);

                    // Suma los valores convertidos a los totales acumulativos
                    totalBases += bases;
                    totalIva += iva;
                    totalRet += ret;
                }

                // Asigna los totales acumulativos a los campos de texto correspondientes
                tbBases.Text = totalBases.ToString("F2");
                tbIva.Text = totalIva.ToString("F2");
                txtRet.Text = totalRet.ToString("F2");
                tbTotal.Text = (totalBases + totalIva - totalRet).ToString("F2"); // Cálculo del total final
            }
        }


        // -----FUNCIONA-----
        private void MostrarTotalesFacturaCompra()
        {
            // Inicializa los campos de texto con "0" para mostrar que aún no hay totales calculados
            tbBases.Text = "0";
            tbIva.Text = "0";
            tbTotal.Text = "0";
            txtRet.Text = "0";

            // Llama a la base de datos para obtener los detalles de la factura de compra
            // usando el ID de documento (IDoc) convertido a un entero
            var ds = PersistenceFactory.Queries(conexion).ObtenerFacturaCompraFromId(int.Parse(this.IDoc));

            // Verifica si hay filas en el conjunto de resultados
            if (ds.Tables[0].Rows.Count > 0)
            {
                // Inicializa variables para acumular los totales
                double totalBases = 0;
                double totalIva = 0;
                double totalRet = 0;
                double total = 0;

                // Recorre cada fila en la tabla de resultados
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    // Declara variables para almacenar los valores temporales
                    double bases, iva, ret;

                    // Convierte los valores a double
                    double.TryParse(Convert.ToString(r["BASEMONEDA"]), out bases);
                    double.TryParse(Convert.ToString(r["TOTIVA"]), out iva);
                    double.TryParse(Convert.ToString(r["TOTIRPFMONEDA"]), out ret);
                    double.TryParse(Convert.ToString(r["TOTMONEDA"]), out total);

                    // Suma los valores convertidos a los totales acumulativos
                    totalBases += bases;
                    totalIva += iva;
                    totalRet += ret;
                }

                // Asigna los totales acumulativos a los campos de texto correspondientes
                tbBases.Text = totalBases.ToString("F2");
                tbIva.Text = totalIva.ToString("F2");
                txtRet.Text = totalRet.ToString("F2");
                tbTotal.Text = (totalBases + totalIva - totalRet).ToString("F2"); // 
            }
        }



        // -----FUNCIONA PERO NO SE MUESTRA YA QUE NO PUEDO CAMBIAR LA FECHA-----
        private void MostrarTotalesAlbaranCompra()
        {
            tbBases.Text = "0";
            tbIva.Text = "0";
            tbTotal.Text = "0";

            var ds = PersistenceFactory.Queries(conexion).ObtenerAlbaranCompraFromId(int.Parse(this.IDoc));
            if (ds.Tables[0].Rows.Count > 0)
            {
                double bases, iva, total, ret, pronto;
                var r = ds.Tables[0].Rows[0];
                double.TryParse(Convert.ToString(r["BASEMONEDA"]), out bases);
                double.TryParse(Convert.ToString(r["TOTMONEDA"]), out total);
                double.TryParse(Convert.ToString(r["TOTIVA"]), out iva);
                double.TryParse(Convert.ToString(r["TOTIRPFMONEDA"]), out ret);
                double.TryParse(Convert.ToString(r["TOTPRONTO"]), out pronto);
                tbBases.Text = bases.ToString();
                tbIva.Text = iva.ToString();
                tbTotal.Text = total.ToString();
                txtRet.Text = ret.ToString();
            }
        }

        private void MostrarTotalesAlbaranVenta()
        {
            // Inicializa los campos de texto con "0" para mostrar que aún no hay totales calculados
            tbBases.Text = "0";
            tbIva.Text = "0";
            tbTotal.Text = "0";
            txtRet.Text = "0";

            // Llama a la base de datos para obtener los detalles del albarán de venta
            // usando el ID de documento (IDoc) convertido a un entero
            var ds = PersistenceFactory.Queries(conexion).ObtenerAlbaranVentaFromId(int.Parse(this.IDoc));

            // Verifica si hay filas en el conjunto de resultados
            if (ds.Tables[0].Rows.Count > 0)
            {
                // Inicializa variables para acumular los totales
                double totalBases = 0;
                double totalIva = 0;
                double totalRet = 0;
                double total = 0;

                // Recorre cada fila en la tabla de resultados
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    // Declara variables para almacenar los valores temporales
                    double bases, iva, ret;

                    // Convierte los valores a double
                    double.TryParse(Convert.ToString(r["BASEMONEDA"]), out bases);
                    double.TryParse(Convert.ToString(r["TOTIVA"]), out iva);
                    double.TryParse(Convert.ToString(r["TOTIRPFMONEDA"]), out ret);
                    double.TryParse(Convert.ToString(r["TOTMONEDA"]), out total);

                    // Suma los valores convertidos a los totales acumulativos
                    totalBases += bases;
                    totalIva += iva;
                    totalRet += ret;
                }

                // Asigna los totales acumulativos a los campos de texto correspondientes
                tbBases.Text = totalBases.ToString("F2");
                tbIva.Text = totalIva.ToString("F2");
                txtRet.Text = totalRet.ToString("F2");
                tbTotal.Text = (totalBases + totalIva - totalRet).ToString("F2"); // Calcula el total final
            }
        }


        // -----FUNCIONA-----
        private void MostrarTotalesPedidosCompra()
        {
            tbBases.Text = "0";
            tbIva.Text = "0";
            tbTotal.Text = "0";

            var ds = PersistenceFactory.Queries(conexion).ObtenerPedidoCompraFromId(int.Parse(this.IDoc));
            if (ds.Tables[0].Rows.Count > 0)
            {
                double bases, iva, total, ret, pronto;
                var r = ds.Tables[0].Rows[0];
                double.TryParse(Convert.ToString(r["BASEMONEDA"]), out bases);
                double.TryParse(Convert.ToString(r["TOTMONEDA"]), out total);
                double.TryParse(Convert.ToString(r["TOTIVA"]), out iva);
                double.TryParse(Convert.ToString(r["TOTIRPFMONEDA"]), out ret);
                double.TryParse(Convert.ToString(r["TOTPRONTO"]), out pronto);
                tbBases.Text = bases.ToString();
                tbIva.Text = iva.ToString();
                tbTotal.Text = total.ToString();
                txtRet.Text = ret.ToString();
            }
        }

        private void MostrarTotalesPedidosVenta()
        {
            tbBases.Text = "0";
            tbIva.Text = "0";
            tbTotal.Text = "0";

            var ds = PersistenceFactory.Queries(conexion).ObtenerPedidoVentaFromId(int.Parse(this.IDoc));
            if (ds.Tables[0].Rows.Count > 0)
            {
                double bases, iva, total, ret, pronto;
                var r = ds.Tables[0].Rows[0];
                double.TryParse(Convert.ToString(r["BASEMONEDA"]), out bases);
                double.TryParse(Convert.ToString(r["TOTMONEDA"]), out total);
                double.TryParse(Convert.ToString(r["TOTIVA"]), out iva);
                double.TryParse(Convert.ToString(r["TOTIRPFMONEDA"]), out ret);
                double.TryParse(Convert.ToString(r["TOTPRONTO"]), out pronto);
                tbBases.Text = bases.ToString();
                tbIva.Text = iva.ToString();
                tbTotal.Text = total.ToString();
                txtRet.Text = ret.ToString();
            }
        }

        private void MostrarTotalesOfertasVenta()
        {
            tbBases.Text = "0";
            tbIva.Text = "0";
            tbTotal.Text = "0";

            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaVentaFromId(int.Parse(this.IDoc));
            if (ds.Tables[0].Rows.Count > 0)
            {
                double bases, iva, total, ret, pronto;
                var r = ds.Tables[0].Rows[0];
                double.TryParse(Convert.ToString(r["BASEMONEDA"]), out bases);
                double.TryParse(Convert.ToString(r["TOTMONEDA"]), out total);
                double.TryParse(Convert.ToString(r["TOTIVA"]), out iva);
                double.TryParse(Convert.ToString(r["TOTIRPFMONEDA"]), out ret);
                double.TryParse(Convert.ToString(r["TOTPRONTO"]), out pronto);
                tbBases.Text = bases.ToString();
                tbIva.Text = iva.ToString();
                tbTotal.Text = total.ToString();
                txtRet.Text = ret.ToString();
            }
        }

        private void MostrarTotalesOfertasCompra()
        {
            // Inicializa los campos de texto con "0" para mostrar que aún no hay totales calculados
            tbBases.Text = "0";
            tbIva.Text = "0";
            tbTotal.Text = "0";
            txtRet.Text = "0";

            // Llama a la base de datos para obtener los detalles del albarán de venta
            // usando el ID de documento (IDoc) convertido a un entero
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaCompraFromId(int.Parse(this.IDoc));

            // Verifica si hay filas en el conjunto de resultados
            if (ds.Tables[0].Rows.Count > 0)
            {
                // Inicializa variables para acumular los totales
                double totalBases = 0;
                double totalIva = 0;
                double totalRet = 0;
                double total = 0;

                // Recorre cada fila en la tabla de resultados
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    // Declara variables para almacenar los valores temporales
                    double bases, iva, ret;

                    // Convierte los valores a double
                    double.TryParse(Convert.ToString(r["BASEMONEDA"]), out bases);
                    double.TryParse(Convert.ToString(r["TOTIVA"]), out iva);
                    double.TryParse(Convert.ToString(r["TOTIRPFMONEDA"]), out ret);
                    double.TryParse(Convert.ToString(r["TOTMONEDA"]), out total);

                    // Suma los valores convertidos a los totales acumulativos
                    totalBases += bases;
                    totalIva += iva;
                    totalRet += ret;
                }

                // Asigna los totales acumulativos a los campos de texto correspondientes
                tbBases.Text = totalBases.ToString("F2");
                tbIva.Text = totalIva.ToString("F2");
                txtRet.Text = totalRet.ToString("F2");
                tbTotal.Text = (totalBases + totalIva - totalRet).ToString("F2"); // Calcula el total final
            }
        }

        
    }
}
