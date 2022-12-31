using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.PrimitiveSet.Primitive.IndexStreamCtr
{
    public partial class IndexStreamCtrEditorForm : Form
    {
        public List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive.IndexStreamCtr> indexStreamCtr_List { get; set; }

        public IndexStreamCtrEditorForm(List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive.IndexStreamCtr> IndexStreamCtrs)
        {
            InitializeComponent();
            indexStreamCtr_List = IndexStreamCtrs;
        }

        Form1 Form;

        private void IndexStreamCtrEditorForm_Load(object sender, EventArgs e)
        {
            Form = (Form1)Application.OpenForms["Form1"];

            IndexStreamCtrEditor_SplitSub.FixedPanel = FixedPanel.Panel2;
            IndexStreamCtrEditor_SplitSub.IsSplitterFixed = true;

            if (indexStreamCtr_List.Count != 0)
            {
                List<string> UDList = new List<string>();

                for (int i = 0; i < indexStreamCtr_List.Count; i++)
                {
                    UDList.Add(i + " : " + indexStreamCtr_List[i].ToString());
                }

                IndexStreamCtr_ListBox.Items.AddRange(UDList.ToArray());

                IndexStreamCtr_ListBox.SelectedIndex = 0;
            }
        }

        private void IndexStreamCtr_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexStreamCtr_ListBox.SelectedIndex == -1) return;
            IndexStreamCtr_PG_Main.SelectedObject = new IndexStreamCtr_PropertyGrid(indexStreamCtr_List[IndexStreamCtr_ListBox.SelectedIndex]);
        }
    }
}
