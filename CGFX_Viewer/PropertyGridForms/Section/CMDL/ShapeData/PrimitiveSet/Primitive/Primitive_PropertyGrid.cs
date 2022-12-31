using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.PrimitiveSet.Primitive
{
    public class Primitive_PropertyGrid
    {
        public int PrimitiveOffset { get; set; }
        public int IndexStreamCount { get; set; }
        public int IndexStreamOffsetListOffset { get; set; }

        public List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive.IndexStreamCtr> IndexStreamCtr_List = new List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive.IndexStreamCtr>();
        [Editor(typeof(IndexStreamCtr.IndexStreamCtrEditor), typeof(UITypeEditor))]
        public List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive.IndexStreamCtr> IndexStreamCtrs { get => IndexStreamCtr_List; set => IndexStreamCtr_List = value; }

        public int BufferObjectCount { get; set; }
        public int BufferObjectListOffset { get; set; }
        public List<int> BufferObjectList { get; set; }
        public int Flags { get; set; }
        public int CommandAllocator { get; set; }

        public Primitive_PropertyGrid(CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive primitive)
        {
            PrimitiveOffset = primitive.PrimitiveOffset;
            IndexStreamCount = primitive.IndexStreamCount;
            IndexStreamOffsetListOffset = primitive.IndexStreamOffsetListOffset;

            IndexStreamCtrs = primitive.IndexStreamCtrList;

            BufferObjectCount = primitive.BufferObjectCount;
            BufferObjectListOffset = primitive.BufferObjectListOffset;
            BufferObjectList = primitive.BufferObjectList;
            Flags = primitive.Flags;
            CommandAllocator = primitive.CommandAllocator;
        }
    }

    //PrimitiveEditor
    public class PrimitiveEditor : UITypeEditor
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
                PrimitiveEditorForm form = new PrimitiveEditorForm(value as List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive>);
                form.ShowDialog();

                value = form.primitive_List;
            }
            return value; // can also replace the wrapper object here
        }
    }
}
