using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CGFX_Viewer
{
	public class CGFXFormat
	{
        public class CGFX
        {
            public char[] CGFX_Header { get; set; } //0x4
            public byte[] BOM { get; set; } //0x2
            public short CGFXHeaderSize { get; set; } //0x2
            public int Revision { get; set; } //0x4
            public int FileSize { get; set; } //0x4
            public int NumOfEntries { get; set; } //0x4
            public DATA Data { get; set; }
            public class DATA
            {
                public char[] DATA_Header { get; set; } //0x4
                public int DATASize { get; set; } //0x4
                public Dictionary<string, DATA.DictOffset> DictOffset_Dictionary { get; set; }

                public class DictOffset
                {
                    public int NumOfEntries { get; set; }
                    public int OffsetToDICT { get; set; }

                    public DictOffset()
                    {
                        NumOfEntries = 0;
                        OffsetToDICT = 0;
                    }

                    public void ReadDictOffset(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        NumOfEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        OffsetToDICT = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }
                }

                public DATA()
                {
                    DATA_Header = "DATA".ToCharArray();
                    DATASize = 0;
                    DictOffset_Dictionary = new Dictionary<string, DATA.DictOffset>();
                }

                public void ReadDATA(BinaryReader br, byte[] BOM)
                {
                    DATA_Header = br.ReadChars(4);
                    if (new string(DATA_Header) != "DATA") throw new Exception("不明なフォーマットです");
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    DATASize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    for (int i = 0; i < 16; i++)
                    {
                        string[] vs = new string[] { "Model", "Textures", "LUTS", "Materials", "Shaders", "Cameras", "Lights", "Fog", "Environments", "Skeleton_Animations", "Texture_Animations", "Visibility_Animations", "Camera_Animations", "Light_Animations", "Fog_Animations", "Emitters" };

                        DATA.DictOffset dictOffset = new DictOffset();
                        dictOffset.ReadDictOffset(br, BOM);
                        DictOffset_Dictionary.Add(vs[i], dictOffset);
                    }
                }
            }

            public Dictionary<string, DICT> DICTAndSectionData { get; set; }

            public void ReadCGFX(BinaryReader br)
            {
                CGFX_Header = br.ReadChars(4);
                if (new string(CGFX_Header) != "CGFX") throw new Exception("不明なフォーマットです");
                BOM = br.ReadBytes(2);

                EndianConvert endianConvert = new EndianConvert(BOM);
                CGFXHeaderSize = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                Revision = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                FileSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                NumOfEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                Data.ReadDATA(br, BOM);

                string[] vs = new string[] { "Model", "Textures", "LUTS", "Materials", "Shaders", "Cameras", "Lights", "Fog", "Environments", "Skeleton_Animations", "Texture_Animations", "Visibility_Animations", "Camera_Animations", "Light_Animations", "Fog_Animations", "Emitters" };

                for (int i = 0; i < Data.DictOffset_Dictionary.Count; i++)
                {
                    if (Data.DictOffset_Dictionary[vs[i]].NumOfEntries != 0)
                    {
                        DICT dICT = new DICT();
                        dICT.ReadDICT(br, BOM);
                        DICTAndSectionData.Add(vs[i], dICT);
                    }
                }
            }

            public CGFX()
            {
                CGFX_Header = "CGFX".ToCharArray();
                BOM = new List<byte>().ToArray();
                CGFXHeaderSize = 0;
                Revision = 0;
                FileSize = 0;
                NumOfEntries = 0;
                Data = new DATA();
                DICTAndSectionData = new Dictionary<string, DICT>();
            }
        }

        public class DICT
        {
            public char[] DICT_Header { get; set; } //0x4
            public int DICTSize { get; set; } //0x4
            public int DICT_NumOfEntries { get; set; } //0x4

            public RootNode RootNodeData { get; set; }
            public class RootNode
            {
                public string Name;
                public int RN_RefBit { get; set; } //0x4
                public short RN_LeftIndex { get; set; } //0x2
                public short RN_RightIndex { get; set; } //0x2
                public int RN_NameOffset { get; set; } //0x4
                public int RN_DataOffset { get; set; } //0x4

                public RootNode()
                {
                    RN_RefBit = 0;
                    RN_LeftIndex = 0;
                    RN_RightIndex = 0;
                    RN_NameOffset = 0;
                    RN_DataOffset = 0;
                }

                public void Read_RootNode(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    RN_RefBit = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    RN_LeftIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                    RN_RightIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                    RN_NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (RN_NameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move RN_NameOffset
                        br.BaseStream.Seek(RN_NameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    RN_DataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (RN_DataOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move RN_DataOffset
                        br.BaseStream.Seek(RN_DataOffset, SeekOrigin.Current);


                        //Read Section Data (?)



                        br.BaseStream.Position = Pos;
                    }
                }

                public int GetLength()
                {
                    return 16;
                }
            }

            public List<DICT_Entry> DICT_Entries { get; set; }
            public class DICT_Entry
            {
                public string Name;

                public int RefBit { get; set; } //0x4
                public short LeftIndex { get; set; } //0x2
                public short RightIndex { get; set; } //0x2
                public int NameOffset { get; set; } //0x4
                public int DataOffset { get; set; } //0x4
                public CGFXData CGFXData { get; set; }

				public void Read_DICTEntry(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    RefBit = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    LeftIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                    RightIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                    NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (NameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(NameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    DataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (DataOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(DataOffset, SeekOrigin.Current);

                        CGFXData = new CGFXData(br.ReadBytes(4));
                        CGFXData.Reader(br, BOM);

						br.BaseStream.Position = Pos;
                    }
                }

                public DICT_Entry()
                {
                    RefBit = 0;
                    LeftIndex = 0;
                    RightIndex = 0;
                    NameOffset = 0;
                    DataOffset = 0;
                    CGFXData = null;
                }

                public int GetLength()
                {
                    return 16;
                }

                public string GetEntryName()
                {
                    return Name;
                }
            }

            public DICT()
            {
                DICT_Header = "DICT".ToCharArray();
                DICTSize = 0;
                DICT_NumOfEntries = 0;
                RootNodeData = new RootNode();
                DICT_Entries = new List<DICT_Entry>();
            }

            public void ReadDICT(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                DICT_Header = br.ReadChars(4);
                if (new string(DICT_Header) != "DICT") throw new Exception("不明なフォーマットです");

                DICTSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                DICT_NumOfEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                RootNodeData.Read_RootNode(br, BOM);

                for (int i = 0; i < DICT_NumOfEntries; i++)
                {
                    DICT_Entry dICT_Entry = new DICT_Entry();
                    dICT_Entry.Read_DICTEntry(br, BOM);
                    DICT_Entries.Add(dICT_Entry);
                }
            }

            public int GetLength()
            {
                int H = DICT_Header.Length;
                int Size = 4;
                int NumOfEntries = 4;
                int RNSize = RootNodeData.GetLength();
                int count = DICT_Entries.Select(x => x.GetLength()).Sum();

                return H + Size + NumOfEntries + RNSize + count;
            }
        }

        public class UserData
        {
            //DICT_Entry -> DICTEntry_OffsetToObject = 04 00 00 00(Null)
            //DICTEntry_OffsetToObject = Null : StringData, Int32Data, RealNumber_FloatData = Null
            //Read : Int32DataEndCode

            public UserDataType Type { get; set; }

            public StringData String_Data { get; set; }
            public class StringData
            {
                public long EndPos;

                public string UDName;

                //Flag : 00 00 00 10
                public int UserDataStringNameOffset { get; set; } //0x4
                public byte[] Flag2 { get; set; } //0x4(Default Value : 02 00 00 00(?))
                public byte[] Flag3 { get; set; } //0x4(Default Value : 02 00 00 00(?))
                public int STRING_ValueCount { get; set; }

                public List<UserDataItem_String> UserDataItem_String_List { get; set; }
                public class UserDataItem_String
                {
                    public int Offset { get; set; } //0x4
                    public string StringData { get; set; }

                    public UserDataItem_String(int InputOffset, string InputString)
                    {
                        Offset = InputOffset;
                        StringData = InputString;
                    }

                    public void ReadData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(Offset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadMultiByte(br, 0x00);

                        StringData = new string(readByteLine.ConvertToCharArray().Where(x => x != '\0').ToArray());

                        br.BaseStream.Position = Pos;
                    }
                }

                public List<UserDataItem_String> ReadUserDataStringList(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UserDataStringNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (UserDataStringNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(UserDataStringNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        UDName = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    Flag2 = endianConvert.Convert(br.ReadBytes(4));
                    Flag3 = endianConvert.Convert(br.ReadBytes(4));
                    STRING_ValueCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    for (int UDStrCount = 0; UDStrCount < STRING_ValueCount; UDStrCount++)
                    {
                        UserDataItem_String userData = new UserDataItem_String(0, "");
                        userData.ReadData(br, BOM);
                        UserDataItem_String_List.Add(userData);
                    }

                    EndPos = br.BaseStream.Position;

                    return UserDataItem_String_List;
                }

                public StringData()
                {
                    UserDataStringNameOffset = 0;
                    Flag2 = new List<byte>().ToArray();
                    Flag3 = new List<byte>().ToArray();
                    STRING_ValueCount = 0;
                    UserDataItem_String_List = new List<UserDataItem_String>();
                }

				public override string ToString()
				{
					return UDName;
				}
			}

            public Int32Data Int32_Data { get; set; }
            public class Int32Data
            {
                public long EndPos;

                public string UDName;

                //Flag : 00 00 00 20
                public int UserDataInt32NameOffset { get; set; } //0x4
                public byte[] Flag2 { get; set; } //0x4(Default Value : 01 00 00 00(?))
                public int INT32_ValueCount { get; set; } //0x4

                public List<UserDataItem_INT32> UserDataItem_Int32Data_List { get; set; }
                public class UserDataItem_INT32
                {
                    public int UserDataItem_Int32_Value { get; set; } //0x4

                    public UserDataItem_INT32(int Input)
                    {
                        UserDataItem_Int32_Value = Input;
                    }

                    public void ReadData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        UserDataItem_Int32_Value = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }
                }

                public List<UserDataItem_INT32> ReadUserDataInt32List(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UserDataInt32NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (UserDataInt32NameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(UserDataInt32NameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        UDName = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    Flag2 = endianConvert.Convert(br.ReadBytes(4));
                    INT32_ValueCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    for (int UDStrCount = 0; UDStrCount < INT32_ValueCount; UDStrCount++)
                    {
                        UserDataItem_INT32 userData = new UserDataItem_INT32(0);
                        userData.ReadData(br, BOM);
                        UserDataItem_Int32Data_List.Add(userData);
                    }

                    EndPos = br.BaseStream.Position;

                    return UserDataItem_Int32Data_List;
                }

                public Int32Data()
                {
                    UserDataInt32NameOffset = 0;
                    Flag2 = new List<byte>().ToArray();
                    INT32_ValueCount = 0;
                    UserDataItem_Int32Data_List = new List<UserDataItem_INT32>();
                }

				public override string ToString()
				{
					return UDName;
				}
			}

            public RealNumber RealNumber_Data { get; set; }
            public class RealNumber
            {
                public long EndPos;

                public string UDName;
                public string UDName_Sub;

                //Flag : 00 00 00 80
                public int RealNumberNameOffset { get; set; } //0x4
                public int SubNameStringOffset { get; set; } //0x4(Using Fog Animation, ColorData, Default = 0)
                public int REALNUMBERCount { get; set; } //0x4

                public int UnkData { get; set; }

                public List<UserDataItem_REALNUMBER> UserDataItem_RealNumber_List { get; set; }
                public class UserDataItem_REALNUMBER
                {
                    public RealNumberType RealNumberType;
                    public float FloatValue { get; set; }
                    public Color color { get; set; }
                    public class Color
                    {
                        public int R { get; set; }
                        public int G { get; set; }
                        public int B { get; set; }
                        public int A { get; set; }

                        public Color(int i1, int i2, int i3, int i4)
                        {
                            R = i1;
                            G = i2;
                            B = i3;
                            A = i4;
                        }

                        public void ReadColorSet(BinaryReader br)
                        {
                            R = br.ReadByte();
                            G = br.ReadByte();
                            B = br.ReadByte();
                            A = br.ReadByte();
                        }
                    }

                    public UserDataItem_REALNUMBER(RealNumberType realNumberType)
                    {
                        RealNumberType = realNumberType;
                        if (realNumberType == RealNumberType.Color) color = new Color(0, 0, 0, 0);
                        else if (realNumberType == RealNumberType.Float) FloatValue = 0;
                    }

                    public void ReadData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        if (RealNumberType == RealNumberType.Color)
                        {
                            color.ReadColorSet(br);
                        }
                        if (RealNumberType == RealNumberType.Float)
                        {
                            FloatValue = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        }
                    }
                }

                public List<UserDataItem_REALNUMBER> ReadUserDataList(BinaryReader br, byte[] BOM, RealNumberType realNumberType)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    RealNumberNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (RealNumberNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(RealNumberNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        UDName = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    SubNameStringOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (SubNameStringOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(SubNameStringOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        UDName_Sub = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    REALNUMBERCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    if (realNumberType == RealNumberType.Float)
                    {
                        for (int UDStrCount = 0; UDStrCount < REALNUMBERCount; UDStrCount++)
                        {
                            UserDataItem_REALNUMBER userDataItem_REALNUMBER = new UserDataItem_REALNUMBER(realNumberType);
                            userDataItem_REALNUMBER.ReadData(br, BOM);

                            UserDataItem_RealNumber_List.Add(userDataItem_REALNUMBER);
                        }
                    }
                    if (realNumberType == RealNumberType.Color)
                    {
                        UnkData = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                        int count = REALNUMBERCount / UnkData;

                        for (int UDStrCount = 0; UDStrCount < count; UDStrCount++)
                        {
                            UserDataItem_REALNUMBER userDataItem_REALNUMBER = new UserDataItem_REALNUMBER(realNumberType);
                            userDataItem_REALNUMBER.ReadData(br, BOM);

                            UserDataItem_RealNumber_List.Add(userDataItem_REALNUMBER);
                        }
                    }

                    EndPos = br.BaseStream.Position;

                    return UserDataItem_RealNumber_List;
                }

                public RealNumber()
                {
                    RealNumberNameOffset = 0;
                    SubNameStringOffset = 0;
                    REALNUMBERCount = 0;
                    UserDataItem_RealNumber_List = new List<UserDataItem_REALNUMBER>();
                }

                public enum RealNumberType
                {
                    Float,
                    Color
                }

				public override string ToString()
				{
					return "Main : " + UDName + " | " + "Sub : " + UDName_Sub;
				}
			}

            public enum UserDataType
            {
                String = 0x10,
                Int32 = 0x20,
                RealNumber = 0x80,
                NOTSET
            }

            public UserData()
            {
                String_Data = new StringData();
                Int32_Data = new Int32Data();
                RealNumber_Data = new RealNumber();
            }

            public UserData(byte type)
            {
                Type = (UserDataType)Enum.ToObject(typeof(UserDataType), (int)type);
                if (type == (int)UserDataType.String) String_Data = new StringData();
                if (type == (int)UserDataType.Int32) Int32_Data = new Int32Data();
                if (type == (int)UserDataType.RealNumber) RealNumber_Data = new RealNumber();

                RealNumber_Data = new RealNumber();
            }

            public UserData(UserDataType type)
            {
                Type = type;
                if (type == UserDataType.String) String_Data = new StringData();
                if (type == UserDataType.Int32) Int32_Data = new Int32Data();
                if (type == UserDataType.RealNumber) RealNumber_Data = new RealNumber();
            }
        }

        public class CGFXSection
		{
            //Models
            public CMDL CMDLSection { get; set; }
            public class CMDL
            {
                public string Name;

                //public byte[] CMDL_BitFlags { get; set; } //0x4(bit7:Has Skeleton Object)
                public char[] CMDL_Header { get; set; } //0x4
                public byte[] CMDL_Revision { get; set; } //0x4
                public int CMDL_ModelNameOffset { get; set; } //0x4
                public int CMDL_UserDataDICTCount { get; set; } //0x4
                public int CMDL_UserDataDICTOffset { get; set; } //0x4
                public DICT CMDL_UserData { get; set; }

				public bool IsBranchVisibleFlag1 { get; set; } //0x4
				public bool IsBranchVisibleFlag2 { get; set; } //0x4

				public CMDL_UnknownSection1 CMDLUnknownSection1 { get; set; }
                public class CMDL_UnknownSection1
                {
                    public byte[] Unknown_Byte5 { get; set; } //0x4
                    public byte[] Unknown_Byte6 { get; set; } //0x4

                    public CMDL_UnknownSection1()
					{
                        Unknown_Byte5 = new List<byte>().ToArray();
                        Unknown_Byte6 = new List<byte>().ToArray();
                    }

                    public void Read_UnkSection1(BinaryReader br, byte[] BOM)
					{
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        Unknown_Byte5 = endianConvert.Convert(br.ReadBytes(4));
                        Unknown_Byte6 = endianConvert.Convert(br.ReadBytes(4));
                    }
                }

                public int CMDL_NumOfEntries_Animation_DICT { get; set; } //Null : 00 00 00 00
                public int CMDL_Offset_Animation_DICT { get; set; } //Null : 00 00 00 00
                public DICT AnimationDICT { get; set; }

                public Transform.Scale Transform_Scale { get; set; }
                public Transform.Rotate Transform_Rotate { get; set; }
                public Transform.Translate Transform_Translate { get; set; }
                public Matrix.LocalMatrix CMDL_4x4_Matrix { get; set; }
                public Matrix.WorldMatrix_Transform CMDL_4x4_Matrix_Transform { get; set; }

                //Mesh
                public int NumOfMeshSOBJEntries { get; set; } //0x4
                public int MeshSOBJListOffset { get; set; } //0x4
                public List<MeshData> meshDatas { get; set; }
                public class MeshData
				{
                    public int MeshDataOffset { get; set; }
                    public Flags Flags { get; set; }
                    public SOBJ SOBJData { get; set; }

                    public MeshData()
					{
                        MeshDataOffset = 0;
                        Flags = null;
                        SOBJData = new SOBJ(SOBJ.SOBJType.Mesh);
					}

                    public void Read(BinaryReader br, byte[] BOM)
					{
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        MeshDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (MeshDataOffset != 0)
                        {
                            long Pos = br.BaseStream.Position;

                            br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //Move DataOffset
                            br.BaseStream.Seek(MeshDataOffset, SeekOrigin.Current);

                            Flags = new Flags(endianConvert.Convert(br.ReadBytes(4)));
                            SOBJData.Read_SOBJ(br, BOM);  //Read

                            br.BaseStream.Position = Pos;
                        }
                    }
				}

                public int NumOfMTOB_DICTEntries { get; set; } //0x4
                public int MTOB_DICTOffset { get; set; } //0x4
                public DICT MTOB_DICT { get; set; }

                //Shape
                public int NumOfVertexInfoSOBJEntries { get; set; } //0x4
                public int VertexInfoSOBJListOffset { get; set; } //0x4
                public List<ShapeData> shapeDatas { get; set; }
                public class ShapeData
                {
                    public int shapeDataOffset { get; set; }
                    public Flags Flags { get; set; }
                    public SOBJ SOBJData { get; set; }

                    public ShapeData()
                    {
                        shapeDataOffset = 0;
                        Flags = null;
                        SOBJData = new SOBJ(SOBJ.SOBJType.Shape);
                    }

                    public void Read(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        shapeDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (shapeDataOffset != 0)
                        {
                            long Pos = br.BaseStream.Position;

                            br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //Move DataOffset
                            br.BaseStream.Seek(shapeDataOffset, SeekOrigin.Current);

                            Flags = new Flags(endianConvert.Convert(br.ReadBytes(4)));
                            SOBJData.Read_SOBJ(br, BOM);  //Read

                            br.BaseStream.Position = Pos;
                        }
                    }
                }

                public int NumOfUnknownDICTEntries { get; set; }
                public int UnknownDICTOffset { get; set; }
                public DICT UnknownDICT { get; set; }

                public bool IsVisible { get; set; } //0x1
                public bool IsNonuniformScalable { get; set; } //0x1
                public byte[] UnknownData3 { get; set; } //0x2

                public byte[] UnknownData2 { get; set; }
                public int LayerID { get; set; }
                public byte[] UnknownData4 { get; set; }

                //public CMDL_SkeletonInfo CMDLSkeletonInfo { get; set; }
                //public class CMDL_SkeletonInfo
                //{
                //    public byte[] SkeletonInfoSOBJOffset { get; set; } //0x4(Null = Delete)
                //    public byte[] CMDL_ZEROPadding { get; set; } //00 00 00 00
                //}

                //public List<CMDL_VertexInfoSOBJ_1_List> CMDL_VertexInfoSOBJ_List1 { get; set; }
                //public class CMDL_VertexInfoSOBJ_1_List
                //{
                //    public byte[] Offset { get; set; } //0x4 (NumOfVertexInfoSOBJEntries_1)
                //}

                //public List<CMDL_VertexInfoSOBJ_2_List> CMDL_VertexInfoSOBJ_List2 { get; set; }
                //public class CMDL_VertexInfoSOBJ_2_List
                //{
                //    public byte[] Offset { get; set; } //0x4 (NumOfVertexInfoSOBJEntries_2)
                //}



                public void ReadCMDL(BinaryReader br, byte[] BOM)
				{
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    CMDL_Header = br.ReadChars(4);
                    if (new string(CMDL_Header) != "CMDL") throw new Exception("不明なフォーマットです");

                    CMDL_Revision = endianConvert.Convert(br.ReadBytes(4));
                    CMDL_ModelNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CMDL_ModelNameOffset != 0)
					{
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(CMDL_ModelNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    CMDL_UserDataDICTCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    CMDL_UserDataDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CMDL_UserDataDICTOffset != 0)
					{
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(CMDL_Offset_Animation_DICT, SeekOrigin.Current);

                        CMDL_UserData.ReadDICT(br, BOM);

                        br.BaseStream.Position = Pos;
                    }

                    IsBranchVisibleFlag1 = BitConverter.ToBoolean(endianConvert.Convert(br.ReadBytes(4)), 0);
                    IsBranchVisibleFlag2 = BitConverter.ToBoolean(endianConvert.Convert(br.ReadBytes(4)), 0);

                    CMDLUnknownSection1.Read_UnkSection1(br, BOM);
                    CMDL_NumOfEntries_Animation_DICT = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    CMDL_Offset_Animation_DICT = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CMDL_Offset_Animation_DICT != 0)
					{
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(CMDL_Offset_Animation_DICT, SeekOrigin.Current);

                        AnimationDICT.ReadDICT(br, BOM);

                        br.BaseStream.Position = Pos;
                    }

                    Transform_Scale.ReadScale(br, BOM);
                    Transform_Rotate.ReadRotate(br, BOM);
                    Transform_Translate.ReadTranslate(br, BOM);
                    CMDL_4x4_Matrix.ReadLocalMatrix(br, BOM);
                    CMDL_4x4_Matrix_Transform.ReadMatrix4x4Transform(br, BOM);

                    //Mesh
                    NumOfMeshSOBJEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    MeshSOBJListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (MeshSOBJListOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(MeshSOBJListOffset, SeekOrigin.Current);

                        for (int i = 0; i < NumOfMeshSOBJEntries; i++)
						{
                            MeshData meshData = new MeshData();
                            meshData.Read(br, BOM);
                            meshDatas.Add(meshData);
						}

                        br.BaseStream.Position = Pos;
                    }

                    NumOfMTOB_DICTEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    MTOB_DICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (MTOB_DICTOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(MTOB_DICTOffset, SeekOrigin.Current);

                        MTOB_DICT.ReadDICT(br, BOM);

                        br.BaseStream.Position = Pos;
                    }

                    //Shape
                    NumOfVertexInfoSOBJEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    VertexInfoSOBJListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (VertexInfoSOBJListOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(VertexInfoSOBJListOffset, SeekOrigin.Current);

                        for (int i = 0; i < NumOfVertexInfoSOBJEntries; i++)
                        {
                            ShapeData shapeData = new ShapeData();
                            shapeData.Read(br, BOM);
                            shapeDatas.Add(shapeData);
                        }

                        br.BaseStream.Position = Pos;
                    }

                    NumOfUnknownDICTEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (UnknownDICTOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(UnknownDICTOffset, SeekOrigin.Current);

                        //No IdentFlag, NameOffset(0x4), Unknown(0x4)
                        UnknownDICT.ReadDICT(br, BOM);

                        br.BaseStream.Position = Pos;
                    }

                    IsVisible = Converter.ByteToBoolean(br.ReadByte());
					IsNonuniformScalable = Converter.ByteToBoolean(br.ReadByte());
                    UnknownData3 = endianConvert.Convert(br.ReadBytes(2));

                    UnknownData2 = endianConvert.Convert(br.ReadBytes(4));
                    LayerID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData4 = endianConvert.Convert(br.ReadBytes(4));
                }

                public CMDL()
				{
                    CMDL_Header = "CMDL".ToCharArray();
                    CMDL_Revision = new List<byte>().ToArray();
                    CMDL_ModelNameOffset = 0;
                    CMDL_UserDataDICTCount = 0;
                    CMDL_UserDataDICTOffset = 0;
                    CMDL_UserData = new DICT();
                    IsBranchVisibleFlag1 = new bool();
                    IsBranchVisibleFlag2 = new bool();
                    CMDLUnknownSection1 = new CMDL_UnknownSection1();
                    CMDL_NumOfEntries_Animation_DICT = 0;
                    CMDL_Offset_Animation_DICT = 0;
                    AnimationDICT = new DICT();
                    Transform_Scale = new Transform.Scale();
                    Transform_Rotate = new Transform.Rotate();
                    Transform_Translate = new Transform.Translate();
                    CMDL_4x4_Matrix = new Matrix.LocalMatrix();
                    CMDL_4x4_Matrix_Transform = new Matrix.WorldMatrix_Transform();
                    NumOfMeshSOBJEntries = 0;
                    MeshSOBJListOffset = 0;
                    meshDatas = new List<MeshData>();
                    NumOfMTOB_DICTEntries = 0;
                    MTOB_DICTOffset = 0;
                    MTOB_DICT = new DICT();
                    NumOfVertexInfoSOBJEntries = 0;
                    VertexInfoSOBJListOffset = 0;
                    shapeDatas = new List<ShapeData>();
                    NumOfUnknownDICTEntries = 0;
                    UnknownDICTOffset = 0;
                    UnknownDICT = new DICT();
                    IsVisible = new bool();
                    IsNonuniformScalable = new bool();
                    UnknownData3 = new List<byte>().ToArray();
                    UnknownData2 = new List<byte>().ToArray();
                    LayerID = 0;
                    UnknownData4 = new List<byte>().ToArray();
                }
            }

            //Textures
            public TXOB TXOBSection { get; set; }
            public class TXOB
			{
                public string Name;
                public char[] TXOB_Header { get; set; }
                public int UnknownData1 { get; set; }
                public int TXOBNameOffset { get; set; }

                public byte[] UnknownByte1 { get; set; }
                public byte[] UnknownByte2 { get; set; }

                public int TextureHeight { get; set; }
                public int TextureWidth { get; set; }

                public byte[] UnknownByte3 { get; set; }
                public byte[] UnknownByte4 { get; set; }

                public int MipMapLevel { get; set; }

                public byte[] UnknownByte5 { get; set; }
                public byte[] UnknownByte6 { get; set; }

                public int TexFormat { get; set; }
                public CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat ImageFormat
				{
					get
					{
                        return (CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat)TexFormat;
					}
					set
					{
                        TexFormat = (int)value;
					}
				}

                public int UnknownData2 { get; set; }

                public int TextureHeight2 { get; set; }
                public int TextureWidth2 { get; set; }

                public int TextureDataSize { get; set; }

                public int TextureDataOffset { get; set; }

                public byte[] TexData { get; set; }
                public Bitmap TXOB_Bitmap
				{
					get
					{
                        return CGFX_Viewer.CGFX.TextureFormat.Textures.ToBitmap(TexData, TextureWidth, TextureHeight, ImageFormat);
                    }
					set
					{
                        TexData = CGFX_Viewer.CGFX.TextureFormat.Textures.FromBitmap(value, ImageFormat);

                    }
				}

                public byte[] UnknownByte7 { get; set; }
                public byte[] UnknownByte8 { get; set; }
                public byte[] UnknownByte9 { get; set; }
                public byte[] UnknownByte10 { get; set; }

                public TXOB()
				{
                    TXOB_Header = "TXOB".ToCharArray();
                    UnknownData1 = 0;
                    TXOBNameOffset = 0;

                    UnknownByte1 = new List<byte>().ToArray();
                    UnknownByte2 = new List<byte>().ToArray();

                    TextureHeight = 0;
                    TextureWidth = 0;

                    UnknownByte3 = new List<byte>().ToArray();
                    UnknownByte4 = new List<byte>().ToArray();

                    MipMapLevel = 0;

                    UnknownByte5 = new List<byte>().ToArray();
                    UnknownByte6 = new List<byte>().ToArray();

                    TexFormat = 0;

                    UnknownData2 = 0;

                    TextureHeight2 = 0;
                    TextureWidth2 = 0;

                    TextureDataSize = 0;
                    TextureDataOffset = 0;

                    TexData = new List<byte>().ToArray();

                    UnknownByte7 = new List<byte>().ToArray();
                    UnknownByte8 = new List<byte>().ToArray();
                    UnknownByte9 = new List<byte>().ToArray();
                    UnknownByte10 = new List<byte>().ToArray();
                }

                public void ReadTXOB(BinaryReader br, byte[] BOM)
				{
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    TXOB_Header = br.ReadChars(4);
                    if (new string(TXOB_Header) != "TXOB") throw new Exception("不明なフォーマットです");
                    UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    TXOBNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TXOBNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(TXOBNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    UnknownByte1 = endianConvert.Convert(br.ReadBytes(4));
                    UnknownByte2 = endianConvert.Convert(br.ReadBytes(4));

                    TextureHeight = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    TextureWidth = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    UnknownByte3 = endianConvert.Convert(br.ReadBytes(4));
                    UnknownByte4 = endianConvert.Convert(br.ReadBytes(4));

                    MipMapLevel = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    UnknownByte5 = endianConvert.Convert(br.ReadBytes(4));
                    UnknownByte6 = endianConvert.Convert(br.ReadBytes(4));

                    TexFormat = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    TextureHeight2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    TextureWidth2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    TextureDataSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    TextureDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TextureDataOffset != 0)
					{
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(TextureDataOffset, SeekOrigin.Current);


                        TexData = endianConvert.Convert(br.ReadBytes(TextureDataSize));
                        //TexData = br.ReadBytes(TextureDataSize);

                        br.BaseStream.Position = Pos;
                    }

                    UnknownByte7 = endianConvert.Convert(br.ReadBytes(4));
                    UnknownByte8 = endianConvert.Convert(br.ReadBytes(4));
                    UnknownByte9 = endianConvert.Convert(br.ReadBytes(4));
                    UnknownByte10 = endianConvert.Convert(br.ReadBytes(4));
                }
            }

            //LUTS
            //Materials
            //Shaders
            //Cameras
            //Lights
            public CFOG CFOGSection { get; set; }
            public class CFOG
            {
                //public long EndPos;
                public string Name;

                public char[] CFOG_Header { get; set; } //0x4
                public byte[] CFOG_UnknownBytes_2 { get; set; } //0x4
                public int CFOG_NameStringOffset { get; set; } //0x4
                public int CFOG_NumOfUserDataDICTEntries { get; set; } //0x4
                public int CFOG_UserDataDICTOffset { get; set; } //0x4
                public DICT UserDataDict { get; set; }
                public FogFlipSetting FogFlipSettings { get; set; }
                public class FogFlipSetting
                {
                    public int Z_flip { get; set; } //0x4

                    public FlipSetting FlipSettings
                    {
                        get => (FlipSetting)Z_flip;
                        set => Z_flip = (int)value;
                    }

                    public enum FlipSetting
                    {
                        Enable = 3,
                        Disable = 1
                    }

                    /// <summary>
                    /// 1 : Disable, 3 = Enable
                    /// </summary>
                    /// <param name="InputZFlip"></param>
                    public FogFlipSetting(int InputZFlip)
                    {
                        Z_flip = InputZFlip;
                    }

                    /// <summary>
                    /// 
                    /// </summary>
                    /// <param name="flipSetting"></param>
                    public FogFlipSetting(FlipSetting flipSetting = FlipSetting.Disable)
                    {
                        FlipSettings = flipSetting;
                    }
                }

                public byte[] CFOG_UnknownBytes_5 { get; set; } //0x4
                public byte[] CFOG_UnknownBytes_6 { get; set; } //0x4
                public byte[] CFOG_UnknownBytes_7 { get; set; } //0x4
                public byte[] CFOG_UnknownBytes_8 { get; set; } //0x4
                public int CFOG_CFOGAnimation_DICTOffset { get; set; } //0x4
                public DICT FogAnimationDICT { get; set; }
                public DICT ColorDICT { get; set; }
                public Transform.Scale Transform_Scale { get; set; }
                public Transform.Rotate Transform_Rotate { get; set; }
                public Transform.Translate Transform_Translate { get; set; }
                public Matrix.LocalMatrix CFOG_4x4_Matrix { get; set; }
                public Matrix.WorldMatrix_Transform CFOG_4x4_Matrix_Transform { get; set; }
                public CFOG_RGBA Color_RGBA { get; set; } //RGBA
                public class CFOG_RGBA
                {
                    public float Color_R { get; set; } //0x4
                    public float Color_G { get; set; } //0x4
                    public float Color_B { get; set; } //0x4
                    public float Color_A { get; set; } //0x4

                    public CFOG_RGBA(float R, float G, float B, float A)
                    {
                        Color_R = R;
                        Color_G = G;
                        Color_B = B;
                        Color_A = A;
                    }

                    public void ReadCFOGRGBA(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        Color_R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        Color_G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        Color_B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        Color_A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }
                }

                public int CFOG_UnknownOffset1 { get; set; } //0x4(UserData : float)
                public Flags Flag { get; set; }
                public UserData UnknownUserData { get; set; }

                public int FogSettingOffset { get; set; } //0x4
                public CFOGSetting CFOGSettings { get; set; }
                public class CFOGSetting
                {
                    public int FogType { get; set; } //0x4()
                    public FogSuffixType FogSuffixTypes => GetFogType();

                    public float FogStart { get; set; } //0x4
                    public float FogEnd { get; set; } //0x4
                    public float Concentration { get; set; } //0x4

                    public enum FogSuffixType
                    {
                        LinearExponential = 1,
                        ExponentialFunctions = 2,
                        SquareOfExponentialFunctions = 3
                    }

                    public FogSuffixType GetFogType()
                    {
                        return (FogSuffixType)FogType;
                    }

                    public void ReadCFOGSetting(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        FogType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        FogStart = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        FogEnd = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        Concentration = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }

                    public CFOGSetting()
                    {
                        FogType = 0;
                        FogStart = 0;
                        FogEnd = 0;
                        Concentration = 0;
                    }
                }

                public CFOG()
                {
                    CFOG_Header = "CFOG".ToCharArray();
                    CFOG_UnknownBytes_2 = new List<byte>().ToArray();
                    CFOG_NameStringOffset = 0;
                    CFOG_NumOfUserDataDICTEntries = 0;
                    CFOG_UserDataDICTOffset = 0;
                    UserDataDict = new DICT();
                    FogFlipSettings = new FogFlipSetting(1);
                    CFOG_UnknownBytes_5 = new List<byte>().ToArray();
                    CFOG_UnknownBytes_6 = new List<byte>().ToArray();
                    CFOG_UnknownBytes_7 = new List<byte>().ToArray();
                    CFOG_UnknownBytes_8 = new List<byte>().ToArray();
                    CFOG_CFOGAnimation_DICTOffset = 0;
                    FogAnimationDICT = new DICT();
                    ColorDICT = new DICT();
                    Transform_Scale = new Transform.Scale();
                    Transform_Rotate = new Transform.Rotate();
                    Transform_Translate = new Transform.Translate();
                    CFOG_4x4_Matrix = new Matrix.LocalMatrix();
                    CFOG_4x4_Matrix_Transform = new Matrix.WorldMatrix_Transform();
                    Color_RGBA = new CFOG_RGBA(0, 0, 0, 0);
                    CFOG_UnknownOffset1 = 0;
                    Flag = new Flags(new List<byte>().ToArray());
                    UnknownUserData = new UserData();
                    FogSettingOffset = 0;
                    CFOGSettings = new CFOGSetting();
                }

                public void ReadCFOG(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    CFOG_Header = br.ReadChars(4);
                    if (new string(CFOG_Header) != "CFOG") throw new Exception("不明なフォーマットです");

                    CFOG_UnknownBytes_2 = endianConvert.Convert(br.ReadBytes(4));
                    CFOG_NameStringOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CFOG_NameStringOffset != 0)
					{
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(CFOG_NameStringOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    CFOG_NumOfUserDataDICTEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    CFOG_UserDataDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CFOG_UserDataDICTOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(CFOG_UserDataDICTOffset, SeekOrigin.Current);

                        UserDataDict.ReadDICT(br, BOM);

                        br.BaseStream.Position = Pos;
                    }

                    FogFlipSettings.Z_flip = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    CFOG_UnknownBytes_5 = endianConvert.Convert(br.ReadBytes(4));
                    CFOG_UnknownBytes_6 = endianConvert.Convert(br.ReadBytes(4));
                    CFOG_UnknownBytes_7 = endianConvert.Convert(br.ReadBytes(4));
                    CFOG_UnknownBytes_8 = endianConvert.Convert(br.ReadBytes(4)); //FogAnimCount (?)
                    CFOG_CFOGAnimation_DICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CFOG_CFOGAnimation_DICTOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(CFOG_CFOGAnimation_DICTOffset, SeekOrigin.Current);

                        FogAnimationDICT.ReadDICT(br, BOM);

                        #region ColorDICT
                        var SetPos = FogAnimationDICT.DICT_Entries.Last().CGFXData.UserData.RealNumber_Data.EndPos;
                        br.BaseStream.Position = SetPos;

                        ColorDICT.ReadDICT(br, BOM);
                        #endregion

                        br.BaseStream.Position = Pos;
                    }

                    Transform_Scale.ReadScale(br, BOM);
                    Transform_Rotate.ReadRotate(br, BOM);
                    Transform_Translate.ReadTranslate(br, BOM);
                    CFOG_4x4_Matrix.ReadLocalMatrix(br, BOM);
                    CFOG_4x4_Matrix_Transform.ReadMatrix4x4Transform(br, BOM);
                    Color_RGBA.ReadCFOGRGBA(br, BOM);

                    //UserDataType => Float(No DICT)
                    CFOG_UnknownOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CFOG_UnknownOffset1 != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(CFOG_UnknownOffset1, SeekOrigin.Current);

                        //UserData => Float
                        Flag = new Flags(br.ReadBytes(4));

                        UserData userData = new UserData(UserData.UserDataType.RealNumber);
                        userData.RealNumber_Data.ReadUserDataList(br, BOM, UserData.RealNumber.RealNumberType.Color);

                        UnknownUserData = userData;

                        br.BaseStream.Position = Pos;
                    }

                    FogSettingOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (FogSettingOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(FogSettingOffset, SeekOrigin.Current);

                        CFOGSettings.ReadCFOGSetting(br, BOM);

                        br.BaseStream.Position = Pos;
                    }
                }
            }

            //Environments
            public CENV CENVSection { get; set; }
            public class CENV
			{
                public string Name;
                public char[] CENV_Header { get; set; } //0x4

                //CENV_Name
                public int CENV_NumOfStrEntries { get; set; } //0x4
                public int CENV_StringNameOffset { get; set; } //0x4
                //public string CENV_Name { get; set; }


                //UserData
                public int CENV_NumOfUserDataDICTOffsetEntries { get; set; } //0x4
                public int CENV_UserDataDICTOffset { get; set; }  //0x4
                public DICT CENV_UserData_DICT { get; set; }

                //Camera
                public int CENV_NumOfCCAMOffsetEntries { get; set; }  //0x4
                public int CENV_CCAMOffset { get; set; }  //0x4
                //public List<CCAM> CCAMList { get; set; }
                //public class CENV_CCAMStringOffset
                //{
                //    public byte[] Offset { get; set; }
                //    //CCAM
                //}

                public List<CENV_CCAMName> CENV_CCAMName_List { get; set; }
                public class CENV_CCAMName
                {
                    public int CCAM_Number { get; set; } //0x4
                    public int CCAM_StringOffset { get; set; } //0x8
                    public string Name { get; set; }
                }

                //Light
                public byte[] CENV_NumOfLIGHTOffsetEntries { get; set; }  //0x4
                public byte[] CENV_LIGHTOffset { get; set; }  //0x4

                //public byte[] CENV_LIGHTGroupOffset { get; set; }

                public List<CENV_LIGHTNameGroup> CENV_LightNameGroup { get; set; }
                public class CENV_LIGHTNameGroup
                {
                    public byte[] CENV_LIGHTGroupOffset { get; set; }
                    public byte[] CENV_LIGHTNameOffsetCount { get; set; }
                    public byte[] CENV_LIGHTNameListFlag { get; set; } //0x4(04 00 00 00)

                    public byte[] LIGHTGroupNumber { get; set; }
                    public List<CENV_LIGHTName_List> CENV_LightName_List { get; set; }
                    public class CENV_LIGHTName_List
                    {
                        public int LIGHT_Number { get; set; }
                        public string LIGHT_Name { get; set; }
                    }
                }

                public void Read_CENV(BinaryReader br, byte[] BOM)
				{
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    CENV_Header = br.ReadChars(4);
                    if (new string(CENV_Header) != "CENV") throw new Exception("不明なフォーマットです");

                    CENV_NumOfStrEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    CENV_StringNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CENV_StringNameOffset != 0)
					{
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(CENV_StringNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    CENV_NumOfUserDataDICTOffsetEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    CENV_UserDataDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CENV_UserDataDICTOffset != 0)
					{
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(CENV_UserDataDICTOffset, SeekOrigin.Current);

                        CENV_UserData_DICT.ReadDICT(br, BOM);

                        br.BaseStream.Position = Pos;
                    }
                }

                public CENV()
				{
                    CENV_Header = "CENV".ToCharArray();
                    CENV_NumOfStrEntries = 0;
                    CENV_StringNameOffset = 0;

                    CENV_NumOfUserDataDICTOffsetEntries = 0;
                    CENV_UserDataDICTOffset = 0;
                    CENV_UserData_DICT = new DICT();
				}
			}


            //Skeleton Animations
            //Texture Animations
            //Visibility Animations
            //Camera Animations
            //Light Animations
            //Emitters
            //Unknown

            public CGFXSection()
			{
                CMDLSection = new CMDL();
                TXOBSection = new TXOB();
                CFOGSection = new CFOG();
                CENVSection = new CENV();
			}
        }

        public class Transform
        {
            public Scale TrScale { get; set; }
            public class Scale
            {
                public float Scale_X { get; set; }
                public float Scale_Y { get; set; }
                public float Scale_Z { get; set; }

                public Scale()
                {
                    Scale_X = 0;
                    Scale_Y = 0;
                    Scale_Z = 0;
                }

                public void ReadScale(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    Scale_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    Scale_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    Scale_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }
            }

            public Rotate TrRotate { get; set; }
            public class Rotate
            {
                public float Rotate_X { get; set; }
                public float Rotate_Y { get; set; }
                public float Rotate_Z { get; set; }

                public Rotate()
                {
                    Rotate_X = 0;
                    Rotate_Y = 0;
                    Rotate_Z = 0;
                }

                public void ReadRotate(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    Rotate_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    Rotate_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    Rotate_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }
            }

            public Translate TrTranslate { get; set; }
            public class Translate
            {
                public float Translate_X { get; set; }
                public float Translate_Y { get; set; }
                public float Translate_Z { get; set; }

                public Translate()
                {
                    Translate_X = 0;
                    Translate_Y = 0;
                    Translate_Z = 0;
                }

                public void ReadTranslate(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    Translate_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    Translate_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    Translate_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }
            }
        }

        public class Matrix
        {
            public class LocalMatrix
            {
                //4x4 Matrix(4byte)
                //m11, m12, m13, m14
                //m21, m22, m23, m24
                //m31, m32, m33, m34
                //m41, m42, m43, m44
                public float M11 { get; set; }
                public float M12 { get; set; }
                public float M13 { get; set; }
                public float M14 { get; set; }

                public float M21 { get; set; }
                public float M22 { get; set; }
                public float M23 { get; set; }
                public float M24 { get; set; }

                public float M31 { get; set; }
                public float M32 { get; set; }
                public float M33 { get; set; }
                public float M34 { get; set; }

                public float M41 { get; set; }
                public float M42 { get; set; }
                public float M43 { get; set; }
                public float M44 { get; set; }

                public LocalMatrix()
                {
                    M11 = 0;
                    M12 = 0;
                    M13 = 0;
                    M14 = 0;

                    M21 = 0;
                    M22 = 0;
                    M23 = 0;
                    M24 = 0;

                    M31 = 0;
                    M32 = 0;
                    M33 = 0;
                    M34 = 0;

                    M41 = 0;
                    M42 = 0;
                    M43 = 0;
                    M44 = 0;
                }

                public void ReadLocalMatrix(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    M11 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M12 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M13 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M14 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M21 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M22 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M23 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M24 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M31 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M32 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M33 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M34 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //M41 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //M42 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //M43 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //M44 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }
            }

            public class WorldMatrix_Transform
            {
                //4x4 Matrix[Transform](4byte)
                //m11, m12, m13, m14
                //m21, m22, m23, m24
                //m31, m32, m33, m34
                //m41, m42, m43, m44
                public float M11 { get; set; }
                public float M12 { get; set; }
                public float M13 { get; set; }
                public float M14 { get; set; }

                public float M21 { get; set; }
                public float M22 { get; set; }
                public float M23 { get; set; }
                public float M24 { get; set; }

                public float M31 { get; set; }
                public float M32 { get; set; }
                public float M33 { get; set; }
                public float M34 { get; set; }

                public float M41 { get; set; }
                public float M42 { get; set; }
                public float M43 { get; set; }
                public float M44 { get; set; }

                public WorldMatrix_Transform()
                {
                    M11 = 0;
                    M12 = 0;
                    M13 = 0;
                    M14 = 0;

                    M21 = 0;
                    M22 = 0;
                    M23 = 0;
                    M24 = 0;

                    M31 = 0;
                    M32 = 0;
                    M33 = 0;
                    M34 = 0;

                    M41 = 0;
                    M42 = 0;
                    M43 = 0;
                    M44 = 0;
                }

                public void ReadMatrix4x4Transform(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    M11 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M12 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M13 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M14 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M21 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M22 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M23 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M24 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M31 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M32 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M33 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M34 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //M41 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //M42 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //M43 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //M44 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }
            }


            public class Matrix_BoundingBox
            {
                public float M11 { get; set; }
                public float M12 { get; set; }
                public float M13 { get; set; }

                public float M21 { get; set; }
                public float M22 { get; set; }
                public float M23 { get; set; }

                public float M31 { get; set; }
                public float M32 { get; set; }
                public float M33 { get; set; }

                public Matrix_BoundingBox(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
                {
                    M11 = m11;
                    M12 = m12;
                    M13 = m13;

                    M21 = m21;
                    M22 = m22;
                    M23 = m23;

                    M31 = m31;
                    M32 = m32;
                    M33 = m33;
                }

                public void ReadBoundingBoxMatrix(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    M11 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M12 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M13 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M21 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M22 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M23 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M31 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M32 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    M33 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }
            }
        }

        public class SOBJ
		{
            //10 00 00 01 -> Shape
            //10 00 00 00 -> Mesh
            internal SOBJType Types;
            public enum SOBJType
            {
                Mesh = 0,
                Shape = 1
            }

            public Mesh Meshes { get; set; }
            public class Mesh
			{
                public string Name;
                public string MeshName;
                public string MeshNodeName;

                public char[] SOBJ_Header { get; set; }
                public int Revision { get; set; }
                public int SOBJNameOffset { get; set; }
                public int UnknownData2 { get; set; }
                public int UnknownOffset1 { get; set; } //Array (float (?))
                public int UnknownData4 { get; set; }
                public int ShapeIndex { get; set; }
                public int MaterialIndex { get; set; }
                public int OwnerModelOffset { get; set; }
                public bool IsVisible { get; set; }
                public byte RenderPriority { get; set; }
                public short MeshNodeVisibilityIndex { get; set; }

				public int Unknown1 { get; set; }
				public int Unknown2 { get; set; }
				public int Unknown3 { get; set; }
				public int Unknown4 { get; set; }
				public int Unknown5 { get; set; }
				public int Unknown6 { get; set; }
				public int Unknown7 { get; set; }
				public int Unknown8 { get; set; }
				public int Unknown9 { get; set; }
				public int Unknown10 { get; set; }
				public int Unknown11 { get; set; }
				public int Unknown12 { get; set; }
				public int Unknown13 { get; set; }
				public int Unknown14 { get; set; }
				public int Unknown15 { get; set; }
				public int Unknown16 { get; set; }
                public int Unknown17 { get; set; }
                public int MeshNameOffset { get; set; }
				public int Unknown19 { get; set; }

				public int MeshNodeNameOffset { get; set; }

                public UnknownDataSection2 unknownDataSection2 { get; set; }
                public class UnknownDataSection2
                {
                    public int UnknownData1 { get; set; }
                    public int UnknownData2 { get; set; }
                    public int UnknownData3 { get; set; }
                    public int UnknownData4 { get; set; }

                    public UnknownDataSection2()
                    {
                        UnknownData1 = 0;
                        UnknownData2 = 0;
                        UnknownData3 = 0;
                        UnknownData4 = 0;
                    }

                    public void Read_UnknownDataSection2(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }
                }

                public void Read_MeshData(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    SOBJ_Header = br.ReadChars(4);
                    if (new string(SOBJ_Header) != "SOBJ") throw new Exception("不明なフォーマットです");
                    Revision = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    SOBJNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (SOBJNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(SOBJNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    ShapeIndex = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    MaterialIndex = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    OwnerModelOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0); //This - ThisPosition
                    IsVisible = Converter.ByteToBoolean(br.ReadByte());
                    RenderPriority = br.ReadByte();
                    MeshNodeVisibilityIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);

					Unknown1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown6 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown7 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown8 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown9 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown10 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown11 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown12 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown13 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown14 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					Unknown15 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    Unknown16 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    Unknown17 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
					MeshNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (MeshNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(MeshNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        MeshName = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    Unknown19 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

					MeshNodeNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (MeshNodeNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(MeshNodeNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        MeshNodeName = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    unknownDataSection2.Read_UnknownDataSection2(br, BOM);
                }

                public Mesh()
				{
                    SOBJ_Header = "SOBJ".ToCharArray();
                    Revision = 0;
                    SOBJNameOffset = 0;
                    UnknownData2 = 0;
                    UnknownOffset1 = 0;
                    UnknownData4 = 0;
                    ShapeIndex = 0;
                    MaterialIndex = 0;
                    OwnerModelOffset = 0;
                    IsVisible = new bool();
                    RenderPriority = 0;
                    MeshNodeVisibilityIndex = 0;
					Unknown1 = 0;
					Unknown2 = 0;
					Unknown3 = 0;
					Unknown4 = 0;
					Unknown5 = 0;
					Unknown6 = 0;
					Unknown7 = 0;
					Unknown8 = 0;
					Unknown9 = 0;
					Unknown10 = 0;
					Unknown11 = 0;
					Unknown12 = 0;
					Unknown13 = 0;
					Unknown14 = 0;
					Unknown15 = 0;
					Unknown16 = 0;
                    Unknown17 = 0;
                    MeshNameOffset = 0;
					Unknown19 = 0;
					MeshNodeNameOffset = 0;
                    unknownDataSection2 = new UnknownDataSection2();
                }
            }

            public Shape Shapes { get; set; }
            public class Shape
			{
                public string Name;
                public char[] SOBJ_Header { get; set; }
                public int Revision { get; set; }
                public int SOBJNameOffset { get; set; }
                public int UnknownData2 { get; set; }
                public int UnknownData3 { get; set; }
                public int ShapeFlag { get; set; }
                public int OrientedBoundingBoxOffset { get; set; }
                public BoundingBox OrientedBoundingBox { get; set; }
                public class BoundingBox
				{
                    public Flags Flags;
                    public Vector3D Position { get; set; }
                    public Matrix.Matrix_BoundingBox Matrix_BoundingBox { get; set; }
                    public Vector3D Size { get; set; }

                    public void Read(BinaryReader br, byte[] BOM)
					{
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        Flags = new Flags(br.ReadBytes(4));
                        if (Flags.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x80 }) == true)
						{
                            Position = Converter.ByteArrayToVector3D(new byte[][] { endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)) });
                            Matrix_BoundingBox.ReadBoundingBoxMatrix(br, BOM);
                            Size = Converter.ByteArrayToVector3D(new byte[][] { endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)) });
                        }
					}

                    public BoundingBox(Flags flags)
					{
                        Flags = flags;
                        Position = new Vector3D(0, 0, 0);
                        Matrix_BoundingBox = new Matrix.Matrix_BoundingBox(1, 0, 0, 0, 1, 0, 0, 0, 1);
                        Size = new Vector3D(1, 1, 1);
					}
				}
                public Vector3D PositionOffset { get; set; }
                public int PrimitiveSetCount { get; set; }
                public int PrimitiveSetListOffset { get; set; }
                public List<PrimitiveSet> primitiveSets { get; set; }
                public class PrimitiveSet
				{
                    public int PrimitiveSetOffset { get; set; }
                    public int RelatedBoneCount { get; set; }
                    public int RelatedBoneListOffset { get; set; }
                    public List<int> RelatedBoneList { get; set; }
                    public int SkinningMode { get; set; }
                    public int PrimitiveCount { get; set; }
                    public int PrimitiveOffsetListOffset { get; set; }
                    public List<Primitive> Primitives { get; set; }
                    public class Primitive
					{
                        public int PrimitiveOffset { get; set; }
                        public int IndexStreamCount { get; set; }
                        public int IndexStreamOffsetListOffset { get; set; }
                        public List<IndexStreamCtr> IndexStreamCtrList { get; set; }
                        public class IndexStreamCtr
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
                                    foreach(var r in Faces)
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
                            public BoundingBox BoundingBox { get; set; }
                            
                            public void ReadIndexStreamCtr(BinaryReader br, byte[] BOM)
                            {
                                EndianConvert endianConvert = new EndianConvert(BOM);
                                IndexStreamOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                if (IndexStreamOffset != 0)
								{
                                    long Pos = br.BaseStream.Position;

                                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                                    //Move Offset
                                    br.BaseStream.Seek(IndexStreamOffset, SeekOrigin.Current);

                                    #region Read IndexStreamCtr
                                    Flags = new Flags(br.ReadBytes(4));
                                    PrimitiveMode = br.ReadByte();
                                    IsVisible = Converter.ByteToBoolean(br.ReadByte());
                                    UnknownData = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                                    FaceDataLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    FaceDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    if (FaceDataOffset != 0)
									{
                                        long Pos2 = br.BaseStream.Position;

                                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                                        //Move Offset
                                        br.BaseStream.Seek(FaceDataOffset, SeekOrigin.Current);

										if (Flags.IdentFlag[0] == 0x01)
										{
											int count = (int)(FaceDataLength / 3);
											for (int i = 0; i < count; i++)
											{
												Faces.Add(new Face(br.ReadByte(), br.ReadByte(), br.ReadByte()));
											}
										}
										if (Flags.IdentFlag[0] == 0x03)
										{
											int count = (int)(FaceDataLength / 2 / 3);
											for (int i = 0; i < count; i++)
											{
												var t0 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
												var t1 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
												var t2 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);

												Faces.Add(new Face(t0, t1, t2));
											}
										}

										br.BaseStream.Position = Pos2;
                                    }

                                    BufferObject = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    LocationFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    CommandCache = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    CommandCacheSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    LocationAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    MemoryArea = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    BoundingBoxOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    if (BoundingBoxOffset != 0)
									{
                                        long Pos3 = br.BaseStream.Position;

                                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                                        //Move Offset
                                        br.BaseStream.Seek(BoundingBoxOffset, SeekOrigin.Current);

                                        //Flags flags = new Flags(br.ReadBytes(4));
                                        BoundingBox boundingBox = new BoundingBox(new CGFX_Viewer.Flags(new List<byte>().ToArray()));
                                        boundingBox.Read(br, BOM);
                                        BoundingBox = boundingBox;

                                        br.BaseStream.Position = Pos3;
                                    }
                                    if (BoundingBoxOffset == 0) BoundingBox = null;
                                    #endregion

                                    br.BaseStream.Position = Pos;
                                }
                            }

                            public IndexStreamCtr()
							{
                                IndexStreamOffset = 0;
                                Flags = new Flags(new List<byte>().ToArray());
                                PrimitiveMode = 0;
                                IsVisible = new bool();
                                UnknownData = 0;
                                FaceDataLength = 0;
                                FaceDataOffset = 0;
                                Faces = new List<Face>();
                                BufferObject = 0;
                                LocationFlag = 0;
                                CommandCache = 0;
                                CommandCacheSize = 0;
                                LocationAddress = 0;
                                MemoryArea = 0;
                                BoundingBoxOffset = 0;
                                BoundingBox = new BoundingBox(new Flags(new List<byte>().ToArray()));
                            }
                        }
                        public int BufferObjectCount { get; set; }
                        public int BufferObjectListOffset { get; set; }
                        public List<int> BufferObjectList { get; set; }
                        public int Flags { get; set; }
                        public int CommandAllocator { get; set; }
                        //face info array
                        //unknown 2 array

                        public void ReadPrimitive(BinaryReader br, byte[] BOM)
						{
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            PrimitiveOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            IndexStreamCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            IndexStreamOffsetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (IndexStreamOffsetListOffset != 0)
							{
                                long Pos = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move Offset
                                br.BaseStream.Seek(IndexStreamOffsetListOffset, SeekOrigin.Current);

                                for (int i = 0; i < IndexStreamCount; i++)
								{
                                    IndexStreamCtr indexStreamCtr = new IndexStreamCtr();
                                    indexStreamCtr.ReadIndexStreamCtr(br, BOM);
                                    IndexStreamCtrList.Add(indexStreamCtr);
                                }

                                br.BaseStream.Position = Pos;
                            }

                            BufferObjectCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            BufferObjectListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (BufferObjectListOffset != 0)
							{
                                long Pos2 = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move NameOffset
                                br.BaseStream.Seek(BufferObjectListOffset, SeekOrigin.Current);

                                BinaryReadHelper binaryReadHelper = new BinaryReadHelper(br, BOM);
                                var s = binaryReadHelper.ReadArray<int>(BufferObjectCount, 4);
                                BufferObjectList = s.ToList();

                                br.BaseStream.Position = Pos2;
                            }

                            Flags = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            CommandAllocator = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        }

                        public Primitive()
						{
                            PrimitiveOffset = 0;
                            IndexStreamCount = 0;
                            IndexStreamOffsetListOffset = 0;
                            IndexStreamCtrList = new List<IndexStreamCtr>();
                            BufferObjectCount = 0;
                            BufferObjectListOffset = 0;
                            BufferObjectList = new List<int>();
                            Flags = 0;
                            CommandAllocator = 0;
						}
					}

                    public void ReadPrimitiveSet(BinaryReader br, byte[] BOM)
					{
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        PrimitiveSetOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (PrimitiveSetOffset != 0)
						{
                            long Pos = br.BaseStream.Position;

                            br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //Move Offset
                            br.BaseStream.Seek(PrimitiveSetOffset, SeekOrigin.Current);

                            #region Read RelatedBone
                            RelatedBoneCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            RelatedBoneListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (RelatedBoneListOffset != 0)
							{
                                long Pos2 = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move NameOffset
                                br.BaseStream.Seek(RelatedBoneListOffset, SeekOrigin.Current);

                                BinaryReadHelper binaryReadHelper = new BinaryReadHelper(br, BOM);
                                var s = binaryReadHelper.ReadArray<int>(RelatedBoneCount, 4);
                                RelatedBoneList = s.ToList();

                                br.BaseStream.Position = Pos2;
                            }

                            SkinningMode = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                            PrimitiveCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            PrimitiveOffsetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (PrimitiveOffsetListOffset != 0)
							{
                                long Pos2 = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move Offset
                                br.BaseStream.Seek(PrimitiveOffsetListOffset, SeekOrigin.Current);

                                for (int i = 0; i < PrimitiveCount; i++)
								{
                                    //Read Primitive
                                    Primitive primitive = new Primitive();
                                    primitive.ReadPrimitive(br, BOM);
                                    Primitives.Add(primitive);
                                }

                                br.BaseStream.Position = Pos2;
                            }
                            #endregion

                            br.BaseStream.Position = Pos;
                        }
					}

                    public PrimitiveSet()
					{
                        RelatedBoneCount = 0;
                        RelatedBoneListOffset = 0;
                        RelatedBoneList = new List<int>();
                        SkinningMode = 0;
                        PrimitiveCount = 0;
                        PrimitiveOffsetListOffset = 0;
                        Primitives = new List<Primitive>();
					}
                }
                public int BaseAddress { get; set; }
                public int VertexAttributeCount { get; set; }
                public int VertexAttributeOffsetListOffset { get; set; }
                public List<VertexAttribute> VertexAttributes { get; set; }
                public class VertexAttribute
				{
                    public int VertexAttributeOffset { get; set; }
                    public Flags Flag { get; set; }
                    public Stream Streams { get; set; }
                    public class Stream
                    {
                        public int BufferObject { get; set; }
                        public int LocationFlag { get; set; }
                        public int VertexStreamLength { get; set; }
                        public int VertexStreamOffset { get; set; }
                        public List<byte> VertexStreamList { get; set; }
                        public int LocationAddress { get; set; } //VertexStreamOffset seems to be used when this one is 0...
                        public int MemoryArea { get; set; }
                        public List<Polygon> PolygonList { get; set; }

                        public int UnknownData1 { get; set; }
                        public int UnknownData2 { get; set; }

                        public int VertexDataEntrySize { get; set; }
                        public int NrVertexStreams { get; set; }
                        public int VertexStreamsOffsetListOffset { get; set; }
                        public List<VertexStream> VertexStreams { get; set; }
                        public class VertexStream
						{
                            public int VertexStreamsOffset { get; set; }

                            public Flags Flags { get; set; }
                            //public int VertexAttributeUsageNum { get; set; }
                            //public CGFX_Viewer.VertexAttribute.Usage.UsageType VertexAttributeUsageFlag => (CGFX_Viewer.VertexAttribute.Usage.UsageType)VertexAttributeUsageNum;
                            //public int VertexAttributeFlagNum { get; set; }
                            //public CGFX_Viewer.VertexAttribute.Flag.FlagType VertexAttributeFlag => (CGFX_Viewer.VertexAttribute.Flag.FlagType)VertexAttributeFlagNum;
                            public CGFX_Viewer.VertexAttribute.Usage VertexAttributeUsageFlag { get; set; }
                            public CGFX_Viewer.VertexAttribute.Flag VertexAttributeFlag { get; set; }

                            public int BufferObject { get; set; }
                            public int LocationFlag { get; set; } //0x10
                            public int VertexStreamLength { get; set; }
                            public int VertexStreamOffset { get; set; }

                            public int LocationAddress { get; set; }
                            public int MemoryArea { get; set; } //0x20
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

                                public int GetFormatTypeLength()
                                {
                                    int n = -1;
                                    if (FormatTypes == FormatType.BYTE || FormatTypes == FormatType.UNSIGNED_BYTE) n = 1;
                                    if (FormatTypes == FormatType.SHORT) n = 2;
                                    if (FormatTypes == FormatType.FLOAT) n = 4;
                                    return n;
                                }

                                //public int FormatType { get; set; }
                                public int ComponentCount { get; set; } //For example XYZ = 3, ST = 2, RGBA = 4

                                public ComponentType ComponentTypeFlag => (ComponentType)ComponentCount;
                                public enum ComponentType
                                {
                                    ST = 2,
                                    XYZ = 3,
                                    RGBA = 4
                                }

                                //public List<float> Vs { get; set; }


                                public void ReadComponent(BinaryReader br, byte[] BOM)
                                {
                                    EndianConvert endianConvert = new EndianConvert(BOM);
                                    Flags = new Flags(br.ReadBytes(4));
                                    //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                }

                                public Component()
                                {
                                    Flags = new Flags(new List<byte>().ToArray());
                                    //FormatType = 0;
                                    ComponentCount = 0;
                                }
                            }

                            //public int FormatType { get; set; }
                            //public int ComponentCount { get; set; }
                            public float Scale { get; set; }
                            public int Offset { get; set; }

                            public void ReadVertexData(BinaryReader br, byte[] BOM)
                            {
                                EndianConvert endianConvert = new EndianConvert(BOM);
                                VertexStreamsOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                if (VertexStreamsOffset != 0)
                                {
                                    long Pos = br.BaseStream.Position;

                                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                                    //Move NameOffset
                                    br.BaseStream.Seek(VertexStreamsOffset, SeekOrigin.Current);

                                    Flags = new Flags(br.ReadBytes(4));
                                    //VertexAttributeUsageNum = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    //VertexAttributeFlagNum = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    VertexAttributeUsageFlag = new CGFX_Viewer.VertexAttribute.Usage(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));
                                    VertexAttributeFlag = new CGFX_Viewer.VertexAttribute.Flag(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));


                                    BufferObject = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    LocationFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    VertexStreamLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    VertexStreamOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    if (VertexStreamOffset != 0) return; //Unused(?)
                                    LocationAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    MemoryArea = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    Components.ReadComponent(br, BOM);
                                    //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    //ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    Scale = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                                    br.BaseStream.Position = Pos;
                                }
                            }

                            public VertexStream()
                            {
                                VertexStreamsOffset = 0;

                                Flags = new Flags(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                //VertexAttributeUsageNum = 0;
                                //VertexAttributeFlagNum = 0;
                                VertexAttributeUsageFlag = new CGFX_Viewer.VertexAttribute.Usage(-1);
                                VertexAttributeFlag = new CGFX_Viewer.VertexAttribute.Flag(-1);

                                BufferObject = 0;
                                LocationFlag = 0;
                                VertexStreamLength = 0;
                                VertexStreamOffset = 0;

                                LocationAddress = 0;
                                MemoryArea = 0;
                                Components = new Component();
                                //FormatType = 0;
                                //ComponentCount = 0;
                                Scale = 0;
                                Offset = 0;
                            }

                            #region cls
                            //public int BufferObject { get; set; }
                            //public int LocationFlag { get; set; }
                            //public int VertexStreamLength { get; set; }
                            //                     public int VertexStreamOffset { get; set; }
                            //                     public List<VertexData> VertexDatas { get; set; }
                            //                     public class VertexData
                            //{
                            //                         public int BufferObject { get; set; }
                            //                         public int LocationFlag { get; set; } //0x10
                            //                         public int VertexStreamLength { get; set; }
                            //                         public int VertexStreamOffset { get; set; }

                            //                         public int LocationAddress { get; set; }
                            //                         public int MemoryArea { get; set; } //0x20
                            //                         public Component Components { get; set; }
                            //                         public class Component
                            //	{
                            //                             public Flags Flags { get; set; }

                            //                             public FormatType FormatTypes
                            //		{
                            //			get => (FormatType)Flags.IdentFlag[0];
                            //			set => Flags.IdentFlag[0] = Convert.ToByte(Enum.ToObject(typeof(FormatType), value));
                            //		}
                            //		public enum FormatType
                            //		{
                            //                                 BYTE = 0,
                            //                                 UNSIGNED_BYTE = 1,
                            //                                 SHORT = 2,//might also be unsigned short
                            //                                 FLOAT = 6
                            //                             }

                            //                             //public int FormatType { get; set; }
                            //                             public int ComponentCount { get; set; } //For example XYZ = 3, ST = 2, RGBA = 4
                            //                             //public List<float> Vs { get; set; }


                            //                             public void ReadComponent(BinaryReader br, byte[] BOM)
                            //		{
                            //                                 EndianConvert endianConvert = new EndianConvert(BOM);
                            //                                 Flags = new Flags(br.ReadBytes(4));
                            //                                 //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                                 ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                             }

                            //                             public Component()
                            //		{
                            //                                 Flags = new Flags(new List<byte>().ToArray());
                            //                                 //FormatType = 0;
                            //                                 ComponentCount = 0;
                            //                             }
                            //                         }

                            //                         //public int FormatType { get; set; }
                            //                         //public int ComponentCount { get; set; }
                            //                         public float Scale { get; set; }
                            //                         public int Offset { get; set; }

                            //                         public void ReadVertexData(BinaryReader br, byte[] BOM)
                            //	{
                            //                             EndianConvert endianConvert = new EndianConvert(BOM);
                            //                             BufferObject = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                             LocationFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                             VertexStreamLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                             VertexStreamOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                             if (VertexStreamOffset != 0) return; //Unused(?)
                            //                             LocationAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                             MemoryArea = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                             Components.ReadComponent(br, BOM);
                            //                             //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                             //ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                             Scale = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                             Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         }

                            //                         public VertexData()
                            //	{
                            //                             BufferObject = 0;
                            //                             LocationFlag = 0;
                            //                             VertexStreamLength = 0;
                            //                             VertexStreamOffset = 0;

                            //                             LocationAddress = 0;
                            //                             MemoryArea = 0;
                            //                             Components = new Component();
                            //                             //FormatType = 0;
                            //                             //ComponentCount = 0;
                            //                             Scale = 0;
                            //                             Offset = 0;
                            //                         }
                            //                     }
                            //public int LocationAddress { get; set; }
                            //                     public int VertexDataEntrySize { get; set; }//Stride
                            //                     public int NrVertexStreams { get; set; } //Nr Attributes
                            //                     public int VertexStreamsOffsetArrayOffset { get; set; }

                            //                     public void ReadVertexStream(BinaryReader br, byte[] BOM)
                            //{
                            //                         EndianConvert endianConvert = new EndianConvert(BOM);
                            //                         BufferObject = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         LocationFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         VertexStreamLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         VertexStreamOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         if (VertexStreamOffset != 0)
                            //	{
                            //                             long Pos = br.BaseStream.Position;

                            //                             br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //                             //Move Offset
                            //                             br.BaseStream.Seek(VertexStreamOffset, SeekOrigin.Current);

                            //                             //for (int i = 0; i < VertexStreamLength; i++)

                            //                             br.BaseStream.Position = Pos;
                            //                         }
                            //                         LocationAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         VertexDataEntrySize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);//Stride
                            //                         NrVertexStreams = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);//Nr Attributes
                            //                         VertexStreamsOffsetArrayOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         if (VertexStreamsOffsetArrayOffset != 0)
                            //	{
                            //                             long Pos = br.BaseStream.Position;

                            //                             br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //                             //Move Offset
                            //                             br.BaseStream.Seek(VertexStreamsOffsetArrayOffset, SeekOrigin.Current);

                            //                             //for (int i = 0; i < NrVertexStreams; i++)
                            //                             //{

                            //                             //}

                            //                             br.BaseStream.Position = Pos;



                            //                         }

                            //                         //MemoryArea = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         //ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         //Scale = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                         //Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //                     }

                            //                     public VertexStream()
                            //{
                            //                         BufferObject = 0;
                            //                         LocationFlag = 0;
                            //                         VertexStreamLength = 0;
                            //                         VertexStreamOffset = 0;
                            //                         VertexDatas = new List<VertexData>();
                            //                         LocationAddress = 0;
                            //                         VertexDataEntrySize = 0;
                            //                         NrVertexStreams = 0;
                            //                         VertexStreamsOffsetArrayOffset = 0;

                            //                         //FormatType = 0;
                            //                         //ComponentCount = 0;
                            //                         //Scale = 0;
                            //                         //Offset = 0;
                            //                     }
                            #endregion
                        }

						public void ReadStream(BinaryReader br, byte[] BOM)
						{
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            BufferObject = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            LocationFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            VertexStreamLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            VertexStreamOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (VertexStreamOffset != 0)
							{

                                long Pos = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move NameOffset
                                br.BaseStream.Seek(VertexStreamOffset, SeekOrigin.Current);

                                BinaryReadHelper binaryReadHelper = new BinaryReadHelper(br, BOM);
                                var s = binaryReadHelper.ReadArray(VertexStreamLength);
                                VertexStreamList = s.ToList();

                                br.BaseStream.Position = Pos;
                            }

                            MemoryArea = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0); //DataOffset
                            LocationAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0); //DataSize
                            
                            long Pos1 = MemoryArea != 0 ? br.BaseStream.Position : 0;

                            #region Del
                            //if (MemoryArea != 0)
                            //{
                            //    long Pos = br.BaseStream.Position;

                            //    br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //    //Move NameOffset
                            //    br.BaseStream.Seek(MemoryArea, SeekOrigin.Current);

                            //    int PolygonCount = LocationAddress / 32; //Position, Normal, TextureCoordinate
                            //    for (int i = 0; i < PolygonCount; i++)
                            //    {
                            //        Polygon polygon = new Polygon()
                            //        {
                            //            Vertex = Converter.ByteArrayToPoint3D(new byte[][] { endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)) }),
                            //            Normal = Converter.ByteArrayToVector3D(new byte[][] { endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)) }),
                            //            TexCoord = new Polygon.TextureCoordinate(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0))
                            //        };

                            //        PolygonList.Add(polygon);
                            //    }

                            //    br.BaseStream.Position = Pos;
                            //}
                            #endregion

                            //8byte(?)
                            UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);


                            VertexDataEntrySize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            NrVertexStreams = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            VertexStreamsOffsetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (VertexStreamsOffsetListOffset != 0)
							{
                                long Pos = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move NameOffset
                                br.BaseStream.Seek(VertexStreamsOffsetListOffset, SeekOrigin.Current);

                                for (int i = 0; i < NrVertexStreams; i++)
								{
                                    VertexStream vertexStream = new VertexStream();
                                    vertexStream.ReadVertexData(br, BOM);
                                    VertexStreams.Add(vertexStream);
                                }

                                br.BaseStream.Position = Pos;
                            }

                            if (VertexStreamOffset == 0)
                            {
                                int AllComponentLength = 0;
                                foreach (var tr in VertexStreams) AllComponentLength += tr.Components.ComponentCount * tr.Components.GetFormatTypeLength();
                                var Count = MemoryArea / AllComponentLength;

                                br.BaseStream.Position = Pos1;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move NameOffset
                                br.BaseStream.Seek(LocationAddress, SeekOrigin.Current);

                                for (int iu = 0; iu < Count; iu++)
                                {
                                    Polygon polygon1 = new Polygon();

                                    foreach (var h in VertexStreams)
                                    {
                                        var g = h.VertexAttributeFlag;

                                        int CompCount = h.Components.GetFormatTypeLength();

                                        if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.Position)
                                        {
                                            polygon1.Vertex = Converter.ByteArrayToPoint3D(new byte[][] { br.ReadBytes(CompCount), br.ReadBytes(CompCount), br.ReadBytes(CompCount) });
                                        }
                                        else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.Normal)
                                        {
                                            polygon1.Normal = Converter.ByteArrayToVector3D(new byte[][] { br.ReadBytes(CompCount), br.ReadBytes(CompCount), br.ReadBytes(CompCount) });
                                        }
                                        else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.Tangent)
                                        {
                                            return;
                                        }
                                        else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.TextureCoordinate0)
                                        {
                                            polygon1.TexCoord = new Polygon.TextureCoordinate(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0));
                                        }
                                        else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.TextureCoordinate1)
                                        {
                                            polygon1.TexCoord2 = new Polygon.TextureCoordinate(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0));
                                        }
                                        else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.TextureCoordinate2)
                                        {
                                            polygon1.TexCoord3 = new Polygon.TextureCoordinate(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0));
                                        }
                                        else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.Color)
                                        {
                                            polygon1.ColorData = new Polygon.Color(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
                                        }
                                    }

                                    PolygonList.Add(polygon1);
                                }

                            }
                            if (VertexStreamOffset != 0)
                            {
                                var f = VertexStreamLength / VertexDataEntrySize;
                            }

                            ////int q = VertexStreamLength / VertexDataEntrySize;

                        }

                        public Stream()
						{
                            BufferObject = 0;
                            LocationFlag = 0;
                            VertexStreamLength = 0;
                            VertexStreamOffset = 0;
                            VertexStreamList = new List<byte>();
                            LocationAddress = 0;
                            MemoryArea = 0;
                            PolygonList = new List<Polygon>();

                            UnknownData1 = 0;
                            UnknownData2 = 0;

                            VertexDataEntrySize = 0;
                            NrVertexStreams = 0;
                            VertexStreamsOffsetListOffset = 0;
                            VertexStreams = new List<VertexStream>();
                        }
                    }

                    public Param Params { get; set; }
                    public class Param
                    {
                        public int FormatType { get; set; }
                        public int ComponentCount { get; set; } //For example XYZ = 3, ST = 2, RGBA = 4
                        public float Scale { get; set; }
                        public int AttributeCount { get; set; }
                        public int AttributeListOffset { get; set; }
                        public List<float> AttributeList { get; set; }

                        public void ReadParam(BinaryReader br, byte[] BOM)
						{
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            Scale = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                            AttributeCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            AttributeListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (AttributeListOffset != 0)
							{
                                long Pos = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move NameOffset
                                br.BaseStream.Seek(AttributeListOffset, SeekOrigin.Current);

                                BinaryReadHelper binaryReadHelper = new BinaryReadHelper(br, BOM);
                                var s = binaryReadHelper.ReadArray<float>(AttributeCount, 4);
                                AttributeList = s.ToList();

                                br.BaseStream.Position = Pos;


                            }
                        }

                        public Param()
						{
                            FormatType = 0;
                            ComponentCount = 0;
                            Scale = 0;
                            AttributeCount = 0;
                            AttributeListOffset = 0;
                            AttributeList = new List<float>();
                        }
                    }

                    public void ReadVertexAttribute(BinaryReader br, byte[] BOM)
					{
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        VertexAttributeOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (VertexAttributeOffset != 0)
						{
							long Pos = br.BaseStream.Position;

							br.BaseStream.Seek(-4, SeekOrigin.Current);

							//Move Offset
							br.BaseStream.Seek(VertexAttributeOffset, SeekOrigin.Current);

                            Flag = new Flags(br.ReadBytes(4));
                            if (Flag.IdentFlag.SequenceEqual(new byte[] { 0x02, 0x00, 0x00, 0x40 })) Streams.ReadStream(br, BOM);
                            if (Flag.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x80 })) Params.ReadParam(br, BOM);

                            br.BaseStream.Position = Pos;
						}
                    }

                    public VertexAttribute()
                    {
                        VertexAttributeOffset = 0;
                        Flag = new Flags(new List<byte>().ToArray());
                        Streams = new Stream();
                        Params = new Param();
                    }
                }
                public int BlendShapeOffset { get; set; }
                public BlendShape BlendShapes { get; set; }
                public class BlendShape
				{
                    public int UnknownData1 { get; set; }
                    public int UnknownData2 { get; set; }
                    public int UnknownData3 { get; set; }
                    public int UnknownData4 { get; set; }
                    public int UnknownData5 { get; set; }

                    public void ReadBlendShape(BinaryReader br, byte[] BOM)
					{
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }

                    public BlendShape()
					{
                        UnknownData1 = 0;
                        UnknownData2 = 0;
                        UnknownData3 = 0;
                        UnknownData4 = 0;
                        UnknownData5 = 0;
                    }
                }

                public void Read_ShapeData(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    SOBJ_Header = br.ReadChars(4);
                    if (new string(SOBJ_Header) != "SOBJ") throw new Exception("不明なフォーマットです");
                    Revision = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    SOBJNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (SOBJNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(SOBJNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    ShapeFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    OrientedBoundingBoxOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (OrientedBoundingBoxOffset != 0)
					{
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(OrientedBoundingBoxOffset, SeekOrigin.Current);

                        Flags flags = new Flags(br.ReadBytes(4));
                        BoundingBox boundingBox = new BoundingBox(flags);
                        boundingBox.Read(br, BOM);
                        OrientedBoundingBox = boundingBox;

                        br.BaseStream.Position = Pos;
                    }

                    PositionOffset = Converter.ByteArrayToVector3D(new byte[][] { endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)) });
                    PrimitiveSetCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    PrimitiveSetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (PrimitiveSetListOffset != 0)
					{
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move Offset
                        br.BaseStream.Seek(PrimitiveSetListOffset, SeekOrigin.Current);

                        //Read PrimitiveSet
                        for (int yi = 0; yi < PrimitiveSetCount; yi++)
                        {
                            PrimitiveSet primitiveSet = new PrimitiveSet();
                            primitiveSet.ReadPrimitiveSet(br, BOM);
                            primitiveSets.Add(primitiveSet);
                        }

                        br.BaseStream.Position = Pos;
                    }

                    BaseAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    VertexAttributeCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    VertexAttributeOffsetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (VertexAttributeOffsetListOffset != 0)
					{
						long Pos = br.BaseStream.Position;

						br.BaseStream.Seek(-4, SeekOrigin.Current);

						//Move Offset
						br.BaseStream.Seek(VertexAttributeOffsetListOffset, SeekOrigin.Current);

                        for (int yi = 0; yi < VertexAttributeCount; yi++)
                        {
                            //Read VertexAttribute
                            VertexAttribute vertexAttribute = new VertexAttribute();
                            vertexAttribute.ReadVertexAttribute(br, BOM);
                            VertexAttributes.Add(vertexAttribute);
                        }

						br.BaseStream.Position = Pos;
					}

                    BlendShapeOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (BlendShapeOffset != 0)
					{
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move Offset
                        br.BaseStream.Seek(BlendShapeOffset, SeekOrigin.Current);

                        BlendShapes.ReadBlendShape(br, BOM);

                        br.BaseStream.Position = Pos;
                    }
                }

                public Shape()
				{
                    SOBJ_Header = "SOBJ".ToCharArray();
                    Revision = 0;
                    SOBJNameOffset = 0;
                    Revision = 0;
                    SOBJNameOffset = 0;
                    UnknownData2 = 0;
                    UnknownData3 = 0;
                    ShapeFlag = 0;
                    OrientedBoundingBoxOffset = 0;
                    OrientedBoundingBox = new BoundingBox(new Flags(new List<byte>().ToArray()));
                    PositionOffset = new Vector3D(0, 0, 0);
                    PrimitiveSetCount = 0;
                    PrimitiveSetListOffset = 0;
                    primitiveSets = new List<PrimitiveSet>();
                    BaseAddress = 0;
                    VertexAttributeCount = 0;
                    VertexAttributeOffsetListOffset = 0;
                    VertexAttributes = new List<VertexAttribute>();
                    BlendShapeOffset = 0;
                    BlendShapes = new BlendShape();
                }

				public override string ToString()
				{
					return Name;
				}
			}

            public SOBJ(SOBJType Type)
			{
                Types = Type;
                if (Type == SOBJType.Mesh)
				{
                    Meshes = new Mesh();
                    Shapes = null;
				}
                if (Type == SOBJType.Shape)
				{
                    Meshes = null;
                    Shapes = new Shape();
				}
			}

            //public SOBJ(Mesh mesh = null, Shape shape = null)
			//{
            //    if (mesh == null && shape == null) throw new Exception("meshとshapeが両方とも空です");
            //    Meshes = mesh;
            //    Shapes = shape;
			//}

            public void Read_SOBJ(BinaryReader br, byte[] BOM)
			{
                if (Types == SOBJType.Mesh)
				{
                    //ReadMesh
                    Meshes.Read_MeshData(br, BOM);
				}
                if (Types == SOBJType.Shape)
				{
                    //ReadShape
                    Shapes.Read_ShapeData(br, BOM);
				}
			}
		}

        public class CGFXData
		{
            public byte[] IdentificationFlags { get; set; }
            public Flags Flag => new Flags(IdentificationFlags);
            public CGFXSection CGFXSectionData { get; set; }
            public UserData UserData { get; set; }
            public NativeDataSection NativeDataSections { get; set; }
            public class NativeDataSection
            {
                public CFOG CFOG_Native { get; set; }
                public class CFOG
                {
                    public ColorData Color_Data { get; set; }
                    public class ColorData
                    {
                        public long EndPos;

                        public int UnknownData1 { get; set; } //0x4
                        public int UnknownData2 { get; set; } //0x4
                        public int UnknownData3 { get; set; } //0x4
                        public int UnknownData4 { get; set; } //0x4
                        public int UnknownData5 { get; set; } //0x4
                        public int UnknownData6 { get; set; } //0x4
                        public int UnknownData7 { get; set; } //0x4
                        public int UnknownData8 { get; set; } //0x4

                        public ColorData()
                        {
                            UnknownData1 = 0;
                            UnknownData2 = 0;
                            UnknownData3 = 0;
                            UnknownData4 = 0;
                            UnknownData5 = 0;
                            UnknownData6 = 0;
                            UnknownData7 = 0;
                            UnknownData8 = 0;
                        }

                        public void ReadColorData(BinaryReader br, byte[] BOM)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData6 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData7 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData8 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                            EndPos = br.BaseStream.Position;
                        }
                    }

                    public CFOG()
                    {
                        Color_Data = new ColorData();
                    }
                }

                public NativeDataSection()
                {
                    CFOG_Native = new CFOG();
                }
            }

            public void Reader(BinaryReader br, byte[] BOM)
			{
                var f2_s0 = Flag.GetF2_S0();

                if (Flag.GetF3() == Flags.F3.t0)
                {
                    if (Flag.GetF2_S1() == Flags.F2_S1.t0)
                    {
                        //Section
                        if (Flag.GetF0_S0() == Flags.F0_S0.t1)
                        {
                            char[] ty = br.ReadChars(4);
                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                            if (new string(ty) == "CENV")
                            {
                                //CENV
                                CGFXSectionData.CENVSection.Read_CENV(br, BOM);
                            }
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t2) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t3) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t4) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t5) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t6) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t7) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t8) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t9) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t10) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t11) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t12) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t13) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t14) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t15) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t16) return;

                    }
                    if (Flag.GetF2_S1() == Flags.F2_S1.t4)
                    {
                        //Color (?)
                        NativeDataSection.CFOG.ColorData colorData = new NativeDataSection.CFOG.ColorData();
                        colorData.ReadColorData(br, BOM);
                        NativeDataSections.CFOG_Native.Color_Data = colorData;
                    }
                }
                if (Flag.GetF3() == Flags.F3.t1)
                {
                    if (Flag.GetF2_S1() == Flags.F2_S1.t0)
                    {
                        //Section
                        if (Flag.GetF0_S0() == Flags.F0_S0.t1)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0)
                            {
                                //SOBJ
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t2)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0)
                            {
                                //SOBJ
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t3) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t4) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t5) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t6) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t7) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t8) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t9) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t10) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t11) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t12) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t13) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t14) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t15) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t16) return;
                    }
                    if (Flag.GetF2_S1() == Flags.F2_S1.t4) return;
                }
                if (Flag.GetF3() == Flags.F3.t2)
                {
                    if (Flag.GetF2_S1() == Flags.F2_S1.t0)
                    {
                        //Section
                        if (Flag.GetF0_S0() == Flags.F0_S0.t1)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0)
                            {
                                //SOBJ
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t2) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t3) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t4) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t5) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t6) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t7) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t8) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t9) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t10) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t11) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t12) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t13) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t14) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t15) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t16) return;
                    }
                    if (Flag.GetF2_S1() == Flags.F2_S1.t4) return;
                }
                if (Flag.GetF3() == Flags.F3.t3)
                {
                    if (Flag.GetF2_S1() == Flags.F2_S1.t0)
                    {
                        //Section
                    }
                    if (Flag.GetF2_S1() == Flags.F2_S1.t4) return;
                }
                if (Flag.GetF3() == Flags.F3.t4)
                {
                    if (Flag.GetF2_S1() == Flags.F2_S1.t0)
                    {
                        //Section
                        if (Flag.GetF0_S0() == Flags.F0_S0.t1)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0)
                            {
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "MTOB")
                                {
                                    //MTOB
                                }
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t2) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t3) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t4) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t5) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t6) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t7) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t8) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t9) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t10) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t11) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t12) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t13) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t14) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t15) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t16) return;
                    }
                    if (Flag.GetF2_S1() == Flags.F2_S1.t4) return;
                }
                if (Flag.GetF3() == Flags.F3.t5)
                {
                    if (Flag.GetF2_S1() == Flags.F2_S1.t0)
                    {
                        //Section
                        if (Flag.GetF0_S0() == Flags.F0_S0.t1)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0)
                            {
                                //UserData(String)
                                UserData userData = new UserData(UserData.UserDataType.String);
                                userData.String_Data.ReadUserDataStringList(br, BOM);

                                UserData.Type = UserData.UserDataType.String;
                                UserData = userData;
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1)
                            {
                                //SOBJ
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t2) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t3) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t4) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t5) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t6) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t7) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t8) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t9) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t10) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t11) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t12) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t13) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t14) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t15) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t16) return;
                    }
                    if (Flag.GetF2_S1() == Flags.F2_S1.t4) return;
                }
                if (Flag.GetF3() == Flags.F3.t6)
                {
                    if (Flag.GetF2_S1() == Flags.F2_S1.t0)
                    {
                        //Section
                        if (Flag.GetF0_S0() == Flags.F0_S0.t1)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0)
                            {
                                //UserData(Int32)
                                UserData userData = new UserData(UserData.UserDataType.Int32);
                                userData.Int32_Data.ReadUserDataInt32List(br, BOM);

                                UserData.Type = UserData.UserDataType.Int32;
                                UserData = userData;
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4)
                            {
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "TXOB")
                                {
                                    //TXOB(Texture)
                                    CGFXSectionData.TXOBSection.ReadTXOB(br, new byte[] { 0xFF, 0xFE });
                                }
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t2)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1)
                            {
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "TXOB")
                                {
                                    //TXOB(Texture:Shader)
                                    CGFXSectionData.TXOBSection.ReadTXOB(br, new byte[] { 0xFF, 0xFE });
                                }
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t3) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t4) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t5) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t6) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t7) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t8) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t9) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t10) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t11) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t12) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t13) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t14) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t15) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t16) return;
                    }
                    if (Flag.GetF2_S1() == Flags.F2_S1.t4) return;
                }
                if (Flag.GetF3() == Flags.F3.t7)
                {
                    if (Flag.GetF2_S1() == Flags.F2_S1.t0)
                    {
                        //Section
                        if (Flag.GetF0_S0() == Flags.F0_S0.t1)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10)
                            {
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "CCAM")
                                {
                                    //CCAM
                                }
                            }
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t2)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2)
                            {
                                //CMDL(Skeletal)
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "CMDL")
                                {
                                    CGFXSectionData.CMDLSection.ReadCMDL(br, BOM);
                                }
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t3)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2)
                            {
                                //Type(?)
                                if (Flag.GetF1() == Flags.F1.t1)
                                {
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "CHLT")
                                    {
                                        //CHLT
                                    }
                                }
                                if (Flag.GetF1() == Flags.F1.t2)
                                {
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "CVLT")
                                    {
                                        //CVLT
                                    }
                                }
                                if (Flag.GetF1() == Flags.F1.t4)
                                {
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "CALT")
                                    {
                                        //CALT
                                    }
                                }
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t4) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t5)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2)
                            {
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "CFOG")
                                {
                                    //CFOG
                                    CGFXSectionData.CFOGSection.ReadCFOG(br, BOM);
                                }
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t6) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t7) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t8) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t9) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t10)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2)
                            {
                                //CMDL(Primitive)
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "CMDL")
                                {
                                    CGFXSectionData.CMDLSection.ReadCMDL(br, BOM);
                                }
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t11)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2)
                            {
                                //Type(?)
                                if (Flag.GetF1() == Flags.F1.t0)
                                {
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "CFLT")
                                    {
                                        //CFLT
                                    }
                                }
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t12) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t13) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t14) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t15) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t16) return;
                    }
                    if (Flag.GetF2_S1() == Flags.F2_S1.t4) return;
                }
                if (Flag.GetF3() == Flags.F3.t8)
                {
                    if (Flag.GetF2_S1() == Flags.F2_S1.t0)
                    {
                        //Section
                        if (Flag.GetF0_S0() == Flags.F0_S0.t1)
                        {
                            if (Flag.GetF0_S1() == Flags.F0_S1.t0)
                            {
                                //UserData(Float)
                                UserData userData = new UserData(UserData.UserDataType.RealNumber);
                                userData.RealNumber_Data.ReadUserDataList(br, BOM, UserData.RealNumber.RealNumberType.Float);

                                UserData.Type = UserData.UserDataType.RealNumber;
                                UserData = userData;
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t1)
                            {
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "SHDR")
                                {
                                    //SHDR
                                }
                            }
                            if (Flag.GetF0_S1() == Flags.F0_S1.t2) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t3) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t4) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t5) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t6) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t7) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t8) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t9) return;
                            if (Flag.GetF0_S1() == Flags.F0_S1.t10) return;
                        }
                        if (Flag.GetF0_S0() == Flags.F0_S0.t2) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t3) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t4) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t5) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t6) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t7) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t8) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t9) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t10) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t11) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t12) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t13) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t14) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t15) return;
                        if (Flag.GetF0_S0() == Flags.F0_S0.t16) return;
                    }
                    if (Flag.GetF2_S1() == Flags.F2_S1.t4) return;
                }
            }

            public CGFXData(byte[] IdentificationFlag)
			{
                IdentificationFlags = IdentificationFlag;
                CGFXSectionData = new CGFXSection();
                UserData = new UserData();
                NativeDataSections = new NativeDataSection();
			}
		}



        //public class CENV
        //{
        //    public byte[] CENV_Unknownbyte1 { get; set; }
        //    public char[] CENV_Header { get; set; } //0x4

        //    //CENV_Name
        //    public byte[] CENV_NumOfStrEntries { get; set; } //0x4
        //    public byte[] CENV_StringNameOffset { get; set; } //0x4
        //    public string CENV_Name { get; set; }


        //    //UserData
        //    public byte[] CENV_NumOfUserDataDICTOffsetEntries { get; set; } //0x4
        //    public byte[] CENV_UserDataDICTOffset { get; set; }  //0x4
        //    public DICT CENV_UserData_DICT { get; set; }

        //    //Camera
        //    public byte[] CENV_NumOfCCAMOffsetEntries { get; set; }  //0x4
        //    public byte[] CENV_CCAMOffset { get; set; }  //0x4
        //    public class CENV_CCAMStringOffset
        //    {
        //        public byte[] Offset { get; set; }
        //    }

        //    public List<CENV_CCAMName> CENV_CCAMName_List { get; set; }
        //    public class CENV_CCAMName
        //    {
        //        public byte[] CCAM_Number { get; set; } //0x4
        //        public byte[] CCAM_StringOffset { get; set; } //0x8
        //        public string Name { get; set; }
        //    }

        //    //Light
        //    public byte[] CENV_NumOfLIGHTOffsetEntries { get; set; }  //0x4
        //    public byte[] CENV_LIGHTOffset { get; set; }  //0x4

        //    //public byte[] CENV_LIGHTGroupOffset { get; set; }

        //    public List<CENV_LIGHTNameGroup> CENV_LightNameGroup { get; set; }
        //    public class CENV_LIGHTNameGroup
        //    {
        //        public byte[] CENV_LIGHTGroupOffset { get; set; }
        //        public byte[] CENV_LIGHTNameOffsetCount { get; set; }
        //        public byte[] CENV_LIGHTNameListFlag { get; set; } //0x4(04 00 00 00)

        //        public byte[] LIGHTGroupNumber { get; set; }
        //        public List<CENV_LIGHTName_List> CENV_LightName_List { get; set; }
        //        public class CENV_LIGHTName_List
        //        {
        //            public int LIGHT_Number { get; set; }
        //            public string LIGHT_Name { get; set; }
        //        }
        //    }


        //    //public Dictionary<byte[], CENV_LIGHTNameGroup> CENV_LightNameGroup { get; set; }
        //    //public class CENV_LIGHTNameGroup
        //    //{
        //    //    public

        //    //    public class CENV_LIGHTName_Lists
        //    //    {

        //    //    }
        //    //}





        //    //Fog
        //    public byte[] CENV_NumOfCFOGOffsetEntries { get; set; } //0x4
        //    public byte[] CENV_CFOGOffset { get; set; }

        //    //public DICT CENV_UserDataDICT { get; set; }


        //    public List<StringOffsetInfo_Offset> StringOffsetInfo_Offset_List { get; set; }

        //    public List<CENV_StringOffsetInfo> CENV_StringOffsetInfo_List { get; set; }
        //}

        //public class StringOffsetInfo_Offset
        //{
        //    public byte[] StringOffsetInfo_Offset_Value { get; set; } //0x4
        //}

        //public class CENV_StringOffsetInfo
        //{
        //    public byte[] CFOGNumber { get; set; } //0x4, 1～N
        //    public byte[] CFOGNameStrOffset { get; set; } //0x4, CFOGNumOffset = CFOGCount + 1

        //    public byte[] UnknownByte1 { get; set; } //0x4

        //    //FogNameStrings
        //    public string CFOG_Name { get; set; }
        //}

    }
}
