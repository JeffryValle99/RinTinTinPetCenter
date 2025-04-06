namespace ProyectoVeterinaria
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnfrmControl = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnfrmControl
            // 
            this.btnfrmControl.Location = new System.Drawing.Point(145, 70);
            this.btnfrmControl.Name = "btnfrmControl";
            this.btnfrmControl.Size = new System.Drawing.Size(189, 40);
            this.btnfrmControl.TabIndex = 10;
            this.btnfrmControl.Text = "Abrir";
            this.btnfrmControl.UseVisualStyleBackColor = true;
            this.btnfrmControl.Click += new System.EventHandler(this.btnfrmControl_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(484, 212);
            this.Controls.Add(this.btnfrmControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnfrmControl;
    }
}

