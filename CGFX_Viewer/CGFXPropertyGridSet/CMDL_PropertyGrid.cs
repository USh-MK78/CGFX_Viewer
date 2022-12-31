using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace CGFX_Viewer.CGFXPropertyGridSet
{
	[TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
	public class CMDL_PropertyGrid
	{
        public string Name;
        public byte[] CMDL_Revision { get; set; } //0x4
        public int CMDL_ModelNameOffset { get; set; } //0x4

        public List<CGFXFormat.UserData> UserData_List = new List<CGFXFormat.UserData>();
        [Editor(typeof(CGFX_CustomPropertyGridClass.UserDataDictionaryEditor), typeof(UITypeEditor))]
        public List<CGFXFormat.UserData> userDataList { get => UserData_List; set => UserData_List = value; }


        //public int CMDL_UserDataDICTCount { get; set; } //0x4
        //public int CMDL_UserDataDICTOffset { get; set; } //0x4
        //public DICT CMDL_UserData { get; set; }

        public bool IsBranchVisibleFlag1 { get; set; } //0x4
        public bool IsBranchVisibleFlag2 { get; set; } //0x4

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public CMDL_UnknownSection1 CMDLUnknownSection1 { get; set; } = new CMDL_UnknownSection1();
        public class CMDL_UnknownSection1
        {
            public byte[] Unknown_Byte5 { get; set; } //0x4
            public byte[] Unknown_Byte6 { get; set; } //0x4

            public CMDL_UnknownSection1()
            {
                Unknown_Byte5 = new List<byte>().ToArray();
                Unknown_Byte6 = new List<byte>().ToArray();
            }

            public CMDL_UnknownSection1(CGFXFormat.CGFXSection.CMDL.CMDL_UnknownSection1 cMDL_UnknownSection1)
			{
                Unknown_Byte5 = cMDL_UnknownSection1.Unknown_Byte5;
                Unknown_Byte6 = cMDL_UnknownSection1.Unknown_Byte6;
            }
        }

        public int CMDL_NumOfEntries_Animation_DICT { get; set; } //Null : 00 00 00 00
        public int CMDL_Offset_Animation_DICT { get; set; } //Null : 00 00 00 00
        //public DICT AnimationDICT { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public CGFXFormat.Transform.Scale Scale { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public CGFXFormat.Transform.Rotate Rotate { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public CGFXFormat.Transform.Translate Translate { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public CGFXFormat.Matrix.LocalMatrix Local_Matrix { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public CGFXFormat.Matrix.WorldMatrix_Transform WorldMatrix_Transform { get; set; }


        //Mesh
        public int MeshSOBJEntries { get; set; } //0x4
        public int MeshSOBJListOffset { get; set; } //0x4

        public List<CGFXFormat.CGFXSection.CMDL.MeshData> MeshData_List = new List<CGFXFormat.CGFXSection.CMDL.MeshData>();
        //[Editor(typeof(CGFX_CustomPropertyGridClass.UserDataDictionaryEditor), typeof(UITypeEditor))]
        public List<CGFXFormat.CGFXSection.CMDL.MeshData> meshDataList { get => MeshData_List; set => MeshData_List = value; }

        //public class MeshData
        //{
        //    public int MeshDataOffset { get; set; }
        //    public Flags Flags { get; set; }
        //    public CGFXFormat.SOBJ SOBJData { get; set; }
        //}

        public int NumOfMTOB_DICTEntries { get; set; } //0x4
        public int MTOB_DICTOffset { get; set; } //0x4

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public CGFXFormat.DICT MTOB_DICT { get; set; }

        //Shape
        public int NumOfVertexInfoSOBJEntries_2 { get; set; } //0x4
        public int VertexInfoSOBJListOffset_2 { get; set; } //0x4

        public List<CGFXFormat.CGFXSection.CMDL.ShapeData> ShapeData_List = new List<CGFXFormat.CGFXSection.CMDL.ShapeData>();
        //[Editor(typeof(CGFX_CustomPropertyGridClass.UserDataDictionaryEditor), typeof(UITypeEditor))]
        public List<CGFXFormat.CGFXSection.CMDL.ShapeData> shapeDataList { get => ShapeData_List; set => ShapeData_List = value; }

        //UnknownDICT
        public int NumOfUnknownDICTEntries { get; set; }
        public int UnknownDICTOffset { get; set; }
        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public CGFXFormat.DICT UnknownDICT { get; set; }

        public bool IsVisible { get; set; } //0x1
        public bool IsNonuniformScalable { get; set; } //0x1
        public byte[] UnknownData3 { get; set; } //0x2

        public byte[] UnknownData2 { get; set; }
        public int LayerID { get; set; }
        //public byte[] UnknownData4 { get; set; }


        public CMDL_PropertyGrid(CGFXFormat.CGFXSection.CMDL CMDLData)
		{
            Name = CMDLData.Name;
            CMDL_Revision = CMDLData.CMDL_Revision;
            userDataList = CMDLData.CMDL_UserData.DICT_Entries.Select(x => x.CGFXData.UserData).ToList();

            IsBranchVisibleFlag1 = CMDLData.IsBranchVisibleFlag1;
            IsBranchVisibleFlag2 = CMDLData.IsBranchVisibleFlag2;

            CMDLUnknownSection1 = new CMDL_UnknownSection1(CMDLData.CMDLUnknownSection1);
            //AnimationDICT

            Scale = CMDLData.Transform_Scale;
            Rotate = CMDLData.Transform_Rotate;
            Translate = CMDLData.Transform_Translate;
            Local_Matrix = CMDLData.CMDL_4x4_Matrix;
            WorldMatrix_Transform = CMDLData.CMDL_4x4_Matrix_Transform;

            //Scale, Rotate, Translate, Matrix... etc
            //CMDLData.


            meshDataList = CMDLData.meshDatas;

            //MTOB
            MTOB_DICT = CMDLData.MTOB_DICT;

            shapeDataList = CMDLData.shapeDatas;
            
            //UnknownDICT
            UnknownDICT = CMDLData.UnknownDICT;

            IsVisible = CMDLData.IsVisible;
            IsNonuniformScalable = CMDLData.IsNonuniformScalable;
            UnknownData3 = CMDLData.UnknownData3;
            UnknownData2 = CMDLData.UnknownData2;
            LayerID = CMDLData.LayerID;
            //UnknownData4 = CMDLData.UnknownData2;

        }
	}
}
