using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using hoccsharp.Models;

namespace hoccsharp.Forms
{
    public partial class frmBookingManager : Form
    {
        private BindingList<BookingModel> bookingList = new BindingList<BookingModel>();
        private BindingSource bindingSource = new BindingSource();
        private ErrorProvider errorProvider = new ErrorProvider();
        private int autoIncrementId = 1;

        // Khai báo các Controls UI
        private TextBox txtBookingId = new TextBox();
        private TextBox txtCustomerName = new TextBox();
        private TextBox txtIdentityCard = new TextBox();
        private ComboBox cboRoomType = new ComboBox();
        private DateTimePicker dtpCheckIn = new DateTimePicker();
        private NumericUpDown numDays = new NumericUpDown();
        private TextBox txtPricePerNight = new TextBox();
        private DataGridView dgvBookings = new DataGridView();
        private Button btnAdd = new Button();
        private Button btnUpdate = new Button();
        private Button btnDelete = new Button();
        private Button btnReset = new Button();

        public frmBookingManager()
        {
            InitializeComponent();
            SetupDataGridView();
            SetupDataBinding();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý đặt phòng khách sạn";
            this.Size = new Size(850, 550);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblId = new Label { Text = "Mã đặt:", Location = new Point(20, 20), AutoSize = true };
            txtBookingId.Location = new Point(120, 17);
            txtBookingId.ReadOnly = true;

            Label lblName = new Label { Text = "Tên khách:", Location = new Point(20, 55), AutoSize = true };
            txtCustomerName.Location = new Point(120, 52);

            Label lblCc = new Label { Text = "CCCD/CMND:", Location = new Point(20, 90), AutoSize = true };
            txtIdentityCard.Location = new Point(120, 87);

            Label lblRoom = new Label { Text = "Loại phòng:", Location = new Point(300, 20), AutoSize = true };
            cboRoomType.Location = new Point(390, 17);
            cboRoomType.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblDate = new Label { Text = "Ngày nhận:", Location = new Point(300, 55), AutoSize = true };
            dtpCheckIn.Location = new Point(390, 52);
            dtpCheckIn.Format = DateTimePickerFormat.Short;

            Label lblDays = new Label { Text = "Số ngày ở:", Location = new Point(300, 90), AutoSize = true };
            numDays.Location = new Point(390, 87);
            numDays.Minimum = 1;
            numDays.Maximum = 365;
            numDays.Value = 1;

            Label lblPrice = new Label { Text = "Giá phòng/đêm:", Location = new Point(580, 20), AutoSize = true };
            txtPricePerNight.Location = new Point(680, 17);

            btnAdd.Text = "Thêm";
            btnAdd.Location = new Point(580, 50);
            btnAdd.Click += btnAdd_Click;

            btnUpdate.Text = "Cập nhật";
            btnUpdate.Location = new Point(665, 50);
            btnUpdate.Click += btnUpdate_Click;

            btnDelete.Text = "Xóa";
            btnDelete.Location = new Point(750, 50);
            btnDelete.Click += btnDelete_Click;

            btnReset.Text = "Làm mới";
            btnReset.Location = new Point(580, 85);
            btnReset.Width = 245;
            btnReset.Click += btnReset_Click;

            dgvBookings.Location = new Point(20, 130);
            dgvBookings.Size = new Size(805, 360);
            dgvBookings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.Controls.AddRange(new Control[] {
                lblId, txtBookingId,
                lblName, txtCustomerName,
                lblCc, txtIdentityCard,
                lblRoom, cboRoomType,
                lblDate, dtpCheckIn,
                lblDays, numDays,
                lblPrice, txtPricePerNight,
                btnAdd, btnUpdate, btnDelete, btnReset,
                dgvBookings
            });
        }

