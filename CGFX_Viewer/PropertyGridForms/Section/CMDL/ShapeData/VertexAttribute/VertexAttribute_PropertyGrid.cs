using CGFX_Viewer.CGFXPropertyGridSet;
using CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.VertexAttribute.Stream.VertexStream;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.VertexAttribute
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class VertexAttribute_PropertyGrid
    {
        public int VertexAttributeOffset { get; set; }
        public Flags Flag { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public Stream.Stream_PropertyGrid Streams { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public Param.Param_PropertyGrid Params { get; set; }

        public VertexAttribute_PropertyGrid(CGFXFormat.SOBJ.Shape.VertexAttribute vertexAttribute)
        {
            VertexAttributeOffset = vertexAttribute.VertexAttributeOffset;
            Flag = vertexAttribute.Flag;
            Streams = new Stream.Stream_PropertyGrid(vertexAttribute.Streams);
            Params = new Param.Param_PropertyGrid(vertexAttribute.Params);
        }

        public VertexAttribute_PropertyGrid()
        {
            VertexAttributeOffset = 0;
            Flag = new Flags(new List<byte>().ToArray());
            Streams = new Stream.Stream_PropertyGrid();
            Params = new Param.Param_PropertyGrid();
        }
    }

    //VertexAttributeEditor
    public class VertexAttributeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (svc != null && value != null)
            {
                VertexAttributeEditorForm form = new VertexAttributeEditorForm(value as List<CGFXFormat.SOBJ.Shape.VertexAttribute>);
                form.ShowDialog();

                value = form.vertexAttribute_List;
            }
            return value; // can also replace the wrapper object here
        }
    }
}
