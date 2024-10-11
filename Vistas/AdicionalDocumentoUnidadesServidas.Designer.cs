using System.Windows.Forms;

namespace InformacionAdicional.Vistas
{
    public partial class AdicionalDocumentoUnidadesServidas : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btServir = new System.Windows.Forms.Button();
            this.btServirTodo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 12);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(776, 426);
            this.dataGridView.TabIndex = 5;
            this.dataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_CellFormatting);
            this.dataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView_CellValidating);
            // 
            // btServir
            // 
            this.btServir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btServir.Location = new System.Drawing.Point(538, 377);
            this.btServir.Name = "btServir";
            this.btServir.Size = new System.Drawing.Size(101, 34);
            this.btServir.TabIndex = 6;
            this.btServir.Text = "Servir";
            this.btServir.UseVisualStyleBackColor = true;
            this.btServir.Click += new System.EventHandler(this.btServir_Click);
            // 
            // btServirTodo
            // 
            this.btServirTodo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btServirTodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btServirTodo.Location = new System.Drawing.Point(667, 377);
            this.btServirTodo.Name = "btServirTodo";
            this.btServirTodo.Size = new System.Drawing.Size(101, 34);
            this.btServirTodo.TabIndex = 7;
            this.btServirTodo.Text = "Servir Todo";
            this.btServirTodo.UseVisualStyleBackColor = true;
            this.btServirTodo.Click += new System.EventHandler(this.btServirTodo_Click);
            // 
            // AdicionalDocumentoUnidadesServidas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btServirTodo);
            this.Controls.Add(this.btServir);
            this.Controls.Add(this.dataGridView);
            this.Name = "AdicionalDocumentoUnidadesServidas";
            this.Text = "Elegir el número de unidades a servir";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView;

        private string connectionString = "Data Source=AUXILIAR-MSI\\A3ERP;Initial Catalog=EjemploDDBB;User Id=Sa;Password=demo";
        private Button btServir;
        private Button btServirTodo;
    }
}