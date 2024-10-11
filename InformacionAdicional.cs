using a3ERPActiveX;
using InformacionAdicional.Vistas;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.Layouts;

namespace InformacionAdicional
{
    public class InformacionAdicional
    {
        private static Enlace a3enlace = null;

        public static string conexion = "Data Source=AUXILIAR-MSI\\A3ERP;Initial Catalog=EjemploDDBB;User Id=Sa;Password=demo";

        //public static string conexion = "Data Source=PABLO-ERP\\A3ERP;Initial Catalog=DEMO;User Id=Sa;Password=demo";

        public static bool ventanaActiva = false;
        public static bool ActivarVentana = true;
        public List<string[]> refe;

        private SqlConnection connEmpresa = new SqlConnection();
        SqlCommand cmd;

        string Usuario = "";
        string Servidor = "";
        string BaseDeDatos = "";
        string Password = "";

        //Botones adicionales
        private AdicionalDocumentoCompras adicDocCompras = new AdicionalDocumentoCompras(conexion, null, a3enlace, null);

        // Botón adicional para facturar un albarán
        private AdicionalBotones adicBotonAlbaran = new AdicionalBotones(conexion, null, a3enlace, null);

        //COMPARTIDO ENTRE EVENTOS DE DLL Y EVENTOS DE MENÚ
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private string buildConnection(string conexionNexus)
        {
            string[] Copia;
            Copia = conexionNexus.Split(new Char[] { ';' });
            foreach (string item in Copia)
            {
                string[] items = item.Split(new Char[] { '=' });
                if (items[0].ToUpper() == "USER ID")
                    Usuario = items[1];
                else if (items[0].ToUpper() == "INITIAL CATALOG")
                    BaseDeDatos = items[1];
                else if (items[0].ToUpper() == "DATA SOURCE")
                    Servidor = items[1];
                else if (items[0].ToUpper() == "PASSWORD")
                    Password = items[1];
            }
            return string.Format(conexion, Servidor, BaseDeDatos, Usuario, Password);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private const UInt32 WM_CLOSE = 0x0010;

        public object[] ListaProcedimientos()
        {
            object[] result = { "INICIAR", "FINALIZAR", "AntesDeGuardarDocumentoV2", "DespuesDeNuevoFormulario", "AntesDeDestruirFormulario", "DespuesDeCargarDocumentoV2", "DespuesDeGuardarDocumentoV2" };

            return result;
        }

        // Une nuestra programacion con la base de datos y el programa
        public void Iniciar(string connEmpresaNexus, string connSistemaNexus)
        {
            connEmpresa.ConnectionString = buildConnection(connEmpresaNexus);
            connEmpresa.Open();

            a3enlace = new Enlace();
            a3enlace.RaiseOnException = true;
            a3enlace.VerBarraDeProgreso = true;
            a3enlace.Iniciar("");

            cmd = new SqlCommand();
            cmd.Connection = connEmpresa;
        }

        [SuppressUnmanagedCodeSecurity]
        internal static class UnsafeNativeMethods
        {
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        }

        public void DespuesDeGuardarDocumentoV2(string documento, double idDoc, int estado)
        {
            if (documento.Equals("PC"))
            {
                adicDocCompras.setIDoc(idDoc.ToString(), documento);
                adicBotonAlbaran.setIDoc(idDoc.ToString(), documento);
            }
            if (documento.Equals("AC"))
            {
                adicDocCompras.setIDoc(idDoc.ToString(), documento);
                adicBotonAlbaran.setIDoc(idDoc.ToString(), documento);
            }
            if (documento.Equals("FC"))
            {
                adicDocCompras.setIDoc(idDoc.ToString(), documento);
                adicBotonAlbaran.setIDoc(idDoc.ToString(), documento);
            }
            if (documento.Equals("PV"))
            {
                adicDocCompras.setIDoc(idDoc.ToString(), documento);
                adicBotonAlbaran.setIDoc(idDoc.ToString(), documento);
            }
            if (documento.Equals("AV"))
            {
                adicDocCompras.setIDoc(idDoc.ToString(), documento);
                adicBotonAlbaran.setIDoc(idDoc.ToString(), documento);
            }
            if (documento.Equals("OC"))
            {
                adicDocCompras.setIDoc(idDoc.ToString(), documento);
                adicBotonAlbaran.setIDoc(idDoc.ToString(), documento);
            }
            if (documento.Equals("OV"))
            {
                adicDocCompras.setIDoc(idDoc.ToString(), documento);
                adicBotonAlbaran.setIDoc(idDoc.ToString(), documento);
            }
        }

        public bool AntesDeGuardarDocumentoV2(string Documento, double IdDoc, ref object Cabecera, ref object Lineas, int estado)
        {
            try
            {
                if (estado != 2)
                {
                    if ((Documento.Equals("PC") || Documento.Equals("PV") || Documento.Equals("FC") || Documento.Equals("FV") || Documento.Equals("AC") || Documento.Equals("AV") || Documento.Equals("OC") || Documento.Equals("OV") && IdDoc > 0))
                    {
                        // Establezco los valores desde la cabecera
                        adicDocCompras.setValoresAntes(IdDoc.ToString(),
                        GetValorAntes(Cabecera as object[], "BASEMONEDA").ToString(),
                        GetValorAntes(Cabecera as object[], "TOTMONEDA").ToString(),
                        GetValorAntes(Cabecera as object[], "TOTIVAMONEDA").ToString(),
                        GetValorAntes(Cabecera as object[], "TOTIRPFMONEDA").ToString());

                        adicBotonAlbaran.setIDoc(IdDoc.ToString(), Documento);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void DespuesDeNuevoFormulario(ref string ClaseFormulario, int HandleFormulario)
        {
            if (ClaseFormulario.Equals("TfrmFactC") || ClaseFormulario.Equals("TfrmFactCLay") || ClaseFormulario.Equals("TfrmFactV") || ClaseFormulario.Equals("TfrmFactVLay"))
            {
                IntPtr whandle = new IntPtr(HandleFormulario);
                adicDocCompras = new AdicionalDocumentoCompras(conexion, ClaseFormulario.Replace("Tfrm", "").Replace("Lay", "").ToUpper(), a3enlace, null);
                SetParent(adicDocCompras.Handle, whandle);
                adicDocCompras.ParentHwnd = whandle;
                adicDocCompras.Show();
            }

            // Verifico si es la ventana de albaranes
            else if (ClaseFormulario.Equals("TfrmAlbaV") || ClaseFormulario.Equals("TfrmAlbaCLay") || ClaseFormulario.Equals("TfrmAlbaC") || ClaseFormulario.Equals("TfrmAlbaVLay"))
            {
                // Muestro botón de facturar albarán
                IntPtr whandle = new IntPtr(HandleFormulario);
                adicBotonAlbaran = new AdicionalBotones(conexion, ClaseFormulario.Replace("Tfrm", "").Replace("Lay", "").ToUpper(), a3enlace, null);
                SetParent(adicBotonAlbaran.Handle, whandle);
                adicBotonAlbaran.ParentHwnd = whandle;
                

                // Muestro campo
                adicDocCompras = new AdicionalDocumentoCompras(conexion, ClaseFormulario.Replace("Tfrm", "").Replace("Lay", "").ToUpper(), a3enlace, null);
                SetParent(adicDocCompras.Handle, whandle);
                adicDocCompras.ParentHwnd = whandle;
                adicDocCompras.Show();

            }// Verifico si es la ventana de pedidos
            else if (ClaseFormulario.Equals("TfrmPediC") || ClaseFormulario.Equals("TfrmPediCLay") || ClaseFormulario.Equals("TfrmPediV") || ClaseFormulario.Equals("TfrmPediVLay"))
            {
                IntPtr whandle = new IntPtr(HandleFormulario);
                adicDocCompras = new AdicionalDocumentoCompras(conexion, ClaseFormulario.Replace("Tfrm", "").Replace("Lay", "").ToUpper(), a3enlace, null);
                SetParent(adicDocCompras.Handle, whandle);
                adicDocCompras.ParentHwnd = whandle;
                adicDocCompras.Show();

                // Muestro los dos botones
                adicBotonAlbaran = new AdicionalBotones(conexion, ClaseFormulario.Replace("Tfrm", "").Replace("Lay", "").ToUpper(), a3enlace, null);
                SetParent(adicBotonAlbaran.Handle, whandle);
                adicBotonAlbaran.ParentHwnd = whandle;
                

            }// Verifico si es la ventana de ofertas
            else if (ClaseFormulario.Equals("TfrmOferC") || ClaseFormulario.Equals("TfrmOferCLay") || ClaseFormulario.Equals("TfrmOferV") || ClaseFormulario.Equals("TfrmOferVLay"))
            {
                // Muestro botones
                IntPtr whandle = new IntPtr(HandleFormulario);
                adicBotonAlbaran = new AdicionalBotones(conexion, ClaseFormulario.Replace("Tfrm", "").Replace("Lay", "").ToUpper(), a3enlace, null);
                SetParent(adicBotonAlbaran.Handle, whandle);
                adicBotonAlbaran.ParentHwnd = whandle;
                

                // Muestro campo
                adicDocCompras = new AdicionalDocumentoCompras(conexion, ClaseFormulario.Replace("Tfrm", "").Replace("Lay", "").ToUpper(), a3enlace, null);
                SetParent(adicDocCompras.Handle, whandle);
                adicDocCompras.ParentHwnd = whandle;
                adicDocCompras.Show();

            }
        }

        public void AntesDeDestruirFormulario(string ClaseFormulario, int HandleFormulario)
        {
            if (ClaseFormulario.Equals("TfrmFactC") || ClaseFormulario.Equals("TfrmFactCLay") || ClaseFormulario.Equals("TfrmAlbaC") || ClaseFormulario.Equals("TfrmAlbaCLay") || ClaseFormulario.Equals("TfrmPediC") || ClaseFormulario.Equals("TfrmPediCLay") || ClaseFormulario.Equals("TfrmOferC") || ClaseFormulario.Equals("TfrmOferCLay") || ClaseFormulario.Equals("TfrmOferV") || ClaseFormulario.Equals("TfrmOferVLay"))
            {
                try
                {
                    if (!adicDocCompras.IsDisposed)
                    {
                        SetParent(adicDocCompras.Handle, IntPtr.Zero);
                        adicDocCompras.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(ex.Message), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public object DespuesDeCargarDocumentoV2(string Documento, decimal IdDoc, object Cabecera, object Lineas, int Estado)
        {
            try
            {
                if (Documento.Equals("PC") || Documento.Equals("AC") || Documento.Equals("FC") || Documento.Equals("AV") || Documento.Equals("PV") || Documento.Equals("FV") || Documento.Equals("OC") || Documento.Equals("OV"))
                {
                    adicDocCompras.setIDoc(IdDoc.ToString(), Documento);
                    adicBotonAlbaran.setIDoc(IdDoc.ToString(), Documento); // Aqui si llega el IDoc
                    //Muestro el formulario para todos
                    adicDocCompras.Show();
              
                }

                // Dependiendo de que documento es muestro los botones de una manera u otra
                if (Documento.Equals("PC") || Documento.Equals("PV"))
                    adicBotonAlbaran.showParaPedidos();
                else if (Documento.Equals("OC") || Documento.Equals("OV"))
                    adicBotonAlbaran.showParaOfertas();
                else if (Documento.Equals("AC") || Documento.Equals("AV"))
                {
                    adicBotonAlbaran.showParaAlbaranes();
                    adicBotonAlbaran.setIDoc(IdDoc.ToString(), Documento);
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en DespuesDeCargarDocumentoV2: " + ex.Message);
            }
            return null;
        }

        private object GetValorAntes(object[] Datos, string Campo)
        {
            object result = null;
            for (int i = 1; i <= Convert.ToInt16((Datos[1] as object[])[0]); i++)
            {
                object[] item = (Datos[1] as object[])[i] as object[];
                if (item[0].ToString().ToUpper() == Campo.ToUpper())
                    result = item[1];
            }
            return result;
        }
    }
}
