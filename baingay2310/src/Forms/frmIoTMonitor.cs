using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hoccsharp.Forms
{
    public partial class frmIoTMonitor : Form
    {
        private CancellationTokenSource? _cts;
        private int _cycleCount = 0;

        private Button btnStartMonitor = new Button();
        private Button btnStopMonitor = new Button();
        private Label lblTemperature = new Label();
        private DataGridView dgvLogs = new DataGridView();
        private ToolStripStatusLabel lblStatus = new ToolStripStatusLabel();

        public frmIoTMonitor()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Mô phỏng giám sát IoT Real-time";
            this.Size = new Size(650, 480);
            this.StartPosition = FormStartPosition.CenterScreen;

            btnStartMonitor.Text = "Bắt đầu Giám sát";
            btnStartMonitor.Location = new Point(20, 20);
            btnStartMonitor.Size = new Size(130, 35);
            btnStartMonitor.Click += btnStartMonitor_Click;

            btnStopMonitor.Text = "Dừng Giám sát";
            btnStopMonitor.Location = new Point(160, 20);
            btnStopMonitor.Size = new Size(120, 35);
            btnStopMonitor.Enabled = false;
            btnStopMonitor.Click += btnStopMonitor_Click;

            lblTemperature.Text = "-- °C";
            lblTemperature.Font = new Font("Arial", 22, FontStyle.Bold);
            lblTemperature.Location = new Point(320, 20);
            lblTemperature.AutoSize = true;

            dgvLogs.Location = new Point(20, 70);
            dgvLogs.Size = new Size(590, 330);
            dgvLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLogs.Columns.Add("Time", "Thời gian");
            dgvLogs.Columns.Add("Temp", "Nhiệt độ");
            dgvLogs.Columns.Add("Status", "Trạng thái");

            StatusStrip statusStrip = new StatusStrip();
            lblStatus.Text = "Trạng thái: Chưa kết nối | Chu kỳ nhận: 0";
            statusStrip.Items.Add(lblStatus);

            this.Controls.AddRange(new Control[] { btnStartMonitor, btnStopMonitor, lblTemperature, dgvLogs, statusStrip });
        }

        private void btnStartMonitor_Click(object? sender, EventArgs e)
        {
            btnStartMonitor.Enabled = false;
            btnStopMonitor.Enabled = true;
            _cts = new CancellationTokenSource();

            Task.Run(() => IoTMonitoringLoop(_cts.Token), _cts.Token);
        }

        private void IoTMonitoringLoop(CancellationToken token)
        {
            Random random = new Random();

            while (!token.IsCancellationRequested)
            {
                int temperature = random.Next(30, 43);
                _cycleCount++;

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateUI(temperature, _cycleCount)));
                }
                else
                {
                    UpdateUI(temperature, _cycleCount);
                }

                Thread.Sleep(2000);
            }
        }

        private void UpdateUI(int temp, int cycle)
        {
            lblTemperature.Text = $"{temp} °C";
            lblTemperature.ForeColor = (temp > 37) ? Color.Red : Color.Green;

            dgvLogs.Rows.Add(DateTime.Now.ToString("HH:mm:ss"), $"{temp} °C", temp > 37 ? "Cảnh báo sốt" : "Bình thường");
            lblStatus.Text = $"Trạng thái: Đang kết nối | Chu kỳ nhận: {cycle}";
        }

        private void btnStopMonitor_Click(object? sender, EventArgs e)
        {
            _cts?.Cancel();
            btnStartMonitor.Enabled = true;
            btnStopMonitor.Enabled = false;
            lblStatus.Text = "Trạng thái: Đã dừng giám sát";
        }
    }
}