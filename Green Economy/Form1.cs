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
        // Percorso del file dati.json nella cartella di esecuzione del programma
        private string JsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dati.json");

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

            // Inizializza il file JSON se non esiste o è vuoto
            InitializeJSONFile();

            // Carica dati salvati se esistono (mantieni lo storico)
            _tuttiDati = CaricaJSON();
            if (_tuttiDati.Count > 0)
            {
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
            RefreshDataGridView(filtrati.OrderByDescending(x => x.DataOra));

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

                    // scarica i dati per la provincia (record correnti)
                    List<DatoAmbientale> dati = await ScaricaDatiProvinciaAsync(nome, lat, lon);
                    nuovi.AddRange(dati);

                    progressBar.Value++;
                    Application.DoEvents();
                }

                // Aggiungi i nuovi record allo storico e aggiorna la UI (helper semplificato)
                UpdateAfterDownload(nuovi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore:\n" + ex.Message);
                lblStato.Text = "Errore durante il download.";
            }

            btnScarica.Enabled = true;
            progressBar.Value = 0;
        }

        private async Task<List<DatoAmbientale>> ScaricaDatiProvinciaAsync(
            string nome, double lat, double lon)
        {
            List<DatoAmbientale> risultati = new List<DatoAmbientale>();

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(15);
                client.DefaultRequestHeaders.Add("User-Agent", "GreenEconomy/1.0");

                string sLat = lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string sLon = lon.ToString(System.Globalization.CultureInfo.InvariantCulture);
                // Scarica solo i dati del giorno corrente
                string dataInizio = DateTime.Now.ToString("yyyy-MM-dd");
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

                // Prendiamo solo il valore dell'ora più recente disponibile (<= ora corrente)
                try
                {
                    var times = meteo.hourly.time;
                    int len = times.Count;
                    DateTime now = DateTime.Now;
                    int bestIdx = -1;
                    for (int i = 0; i < len; i++)
                    {
                        try
                        {
                            DateTime dt = DateTime.Parse((string)times[i], System.Globalization.CultureInfo.InvariantCulture);
                            if (dt <= now) bestIdx = i;
                        }
                        catch { }
                    }

                    if (bestIdx == -1) bestIdx = Math.Max(0, len - 1);

                    // Impostiamo la data/ora al momento del download (precisione al minuto)
                    DateTime downloadTime = DateTime.Now;
                    var oraCorrenteTroncata = new DateTime(downloadTime.Year, downloadTime.Month, downloadTime.Day, downloadTime.Hour, downloadTime.Minute, 0);

                    risultati.Add(new DatoAmbientale
                    {
                        Provincia = nome,
                        DataOra = oraCorrenteTroncata,
                        Temperatura = LeggiDouble(meteo.hourly.temperature_2m, bestIdx),
                        Vento = LeggiDouble(meteo.hourly.windspeed_10m, bestIdx),
                        Umidita = (int)LeggiDouble(meteo.hourly.relativehumidity_2m, bestIdx),
                        Inquinamento = (int)Math.Round(LeggiDouble(aria.hourly.pm2_5, bestIdx))
                    });
                }
                catch
                {
                    // fallback: singolo valore a mezzogiorno di oggi
                    int idx = 12;
                    risultati.Add(new DatoAmbientale
                    {
                        Provincia = nome,
                        DataOra = DateTime.Now.Date.AddHours(12),
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

        // Sovrascrive il file JSON con la lista fornita (pulizia completa)
        private void SalvaJSONOverwrite(List<DatoAmbientale> nuovi)
        {
            File.WriteAllText(JsonPath, JsonConvert.SerializeObject(nuovi, Formatting.Indented));
        }

        // Riduce lo storico mantenendo solo l'ultimo record per provincia
        private List<DatoAmbientale> PulisciStorico(List<DatoAmbientale> storico)
        {
            return storico
                .Where(x => !string.IsNullOrEmpty(x.Provincia))
                .GroupBy(x => x.Provincia)
                .Select(g => g.OrderByDescending(x => x.DataOra).First())
                .OrderBy(x => x.Provincia)
                .ToList();
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

        // Semplifica l'aggiornamento della DataGridView in un unico posto
        private void RefreshDataGridView(IEnumerable<DatoAmbientale> lista)
        {
            var toShow = lista?.ToList() ?? new List<DatoAmbientale>();
            dgvDati.DataSource = null;
            dgvDati.DataSource = toShow;
            try
            {
                if (dgvDati.Columns.Contains("DataOra"))
                {
                    dgvDati.Columns["DataOra"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    dgvDati.Columns["DataOra"].HeaderText = "Data e Ora";
                }
            }
            catch { }
        }

        // Restituisce l'ultimo record disponibile per provincia da una sorgente
        private List<DatoAmbientale> GetLatestPerProvince(IEnumerable<DatoAmbientale> source)
        {
            return (source ?? Enumerable.Empty<DatoAmbientale>())
                .Where(x => !string.IsNullOrEmpty(x.Provincia))
                .GroupBy(x => x.Provincia)
                .Select(g => g.OrderByDescending(x => x.DataOra).First())
                .OrderBy(x => x.Provincia)
                .ToList();
        }

        // Operazioni dopo il download: salva, ricarica storico, aggiorna griglia e grafico
        private void UpdateAfterDownload(List<DatoAmbientale> nuovi)
        {
            SalvaJSON(nuovi); // aggiunge allo storico
            _tuttiDati = CaricaJSON();
            cmbProvincia.SelectedIndex = 0;

            // mostra tutto lo storico
            RefreshDataGridView(_tuttiDati.OrderByDescending(x => x.DataOra));

            // aggiorna grafico con l'ultimo per provincia
            var perGrafico = GetLatestPerProvince(_tuttiDati);
            DisegnaGrafico(perGrafico);

            lblStato.Text = "Completato – " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") +
                            "  |  Scaricati: " + nuovi.Count + "  |  Record totali: " + _tuttiDati.Count;

            MessageBox.Show("Scaricati " + nuovi.Count + " record (1 per provincia, ora corrente).\nI dati vecchi sono stati mantenuti nel file.");
        }

        // Crea il file JSON vuoto se non esiste o non contiene dati validi
        private void InitializeJSONFile()
        {
            try
            {
                // Se il file non esiste, crealo con un array vuoto
                if (!File.Exists(JsonPath))
                {
                    File.WriteAllText(JsonPath, "[]");
                    return;
                }

                // Leggi il file e verifica se è valido
                string content = File.ReadAllText(JsonPath);

                // Se è vuoto o non è valido JSON, ricrealo
                if (string.IsNullOrWhiteSpace(content))
                {
                    File.WriteAllText(JsonPath, "[]");
                    return;
                }

                // Prova a deserializzare: se fallisce, ricrea il file
                try
                {
                    JsonConvert.DeserializeObject<List<DatoAmbientale>>(content);
                }
                catch
                {
                    File.WriteAllText(JsonPath, "[]");
                }
            }
            catch { /* ignora errori nella inizializzazione */ }
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
