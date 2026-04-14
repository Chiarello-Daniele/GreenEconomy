namespace Green_Economy
{
    partial class FormInfo
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitolo = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.RichTextBox();
            this.btnChiudi = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // ── lblTitolo ──────────────────────────────────────────────────
            this.lblTitolo.Location = new System.Drawing.Point(16, 14);
            this.lblTitolo.Size = new System.Drawing.Size(510, 30);
            this.lblTitolo.Text = "🌿 Green Economy – Informazioni sul programma";
            this.lblTitolo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitolo.ForeColor = System.Drawing.Color.DarkGreen;

            // ── txtInfo ────────────────────────────────────────────────────
            this.txtInfo.Location = new System.Drawing.Point(16, 52);
            this.txtInfo.Size = new System.Drawing.Size(512, 240);
            this.txtInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtInfo.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.txtInfo.BackColor = System.Drawing.Color.White;
            this.txtInfo.ReadOnly = true;
            this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtInfo.WordWrap = true;
            this.txtInfo.Text =
                "Green Economy è un'applicazione per il monitoraggio ambientale " +
                "delle sette province della Regione Veneto: Venezia, Verona, Padova, " +
                "Treviso, Vicenza, Belluno e Rovigo.\r\n\r\n" +
                "Premendo il pulsante \"Scarica Dati\" il programma si collega alle API " +
                "gratuite di Open-Meteo e scarica, per ciascuna provincia, i dati " +
                "degli ultimi quattro giorni: temperatura (°C), velocità del vento (km/h), " +
                "umidità relativa (%) e concentrazione di PM2.5 (µg/m³).\r\n\r\n" +
                "I dati vengono salvati nel file \"dati.json\" e restano disponibili " +
                "anche senza connessione a Internet nelle sessioni successive.\r\n\r\n" +
                "Dalla tendina \"Provincia\" puoi filtrare la griglia e il grafico " +
                "per una singola provincia oppure visualizzare tutte insieme.\r\n\r\n" +
                "Il pulsante \"Analizza\" apre una finestra con le classifiche delle " +
                "province ordinate per ciascun indicatore ambientale, così da " +
                "confrontare rapidamente la qualità dell'aria e le condizioni " +
                "climatiche su tutto il territorio veneto.";

            // ── btnChiudi ──────────────────────────────────────────────────
            this.btnChiudi.Location = new System.Drawing.Point(209, 308);
            this.btnChiudi.Size = new System.Drawing.Size(120, 34);
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnChiudi.BackColor = System.Drawing.Color.SteelBlue;
            this.btnChiudi.ForeColor = System.Drawing.Color.White;
            this.btnChiudi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);

            // ── FormInfo ───────────────────────────────────────────────────
            this.Text = "Informazioni";
            this.ClientSize = new System.Drawing.Size(544, 358);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.Controls.Add(this.lblTitolo);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.btnChiudi);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitolo;
        private System.Windows.Forms.RichTextBox txtInfo;
        private System.Windows.Forms.Button btnChiudi;
    }
}
