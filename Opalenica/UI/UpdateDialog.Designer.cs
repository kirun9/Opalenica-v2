namespace Opalenica.UI;

partial class UpdateDialog
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
            this.ContinueButton = new System.Windows.Forms.Button();
            this.myProgressBar2 = new Opalenica.UI.MyProgressBar();
            this.myProgressBar1 = new Opalenica.UI.MyProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ContinueButton
            // 
            this.ContinueButton.DialogResult = System.Windows.Forms.DialogResult.Continue;
            this.ContinueButton.Enabled = false;
            this.ContinueButton.FlatAppearance.BorderSize = 2;
            this.ContinueButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ContinueButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ContinueButton.Location = new System.Drawing.Point(290, 163);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(221, 49);
            this.ContinueButton.TabIndex = 11;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = true;
            // 
            // myProgressBar2
            // 
            this.myProgressBar2.DisplayStyle = Opalenica.UI.ProgressBarDisplayText.CustomText;
            this.myProgressBar2.Location = new System.Drawing.Point(58, 91);
            this.myProgressBar2.Name = "myProgressBar2";
            this.myProgressBar2.Size = new System.Drawing.Size(453, 27);
            this.myProgressBar2.TabIndex = 10;
            this.myProgressBar2.Text = "myProgressBar2";
            this.myProgressBar2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.myProgressBar2.TextColor = System.Drawing.Color.Black;
            // 
            // myProgressBar1
            // 
            this.myProgressBar1.DisplayStyle = Opalenica.UI.ProgressBarDisplayText.Percentage;
            this.myProgressBar1.Location = new System.Drawing.Point(58, 58);
            this.myProgressBar1.Name = "myProgressBar1";
            this.myProgressBar1.Size = new System.Drawing.Size(453, 27);
            this.myProgressBar1.TabIndex = 9;
            this.myProgressBar1.Text = "myProgressBar1";
            this.myProgressBar1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.myProgressBar1.TextColor = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 0);
            this.label1.Size = new System.Drawing.Size(569, 49);
            this.label1.TabIndex = 8;
            this.label1.Text = "Downloading update";
            // 
            // UpdateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(569, 218);
            this.ControlBox = false;
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.myProgressBar2);
            this.Controls.Add(this.myProgressBar1);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UpdateDialog";
            this.ResumeLayout(false);

    }

    #endregion

    private Button ContinueButton;
    private Opalenica.UI.MyProgressBar myProgressBar2;
    private Opalenica.UI.MyProgressBar myProgressBar1;
    private Label label1;
}