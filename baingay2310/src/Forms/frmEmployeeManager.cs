using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using hoccsharp.Models;
using WinFormsTimer = System.Windows.Forms.Timer;

namespace hoccsharp.Forms
{
    public partial class frmEmployeeManager : Form
    {
        private BindingList<EmployeeModel> employeeList = new BindingList<EmployeeModel>();

        private PictureBox picAvatar = new PictureBox();
        private DataGridView dgvEmployees = new DataGridView();
        private ToolStripStatusLabel lblTotalEmployees = new ToolStripStatusLabel();
        private ToolStripStatusLabel lblTotalSalary = new ToolStripStatusLabel();
        private ToolStripStatusLabel lblTimeStatus = new ToolStripStatusLabel();

        public frmEmployeeManager()
        {
            InitializeComponent();
            employeeList.ListChanged += (s, e) => UpdateStatusStrip();
            dgvEmployees.DataSource = employeeList;

            WinFormsTimer timer = new WinFormsTimer { Interval = 1000 };
            timer.Tick += (s, e) => lblTimeStatus.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timer.Start();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý hồ sơ nhân sự";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem menuFile = new ToolStripMenuItem("File");
            ToolStripMenuItem menuOpen = new ToolStripMenuItem("Mở CSV", null, btnOpenCsv_Click);
            ToolStripMenuItem menuSave = new ToolStripMenuItem("Lưu CSV", null, btnSaveCsv_Click);
            ToolStripMenuItem menuExit = new ToolStripMenuItem("Thoát", null, (s, e) => this.Close());
            menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuOpen, menuSave, menuExit });
            menuStrip.Items.Add(menuFile);

            ToolStrip toolStrip = new ToolStrip();
            ToolStripButton btnOpenTool = new ToolStripButton("Mở CSV", null, btnOpenCsv_Click);
            ToolStripButton btnSaveTool = new ToolStripButton("Lưu CSV", null, btnSaveCsv_Click);
            toolStrip.Items.AddRange(new ToolStripItem[] { btnOpenTool, btnSaveTool });

            StatusStrip statusStrip = new StatusStrip();
            lblTotalEmployees.Text = "Tổng số nhân viên: 0";
            lblTotalSalary.Text = "Tổng quỹ lương: 0 VNĐ";
            lblTimeStatus.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            statusStrip.Items.AddRange(new ToolStripItem[] { lblTotalEmployees, new ToolStripStatusLabel(" | "), lblTotalSalary, new ToolStripStatusLabel(" | "), lblTimeStatus });

            picAvatar.Size = new Size(120, 140);
            picAvatar.Location = new Point(20, 60);
            picAvatar.BorderStyle = BorderStyle.FixedSingle;
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;

            Button btnSelectAvatar = new Button { Text = "Chọn ảnh", Location = new Point(20, 210), Size = new Size(120, 30) };
            btnSelectAvatar.Click += btnSelectAvatar_Click;

            dgvEmployees.Location = new Point(160, 60);
            dgvEmployees.Size = new Size(600, 350);
            dgvEmployees.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.Controls.Add(dgvEmployees);
            this.Controls.Add(picAvatar);
            this.Controls.Add(btnSelectAvatar);
            this.Controls.Add(toolStrip);
            this.Controls.Add(menuStrip);
            this.Controls.Add(statusStrip);
            this.MainMenuStrip = menuStrip;
        }

        private void btnSelectAvatar_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picAvatar.Image = Image.FromFile(ofd.FileName);
                    picAvatar.Tag = ofd.FileName;
                }
            }
        }

        private void btnSaveCsv_Click(object? sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV Files (*.csv)|*.csv" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("EmployeeId,FullName,Position,Salary,AvatarPath");
                    foreach (var emp in employeeList)
                    {
                        sb.AppendLine($"{emp.EmployeeId},{emp.FullName},{emp.Position},{emp.Salary},{emp.AvatarPath}");
                    }
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất danh sách CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnOpenCsv_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "CSV Files (*.csv)|*.csv" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    employeeList.Clear();
                    var lines = File.ReadAllLines(ofd.FileName, Encoding.UTF8);
                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(lines[i])) continue;
                        var parts = lines[i].Split(',');
                        if (parts.Length >= 5)
                        {
                            employeeList.Add(new EmployeeModel
                            {
                                EmployeeId = parts[0],
                                FullName = parts[1],
                                Position = parts[2],
                                Salary = decimal.TryParse(parts[3], out decimal sal) ? sal : 0,
                                AvatarPath = parts[4]
                            });
                        }
                    }
                    UpdateStatusStrip();
                }
            }
        }

        private void UpdateStatusStrip()
        {
            lblTotalEmployees.Text = $"Tổng số nhân viên: {employeeList.Count}";
            lblTotalSalary.Text = $"Tổng quỹ lương: {employeeList.Sum(x => x.Salary):N0} VNĐ";
        }
    }
}