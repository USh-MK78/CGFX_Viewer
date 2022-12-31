using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.PrimitiveSet
{
    public partial class PrimitiveSetEditorForm : Form
    {
        public List<CGFXFormat.SOBJ.Shape.PrimitiveSet> primitiveSet_List { get; set; }

        public PrimitiveSetEditorForm(List<CGFXFormat.SOBJ.Shape.PrimitiveSet> primitiveSets)
        {
            InitializeComponent();
            primitiveSet_List = primitiveSets;
        }

        Form1 Form;

        private void PrimitiveSetEditor_Load(object sender, EventArgs e)
        {
            Form = (Form1)Application.OpenForms["Form1"];

            PrimitiveSetEditor_SplitSub.FixedPanel = FixedPanel.Panel2;
            PrimitiveSetEditor_SplitSub.IsSplitterFixed = true;

            if (primitiveSet_List.Count != 0)
            {
                List<string> UDList = new List<string>();

                for (int i = 0; i < primitiveSet_List.Count; i++)
                {
                    UDList.Add(i + " : " + primitiveSet_List[i].ToString());
                }

                PrimitiveSet_ListBox.Items.AddRange(UDList.ToArray());

                PrimitiveSet_ListBox.SelectedIndex = 0;
            }
        }

        private void AddPrimitiveSet_Btn_Click(object sender, EventArgs e)
        {

        }

        private void DeletePrimitiveSet_Btn_Click(object sender, EventArgs e)
        {

        }

        private void PrimitiveSet_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PrimitiveSet_ListBox.SelectedIndex == -1) return;
            PrimitiveSet_PG_Main.SelectedObject = new PrimitiveSet_PropertyGrid(primitiveSet_List[PrimitiveSet_ListBox.SelectedIndex]);
        }
    }
}
