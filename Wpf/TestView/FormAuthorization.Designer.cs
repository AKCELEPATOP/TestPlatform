using System;

namespace TestView
{
    partial class FormAuthorization
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// 
        public void DisposeForm()
        {
            Dispose(true);
        }

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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sign_In = new System.Windows.Forms.Button();
            this.sign_Up = new System.Windows.Forms.Button();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль";
            // 
            // sign_In
            // 
            this.sign_In.AutoSize = true;
            this.sign_In.Location = new System.Drawing.Point(87, 106);
            this.sign_In.Name = "sign_In";
            this.sign_In.Size = new System.Drawing.Size(132, 29);
            this.sign_In.TabIndex = 2;
            this.sign_In.Text = "Войти";
            this.sign_In.UseVisualStyleBackColor = true;
            this.sign_In.Click += new System.EventHandler(this.button1_Click);
            // 
            // sign_Up
            // 
            this.sign_Up.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.sign_Up.Location = new System.Drawing.Point(87, 144);
            this.sign_Up.Name = "sign_Up";
            this.sign_Up.Size = new System.Drawing.Size(132, 29);
            this.sign_Up.TabIndex = 3;
            this.sign_Up.Text = "Регистрация";
            this.sign_Up.UseVisualStyleBackColor = true;
            this.sign_Up.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Location = new System.Drawing.Point(87, 29);
            this.textBoxLogin.MaxLength = 32;
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(167, 20);
            this.textBoxLogin.TabIndex = 4;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(87, 65);
            this.textBoxPassword.MaxLength = 32;
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(167, 20);
            this.textBoxPassword.TabIndex = 5;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // FormAuthorization
            // 
            this.AcceptButton = this.sign_In;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 196);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxLogin);
            this.Controls.Add(this.sign_Up);
            this.Controls.Add(this.sign_In);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormAuthorization";
            this.Padding = new System.Windows.Forms.Padding(20, 60, 20, 20);
            this.Text = "Авторизация";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void textBoxPassword_KeyDown(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
        


        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button sign_In;
        private System.Windows.Forms.Button sign_Up;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.TextBox textBoxPassword;
    }
}
