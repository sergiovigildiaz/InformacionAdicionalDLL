namespace InformacionAdicional.Vistas
{
    partial class AdicionalBotones
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
            this.visualStudio2012LightTheme1 = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.botonFacturarAlbaran = new System.Windows.Forms.Button();
            this.btAAlbaran = new System.Windows.Forms.Button();
            this.btAPedido = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // botonFacturarAlbaran
            // 
            this.botonFacturarAlbaran.Location = new System.Drawing.Point(332, 10);
            this.botonFacturarAlbaran.Name = "botonFacturarAlbaran";
            this.botonFacturarAlbaran.Size = new System.Drawing.Size(141, 26);
            this.botonFacturarAlbaran.TabIndex = 0;
            this.botonFacturarAlbaran.Text = "A Factura";
            this.botonFacturarAlbaran.UseVisualStyleBackColor = true;
            this.botonFacturarAlbaran.Click += new System.EventHandler(this.botonAFactura_Click);
            // 
            // btAAlbaran
            // 
            this.btAAlbaran.Location = new System.Drawing.Point(173, 10);
            this.btAAlbaran.Name = "btAAlbaran";
            this.btAAlbaran.Size = new System.Drawing.Size(141, 26);
            this.btAAlbaran.TabIndex = 1;
            this.btAAlbaran.Text = "A Albarán";
            this.btAAlbaran.UseVisualStyleBackColor = true;
            this.btAAlbaran.Click += new System.EventHandler(this.botonAAlbaran_Click);
            // 
            // btAPedido
            // 
            this.btAPedido.Location = new System.Drawing.Point(12, 10);
            this.btAPedido.Name = "btAPedido";
            this.btAPedido.Size = new System.Drawing.Size(141, 26);
            this.btAPedido.TabIndex = 2;
            this.btAPedido.Text = "A Pedido";
            this.btAPedido.UseVisualStyleBackColor = true;
            this.btAPedido.Click += new System.EventHandler(this.botonAPedido_Click);
            // 
            // AdicionalBotonAlbaran
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 50);
            this.Controls.Add(this.btAPedido);
            this.Controls.Add(this.btAAlbaran);
            this.Controls.Add(this.botonFacturarAlbaran);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdicionalBotonAlbaran";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "AdicionalDocumentoCompras";
            this.ThemeName = "VisualStudio2012Light";
            this.Shown += new System.EventHandler(this.adicionalDoc_Shown);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme1;
        private System.Windows.Forms.Button botonFacturarAlbaran;
        private System.Windows.Forms.Button btAAlbaran;
        private System.Windows.Forms.Button btAPedido;
    }
}