using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Green_Economy
{
    public partial class Form1 : Form
    {
        private const string JsonPath = "dati.json";

        private readonly List<(string Nome, double Lat, double Lon)> _province =
            new List<(string, double, double)>
            {
                ("Venezia", 45.4408, 12.3155),
                ("Verona",  45.4384, 10.9916),
                ("Padova",  45.4064, 11.8768),
                ("Treviso", 45.6669, 12.2430),
                ("Vicenza", 45.5455, 11.5354),
                ("Belluno", 46.1397, 12.2166),
                ("Rovigo",  45.0704, 11.7902)
            };

        private List<DatoAmbientale> _tuttiDati = new List<DatoAmbientale>();

        public Form1()
        {
            InitializeComponent();

            // Popola la tendina
            cmbProvincia.Items.Add("Tutte le province");
            foreach (var p in _province)
                cmbProvincia.Items.Add(p.Nome);
            cmbProvincia.SelectedIndex = 0;

            // Carica dati salvati se esistono
            if (File.Exists(JsonPath))
            {
                _tuttiDati = CaricaJSON();
                MostraDati();
            }
        }

        // ── Aggiorna griglia e grafico in base alla tendina ──────────────────
        private void MostraDati()
        {
            if (_tuttiDati.Count == 0) return;

            string scelta = cmbProvincia.SelectedItem.ToString();

            List<DatoAmbientale> filtrati;
            if (scelta == "Tutte le province")
                filtrati = _tuttiDati.Where(x => x.Provincia != null).ToList();
            else
                filtrati = _tuttiDati.Where(x => x.Provincia == scelta).ToList();

            // Griglia: tutti i record filtrati, dal più recente
            dgvDati.DataSource = filtrati.OrderByDescending(x => x.DataOra).ToList();

            // Grafico: un record per provincia (l'ultimo disponibile)
            List<DatoAmbientale> perGrafico = filtrati
                .GroupBy(x => x.Provincia)
                .Select(g => g.OrderByDescending(x => x.DataOra).First())
                .OrderBy(x => x.Provincia)
                .ToList();

            DisegnaGrafico(perGrafico);

            lblStato.Text = "Record totali: " + _tuttiDati.Count +
                            "  |  Visualizzati: " + filtrati.Count;
        }

        // ── Evento tendina: aggiorna la vista ────────────────────────────────
        private void cmbProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostraDati();
        }

        // ── Bottone Scarica ──────────────────────────────────────────────────
        private async void btnScarica_Click(object sender, EventArgs e)
        {
            btnScarica.Enabled = false;
            progressBar.Value = 0;
            progressBar.Maximum = _province.Count;

            List<DatoAmbientale> nuovi = new List<DatoAmbientale>();

            try
            {
                foreach (var (nome, lat, lon) in _province)
                {
                    lblStato.Text = "Scaricando " + nome + "...";
                    Application.DoEvents();

                    List<DatoAmbientale> giorni = await ScaricaQuattroGiorniAsync(nome, lat, lon);
                    nuovi.AddRange(giorni);

                    progressBar.Value++;
                    Application.DoEvents();
                }

                SalvaJSON(nuovi);
                _tuttiDati = CaricaJSON();
                MostraDati();

                lblStato.Text = " Completato – " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                MessageBox.Show("Scaricati " + nuovi.Count + " record (4 giorni × 7 province).");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore:\n" + ex.Message);
                lblStato.Text = "Errore durante il download.";
            }

            btnScarica.Enabled = true;
            progressBar.Value = 0;
        }

        private async Task<List<DatoAmbientale>> ScaricaQuattroGiorniAsync(
            string nome, double lat, double lon)
        {
            List<DatoAmbientale> risultati = new List<DatoAmbientale>();

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(15);
                client.DefaultRequestHeaders.Add("User-Agent", "GreenEconomy/1.0");

                string sLat = lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string sLon = lon.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string dataInizio = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
                string dataFine = DateTime.Now.ToString("yyyy-MM-dd");

                // API meteo
                string urlMeteo =
                    "https://api.open-meteo.com/v1/forecast" +
                    "?latitude=" + sLat + "&longitude=" + sLon +
                    "&hourly=temperature_2m,windspeed_10m,relativehumidity_2m" +
                    "&start_date=" + dataInizio + "&end_date=" + dataFine +
                    "&timezone=Europe%2FRome";

                dynamic meteo = JsonConvert.DeserializeObject(
                    await client.GetStringAsync(urlMeteo));

                // API qualità aria
                string urlAria =
                    "https://air-quality-api.open-meteo.com/v1/air-quality" +
                    "?latitude=" + sLat + "&longitude=" + sLon +
                    "&hourly=pm2_5" +
                    "&start_date=" + dataInizio + "&end_date=" + dataFine +
                    "&timezone=Europe%2FRome";

                dynamic aria = JsonConvert.DeserializeObject(
                    await client.GetStringAsync(urlAria));

                // Un record per ogni giorno 
                for (int g = 0; g < 4; g++)
                {
                    int idx = g * 24 + 12;

                    risultati.Add(new DatoAmbientale
                    {
                        Provincia = nome,
                        DataOra = DateTime.Now.AddDays(-(3 - g)).Date.AddHours(12),
                        Temperatura = LeggiDouble(meteo.hourly.temperature_2m, idx),
                        Vento = LeggiDouble(meteo.hourly.windspeed_10m, idx),
                        Umidita = (int)LeggiDouble(meteo.hourly.relativehumidity_2m, idx),
                        Inquinamento = (int)Math.Round(LeggiDouble(aria.hourly.pm2_5, idx))
                    });
                }
            }

            return risultati;
        }

        private double LeggiDouble(dynamic array, int idx)
        {
            try
            {
                if (array[idx] == null) return 0;
                return (double)array[idx];
            }
            catch { return 0; }
        }

        private void btnAnalizza_Click(object sender, EventArgs e)
        {
            List<DatoAmbientale> lista = CaricaJSON();
            if (lista.Count == 0)
            {
                MessageBox.Show("Nessun dato. Premi prima 'Scarica Dati'.");
                return;
            }
            FormAnalisi fa = new FormAnalisi(lista);
            fa.ShowDialog();
            fa.Dispose();
        }


        private void btnInfo_Click(object sender, EventArgs e)
        {
            FormInfo fi = new FormInfo();
            fi.ShowDialog();
            fi.Dispose();
        }

        private void SalvaJSON(List<DatoAmbientale> nuovi)
        {
            List<DatoAmbientale> storico = CaricaJSON();
            storico.AddRange(nuovi);
            File.WriteAllText(JsonPath, JsonConvert.SerializeObject(storico, Formatting.Indented));
        }

        private List<DatoAmbientale> CaricaJSON()
        {
            try
            {
                return JsonConvert.DeserializeObject<List<DatoAmbientale>>(
                    File.ReadAllText(JsonPath)) ?? new List<DatoAmbientale>();
            }
            catch { return new List<DatoAmbientale>(); }
        }

        private void DisegnaGrafico(List<DatoAmbientale> lista)
        {
            chartAnalisi.Series.Clear();
            chartAnalisi.ChartAreas[0].AxisX.Interval = 1;
            chartAnalisi.ChartAreas[0].AxisX.LabelStyle.Angle = -30;

            var sTemp = chartAnalisi.Series.Add("Temperatura (°C)");
            sTemp.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            sTemp.Color = System.Drawing.Color.OrangeRed;

            var sPm = chartAnalisi.Series.Add("PM2.5 (µg/m³)");
            sPm.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            sPm.Color = System.Drawing.Color.SteelBlue;

            var sUmid = chartAnalisi.Series.Add("Umidità (%)");
            sUmid.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            sUmid.Color = System.Drawing.Color.MediumSeaGreen;

            var sVento = chartAnalisi.Series.Add("Vento (km/h)");
            sVento.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            sVento.Color = System.Drawing.Color.MediumPurple;

            foreach (DatoAmbientale d in lista)
            {
                sTemp.Points.AddXY(d.Provincia, d.Temperatura);
                sPm.Points.AddXY(d.Provincia, d.Inquinamento);
                sUmid.Points.AddXY(d.Provincia, d.Umidita);
                sVento.Points.AddXY(d.Provincia, d.Vento);
            }
        }
    }
}
