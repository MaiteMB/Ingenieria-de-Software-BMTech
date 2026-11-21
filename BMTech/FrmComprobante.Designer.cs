namespace mb506.BMTech
{
    partial class mb506FrmComprobante
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void mb506DibujarPagina(System.Drawing.Printing.PrintPageEventArgs pagina, string contenido, out int caracteres)
        {
            int lineas;
            pagina.Graphics.MeasureString(contenido, this.txtComprobante.Font, pagina.MarginBounds.Size,
                System.Drawing.StringFormat.GenericTypographic, out caracteres, out lineas);
            pagina.Graphics.DrawString(contenido.Substring(0, caracteres), this.txtComprobante.Font,
                System.Drawing.Brushes.Black, pagina.MarginBounds, System.Drawing.StringFormat.GenericTypographic);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtComprobante = new System.Windows.Forms.TextBox();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnPdf = new System.Windows.Forms.Button();
            this.btnXml = new System.Windows.Forms.Button();
            this.btnImportarXml = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // txtComprobante
            this.txtComprobante.Location = new System.Drawing.Point(20, 20);
            this.txtComprobante.Size = new System.Drawing.Size(700, 540);
            this.txtComprobante.Multiline = true;
            this.txtComprobante.ReadOnly = true;
            this.txtComprobante.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtComprobante.WordWrap = false;
            this.txtComprobante.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtComprobante.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtComprobante.Name = "txtComprobante";
            this.txtComprobante.TabIndex = 0;
            // btnImprimir
            this.btnImprimir.Location = new System.Drawing.Point(20, 580);
            this.btnImprimir.Size = new System.Drawing.Size(130, 36);
            this.btnImprimir.BackColor = System.Drawing.Color.Teal;
            this.btnImprimir.ForeColor = System.Drawing.Color.White;
            this.btnImprimir.UseVisualStyleBackColor = false;
            this.btnImprimir.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.TabIndex = 1;
            // btnPdf
            this.btnPdf.Location = new System.Drawing.Point(170, 580);
            this.btnPdf.Size = new System.Drawing.Size(150, 36);
            this.btnPdf.BackColor = System.Drawing.Color.Teal;
            this.btnPdf.ForeColor = System.Drawing.Color.White;
            this.btnPdf.UseVisualStyleBackColor = false;
            this.btnPdf.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPdf.Text = "Guardar PDF";
            this.btnPdf.Name = "btnPdf";
            this.btnPdf.TabIndex = 2;
            // btnXml
            this.btnXml.Location = new System.Drawing.Point(340, 580);
            this.btnXml.Size = new System.Drawing.Size(150, 36);
            this.btnXml.BackColor = System.Drawing.Color.Teal;
            this.btnXml.ForeColor = System.Drawing.Color.White;
            this.btnXml.UseVisualStyleBackColor = false;
            this.btnXml.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnXml.Text = "Exportar XML";
            this.btnXml.Name = "btnXml";
            this.btnXml.TabIndex = 3;
            // btnImportarXml
            this.btnImportarXml.Location = new System.Drawing.Point(510, 580);
            this.btnImportarXml.Size = new System.Drawing.Size(150, 36);
            this.btnImportarXml.BackColor = System.Drawing.Color.Teal;
            this.btnImportarXml.ForeColor = System.Drawing.Color.White;
            this.btnImportarXml.UseVisualStyleBackColor = false;
            this.btnImportarXml.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnImportarXml.Text = "Importar XML";
            this.btnImportarXml.Name = "btnImportarXml";
            this.btnImportarXml.TabIndex = 4;
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.txtComprobante, this.btnImprimir, this.btnPdf, this.btnXml, this.btnImportarXml });
            this.btnImprimir.Click += new System.EventHandler(this.mb506Imprimir);
            this.btnPdf.Click += new System.EventHandler(this.mb506GuardarPdf);
            this.btnXml.Click += new System.EventHandler(this.mb506GuardarXml);
            this.btnImportarXml.Click += new System.EventHandler(this.mb506ImportarXml);
            this.Load += new System.EventHandler(this.mb506Cargar);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(740, 640);
            this.MinimumSize = new System.Drawing.Size(756, 679);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Name = "FrmComprobante";
            this.Text = "Comprobante de venta";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtComprobante;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnPdf;
        private System.Windows.Forms.Button btnXml;
        private System.Windows.Forms.Button btnImportarXml;
    }
}
