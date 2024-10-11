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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Telerik.WinControls.NativeMethods;

namespace InformacionAdicional.Vistas
{
    public partial class AdicionalBotones : RadForm
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

        private string tipo;
        private string conexion;
        private string doc;
        private Enlace enlace;


        public AdicionalBotones(string conexion, string doc, Enlace enlace, string tipo)
        {
            InitializeComponent();
            this.conexion = conexion;
            this.doc = doc;
            this.enlace = enlace;
            this.tipo = tipo;
        }

        // Muestra los botones de forma general
        private void adicionalDoc_Shown(object sender, EventArgs e)
        {
            RECT rct;
            if (GetWindowRect(ParentHwnd, out rct))
            {
                quitarFocusVisual();

                var p = new Point(830, 52); // Estas coordenadas son las perfectas para los 3 botones - Opción Oferta
                this.Location = p;

                // Lógica de habilitación de botones según el tipo de documento
                if (doc == "ALBAV" || doc == "ALBAC")
                {
                    if (AlbaranFueServido(int.Parse(this.iDoc)))
                    {
                        this.botonFacturarAlbaran.Enabled = false;
                    }
                    else
                    {
                        this.botonFacturarAlbaran.Enabled = true;
                    }
                }
                else if (doc == "PEDIV" || doc == "PEDIC")
                {
                    if (PedidoFueServido(int.Parse(this.iDoc)))
                    {
                        this.botonFacturarAlbaran.Enabled = false;
                        this.btAAlbaran.Enabled = false;
                    }
                    else
                    {
                        this.botonFacturarAlbaran.Enabled = true;
                        this.btAAlbaran.Enabled = true;
                    }
                }
                else if (doc == "OFERV" || doc == "OFERC")
                {
                    if (OfertaFueServida(int.Parse(this.iDoc)))
                    {
                        this.botonFacturarAlbaran.Enabled = false;
                        this.btAAlbaran.Enabled = false;
                        this.btAPedido.Enabled = false;
                    }
                    else
                    {
                        this.botonFacturarAlbaran.Enabled = true;
                        this.btAAlbaran.Enabled = true;
                        this.btAPedido.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Debes de seleccionar una oferta, pedido o albarán.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        // Elimina el focus visual de los botones
        public void quitarFocusVisual()
        {
            // Crea un panel invisible y asigna el enfoque a él
            Panel invisiblePanel = new Panel();
            invisiblePanel.Size = new Size(0, 0); // Hacerlo invisible
            this.Controls.Add(invisiblePanel);

            // Establecer el foco en el panel invisible
            this.ActiveControl = invisiblePanel;
        }

        public void showParaPedidos()
        {
            RECT rct;
            if (GetWindowRect(ParentHwnd, out rct))
            {
                // Oculto el botón 'A Pedido'
                this.btAPedido.Visible = false;
                
                // Cambio las coordenadas del botón 'A Albarán' por el de 'A Pedido' y las de 'A Factura' por las de 'A Albarán' 
                this.botonFacturarAlbaran.Location = new Point(btAAlbaran.Location.X, btAAlbaran.Location.Y);
                this.btAAlbaran.Location = new Point(btAPedido.Location.X, btAPedido.Location.Y);

                if (DeberiaMostrarDocumentoBotones())
                    this.Show();
            }
        }

        public void showParaOfertas()
        {
            RECT rct;
            if (GetWindowRect(ParentHwnd, out rct))
            {
                // Simplemente muestra los tres botones
                if (DeberiaMostrarDocumentoBotones())
                    this.Show();
            }
        }

        public void showParaAlbaranes()
        {
            RECT rct;
            if (GetWindowRect(ParentHwnd, out rct))
            {
                // Oculto los otros dos botones
                this.btAAlbaran.Visible = false;
                this.btAPedido.Visible = false;

                // Al botón restante le asigno la posición del primer botón
                this.botonFacturarAlbaran.Location = new Point(btAPedido.Location.X, btAPedido.Location.Y);

                if (DeberiaMostrarDocumentoBotones())
                    this.Show();
            }
        }

        // Comprueba el documento
        public bool DeberiaMostrarDocumentoBotones()
        {
            // Tipo documento
            var documento = this.doc.ToUpper();

            // Ruta del archivo que dice si se muestra o no el documento
            string rutaArchivo = Path.GetFullPath("..\\..\\MostrarDocumentos.txt");

            // Verificar si el archivo existe
            if (File.Exists(rutaArchivo))
            {
                // Leer todas las líneas del archivo
                var lineas = File.ReadAllLines(rutaArchivo);


                for (int i = 11; i < lineas.Length; i++)
                {
                    var linea = lineas[i];
                    var partes = linea.Split('=');

                    if (partes.Length == 2)
                    {
                        string tipoDoc = partes[0].Trim();
                        string mostrar = partes[1].Trim();

                        // Comprobar si el documento debe mostrarse
                        if ((tipoDoc.Equals(documento, StringComparison.OrdinalIgnoreCase)) && (Boolean.Parse(mostrar)))
                        {
                            return true;
                        }
                        else
                        {
                            this.Visible = false;
                        }
                    }
                }
            }
            return false;
        }

        // Getters y Setters de iDoc
        public void setIDoc(string iDoc, string doc)
        {
            this.iDoc = iDoc;
        }

        
        public string getIDoc() => iDoc;

        // Acción al clicar el botón 'A Factura'
        private void botonAFactura_Click(object sender, EventArgs e)
        {
            if (doc == "ALBAV")
                albaranVentaAFactura();
            else if (doc == "ALBAC")
                albaranCompraAFactura();
            else if (doc == "PEDIV" || doc == "PEDIC" || doc == "OFERV" || doc == "OFERC")
                cargarArticulosAFactura();
            else
            {
                MessageBox.Show("Debes de seleccionar una oferta, pedido o albarán.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // Acción al clicar el botón 'A Albarán'
        private void botonAAlbaran_Click(object sender, EventArgs e)
        {
            if (doc == "PEDIV" || doc == "PEDIC" || doc == "OFERV" || doc == "OFERC")
                cargarArticulosAAlbaran();
            else
            {
                MessageBox.Show("Debes de seleccionar una oferta o pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // Acción al clicar el botón 'A Pedido'
        private void botonAPedido_Click(object sender, EventArgs e)
        {
            if (doc == "OFERV" || doc == "OFERC")
                cargarArticulosAPedido();
            else
            {
                MessageBox.Show("Debes de seleccionar una oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // Carga los artículos con destino a Pedido
        private void cargarArticulosAPedido()
        {
            var unidadesServidasForm = new AdicionalDocumentoUnidadesServidas(conexion, int.Parse(iDoc), doc, "PEDIDO");
            unidadesServidasForm.CargarArticulos();
            unidadesServidasForm.ShowDialog();
        }

        // Carga los artículos con destino a Albarán
        private void cargarArticulosAAlbaran()
        {
            var unidadesServidasForm = new AdicionalDocumentoUnidadesServidas(conexion, int.Parse(iDoc), doc, "ALBARAN");
            unidadesServidasForm.CargarArticulos();
            unidadesServidasForm.ShowDialog();
        }

        // Carga los artículos con destino a Factura
        private void cargarArticulosAFactura()
        {
            var unidadesServidasForm = new AdicionalDocumentoUnidadesServidas(conexion, int.Parse(iDoc), doc, "FACTURA");
            unidadesServidasForm.CargarArticulos();
            unidadesServidasForm.ShowDialog();
        }

        // Pasa un albarán de venta a factura
        private void albaranVentaAFactura()
        {
            var unidadesServidasForm = new AdicionalDocumentoUnidadesServidas(conexion, int.Parse(iDoc), doc, "FACTURA");
            unidadesServidasForm.deshabilitarBotonServir();
            unidadesServidasForm.CargarArticulos();

            unidadesServidasForm.ShowDialog(); 
        }


        // Pasa un albarán de compra a factura
        private void albaranCompraAFactura()
        {
            var unidadesServidasForm = new AdicionalDocumentoUnidadesServidas(conexion, int.Parse(iDoc), doc, "FACTURA");
            unidadesServidasForm.deshabilitarBotonServir();
            unidadesServidasForm.CargarArticulos();
            unidadesServidasForm.ShowDialog();
        }

        // Devuelve true si el albarán está servido y false si no
        public bool AlbaranFueServido(int iDoc)
        {
            bool albaranServido = true;

            // Para albarán de venta
            if (doc == "ALBAV")
            {
                DataSet dataSetAlbaranVenta = PersistenceFactory.Queries(conexion).ObtenerArticulosAlbaranVenta(iDoc);

                if (dataSetAlbaranVenta != null && dataSetAlbaranVenta.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSetAlbaranVenta.Tables[0].Rows)
                    {
                        int unidades = Convert.ToInt32(row["UNIDADES"]);
                        int unidadesServidas = Convert.ToInt32(row["UNISERVIDA"]);
                        int unidadesAnuladas = Convert.ToInt32(row["UNIANULADA"]);

                        if (unidades != (unidadesServidas + unidadesAnuladas))
                        {
                            albaranServido = false;
                            break;
                        }
                    }
                }
                else
                {
                    albaranServido = false;
                }
            }
            // Para albarán de compra
            else if (doc == "ALBAC")
            {
                DataSet dataSetAlbaranCompra = PersistenceFactory.Queries(conexion).ObtenerArticulosAlbaranCompra(iDoc);

                if (dataSetAlbaranCompra != null && dataSetAlbaranCompra.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSetAlbaranCompra.Tables[0].Rows)
                    {
                        int unidades = Convert.ToInt32(row["UNIDADES"]);
                        int unidadesServidas = Convert.ToInt32(row["UNISERVIDA"]);
                        int unidadesAnuladas = Convert.ToInt32(row["UNIANULADA"]);

                        if (unidades != (unidadesServidas + unidadesAnuladas))
                        {
                            albaranServido = false;
                            break;
                        }
                    }
                }
                else
                {
                    albaranServido = false;
                }
            }

            return albaranServido;
        }

        // Devuelve true si la oferta está servida y false si no
        public bool OfertaFueServida(int iDoc)
        {
            bool ofertaServida = true;

            // Para oferta de venta
            if (doc == "OFERV")
            {
                DataSet dataSetOfertaVenta = PersistenceFactory.Queries(conexion).ObtenerArticulosOfertaVenta(iDoc);

                if (dataSetOfertaVenta != null && dataSetOfertaVenta.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSetOfertaVenta.Tables[0].Rows)
                    {
                        int unidades = Convert.ToInt32(row["UNIDADES"]);
                        int unidadesServidas = Convert.ToInt32(row["UNISERVIDA"]);
                        int unidadesAnuladas = Convert.ToInt32(row["UNIANULADA"]);

                        if (unidades != (unidadesServidas + unidadesAnuladas))
                        {
                            ofertaServida = false;
                            break;
                        }
                    }
                }
                else
                {
                    ofertaServida = false;
                }
            }
            // Para oferta de compra
            else if (doc == "OFERC")
            {
                DataSet dataSetOfertaCompra = PersistenceFactory.Queries(conexion).ObtenerArticulosOfertaCompra(iDoc);

                if (dataSetOfertaCompra != null && dataSetOfertaCompra.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSetOfertaCompra.Tables[0].Rows)
                    {
                        int unidades = Convert.ToInt32(row["UNIDADES"]);
                        int unidadesServidas = Convert.ToInt32(row["UNISERVIDA"]);
                        int unidadesAnuladas = Convert.ToInt32(row["UNIANULADA"]);

                        if (unidades != (unidadesServidas + unidadesAnuladas))
                        {
                            ofertaServida = false;
                            break;
                        }
                    }
                }
                else
                {
                    ofertaServida = false;
                }
            }

            return ofertaServida;
        }

        // Devuelve true si el pedido está servido y false si no
        public bool PedidoFueServido(int iDoc)
        {
            bool pedidoServido = true;

            // Para pedido de venta
            if (doc == "PEDIV")
            {
                DataSet dataSetPedidoVenta = PersistenceFactory.Queries(conexion).ObtenerArticulosPedidoVenta(iDoc);

                if (dataSetPedidoVenta != null && dataSetPedidoVenta.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSetPedidoVenta.Tables[0].Rows)
                    {
                        int unidades = Convert.ToInt32(row["UNIDADES"]);
                        int unidadesServidas = Convert.ToInt32(row["UNISERVIDA"]);
                        int unidadesAnuladas = Convert.ToInt32(row["UNIANULADA"]);

                        if (unidades != (unidadesServidas + unidadesAnuladas))
                        {
                            pedidoServido = false;
                            break;
                        }
                    }
                }
                else
                {
                    pedidoServido = false;
                }
            }
            // Para pedido de compra
            else if (doc == "PEDIC")
            {
                DataSet dataSetPedidoCompra = PersistenceFactory.Queries(conexion).ObtenerArticulosPedidoCompra(iDoc);

                if (dataSetPedidoCompra != null && dataSetPedidoCompra.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSetPedidoCompra.Tables[0].Rows)
                    {
                        int unidades = Convert.ToInt32(row["UNIDADES"]);
                        int unidadesServidas = Convert.ToInt32(row["UNISERVIDA"]);
                        int unidadesAnuladas = Convert.ToInt32(row["UNIANULADA"]);

                        if (unidades != (unidadesServidas + unidadesAnuladas))
                        {
                            pedidoServido = false;
                            break;
                        }
                    }
                }
                else
                {
                    pedidoServido = false;
                }
            }

            return pedidoServido;
        }
    }
}
