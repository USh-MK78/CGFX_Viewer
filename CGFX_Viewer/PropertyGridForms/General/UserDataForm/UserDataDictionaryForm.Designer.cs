
namespace CGFX_Viewer.Forms.General.UserDataForm
{
	partial class UserDataDictionaryForm
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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.AddUserData_Btn = new System.Windows.Forms.Button();
			this.DeleteUserData_Btn = new System.Windows.Forms.Button();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(6, 6);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(120, 280);
			this.listBox1.TabIndex = 0;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// AddUserData_Btn
			// 
			this.AddUserData_Btn.Location = new System.Drawing.Point(6, 291);
			this.AddUserData_Btn.Name = "AddUserData_Btn";
			this.AddUserData_Btn.Size = new System.Drawing.Size(47, 23);
			this.AddUserData_Btn.TabIndex = 1;
			this.AddUserData_Btn.Text = "Add";
			this.AddUserData_Btn.UseVisualStyleBackColor = true;
			this.AddUserData_Btn.Click += new System.EventHandler(this.AddUserData_Btn_Click);
			// 
			// DeleteUserData_Btn
			// 
			this.DeleteUserData_Btn.Location = new System.Drawing.Point(79, 291);
			this.DeleteUserData_Btn.Name = "DeleteUserData_Btn";
			this.DeleteUserData_Btn.Size = new System.Drawing.Size(47, 23);
			this.DeleteUserData_Btn.TabIndex = 2;
			this.DeleteUserData_Btn.Text = "Delete";
			this.DeleteUserData_Btn.UseVisualStyleBackColor = true;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Location = new System.Drawing.Point(132, 6);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(270, 308);
			this.propertyGrid1.TabIndex = 3;
			// 
			// UserDataDictionaryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(407, 318);
			this.Controls.Add(this.propertyGrid1);
			this.Controls.Add(this.DeleteUserData_Btn);
			this.Controls.Add(this.AddUserData_Btn);
			this.Controls.Add(this.listBox1);
			this.Name = "UserDataDictionaryForm";
			this.Text = "UserDataDictionaryForm";
			this.Load += new System.EventHandler(this.UserDataDictionaryForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button AddUserData_Btn;
		private System.Windows.Forms.Button DeleteUserData_Btn;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
	}
}