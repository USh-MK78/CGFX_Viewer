namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.VertexAttribute.Stream.VertexStream
{
    partial class VertexStreamEditorForm
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
            this.VertexStream_PG_Main = new System.Windows.Forms.PropertyGrid();
            this.DeleteVertexStream_Btn = new System.Windows.Forms.Button();
            this.AddVertexStream_Btn = new System.Windows.Forms.Button();
            this.VertexStream_ListBox = new System.Windows.Forms.ListBox();
            this.VertexStreamEditor_SplitMain = new System.Windows.Forms.SplitContainer();
            this.VertexStreamEditor_SplitSub = new System.Windows.Forms.SplitContainer();
            this.VertexStream_ListDown_Btn = new System.Windows.Forms.Button();
            this.VertexStream_ListUp_Btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.VertexStreamEditor_SplitMain)).BeginInit();
            this.VertexStreamEditor_SplitMain.Panel1.SuspendLayout();
            this.VertexStreamEditor_SplitMain.Panel2.SuspendLayout();
            this.VertexStreamEditor_SplitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VertexStreamEditor_SplitSub)).BeginInit();
            this.VertexStreamEditor_SplitSub.Panel1.SuspendLayout();
            this.VertexStreamEditor_SplitSub.Panel2.SuspendLayout();
            this.VertexStreamEditor_SplitSub.SuspendLayout();
            this.SuspendLayout();
            // 
            // VertexStream_PG_Main
            // 
            this.VertexStream_PG_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexStream_PG_Main.Location = new System.Drawing.Point(0, 0);
            this.VertexStream_PG_Main.Name = "VertexStream_PG_Main";
            this.VertexStream_PG_Main.Size = new System.Drawing.Size(341, 416);
            this.VertexStream_PG_Main.TabIndex = 7;
            // 
            // DeleteVertexStream_Btn
            // 
            this.DeleteVertexStream_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteVertexStream_Btn.Location = new System.Drawing.Point(56, 4);
            this.DeleteVertexStream_Btn.Name = "DeleteVertexStream_Btn";
            this.DeleteVertexStream_Btn.Size = new System.Drawing.Size(47, 23);
            this.DeleteVertexStream_Btn.TabIndex = 6;
            this.DeleteVertexStream_Btn.Text = "Delete";
            this.DeleteVertexStream_Btn.UseVisualStyleBackColor = true;
            // 
            // AddVertexStream_Btn
            // 
            this.AddVertexStream_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddVertexStream_Btn.Location = new System.Drawing.Point(6, 4);
            this.AddVertexStream_Btn.Name = "AddVertexStream_Btn";
            this.AddVertexStream_Btn.Size = new System.Drawing.Size(47, 23);
            this.AddVertexStream_Btn.TabIndex = 5;
            this.AddVertexStream_Btn.Text = "Add";
            this.AddVertexStream_Btn.UseVisualStyleBackColor = true;
            // 
            // VertexStream_ListBox
            // 
            this.VertexStream_ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexStream_ListBox.FormattingEnabled = true;
            this.VertexStream_ListBox.ItemHeight = 12;
            this.VertexStream_ListBox.Location = new System.Drawing.Point(0, 0);
            this.VertexStream_ListBox.Name = "VertexStream_ListBox";
            this.VertexStream_ListBox.Size = new System.Drawing.Size(168, 379);
            this.VertexStream_ListBox.TabIndex = 4;
            this.VertexStream_ListBox.SelectedIndexChanged += new System.EventHandler(this.VertexStream_ListBox_SelectedIndexChanged);
            // 
            // VertexStreamEditor_SplitMain
            // 
            this.VertexStreamEditor_SplitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexStreamEditor_SplitMain.Location = new System.Drawing.Point(0, 0);
            this.VertexStreamEditor_SplitMain.Name = "VertexStreamEditor_SplitMain";
            // 
            // VertexStreamEditor_SplitMain.Panel1
            // 
            this.VertexStreamEditor_SplitMain.Panel1.Controls.Add(this.VertexStreamEditor_SplitSub);
            // 
            // VertexStreamEditor_SplitMain.Panel2
            // 
            this.VertexStreamEditor_SplitMain.Panel2.Controls.Add(this.VertexStream_PG_Main);
            this.VertexStreamEditor_SplitMain.Size = new System.Drawing.Size(513, 416);
            this.VertexStreamEditor_SplitMain.SplitterDistance = 168;
            this.VertexStreamEditor_SplitMain.TabIndex = 9;
            // 
            // VertexStreamEditor_SplitSub
            // 
            this.VertexStreamEditor_SplitSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexStreamEditor_SplitSub.Location = new System.Drawing.Point(0, 0);
            this.VertexStreamEditor_SplitSub.Name = "VertexStreamEditor_SplitSub";
            this.VertexStreamEditor_SplitSub.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // VertexStreamEditor_SplitSub.Panel1
            // 
            this.VertexStreamEditor_SplitSub.Panel1.Controls.Add(this.VertexStream_ListBox);
            // 
            // VertexStreamEditor_SplitSub.Panel2
            // 
            this.VertexStreamEditor_SplitSub.Panel2.Controls.Add(this.VertexStream_ListDown_Btn);
            this.VertexStreamEditor_SplitSub.Panel2.Controls.Add(this.VertexStream_ListUp_Btn);
            this.VertexStreamEditor_SplitSub.Panel2.Controls.Add(this.DeleteVertexStream_Btn);
            this.VertexStreamEditor_SplitSub.Panel2.Controls.Add(this.AddVertexStream_Btn);
            this.VertexStreamEditor_SplitSub.Size = new System.Drawing.Size(168, 416);
            this.VertexStreamEditor_SplitSub.SplitterDistance = 379;
            this.VertexStreamEditor_SplitSub.TabIndex = 0;
            // 
            // VertexStream_ListDown_Btn
            // 
            this.VertexStream_ListDown_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VertexStream_ListDown_Btn.Location = new System.Drawing.Point(138, 4);
            this.VertexStream_ListDown_Btn.Name = "VertexStream_ListDown_Btn";
            this.VertexStream_ListDown_Btn.Size = new System.Drawing.Size(23, 23);
            this.VertexStream_ListDown_Btn.TabIndex = 8;
            this.VertexStream_ListDown_Btn.Text = "-";
            this.VertexStream_ListDown_Btn.UseVisualStyleBackColor = true;
            // 
            // VertexStream_ListUp_Btn
            // 
            this.VertexStream_ListUp_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VertexStream_ListUp_Btn.Location = new System.Drawing.Point(109, 4);
            this.VertexStream_ListUp_Btn.Name = "VertexStream_ListUp_Btn";
            this.VertexStream_ListUp_Btn.Size = new System.Drawing.Size(23, 23);
            this.VertexStream_ListUp_Btn.TabIndex = 7;
            this.VertexStream_ListUp_Btn.Text = "+";
            this.VertexStream_ListUp_Btn.UseVisualStyleBackColor = true;
            // 
            // VertexStreamEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 416);
            this.Controls.Add(this.VertexStreamEditor_SplitMain);
            this.Name = "VertexStreamEditorForm";
            this.Text = "VertexStreamEditorForm";
            this.Load += new System.EventHandler(this.VertexStreamForm_Load);
            this.VertexStreamEditor_SplitMain.Panel1.ResumeLayout(false);
            this.VertexStreamEditor_SplitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VertexStreamEditor_SplitMain)).EndInit();
            this.VertexStreamEditor_SplitMain.ResumeLayout(false);
            this.VertexStreamEditor_SplitSub.Panel1.ResumeLayout(false);
            this.VertexStreamEditor_SplitSub.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VertexStreamEditor_SplitSub)).EndInit();
            this.VertexStreamEditor_SplitSub.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid VertexStream_PG_Main;
        private System.Windows.Forms.Button DeleteVertexStream_Btn;
        private System.Windows.Forms.Button AddVertexStream_Btn;
        private System.Windows.Forms.ListBox VertexStream_ListBox;
        private System.Windows.Forms.SplitContainer VertexStreamEditor_SplitMain;
        private System.Windows.Forms.SplitContainer VertexStreamEditor_SplitSub;
        private System.Windows.Forms.Button VertexStream_ListDown_Btn;
        private System.Windows.Forms.Button VertexStream_ListUp_Btn;
    }
}