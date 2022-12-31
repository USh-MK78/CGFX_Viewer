
namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.VertexAttribute
{
    partial class VertexAttributeEditorForm
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
            this.VertexAttribute_ListBox = new System.Windows.Forms.ListBox();
            this.VertexAttributeEditor_TabMain = new System.Windows.Forms.TabControl();
            this.tabPage_Param = new System.Windows.Forms.TabPage();
            this.VertexAttributeEditor_PG_Param = new System.Windows.Forms.PropertyGrid();
            this.tabPage_Stream = new System.Windows.Forms.TabPage();
            this.VertexAttributeEditor_PG_Stream = new System.Windows.Forms.PropertyGrid();
            this.VertexAttributeEditor_SplitMain = new System.Windows.Forms.SplitContainer();
            this.VertexAttributeEditor_SplitSub = new System.Windows.Forms.SplitContainer();
            this.VertexAttribute_AddBtn = new System.Windows.Forms.Button();
            this.VertexAttribute_DownItemBtn = new System.Windows.Forms.Button();
            this.VertexAttribute_DeleteBtn = new System.Windows.Forms.Button();
            this.VertexAttribute_UpItemBtn = new System.Windows.Forms.Button();
            this.tabPage_Main = new System.Windows.Forms.TabPage();
            this.VertexAttributeEditor_PG_Main = new System.Windows.Forms.PropertyGrid();
            this.VertexAttributeEditor_TabMain.SuspendLayout();
            this.tabPage_Param.SuspendLayout();
            this.tabPage_Stream.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VertexAttributeEditor_SplitMain)).BeginInit();
            this.VertexAttributeEditor_SplitMain.Panel1.SuspendLayout();
            this.VertexAttributeEditor_SplitMain.Panel2.SuspendLayout();
            this.VertexAttributeEditor_SplitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VertexAttributeEditor_SplitSub)).BeginInit();
            this.VertexAttributeEditor_SplitSub.Panel1.SuspendLayout();
            this.VertexAttributeEditor_SplitSub.Panel2.SuspendLayout();
            this.VertexAttributeEditor_SplitSub.SuspendLayout();
            this.tabPage_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // VertexAttribute_ListBox
            // 
            this.VertexAttribute_ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexAttribute_ListBox.FormattingEnabled = true;
            this.VertexAttribute_ListBox.ItemHeight = 12;
            this.VertexAttribute_ListBox.Location = new System.Drawing.Point(0, 0);
            this.VertexAttribute_ListBox.Name = "VertexAttribute_ListBox";
            this.VertexAttribute_ListBox.Size = new System.Drawing.Size(190, 405);
            this.VertexAttribute_ListBox.TabIndex = 0;
            this.VertexAttribute_ListBox.SelectedIndexChanged += new System.EventHandler(this.VertexAttributeListBox_SelectedIndexChanged);
            // 
            // VertexAttributeEditor_TabMain
            // 
            this.VertexAttributeEditor_TabMain.Controls.Add(this.tabPage_Main);
            this.VertexAttributeEditor_TabMain.Controls.Add(this.tabPage_Param);
            this.VertexAttributeEditor_TabMain.Controls.Add(this.tabPage_Stream);
            this.VertexAttributeEditor_TabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexAttributeEditor_TabMain.Location = new System.Drawing.Point(0, 0);
            this.VertexAttributeEditor_TabMain.Name = "VertexAttributeEditor_TabMain";
            this.VertexAttributeEditor_TabMain.SelectedIndex = 0;
            this.VertexAttributeEditor_TabMain.Size = new System.Drawing.Size(480, 450);
            this.VertexAttributeEditor_TabMain.TabIndex = 1;
            // 
            // tabPage_Param
            // 
            this.tabPage_Param.Controls.Add(this.VertexAttributeEditor_PG_Param);
            this.tabPage_Param.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Param.Name = "tabPage_Param";
            this.tabPage_Param.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Param.Size = new System.Drawing.Size(472, 424);
            this.tabPage_Param.TabIndex = 0;
            this.tabPage_Param.Text = "Param";
            this.tabPage_Param.UseVisualStyleBackColor = true;
            // 
            // VertexAttributeEditor_PG_Param
            // 
            this.VertexAttributeEditor_PG_Param.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexAttributeEditor_PG_Param.Location = new System.Drawing.Point(3, 3);
            this.VertexAttributeEditor_PG_Param.Name = "VertexAttributeEditor_PG_Param";
            this.VertexAttributeEditor_PG_Param.Size = new System.Drawing.Size(466, 418);
            this.VertexAttributeEditor_PG_Param.TabIndex = 0;
            // 
            // tabPage_Stream
            // 
            this.tabPage_Stream.Controls.Add(this.VertexAttributeEditor_PG_Stream);
            this.tabPage_Stream.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Stream.Name = "tabPage_Stream";
            this.tabPage_Stream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Stream.Size = new System.Drawing.Size(472, 424);
            this.tabPage_Stream.TabIndex = 1;
            this.tabPage_Stream.Text = "Stream";
            this.tabPage_Stream.UseVisualStyleBackColor = true;
            // 
            // VertexAttributeEditor_PG_Stream
            // 
            this.VertexAttributeEditor_PG_Stream.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexAttributeEditor_PG_Stream.Location = new System.Drawing.Point(3, 3);
            this.VertexAttributeEditor_PG_Stream.Name = "VertexAttributeEditor_PG_Stream";
            this.VertexAttributeEditor_PG_Stream.Size = new System.Drawing.Size(466, 418);
            this.VertexAttributeEditor_PG_Stream.TabIndex = 0;
            // 
            // VertexAttributeEditor_SplitMain
            // 
            this.VertexAttributeEditor_SplitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexAttributeEditor_SplitMain.Location = new System.Drawing.Point(0, 0);
            this.VertexAttributeEditor_SplitMain.Name = "VertexAttributeEditor_SplitMain";
            // 
            // VertexAttributeEditor_SplitMain.Panel1
            // 
            this.VertexAttributeEditor_SplitMain.Panel1.Controls.Add(this.VertexAttributeEditor_SplitSub);
            // 
            // VertexAttributeEditor_SplitMain.Panel2
            // 
            this.VertexAttributeEditor_SplitMain.Panel2.Controls.Add(this.VertexAttributeEditor_TabMain);
            this.VertexAttributeEditor_SplitMain.Size = new System.Drawing.Size(674, 450);
            this.VertexAttributeEditor_SplitMain.SplitterDistance = 190;
            this.VertexAttributeEditor_SplitMain.TabIndex = 2;
            // 
            // VertexAttributeEditor_SplitSub
            // 
            this.VertexAttributeEditor_SplitSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexAttributeEditor_SplitSub.Location = new System.Drawing.Point(0, 0);
            this.VertexAttributeEditor_SplitSub.Name = "VertexAttributeEditor_SplitSub";
            this.VertexAttributeEditor_SplitSub.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // VertexAttributeEditor_SplitSub.Panel1
            // 
            this.VertexAttributeEditor_SplitSub.Panel1.Controls.Add(this.VertexAttribute_ListBox);
            // 
            // VertexAttributeEditor_SplitSub.Panel2
            // 
            this.VertexAttributeEditor_SplitSub.Panel2.Controls.Add(this.VertexAttribute_AddBtn);
            this.VertexAttributeEditor_SplitSub.Panel2.Controls.Add(this.VertexAttribute_DownItemBtn);
            this.VertexAttributeEditor_SplitSub.Panel2.Controls.Add(this.VertexAttribute_DeleteBtn);
            this.VertexAttributeEditor_SplitSub.Panel2.Controls.Add(this.VertexAttribute_UpItemBtn);
            this.VertexAttributeEditor_SplitSub.Size = new System.Drawing.Size(190, 450);
            this.VertexAttributeEditor_SplitSub.SplitterDistance = 405;
            this.VertexAttributeEditor_SplitSub.TabIndex = 5;
            // 
            // VertexAttribute_AddBtn
            // 
            this.VertexAttribute_AddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VertexAttribute_AddBtn.Location = new System.Drawing.Point(8, 9);
            this.VertexAttribute_AddBtn.Name = "VertexAttribute_AddBtn";
            this.VertexAttribute_AddBtn.Size = new System.Drawing.Size(52, 23);
            this.VertexAttribute_AddBtn.TabIndex = 1;
            this.VertexAttribute_AddBtn.Text = "Add";
            this.VertexAttribute_AddBtn.UseVisualStyleBackColor = true;
            // 
            // VertexAttribute_DownItemBtn
            // 
            this.VertexAttribute_DownItemBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VertexAttribute_DownItemBtn.Location = new System.Drawing.Point(157, 8);
            this.VertexAttribute_DownItemBtn.Name = "VertexAttribute_DownItemBtn";
            this.VertexAttribute_DownItemBtn.Size = new System.Drawing.Size(27, 23);
            this.VertexAttribute_DownItemBtn.TabIndex = 4;
            this.VertexAttribute_DownItemBtn.Text = "↓";
            this.VertexAttribute_DownItemBtn.UseVisualStyleBackColor = true;
            // 
            // VertexAttribute_DeleteBtn
            // 
            this.VertexAttribute_DeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VertexAttribute_DeleteBtn.Location = new System.Drawing.Point(66, 8);
            this.VertexAttribute_DeleteBtn.Name = "VertexAttribute_DeleteBtn";
            this.VertexAttribute_DeleteBtn.Size = new System.Drawing.Size(55, 23);
            this.VertexAttribute_DeleteBtn.TabIndex = 2;
            this.VertexAttribute_DeleteBtn.Text = "Delete";
            this.VertexAttribute_DeleteBtn.UseVisualStyleBackColor = true;
            // 
            // VertexAttribute_UpItemBtn
            // 
            this.VertexAttribute_UpItemBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VertexAttribute_UpItemBtn.Location = new System.Drawing.Point(124, 8);
            this.VertexAttribute_UpItemBtn.Name = "VertexAttribute_UpItemBtn";
            this.VertexAttribute_UpItemBtn.Size = new System.Drawing.Size(27, 23);
            this.VertexAttribute_UpItemBtn.TabIndex = 3;
            this.VertexAttribute_UpItemBtn.Text = "↑";
            this.VertexAttribute_UpItemBtn.UseVisualStyleBackColor = true;
            // 
            // tabPage_Main
            // 
            this.tabPage_Main.Controls.Add(this.VertexAttributeEditor_PG_Main);
            this.tabPage_Main.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Main.Name = "tabPage_Main";
            this.tabPage_Main.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Main.Size = new System.Drawing.Size(472, 424);
            this.tabPage_Main.TabIndex = 2;
            this.tabPage_Main.Text = "Main";
            this.tabPage_Main.UseVisualStyleBackColor = true;
            // 
            // VertexAttributeEditor_PG_Main
            // 
            this.VertexAttributeEditor_PG_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VertexAttributeEditor_PG_Main.Location = new System.Drawing.Point(3, 3);
            this.VertexAttributeEditor_PG_Main.Name = "VertexAttributeEditor_PG_Main";
            this.VertexAttributeEditor_PG_Main.Size = new System.Drawing.Size(466, 418);
            this.VertexAttributeEditor_PG_Main.TabIndex = 0;
            // 
            // VertexAttributeEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 450);
            this.Controls.Add(this.VertexAttributeEditor_SplitMain);
            this.Name = "VertexAttributeEditorForm";
            this.Text = "VertexAttributeEditor";
            this.Load += new System.EventHandler(this.VertexAttributeEditor_Load);
            this.VertexAttributeEditor_TabMain.ResumeLayout(false);
            this.tabPage_Param.ResumeLayout(false);
            this.tabPage_Stream.ResumeLayout(false);
            this.VertexAttributeEditor_SplitMain.Panel1.ResumeLayout(false);
            this.VertexAttributeEditor_SplitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VertexAttributeEditor_SplitMain)).EndInit();
            this.VertexAttributeEditor_SplitMain.ResumeLayout(false);
            this.VertexAttributeEditor_SplitSub.Panel1.ResumeLayout(false);
            this.VertexAttributeEditor_SplitSub.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VertexAttributeEditor_SplitSub)).EndInit();
            this.VertexAttributeEditor_SplitSub.ResumeLayout(false);
            this.tabPage_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox VertexAttribute_ListBox;
        private System.Windows.Forms.TabControl VertexAttributeEditor_TabMain;
        private System.Windows.Forms.TabPage tabPage_Param;
        private System.Windows.Forms.TabPage tabPage_Stream;
        private System.Windows.Forms.SplitContainer VertexAttributeEditor_SplitMain;
        private System.Windows.Forms.Button VertexAttribute_DownItemBtn;
        private System.Windows.Forms.Button VertexAttribute_UpItemBtn;
        private System.Windows.Forms.Button VertexAttribute_DeleteBtn;
        private System.Windows.Forms.Button VertexAttribute_AddBtn;
        private System.Windows.Forms.PropertyGrid VertexAttributeEditor_PG_Param;
        private System.Windows.Forms.PropertyGrid VertexAttributeEditor_PG_Stream;
        private System.Windows.Forms.SplitContainer VertexAttributeEditor_SplitSub;
        private System.Windows.Forms.TabPage tabPage_Main;
        private System.Windows.Forms.PropertyGrid VertexAttributeEditor_PG_Main;
    }
}