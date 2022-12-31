
namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.PrimitiveSet.Primitive
{
    partial class PrimitiveEditorForm
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
            this.Primitive_ListBox = new System.Windows.Forms.ListBox();
            this.Primitive_PG_Main = new System.Windows.Forms.PropertyGrid();
            this.DeletePrimitive_Btn = new System.Windows.Forms.Button();
            this.AddPrimitive_Btn = new System.Windows.Forms.Button();
            this.PrimitiveEditor_SplitMain = new System.Windows.Forms.SplitContainer();
            this.PrimitiveEditor_SplitSub = new System.Windows.Forms.SplitContainer();
            this.Primitive_ListDown_Btn = new System.Windows.Forms.Button();
            this.Primitive_ListUp_Btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PrimitiveEditor_SplitMain)).BeginInit();
            this.PrimitiveEditor_SplitMain.Panel1.SuspendLayout();
            this.PrimitiveEditor_SplitMain.Panel2.SuspendLayout();
            this.PrimitiveEditor_SplitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PrimitiveEditor_SplitSub)).BeginInit();
            this.PrimitiveEditor_SplitSub.Panel1.SuspendLayout();
            this.PrimitiveEditor_SplitSub.Panel2.SuspendLayout();
            this.PrimitiveEditor_SplitSub.SuspendLayout();
            this.SuspendLayout();
            // 
            // Primitive_ListBox
            // 
            this.Primitive_ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Primitive_ListBox.FormattingEnabled = true;
            this.Primitive_ListBox.ItemHeight = 12;
            this.Primitive_ListBox.Location = new System.Drawing.Point(0, 0);
            this.Primitive_ListBox.Name = "Primitive_ListBox";
            this.Primitive_ListBox.Size = new System.Drawing.Size(173, 396);
            this.Primitive_ListBox.TabIndex = 4;
            this.Primitive_ListBox.SelectedIndexChanged += new System.EventHandler(this.Primitive_ListBox_SelectedIndexChanged);
            // 
            // Primitive_PG_Main
            // 
            this.Primitive_PG_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Primitive_PG_Main.Location = new System.Drawing.Point(0, 0);
            this.Primitive_PG_Main.Name = "Primitive_PG_Main";
            this.Primitive_PG_Main.Size = new System.Drawing.Size(352, 434);
            this.Primitive_PG_Main.TabIndex = 7;
            // 
            // DeletePrimitive_Btn
            // 
            this.DeletePrimitive_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeletePrimitive_Btn.Location = new System.Drawing.Point(56, 5);
            this.DeletePrimitive_Btn.Name = "DeletePrimitive_Btn";
            this.DeletePrimitive_Btn.Size = new System.Drawing.Size(47, 23);
            this.DeletePrimitive_Btn.TabIndex = 6;
            this.DeletePrimitive_Btn.Text = "Delete";
            this.DeletePrimitive_Btn.UseVisualStyleBackColor = true;
            // 
            // AddPrimitive_Btn
            // 
            this.AddPrimitive_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddPrimitive_Btn.Location = new System.Drawing.Point(6, 5);
            this.AddPrimitive_Btn.Name = "AddPrimitive_Btn";
            this.AddPrimitive_Btn.Size = new System.Drawing.Size(47, 23);
            this.AddPrimitive_Btn.TabIndex = 5;
            this.AddPrimitive_Btn.Text = "Add";
            this.AddPrimitive_Btn.UseVisualStyleBackColor = true;
            // 
            // PrimitiveEditor_SplitMain
            // 
            this.PrimitiveEditor_SplitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrimitiveEditor_SplitMain.Location = new System.Drawing.Point(0, 0);
            this.PrimitiveEditor_SplitMain.Name = "PrimitiveEditor_SplitMain";
            // 
            // PrimitiveEditor_SplitMain.Panel1
            // 
            this.PrimitiveEditor_SplitMain.Panel1.Controls.Add(this.PrimitiveEditor_SplitSub);
            // 
            // PrimitiveEditor_SplitMain.Panel2
            // 
            this.PrimitiveEditor_SplitMain.Panel2.Controls.Add(this.Primitive_PG_Main);
            this.PrimitiveEditor_SplitMain.Size = new System.Drawing.Size(529, 434);
            this.PrimitiveEditor_SplitMain.SplitterDistance = 173;
            this.PrimitiveEditor_SplitMain.TabIndex = 9;
            // 
            // PrimitiveEditor_SplitSub
            // 
            this.PrimitiveEditor_SplitSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrimitiveEditor_SplitSub.Location = new System.Drawing.Point(0, 0);
            this.PrimitiveEditor_SplitSub.Name = "PrimitiveEditor_SplitSub";
            this.PrimitiveEditor_SplitSub.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // PrimitiveEditor_SplitSub.Panel1
            // 
            this.PrimitiveEditor_SplitSub.Panel1.Controls.Add(this.Primitive_ListBox);
            // 
            // PrimitiveEditor_SplitSub.Panel2
            // 
            this.PrimitiveEditor_SplitSub.Panel2.Controls.Add(this.Primitive_ListDown_Btn);
            this.PrimitiveEditor_SplitSub.Panel2.Controls.Add(this.Primitive_ListUp_Btn);
            this.PrimitiveEditor_SplitSub.Panel2.Controls.Add(this.DeletePrimitive_Btn);
            this.PrimitiveEditor_SplitSub.Panel2.Controls.Add(this.AddPrimitive_Btn);
            this.PrimitiveEditor_SplitSub.Size = new System.Drawing.Size(173, 434);
            this.PrimitiveEditor_SplitSub.SplitterDistance = 396;
            this.PrimitiveEditor_SplitSub.TabIndex = 0;
            // 
            // Primitive_ListDown_Btn
            // 
            this.Primitive_ListDown_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Primitive_ListDown_Btn.Location = new System.Drawing.Point(143, 5);
            this.Primitive_ListDown_Btn.Name = "Primitive_ListDown_Btn";
            this.Primitive_ListDown_Btn.Size = new System.Drawing.Size(23, 23);
            this.Primitive_ListDown_Btn.TabIndex = 8;
            this.Primitive_ListDown_Btn.Text = "-";
            this.Primitive_ListDown_Btn.UseVisualStyleBackColor = true;
            // 
            // Primitive_ListUp_Btn
            // 
            this.Primitive_ListUp_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Primitive_ListUp_Btn.Location = new System.Drawing.Point(114, 5);
            this.Primitive_ListUp_Btn.Name = "Primitive_ListUp_Btn";
            this.Primitive_ListUp_Btn.Size = new System.Drawing.Size(23, 23);
            this.Primitive_ListUp_Btn.TabIndex = 7;
            this.Primitive_ListUp_Btn.Text = "+";
            this.Primitive_ListUp_Btn.UseVisualStyleBackColor = true;
            // 
            // PrimitiveEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 434);
            this.Controls.Add(this.PrimitiveEditor_SplitMain);
            this.Name = "PrimitiveEditor";
            this.Text = "PrimitiveEditor";
            this.Load += new System.EventHandler(this.PrimitiveEditor_Load);
            this.PrimitiveEditor_SplitMain.Panel1.ResumeLayout(false);
            this.PrimitiveEditor_SplitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PrimitiveEditor_SplitMain)).EndInit();
            this.PrimitiveEditor_SplitMain.ResumeLayout(false);
            this.PrimitiveEditor_SplitSub.Panel1.ResumeLayout(false);
            this.PrimitiveEditor_SplitSub.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PrimitiveEditor_SplitSub)).EndInit();
            this.PrimitiveEditor_SplitSub.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Primitive_ListBox;
        private System.Windows.Forms.PropertyGrid Primitive_PG_Main;
        private System.Windows.Forms.Button DeletePrimitive_Btn;
        private System.Windows.Forms.Button AddPrimitive_Btn;
        private System.Windows.Forms.SplitContainer PrimitiveEditor_SplitMain;
        private System.Windows.Forms.SplitContainer PrimitiveEditor_SplitSub;
        private System.Windows.Forms.Button Primitive_ListDown_Btn;
        private System.Windows.Forms.Button Primitive_ListUp_Btn;
    }
}