namespace CapaPresentacion
{
    partial class Frm_Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStripPrincipal = new System.Windows.Forms.MenuStrip();
            this.cerrarSesionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripPrincipal = new System.Windows.Forms.StatusStrip();
            this.tslUsuarioLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.statusStripPrincipal.SuspendLayout();
            this.menuStripPrincipal.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripPrincipal
            // 
            this.menuStripPrincipal.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.menuStripPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cerrarSesionToolStripMenuItem});
            this.menuStripPrincipal.Location = new System.Drawing.Point(0, 0);
            this.menuStripPrincipal.Name = "menuStripPrincipal";
            this.menuStripPrincipal.Size = new System.Drawing.Size(800, 28);
            this.menuStripPrincipal.TabIndex = 0;
            this.menuStripPrincipal.Text = "menuStrip1";
            // 
            // cerrarSesionToolStripMenuItem
            // 
            this.cerrarSesionToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cerrarSesionToolStripMenuItem.BackColor = System.Drawing.Color.MistyRose;
            this.cerrarSesionToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.cerrarSesionToolStripMenuItem.Name = "cerrarSesionToolStripMenuItem";
            this.cerrarSesionToolStripMenuItem.Size = new System.Drawing.Size(109, 24);
            this.cerrarSesionToolStripMenuItem.Text = "Cerrar Sesión";
            this.cerrarSesionToolStripMenuItem.Click += new System.EventHandler(this.cerrarSesionToolStripMenuItem_Click);
            // 
            // statusStripPrincipal
            // 
            this.statusStripPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslUsuarioLabel});
            this.statusStripPrincipal.Location = new System.Drawing.Point(0, 428);
            this.statusStripPrincipal.Name = "statusStripPrincipal";
            this.statusStripPrincipal.Size = new System.Drawing.Size(800, 22);
            this.statusStripPrincipal.TabIndex = 1;
            this.statusStripPrincipal.Text = "statusStrip1";
            // 
            // tslUsuarioLabel
            // 
            this.tslUsuarioLabel.Name = "tslUsuarioLabel";
            this.tslUsuarioLabel.Size = new System.Drawing.Size(50, 17);
            this.tslUsuarioLabel.Text = "Usuario:";
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.AutoSize = true;
            this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBienvenida.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblBienvenida.Location = new System.Drawing.Point(240, 180);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(320, 45);
            this.lblBienvenida.TabIndex = 2;
            this.lblBienvenida.Text = "BIENVENIDO A ADCIVET";
            // 
            // Frm_Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblBienvenida);
            this.Controls.Add(this.statusStripPrincipal);
            this.Controls.Add(this.menuStripPrincipal);
            this.MainMenuStrip = this.menuStripPrincipal;
            this.Name = "Frm_Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ADCIVET - Home";
            this.Load += new System.EventHandler(this.Frm_Home_Load);
            this.statusStripPrincipal.ResumeLayout(false);
            this.statusStripPrincipal.PerformLayout();
            this.menuStripPrincipal.ResumeLayout(false);
            this.menuStripPrincipal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripPrincipal;
        private System.Windows.Forms.ToolStripMenuItem cerrarSesionToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStripPrincipal;
        private System.Windows.Forms.ToolStripStatusLabel tslUsuarioLabel;
        private System.Windows.Forms.Label lblBienvenida;
    }
}