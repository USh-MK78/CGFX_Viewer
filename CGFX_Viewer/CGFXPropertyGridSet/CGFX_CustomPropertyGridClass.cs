using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CGFX_Viewer.CGFXPropertyGridSet
{
	public class CGFX_CustomPropertyGridClass
	{
        public class CustomSortTypeConverter : TypeConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                PropertyDescriptorCollection PDC = TypeDescriptor.GetProperties(value, attributes);

                Type type = value.GetType();

                List<string> list = type.GetProperties().Select(x => x.Name).ToList();

                return PDC.Sort(list.ToArray());
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }

        public class CustomExpandableObjectSortTypeConverter : ExpandableObjectConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                PropertyDescriptorCollection PDC = TypeDescriptor.GetProperties(value, attributes);

                Type type = value.GetType();

                List<string> list = type.GetProperties().Select(x => x.Name).ToList();

                return PDC.Sort(list.ToArray());
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }

        //UserDataDictionary
        public class UserDataDictionaryEditor : UITypeEditor
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
                    Forms.General.UserDataForm.UserDataDictionaryForm form = new Forms.General.UserDataForm.UserDataDictionaryForm(value as List<CGFXFormat.UserData>);
                    form.ShowDialog();

                    value = form.userData_List;
                }
                return value; // can also replace the wrapper object here
            }
        }

        /// <summary>
        /// ColorView (PropertyGrid)
        /// </summary>
        public class CustomRGBAColorEditor : UITypeEditor
        {
            private IWindowsFormsEditorService _WinFormEditorService;

            //編集時にドロップダウンスタイルで表示
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                _WinFormEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (value != null)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Location = new Point(10, 25),
                        Width = 80,
                        Height = 80,
                        SizeMode = PictureBoxSizeMode.AutoSize,
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    Label LBL_ColorR = new Label { Location = new System.Drawing.Point(100, 20), Text = "R : " + ((Color)value).R, };
                    Label LBL_ColorG = new Label { Location = new System.Drawing.Point(100, 45), Text = "G : " + ((Color)value).G, };
                    Label LBL_ColorB = new Label { Location = new System.Drawing.Point(100, 70), Text = "B : " + ((Color)value).B, };
                    Label LBL_ColorA = new Label { Location = new System.Drawing.Point(100, 95), Text = "A : " + ((Color)value).A, };

                    Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            if (((Color)value).A != 0) bitmap.SetPixel(i, j, (Color)value);
                            if (((Color)value).A == 0) bitmap.SetPixel(i, j, Color.FromArgb(1, ((Color)value).R, ((Color)value).G, ((Color)value).B));
                        }
                    }

                    pictureBox.Image = bitmap;

                    //Add controls to the GroupBox to combine multiple controls into a single control.
                    GroupBox groupBox = new GroupBox();
                    groupBox.Width = 150;
                    groupBox.Height = 150;
                    groupBox.Controls.Add(pictureBox);
                    groupBox.Controls.Add(LBL_ColorR);
                    groupBox.Controls.Add(LBL_ColorG);
                    groupBox.Controls.Add(LBL_ColorB);
                    groupBox.Controls.Add(LBL_ColorA);

                    _WinFormEditorService.DropDownControl(groupBox);
                }

                return (Color)value;
            }
        }
    }
}
