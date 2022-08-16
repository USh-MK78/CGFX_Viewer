using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CGFX_Viewer
{
    public class EndianConvert
	{
        public enum Endian
		{
            BigEndian = 65534,
            LittleEndian = 65279
		}

        public byte[] BOM { get; set; }
        public Endian Endians => EndianCheck();

        public EndianConvert(byte[] InputBOM)
		{
            BOM = InputBOM;
		}

        public Endian EndianCheck()
		{
            bool LE = BOM.SequenceEqual(new byte[] { 0xFF, 0xFE });
            bool BE = BOM.SequenceEqual(new byte[] { 0xFE, 0xFF });

            Endian BOMSetting = Endian.BigEndian;

            if ((LE || BE) == true)
            {
                if (LE == true) BOMSetting = Endian.LittleEndian;
                if (BE == true) BOMSetting = Endian.BigEndian;
            }

            return BOMSetting;
        }

        public byte[] Convert(byte[] Input)
		{
            if (Endians == Endian.BigEndian)
			{
                return Input.Reverse().ToArray();
            }
            if (Endians == Endian.LittleEndian)
            {
                return Input;
            }

            return Input;
        }
	}

    public class ReadByteLine
    {
        public List<byte> charByteList { get; set; }

        public ReadByteLine(List<byte> Input)
        {
            charByteList = Input;
        }

        public ReadByteLine(List<char> Input)
        {
            charByteList = Input.Select(x => (byte)x).ToArray().ToList();
        }

        public void ReadByte(BinaryReader br, byte Split = 0x00)
        {
            //var br = br.BaseStream;
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                byte PickStr = br.ReadByte();
                charByteList.Add(PickStr);
                if (PickStr == Split)
                {
                    break;
                }
            }
        }

        public void ReadMultiByte(BinaryReader br, byte Split = 0x00)
        {
            //var br = br.BaseStream;
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                byte[] PickStr = br.ReadBytes(2);
                charByteList.Add(PickStr[0]);
                charByteList.Add(PickStr[1]);
                if (PickStr[0] == Split)
                {
                    break;
                }
            }
        }

        public void ReadByte(BinaryReader br, char Split = '\0')
        {
            //var br = br.BaseStream;
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                byte PickStr = br.ReadByte();
                charByteList.Add(PickStr);
                if (PickStr == Split)
                {
                    break;
                }
            }
        }

        public void WriteByte(BinaryWriter bw)
        {
            bw.Write(ConvertToCharArray());
        }

        public char[] ConvertToCharArray()
        {
            return charByteList.Select(x => (char)x).ToArray();
        }

        public int GetLength()
        {
            return charByteList.ToArray().Length;
        }
    }

    public class Flags
	{
        public byte[] IdentFlag { get; set; } //0x4

        public Flags(byte[] b)
		{
            IdentFlag = b;
		}

        public F0_S0 F0_S0Flag => GetF0_S0();
        public F0_S1 F0_S1Flag => GetF0_S1();

        /// <summary>
        /// Extract upper 4 bits
        /// </summary>
        /// <returns></returns>
        public F0_S0 GetF0_S0()
		{
            return (F0_S0)(IdentFlag[0] & 0xF0);
		}

        /// <summary>
        /// Extract lower 4 bits
        /// </summary>
        /// <returns></returns>
        public F0_S1 GetF0_S1()
        {
            return (F0_S1)(IdentFlag[0] & 0x0F);
        }

        // IdentFlag & 0x0F : 0xF2 & 0x0F = 0x02
        // IdentFlag & 0x0F : 0xF2 & 0xF0 = 0xF0

        //
        public enum F0_S0
		{
            t1 = 0x00, //None
            t2 = 0x10, //SimplificationSkeletalModel
            t3 = 0x20, //Light
            t4 = 0x30, 
            t5 = 0x40, //Fog
            t6 = 0x50,
            t7 = 0x60,
            t8 = 0x70,
            t9 = 0x80,
            t10 = 0x90, //Compress Material, Primitive Model
            t11 = 0xA0, //Light
            t12 = 0xB0,
            t13 = 0xC0,
            t14 = 0xD0,
            t15 = 0xE0,
            t16 = 0xF0
        }

        //DataType
        public enum F0_S1
		{
            t0 = 0x00, //None
            t1 = 0x01, //Texture
            t2 = 0x02, //3D_Data
            t3 = 0x03, //???
            t4 = 0x04, //Shader
            t5 = 0x05,
            t6 = 0x06,
            t7 = 0x07,
            t8 = 0x08,
            t9 = 0x09,
            t10 = 0x0A
        }


        public F1 F1Flag => GetF1();

        public F1 GetF1()
        {
            return (F1)IdentFlag[1];
        }

        public enum F1
        {
            t0 = 0x00, //Light(CFLT)
            t1 = 0x01, //Light(CHLT)
            t2 = 0x02, //Light(CVLT)
            t4 = 0x04 //Light(CALT)
        }

		//      public enum F1
		//{
		//          t0 = 0x00,
		//          t1 = 0x02,
		//          t2 = 0x04
		//}


		//public F2 F2Flag => GetF2();

		//public F2 GetF2()
		//{
		//	return (F2)IdentFlag[2];
		//}

		//public enum F2
		//{
		//	t1 = 0x00,
		//	t2 = 0x04,
		//	t3 = 0x80
		//}

		public F2_S0 F2_S0Flag => GetF2_S0();
		public F2_S1 F2_S1Flag => GetF2_S1();

		/// <summary>
		/// Extract upper 4 bits
		/// </summary>
		/// <returns></returns>
		public F2_S0 GetF2_S0()
		{
			return (F2_S0)(IdentFlag[2] & 0xF0);
		}

		/// <summary>
		/// Extract lower 4 bits
		/// </summary>
		/// <returns></returns>
		public F2_S1 GetF2_S1()
		{
			return (F2_S1)(IdentFlag[2] & 0x0F);
		}

		public enum F2_S0
		{
			t0 = 0x00,
			t8 = 0x80
		}

		public enum F2_S1
		{
			t0 = 0x00, //SectionData
			t4 = 0x04 //Color(?)
		}


		public F3 F3Flag => GetF3();

        public F3 GetF3()
		{
            return (F3)IdentFlag[3]; //IdentFlag[3] >> 1,  IdentFlag[3] << 1
        }

        public enum F3
		{
            t0 = 0x00, //SOBJ(Mesh)
            t1 = 0x01, //SOBJ(Shape)
            t2 = 0x02, //SOBJ(SK)
            t3 = 0x04, //LUT(?)
            t4 = 0x08, //Material
            t5 = 0x10, //[F0_S0=0, F0_S1=1] -> SOBJ(Vertex Shader[?]), [F0_S0=0, F0_S1=0] -> UserData(String)
            t6 = 0x20, //Texture, [F0_S0=0, F0_S1=0] -> UserData(Int32)
            t7 = 0x40, //3DScene
            t8 = 0x80 //Shader, [F0_S0=0, F0_S1=0] -> UserData(Int32)
        }
	}

    public class TaskHelper
	{
        public static Task<T> RunTask<T>(object obj)
		{
            Task<T> r = Task.Run(() => { return (T)obj; });
            return r;
		}

        public static Task<T> RunTask<T>(BinaryReader br, int Offset, object obj)
        {
            Task<T> r = Task.Run(() => 
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(Offset, SeekOrigin.Current);

                var output = (T)obj;

                br.BaseStream.Position = Pos;


                return output;
            });

            return r;
        }
    }

    public class Converter
	{
        public static bool ByteToBoolean(byte Input)
		{
            bool b = new bool();
            if (Input == 0) b = false;
            if (Input == 1) b = true;
            return b;
		}

        public static byte BooleanToByte(bool Input)
		{
            return Convert.ToByte(Input);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BVector3D"></param>
        /// <returns></returns>
        public static Vector3D ByteArrayToVector3D(byte[][] BVector3D)
        {
            double Value_X = BitConverter.ToSingle(BVector3D[0], 0);
            double Value_Y = BitConverter.ToSingle(BVector3D[1], 0);
            double Value_Z = BitConverter.ToSingle(BVector3D[2], 0);

            return new Vector3D(Value_X, Value_Y, Value_Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Vector3D"></param>
        /// <returns></returns>
        public static byte[][] Vector3DToByteArray(Vector3D Vector3D)
        {
            byte[] Byte_X = BitConverter.GetBytes(Convert.ToSingle(Vector3D.X));
            byte[] Byte_Y = BitConverter.GetBytes(Convert.ToSingle(Vector3D.Y));
            byte[] Byte_Z = BitConverter.GetBytes(Convert.ToSingle(Vector3D.Z));

            return new byte[][] { Byte_X, Byte_Y, Byte_Z };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BPoint3D"></param>
        /// <returns></returns>
        public static Point3D ByteArrayToPoint3D(byte[][] BPoint3D)
        {
            double Value_X = BitConverter.ToSingle(BPoint3D[0], 0);
            double Value_Y = BitConverter.ToSingle(BPoint3D[1], 0);
            double Value_Z = BitConverter.ToSingle(BPoint3D[2], 0);

            return new Point3D(Value_X, Value_Y, Value_Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point3D"></param>
        /// <returns></returns>
        public static byte[][] Point3DToByteArray(Point3D Point3D)
        {
            byte[] Byte_X = BitConverter.GetBytes(Convert.ToSingle(Point3D.X));
            byte[] Byte_Y = BitConverter.GetBytes(Convert.ToSingle(Point3D.Y));
            byte[] Byte_Z = BitConverter.GetBytes(Convert.ToSingle(Point3D.Z));

            return new byte[][] { Byte_X, Byte_Y, Byte_Z };
        }

        public enum ConvertType
        {
            Boolean,
            Char,
            Double,
            Int16,
            Int32,
            Int64,
            Single,
            UInt16,
            UInt32,
            UInt64
        }

        public static T CustomBitConverter<T>(byte[] byteAry, int startIndex, ConvertType convertType)
        {
            object obj = new object();
            if (convertType == ConvertType.Boolean) obj = BitConverter.ToBoolean(byteAry, startIndex);
            if (convertType == ConvertType.Char) obj = BitConverter.ToChar(byteAry, startIndex);
            if (convertType == ConvertType.Double) obj = BitConverter.ToDouble(byteAry, startIndex);
            if (convertType == ConvertType.Int16) obj = BitConverter.ToInt16(byteAry, startIndex);
            if (convertType == ConvertType.Int32) obj = BitConverter.ToInt32(byteAry, startIndex);
            if (convertType == ConvertType.Int64) obj = BitConverter.ToInt64(byteAry, startIndex);
            if (convertType == ConvertType.Single) obj = BitConverter.ToSingle(byteAry, startIndex);
            if (convertType == ConvertType.UInt16) obj = BitConverter.ToUInt16(byteAry, startIndex);
            if (convertType == ConvertType.UInt32) obj = BitConverter.ToUInt32(byteAry, startIndex);
            if (convertType == ConvertType.UInt64) obj = BitConverter.ToUInt64(byteAry, startIndex);
            return (T)obj;
        }

        public static T CustomBitConverter<T>(byte[] byteAry, int startIndex)
        {
            object obj = new object();
            if (typeof(T) == typeof(bool)) obj = BitConverter.ToBoolean(byteAry, startIndex);
            if (typeof(T) == typeof(char)) obj = BitConverter.ToChar(byteAry, startIndex);
            if (typeof(T) == typeof(double)) obj = BitConverter.ToDouble(byteAry, startIndex);
            if (typeof(T) == typeof(short)) obj = BitConverter.ToInt16(byteAry, startIndex);
            if (typeof(T) == typeof(int)) obj = BitConverter.ToInt32(byteAry, startIndex);
            if (typeof(T) == typeof(long)) obj = BitConverter.ToInt64(byteAry, startIndex);
            if (typeof(T) == typeof(float)) obj = BitConverter.ToSingle(byteAry, startIndex);
            if (typeof(T) == typeof(ushort)) obj = BitConverter.ToUInt16(byteAry, startIndex);
            if (typeof(T) == typeof(uint)) obj = BitConverter.ToUInt32(byteAry, startIndex);
            if (typeof(T) == typeof(ulong)) obj = BitConverter.ToUInt64(byteAry, startIndex);
            return (T)obj;
        }
    }

    public class BinaryReadHelper
	{
        public byte[] BOMs;
        public BinaryReader BR { get; set; }
        public T[] ReadArray<T>(int Count, int ByteLength)
		{
            T[] Ary = new T[Count];
            for (int i = 0; i < Count; i++) Ary[i] = Converter.CustomBitConverter<T>(BR.ReadBytes(ByteLength), 0);
            return Ary;
		}

        public byte[] ReadArray(int Count)
		{
            byte[] Ary = new byte[Count];
            for (int i = 0; i < Count; i++) Ary[i] = BR.ReadByte();
            return Ary;
        }

        public BinaryReadHelper(BinaryReader br, byte[] BOM)
		{
            BOMs = BOM;
            BR = br;
        }
	}

    public struct Face
    {
        public int f0 { get; set; }
        public int f1 { get; set; }
        public int f2 { get; set; }

        public Face(int i0, int i1, int i2)
        {
            this.f0 = i0;
            this.f1 = i1;
            this.f2 = i2;
        }
    }


    public class CGFXVector
	{

        public struct Vector3D<T>
        {
            public T X;
            public T Y;
            public T Z;

            public Vector3D(T x, T y, T z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }

            public Vector3D ToVector3D()
            {
                Vector3D vector3D = new Vector3D(0, 0, 0);
                if (typeof(T) == typeof(float))
                {
                    List<Vector3D<byte>> vector3Ds = new List<Vector3D<byte>>();
                }
                if (typeof(T) == typeof(double))
                {

                }

                return vector3D;
            }
        }

        //      public struct Vector3D<T>
        //{
        //          public T X;
        //          public T Y;
        //          public T Z;

        //          public Vector3D(T x, T y, T z)
        //	{
        //              this.X = x;
        //              this.Y = y;
        //              this.Z = z;
        //	}

        //          public Vector3D ToVector3D()
        //	{
        //              Vector3D vector3D = new Vector3D(0, 0, 0);
        //              if (typeof(T) == typeof(float))
        //		{

        //		}
        //              if (typeof(T) == typeof(double))
        //		{

        //		}

        //              return vector3D;
        //          }
        //}
    }

 //   public class CGFXHelper
	//{
 //       public class Polygon
	//	{
 //           public List<CGFXVector.Vector3D<sbyte>> Vs1 { get; set; }
 //           public List<CGFXVector.Vector3D<byte>> Vs2 { get; set; }
 //           public List<CGFXVector.Vector3D<short>> Vs3 { get; set; }
 //           public List<CGFXVector.Vector3D<float>> Vs4 { get; set; }

 //           public Polygon()
	//		{
 //               Vs1 = new List<CGFXVector.Vector3D<sbyte>>();
 //               Vs2 = new List<CGFXVector.Vector3D<byte>>();
 //               Vs3 = new List<CGFXVector.Vector3D<short>>();
 //               Vs4 = new List<CGFXVector.Vector3D<float>>();
 //           }
 //       }



 //       public static void d()
	//	{
 //           //if (Components.FormatTypes == Component.FormatType.BYTE)
 //           //{
 //           //    Vs1.Add();

 //           //}
 //           //if (Components.FormatTypes == Component.FormatType.UNSIGNED_BYTE)
 //           //{
 //           //    Vs2.Add();

 //           //}
 //           //if (Components.FormatTypes == Component.FormatType.SHORT)
 //           //{
 //           //    Vs3.Add();


 //           //}
 //           //if (Components.FormatTypes == Component.FormatType.FLOAT)
 //           //{
 //           //    Vs1.Add();

 //           //}
 //       }
 //   }

    //public class AnonymousTypeList<System.Type<T>> ; IList<System.Type<T>>

    //public class Polygon<T>
    //{
    //	public List<Face<T>> Faces { get; set; }
    //	public class Face<T>
    //	{
    //		public T f0 { get; set; }
    //		public T f1 { get; set; }
    //		public T f2 { get; set; }

    //		public Face(object i0, object i1, object i2)
    //		{
    //			if (typeof(T) == typeof(byte) || typeof(T) == typeof(short))
    //			{
    //				f0 = (T)i0;
    //				f1 = (T)i1;
    //				f2 = (T)i2;
    //			}

    //			//f0 = (T)i0;
    //			//f1 = (T)i1;
    //			//f2 = (T)i2;
    //		}
    //	}

    //	public Polygon()
    //	{
    //		Faces = new List<Face<T>>();
    //	}
    //}

    public class Polygon
    {
        //public Vector3D Position { get; set; }
        public Point3D Position { get; set; }
        public Vector3D Normal { get; set; }
        public TextureCoordinate TexCoord { get; set; }

        public struct TextureCoordinate
        {
            public double X;
            public double Y;

            public TextureCoordinate(double pX, double pY)
            {
                X = pX;
                Y = pY;
            }
        }

        public Polygon()
        {
            //Position = new Vector3D();
            Position = new Point3D();
            Normal = new Vector3D();
            TexCoord = new TextureCoordinate();
        }
    }
}
