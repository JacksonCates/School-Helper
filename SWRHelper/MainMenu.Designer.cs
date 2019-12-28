namespace SWRHelper
{
    partial class MainMenu
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
            this.MPButton = new System.Windows.Forms.Button();
            this.SPButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MPButton
            // 
            this.MPButton.Location = new System.Drawing.Point(12, 12);
            this.MPButton.Name = "MPButton";
            this.MPButton.Size = new System.Drawing.Size(75, 23);
            this.MPButton.TabIndex = 0;
            this.MPButton.Text = "MultiPlayer";
            this.MPButton.UseVisualStyleBackColor = true;
            this.MPButton.Click += new System.EventHandler(this.MPButton_Click);
            // 
            // SPButton
            // 
            this.SPButton.Location = new System.Drawing.Point(276, 13);
            this.SPButton.Name = "SPButton";
            this.SPButton.Size = new System.Drawing.Size(75, 23);
            this.SPButton.TabIndex = 1;
            this.SPButton.Text = "SinglePlayer";
            this.SPButton.UseVisualStyleBackColor = true;
            this.SPButton.Click += new System.EventHandler(this.SPButton_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 48);
            this.Controls.Add(this.SPButton);
            this.Controls.Add(this.MPButton);
            this.Name = "MainMenu";
            this.Text = "Welcome to Jackson\'s SWR Helper";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MPButton;
        private System.Windows.Forms.Button SPButton;
    }
}

