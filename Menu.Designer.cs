namespace DoorDecCreator
{
    partial class Menu
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
            this.label1 = new System.Windows.Forms.Label();
            this.SingleFormButton = new System.Windows.Forms.Button();
            this.MultiDoorDecButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome to Jackson\'s Door Dec Creator!";
            // 
            // SingleFormButton
            // 
            this.SingleFormButton.Location = new System.Drawing.Point(16, 61);
            this.SingleFormButton.Name = "SingleFormButton";
            this.SingleFormButton.Size = new System.Drawing.Size(104, 23);
            this.SingleFormButton.TabIndex = 1;
            this.SingleFormButton.Text = "Single Door Dec";
            this.SingleFormButton.UseVisualStyleBackColor = true;
            this.SingleFormButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // MultiDoorDecButton
            // 
            this.MultiDoorDecButton.Location = new System.Drawing.Point(217, 61);
            this.MultiDoorDecButton.Name = "MultiDoorDecButton";
            this.MultiDoorDecButton.Size = new System.Drawing.Size(104, 23);
            this.MultiDoorDecButton.TabIndex = 2;
            this.MultiDoorDecButton.Text = "Mutiple Door Decs";
            this.MultiDoorDecButton.UseVisualStyleBackColor = true;
            this.MultiDoorDecButton.Click += new System.EventHandler(this.MultiDoorDecButton_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 101);
            this.Controls.Add(this.MultiDoorDecButton);
            this.Controls.Add(this.SingleFormButton);
            this.Controls.Add(this.label1);
            this.Name = "Menu";
            this.Text = "Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SingleFormButton;
        private System.Windows.Forms.Button MultiDoorDecButton;
    }
}