using a3ERPActiveX;
using InformacionAdicional.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.Layouts;

namespace InformacionAdicional.Vistas
{
    public partial class AdicionalDocumentoUnidadesServidas : Form
    {
        private string conexion; // Variable para la conexión a la base de datos
        private int documentoId; // ID del documento
        private string tipoDocumento; // Tipo de documento (pedido, oferta, albarán)
        private Queries queries; // Instancia de la clase Queries
        private string tipoFinalidad; // Dice si será servido a FACTURA, PEDIDO o ALBARAN

        // Constructor que acepta conexión, ID del documento y tipo de documento
        public AdicionalDocumentoUnidadesServidas(string conexion, int documentoId, string tipoDocumento, string tipoFinalidad)
        {
            InitializeComponent();
            this.conexion = conexion;
            this.documentoId = documentoId;
            this.tipoDocumento = tipoDocumento;
            this.queries = new Queries(conexion);
            this.tipoFinalidad = tipoFinalidad;
        }

        private void AdicionalDocumentoUnidadesServidas_Load(object sender, EventArgs e)
        {
            CargarArticulos();

            // Le asocio el evento de validar las celdas
            dataGridView.CellValidating += dataGridView_CellValidating;
        }

        // Método para cargar los artículos
        public void CargarArticulos()
        {
            // Según el tipo de documento, llama a la consulta adecuada
            switch (tipoDocumento)
            {
                case "ALBAC":
                    cargarAlbaranCompra();
                    break;
                case "ALBAV":
                    cargarAlbaranVenta();
                    break;
                case "PEDIC":
                    cargarPedidoCompra();
                    break;
                case "PEDIV":
                    cargarPedidoVenta();
                    break;
                case "OFERC":
                    cargarOfertaCompra();
                    break;
                case "OFERV":
                    cargarOfertaVenta();
                    break;
                default:
                    MessageBox.Show("Tipo de documento no válido.");
                    return;
            }
        }

        // Función al clicar el botón 'SERVIR TODO'
        private void btServirTodo_Click(object sender, EventArgs e)
        {
            switch (tipoDocumento)
            {
                case "ALBAC":
                    if (tipoFinalidad == "FACTURA")
                    {
                        servirTodoAlbaCFactura();
                    }
                    break;
                case "ALBAV":
                    if (tipoFinalidad == "FACTURA")
                    {
                        servirTodoAlbaVFactura();
                    }
                    break;
                case "PEDIC":
                    if (tipoFinalidad == "FACTURA")
                    {
                        servirTodoPediCFactura();
                    } else if (tipoFinalidad == "ALBARAN")
                    {
                        servirTodoPediCAlbaran();
                    }
                    break;
                case "PEDIV":
                    if (tipoFinalidad == "FACTURA")
                    {
                        servirTodoPediVFactura();
                    } else if (tipoFinalidad == "ALBARAN")
                    {
                        servirTodoPediVAlbaran();
                    }
                    break;
                case "OFERC":
                    if (tipoFinalidad == "FACTURA")
                    {
                        servirTodoOferCFactura();
                    } else if(tipoFinalidad == "PEDIDO")
                    {
                        servirTodoOferCPedido();
                    } else if (tipoFinalidad == "ALBARAN")
                    {
                        servirTodoOferCAlbaran();
                    }
                    break;
                case "OFERV":
                    if (tipoFinalidad == "FACTURA")
                    {
                        servirTodoOferVFactura();
                    } else if (tipoFinalidad == "PEDIDO")
                    {
                        servirTodoOferVPedido();
                    } else if (tipoFinalidad == "ALBARAN")
                    {
                        servirTodoOferVAlbaran();
                    }
                    break;
                default:
                    MessageBox.Show("Debes de seleccionar una oferta, pedido o albarán.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
        }

        // Función al clicar el botón 'SERVIR'
        private void btServir_Click(object sender, EventArgs e)
        {
            switch (tipoDocumento)
            {
                // LOS ALBARANES SOLO SE PUEDEN SERVIR COMPLETOS
                case "ALBAC":
                    if (tipoFinalidad == "FACTURA")
                    {   // No hace nada ya que este botón no está activo en los albaranes
                    }
                    break;
                case "ALBAV":
                    if (tipoFinalidad == "FACTURA")
                    {   // No hace nada ya que este botón no está activo en los albaranes
                    }
                    break;
                case "PEDIC":
                    if (tipoFinalidad == "FACTURA")
                    {
                        servirPediCFactura();
                    }
                    else if (tipoFinalidad == "ALBARAN")
                    {
                        servirPediCAlbaran();
                    }
                    break;
                case "PEDIV":
                    if (tipoFinalidad == "FACTURA")
                    {
                        servirPediVFactura();
                    }
                    else if (tipoFinalidad == "ALBARAN")
                    {
                        servirPediVAlbaran();
                    }
                    break;
                case "OFERC":
                    if (tipoFinalidad == "FACTURA")
                    {
                        servirOferCFactura();
                    }
                    else if (tipoFinalidad == "PEDIDO")
                    {
                        servirOferCPedido();
                    }
                    else if (tipoFinalidad == "ALBARAN")
                    {
                        servirOferCAlbaran();
                    }
                    break;
                case "OFERV":
                    if (tipoFinalidad == "FACTURA")
                    {
                        servirOferVFactura();
                    }
                    else if (tipoFinalidad == "PEDIDO")
                    {
                        servirOferVPedido();
                    }
                    else if (tipoFinalidad == "ALBARAN")
                    {
                        servirOferVAlbaran();
                    }
                    break;
                default:
                    MessageBox.Show("Debes de seleccionar una oferta, pedido o albarán.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
        }

        // Deshabilita el botón 'SERVIR' ya que en los albaranes no puede utilizarse
        public void deshabilitarBotonServir()
        {
            //Deshabilito el botón
            btServir.Enabled = false;
            // Tooltip explicativo de por que el botón SERVIR está deshabilitado
            System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
            toolTip.AutoPopDelay = 5000;  // El tiempo que permanece visible (en milisegundos)
            toolTip.InitialDelay = 1000;  // El retraso inicial antes de aparecer
            toolTip.ReshowDelay = 500;    // El tiempo entre la desaparición y la reaparición
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(btServir, "En los albaranes solo se puede servir todo el documento.");
        }

        // Manejador del evento DataBindingComplete
        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dataGridView.Rows)
            {
                int unidades = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);

                // Calcula el valor por defecto de las Unidades a Servir
                int unidadesAServirPorDefecto = unidades - (unidadesServidas + unidadesAnuladas);

                // Establece el valor por defecto de Unidades a Anular en 0
                int unidadesAAnularPorDefecto = 0;

                // Asigna el valor calculado a la celda "Unidades a Servir"
                fila.Cells["UnidadesServir"].Value = unidadesAServirPorDefecto;

                // Asigna el valor a la celda "Unidades a Anular"
                fila.Cells["UnidadesAnular"].Value = unidadesAAnularPorDefecto;
            }
        }

        private void cargarOfertaVenta()
        {
            DataSet dataSet = queries.ObtenerArticulosOfertaVenta(documentoId);

            // Verifica si hay datos y los carga en el DataGridView
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                dataGridView.DataSource = dataSet.Tables[0];

                // Invertir el orden de las filas en el DataTable
                DataTable dataTable = dataSet.Tables[0];
                DataTable dataTableInvertido = dataTable.Clone();  // Clona la estructura de la tabla original
                // Recorre las filas en orden inverso y las añade a la nueva tabla
                for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
                {
                    dataTableInvertido.ImportRow(dataTable.Rows[i]);
                }
                // Asignar el DataTable invertido al DataGridView
                dataGridView.DataSource = dataTableInvertido;

                // Deshabilita la opción de añadir nuevas filas
                dataGridView.AllowUserToAddRows = false;

                dataGridView.Columns["DESCLIN"].HeaderText = "Descripción";
                dataGridView.Columns["UNIDADES"].HeaderText = "Unidades";
                dataGridView.Columns["UNISERVIDA"].HeaderText = "Uds. Servidas";
                dataGridView.Columns["UNIANULADA"].HeaderText = "Uds. Anuladas";

                // Hacer que todas las columnas sean de solo lectura excepto la columna "Unidades a Servir"
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.ReadOnly = true; // Hacer que todas las columnas sean de solo lectura
                }

                // Oculto las columnas que no quiero mostrar
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Name != "DESCLIN" && column.Name != "UNIDADES" && column.Name != "UNISERVIDA" && column.Name != "UNIANULADA")
                    {
                        column.Visible = false;
                    }
                }

