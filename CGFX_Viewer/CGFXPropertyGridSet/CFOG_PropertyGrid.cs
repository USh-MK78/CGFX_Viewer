using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace CGFX_Viewer.CGFXPropertyGridSet
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class CFOG_PropertyGrid
	{
        public string Name { get; set; }

        public List<CGFXFormat.UserData> UserData_List = new List<CGFXFormat.UserData>();
        [Editor(typeof(CGFX_CustomPropertyGridClass.UserDataDictionaryEditor), typeof(UITypeEditor))]
        public List<CGFXFormat.UserData> userDataList { get => UserData_List; set => UserData_List = value; }


        public CGFXFormat.CGFXSection.CFOG.CFOGSetting.FogSuffixType FogSuffixType { get; set; }

        public float FogStart { get; set; }
        public float FogEnd { get; set; }
        public float Concentration { get; set; }

        public CGFXFormat.CGFXSection.CFOG.FogFlipSetting.FlipSetting FlipSetting { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public RGB RGB_Color { get; set; } = new RGB();
        public class RGB
		{
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
            public float A { get; set; }

            public Color ToColor()
			{
                var ColorR = (int)(R * 255);
                var ColorG = (int)(G * 255);
                var ColorB = (int)(B * 255);
                var ColorA = (int)(A * 255);

                return Color.FromArgb(ColorA, ColorR, ColorG, ColorB);
            }

            [Editor(typeof(CGFX_CustomPropertyGridClass.CustomRGBAColorEditor), typeof(UITypeEditor))]
            public Color RGBColor
			{
				get => ToColor();
				set
				{
					R = (float)Math.Round((value.R / 255F), 2, MidpointRounding.AwayFromZero);
					G = (float)Math.Round((value.G / 255F), 2, MidpointRounding.AwayFromZero);
					B = (float)Math.Round((value.B / 255F), 2, MidpointRounding.AwayFromZero);
					A = (float)Math.Round((value.A / 255F), 2, MidpointRounding.AwayFromZero);
				}
			}

			public RGB()
            {
                R = 0;
                G = 0;
                B = 0;
                A = 0;
            }

            public RGB(float InputR, float InputG, float InputB, float InputA)
			{
                R = InputR;
                G = InputG;
                B = InputB;
                A = InputA;
            }

			public override string ToString()
			{
				return "RGBA Color";
			}
		}

        internal CFOG_PropertyGrid(CGFXFormat.CGFXSection.CFOG CFOGData)
		{
            Name = CFOGData.Name;

            userDataList = CFOGData.UserDataDict.DICT_Entries.Select(x => x.CGFXData.UserData).ToList();


            FogSuffixType = CFOGData.CFOGSettings.FogSuffixTypes;
            FogStart = CFOGData.CFOGSettings.FogStart;
            FogEnd = CFOGData.CFOGSettings.FogEnd;
            Concentration = CFOGData.CFOGSettings.Concentration;

            FlipSetting = CFOGData.FogFlipSettings.FlipSettings;

            RGB_Color = new RGB(CFOGData.Color_RGBA.Color_R, CFOGData.Color_RGBA.Color_G, CFOGData.Color_RGBA.Color_B, CFOGData.Color_RGBA.Color_A);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
