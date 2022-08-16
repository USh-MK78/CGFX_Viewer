using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer.PropertyGridForms.General.UserDataForm
{
	public class UserDataEntryPropertyGrid
	{
        [TypeConverter(typeof(CGFXPropertyGridSet.CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public StringData String_Data { get; set; }
        public class StringData
        {
            public string UDName { get; set; }

            //Flag : 00 00 00 10
            public byte[] Flag2 { get; set; } //0x4(Default Value : 02 00 00 00(?))
            public byte[] Flag3 { get; set; } //0x4(Default Value : 02 00 00 00(?))
            public int STRING_ValueCount { get; set; }

            public List<CGFXFormat.UserData.StringData.UserDataItem_String> UserDataStringList { get; set; }

            public StringData(CGFXFormat.UserData.StringData stringData)
            {
                UDName = stringData.UDName;
                //UserDataStringNameOffset = 0;
                Flag2 = stringData.Flag2; //Default Value : 02 00 00 00(?)
                Flag3 = stringData.Flag3; //Default Value : 02 00 00 00(?)
                STRING_ValueCount = stringData.STRING_ValueCount;
                UserDataStringList = stringData.UserDataItem_String_List;
            }
        }

        [TypeConverter(typeof(CGFXPropertyGridSet.CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public Int32Data Int32_Data { get; set; }
        public class Int32Data
        {
            public string UDName { get; set; }

            //Flag : 00 00 00 20
            public byte[] Flag2 { get; set; } //0x4(Default Value : 01 00 00 00(?))
            public int INT32_ValueCount { get; set; } //0x4

            public List<CGFXFormat.UserData.Int32Data.UserDataItem_INT32> UserDataInt32List { get; set; }

            public Int32Data(CGFXFormat.UserData.Int32Data int32Data)
            {
                UDName = int32Data.UDName;
                //UserDataInt32NameOffset = 0;
                Flag2 = int32Data.Flag2;
                INT32_ValueCount = int32Data.INT32_ValueCount;
                UserDataInt32List = int32Data.UserDataItem_Int32Data_List;
            }
        }

        [TypeConverter(typeof(CGFXPropertyGridSet.CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public RealNumber RealNumber_Data { get; set; }
        public class RealNumber
        {
            public string UDName { get; set; }
            public string UDName_Sub { get; set; }

            //Flag : 00 00 00 80
            public int REALNUMBERCount { get; set; } //0x4

            public int UnkData { get; set; }

            public List<CGFXFormat.UserData.RealNumber.UserDataItem_REALNUMBER> UserDataRealNumberList { get; set; }

            public RealNumber(CGFXFormat.UserData.RealNumber realNumber)
            {
                UDName = realNumber.UDName;
                UDName_Sub = realNumber.UDName_Sub;
                //RealNumberNameOffset = 0;
                //SubNameStringOffset = 0; //Using Fog Animation, ColorData, Default = 0
                REALNUMBERCount = realNumber.REALNUMBERCount;
                UserDataRealNumberList = realNumber.UserDataItem_RealNumber_List;
            }

            //public enum RealNumberType
            //{
            //    Float,
            //    Color
            //}
        }

        public UserDataEntryPropertyGrid(CGFXFormat.UserData userDataEntry)
        {
            if (userDataEntry.Type == CGFXFormat.UserData.UserDataType.String)
			{
                String_Data = new StringData(userDataEntry.String_Data);
			}
            if (userDataEntry.Type == CGFXFormat.UserData.UserDataType.Int32)
			{
                Int32_Data = new Int32Data(userDataEntry.Int32_Data);
			}
            if (userDataEntry.Type == CGFXFormat.UserData.UserDataType.RealNumber)
			{
                RealNumber_Data = new RealNumber(userDataEntry.RealNumber_Data);
			}
        }
    }
}
