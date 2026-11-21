namespace mb506.BMTech
{
    partial class mb506FrmHistorialVentas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblDesde = new System.Windows.Forms.Label();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dgvVentas = new System.Windows.Forms.DataGridView();
            this.colVentasIdVenta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVentasFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVentasDniCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVentasCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVentasVendedor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVentasTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVentasEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVentasFechaEntrega = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabsDetalle = new System.Windows.Forms.TabControl();
            this.tabDetalle = new System.Windows.Forms.TabPage();
            this.tabPagos = new System.Windows.Forms.TabPage();
            this.dgvDetalles = new System.Windows.Forms.DataGridView();
            this.colDetallesIdProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetallesCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetallesDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetallesCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetallesPrecioUnitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetallesSubtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPagos = new System.Windows.Forms.DataGridView();
            this.colPagosFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPagosMedioPago = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPagosImporte = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPagosNumeroOperacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPagosVerificado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnVerificarPago = new System.Windows.Forms.Button();
            this.btnComprobante = new System.Windows.Forms.Button();
            this.btnEntregar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            System.Windows.Forms.DataGridViewCellStyle styleColVentasTotal = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle styleColDetallesPrecioUnitario = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle styleColDetallesSubtotal = new System.Windows.Forms.DataGridViewCellStyle();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).BeginInit();
            this.tabsDetalle.SuspendLayout();
            this.tabDetalle.SuspendLayout();
            this.tabPagos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagos)).BeginInit();
            this.SuspendLayout();
            // lblDesde
            this.lblDesde.Location = new System.Drawing.Point(20, 24);
            this.lblDesde.Size = new System.Drawing.Size(70, 25);
            this.lblDesde.Text = "Desde";
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.TabIndex = 0;
            // lblHasta
            this.lblHasta.Location = new System.Drawing.Point(300, 24);
            this.lblHasta.Size = new System.Drawing.Size(70, 25);
            this.lblHasta.Text = "Hasta";
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.TabIndex = 1;
            // dtpDesde
            this.dtpDesde.Location = new System.Drawing.Point(90, 20);
            this.dtpDesde.Size = new System.Drawing.Size(180, 28);
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.TabIndex = 2;
            // dtpHasta
            this.dtpHasta.Location = new System.Drawing.Point(370, 20);
            this.dtpHasta.Size = new System.Drawing.Size(180, 28);
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.TabIndex = 3;
            // btnBuscar
            this.btnBuscar.Location = new System.Drawing.Point(580, 18);
            this.btnBuscar.Size = new System.Drawing.Size(130, 34);
            this.btnBuscar.BackColor = System.Drawing.Color.Teal;
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.TabIndex = 4;
            // dgvVentas
            this.dgvVentas.Location = new System.Drawing.Point(20, 75);
            this.dgvVentas.Size = new System.Drawing.Size(960, 270);
            this.dgvVentas.AllowUserToAddRows = false;
            this.dgvVentas.AllowUserToDeleteRows = false;
            this.dgvVentas.AutoGenerateColumns = false;
            this.dgvVentas.ReadOnly = true;
            this.dgvVentas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVentas.MultiSelect = false;
            this.dgvVentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVentas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvVentas.Name = "dgvVentas";
            this.dgvVentas.TabIndex = 5;
            // tabsDetalle
            this.tabsDetalle.Location = new System.Drawing.Point(20, 360);
            this.tabsDetalle.Size = new System.Drawing.Size(960, 255);
            this.tabsDetalle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.tabsDetalle.Name = "tabsDetalle";
            this.tabsDetalle.TabIndex = 6;
            // tabDetalle
            this.tabDetalle.Text = "Detalle de venta";
            this.tabDetalle.Name = "tabDetalle";
            // tabPagos
            this.tabPagos.Text = "Pagos";
            this.tabPagos.Name = "tabPagos";
            // dgvDetalles
            this.dgvDetalles.Location = new System.Drawing.Point(0, 0);
            this.dgvDetalles.Size = new System.Drawing.Size(950, 220);
            this.dgvDetalles.AllowUserToAddRows = false;
            this.dgvDetalles.AllowUserToDeleteRows = false;
            this.dgvDetalles.AutoGenerateColumns = false;
            this.dgvDetalles.ReadOnly = true;
            this.dgvDetalles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalles.MultiSelect = false;
            this.dgvDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalles.Name = "dgvDetalles";
            this.dgvDetalles.TabIndex = 9;
            // dgvPagos
            this.dgvPagos.Location = new System.Drawing.Point(0, 0);
            this.dgvPagos.Size = new System.Drawing.Size(950, 220);
            this.dgvPagos.AllowUserToAddRows = false;
            this.dgvPagos.AllowUserToDeleteRows = false;
            this.dgvPagos.AutoGenerateColumns = false;
            this.dgvPagos.ReadOnly = true;
            this.dgvPagos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPagos.Name = "dgvPagos";
            this.dgvPagos.TabIndex = 10;
            // btnVerificarPago
            this.btnVerificarPago.Location = new System.Drawing.Point(20, 635);
            this.btnVerificarPago.Size = new System.Drawing.Size(175, 36);
            this.btnVerificarPago.BackColor = System.Drawing.Color.Teal;
            this.btnVerificarPago.ForeColor = System.Drawing.Color.White;
            this.btnVerificarPago.UseVisualStyleBackColor = false;
            this.btnVerificarPago.Text = "Verificar pago";
            this.btnVerificarPago.Name = "btnVerificarPago";
            this.btnVerificarPago.TabIndex = 11;
            // btnComprobante
            this.btnComprobante.Location = new System.Drawing.Point(210, 635);
            this.btnComprobante.Size = new System.Drawing.Size(175, 36);
            this.btnComprobante.BackColor = System.Drawing.Color.Teal;
            this.btnComprobante.ForeColor = System.Drawing.Color.White;
            this.btnComprobante.UseVisualStyleBackColor = false;
            this.btnComprobante.Text = "Comprobante";
            this.btnComprobante.Name = "btnComprobante";
            this.btnComprobante.TabIndex = 12;
            // btnEntregar
            this.btnEntregar.Location = new System.Drawing.Point(400, 635);
            this.btnEntregar.Size = new System.Drawing.Size(175, 36);
            this.btnEntregar.BackColor = System.Drawing.Color.Teal;
            this.btnEntregar.ForeColor = System.Drawing.Color.White;
            this.btnEntregar.UseVisualStyleBackColor = false;
            this.btnEntregar.Text = "Registrar entrega";
            this.btnEntregar.Name = "btnEntregar";
            this.btnEntregar.TabIndex = 13;
            // btnCancelar
            this.btnCancelar.Location = new System.Drawing.Point(590, 635);
            this.btnCancelar.Size = new System.Drawing.Size(175, 36);
            this.btnCancelar.BackColor = System.Drawing.Color.Teal;
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Text = "Cancelar venta";
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.TabIndex = 14;
            this.colVentasIdVenta.DataPropertyName = "idVenta";
            this.colVentasIdVenta.HeaderText = "idVenta";
            this.colVentasIdVenta.Name = "idVenta";
            this.colVentasFecha.DataPropertyName = "fecha";
            this.colVentasFecha.HeaderText = "fecha";
            this.colVentasFecha.Name = "fecha";
            this.colVentasDniCliente.DataPropertyName = "dniCliente";
            this.colVentasDniCliente.HeaderText = "dniCliente";
            this.colVentasDniCliente.Name = "dniCliente";
            this.colVentasCliente.DataPropertyName = "cliente";
            this.colVentasCliente.HeaderText = "cliente";
            this.colVentasCliente.Name = "cliente";
            this.colVentasVendedor.DataPropertyName = "vendedor";
            this.colVentasVendedor.HeaderText = "vendedor";
            this.colVentasVendedor.Name = "vendedor";
            this.colVentasTotal.DataPropertyName = "total";
            this.colVentasTotal.HeaderText = "total";
            this.colVentasTotal.Name = "total";
            this.colVentasEstado.DataPropertyName = "estado";
            this.colVentasEstado.HeaderText = "estado";
            this.colVentasEstado.Name = "estado";
            this.colVentasFechaEntrega.DataPropertyName = "fechaEntrega";
            this.colVentasFechaEntrega.HeaderText = "fechaEntrega";
            this.colVentasFechaEntrega.Name = "fechaEntrega";
            this.colDetallesIdProducto.DataPropertyName = "idProducto";
            this.colDetallesIdProducto.HeaderText = "idProducto";
            this.colDetallesIdProducto.Name = "idProducto";
            this.colDetallesIdProducto.Visible = false;
            this.colDetallesCodigo.DataPropertyName = "codigo";
            this.colDetallesCodigo.HeaderText = "codigo";
            this.colDetallesCodigo.Name = "codigo";
            this.colDetallesDescripcion.DataPropertyName = "descripcion";
            this.colDetallesDescripcion.HeaderText = "descripcion";
            this.colDetallesDescripcion.Name = "descripcion";
            this.colDetallesCantidad.DataPropertyName = "cantidad";
            this.colDetallesCantidad.HeaderText = "cantidad";
            this.colDetallesCantidad.Name = "cantidad";
            this.colDetallesPrecioUnitario.DataPropertyName = "precioUnitario";
            this.colDetallesPrecioUnitario.HeaderText = "precioUnitario";
            this.colDetallesPrecioUnitario.Name = "precioUnitario";
            this.colDetallesSubtotal.DataPropertyName = "subtotal";
            this.colDetallesSubtotal.HeaderText = "subtotal";
            this.colDetallesSubtotal.Name = "subtotal";
            this.colPagosFecha.DataPropertyName = "fecha";
            this.colPagosFecha.HeaderText = "fecha";
            this.colPagosFecha.Name = "colPagosFecha";
            this.colPagosMedioPago.DataPropertyName = "medioPago";
            this.colPagosMedioPago.HeaderText = "medioPago";
            this.colPagosMedioPago.Name = "medioPago";
            this.colPagosImporte.DataPropertyName = "importe";
            this.colPagosImporte.HeaderText = "importe";
            this.colPagosImporte.Name = "importe";
            this.colPagosNumeroOperacion.DataPropertyName = "numeroOperacion";
            this.colPagosNumeroOperacion.HeaderText = "numeroOperacion";
            this.colPagosNumeroOperacion.Name = "numeroOperacion";
            this.colPagosVerificado.DataPropertyName = "verificado";
            this.colPagosVerificado.HeaderText = "verificado";
            this.colPagosVerificado.Name = "verificado";
            styleColVentasTotal.Format = "N2";
            this.colVentasTotal.DefaultCellStyle = styleColVentasTotal;
            styleColDetallesPrecioUnitario.Format = "N2";
            this.colDetallesPrecioUnitario.DefaultCellStyle = styleColDetallesPrecioUnitario;
            styleColDetallesSubtotal.Format = "N2";
            this.colDetallesSubtotal.DefaultCellStyle = styleColDetallesSubtotal;
            this.dgvVentas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colVentasIdVenta, this.colVentasFecha, this.colVentasDniCliente, this.colVentasCliente, this.colVentasVendedor, this.colVentasTotal, this.colVentasEstado, this.colVentasFechaEntrega });
            this.dgvDetalles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colDetallesIdProducto, this.colDetallesCodigo, this.colDetallesDescripcion, this.colDetallesCantidad, this.colDetallesPrecioUnitario, this.colDetallesSubtotal });
            this.dgvPagos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colPagosFecha, this.colPagosMedioPago, this.colPagosImporte, this.colPagosNumeroOperacion, this.colPagosVerificado });
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblDesde, this.lblHasta, this.dtpDesde, this.dtpHasta, this.btnBuscar, this.dgvVentas, this.tabsDetalle, this.btnVerificarPago, this.btnComprobante, this.btnEntregar, this.btnCancelar });
            this.tabsDetalle.Controls.AddRange(new System.Windows.Forms.Control[] { this.tabDetalle, this.tabPagos });
            this.tabDetalle.Controls.AddRange(new System.Windows.Forms.Control[] { this.dgvDetalles });
            this.tabPagos.Controls.AddRange(new System.Windows.Forms.Control[] { this.dgvPagos });
            this.btnBuscar.Click += new System.EventHandler(this.mb506Buscar);
            this.btnVerificarPago.Click += new System.EventHandler(this.mb506VerificarPago);
            this.btnComprobante.Click += new System.EventHandler(this.mb506Comprobante);
            this.btnEntregar.Click += new System.EventHandler(this.mb506Entregar);
            this.btnCancelar.Click += new System.EventHandler(this.mb506Cancelar);
            this.dgvVentas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mb506Seleccionar);
            this.dgvVentas.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.mb506FormatearVenta);
            this.dgvPagos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.mb506FormatearPago);
            this.Load += new System.EventHandler(this.mb506FrmHistorialVentas_Load);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1000, 690);
            this.MinimumSize = new System.Drawing.Size(1016, 729);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "FrmHistorialVentas";
            this.Text = "Historial de ventas";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).EndInit();
            this.tabPagos.ResumeLayout(false);
            this.tabPagos.PerformLayout();
            this.tabDetalle.ResumeLayout(false);
            this.tabDetalle.PerformLayout();
            this.tabsDetalle.ResumeLayout(false);
            this.tabsDetalle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView dgvVentas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVentasIdVenta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVentasFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVentasDniCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVentasCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVentasVendedor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVentasTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVentasEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVentasFechaEntrega;
        private System.Windows.Forms.TabControl tabsDetalle;
        private System.Windows.Forms.TabPage tabDetalle;
        private System.Windows.Forms.TabPage tabPagos;
        private System.Windows.Forms.DataGridView dgvDetalles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetallesIdProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetallesCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetallesDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetallesCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetallesPrecioUnitario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetallesSubtotal;
        private System.Windows.Forms.DataGridView dgvPagos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPagosFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPagosMedioPago;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPagosImporte;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPagosNumeroOperacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPagosVerificado;
        private System.Windows.Forms.Button btnVerificarPago;
        private System.Windows.Forms.Button btnComprobante;
        private System.Windows.Forms.Button btnEntregar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
