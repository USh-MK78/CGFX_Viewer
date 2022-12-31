using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer.CGFXPropertyGridSet
{
	[TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
	public class TXOB_PropertyGrid
	{
        public string TXOBName { get; set; }
        public int UnknownData1 { get; set; }
        //public int TXOBNameOffset { get; set; }

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

        internal byte[] TexData = new List<byte>().ToArray();
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

        internal TXOB_PropertyGrid(CGFXFormat.CGFXSection.TXOB.Texture TXOBSection)
		{
            TXOBName = TXOBSection.Name;

            UnknownData1 = TXOBSection.UnknownData1;

            UnknownByte1 = TXOBSection.UnknownByte1;
            UnknownByte2 = TXOBSection.UnknownByte2;

            TextureHeight = TXOBSection.TextureHeight;
            TextureWidth = TXOBSection.TextureWidth;

            UnknownByte3 = TXOBSection.UnknownByte3;
            UnknownByte4 = TXOBSection.UnknownByte4;

            MipMapLevel = TXOBSection.MipMapLevel;

            UnknownByte5 = TXOBSection.UnknownByte5;
            UnknownByte6 = TXOBSection.UnknownByte6;

            TexFormat = TXOBSection.TexFormat;

            UnknownData2 = TXOBSection.UnknownData2;

            TextureHeight2 = TXOBSection.TextureHeight2;
            TextureWidth2 = TXOBSection.TextureWidth2;

            TextureDataSize = TXOBSection.TextureDataSize;

            TextureDataOffset = TXOBSection.TextureDataOffset;

            TexData = TXOBSection.TexData;

            UnknownByte7 = TXOBSection.UnknownByte7;
            UnknownByte8 = TXOBSection.UnknownByte8;
            UnknownByte9 = TXOBSection.UnknownByte9;
            UnknownByte10 = TXOBSection.UnknownByte10;
        }
    }
}
