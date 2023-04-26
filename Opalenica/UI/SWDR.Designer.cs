namespace Opalenica.UI;

partial class SWDRForm
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
            this.myProgressBar1 = new Opalenica.UI.MyProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // myProgressBar1
            // 
            this.myProgressBar1.BackColor = System.Drawing.SystemColors.Control;
            this.myProgressBar1.DisplayStyle = Opalenica.UI.ProgressBarDisplayText.Percentage;
            this.myProgressBar1.Location = new System.Drawing.Point(303, 363);
            this.myProgressBar1.Name = "myProgressBar1";
            this.myProgressBar1.Size = new System.Drawing.Size(403, 48);
            this.myProgressBar1.TabIndex = 0;
            this.myProgressBar1.Text = "Progress";
            this.myProgressBar1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.myProgressBar1.TextColor = System.Drawing.Color.Black;
            this.myProgressBar1.Value = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(303, 318);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(403, 42);
            this.label1.TabIndex = 1;
            this.label1.Text = "Progres prac nad SWDR";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SWDRForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.myProgressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SWDRForm";
            this.Text = "SWDR";
            this.Load += new System.EventHandler(this.SWDRForm_Load);
            this.ResumeLayout(false);

    }

    #endregion

    private MyProgressBar myProgressBar1;
    private Label label1;
}