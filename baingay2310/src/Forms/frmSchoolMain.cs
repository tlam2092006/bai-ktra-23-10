using System;
using System.Drawing;
using System.Windows.Forms;

namespace hoccsharp.Forms
{
    public partial class frmSchoolMain : Form
    {
        public frmSchoolMain()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Hệ thống Quản lý Tổng hợp - Chọn Bài tập";
            this.Size = new Size(1100, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;

            MenuStrip menuStrip = new MenuStrip();

            // 1. Menu Hệ thống
            ToolStripMenuItem menuSystem = new ToolStripMenuItem("Hệ thống");
            menuSystem.DropDownItems.Add(new ToolStripMenuItem("Thoát", null, (s, e) => Application.Exit()));

            // 2. Menu Chọn Bài Tập (Bấm bài nào mở bài đó)
            ToolStripMenuItem menuExercises = new ToolStripMenuItem("Chọn Bài Tập");

            menuExercises.DropDownItems.Add(new ToolStripMenuItem("Bài 1: Quản lý Đặt phòng Khách sạn", null, (s, e) => OpenChildForm<frmBookingManager>()));
            menuExercises.DropDownItems.Add(new ToolStripMenuItem("Bài 2: Quản lý Hồ sơ Nhân sự (CSV)", null, (s, e) => OpenChildForm<frmEmployeeManager>()));

            // Bài 3: Form con Sinh viên & Môn học
            ToolStripMenuItem menuBai3 = new ToolStripMenuItem("Bài 3: Quản lý Trường học (MDI)");
            menuBai3.DropDownItems.Add(new ToolStripMenuItem("Danh sách Sinh viên", null, (s, e) => OpenChildForm<frmStudentList>()));
            menuBai3.DropDownItems.Add(new ToolStripMenuItem("Danh mục Môn học", null, (s, e) => OpenChildForm<frmCourseList>()));
            menuExercises.DropDownItems.Add(menuBai3);

            menuExercises.DropDownItems.Add(new ToolStripMenuItem("Bài 4: Báo cáo Kho hàng (Async/Await)", null, (s, e) => OpenChildForm<frmWarehouseAnalytics>()));
            menuExercises.DropDownItems.Add(new ToolStripMenuItem("Bài 5: Giám sát IoT Real-time (Thread-Safety)", null, (s, e) => OpenChildForm<frmIoTMonitor>()));

            // 3. Menu Điều hướng Cửa sổ MDI
            ToolStripMenuItem menuWindow = new ToolStripMenuItem("Cửa sổ");
            menuWindow.DropDownItems.Add(new ToolStripMenuItem("Xếp tầng (Cascade)", null, (s, e) => LayoutMdi(MdiLayout.Cascade)));
            menuWindow.DropDownItems.Add(new ToolStripMenuItem("Xếp ngang (Tile Horizontal)", null, (s, e) => LayoutMdi(MdiLayout.TileHorizontal)));
            menuWindow.DropDownItems.Add(new ToolStripMenuItem("Xếp dọc (Tile Vertical)", null, (s, e) => LayoutMdi(MdiLayout.TileVertical)));
            menuWindow.DropDownItems.Add(new ToolStripMenuItem("Đóng tất cả cửa sổ", null, (s, e) => {
                foreach (Form child in this.MdiChildren) child.Close();
            }));

            menuStrip.Items.AddRange(new ToolStripItem[] { menuSystem, menuExercises, menuWindow });

            StatusStrip statusStrip = new StatusStrip();
            statusStrip.Items.Add(new ToolStripStatusLabel("Vui lòng vào menu 'Chọn Bài Tập' để mở bài tương ứng."));

            this.Controls.Add(menuStrip);
            this.Controls.Add(statusStrip);
            this.MainMenuStrip = menuStrip;
        }

        /// <summary>
        /// Hàm mở Form con: Nếu Form đã mở rồi thì đưa lên trước (Activate), nếu chưa mở mới khởi tạo.
        /// </summary>
        private void OpenChildForm<T>() where T : Form, new()
        {
            // 1. Kiểm tra xem Form đã có trong danh sách MdiChildren chưa
            foreach (Form child in this.MdiChildren)
            {
                if (child is T)
                {
                    child.Activate(); // Đưa Form đang mở lên phía trước
                    return;
                }
            }

            // 2. Nếu chưa có, tạo Instance mới và nhúng vào MDI Parent
            T newChild = new T
            {
                MdiParent = this
            };
            newChild.Show();
        }
    }

    public class frmStudentList : Form
    {
        public frmStudentList()
        {
            Text = "Danh sách Sinh viên";
            Size = new Size(500, 350);
            Controls.Add(new Label { Text = "Màn hình Quản lý Sinh viên (Bài 3)", AutoSize = true, Location = new Point(20, 20) });
        }
    }

    public class frmCourseList : Form
    {
        public frmCourseList()
        {
            Text = "Danh mục Môn học";
            Size = new Size(500, 350);
            Controls.Add(new Label { Text = "Màn hình Quản lý Môn học (Bài 3)", AutoSize = true, Location = new Point(20, 20) });
        }
    }
}