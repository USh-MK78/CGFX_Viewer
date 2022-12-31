using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer.CGFXPropertyGridSet
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class MTOB_PropertyGrid
    {
        public string Name;

        //public char[] MTOB_Header { get; set; }
        public byte[] Revision { get; set; }
        //public int NameOffset { get; set; }

        public int UnknownData1 { get; set; }
        public int UnknownData2 { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public CGFXFormat.CGFXSection.MTOB.LightSetting LightSetting { get; set; }

        public int Value => LightSetting.Value;
        public bool IsFragmentLighting => LightSetting.IsFragmentLighting;
        public bool IsVertexLighting => LightSetting.IsVertexLighting;
        public bool IsHemiSphereLighting => LightSetting.IsHemiSphereLighting;

        public bool EnableOcclusion => LightSetting.EnableOcclusion;

        //[ReadOnly(VertexLightSetting)]

        //public int IsFragmentLighting { get; set; }
        public int UnknownData4 { get; set; }
        public int DrawingLayer { get; set; }

        //MaterialColor
        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public MaterialColor MaterialColors { get; set; } = new MaterialColor();
        public class MaterialColor
        {
            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Emission EmissionData { get; set; } = new Emission();
            public class Emission
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadEmissionColor(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Emission(CGFXFormat.CGFXSection.MTOB.MaterialColor.Emission emission)
                {
                    R = emission.R;
                    G = emission.G;
                    B = emission.B;
                    A = emission.A;
                }

                public Emission()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 0;
                }

                public override string ToString()
                {
                    return "Emission";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Ambient AmbientData { get; set; } = new Ambient();
            public class Ambient
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadAmbientColor(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Ambient(CGFXFormat.CGFXSection.MTOB.MaterialColor.Ambient ambient)
                {
                    R = ambient.R;
                    G = ambient.G;
                    B = ambient.B;
                    A = ambient.A;
                }

                public Ambient()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Ambient";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Diffuse DiffuseData { get; set; } = new Diffuse();
            public class Diffuse
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadDiffuseColor(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Diffuse(CGFXFormat.CGFXSection.MTOB.MaterialColor.Diffuse diffuse)
                {
                    R = diffuse.R;
                    G = diffuse.G;
                    B = diffuse.B;
                    A = diffuse.A;
                }

                public Diffuse()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Diffuse";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Speculer0 Speculer0Data { get; set; } = new Speculer0();
            public class Speculer0
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadSpeculer0Color(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Speculer0(CGFXFormat.CGFXSection.MTOB.MaterialColor.Speculer0 speculer0)
                {
                    R = speculer0.R;
                    G = speculer0.G;
                    B = speculer0.B;
                    A = speculer0.A;
                }

                public Speculer0()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Speculer0";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Speculer1 Speculer1Data { get; set; } = new Speculer1();
            public class Speculer1
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadSpeculer1Color(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Speculer1(CGFXFormat.CGFXSection.MTOB.MaterialColor.Speculer1 speculer1)
                {
                    R = speculer1.R;
                    G = speculer1.G;
                    B = speculer1.B;
                    A = speculer1.A;
                }

                public Speculer1()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Speculer1";
                }
            }

            //public void ReadMaterialColor(BinaryReader br, byte[] BOM)
            //{
            //    EndianConvert endianConvert = new EndianConvert(BOM);
            //    EmissionData.ReadEmissionColor(br, BOM);
            //    UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            //    AmbientData.ReadAmbientColor(br, BOM);
            //    UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            //    DiffuseData.ReadDiffuseColor(br, BOM);

            //    Speculer0Data.ReadSpeculer0Color(br, BOM);
            //    UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            //    Speculer1Data.ReadSpeculer1Color(br, BOM);
            //    UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            //}

            public MaterialColor(CGFXFormat.CGFXSection.MTOB.MaterialColor materialColor)
            {
                EmissionData = new Emission(materialColor.EmissionData);
                AmbientData = new Ambient(materialColor.AmbientData);
                DiffuseData = new Diffuse(materialColor.DiffuseData);
                Speculer0Data = new Speculer0(materialColor.Speculer0Data);
                Speculer1Data = new Speculer1(materialColor.Speculer1Data);
            }

            public MaterialColor()
            {
                EmissionData = new Emission();
                AmbientData = new Ambient();
                DiffuseData = new Diffuse();
                Speculer0Data = new Speculer0();
                Speculer1Data = new Speculer1();
            }

            public override string ToString()
            {
                return "Material Color";
            }
        }

        //ConstantColor
        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public ConstantColor ConstantColorData { get; set; } = new ConstantColor();
        public class ConstantColor
        {
            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant0 Constant0Data { get; set; } = new Constant0();
            public class Constant0
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadConstant0Color(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Constant0(CGFXFormat.CGFXSection.MTOB.ConstantColor.Constant0 constant0)
                {
                    R = constant0.R;
                    G = constant0.G;
                    B = constant0.B;
                    A = constant0.A;
                }

                public Constant0()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 0";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant1 Constant1Data { get; set; } = new Constant1();
            public class Constant1
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadConstant1Color(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Constant1(CGFXFormat.CGFXSection.MTOB.ConstantColor.Constant1 constant1)
                {
                    R = constant1.R;
                    G = constant1.G;
                    B = constant1.B;
                    A = constant1.A;
                }

                public Constant1()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 1";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant2 Constant2Data { get; set; } = new Constant2();
            public class Constant2
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadConstant2Color(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Constant2(CGFXFormat.CGFXSection.MTOB.ConstantColor.Constant2 constant2)
                {
                    R = constant2.R;
                    G = constant2.G;
                    B = constant2.B;
                    A = constant2.A;
                }

                public Constant2()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 2";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant3 Constant3Data { get; set; } = new Constant3();
            public class Constant3
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadConstant3Color(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Constant3(CGFXFormat.CGFXSection.MTOB.ConstantColor.Constant3 constant3)
                {
                    R = constant3.R;
                    G = constant3.G;
                    B = constant3.B;
                    A = constant3.A;
                }

                public Constant3()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 3";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant4 Constant4Data { get; set; } = new Constant4();
            public class Constant4
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadConstant4Color(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Constant4(CGFXFormat.CGFXSection.MTOB.ConstantColor.Constant4 constant4)
                {
                    R = constant4.R;
                    G = constant4.G;
                    B = constant4.B;
                    A = constant4.A;
                }

                public Constant4()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 4";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant5 Constant5Data { get; set; } = new Constant5();
            public class Constant5
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                //public void ReadConstant5Color(BinaryReader br, byte[] BOM)
                //{
                //    EndianConvert endianConvert = new EndianConvert(BOM);
                //    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //}

                public Constant5(CGFXFormat.CGFXSection.MTOB.ConstantColor.Constant5 constant5)
                {
                    R = constant5.R;
                    G = constant5.G;
                    B = constant5.B;
                    A = constant5.A;
                }

                public Constant5()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 5";
                }
            }

            //public void ReadConstantColor(BinaryReader br, byte[] BOM)
            //{
            //    Constant0Data.ReadConstant0Color(br, BOM);
            //    Constant1Data.ReadConstant1Color(br, BOM);
            //    Constant2Data.ReadConstant2Color(br, BOM);
            //    Constant3Data.ReadConstant3Color(br, BOM);
            //    Constant4Data.ReadConstant4Color(br, BOM);
            //    Constant5Data.ReadConstant5Color(br, BOM);
            //}

            public ConstantColor(CGFXFormat.CGFXSection.MTOB.ConstantColor constantColor)
            {
                Constant0Data = new Constant0(constantColor.Constant0Data);
                Constant1Data = new Constant1(constantColor.Constant1Data);
                Constant2Data = new Constant2(constantColor.Constant2Data);
                Constant3Data = new Constant3(constantColor.Constant3Data);
                Constant4Data = new Constant4(constantColor.Constant4Data);
                Constant5Data = new Constant5(constantColor.Constant5Data);
            }

            public ConstantColor()
            {
                Constant0Data = new Constant0();
                Constant1Data = new Constant1();
                Constant2Data = new Constant2();
                Constant3Data = new Constant3();
                Constant4Data = new Constant4();
                Constant5Data = new Constant5();
            }

            public override string ToString()
            {
                return "Constant Color";
            }
        }

        public byte[] UnknownData5 { get; set; }




        public MTOB_PropertyGrid(CGFXFormat.CGFXSection.MTOB MTOB)
        {
            Name = MTOB.Name;
            //MTOB_Header = "MTOB".ToArray();
            Revision = MTOB.Revision;
            //NameOffset = 0;
            UnknownData1 = MTOB.UnknownData1;
            UnknownData2 = MTOB.UnknownData2;
            LightSetting = MTOB.LightSettings;
            //IsFragmentLighting = MTOB.IsFragmentLighting;
            UnknownData4 = MTOB.UnknownData4;
            DrawingLayer = MTOB.DrawingLayer;

            MaterialColors = new MaterialColor(MTOB.MaterialColors);
            ConstantColorData = new ConstantColor(MTOB.ConstantColorData);

            UnknownData5 = MTOB.UnknownData5;

        }

        public MTOB_PropertyGrid()
        {
            Name = null;
            //MTOB_Header = "MTOB".ToArray();
            Revision = new List<byte>().ToArray();
            //NameOffset = 0;
            UnknownData1 = 0;
            UnknownData2 = 0;
            LightSetting = new CGFXFormat.CGFXSection.MTOB.LightSetting(0);
            //IsFragmentLighting = 0;
            UnknownData4 = 0;
            DrawingLayer = 0;

            MaterialColors = new MaterialColor();
            ConstantColorData = new ConstantColor();

            UnknownData5 = new List<byte>().ToArray();

        }

        public override string ToString()
        {
            return Name;
        }
    }
}
