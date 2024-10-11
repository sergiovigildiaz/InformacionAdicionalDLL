using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformacionAdicional.Persistence
{
    class Queries
    {
        private static string DLL_Lejias;

        public Queries(string conexion)
        {
            DLL_Lejias = conexion;
        }

        public DataSet ObtenerFacturaCompraFromId(int idfacc)
        {
            string query = string.Format("SELECT TOP 1 * FROM CABEFACC WHERE IDFACC = {0}", idfacc);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerFacturaVentaFromId(int idfacv)
        {
            string query = string.Format("SELECT TOP 1 * FROM CABEFACV WHERE IDFACV = {0}", idfacv);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerAlbaranCompraFromId(int idalbc)
        {
            string query = string.Format("SELECT TOP 1 * FROM CABEALBC WHERE IDALBC = {0}", idalbc);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerAlbaranVentaFromId(int idalbv)
        {
            string query = string.Format("SELECT TOP 1 * FROM CABEALBV WHERE IDALBV = {0}", idalbv);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerPedidoVentaFromId(int idpedv)
        {
            string query = string.Format("SELECT TOP 1 * FROM CABEPEDV WHERE IDPEDV = {0}", idpedv);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerPedidoCompraFromId(int idpedc)
        {
            string query = string.Format("SELECT TOP 1 * FROM CABEPEDC WHERE IDPEDC = {0}", idpedc);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerOfertaCompraFromId(int idofec)
        {
            string query = string.Format("SELECT TOP 1 * FROM CABEOFEC WHERE IDOFEC = {0}", idofec);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerOfertaVentaFromId(int idofev)
        {
            string query = string.Format("SELECT TOP 1 * FROM CABEOFEV WHERE IDOFEV = {0}", idofev);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        /**
         * Obtiene un pedido de venta dado su IDPEDV si ha sido servido, es decir, si
         * Unidades Totales = Unidades Servidas + Unidades Anuladas
         */
        public DataSet ObtenerPedidoVentaServido(int idPedidoVenta)
        {
            string query = string.Format("SELECT TOP 1 * FROM LINEPEDI WHERE IDPEDV = {0} AND UNIDADES = (UNISERVIDA + UNIANULADA)", idPedidoVenta);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        /**
         * Obtiene un pedido de compra dado su IDPEDC si ha sido servido, es decir, si
         * Unidades Totales = Unidades Servidas + Unidades Anuladas
         */
        public DataSet ObtenerPedidoCompraServido(int idPedidoCompra)
        {
            string query = string.Format("SELECT TOP 1 * FROM LINEPEDI WHERE IDPEDC = {0} AND UNIDADES = (UNISERVIDA + UNIANULADA)", idPedidoCompra);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        /**
         * Obtiene una oferta de venta dado su IDOFEV si ha sido servida, es decir, si
         * Unidades Totales = Unidades Servidas + Unidades Anuladas
         */
        public DataSet ObtenerOfertaVentaServida(int idOfertaVenta)
        {
            string query = string.Format("SELECT TOP 1 * FROM LINEOFER WHERE IDOFEV = {0} AND UNIDADES = (UNISERVIDA + UNIANULADA)", idOfertaVenta);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        /**
         * Obtiene una oferta de compra dado su IDOFEC si ha sido servida, es decir, si
         * Unidades Totales = Unidades Servidas + Unidades Anuladas
         */
        public DataSet ObtenerOfertaCompraServida(int idOfertaCompra)
        {
            string query = string.Format("SELECT TOP 1 * FROM LINEOFER WHERE IDOFEC = {0} AND UNIDADES = (UNISERVIDA + UNIANULADA)", idOfertaCompra);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        /**
         * Obtiene un albarán de venta dado su IDALBV si ha sido servido, es decir, si
         * Unidades Totales = Unidades Servidas + Unidades Anuladas
         */
        public DataSet ObtenerAlbaranVentaServido(int idAlbaranVenta)
        {
            string query = string.Format("SELECT TOP 1 * FROM LINEALBA WHERE IDALBV = {0} AND UNIDADES = (UNISERVIDA + UNIANULADA)", idAlbaranVenta);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        /**
         * Obtiene un albarán de compra dado su IDALBC si ha sido servido, es decir, si
         * Unidades Totales = Unidades Servidas + Unidades Anuladas
         */
        public DataSet ObtenerAlbaranCompraServido(int idAlbaranCompra)
        {
            string query = string.Format("SELECT TOP 1 * FROM LINEALBA WHERE IDALBC = {0} AND UNIDADES = (UNISERVIDA + UNIANULADA)", idAlbaranCompra);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerArticulosAlbaranVenta(int idAlbaranVenta)
        {
            string query = string.Format("SELECT * FROM LINEALBA WHERE IDALBV = {0}", idAlbaranVenta);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerArticulosAlbaranCompra(int idAlbaranCompra)
        {
            string query = string.Format("SELECT * FROM LINEALBA WHERE IDALBC = {0}", idAlbaranCompra);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerArticulosPedidoCompra(int idPedidoCompra)
        {
            string query = string.Format("SELECT * FROM LINEPEDI WHERE IDPEDC = {0}", idPedidoCompra);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerArticulosPedidoVenta(int idPedidoVenta)
        {
            string query = string.Format("SELECT * FROM LINEPEDI WHERE IDPEDV = {0}", idPedidoVenta);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerArticulosOfertaCompra(int idOfertaCompra)
        {
            string query = string.Format("SELECT * FROM LINEOFER WHERE IDOFEC = {0}", idOfertaCompra);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }

        public DataSet ObtenerArticulosOfertaVenta(int idOfertaVenta)
        {
            string query = string.Format("SELECT * FROM LINEOFER WHERE IDOFEV = {0}", idOfertaVenta);
            return BaseQuery.GetDataSet(query, DLL_Lejias);
        }
    }
}
