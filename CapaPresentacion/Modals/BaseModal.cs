using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Modals
{
    public partial class BaseModal : Form
    {
        protected Panel pnlHeader;
        protected Label lblModalTitle;
        protected Button btnMaximize;
        protected Panel pnlFooter;
        protected Button btnAccept;
        protected Button btnCancel;
        protected Panel pnlContent;

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public BaseModal()
        {
            InitializeComponent();
            ConfigurarEventoArrastre();
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblModalTitle = new Label();
            this.btnMaximize = new Button();
            this.pnlFooter = new Panel();
            this.btnAccept = new Button();
            this.btnCancel = new Button();
            this.pnlContent = new Panel();

            // Form
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Padding = new Padding(2);
            this.Size = new Size(500, 550);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            // Header
            this.pnlHeader.BackColor = Color.FromArgb(15, 23, 42);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 55;
            this.pnlHeader.Controls.Add(this.lblModalTitle);
            this.pnlHeader.Controls.Add(this.btnMaximize);

            this.lblModalTitle.ForeColor = Color.White;
            this.lblModalTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            this.lblModalTitle.Location = new Point(20, 18);
            this.lblModalTitle.AutoSize = true;
            this.lblModalTitle.Text = "TITULO MODAL";

            this.btnMaximize.Text = "▢";
            this.btnMaximize.Dock = DockStyle.Right;
            this.btnMaximize.Width = 45;
            this.btnMaximize.FlatStyle = FlatStyle.Flat;
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.ForeColor = Color.White;
            this.btnMaximize.Cursor = Cursors.Hand;
            this.btnMaximize.Click += (s, e) => {
                if (this.WindowState == FormWindowState.Maximized) this.WindowState = FormWindowState.Normal;
                else this.WindowState = FormWindowState.Maximized;
            };

            // Footer
            this.pnlFooter.Dock = DockStyle.Bottom;
            this.pnlFooter.Height = 70;
            this.pnlFooter.BackColor = Color.FromArgb(248, 250, 252);
            this.pnlFooter.Padding = new Padding(15);
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Controls.Add(this.btnAccept);

            this.btnAccept.Text = "ACEPTAR";
            this.btnAccept.BackColor = Color.FromArgb(37, 99, 235);
            this.btnAccept.ForeColor = Color.White;
            this.btnAccept.FlatStyle = FlatStyle.Flat;
            this.btnAccept.FlatAppearance.BorderSize = 0;
            this.btnAccept.Dock = DockStyle.Right;
            this.btnAccept.Width = 110;
            this.btnAccept.Cursor = Cursors.Hand;
            this.btnAccept.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            this.btnCancel.Text = "CANCELAR";
            this.btnCancel.BackColor = Color.FromArgb(226, 232, 240);
            this.btnCancel.ForeColor = Color.FromArgb(15, 23, 42);
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Dock = DockStyle.Right;
            this.btnCancel.Width = 110;
            this.btnCancel.Cursor = Cursors.Hand;
            this.btnCancel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.btnCancel.Margin = new Padding(0, 0, 15, 0);

            // Content
            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.Padding = new Padding(25);
            this.pnlContent.AutoScroll = true;

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);

            // Paint border
            this.Paint += (s, e) => {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(203, 213, 225), 1), 0, 0, this.Width - 1, this.Height - 1);
            };

            this.btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
        }

        private void ConfigurarEventoArrastre()
        {
            this.pnlHeader.MouseDown += (s, e) => {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0);
            };
        }

        private const int cGrip = 16;
        private const int cCaption = 32;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);

                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;
                    return;
                }

                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17;
                    return;
                }
                
                if (pos.X <= cGrip) m.Result = (IntPtr)10;
                else if (pos.X >= this.ClientSize.Width - cGrip) m.Result = (IntPtr)11;
                else if (pos.Y <= cGrip) m.Result = (IntPtr)12;
                else if (pos.Y >= this.ClientSize.Height - cGrip) m.Result = (IntPtr)15;
            }
            base.WndProc(ref m);
        }
    }
}
