using System;
using System.Windows.Forms;
using hoccsharp.Forms;

namespace hoccsharp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Mở giao diện chính (Hub chuyển bài tập)
            Application.Run(new frmSchoolMain());
        }
    }
}