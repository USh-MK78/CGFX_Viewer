using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.VertexAttribute.Stream.VertexStream
{
    public partial class VertexStreamEditorForm : Form
    {
        public List<CGFXFormat.SOBJ.Shape.VertexAttribute.Stream.VertexStream> vertexStream_list { get; set; }

        public VertexStreamEditorForm(List<CGFXFormat.SOBJ.Shape.VertexAttribute.Stream.VertexStream> vertexStreams)
        {
            InitializeComponent();
            vertexStream_list = vertexStreams;
        }

        Form1 Form;

        private void VertexStreamForm_Load(object sender, EventArgs e)
        {
            Form = (Form1)Application.OpenForms["Form1"];

            VertexStreamEditor_SplitSub.FixedPanel = FixedPanel.Panel2;
            VertexStreamEditor_SplitSub.IsSplitterFixed = true;

            if (vertexStream_list.Count != 0)
            {
                List<string> UDList = new List<string>();

                for (int i = 0; i < vertexStream_list.Count; i++)
                {
                    UDList.Add(i + " : " + vertexStream_list[i].ToString());
                }

                VertexStream_ListBox.Items.AddRange(UDList.ToArray());

                VertexStream_ListBox.SelectedIndex = 0;
            }
        }

        private void VertexStream_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            VertexStream_PG_Main.SelectedObject = new VertexStream_PropertyGrid(vertexStream_list[VertexStream_ListBox.SelectedIndex]);
        }
    }
}
