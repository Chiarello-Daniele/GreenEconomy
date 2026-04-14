namespace Green_Economy
{
    partial class FormAnalisi
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabClassifica = new System.Windows.Forms.TabPage();

            this.tabControl.SuspendLayout();
            this.SuspendLayout();

            // ── tabControl ─────────────────────────────────────────────────
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Controls.Add(this.tabClassifica);

            // ── tabClassifica ──────────────────────────────────────────────
            this.tabClassifica.Text = " Classifica Province";
            this.tabClassifica.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabClassifica.Padding = new System.Windows.Forms.Padding(8);

            // ── FormAnalisi ────────────────────────────────────────────────
            this.Text = "Analisi – Province del Veneto";
            this.Size = new System.Drawing.Size(950, 620);
            this.MinimumSize = new System.Drawing.Size(900, 580);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Controls.Add(this.tabControl);

            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabClassifica;
    }
}
