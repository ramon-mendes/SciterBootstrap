﻿namespace SciterBootstrap
{
	partial class Form1
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
			if(disposing && (components != null))
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
			this.sciterControl1 = new SciterSharp.WinForms.SciterControl();
			this.SuspendLayout();
			// 
			// sciterControl1
			// 
			this.sciterControl1.Location = new System.Drawing.Point(86, 76);
			this.sciterControl1.Name = "sciterControl1";
			this.sciterControl1.Size = new System.Drawing.Size(373, 198);
			this.sciterControl1.TabIndex = 0;
			this.sciterControl1.Text = "sciterControl1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(544, 363);
			this.Controls.Add(this.sciterControl1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private SciterSharp.WinForms.SciterControl sciterControl1;
	}
}

