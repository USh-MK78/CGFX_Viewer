using CGFX_Viewer.CGFXPropertyGridSet;
using CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.PrimitiveSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData.VertexAttribute.Stream.VertexStream
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class VertexStream_PropertyGrid
    {
        public int VertexStreamsOffset { get; set; }

        public Flags Flags { get; set; }
        public CGFX_Viewer.VertexAttribute.Usage VertexAttributeUsageFlag { get; set; }
        public CGFX_Viewer.VertexAttribute.Flag VertexAttributeFlag { get; set; }

        public int BufferObject { get; set; }
        public int LocationFlag { get; set; } //0x10
        public int VertexStreamLength { get; set; }
        public int VertexStreamOffset { get; set; }

        public int LocationAddress { get; set; }
        public int MemoryArea { get; set; } //0x20

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public Component Components { get; set; }
        public class Component
        {
            public Flags Flags { get; set; }

            public FormatType FormatTypes
            {
                get => (FormatType)Flags.IdentFlag[0];
                set => Flags.IdentFlag[0] = Convert.ToByte(Enum.ToObject(typeof(FormatType), value));
            }

            public enum FormatType
            {
                BYTE = 0,
                UNSIGNED_BYTE = 1,
                SHORT = 2,//might also be unsigned short
                FLOAT = 6
            }

            public int ComponentCount { get; set; } //For example XYZ = 3, ST = 2, RGBA = 4
            public ComponentType ComponentTypeFlag => (ComponentType)ComponentCount;
            public enum ComponentType
            {
                ST = 2,
                XYZ = 3,
                RGBA = 4
            }

            public Component(CGFXFormat.SOBJ.Shape.VertexAttribute.Stream.VertexStream.Component component)
            {
                Flags = component.Flags;
                ComponentCount = component.ComponentCount;
            }

            public Component()
            {
                Flags = new Flags(new List<byte>().ToArray());
                ComponentCount = 0;
            }
        }

        public float Scale { get; set; }
        public int Offset { get; set; }

        public VertexStream_PropertyGrid(CGFXFormat.SOBJ.Shape.VertexAttribute.Stream.VertexStream vertexStream)
        {
            VertexStreamsOffset = vertexStream.VertexStreamsOffset;
            VertexAttributeUsageFlag = vertexStream.VertexAttributeUsageFlag;
            VertexAttributeFlag = vertexStream.VertexAttributeFlag;

            BufferObject = vertexStream.BufferObject;
            LocationFlag = vertexStream.LocationFlag;

            VertexStreamLength = vertexStream.VertexStreamLength;
            VertexStreamOffset = vertexStream.VertexStreamOffset;

            LocationAddress = vertexStream.LocationAddress;
            MemoryArea = vertexStream.MemoryArea;

            Components = new Component(vertexStream.Components);

            Scale = vertexStream.Scale;
            Offset = vertexStream.Offset;
        }

        public VertexStream_PropertyGrid()
        {
            VertexStreamsOffset = 0;

            Flags = new Flags(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            VertexAttributeUsageFlag = new CGFX_Viewer.VertexAttribute.Usage(-1);
            VertexAttributeFlag = new CGFX_Viewer.VertexAttribute.Flag(-1);

            BufferObject = 0;
            LocationFlag = 0;
            VertexStreamLength = 0;
            VertexStreamOffset = 0;

            LocationAddress = 0;
            MemoryArea = 0;
            Components = new Component();
            Scale = 0;
            Offset = 0;
        }
    }

    //VertexStreamEditor
    public class VertexStreamEditor : UITypeEditor
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
                VertexStreamEditorForm form = new VertexStreamEditorForm(value as List<CGFXFormat.SOBJ.Shape.VertexAttribute.Stream.VertexStream>);
                form.ShowDialog();

                value = form.vertexStream_list;
            }
            return value; // can also replace the wrapper object here
        }
    }
}
