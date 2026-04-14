namespace Green_Economy
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 =
                new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 =
                new System.Windows.Forms.DataVisualization.Charting.Legend();

            this.lblTitolo = new System.Windows.Forms.Label();
            this.btnScarica = new System.Windows.Forms.Button();
            this.btnAnalizza = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblProvincia = new System.Windows.Forms.Label();
            this.cmbProvincia = new System.Windows.Forms.ComboBox();
            this.dgvDati = new System.Windows.Forms.DataGridView();
            this.chartAnalisi = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblStato = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvDati)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAnalisi)).BeginInit();
            this.SuspendLayout();

            // ── lblTitolo ──────────────────────────────────────────────────
            this.lblTitolo.Location = new System.Drawing.Point(12, 10);
            this.lblTitolo.Size = new System.Drawing.Size(440, 32);
            this.lblTitolo.Text = "🌿 Green Economy – Province del Veneto";
            this.lblTitolo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitolo.ForeColor = System.Drawing.Color.DarkGreen;

            // ── btnScarica ─────────────────────────────────────────────────
            this.btnScarica.Location = new System.Drawing.Point(12, 52);
            this.btnScarica.Size = new System.Drawing.Size(145, 36);
            this.btnScarica.Text = "Scarica Dati";
            this.btnScarica.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnScarica.BackColor = System.Drawing.Color.SteelBlue;
            this.btnScarica.ForeColor = System.Drawing.Color.White;
            this.btnScarica.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScarica.Click += new System.EventHandler(this.btnScarica_Click);

            // ── btnAnalizza ────────────────────────────────────────────────
            this.btnAnalizza.Location = new System.Drawing.Point(165, 52);
            this.btnAnalizza.Size = new System.Drawing.Size(115, 36);
            this.btnAnalizza.Text = "Analizza";
            this.btnAnalizza.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAnalizza.BackColor = System.Drawing.Color.DarkOrange;
            this.btnAnalizza.ForeColor = System.Drawing.Color.White;
            this.btnAnalizza.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalizza.Click += new System.EventHandler(this.btnAnalizza_Click);

            // ── btnInfo ────────────────────────────────────────────────────
            this.btnInfo.Location = new System.Drawing.Point(288, 52);
            this.btnInfo.Size = new System.Drawing.Size(90, 36);
            this.btnInfo.Text = "ℹ  Info";
            this.btnInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnInfo.BackColor = System.Drawing.Color.SeaGreen;
            this.btnInfo.ForeColor = System.Drawing.Color.White;
            this.btnInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);

            // ── progressBar ────────────────────────────────────────────────
            this.progressBar.Location = new System.Drawing.Point(12, 98);
            this.progressBar.Size = new System.Drawing.Size(370, 16);
            this.progressBar.Minimum = 0;
            this.progressBar.Maximum = 7;
            this.progressBar.Value = 0;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;

            // ── lblFile ────────────────────────────────────────────────────
            this.lblFile.Location = new System.Drawing.Point(12, 120);
            this.lblFile.Size = new System.Drawing.Size(370, 18);
            this.lblFile.Text = "File: dati.json";
            this.lblFile.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblFile.ForeColor = System.Drawing.Color.Gray;

            // ── lblProvincia ───────────────────────────────────────────────
            this.lblProvincia.Location = new System.Drawing.Point(12, 147);
            this.lblProvincia.Size = new System.Drawing.Size(85, 22);
            this.lblProvincia.Text = "Provincia:";
            this.lblProvincia.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblProvincia.ForeColor = System.Drawing.Color.DarkSlateGray;

            // ── cmbProvincia ───────────────────────────────────────────────
            this.cmbProvincia.Location = new System.Drawing.Point(100, 145);
            this.cmbProvincia.Size = new System.Drawing.Size(280, 24);
            this.cmbProvincia.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbProvincia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProvincia.SelectedIndexChanged += new System.EventHandler(this.cmbProvincia_SelectedIndexChanged);

            // ── dgvDati ────────────────────────────────────────────────────
            this.dgvDati.Location = new System.Drawing.Point(12, 178);
            this.dgvDati.Size = new System.Drawing.Size(370, 330);
            this.dgvDati.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDati.ReadOnly = true;
            this.dgvDati.AllowUserToAddRows = false;
            this.dgvDati.RowHeadersVisible = false;
            this.dgvDati.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvDati.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvDati.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;
            this.dgvDati.ColumnHeadersDefaultCellStyle.Font =
                new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            // ── chartAnalisi ───────────────────────────────────────────────
            chartArea1.Name = "ChartArea1";
            chartArea1.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Segoe UI", 8F);
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Segoe UI", 8F);
            chartArea1.AxisX.Interval = 1;
            this.chartAnalisi.ChartAreas.Add(chartArea1);

            legend1.Name = "Legend1";
            legend1.Font = new System.Drawing.Font("Segoe UI", 9F);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            this.chartAnalisi.Legends.Add(legend1);

            this.chartAnalisi.Location = new System.Drawing.Point(400, 10);
            this.chartAnalisi.Size = new System.Drawing.Size(590, 495);
            this.chartAnalisi.BackColor = System.Drawing.Color.White;
            this.chartAnalisi.BorderlineColor = System.Drawing.Color.LightGray;
            this.chartAnalisi.BorderlineDashStyle =
                System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;

            // ── lblStato ───────────────────────────────────────────────────
            this.lblStato.Location = new System.Drawing.Point(12, 518);
            this.lblStato.Size = new System.Drawing.Size(975, 22);
            this.lblStato.Text = "";
            this.lblStato.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStato.ForeColor = System.Drawing.Color.DarkSlateGray;

            // ── Form1 ──────────────────────────────────────────────────────
            this.ClientSize = new System.Drawing.Size(1005, 552);
            this.Text = "Green Economy – Analisi Ambientale Veneto";
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MinimumSize = new System.Drawing.Size(1021, 591);

            this.Controls.Add(this.lblTitolo);
            this.Controls.Add(this.btnScarica);
            this.Controls.Add(this.btnAnalizza);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.lblProvincia);
            this.Controls.Add(this.cmbProvincia);
            this.Controls.Add(this.dgvDati);
            this.Controls.Add(this.chartAnalisi);
            this.Controls.Add(this.lblStato);

            ((System.ComponentModel.ISupportInitialize)(this.dgvDati)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAnalisi)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitolo;
        private System.Windows.Forms.Button btnScarica;
        private System.Windows.Forms.Button btnAnalizza;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblProvincia;
        private System.Windows.Forms.ComboBox cmbProvincia;
        private System.Windows.Forms.DataGridView dgvDati;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAnalisi;
        private System.Windows.Forms.Label lblStato;
    }
}
