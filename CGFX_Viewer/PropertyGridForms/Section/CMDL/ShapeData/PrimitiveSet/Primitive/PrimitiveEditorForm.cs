using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.PrimitiveSet.Primitive
{
    public partial class PrimitiveEditorForm : Form
    {
        public List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive> primitive_List { get; set; }

        public PrimitiveEditorForm(List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive> primitives)
        {
            InitializeComponent();
            primitive_List = primitives;
        }

        Form1 Form;

        private void PrimitiveEditor_Load(object sender, EventArgs e)
        {
            Form = (Form1)Application.OpenForms["Form1"];

            PrimitiveEditor_SplitSub.FixedPanel = FixedPanel.Panel2;
            PrimitiveEditor_SplitSub.IsSplitterFixed = true;

            if (primitive_List.Count != 0)
            {
                List<string> UDList = new List<string>();

                for (int i = 0; i < primitive_List.Count; i++)
                {
                    UDList.Add(i + " : " + primitive_List[i].ToString());
                }

                Primitive_ListBox.Items.AddRange(UDList.ToArray());

                Primitive_ListBox.SelectedIndex = 0;
            }
        }

        private void Primitive_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Primitive_ListBox.SelectedIndex == -1) return;
            Primitive_PG_Main.SelectedObject = new Primitive_PropertyGrid(primitive_List[Primitive_ListBox.SelectedIndex]);
        }


    }
}
