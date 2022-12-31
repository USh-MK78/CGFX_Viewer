using CGFX_Viewer.CGFXPropertyGridSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Media.Media3D;

namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.PrimitiveSet.Primitive.IndexStreamCtr
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class IndexStreamCtr_PropertyGrid
    {
        public int IndexStreamOffset { get; set; }
        public Flags Flags { get; set; } //IdentFlag : 0x01 -> Byte, 0x03 -> Short
        public byte PrimitiveMode { get; set; }
        public bool IsVisible { get; set; }
        public short UnknownData { get; set; }
        public int FaceDataLength { get; set; }
        public int FaceDataOffset { get; set; }
        public List<Face> Faces { get; set; }
        public List<int> FaceArray
        {
            get
            {
                List<int> res = new List<int>();
                foreach (var r in Faces)
                {
                    res.Add(r.f0);
                    res.Add(r.f1);
                    res.Add(r.f2);
                }

                return res;
            }
        }

        public int BufferObject { get; set; }
        public int LocationFlag { get; set; }
        public int CommandCache { get; set; }
        public int CommandCacheSize { get; set; }
        public int LocationAddress { get; set; }
        public int MemoryArea { get; set; }
        public int BoundingBoxOffset { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public BoundingBox BoundingBoxSetting { get; set; } = new BoundingBox();
        public class BoundingBox
        {
            public Flags Flags;
            public Vector3D Position { get; set; }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public CGFXFormat.Matrix.Matrix_BoundingBox Matrix_BoundingBox { get; set; }

            public Vector3D Size { get; set; }

            public BoundingBox(CGFXFormat.SOBJ.Shape.BoundingBox boundingBox)
            {
                Flags = boundingBox.Flags;
                Position = boundingBox.Position;
                Matrix_BoundingBox = boundingBox.Matrix_BoundingBox;
                Size = boundingBox.Size;
            }

            public BoundingBox()
            {
                Flags = new Flags(new byte[] { 0x00, 0x00, 0x00, 0x80 });
                Position = new Vector3D(0, 0, 0);
                Matrix_BoundingBox = new CGFXFormat.Matrix.Matrix_BoundingBox(1, 0, 0, 0, 1, 0, 0, 0, 1);
                Size = new Vector3D(1, 1, 1);
            }

            public override string ToString()
            {
                return "BoundingBox";
            }
        }

        public IndexStreamCtr_PropertyGrid(CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive.IndexStreamCtr indexStreamCtr)
        {
            IndexStreamOffset = indexStreamCtr.IndexStreamOffset;
            Flags = indexStreamCtr.Flags;
            PrimitiveMode = indexStreamCtr.PrimitiveMode;
            IsVisible = indexStreamCtr.IsVisible;
            UnknownData = indexStreamCtr.UnknownData;
            FaceDataLength = indexStreamCtr.FaceDataLength;
            FaceDataOffset = indexStreamCtr.FaceDataOffset;

            Faces = indexStreamCtr.Faces;

            BufferObject = indexStreamCtr.BufferObject;
            LocationFlag = indexStreamCtr.LocationFlag;
            CommandCache = indexStreamCtr.CommandCache;
            CommandCacheSize = indexStreamCtr.CommandCacheSize;
            LocationAddress = indexStreamCtr.LocationAddress;
            MemoryArea = indexStreamCtr.MemoryArea;
            BoundingBoxOffset = indexStreamCtr.BoundingBoxOffset;
            if (indexStreamCtr.BoundingBox != null)
            {
                BoundingBoxSetting = new BoundingBox(indexStreamCtr.BoundingBox);
            }
        }
    }

    //IndexStreamCtrEditor
    public class IndexStreamCtrEditor : UITypeEditor
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
                IndexStreamCtrEditorForm form = new IndexStreamCtrEditorForm(value as List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive.IndexStreamCtr>);
                form.ShowDialog();

                value = form.indexStreamCtr_List;
            }
            return value; // can also replace the wrapper object here
        }
    }
}
