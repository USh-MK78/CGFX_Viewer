using CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.PrimitiveSet.Primitive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.VertexAttribute
{
    public partial class VertexAttributeEditorForm : Form
    {
        public List<CGFXFormat.SOBJ.Shape.VertexAttribute> vertexAttribute_List { get; set; }

        public VertexAttributeEditorForm(List<CGFXFormat.SOBJ.Shape.VertexAttribute> vertexAttributes)
        {
            InitializeComponent();
            vertexAttribute_List = vertexAttributes;
        }

        Form1 Form;

        private void VertexAttributeEditor_Load(object sender, EventArgs e)
        {
            Form = (Form1)Application.OpenForms["Form1"];

            VertexAttributeEditor_SplitSub.FixedPanel = FixedPanel.Panel2;
            VertexAttributeEditor_SplitSub.IsSplitterFixed = true;

            if (vertexAttribute_List.Count != 0)
            {
                List<string> UDList = new List<string>();

                for (int i = 0; i < vertexAttribute_List.Count; i++)
                {
                    UDList.Add(i + " : " + vertexAttribute_List[i].ToString());
                }

                VertexAttribute_ListBox.Items.AddRange(UDList.ToArray());

                VertexAttribute_ListBox.SelectedIndex = 0;
            }
        }

        private void VertexAttributeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VertexAttribute_ListBox.SelectedIndex == -1) return;
            if (VertexAttribute_ListBox.SelectedIndex != -1)
            {
                VertexAttributeEditor_PG_Main.SelectedObject = new VertexAttribute_PropertyGrid(vertexAttribute_List[VertexAttribute_ListBox.SelectedIndex]);

                if (vertexAttribute_List[VertexAttribute_ListBox.SelectedIndex].Params != new CGFXFormat.SOBJ.Shape.VertexAttribute.Param())
                {
                    VertexAttributeEditor_PG_Param.SelectedObject = new Param.Param_PropertyGrid(vertexAttribute_List[VertexAttribute_ListBox.SelectedIndex].Params);
                }
                if (vertexAttribute_List[VertexAttribute_ListBox.SelectedIndex].Streams != new CGFXFormat.SOBJ.Shape.VertexAttribute.Stream())
                {
                    VertexAttributeEditor_PG_Stream.SelectedObject = new Stream.Stream_PropertyGrid(vertexAttribute_List[VertexAttribute_ListBox.SelectedIndex].Streams);
                    //VertexAttributeEditor_TabMain
                }
            }
        }
    }
}
