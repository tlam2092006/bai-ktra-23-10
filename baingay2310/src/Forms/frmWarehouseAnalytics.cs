using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hoccsharp.Forms
{
    public partial class frmWarehouseAnalytics : Form
    {
        private CancellationTokenSource? _cts;

        private Button btnAnalyze = new Button();
        private Button btnCancel = new Button();
        private ProgressBar pgbStatus = new ProgressBar();
        private Label lblPercent = new Label();
        private DataGridView dgvReportData = new DataGridView();

        public frmWarehouseAnalytics()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Bảng điều khiển tổng hợp báo cáo kho hàng";
            this.Size = new Size(700, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            btnAnalyze.Text = "Thống Kê Kho";
            btnAnalyze.Location = new Point(20, 20);
            btnAnalyze.Size = new Size(120, 30);
            btnAnalyze.Click += btnAnalyze_Click;

            btnCancel.Text = "Hủy Tác Vụ";
            btnCancel.Location = new Point(150, 20);
            btnCancel.Size = new Size(100, 30);
            btnCancel.Enabled = false;
            btnCancel.Click += btnCancel_Click;

            pgbStatus.Location = new Point(20, 65);
            pgbStatus.Size = new Size(550, 25);

            lblPercent.Text = "0%";
            lblPercent.Location = new Point(580, 68);
            lblPercent.AutoSize = true;

            dgvReportData.Location = new Point(20, 105);
            dgvReportData.Size = new Size(640, 280);
            dgvReportData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.Controls.AddRange(new Control[] { btnAnalyze, btnCancel, pgbStatus, lblPercent, dgvReportData });
        }

        private async void btnAnalyze_Click(object? sender, EventArgs e)
        {
            btnAnalyze.Enabled = false;
            btnCancel.Enabled = true;
            pgbStatus.Value = 0;
            lblPercent.Text = "0%";

            _cts = new CancellationTokenSource();

            try
            {
                var progress = new Progress<int>(percent =>
                {
                    pgbStatus.Value = percent;
                    lblPercent.Text = $"{percent}%";
                });

                await Task.Run(() => PerformWarehouseCalculation(progress, _cts.Token), _cts.Token);
                MessageBox.Show("Thống kê hoàn tất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                pgbStatus.Value = 0;
                lblPercent.Text = "Đã hủy tác vụ!";
                MessageBox.Show("Đã hủy quá trình thống kê!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnAnalyze.Enabled = true;
                btnCancel.Enabled = false;
            }
        }

        private void PerformWarehouseCalculation(IProgress<int> progress, CancellationToken token)
        {
            for (int i = 1; i <= 100; i++)
            {
                token.ThrowIfCancellationRequested();
                Thread.Sleep(45);
                progress?.Report(i);
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            _cts?.Cancel();
        }
    }
}