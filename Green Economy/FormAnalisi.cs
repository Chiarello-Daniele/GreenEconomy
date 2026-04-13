using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Green_Economy
{
    public partial class FormAnalisi : Form
    {
        private readonly List<DatoAmbientale> _dati;

        public FormAnalisi(List<DatoAmbientale> dati)
        {
            _dati = dati;
            InitializeComponent();
            CaricaClassifica();
        }

        private void CaricaClassifica()
        {
            // Prende l'ultimo record per ciascuna provincia
            List<DatoAmbientale> ultimi = _dati
                .Where(x => x.Provincia != null)
                .GroupBy(x => x.Provincia)
                .Select(g => g.OrderByDescending(x => x.DataOra).First())
                .ToList();

            if (ultimi.Count == 0) return;

            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.ColumnCount = 2;
            panel.RowCount = 2;
            panel.Padding = new Padding(8);
            panel.BackColor = Color.WhiteSmoke;
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tabClassifica.Controls.Add(panel);

            // Quattro grafici a barre: uno per indicatore
            List<(string Provincia, double Valore)> datiTemp = ultimi
                .OrderByDescending(x => x.Temperatura)
                .Select(x => (x.Provincia, (double)x.Temperatura))
                .ToList();

            List<(string Provincia, double Valore)> datiPm = ultimi
                .OrderByDescending(x => x.Inquinamento)
                .Select(x => (x.Provincia, (double)x.Inquinamento))
                .ToList();

            List<(string Provincia, double Valore)> datiUmid = ultimi
                .OrderByDescending(x => x.Umidita)
                .Select(x => (x.Provincia, (double)x.Umidita))
                .ToList();

            List<(string Provincia, double Valore)> datiVento = ultimi
                .OrderByDescending(x => x.Vento)
                .Select(x => (x.Provincia, (double)x.Vento))
                .ToList();

            panel.Controls.Add(CreaGrafico(datiTemp, "Temperatura (°C)", Color.OrangeRed), 0, 0);
            panel.Controls.Add(CreaGrafico(datiPm, "PM2.5 (µg/m³)", Color.SteelBlue), 1, 0);
            panel.Controls.Add(CreaGrafico(datiUmid, "Umidità (%)", Color.MediumSeaGreen), 0, 1);
            panel.Controls.Add(CreaGrafico(datiVento, "Vento (km/h)", Color.MediumPurple), 1, 1);
        }

        private Chart CreaGrafico(
            List<(string Provincia, double Valore)> dati,
            string titolo,
            Color colore)
        {
            Chart chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.White;
            chart.Margin = new Padding(4);

            ChartArea area = new ChartArea("area");
            area.BackColor = Color.WhiteSmoke;
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            chart.ChartAreas.Add(area);

            Title t = new Title(titolo);
            t.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            t.ForeColor = Color.DarkSlateGray;
            chart.Titles.Add(t);

            Series serie = new Series(titolo);
            serie.ChartType = SeriesChartType.Bar;
            serie.Color = colore;
            serie.IsValueShownAsLabel = true;
            serie.LabelForeColor = Color.Black;
            serie.Font = new Font("Segoe UI", 8F);

            for (int i = 0; i < dati.Count; i++)
            {
                string etichetta = dati[i].Provincia;
                if (i == 0) etichetta = "1. " + etichetta;
                else if (i == 1) etichetta = "2. " + etichetta;
                else if (i == 2) etichetta = "3. " + etichetta;

                serie.Points.AddXY(etichetta, dati[i].Valore);
            }

            chart.Series.Add(serie);
            return chart;
        }
    }
}
