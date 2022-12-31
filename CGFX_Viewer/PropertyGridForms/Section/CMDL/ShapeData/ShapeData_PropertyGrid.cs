using CGFX_Viewer.CGFXPropertyGridSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CGFX_Viewer.PropertyGridForms.Section.CMDL.ShapeData
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    internal class ShapeData_PropertyGrid
    {
        public string Name;

        [ReadOnly(true)]
        public char[] SOBJ_Header { get; set; }

        public int Revision { get; set; }

        [ReadOnly(true)]
        public int SOBJNameOffset { get; set; }

        public int UnknownData2 { get; set; }
        public int UnknownData3 { get; set; }
        public int ShapeFlag { get; set; }

        [ReadOnly(true)]
        public int OrientedBoundingBoxOffset { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public BoundingBox OrientedBoundingBox { get; set; } = new BoundingBox();
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
                Flags = new Flags(new byte[] {0x00, 0x00, 0x00, 0x80});
                Position = new Vector3D(0, 0, 0);
                Matrix_BoundingBox = new CGFXFormat.Matrix.Matrix_BoundingBox(1, 0, 0, 0, 1, 0, 0, 0, 1);
                Size = new Vector3D(1, 1, 1);
            }

            public override string ToString()
            {
                return "OrientedBoundingBox";
            }
        }
        public Vector3D PositionOffset { get; set; }
        public int PrimitiveSetCount { get; set; }

        [ReadOnly(true)]
        public int PrimitiveSetListOffset { get; set; }

        public List<CGFXFormat.SOBJ.Shape.PrimitiveSet> PrimitiveSet_List = new List<CGFXFormat.SOBJ.Shape.PrimitiveSet>();
        [Editor(typeof(PrimitiveSet.PrimitiveSetEditor), typeof(UITypeEditor))]
        public List<CGFXFormat.SOBJ.Shape.PrimitiveSet> primitiveSets { get => PrimitiveSet_List; set => PrimitiveSet_List = value; }

        [ReadOnly(true)]
        public int BaseAddress { get; set; }

        [ReadOnly(true)]
        public int VertexAttributeCount { get; set; }

        [ReadOnly(true)]
        public int VertexAttributeOffsetListOffset { get; set; }

        //Editor
        public List<CGFXFormat.SOBJ.Shape.VertexAttribute> VertexAttribute_List = new List<CGFXFormat.SOBJ.Shape.VertexAttribute>();
        [Editor(typeof(VertexAttribute.VertexAttributeEditor), typeof(UITypeEditor))]
        public List<CGFXFormat.SOBJ.Shape.VertexAttribute> VertexAttributes { get => VertexAttribute_List; set => VertexAttribute_List = value; }

        //public List<CGFXFormat.SOBJ.Shape.VertexAttribute> VertexAttributes { get; set; }

        [ReadOnly(true)]
        public int BlendShapeOffset { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public BlendShape BlendShapes { get; set; }
        public class BlendShape
        {
            public int UnknownData1 { get; set; }
            public int UnknownData2 { get; set; }
            public int UnknownData3 { get; set; }
            public int UnknownData4 { get; set; }
            public int UnknownData5 { get; set; }

            public BlendShape(CGFXFormat.SOBJ.Shape.BlendShape blendShape)
            {
                UnknownData1 = blendShape.UnknownData1;
                UnknownData2 = blendShape.UnknownData2;
                UnknownData3 = blendShape.UnknownData3;
                UnknownData4 = blendShape.UnknownData4;
                UnknownData5 = blendShape.UnknownData5;
            }

            public BlendShape()
            {
                UnknownData1 = 0;
                UnknownData2 = 0;
                UnknownData3 = 0;
                UnknownData4 = 0;
                UnknownData5 = 0;
            }

            public override string ToString()
            {
                return "BlendShape";
            }
        }

        public ShapeData_PropertyGrid(CGFXFormat.SOBJ.Shape shape)
        {
            Name = shape.Name;
            SOBJ_Header = shape.SOBJ_Header;
            Revision = shape.Revision;
            SOBJNameOffset = shape.SOBJNameOffset;

            UnknownData2 = shape.UnknownData2;
            UnknownData3 = shape.UnknownData3;

            ShapeFlag = shape.ShapeFlag;

            OrientedBoundingBoxOffset = shape.OrientedBoundingBoxOffset;
            OrientedBoundingBox = new BoundingBox(shape.OrientedBoundingBox);

            PositionOffset = shape.PositionOffset;

            PrimitiveSetCount = shape.PrimitiveSetCount;
            PrimitiveSetListOffset = shape.PrimitiveSetListOffset;

            primitiveSets = shape.primitiveSets;

            BaseAddress = shape.BaseAddress;
            VertexAttributeCount = shape.VertexAttributeCount;
            VertexAttributeOffsetListOffset = shape.VertexAttributeOffsetListOffset;

            VertexAttributes = shape.VertexAttributes;

            BlendShapeOffset = shape.BlendShapeOffset;
            BlendShapes = new BlendShape(shape.BlendShapes);
        }

        public override string ToString()
        {
            return "Shape : " + Name;
        }
    }
}