        private void SetupDataGridView()
        {
            dgvBookings.AutoGenerateColumns = false;
            dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBookings.ReadOnly = true;
            dgvBookings.MultiSelect = false;

            dgvBookings.Columns.Clear();
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "BookingId", HeaderText = "Mã Đặt" });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CustomerName", HeaderText = "Tên Khách" });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdentityCard", HeaderText = "CCCD" });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RoomType", HeaderText = "Loại Phòng" });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CheckInDate", HeaderText = "Ngày Nhận", DefaultCellStyle = { Format = "dd/MM/yyyy" } });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Days", HeaderText = "Số Đêm" });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PricePerNight", HeaderText = "Giá/Đêm", DefaultCellStyle = { Format = "N0" } });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TotalAmount", HeaderText = "Tổng Tiền", DefaultCellStyle = { Format = "N0" } });
        }

        private void SetupDataBinding()
        {
            cboRoomType.DataSource = new string[] { "Standard", "Deluxe", "VIP" };
            bindingSource.DataSource = bookingList;
            dgvBookings.DataSource = bindingSource;

            txtBookingId.DataBindings.Add("Text", bindingSource, "BookingId", true, DataSourceUpdateMode.Never);
            txtCustomerName.DataBindings.Add("Text", bindingSource, "CustomerName", true, DataSourceUpdateMode.Never);
            txtIdentityCard.DataBindings.Add("Text", bindingSource, "IdentityCard", true, DataSourceUpdateMode.Never);
            cboRoomType.DataBindings.Add("SelectedItem", bindingSource, "RoomType", true, DataSourceUpdateMode.Never);
            dtpCheckIn.DataBindings.Add("Value", bindingSource, "CheckInDate", true, DataSourceUpdateMode.Never);
            numDays.DataBindings.Add("Value", bindingSource, "Days", true, DataSourceUpdateMode.Never);
            txtPricePerNight.DataBindings.Add("Text", bindingSource, "PricePerNight", true, DataSourceUpdateMode.Never, "0", "N0");

            GenerateNewBookingId();
        }

        private void GenerateNewBookingId()
        {
            txtBookingId.Text = $"BK{autoIncrementId:D3}";
        }

        private bool ValidateData()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                errorProvider.SetError(txtCustomerName, "Tên khách hàng không được để trống!");
                isValid = false;
            }

            string cccd = txtIdentityCard.Text.Trim();
            if (!Regex.IsMatch(cccd, @"^\d{9}$|^\d{12}$"))
            {
                errorProvider.SetError(txtIdentityCard, "CCCD phải đủ 9 hoặc 12 chữ số!");
                isValid = false;
            }

            string rawPrice = txtPricePerNight.Text.Replace(",", "").Replace(".", "").Trim();
            if (!decimal.TryParse(rawPrice, out decimal price) || price <= 0)
            {
                errorProvider.SetError(txtPricePerNight, "Giá phòng phải là số lớn hơn 0!");
                isValid = false;
            }

            return isValid;
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            if (!ValidateData()) return;

            string rawPrice = txtPricePerNight.Text.Replace(",", "").Replace(".", "").Trim();
            var booking = new BookingModel
            {
                BookingId = txtBookingId.Text,
                CustomerName = txtCustomerName.Text.Trim(),
                IdentityCard = txtIdentityCard.Text.Trim(),
                RoomType = cboRoomType.SelectedItem?.ToString() ?? "Standard",
                CheckInDate = dtpCheckIn.Value,
                Days = (int)numDays.Value,
                PricePerNight = decimal.Parse(rawPrice)
            };

            bookingList.Add(booking);
            autoIncrementId++;
            btnReset_Click(sender, e);
        }

        private void btnUpdate_Click(object? sender, EventArgs e)
        {
            if (bindingSource.Current == null || !ValidateData()) return;

            string rawPrice = txtPricePerNight.Text.Replace(",", "").Replace(".", "").Trim();
            var current = (BookingModel)bindingSource.Current;
            current.CustomerName = txtCustomerName.Text.Trim();
            current.IdentityCard = txtIdentityCard.Text.Trim();
            current.RoomType = cboRoomType.SelectedItem?.ToString() ?? "Standard";
            current.CheckInDate = dtpCheckIn.Value;
            current.Days = (int)numDays.Value;
            current.PricePerNight = decimal.Parse(rawPrice);

            bindingSource.ResetCurrentItem();
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (bindingSource.Current == null) return;

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa đơn đặt phòng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                bindingSource.RemoveCurrent();
                btnReset_Click(sender, e);
            }
        }

        private void btnReset_Click(object? sender, EventArgs e)
        {
            errorProvider.Clear();
            GenerateNewBookingId();
            txtCustomerName.Clear();
            txtIdentityCard.Clear();
            if (cboRoomType.Items.Count > 0) cboRoomType.SelectedIndex = 0;
            dtpCheckIn.Value = DateTime.Now;
            numDays.Value = 1;
            txtPricePerNight.Clear();
        }
    }
}