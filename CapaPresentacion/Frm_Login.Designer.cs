namespace CapaPresentacion
{
    partial class Frm_Login
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
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtClave = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblClave = new System.Windows.Forms.Label();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pnlLoginCard = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pnlBordeClave = new System.Windows.Forms.Panel();
            this.pnlBordeUsuario = new System.Windows.Forms.Panel();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.pnlLoginCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUsuario
            // 
            this.txtUsuario.BackColor = System.Drawing.Color.White;
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsuario.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtUsuario.Location = new System.Drawing.Point(30, 155);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(290, 22);
            this.txtUsuario.TabIndex = 0;
            // 
            // txtClave
            // 
            this.txtClave.BackColor = System.Drawing.Color.White;
            this.txtClave.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtClave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtClave.Location = new System.Drawing.Point(30, 235);
            this.txtClave.Name = "txtClave";
            this.txtClave.PasswordChar = '●';
            this.txtClave.Size = new System.Drawing.Size(290, 22);
            this.txtClave.TabIndex = 1;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.Color.Gray;
            this.lblUsuario.Location = new System.Drawing.Point(30, 130);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(128, 17);
            this.lblUsuario.TabIndex = 2;
            this.lblUsuario.Text = "Nombre de Usuario";
            // 
            // lblClave
            // 
            this.lblClave.AutoSize = true;
            this.lblClave.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClave.ForeColor = System.Drawing.Color.Gray;
            this.lblClave.Location = new System.Drawing.Point(30, 210);
            this.lblClave.Name = "lblClave";
            this.lblClave.Size = new System.Drawing.Size(77, 17);
            this.lblClave.TabIndex = 3;
            this.lblClave.Text = "Contraseña";
            // 
            // btnIngresar
            // 
            this.btnIngresar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(117)))), ((int)(((byte)(188)))));
            this.btnIngresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIngresar.FlatAppearance.BorderSize = 0;
            this.btnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresar.ForeColor = System.Drawing.Color.White;
            this.btnIngresar.Location = new System.Drawing.Point(30, 300);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(290, 45);
            this.btnIngresar.TabIndex = 4;
            this.btnIngresar.Text = "Iniciar Sesión";
            this.btnIngresar.UseVisualStyleBackColor = false;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.Gray;
            this.btnCancelar.Location = new System.Drawing.Point(135, 355);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(80, 30);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cerrar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(117)))), ((int)(((byte)(188)))));
            this.lblTitulo.Location = new System.Drawing.Point(105, 30);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(130, 37);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "ADCIVET";
            // 
            // pnlLoginCard
            // 
            this.pnlLoginCard.BackColor = System.Drawing.Color.White;
            this.pnlLoginCard.Controls.Add(this.lblLogo);
            this.pnlLoginCard.Controls.Add(this.pnlBordeClave);
            this.pnlLoginCard.Controls.Add(this.pnlBordeUsuario);
            this.pnlLoginCard.Controls.Add(this.lblSubtitulo);
            this.pnlLoginCard.Controls.Add(this.lblTitulo);
            this.pnlLoginCard.Controls.Add(this.btnCancelar);
            this.pnlLoginCard.Controls.Add(this.btnIngresar);
            this.pnlLoginCard.Controls.Add(this.lblClave);
            this.pnlLoginCard.Controls.Add(this.lblUsuario);
            this.pnlLoginCard.Controls.Add(this.txtClave);
            this.pnlLoginCard.Controls.Add(this.txtUsuario);
            this.pnlLoginCard.Location = new System.Drawing.Point(40, 40);
            this.pnlLoginCard.Name = "pnlLoginCard";
            this.pnlLoginCard.Size = new System.Drawing.Size(350, 420);
            this.pnlLoginCard.TabIndex = 6;
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI Symbol", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(117)))), ((int)(((byte)(188)))));
            this.lblLogo.Location = new System.Drawing.Point(50, 25);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(52, 45);
            this.lblLogo.TabIndex = 9;
            this.lblLogo.Text = "🐾";
            // 
            // pnlBordeClave
            // 
            this.pnlBordeClave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlBordeClave.Location = new System.Drawing.Point(30, 260);
            this.pnlBordeClave.Name = "pnlBordeClave";
            this.pnlBordeClave.Size = new System.Drawing.Size(290, 1);
            this.pnlBordeClave.TabIndex = 8;
            // 
            // pnlBordeUsuario
            // 
            this.pnlBordeUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlBordeUsuario.Location = new System.Drawing.Point(30, 180);
            this.pnlBordeUsuario.Name = "pnlBordeUsuario";
            this.pnlBordeUsuario.Size = new System.Drawing.Size(290, 1);
            this.pnlBordeUsuario.TabIndex = 7;
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitulo.ForeColor = System.Drawing.Color.DimGray;
            this.lblSubtitulo.Location = new System.Drawing.Point(50, 75);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(252, 19);
            this.lblSubtitulo.TabIndex = 6;
            this.lblSubtitulo.Text = "Software de Gestión Integral Veterinaria";
            this.lblSubtitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Frm_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(241)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(430, 500);
            this.Controls.Add(this.pnlLoginCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Iniciar Sesión - ADCIVET";
            this.pnlLoginCard.ResumeLayout(false);
            this.pnlLoginCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtClave;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblClave;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlLoginCard;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Panel pnlBordeClave;
        private System.Windows.Forms.Panel pnlBordeUsuario;
        private System.Windows.Forms.Label lblLogo;
    }
}