                // COLUMNA EXTRA para "Unidades a Servir"
                DataGridViewTextBoxColumn unidadesServirColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesServir",
                    HeaderText = "Unidades a Servir",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesServirColumn); // Añadir la columna al final
                // La columna "Unidades a Servir" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // COLUMNA EXTRA para "Unidades a Anular"
                DataGridViewTextBoxColumn unidadesAnularColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesAnular",
                    HeaderText = "Unidades a Anular",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesAnularColumn); // Añadir la columna al final
                // La columna "Unidades a Anular" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // Suscribir al evento DataBindingComplete
                dataGridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);

                // Ajuste de columnas y filas
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView.Dock = DockStyle.Fill;
            }
            else
            {
                MessageBox.Show("No se encontraron artículos para el documento especificado.");
            }
        }

        private void cargarOfertaCompra()
        {
            DataSet dataSet = queries.ObtenerArticulosOfertaCompra(documentoId);

            // Verifica si hay datos y los carga en el DataGridView
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                dataGridView.DataSource = dataSet.Tables[0];

                // Invertir el orden de las filas en el DataTable
                DataTable dataTable = dataSet.Tables[0];
                DataTable dataTableInvertido = dataTable.Clone();  // Clona la estructura de la tabla original
                // Recorre las filas en orden inverso y las añade a la nueva tabla
                for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
                {
                    dataTableInvertido.ImportRow(dataTable.Rows[i]);
                }
                // Asignar el DataTable invertido al DataGridView
                dataGridView.DataSource = dataTableInvertido;

                // Deshabilita la opción de añadir nuevas filas
                dataGridView.AllowUserToAddRows = false;

                dataGridView.Columns["DESCLIN"].HeaderText = "Descripción";
                dataGridView.Columns["UNIDADES"].HeaderText = "Unidades";
                dataGridView.Columns["UNISERVIDA"].HeaderText = "Uds. Servidas";
                dataGridView.Columns["UNIANULADA"].HeaderText = "Uds. Anuladas";

                // Hacer que todas las columnas sean de solo lectura excepto la columna "Unidades a Servir"
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.ReadOnly = true; // Hacer que todas las columnas sean de solo lectura
                }

                // Oculto las columnas que no quiero mostrar
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Name != "DESCLIN" && column.Name != "UNIDADES" && column.Name != "UNISERVIDA" && column.Name != "UNIANULADA")
                    {
                        column.Visible = false;
                    }
                }

                // COLUMNA EXTRA para "Unidades a Servir"
                DataGridViewTextBoxColumn unidadesServirColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesServir",
                    HeaderText = "Unidades a Servir",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesServirColumn); // Añadir la columna al final
                // La columna "Unidades a Servir" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // COLUMNA EXTRA para "Unidades a Anular"
                DataGridViewTextBoxColumn unidadesAnularColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesAnular",
                    HeaderText = "Unidades a Anular",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesAnularColumn); // Añadir la columna al final
                // La columna "Unidades a Anular" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // Suscribir al evento DataBindingComplete
                dataGridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);

                // Ajuste de columnas y filas
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView.Dock = DockStyle.Fill;
            }
            else
            {
                MessageBox.Show("No se encontraron artículos para el documento especificado.");
            }
        }

        private void cargarPedidoVenta()
        {
            DataSet dataSet = queries.ObtenerArticulosPedidoVenta(documentoId);

            // Verifica si hay datos y los carga en el DataGridView
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                dataGridView.DataSource = dataSet.Tables[0];

                // Invertir el orden de las filas en el DataTable
                DataTable dataTable = dataSet.Tables[0];
                DataTable dataTableInvertido = dataTable.Clone();  // Clona la estructura de la tabla original
                // Recorre las filas en orden inverso y las añade a la nueva tabla
                for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
                {
                    dataTableInvertido.ImportRow(dataTable.Rows[i]);
                }
                // Asignar el DataTable invertido al DataGridView
                dataGridView.DataSource = dataTableInvertido;

                // Deshabilita la opción de añadir nuevas filas
                dataGridView.AllowUserToAddRows = false;

                dataGridView.Columns["DESCLIN"].HeaderText = "Descripción";
                dataGridView.Columns["UNIDADES"].HeaderText = "Unidades";
                dataGridView.Columns["UNISERVIDA"].HeaderText = "Uds. Servidas";
                dataGridView.Columns["UNIANULADA"].HeaderText = "Uds. Anuladas";

                // Hacer que todas las columnas sean de solo lectura excepto la columna "Unidades a Servir"
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.ReadOnly = true; // Hacer que todas las columnas sean de solo lectura
                }

                // Oculto las columnas que no quiero mostrar
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Name != "DESCLIN" && column.Name != "UNIDADES" && column.Name != "UNISERVIDA" && column.Name != "UNIANULADA")
                    {
                        column.Visible = false;
                    }
                }

                // COLUMNA EXTRA para "Unidades a Servir"
                DataGridViewTextBoxColumn unidadesServirColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesServir",
                    HeaderText = "Unidades a Servir",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesServirColumn); // Añadir la columna al final
                // La columna "Unidades a Servir" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // COLUMNA EXTRA para "Unidades a Anular"
                DataGridViewTextBoxColumn unidadesAnularColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesAnular",
                    HeaderText = "Unidades a Anular",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesAnularColumn); // Añadir la columna al final
                // La columna "Unidades a Anular" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // Suscribir al evento DataBindingComplete
                dataGridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);

                // Ajuste de columnas y filas
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView.Dock = DockStyle.Fill;
            }
            else
            {
                MessageBox.Show("No se encontraron artículos para el documento especificado.");
            }
        }

        private void cargarPedidoCompra()
        {
            DataSet dataSet = queries.ObtenerArticulosPedidoCompra(documentoId);

            // Verifica si hay datos y los carga en el DataGridView
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                dataGridView.DataSource = dataSet.Tables[0];

                // Invertir el orden de las filas en el DataTable
                DataTable dataTable = dataSet.Tables[0];
                DataTable dataTableInvertido = dataTable.Clone();  // Clona la estructura de la tabla original
                // Recorre las filas en orden inverso y las añade a la nueva tabla
                for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
                {
                    dataTableInvertido.ImportRow(dataTable.Rows[i]);
                }
                // Asignar el DataTable invertido al DataGridView
                dataGridView.DataSource = dataTableInvertido;

                // Deshabilita la opción de añadir nuevas filas
                dataGridView.AllowUserToAddRows = false;

                dataGridView.Columns["DESCLIN"].HeaderText = "Descripción";
                dataGridView.Columns["UNIDADES"].HeaderText = "Unidades";
                dataGridView.Columns["UNISERVIDA"].HeaderText = "Uds. Servidas";
                dataGridView.Columns["UNIANULADA"].HeaderText = "Uds. Anuladas";

                // Hacer que todas las columnas sean de solo lectura excepto la columna "Unidades a Servir"
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.ReadOnly = true; // Hacer que todas las columnas sean de solo lectura
                }

                // Oculto las columnas que no quiero mostrar
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Name != "DESCLIN" && column.Name != "UNIDADES" && column.Name != "UNISERVIDA" && column.Name != "UNIANULADA")
                    {
                        column.Visible = false;
                    }
                }

                // COLUMNA EXTRA para "Unidades a Servir"
                DataGridViewTextBoxColumn unidadesServirColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesServir",
                    HeaderText = "Unidades a Servir",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesServirColumn); // Añadir la columna al final
                // La columna "Unidades a Servir" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // COLUMNA EXTRA para "Unidades a Anular"
                DataGridViewTextBoxColumn unidadesAnularColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesAnular",
                    HeaderText = "Unidades a Anular",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesAnularColumn); // Añadir la columna al final
                // La columna "Unidades a Anular" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // Suscribir al evento DataBindingComplete
                dataGridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);

                // Ajuste de columnas y filas
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView.Dock = DockStyle.Fill;
            }
            else
            {
                MessageBox.Show("No se encontraron artículos para el documento especificado.");
            }
        }

        private void cargarAlbaranCompra()
        {
            DataSet dataSet = queries.ObtenerArticulosAlbaranCompra(documentoId);

            // Verifica si hay datos y los carga en el DataGridView
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                dataGridView.DataSource = dataSet.Tables[0];

                // Invertir el orden de las filas en el DataTable
                DataTable dataTable = dataSet.Tables[0];
                DataTable dataTableInvertido = dataTable.Clone();  // Clona la estructura de la tabla original
                // Recorre las filas en orden inverso y las añade a la nueva tabla
                for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
                {
                    dataTableInvertido.ImportRow(dataTable.Rows[i]);
                }
                // Asignar el DataTable invertido al DataGridView
                dataGridView.DataSource = dataTableInvertido;

                // Deshabilita la opción de añadir nuevas filas
                dataGridView.AllowUserToAddRows = false;

                // Ajusta las columnas según lo que quieras mostrar
                // Cambia los nombres de las columnas a los reales
                dataGridView.Columns["DESCLIN"].HeaderText = "Descripción";
                dataGridView.Columns["UNIDADES"].HeaderText = "Unidades";
                dataGridView.Columns["UNISERVIDA"].HeaderText = "Uds. Servidas";
                dataGridView.Columns["UNIANULADA"].HeaderText = "Uds. Anuladas";

                // Oculto las columnas que no quiero mostrar
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Name != "DESCLIN" && column.Name != "UNIDADES" && column.Name != "UNISERVIDA" && column.Name != "UNIANULADA")
                    {
                        column.Visible = false;
                    }
                }

                // COLUMNA EXTRA para "Unidades a Servir"
                DataGridViewTextBoxColumn unidadesServirColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesServir",
                    HeaderText = "Unidades a Servir",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesServirColumn); // Añadir la columna al final
                // La columna "Unidades a Servir" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // COLUMNA EXTRA para "Unidades a Anular"
                DataGridViewTextBoxColumn unidadesAnularColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesAnular",
                    HeaderText = "Unidades a Anular",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesAnularColumn); // Añadir la columna al final
                // La columna "Unidades a Anular" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // Hacer que todas las columnas sean de solo lectura excepto la columna "Unidades a Servir"
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.ReadOnly = true; // Hacer que todas las columnas sean de solo lectura
                }

                // Suscribir al evento DataBindingComplete
                dataGridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);

                // Ajuste de columnas y filas
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView.Dock = DockStyle.Fill;
            }
            else
            {
                MessageBox.Show("No se encontraron artículos para el documento especificado.");
            }
        }

        private void cargarAlbaranVenta()
        {
            DataSet dataSet = queries.ObtenerArticulosAlbaranVenta(documentoId);

            // Verifica si hay datos y los carga en el DataGridView
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                dataGridView.DataSource = dataSet.Tables[0];

                // Invertir el orden de las filas en el DataTable
                DataTable dataTable = dataSet.Tables[0];
                DataTable dataTableInvertido = dataTable.Clone();  // Clona la estructura de la tabla original
                // Recorre las filas en orden inverso y las añade a la nueva tabla
                for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
                {
                    dataTableInvertido.ImportRow(dataTable.Rows[i]);
                }
                // Asignar el DataTable invertido al DataGridView
                dataGridView.DataSource = dataTableInvertido;

                // Deshabilita la opción de añadir nuevas filas
                dataGridView.AllowUserToAddRows = false;

                // Ajusta las columnas según lo que quieras mostrar
                // Cambia los nombres de las columnas a los reales
                dataGridView.Columns["DESCLIN"].HeaderText = "Descripción";
                dataGridView.Columns["UNIDADES"].HeaderText = "Unidades";
                dataGridView.Columns["UNISERVIDA"].HeaderText = "Uds. Servidas";
                dataGridView.Columns["UNIANULADA"].HeaderText = "Uds. Anuladas";

                
                // Oculto las columnas que no quiero mostrar
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Name != "DESCLIN" && column.Name != "UNIDADES" && column.Name != "UNISERVIDA" && column.Name != "UNIANULADA")
                    {
                        column.Visible = false;
                    }
                }

                // COLUMNA EXTRA para "Unidades a Servir"
                DataGridViewTextBoxColumn unidadesServirColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesServir",
                    HeaderText = "Unidades a Servir",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesServirColumn); // Añadir la columna al final
                // La columna "Unidades a Servir" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // COLUMNA EXTRA para "Unidades a Anular"
                DataGridViewTextBoxColumn unidadesAnularColumn = new DataGridViewTextBoxColumn
                {
                    Name = "UnidadesAnular",
                    HeaderText = "Unidades a Anular",
                    ValueType = typeof(int), // Esto especifica que el valor será numérico
                };
                dataGridView.Columns.Add(unidadesAnularColumn); // Añadir la columna al final
                // La columna "Unidades a Anular" debe ser editable
                dataGridView.Columns["UnidadesServir"].ReadOnly = false;

                // Hacer que todas las columnas sean de solo lectura excepto la columna "Unidades a Servir"
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.ReadOnly = true; // Hacer que todas las columnas sean de solo lectura
                }

                // Suscribir al evento DataBindingComplete
                dataGridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);

                // Ajuste de columnas y filas
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView.Dock = DockStyle.Fill;
            }
            else
            {
                MessageBox.Show("No se encontraron artículos para el documento especificado.");
            }
        }

        
        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verifica si la columna es "UnidadesServir" o "UnidadesAnular"
            if (dataGridView.Columns[e.ColumnIndex].Name == "UnidadesServir" || dataGridView.Columns[e.ColumnIndex].Name == "UnidadesAnular")
            {
                // Cambia el color del texto a rojo
                e.CellStyle.ForeColor = Color.Red;
            }
        }

        // Valida si en la columna "Unidades a Servir" solo se introducen números enteros y si su valor numérico es válido
        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Verifica si la columna es "Unidades a Servir"
            if (dataGridView.Columns[e.ColumnIndex].Name == "Unidades a Servir")
            {
                int unidadesServir;

                // Si el valor ingresado no es un número entero, muestra un mensaje de error
                if (!int.TryParse(e.FormattedValue.ToString(), out unidadesServir))
                {
                    MessageBox.Show("Por favor, ingrese un valor numérico válido en 'Unidades a Servir'.");
                    e.Cancel = true; // Cancela la entrada no válida
                    return;
                }

                // Obtiene la fila actual
                DataGridViewRow fila = dataGridView.Rows[e.RowIndex];

                // Obtiene los valores de las otras columnas
                int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);

                // Calcula la suma de (Unidades Servidas + Unidades Anuladas + Unidades a Servir)
                int sumaUnidades = unidadesServidas + unidadesAnuladas + unidadesServir;

                // Verifica si la suma de las unidades es menor o igual que las "Unidades" totales
                if (sumaUnidades > unidadesTotales)
                {
                    MessageBox.Show($"La suma de 'Uds. Servidas', 'Uds. Anuladas' y 'Unidades a Servir' no puede ser mayor que {unidadesTotales}.");
                    e.Cancel = true; // Cancela la entrada no válida
                }
            }
        }


        // Sirve todo de un albaran de compra a una factura
        private void servirTodoAlbaCFactura()
        {
            
            // Obtengo el albaran
            Albaran a3Albaran = new Albaran();
            a3Albaran.Iniciar(); // Inicia el objeto Albaran

            // Compruebo si el albarán está activo
            if (a3Albaran.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay un albarán activo actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura la Factura
            Factura a3Factura = new Factura();
            a3Factura.Iniciar();
            a3Factura.OmitirMensajes = true;

            // CODIGO DE PROVEEDOR DEL ALBARÁN
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerAlbaranCompraFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtiene el código del proveedor de la columna CODPRO
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                // Comprueba si el código del proveedor es válido
                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para el albarán.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea una nueva factura y configura el proceso de servir el albarán
                a3Factura.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true, false, true, true);
            }
            else
            {
                MessageBox.Show("No se encontró el albarán correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Factura.IniciarServir("A", Convert.ToDecimal(valorIDoc), false);
            a3Factura.ServirDocumento();              // Sirve el documento
            a3Factura.FinServir();                    // Finaliza el proceso de servir
            decimal idFactura = a3Factura.Anade();    // Guarda la factura en la base de datos
            a3Factura.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("La facturación del albarán ha sido realizada con éxito!", "Facturación hecha con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManFacturaC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve de un albaran de venta a factura
        private void servirTodoAlbaVFactura()
        {
            // Obtengo el albaran
            Albaran a3Albaran = new Albaran();
            a3Albaran.Iniciar(); // Inicia el objeto Albaran

            // Compruebo si el albarán está activo
            if (a3Albaran.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay un albarán activo actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura la Factura
            Factura a3Factura = new Factura();
            a3Factura.Iniciar();
            a3Factura.OmitirMensajes = true;

            // CODIGO DE CLIENTE DEL ALBARAN
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerAlbaranVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtén el código del cliente de la columna CODCLI
                string codigoCliente = Convert.ToString(r["CODCLI"]);

                // Comprueba si el código del cliente es válido
                if (string.IsNullOrEmpty(codigoCliente))
                {
                    MessageBox.Show("No se encontró un código de cliente para el albarán.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea una nueva factura y configura el proceso de servir el albarán
                a3Factura.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoCliente, false, false, true, true);
            }
            else
            {
                MessageBox.Show("No se encontró el albarán correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Factura.IniciarServir("A", Convert.ToDecimal(valorIDoc), false);
            a3Factura.ServirDocumento();              // Sirve el documento
            a3Factura.FinServir();                    // Finaliza el proceso de servir
            decimal idFactura = a3Factura.Anade();    // Guarda la factura en la base de datos
            a3Factura.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("La facturación del albarán ha sido realizada con éxito!", "Facturación hecha con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManFacturaV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve un pedido de compra a factura
        private void servirTodoPediCFactura()
        {
            // Obtengo el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar(); // Inicia el objeto Pedido

            // Compruebo si el pedido está activo
            if (a3Pedido.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay un pedido activo actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura la Factura
            Factura a3Factura = new Factura();
            a3Factura.Iniciar();
            a3Factura.OmitirMensajes = true;

            // CODIGO DE PROVEEDOR DEL PEDIDO
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerPedidoCompraFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtiene el código del proveedor de la columna CODPRO
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                // Comprueba si el código del proveedor es válido
                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para el pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea una nueva factura y configura el proceso de servir el pedido
                a3Factura.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true, false, true, true);
            }
            else
            {
                MessageBox.Show("No se encontró el proveedor correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Factura.IniciarServir("P", Convert.ToDecimal(valorIDoc), false);
            a3Factura.ServirDocumento();              // Sirve el documento 
            a3Factura.FinServir();                    // Finaliza el proceso de servir
            decimal idFactura = a3Factura.Anade();    // Guarda la factura en la base de datos
            a3Factura.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("La facturación del pedido ha sido realizada con éxito!", "Facturación hecha con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManFacturaC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve un pedido de venta a factura
        private void servirTodoPediVFactura()
        {
            // Obtengo el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar(); // Inicia el objeto Pedido

            // Compruebo si el pedido está activo
            if (a3Pedido.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay un pedido activo actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura la Factura
            Factura a3Factura = new Factura();
            a3Factura.Iniciar();
            a3Factura.OmitirMensajes = true;

            // CODIGO DE CLIENTE DEL PEDIDO
            var valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerPedidoVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtén el código del cliente de la columna CODCLI
                string codigoCliente = Convert.ToString(r["CODCLI"]);

                // Comprueba si el código del cliente es válido
                if (string.IsNullOrEmpty(codigoCliente))
                {
                    MessageBox.Show("No se encontró un código de cliente para el pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea una nueva factura y configura el proceso de servir el pedido
                a3Factura.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoCliente, false, false, true, true);
            }
            else
            {
                MessageBox.Show("No se encontró el pedido correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Factura.IniciarServir("P", Convert.ToDecimal(valorIDoc), false);
            a3Factura.ServirDocumento();              // Sirve el documento (convierte el albarán a factura)
            a3Factura.FinServir();                    // Finaliza el proceso de servir
            decimal idFactura = a3Factura.Anade();    // Guarda la factura en la base de datos
            a3Factura.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("La facturación del pedido ha sido realizada con éxito!", "Facturación hecha con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManFacturaV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve una oferta de compra a factura
        private void servirTodoOferCFactura()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activa
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura la Factura
            Factura a3Factura = new Factura();
            a3Factura.Iniciar();
            a3Factura.OmitirMensajes = true;

            // CODIGO DE PROVEEDOR DE LA OFERTA
            var valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaCompraFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtiene el código del proveedor de la columna CODPRO
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                // Comprueba si el código del proveedor es válido
                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Crea una nueva factura y configura el proceso de servir la oferta
                a3Factura.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true, false, true, true);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Factura.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);
            a3Factura.ServirDocumento();              // Sirve el documento
            a3Factura.FinServir();                    // Finaliza el proceso de servir
            decimal idFactura = a3Factura.Anade();    // Guarda la factura en la base de datos
            a3Factura.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("La facturación del pedido ha sido realizada con éxito!", "Facturación hecha con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManFacturaC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        private void servirTodoOferVFactura()
        {

            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activa
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura la Factura
            Factura a3Factura = new Factura();
            a3Factura.Iniciar();
            a3Factura.OmitirMensajes = true;

            // CODIGO DE CLIENTE DE LA OFERTA
            var valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtén el código del cliente de la columna CODCLI
                string codigoCliente = Convert.ToString(r["CODCLI"]);

                // Comprueba si el código del cliente es válido
                if (string.IsNullOrEmpty(codigoCliente))
                {
                    MessageBox.Show("No se encontró un código de cliente para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea una nueva factura y configura el proceso de servir la oferta
                a3Factura.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoCliente, false, false, true, true);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Factura.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);
            a3Factura.ServirDocumento();              // Sirve el documento
            a3Factura.FinServir();                    // Finaliza el proceso de servir
            decimal idFactura = a3Factura.Anade();    // Guarda la factura en la base de datos
            a3Factura.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("La facturación del pedido ha sido realizada con éxito!", "Facturación hecha con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManFacturaV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve una oferta de compra a pedido
        private void servirTodoOferCPedido()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activo
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar();
            a3Pedido.OmitirMensajes = true;

            // CODIGO DE CLIENTE DE LA OFERTA
            var valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaCompraFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtén el código del proveedor de la columna CODPRO
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                // Comprueba si el código del proveedor es válido
                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea un nuevo albarán y configura el proceso de servir la oferta
                a3Pedido.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Pedido.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);
            a3Pedido.ServirDocumento();              // Sirve el documento
            a3Pedido.FinServir();                    // Finaliza el proceso de servir
            decimal idAlbaran = a3Pedido.Anade();    // Guarda el albarán en la base de datos
            a3Pedido.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("Proceso realizado con con éxito!", "Pedido hecho con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManPedidoC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idAlbaran.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        private void servirTodoOferVPedido()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activo
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar();
            a3Pedido.OmitirMensajes = true;

            // CODIGO DE CLIENTE DE LA OFERTA
            var valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtén el código del cliente de la columna CODCLI
                string codigoCliente = Convert.ToString(r["CODCLI"]);

                // Comprueba si el código del cliente es válido
                if (string.IsNullOrEmpty(codigoCliente))
                {
                    MessageBox.Show("No se encontró un código de cliente para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea un nuevo albarán y configura el proceso de servir la oferta
                a3Pedido.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoCliente, false);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Pedido.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);
            a3Pedido.ServirDocumento();              // Sirve el documento
            a3Pedido.FinServir();                    // Finaliza el proceso de servir
            decimal idAlbaran = a3Pedido.Anade();    // Guarda el albarán en la base de datos
            a3Pedido.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("Proceso realizado con con éxito!", "Pedido hecho con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManPedidoV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idAlbaran.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve una oferta de compra a albaran
        private void servirTodoOferCAlbaran()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activo
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el albarán
            Albaran a3Albaran = new Albaran();
            a3Albaran.Iniciar();
            a3Albaran.OmitirMensajes = true;

            // CODIGO DE CLIENTE DEL PEDIDO
            var valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaCompraFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtén el código del proveedor de la columna CODPRO
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                // Comprueba si el código del cliente es válido
                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea un nuevo albarán y configura el proceso de servir la oferta
                a3Albaran.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Albaran.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);
            a3Albaran.ServirDocumento();              // Sirve el documento
            a3Albaran.FinServir();                    // Finaliza el proceso de servir
            decimal idAlbaran = a3Albaran.Anade();    // Guarda el albaran en la base de datos
            a3Albaran.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("Proceso realizado con con éxito!", "Albarán hecho con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManAlbaranC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idAlbaran.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve una oferta de venta a albaran
        private void servirTodoOferVAlbaran()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activo
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el albarán
            Albaran a3Albaran = new Albaran();
            a3Albaran.Iniciar();
            a3Albaran.OmitirMensajes = true;

            // CODIGO DE CLIENTE DEL PEDIDO
            var valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtén el código del cliente de la columna CODCLI
                string codigoCliente = Convert.ToString(r["CODCLI"]); // Esto lo obtiene bien

                // Comprueba si el código del cliente es válido
                if (string.IsNullOrEmpty(codigoCliente))
                {
                    MessageBox.Show("No se encontró un código de cliente para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea un nuevo albarán y configura el proceso de servir la oferta
                a3Albaran.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoCliente, false);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Albaran.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);
            a3Albaran.ServirDocumento();              // Sirve el documento
            a3Albaran.FinServir();                    // Finaliza el proceso de servir
            decimal idAlbaran = a3Albaran.Anade();    // Guarda el albaran en la base de datos
            a3Albaran.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("Proceso realizado con con éxito!", "Albarán hecho con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManAlbaranV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idAlbaran.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        private void servirTodoPediCAlbaran()
        {
            // Obtengo el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar(); // Inicia el objeto Pedido

            // Compruebo si el pedido está activo
            if (a3Pedido.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay un pedido activo actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el albarán
            Albaran a3Albaran = new Albaran();
            a3Albaran.Iniciar();
            a3Albaran.OmitirMensajes = true;

            // CODIGO DE PROVEEDOR DEL PEDIDO
            var valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerPedidoCompraFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtén el código del proveedor de la columna CODPRO
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                // Comprueba si el código del proveedor es válido
                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para el pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea una nueva factura y configura el proceso de servir el albarán
                a3Albaran.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true);
            }
            else
            {
                MessageBox.Show("No se encontró el pedido correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Albaran.IniciarServir("P", Convert.ToDecimal(valorIDoc), false);
            a3Albaran.ServirDocumento();              // Sirve el documento
            a3Albaran.FinServir();                    // Finaliza el proceso de servir
            decimal idAlbaran = a3Albaran.Anade();    // Guarda el albaran en la base de datos
            a3Albaran.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("Proceso realizado con con éxito!", "Albarán hecho con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManAlbaranC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idAlbaran.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        private void servirTodoPediVAlbaran()
        {
            // Obtengo el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar(); // Inicia el objeto Pedido

            // Compruebo si el pedido está activo
            if (a3Pedido.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay un pedido activo actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el albarán
            Albaran a3Albaran = new Albaran();
            a3Albaran.Iniciar();
            a3Albaran.OmitirMensajes = true;

            // CODIGO DE CLIENTE DEL PEDIDO
            var valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerPedidoVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];

                // Obtén el código del cliente de la columna CODCLI
                string codigoCliente = Convert.ToString(r["CODCLI"]); // Esto lo obtiene bien

                // Comprueba si el código del cliente es válido
                if (string.IsNullOrEmpty(codigoCliente))
                {
                    MessageBox.Show("No se encontró un código de cliente para el pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crea una nueva factura y configura el proceso de servir el albarán
                a3Albaran.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoCliente, false);
            }
            else
            {
                MessageBox.Show("No se encontró el pedido correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            a3Albaran.IniciarServir("P", Convert.ToDecimal(valorIDoc), false);
            a3Albaran.ServirDocumento();              // Sirve el documento
            a3Albaran.FinServir();                    // Finaliza el proceso de servir
            decimal idAlbaran = a3Albaran.Anade();    // Guarda el albaran en la base de datos
            a3Albaran.Acabar();                       // Finaliza el proceso de la factura

            // Muestra una alerta indicando que el proceso fue exitoso
            MessageBox.Show("Proceso realizado con con éxito!", "Albarán hecho con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // MOSTRAR VENTANA CON LA FACTURA
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManAlbaranV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idAlbaran.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve un pedido de compra a factura
        private void servirPediCFactura()
        {
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar();

            if (a3Pedido.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay un pedido activo actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Factura a3Factura = new Factura();
            a3Factura.Iniciar();
            a3Factura.OmitirMensajes = true;

            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerPedidoCompraFromId(valorIDoc);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para el pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                a3Factura.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true, false, true, true);
            }
            else
            {
                MessageBox.Show("No se encontró el pedido correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            a3Factura.IniciarServir("P", Convert.ToDecimal(valorIDoc), false);

            try
            {
                // Ordena las filas del DataGridView por el número de línea del pedido
                var orderedRows = dataGridView.Rows.Cast<DataGridViewRow>()
                                                   .OrderBy(fila => Convert.ToInt32(fila.Cells["NUMLINPED"].Value));

                foreach (DataGridViewRow fila in orderedRows)
                {
                    int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                    int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                    int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);
                    int unidadesAServir = Convert.ToInt32(fila.Cells["UnidadesServir"].Value);

                    if (unidadesServidas + unidadesAnuladas >= unidadesTotales)
                    {
                        continue;
                    }

                    if (unidadesAServir < 0 || unidadesServidas + unidadesAnuladas + unidadesAServir > unidadesTotales)
                    {
                        MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} exceden el total permitido.");
                        return;
                    }

                    string numSerie = Convert.ToString(fila.Cells["NUMSERIE"].Value);
                    string lote = Convert.ToString(fila.Cells["LOTE"].Value);
                    int numLinea = Convert.ToInt32(fila.Cells["NUMLINPED"].Value);
                    string ubicacion = Convert.ToString(fila.Cells["UBICACION"].Value);
                    DateTime? fechaCaducidad = fila.Cells["FECCADUC"].Value != DBNull.Value
                                               ? Convert.ToDateTime(fila.Cells["FECCADUC"].Value)
                                               : (DateTime?)null;

                    a3Factura.ServirLinea(0, numLinea, 0, 0, unidadesAServir, 0, numSerie, lote, ubicacion, fechaCaducidad?.ToString("dd/MM/yyyy"));

                    int unidadesAAnular = Convert.ToInt32(fila.Cells["UnidadesAnular"].Value);
                    int unidadesDisponiblesParaAnular = unidadesTotales - (unidadesServidas + unidadesAnuladas);

                    if (unidadesAAnular > 0 && unidadesAAnular <= unidadesDisponiblesParaAnular)
                    {
                        a3Factura.AnularLinea(numLinea, 0, 0, unidadesAAnular);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            a3Factura.FinServir();
            decimal idFactura = a3Factura.Anade();
            a3Factura.Acabar();

            MessageBox.Show("La facturación del pedido ha sido realizada con éxito!", "Facturación hecha con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManFacturaC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve un pedido de venta a factura
        private void servirPediVFactura()
        {
            // Obtengo el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar(); // Inicia el objeto Pedido

            // Compruebo si el pedido está activo
            if (a3Pedido.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay un pedido activo actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura la Factura
            Factura a3Factura = new Factura();
            a3Factura.Iniciar();
            a3Factura.OmitirMensajes = true;

            // CODIGO DE PROVEEDOR DEL PEDIDO
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerPedidoVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];
                string codigoProveedor = Convert.ToString(r["CODCLI"]);

                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de cliente para el pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                a3Factura.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, false, false, true, true);
            }
            else
            {
                MessageBox.Show("No se encontró el pedido correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Inicia el proceso de servir las líneas
            a3Factura.IniciarServir("P", Convert.ToDecimal(valorIDoc), false);

            try
            {
                // Itera sobre las filas del DataGridView para obtener el número de unidades a servir
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    // Obtiene las cantidades de las columnas relevantes
                    int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                    int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                    int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);
                    int unidadesAServir = Convert.ToInt32(fila.Cells["UnidadesServir"].Value);

                    // Verifica si ya se han servido o anulado todas las unidades
                    if (unidadesServidas + unidadesAnuladas >= unidadesTotales)
                    {
                        // No hace nada si todas las unidades de esa fila ya han sido servidas

                    }
                    else
                    {
                        // Verifica que las unidades a servir sean 0 o más
                        if (unidadesAServir < 0)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} deben de tener un valor de 0 o más.");
                            return;
                        }

                        // Verifica que las unidades a servir no excedan el total disponible
                        if (unidadesServidas + unidadesAnuladas + unidadesAServir > unidadesTotales)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} exceden el total permitido.");
                            return;
                        }

                        // Obtengo los parámetros de ServirLinea
                        int numLinea = Convert.ToInt32(fila.Cells["NUMLINPED"].Value);
                        string numSerie = Convert.ToString(fila.Cells["NUMSERIE"].Value);
                        string lote = Convert.ToString(fila.Cells["LOTE"].Value);
                        string ubicacion = Convert.ToString(fila.Cells["UBICACION"].Value);
                        DateTime? fechaCaducidad = fila.Cells["FECCADUC"].Value != DBNull.Value
                                                   ? Convert.ToDateTime(fila.Cells["FECCADUC"].Value)
                                                   : (DateTime?)null;

                        // Primer valor es 0, segundo valor es la columna NUMLINEOFE en ofertas, NUMLINPED en pedidos y NUMLINALB en albaranes, 0,0, unidadesAServir, 0, NUMSERIE,LOTE,UBICACION, FECCADUC
                        a3Factura.ServirLinea(0, numLinea, 0, 0, unidadesAServir, 0, numSerie, lote, ubicacion, fechaCaducidad?.ToString("dd/MM/yyyy"));

                        // Verificación para unidades a anular
                        int unidadesAAnular = Convert.ToInt32(fila.Cells["UnidadesAnular"].Value);
                        int unidadesDisponiblesParaAnular = unidadesTotales - (unidadesServidas + unidadesAnuladas); // Unidades no servidas ni anuladas

                        if (unidadesAAnular > 0 && unidadesAAnular <= unidadesDisponiblesParaAnular)
                        {
                            // Anular solo si hay unidades disponibles para anular
                            a3Factura.AnularLinea(numLinea, 0, 0, unidadesAAnular);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            a3Factura.FinServir(); // Finaliza el proceso de servir
            decimal idFactura = a3Factura.Anade(); // Guarda la factura en la base de datos
            a3Factura.Acabar(); // Finaliza el proceso de la factura

            MessageBox.Show("La facturación del pedido ha sido realizada con éxito!", "Facturación hecha con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Muestra la ventana con la factura generada
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManFacturaV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve una oferta de compra a factura
        private void servirOferCFactura()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activo
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura la Factura
            Factura a3Factura = new Factura();
            a3Factura.Iniciar();
            a3Factura.OmitirMensajes = true;

            // CODIGO DE PROVEEDOR DEL PEDIDO
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaCompraFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                a3Factura.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true, false, true, true);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Inicia el proceso de servir las líneas
            a3Factura.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);

            try
            {
                // Itera sobre las filas del DataGridView para obtener el número de unidades a servir
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    // Obtiene las cantidades de las columnas relevantes
                    int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                    int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                    int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);
                    int unidadesAServir = Convert.ToInt32(fila.Cells["UnidadesServir"].Value);

                    // Verifica si ya se han servido o anulado todas las unidades
                    if (unidadesServidas + unidadesAnuladas >= unidadesTotales)
                    {
                        // No hace nada si todas las unidades de esa fila ya han sido servidas

                    }
                    else
                    {
                        // Verifica que las unidades a servir sean 0 o más
                        if (unidadesAServir < 0)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} deben de tener un valor de 0 o más.");
                            return;
                        }

                        // Verifica que las unidades a servir no excedan el total disponible
                        if (unidadesServidas + unidadesAnuladas + unidadesAServir > unidadesTotales)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} exceden el total permitido.");
                            return;
                        }

                        // Obtengo los parámetros de ServirLinea
                        int numLinea = Convert.ToInt32(fila.Cells["NUMLINOFE"].Value);
                        string numSerie = Convert.ToString(fila.Cells["NUMSERIE"].Value);
                        string lote = Convert.ToString(fila.Cells["LOTE"].Value);
                        string ubicacion = Convert.ToString(fila.Cells["UBICACION"].Value);
                        DateTime? fechaCaducidad = fila.Cells["FECCADUC"].Value != DBNull.Value
                                                   ? Convert.ToDateTime(fila.Cells["FECCADUC"].Value)
                                                   : (DateTime?)null;

                        // Primer valor es 0, segundo valor es la columna NUMLINEOFE en ofertas, NUMLINPED en pedidos y NUMLINALB en albaranes, 0,0, unidadesAServir, 0, NUMSERIE,LOTE,UBICACION, FECCADUC
                        a3Factura.ServirLinea(0, numLinea, 0, 0, unidadesAServir, 0, numSerie, lote, ubicacion, fechaCaducidad?.ToString("dd/MM/yyyy"));

                        // Verificación para unidades a anular
                        int unidadesAAnular = Convert.ToInt32(fila.Cells["UnidadesAnular"].Value);
                        int unidadesDisponiblesParaAnular = unidadesTotales - (unidadesServidas + unidadesAnuladas); // Unidades no servidas ni anuladas

                        if (unidadesAAnular > 0 && unidadesAAnular <= unidadesDisponiblesParaAnular)
                        {
                            // Anular solo si hay unidades disponibles para anular
                            a3Factura.AnularLinea(numLinea, 0, 0, unidadesAAnular);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            a3Factura.FinServir(); // Finaliza el proceso de servir
            decimal idFactura = a3Factura.Anade(); // Guarda la factura en la base de datos
            a3Factura.Acabar(); // Finaliza el proceso de la factura

            MessageBox.Show("La facturación del pedido ha sido realizada con éxito!", "Facturación hecha con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Muestra la ventana con la factura generada
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManFacturaC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve una oferta de venta a factura
        private void servirOferVFactura()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activo
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura la Factura
            Factura a3Factura = new Factura();
            a3Factura.Iniciar();
            a3Factura.OmitirMensajes = true;

            // CODIGO DE CLIENTE DE LA OFERTA
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];
                string codigoProveedor = Convert.ToString(r["CODCLI"]);

                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de cliente para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                a3Factura.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, false, false, true, true);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Inicia el proceso de servir las líneas
            a3Factura.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);

            try
            {
                // Itera sobre las filas del DataGridView para obtener el número de unidades a servir
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    // Obtiene las cantidades de las columnas relevantes
                    int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                    int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                    int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);
                    int unidadesAServir = Convert.ToInt32(fila.Cells["UnidadesServir"].Value);

                    // Verifica si ya se han servido o anulado todas las unidades
                    if (unidadesServidas + unidadesAnuladas >= unidadesTotales)
                    {
                        // No hace nada si todas las unidades de esa fila ya han sido servidas

                    }
                    else
                    {
                        // Verifica que las unidades a servir sean 0 o más
                        if (unidadesAServir < 0)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} deben de tener un valor de 0 o más.");
                            return;
                        }

                        // Verifica que las unidades a servir no excedan el total disponible
                        if (unidadesServidas + unidadesAnuladas + unidadesAServir > unidadesTotales)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} exceden el total permitido.");
                            return;
                        }

                        // Obtengo los parámetros de ServirLinea
                        int numLinea = Convert.ToInt32(fila.Cells["NUMLINOFE"].Value);
                        string numSerie = Convert.ToString(fila.Cells["NUMSERIE"].Value);
                        string lote = Convert.ToString(fila.Cells["LOTE"].Value);
                        string ubicacion = Convert.ToString(fila.Cells["UBICACION"].Value);
                        DateTime? fechaCaducidad = fila.Cells["FECCADUC"].Value != DBNull.Value
                                                   ? Convert.ToDateTime(fila.Cells["FECCADUC"].Value)
                                                   : (DateTime?)null;

                        // Primer valor es 0, segundo valor es la columna NUMLINEOFE en ofertas, NUMLINPED en pedidos y NUMLINALB en albaranes, 0,0, unidadesAServir, 0, NUMSERIE,LOTE,UBICACION, FECCADUC
                        a3Factura.ServirLinea(0, numLinea, 0, 0, unidadesAServir, 0, numSerie, lote, ubicacion, fechaCaducidad?.ToString("dd/MM/yyyy"));

                        // Verificación para unidades a anular
                        int unidadesAAnular = Convert.ToInt32(fila.Cells["UnidadesAnular"].Value);
                        int unidadesDisponiblesParaAnular = unidadesTotales - (unidadesServidas + unidadesAnuladas); // Unidades no servidas ni anuladas

                        if (unidadesAAnular > 0 && unidadesAAnular <= unidadesDisponiblesParaAnular)
                        {
                            // Anular solo si hay unidades disponibles para anular
                            a3Factura.AnularLinea(numLinea, 0, 0, unidadesAAnular);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            a3Factura.FinServir(); // Finaliza el proceso de servir
            decimal idFactura = a3Factura.Anade(); // Guarda la factura en la base de datos
            a3Factura.Acabar(); // Finaliza el proceso de la factura

            MessageBox.Show("La facturación de la oferta ha sido realizada con éxito!", "Facturación hecha con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Muestra la ventana con la factura generada
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManFacturaV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve un pedido de compra a albaran
        public void servirPediCAlbaran()
        {
            // Obtengo el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar(); // Inicia el objeto Pedido

            // Compruebo si el pedido está activo
            if (a3Pedido.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay un pedido activo actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el albaran
            Albaran a3Albaran = new Albaran();
            a3Albaran.Iniciar();
            a3Albaran.OmitirMensajes = true;

            // CODIGO DE PROVEEDOR DEL PEDIDO
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerPedidoCompraFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para el pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                a3Albaran.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true);
            }
            else
            {
                MessageBox.Show("No se encontró el pedido correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Inicia el proceso de servir las líneas
            a3Albaran.IniciarServir("P", Convert.ToDecimal(valorIDoc), false);

            try
            {
                // Itera sobre las filas del DataGridView para obtener el número de unidades a servir
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    // Obtiene las cantidades de las columnas relevantes
                    int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                    int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                    int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);
                    int unidadesAServir = Convert.ToInt32(fila.Cells["UnidadesServir"].Value);

                    // Verifica si ya se han servido o anulado todas las unidades
                    if (unidadesServidas + unidadesAnuladas >= unidadesTotales)
                    {
                        // No hace nada si todas las unidades de esa fila ya han sido servidas

                    }
                    else
                    {
                        // Verifica que las unidades a servir sean 0 o más
                        if (unidadesAServir < 0)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} deben de tener un valor de 0 o más.");
                            return;
                        }

                        // Verifica que las unidades a servir no excedan el total disponible
                        if (unidadesServidas + unidadesAnuladas + unidadesAServir > unidadesTotales)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} exceden el total permitido.");
                            return;
                        }

                        // Obtengo los parámetros de ServirLinea
                        int numLinea = Convert.ToInt32(fila.Cells["NUMLINPED"].Value);
                        string numSerie = Convert.ToString(fila.Cells["NUMSERIE"].Value);
                        string lote = Convert.ToString(fila.Cells["LOTE"].Value);
                        string ubicacion = Convert.ToString(fila.Cells["UBICACION"].Value);
                        DateTime? fechaCaducidad = fila.Cells["FECCADUC"].Value != DBNull.Value
                                                   ? Convert.ToDateTime(fila.Cells["FECCADUC"].Value)
                                                   : (DateTime?)null;

                        // Primer valor es 0, segundo valor es la columna NUMLINEOFE en ofertas, NUMLINPED en pedidos y NUMLINALB en albaranes, 0,0, unidadesAServir, 0, NUMSERIE,LOTE,UBICACION, FECCADUC
                        a3Albaran.ServirLinea(0, numLinea, 0, 0, unidadesAServir, 0, numSerie, lote, ubicacion, fechaCaducidad?.ToString("dd/MM/yyyy"));

                        // Verificación para unidades a anular
                        int unidadesAAnular = Convert.ToInt32(fila.Cells["UnidadesAnular"].Value);
                        int unidadesDisponiblesParaAnular = unidadesTotales - (unidadesServidas + unidadesAnuladas); // Unidades no servidas ni anuladas

                        if (unidadesAAnular > 0 && unidadesAAnular <= unidadesDisponiblesParaAnular)
                        {
                            // Anular solo si hay unidades disponibles para anular
                            a3Albaran.AnularLinea(numLinea, 0, 0, unidadesAAnular);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            a3Albaran.FinServir(); // Finaliza el proceso de servir
            decimal idFactura = a3Albaran.Anade(); // Guarda
            a3Albaran.Acabar(); // Finaliza el proceso 

            MessageBox.Show("El pedido se ha servido a albarán con éxito!", "Pedido servido con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Muestra la ventana con la factura generada
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManAlbaranC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        public void servirPediVAlbaran()
        {
            // Obtengo el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar(); // Inicia el objeto Pedido

            // Compruebo si el pedido está activo
            if (a3Pedido.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay un pedido activo actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el albaran
            Albaran a3Albaran = new Albaran();
            a3Albaran.Iniciar();
            a3Albaran.OmitirMensajes = true;

            // CODIGO DE CLIENTE DEL PEDIDO
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerPedidoVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];
                string codigoProveedor = Convert.ToString(r["CODCLI"]);

                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de cliente para el pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                a3Albaran.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, false);
            }
            else
            {
                MessageBox.Show("No se encontró el pedido correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Inicia el proceso de servir las líneas
            a3Albaran.IniciarServir("P", Convert.ToDecimal(valorIDoc), false);

            try
            {
                // Itera sobre las filas del DataGridView para obtener el número de unidades a servir
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    // Obtiene las cantidades de las columnas relevantes
                    int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                    int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                    int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);
                    int unidadesAServir = Convert.ToInt32(fila.Cells["UnidadesServir"].Value);

                    // Verifica si ya se han servido o anulado todas las unidades
                    if (unidadesServidas + unidadesAnuladas >= unidadesTotales)
                    {
                        // No hace nada si todas las unidades de esa fila ya han sido servidas

                    }
                    else
                    {
                        // Verifica que las unidades a servir sean 0 o más
                        if (unidadesAServir < 0)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} deben de tener un valor de 0 o más.");
                            return;
                        }

                        // Verifica que las unidades a servir no excedan el total disponible
                        if (unidadesServidas + unidadesAnuladas + unidadesAServir > unidadesTotales)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} exceden el total permitido.");
                            return;
                        }

                        // Obtengo los parámetros de ServirLinea
                        int numLinea = Convert.ToInt32(fila.Cells["NUMLINPED"].Value);
                        string numSerie = Convert.ToString(fila.Cells["NUMSERIE"].Value);
                        string lote = Convert.ToString(fila.Cells["LOTE"].Value);
                        string ubicacion = Convert.ToString(fila.Cells["UBICACION"].Value);
                        DateTime? fechaCaducidad = fila.Cells["FECCADUC"].Value != DBNull.Value
                                                   ? Convert.ToDateTime(fila.Cells["FECCADUC"].Value)
                                                   : (DateTime?)null;

                        // Primer valor es 0, segundo valor es la columna NUMLINEOFE en ofertas, NUMLINPED en pedidos y NUMLINALB en albaranes, 0,0, unidadesAServir, 0, NUMSERIE,LOTE,UBICACION, FECCADUC
                        a3Albaran.ServirLinea(0, numLinea, 0, 0, unidadesAServir, 0, numSerie, lote, ubicacion, fechaCaducidad?.ToString("dd/MM/yyyy"));

                        // Verificación para unidades a anular
                        int unidadesAAnular = Convert.ToInt32(fila.Cells["UnidadesAnular"].Value);
                        int unidadesDisponiblesParaAnular = unidadesTotales - (unidadesServidas + unidadesAnuladas); // Unidades no servidas ni anuladas

                        if (unidadesAAnular > 0 && unidadesAAnular <= unidadesDisponiblesParaAnular)
                        {
                            // Anular solo si hay unidades disponibles para anular
                            a3Albaran.AnularLinea(numLinea, 0, 0, unidadesAAnular);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            a3Albaran.FinServir(); // Finaliza el proceso de servir
            decimal idFactura = a3Albaran.Anade(); // Guarda la factura en la base de datos
            a3Albaran.Acabar(); // Finaliza el proceso de la factura

            MessageBox.Show("El pedido de venta se ha servido a albarán con éxito!", "Pedido servido con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Muestra la ventana con la factura generada
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManAlbaranV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve una oferta de compra a pedido
        public void servirOferCPedido()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activo
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar();
            a3Pedido.OmitirMensajes = true;

            // CODIGO DE PROVEEDOR DEL PEDIDO
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaCompraFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                a3Pedido.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Inicia el proceso de servir las líneas
            a3Pedido.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);

            try
            {
                // Itera sobre las filas del DataGridView para obtener el número de unidades a servir
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    // Obtiene las cantidades de las columnas relevantes
                    int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                    int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                    int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);
                    int unidadesAServir = Convert.ToInt32(fila.Cells["UnidadesServir"].Value);

                    // Verifica si ya se han servido o anulado todas las unidades
                    if (unidadesServidas + unidadesAnuladas >= unidadesTotales)
                    {
                        // No hace nada si todas las unidades de esa fila ya han sido servidas

                    } else
                    {
                        // Verifica que las unidades a servir sean 0 o más
                        if (unidadesAServir < 0)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} deben de tener un valor de 0 o más.");
                            return;
                        }

                        // Verifica que las unidades a servir no excedan el total disponible
                        if (unidadesServidas + unidadesAnuladas + unidadesAServir > unidadesTotales)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} exceden el total permitido.");
                            return;
                        }

                        // Obtengo los parámetros de ServirLinea
                        int numLinea = Convert.ToInt32(fila.Cells["NUMLINOFE"].Value);
                        string numSerie = Convert.ToString(fila.Cells["NUMSERIE"].Value);
                        string lote = Convert.ToString(fila.Cells["LOTE"].Value);
                        string ubicacion = Convert.ToString(fila.Cells["UBICACION"].Value);
                        DateTime? fechaCaducidad = fila.Cells["FECCADUC"].Value != DBNull.Value
                                                   ? Convert.ToDateTime(fila.Cells["FECCADUC"].Value)
                                                   : (DateTime?)null;

                        // Primer valor es 0, segundo valor es la columna NUMLINEOFE en ofertas, NUMLINPED en pedidos y NUMLINALB en albaranes, 0,0, unidadesAServir, 0, NUMSERIE,LOTE,UBICACION, FECCADUC
                        a3Pedido.ServirLinea(0, numLinea, 0, 0, unidadesAServir, 0, numSerie, lote, ubicacion, fechaCaducidad?.ToString("dd/MM/yyyy"));

                        // Verificación para unidades a anular
                        int unidadesAAnular = Convert.ToInt32(fila.Cells["UnidadesAnular"].Value);
                        int unidadesDisponiblesParaAnular = unidadesTotales - (unidadesServidas + unidadesAnuladas); // Unidades no servidas ni anuladas

                        if (unidadesAAnular > 0 && unidadesAAnular <= unidadesDisponiblesParaAnular)
                        {
                            // Anular solo si hay unidades disponibles para anular
                            a3Pedido.AnularLinea(numLinea, 0, 0, unidadesAAnular);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            a3Pedido.FinServir(); // Finaliza el proceso de servir
            decimal idFactura = a3Pedido.Anade(); // Guarda la factura en la base de datos
            a3Pedido.Acabar(); // Finaliza el proceso de la factura

            MessageBox.Show("La oferta se ha servido a pedido con éxito!", "Oferta servida con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Muestra la ventana con la factura generada
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManPedidoC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        public void servirOferCAlbaran()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activo
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el albaran
            Albaran a3Albaran = new Albaran();
            a3Albaran.Iniciar();
            a3Albaran.OmitirMensajes = true;

            // CODIGO DE PROVEEDOR DEL PEDIDO
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaCompraFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];
                string codigoProveedor = Convert.ToString(r["CODPRO"]);

                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de proveedor para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                a3Albaran.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, true);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Inicia el proceso de servir las líneas
            a3Albaran.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);

            try
            {
                // Itera sobre las filas del DataGridView para obtener el número de unidades a servir
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    // Obtiene las cantidades de las columnas relevantes
                    int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                    int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                    int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);
                    int unidadesAServir = Convert.ToInt32(fila.Cells["UnidadesServir"].Value);

                    // Verifica si ya se han servido o anulado todas las unidades
                    if (unidadesServidas + unidadesAnuladas >= unidadesTotales)
                    {
                        // No hace nada si todas las unidades de esa fila ya han sido servidas

                    } else
                    {
                        // Verifica que las unidades a servir sean 0 o más
                        if (unidadesAServir < 0)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} deben de tener un valor de 0 o más.");
                            return;
                        }

                        // Verifica que las unidades a servir no excedan el total disponible
                        if (unidadesServidas + unidadesAnuladas + unidadesAServir > unidadesTotales)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} exceden el total permitido.");
                            return;
                        }

                        // Obtengo los parámetros de ServirLinea
                        int numLinea = Convert.ToInt32(fila.Cells["NUMLINOFE"].Value);
                        string numSerie = Convert.ToString(fila.Cells["NUMSERIE"].Value);
                        string lote = Convert.ToString(fila.Cells["LOTE"].Value);
                        string ubicacion = Convert.ToString(fila.Cells["UBICACION"].Value);
                        DateTime? fechaCaducidad = fila.Cells["FECCADUC"].Value != DBNull.Value
                                                   ? Convert.ToDateTime(fila.Cells["FECCADUC"].Value)
                                                   : (DateTime?)null;

                        // Primer valor es 0, segundo valor es la columna NUMLINEOFE en ofertas, NUMLINPED en pedidos y NUMLINALB en albaranes, 0,0, unidadesAServir, 0, NUMSERIE,LOTE,UBICACION, FECCADUC
                        a3Albaran.ServirLinea(0, numLinea, 0, 0, unidadesAServir, 0, numSerie, lote, ubicacion, fechaCaducidad?.ToString("dd/MM/yyyy"));

                        // Verificación para unidades a anular
                        int unidadesAAnular = Convert.ToInt32(fila.Cells["UnidadesAnular"].Value);
                        int unidadesDisponiblesParaAnular = unidadesTotales - (unidadesServidas + unidadesAnuladas); // Unidades no servidas ni anuladas

                        if (unidadesAAnular > 0 && unidadesAAnular <= unidadesDisponiblesParaAnular)
                        {
                            // Anular solo si hay unidades disponibles para anular
                            a3Albaran.AnularLinea(numLinea, 0, 0, unidadesAAnular);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            a3Albaran.FinServir(); // Finaliza el proceso de servir
            decimal idFactura = a3Albaran.Anade(); // Guarda la factura en la base de datos
            a3Albaran.Acabar(); // Finaliza el proceso de la factura

            MessageBox.Show("La oferta se ha servido a albaran con éxito!", "Oferta servida con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Muestra la ventana con la factura generada
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManAlbaranC";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        public void servirOferVPedido()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activo
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el pedido
            Pedido a3Pedido = new Pedido();
            a3Pedido.Iniciar();
            a3Pedido.OmitirMensajes = true;

            // CODIGO DE CLIENTE DE LA OFERTA
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];
                string codigoProveedor = Convert.ToString(r["CODCLI"]);

                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de cliente para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                a3Pedido.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, false);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Inicia el proceso de servir las líneas
            a3Pedido.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);

            try
            {
                // Itera sobre las filas del DataGridView para obtener el número de unidades a servir
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    // Obtiene las cantidades de las columnas relevantes
                    int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                    int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                    int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);
                    int unidadesAServir = Convert.ToInt32(fila.Cells["UnidadesServir"].Value);

                    // Verifica si ya se han servido o anulado todas las unidades
                    if (unidadesServidas + unidadesAnuladas >= unidadesTotales)
                    {
                        // No hace nada si todas las unidades de esa fila ya han sido servidas

                    } else
                    {
                        // Verifica que las unidades a servir sean 0 o más
                        if (unidadesAServir < 0)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} deben de tener un valor de 0 o más.");
                            return;
                        }

                        // Verifica que las unidades a servir no excedan el total disponible
                        if (unidadesServidas + unidadesAnuladas + unidadesAServir > unidadesTotales)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} exceden el total permitido.");
                            return;
                        }

                        // Obtengo los parámetros de ServirLinea
                        int numLinea = Convert.ToInt32(fila.Cells["NUMLINOFE"].Value);
                        string numSerie = Convert.ToString(fila.Cells["NUMSERIE"].Value);
                        string lote = Convert.ToString(fila.Cells["LOTE"].Value);
                        string ubicacion = Convert.ToString(fila.Cells["UBICACION"].Value);
                        DateTime? fechaCaducidad = fila.Cells["FECCADUC"].Value != DBNull.Value
                                                   ? Convert.ToDateTime(fila.Cells["FECCADUC"].Value)
                                                   : (DateTime?)null;

                        // Primer valor es 0, segundo valor es la columna NUMLINEOFE en ofertas, NUMLINPED en pedidos y NUMLINALB en albaranes, 0,0, unidadesAServir, 0, NUMSERIE,LOTE,UBICACION, FECCADUC
                        a3Pedido.ServirLinea(0, numLinea, 0, 0, unidadesAServir, 0, numSerie, lote, ubicacion, fechaCaducidad?.ToString("dd/MM/yyyy"));

                        // Verificación para unidades a anular
                        int unidadesAAnular = Convert.ToInt32(fila.Cells["UnidadesAnular"].Value);
                        int unidadesDisponiblesParaAnular = unidadesTotales - (unidadesServidas + unidadesAnuladas); // Unidades no servidas ni anuladas

                        if (unidadesAAnular > 0 && unidadesAAnular <= unidadesDisponiblesParaAnular)
                        {
                            // Anular solo si hay unidades disponibles para anular
                            a3Pedido.AnularLinea(numLinea, 0, 0, unidadesAAnular);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            a3Pedido.FinServir(); // Finaliza el proceso de servir
            decimal idFactura = a3Pedido.Anade(); // Guarda la factura en la base de datos
            a3Pedido.Acabar(); // Finaliza el proceso de la factura

            MessageBox.Show("La oferta de venta se ha servido a pedido con éxito!", "Oferta servida con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Muestra la ventana con la factura generada
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManPedidoV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }

        // Sirve una oferta de venta a albaran
        public void servirOferVAlbaran()
        {
            // Obtengo la oferta
            Oferta a3Oferta = new Oferta();
            a3Oferta.Iniciar(); // Inicia el objeto Oferta

            // Compruebo si la oferta está activo
            if (a3Oferta.Estado != EstadoMaestro.estM_ACTIVO)
            {
                MessageBox.Show("No hay una oferta activa actualmente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crea y configura el albaran
            Albaran a3Albaran = new Albaran();
            a3Albaran.Iniciar();
            a3Albaran.OmitirMensajes = true;

            // CODIGO DE CLIENTE DE LA OFERTA
            int valorIDoc = documentoId;
            var ds = PersistenceFactory.Queries(conexion).ObtenerOfertaVentaFromId(valorIDoc);

            // Verifica si hay filas en el resultado
            if (ds.Tables[0].Rows.Count > 0)
            {
                var r = ds.Tables[0].Rows[0];
                string codigoProveedor = Convert.ToString(r["CODCLI"]);

                if (string.IsNullOrEmpty(codigoProveedor))
                {
                    MessageBox.Show("No se encontró un código de cliente para la oferta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                a3Albaran.Nuevo(DateTime.Now.ToString("dd/MM/yyyy"), codigoProveedor, false);
            }
            else
            {
                MessageBox.Show("No se encontró la oferta correspondiente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Inicia el proceso de servir las líneas
            a3Albaran.IniciarServir("O", Convert.ToDecimal(valorIDoc), false);

            try
            {
                // Itera sobre las filas del DataGridView para obtener el número de unidades a servir
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    // Obtiene las cantidades de las columnas relevantes
                    int unidadesTotales = Convert.ToInt32(fila.Cells["UNIDADES"].Value);
                    int unidadesServidas = Convert.ToInt32(fila.Cells["UNISERVIDA"].Value);
                    int unidadesAnuladas = Convert.ToInt32(fila.Cells["UNIANULADA"].Value);
                    int unidadesAServir = Convert.ToInt32(fila.Cells["UnidadesServir"].Value);

                    // Verifica si ya se han servido o anulado todas las unidades
                    if (unidadesServidas + unidadesAnuladas >= unidadesTotales)
                    {
                        // No hace nada si todas las unidades de esa fila ya han sido servidas
                        
                    } else
                    {
                        // Verifica que las unidades a servir sean 0 o más
                        if (unidadesAServir < 0)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} deben de tener un valor de 0 o más.");
                            return;
                        }

                        // Verifica que las unidades a servir no excedan el total disponible
                        if (unidadesServidas + unidadesAnuladas + unidadesAServir > unidadesTotales)
                        {
                            MessageBox.Show($"Las unidades a servir en la fila {fila.Index + 1} exceden el total permitido.");
                            return;
                        }

                        // Obtengo los parámetros de ServirLinea
                        int numLinea = Convert.ToInt32(fila.Cells["NUMLINOFE"].Value);
                        string numSerie = Convert.ToString(fila.Cells["NUMSERIE"].Value);
                        string lote = Convert.ToString(fila.Cells["LOTE"].Value);
                        string ubicacion = Convert.ToString(fila.Cells["UBICACION"].Value);
                        DateTime? fechaCaducidad = fila.Cells["FECCADUC"].Value != DBNull.Value
                                                   ? Convert.ToDateTime(fila.Cells["FECCADUC"].Value)
                                                   : (DateTime?)null;

                        // Primer valor es 0, segundo valor es la columna NUMLINEOFE en ofertas, NUMLINPED en pedidos y NUMLINALB en albaranes, 0,0, unidadesAServir, 0, NUMSERIE,LOTE,UBICACION, FECCADUC
                        a3Albaran.ServirLinea(0, numLinea, 0, 0, unidadesAServir, 0, numSerie, lote, ubicacion, fechaCaducidad?.ToString("dd/MM/yyyy"));

                        // Verificación para unidades a anular
                        int unidadesAAnular = Convert.ToInt32(fila.Cells["UnidadesAnular"].Value);
                        int unidadesDisponiblesParaAnular = unidadesTotales - (unidadesServidas + unidadesAnuladas); // Unidades no servidas ni anuladas

                        if (unidadesAAnular > 0 && unidadesAAnular <= unidadesDisponiblesParaAnular)
                        {
                            // Anular solo si hay unidades disponibles para anular
                            a3Albaran.AnularLinea(numLinea, 0, 0, unidadesAAnular);
                        }
                    }
                    

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            a3Albaran.FinServir(); // Finaliza el proceso de servir
            decimal idFactura = a3Albaran.Anade(); // Guarda la factura en la base de datos
            a3Albaran.Acabar(); // Finaliza el proceso de la factura

            MessageBox.Show("La oferta de venta se ha servido a pedido con éxito!", "Oferta servida con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Muestra la ventana con la factura generada
            Opcion op = new Opcion();
            op.Iniciar();
            op.IdOpcion = "ManAlbaranV";
            op.AnadirParametro("Accion", "Edicion");
            op.AnadirParametro("IdDocu", idFactura.ToString().Split(',')[0]);
            op.Ejecutar();
            op.Acabar();
        }
    }
}
