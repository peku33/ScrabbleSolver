namespace ScrabbleSolver.View
{
	partial class ChooseCharacter
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
			this.button1 = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.tileReplaceLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.button1.Location = new System.Drawing.Point(129, 44);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(228, 117);
			this.button1.TabIndex = 5;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.richTextBox1.ForeColor = System.Drawing.Color.Red;
			this.richTextBox1.Location = new System.Drawing.Point(12, 44);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.richTextBox1.Size = new System.Drawing.Size(92, 117);
			this.richTextBox1.TabIndex = 4;
			this.richTextBox1.Text = "";
			this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged_1);
			// 
			// tileReplaceLabel
			// 
			this.tileReplaceLabel.AutoSize = true;
			this.tileReplaceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.tileReplaceLabel.Location = new System.Drawing.Point(109, 9);
			this.tileReplaceLabel.Name = "tileReplaceLabel";
			this.tileReplaceLabel.Size = new System.Drawing.Size(158, 31);
			this.tileReplaceLabel.TabIndex = 3;
			this.tileReplaceLabel.Text = "Select letter";
			this.tileReplaceLabel.Click += new System.EventHandler(this.tileReplaceLabel_Click);
			// 
			// ChooseCharacter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(374, 170);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.tileReplaceLabel);
			this.Name = "ChooseCharacter";
			this.Text = "ChooseCharacter";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Label tileReplaceLabel;
	}
}