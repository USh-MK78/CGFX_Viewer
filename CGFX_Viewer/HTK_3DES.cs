using HelixToolkit.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CGFX_Viewer
{
    public class HTK_3DES
    {
        public class TSRSystem
        {
            public Transform_Value GetTransform_Value { get; set; }

            /// <summary>
            /// Translate,Scale,Rotateの値を格納するクラス
            /// </summary>
            public class Transform_Value
            {
                public Translate Translate_Value { get; set; }
                public class Translate
                {
                    public double X { get; set; }
                    public double Y { get; set; }
                    public double Z { get; set; }

                    public Translate()
                    {
                        X = 0;
                        Y = 0;
                        Z = 0;
                    }

                    public Translate(Vector3D vector3D)
                    {
                        X = vector3D.X;
                        Y = vector3D.Y;
                        Z = vector3D.Z;
                    }

                    public Vector3D ToVector3D()
                    {
                        return new Vector3D(X, Y, Z);
                    }
                }

                public Scale Scale_Value { get; set; }
                public class Scale
                {
                    public double X { get; set; }
                    public double Y { get; set; }
                    public double Z { get; set; }

                    public Scale()
                    {
                        X = 0;
                        Y = 0;
                        Z = 0;
                    }

                    public Scale(Vector3D vector3D, double ScaleFactor = 1)
                    {
                        X = vector3D.X * ScaleFactor;
                        Y = vector3D.Y * ScaleFactor;
                        Z = vector3D.Z * ScaleFactor;
                    }

                    public Vector3D ToVector3D()
                    {
                        return new Vector3D(X, Y, Z);
                    }
                }

                public Rotate Rotate_Value { get; set; }
                public class Rotate
                {
                    public double X { get; set; }
                    public double Y { get; set; }
                    public double Z { get; set; }

                    public Rotate()
                    {
                        X = 0;
                        Y = 0;
                        Z = 0;
                    }

                    public Rotate(Vector3D vector3D)
                    {
                        X = vector3D.X;
                        Y = vector3D.Y;
                        Z = vector3D.Z;
                    }

                    public Vector3D ToVector3D()
                    {
                        return new Vector3D(X, Y, Z);
                    }
                }
            }

            /// <summary>
            /// objファイルを読み込み、ModelVisual3Dを返すメソッド
            /// </summary>
            /// <param name="Path">Model Path</param>
            /// <returns>ModelVisual3D</returns>
            public static ModelVisual3D OBJReader(string Path)
            {
                ModelVisual3D dv3D = new ModelVisual3D();
                ObjReader objRead = new ObjReader();
                dv3D.Content = objRead.Read(Path);

                #region delcode(?)
                //ObjReader objRead = new ObjReader();

                //SortingVisual3D sortingVisual3D = new SortingVisual3D
                //{
                //    Method = SortingMethod.BoundingSphereSurface,
                //    SortingFrequency = 2,
                //    Content = objRead.Read(Path)
                //};

                //ModelVisual3D dv3D = sortingVisual3D;
                #endregion

                return dv3D;
            }

            /// <summary>
            /// ガベージコレクション
            /// </summary>
            public static void GC_Dispose(object f)
            {
                int GCNum = GC.GetGeneration(f);

                GC.Collect(GCNum);
                GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            /// <summary>
            /// ModelVisual3Dに文字列を関連付けて新しいModelVisual3Dを生成する
            /// </summary>
            /// <param name="MV3D">Input ModelVisual3D</param>
            /// <param name="InputString">Input String</param>
            /// <returns></returns>
            public static ModelVisual3D SetStringAndNewMV3D(ModelVisual3D MV3D, string InputString)
            {
                MV3D.SetName(InputString);
                return MV3D;
            }

            /// <summary>
            /// ModelVisual3Dに文字列を関連付ける
            /// </summary>
            /// <param name="MV3D">Input ModelVisual3D</param>
            /// <param name="InputString">Input String</param>
            public static void SetString_MV3D(ModelVisual3D MV3D, string InputString)
            {
                MV3D.SetName(InputString);
            }

            public class Transform
            {
                public Vector3D Rotate3D { get; set; }
                public Vector3D Scale3D { get; set; }
                public Vector3D Translate3D { get; set; }
            }

            /// <summary>
            /// Radianを角度に変換
            /// </summary>
            /// <param name="Radian"></param>
            /// <returns></returns>
            public static float RadianToAngle(double Radian)
            {
                return (float)(Radian * (180 / Math.PI));
            }

            /// <summary>
            /// 角度をRadianに変換
            /// </summary>
            /// <param name="Angle"></param>
            /// <returns></returns>
            public static double AngleToRadian(double Angle)
            {
                return (float)(Angle * (Math.PI / 180));
            }

            public static Vector3D RadianToAngleVector3D(Vector3D vector3D)
            {
                return new Vector3D((float)(vector3D.X * (180 / Math.PI)), (float)(vector3D.Y * (180 / Math.PI)), (float)(vector3D.Z * (180 / Math.PI)));
            }

            public static Vector3D AngleToRadianVector3D(Vector3D vector3D)
            {
                return new Vector3D((float)(vector3D.X * (Math.PI / 180)), (float)(vector3D.Y * (Math.PI / 180)), (float)(vector3D.Z * (Math.PI / 180)));
            }

            public static Point3D CalculateModelCenterPoint(ModelVisual3D MV3D)
            {
                Rect3D r = MV3D.Content.Bounds;
                double cX = r.X + r.SizeX / 2;
                double cY = r.Y + r.SizeY / 2;
                double cZ = r.Z + r.SizeZ / 2;
                Point3D P3 = new Point3D(cX, cY, cZ);

                return P3;
            }

            public static Point3D CalculateModelCenterPoint(Model3D MV3D)
            {
                Rect3D r = MV3D.Bounds;
                double cX = r.X + r.SizeX / 2;
                double cY = r.Y + r.SizeY / 2;
                double cZ = r.Z + r.SizeZ / 2;
                Point3D P3 = new Point3D(cX, cY, cZ);

                return P3;
            }

            public static Vector3D Scalefactor(Vector3D v, double Factor)
            {
                return new Vector3D(v.X / Factor, v.Y / Factor, v.Z / Factor);
            }

            public enum RotationSetting
            {
                Angle,
                Radian
            }

            public class TransformSetting
            {
                public bool IsContent { get; set; } = true;
                public ModelVisual3D InputMV3D { get; set; } = null;
                public Model3D InputM3D
                {
                    set
                    {
                        if (InputMV3D == null) this.InputM3D = value;
                        if (InputMV3D != null)
                        {
                            if (IsContent == true) this.InputM3D = InputMV3D.Content;
                            if (IsContent == false) this.InputM3D = null; //return
                        }
                    }
                    get
                    {
                        Model3D model3D = null;
                        if (InputMV3D == null) model3D = this.InputM3D;
                        if (InputMV3D != null)
                        {
                            if (IsContent == true) model3D = InputMV3D.Content;
                            if (IsContent == false) model3D = null;
                        }

                        return model3D;
                    }
                }

                public RotationSetting RotationSetting { get; set; } = RotationSetting.Angle;

                public ScaleTransformSetting ScaleTransformSettings { get; set; } = new ScaleTransformSetting();
                public class ScaleTransformSetting
                {
                    public Point3D ScaleCenter { get; set; } = new Point3D(0, 0, 0);
                    public double Scalefactor { get; set; } = 1;

                    public ScaleTransformSetting()
                    {
                        ScaleCenter = new Point3D(0, 0, 0);
                        Scalefactor = 2;
                    }
                }

                public RotationCenterSetting RotationCenterSettings { get; set; } = new RotationCenterSetting();
                public class RotationCenterSetting
                {
                    public Vector3D RotationX { get; set; } = new Vector3D(1, 0, 0);
                    public Vector3D RotationY { get; set; } = new Vector3D(0, 1, 0);
                    public Vector3D RotationZ { get; set; } = new Vector3D(0, 0, 1);

                    public RotationCenterSetting()
                    {
                        RotationX = new Vector3D(1, 0, 0);
                        RotationY = new Vector3D(0, 1, 0);
                        RotationZ = new Vector3D(0, 0, 1);
                    }
                }
            }

            public static void New_TransformSystem3D(Transform transform, TransformSetting transformSetting)
            {
                double RotateX = new double();
                double RotateY = new double();
                double RotateZ = new double();

                if (transformSetting.IsContent == true)
                {
                    if (transformSetting.RotationSetting == RotationSetting.Angle)
                    {
                        RotateX = transform.Rotate3D.X;
                        RotateY = transform.Rotate3D.Y;
                        RotateZ = transform.Rotate3D.Z;
                    }
                    if (transformSetting.RotationSetting == RotationSetting.Radian)
                    {
                        RotateX = RadianToAngle(transform.Rotate3D.X);
                        RotateY = RadianToAngle(transform.Rotate3D.Y);
                        RotateZ = RadianToAngle(transform.Rotate3D.Z);
                    }

                    var Rotate3D_X = new RotateTransform3D();
                    Rotate3D_X.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationX, RotateX));

                    var Rotate3D_Y = new RotateTransform3D();
                    Rotate3D_Y.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationY, RotateY));

                    var Rotate3D_Z = new RotateTransform3D();
                    Rotate3D_Z.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationZ, RotateZ));

                    var Scale3D = new ScaleTransform3D(Scalefactor(transform.Scale3D, transformSetting.ScaleTransformSettings.Scalefactor));
                    var Translate3D = new TranslateTransform3D(transform.Translate3D);

                    Transform3DCollection T3D_Collection = new Transform3DCollection();
                    T3D_Collection.Add(Scale3D);
                    T3D_Collection.Add(Rotate3D_X);
                    T3D_Collection.Add(Rotate3D_Y);
                    T3D_Collection.Add(Rotate3D_Z);
                    T3D_Collection.Add(Translate3D);

                    Transform3DGroup T3DGroup = new Transform3DGroup { Children = T3D_Collection };
                    transformSetting.InputM3D.Transform = T3DGroup;
                }
                if (transformSetting.IsContent == false)
                {
                    if (transformSetting.RotationSetting == RotationSetting.Angle)
                    {
                        RotateX = transform.Rotate3D.X;
                        RotateY = transform.Rotate3D.Y;
                        RotateZ = transform.Rotate3D.Z;
                    }
                    if (transformSetting.RotationSetting == RotationSetting.Radian)
                    {
                        RotateX = RadianToAngle(transform.Rotate3D.X);
                        RotateY = RadianToAngle(transform.Rotate3D.Y);
                        RotateZ = RadianToAngle(transform.Rotate3D.Z);
                    }

                    //Model3D Model = MV3D.Content;
                    var Rotate3D_X = new RotateTransform3D();
                    Rotate3D_X.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationX, RotateX));

                    var Rotate3D_Y = new RotateTransform3D();
                    Rotate3D_Y.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationY, RotateY));

                    var Rotate3D_Z = new RotateTransform3D();
                    Rotate3D_Z.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationZ, RotateZ));

                    //CalculateModelCenterPoint(Model)
                    var Scale3D = new ScaleTransform3D(Scalefactor(transform.Scale3D, transformSetting.ScaleTransformSettings.Scalefactor), transformSetting.ScaleTransformSettings.ScaleCenter);
                    var Translate3D = new TranslateTransform3D(transform.Translate3D);

                    Transform3DCollection T3D_Collection = new Transform3DCollection();
                    T3D_Collection.Add(Scale3D);
                    T3D_Collection.Add(Rotate3D_X);
                    T3D_Collection.Add(Rotate3D_Y);
                    T3D_Collection.Add(Rotate3D_Z);
                    T3D_Collection.Add(Translate3D);

                    Transform3DGroup T3DGroup = new Transform3DGroup { Children = T3D_Collection };
                    transformSetting.InputMV3D.Transform = T3DGroup;
                }
            }

            public static void New_TransformSystem3D(Transform_Value transform, TransformSetting transformSetting)
            {
                double RotateX = new double();
                double RotateY = new double();
                double RotateZ = new double();

                if (transformSetting.IsContent == true)
                {
                    if (transformSetting.RotationSetting == RotationSetting.Angle)
                    {
                        RotateX = transform.Rotate_Value.X;
                        RotateY = transform.Rotate_Value.Y;
                        RotateZ = transform.Rotate_Value.Z;
                    }
                    if (transformSetting.RotationSetting == RotationSetting.Radian)
                    {
                        RotateX = RadianToAngle(transform.Rotate_Value.X);
                        RotateY = RadianToAngle(transform.Rotate_Value.Y);
                        RotateZ = RadianToAngle(transform.Rotate_Value.Z);
                    }

                    var Rotate3D_X = new RotateTransform3D();
                    Rotate3D_X.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationX, RotateX));

                    var Rotate3D_Y = new RotateTransform3D();
                    Rotate3D_Y.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationY, RotateY));

                    var Rotate3D_Z = new RotateTransform3D();
                    Rotate3D_Z.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationZ, RotateZ));

                    var Scale = Scalefactor(new Vector3D(transform.Scale_Value.X, transform.Scale_Value.Y, transform.Scale_Value.Z), transformSetting.ScaleTransformSettings.Scalefactor);
                    var Scale3D = new ScaleTransform3D(Scale);
                    var Translate3D = new TranslateTransform3D(transform.Translate_Value.X, transform.Translate_Value.Y, transform.Translate_Value.Z);

                    Transform3DCollection T3D_Collection = new Transform3DCollection();
                    T3D_Collection.Add(Scale3D);
                    T3D_Collection.Add(Rotate3D_X);
                    T3D_Collection.Add(Rotate3D_Y);
                    T3D_Collection.Add(Rotate3D_Z);
                    T3D_Collection.Add(Translate3D);

                    Transform3DGroup T3DGroup = new Transform3DGroup { Children = T3D_Collection };
                    transformSetting.InputM3D.Transform = T3DGroup;
                }
                if (transformSetting.IsContent == false)
                {
                    if (transformSetting.RotationSetting == RotationSetting.Angle)
                    {
                        RotateX = transform.Rotate_Value.X;
                        RotateY = transform.Rotate_Value.Y;
                        RotateZ = transform.Rotate_Value.Z;
                    }
                    if (transformSetting.RotationSetting == RotationSetting.Radian)
                    {
                        RotateX = RadianToAngle(transform.Rotate_Value.X);
                        RotateY = RadianToAngle(transform.Rotate_Value.Y);
                        RotateZ = RadianToAngle(transform.Rotate_Value.Z);
                    }

                    var Rotate3D_X = new RotateTransform3D();
                    Rotate3D_X.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationX, RotateX));

                    var Rotate3D_Y = new RotateTransform3D();
                    Rotate3D_Y.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationY, RotateY));

                    var Rotate3D_Z = new RotateTransform3D();
                    Rotate3D_Z.Rotation = new QuaternionRotation3D(new Quaternion(transformSetting.RotationCenterSettings.RotationZ, RotateZ));

                    //CalculateModelCenterPoint(Model)
                    var Scale = Scalefactor(new Vector3D(transform.Scale_Value.X, transform.Scale_Value.Y, transform.Scale_Value.Z), transformSetting.ScaleTransformSettings.Scalefactor);
                    var Scale3D = new ScaleTransform3D(Scale, transformSetting.ScaleTransformSettings.ScaleCenter);
                    var Translate3D = new TranslateTransform3D(transform.Translate_Value.X, transform.Translate_Value.Y, transform.Translate_Value.Z);

                    Transform3DCollection T3D_Collection = new Transform3DCollection();
                    T3D_Collection.Add(Scale3D);
                    T3D_Collection.Add(Rotate3D_X);
                    T3D_Collection.Add(Rotate3D_Y);
                    T3D_Collection.Add(Rotate3D_Z);
                    T3D_Collection.Add(Translate3D);

                    Transform3DGroup T3DGroup = new Transform3DGroup { Children = T3D_Collection };
                    transformSetting.InputMV3D.Transform = T3DGroup;
                }
            }
        }

        public class Line3DSystem : TSRSystem
        {
            /// <summary>
            /// Point3Dの値を格納するクラス
            /// </summary>
            public class DrawLine_Value
            {
                public Start_Point3D StartPoint3D { get; set; }
                public class Start_Point3D
                {
                    public double X { get; set; }
                    public double Y { get; set; }
                    public double Z { get; set; }
                }

                public End_Point3D EndPoint3D { get; set; }
                public class End_Point3D
                {
                    public double X { get; set; }
                    public double Y { get; set; }
                    public double Z { get; set; }
                }
            }

            /// <summary>
            /// List<DrawLine_Value>を使用してLinesVisual3Dを生成、ModelVisual3Dに変換する
            /// </summary>
            /// <param name="DrawLine_Value_List">Point3D_List</param>
            /// <param name="colors">Set Color</param>
            /// <returns>List<ModelVisual3D>List<ModelVisual3D></returns>
            public List<ModelVisual3D> DrawLinesVisual3D(List<DrawLine_Value> DrawLine_Value_List, List<LinesVisual3D> LV3D_List, System.Windows.Media.Color colors)
            {
                List<ModelVisual3D> ConvertLV3DToMV3D_List = new List<ModelVisual3D>();
                
                //List<Point3D>を使用して線を描く
                for (int i = 0; i < DrawLine_Value_List.Count; i++)
                {
                    List<Point3D> p3d = new List<Point3D>();
                    p3d.Add(new Point3D(DrawLine_Value_List[i].StartPoint3D.X, DrawLine_Value_List[i].StartPoint3D.Y, DrawLine_Value_List[i].StartPoint3D.Z));
                    p3d.Add(new Point3D(DrawLine_Value_List[i].EndPoint3D.X, DrawLine_Value_List[i].EndPoint3D.Y, DrawLine_Value_List[i].EndPoint3D.Z));

                    LV3D_List.Add(new LinesVisual3D { Points = new Point3DCollection(p3d), Color = colors });
                    ConvertLV3DToMV3D_List.Add(LV3D_List[i]);
                }

                return ConvertLV3DToMV3D_List;
            }

            /// <summary>
            /// List<DrawLine_Value>を使用してLinesVisual3Dを生成、ModelVisual3Dに変換する
            /// </summary>
            /// <param name="DrawLine_Value_List">Point3D_List</param>
            /// <param name="colors">Set Color</param>
            /// <returns>List<ModelVisual3D>List<ModelVisual3D></returns>
            public List<ModelVisual3D> DrawLinesVisual3D(List<DrawLine_Value> DrawLine_Value_List, System.Windows.Media.Color colors)
            {
                List<ModelVisual3D> ConvertLV3DToMV3D_List = new List<ModelVisual3D>();
                List<LinesVisual3D> LV3D_List = new List<LinesVisual3D>();


                //List<Point3D>を使用して線を描く
                for (int i = 0; i < DrawLine_Value_List.Count; i++)
                {
                    List<Point3D> p3d = new List<Point3D>();
                    p3d.Add(new Point3D(DrawLine_Value_List[i].StartPoint3D.X, DrawLine_Value_List[i].StartPoint3D.Y, DrawLine_Value_List[i].StartPoint3D.Z));
                    p3d.Add(new Point3D(DrawLine_Value_List[i].EndPoint3D.X, DrawLine_Value_List[i].EndPoint3D.Y, DrawLine_Value_List[i].EndPoint3D.Z));

                    LV3D_List.Add(new LinesVisual3D { Points = new Point3DCollection(p3d), Color = colors });
                    ConvertLV3DToMV3D_List.Add(LV3D_List[i]);
                }

                return ConvertLV3DToMV3D_List;
            }

            /// <summary>
            /// List<Point3D>を使用してLinesVisual3Dを生成、ModelVisual3Dに変換する
            /// </summary>
            /// <param name="P3DList">Point3D_List</param>
            /// <param name="LV3D_List">LineVisual3D_List</param>
            /// <param name="colors">Set Color</param>
            /// <returns>List<ModelVisual3D>List<ModelVisual3D></returns>
            public List<ModelVisual3D> DrawLinesVisual3D(List<Point3D> P3DList, List<LinesVisual3D> LV3D_List, System.Windows.Media.Color colors)
            {
                List<ModelVisual3D> ConvertLV3DToMV3D_List = new List<ModelVisual3D>();

                //List<Point3D>を使用して線を描く
                for (int i = 0; i < P3DList.Count; i++)
                {
                    LV3D_List.Add(new LinesVisual3D { Points = new Point3DCollection(P3DList), Color = colors });
                    ConvertLV3DToMV3D_List.Add(LV3D_List[i]);
                }

                return ConvertLV3DToMV3D_List;
            }

            /// <summary>
            /// List<Point3D>を使用してLinesVisual3Dを生成、ModelVisual3Dに変換する
            /// </summary>
            /// <param name="P3DList">Point3D_List</param>
            /// <param name="LV3D_List">LineVisual3D_List</param>
            /// <param name="colors">Set Color</param>
            /// <returns>List<ModelVisual3D>List<ModelVisual3D></returns>
            public List<ModelVisual3D> DrawLinesVisual3D(List<Point3D> P3DList, System.Windows.Media.Color colors)
            {
                List<ModelVisual3D> ConvertLV3DToMV3D_List = new List<ModelVisual3D>();
                List<LinesVisual3D> LV3D_List = new List<LinesVisual3D>();

                //List<Point3D>を使用して線を描く
                for (int i = 0; i < P3DList.Count; i++)
                {
                    LV3D_List.Add(new LinesVisual3D { Points = new Point3DCollection(P3DList), Color = colors });
                    ConvertLV3DToMV3D_List.Add(LV3D_List[i]);
                }

                return ConvertLV3DToMV3D_List;
            }
        }

        public class CustomModelMV3D : TSRSystem
        {
            /// <summary>
            /// Point3DのListからModelVisual3Dを生成
            /// </summary>
            /// <param name="P3DList">Point3D_List</param>
            /// <param name="LV3D_List">LineVisual3D_List</param>
            /// <param name="colors">Set Color</param>
            /// <returns>List<ModelVisual3D>List<ModelVisual3D></returns>
            public List<ModelVisual3D> CustomModelCreate(List<Point3D> P3DList, List<LinesVisual3D> LV3D_List, System.Windows.Media.Color colors)
            {
                //List<Point3D>を使用して線を描く
                for (int i = 0; i < P3DList.Count; i++)
                {
                    LV3D_List.Add(new LinesVisual3D { Points = new Point3DCollection(P3DList), Color = colors });
                }

                List<ModelVisual3D> ConvertLV3DToMV3D_List = new List<ModelVisual3D>();

                for (int LV3DCount = 0; LV3DCount < LV3D_List.Count; LV3DCount++)
                {
                    //LinesVisual3DをModel3Dに変換
                    Model3D LV3DToM3D = LV3D_List[LV3DCount].Content;
                    ModelVisual3D M3DToMV3D = new ModelVisual3D { Content = LV3DToM3D };

                    //Add
                    ConvertLV3DToMV3D_List.Add(M3DToMV3D);
                }

                return ConvertLV3DToMV3D_List;
            }

            /// <summary>
            /// List<ModelVisual3D>を1つのModelVisual3Dに結合する
            /// </summary>
            /// <param name="MV3D_List"></param>
            /// <returns>ModelVisual3D</returns>
            public ModelVisual3D UnionModelVisual3D(List<ModelVisual3D> MV3D_List)
            {
                Model3DGroup UnionModelVisual3DGroup = new Model3DGroup();

                for (int ModelVisual3DCount = 0; ModelVisual3DCount < MV3D_List.Count; ModelVisual3DCount++)
                {
                    UnionModelVisual3DGroup.Children.Add(MV3D_List[ModelVisual3DCount].Content);
                }

                ModelVisual3D JoinedMV3D = new ModelVisual3D { Content = UnionModelVisual3DGroup };

                return JoinedMV3D;
            }
        }

        public class HitTestHelper
        {
            public class Search
            {
                public enum HitTestType
                {
                    Adorner,
                    Geometry,
                    Point,
                    Ray,
                    RayMeshGeometry3D
                }

                public static HitTestResult HitTestViewport(Visual Target, Point Point2D, HitTestType hitTestType)
                {
                    HitTestResult HTR = null;
                    HitTestResult HTRs = VisualTreeHelper.HitTest(Target, Point2D);
                    if (hitTestType == HitTestType.Adorner) HTR = HTRs as AdornerHitTestResult;
                    if (hitTestType == HitTestType.Geometry) HTR = HTRs as GeometryHitTestResult;
                    if (hitTestType == HitTestType.Point) HTR = HTRs as PointHitTestResult;
                    if (hitTestType == HitTestType.Ray) HTR = HTRs as RayHitTestResult;
                    if (hitTestType == HitTestType.RayMeshGeometry3D) HTR = HTRs as RayMeshGeometry3DHitTestResult;

                    return HTR;
                    //return HTRs as RayMeshGeometry3DHitTestResult;
                }
            }

            //public static T GetObjectName<T>(ModelVisual3D FindMV3D, HitTestResult hitTestResult, )
            //{
            //    object MDLStr_GetName = new object();
            //    if (typeof(ModelVisual3D) == hitTestResult.VisualHit.GetType())
            //    {
            //        //ダウンキャスト
            //        FindMV3D = (ModelVisual3D)hitTestResult.VisualHit;
            //        MDLStr_GetName = HTR.VisualHit.GetName().Split(' ');
            //    }
            //    if (typeof(LinesVisual3D) == HTR.VisualHit.GetType()) return;
            //    if (typeof(TubeVisual3D) == HTR.VisualHit.GetType()) return;


            //    return (T)MDLStr_GetName;
            //}

        }

        public class PathTools : TSRSystem
        {
            public class Rail
            {
                public List<ModelVisual3D> MV3D_List { get; set; }
                public List<LinesVisual3D> LV3D_List { get; set; }
                public List<TubeVisual3D> TV3D_List { get; set; }

                public Rail()
                {
                    MV3D_List = new List<ModelVisual3D>();
                    LV3D_List = new List<LinesVisual3D>();
                    TV3D_List = new List<TubeVisual3D>();
                }

                public Rail(List<ModelVisual3D> MV3DList, List<LinesVisual3D> LV3DList, List<TubeVisual3D> TV3DList)
                {
                    MV3D_List = MV3DList;
                    LV3D_List = LV3DList;
                    TV3D_List = TV3DList;
                }
            }

            public static List<Point3D> MV3DListToPoint3DList(List<ModelVisual3D> MV3DList)
            {
                List<Point3D> point3Ds = new List<Point3D>();

                for (int i = 0; i < MV3DList.Count; i++)
                {
                    Model3D n = MV3DList[i].Content;

                    Point3D p3d = new Point3D(n.Transform.Value.OffsetX, n.Transform.Value.OffsetY, n.Transform.Value.OffsetZ);

                    point3Ds.Add(p3d);
                }

                return point3Ds;
            }

            public static List<LinesVisual3D> DrawPath_Line(UserControl1 UserCtrl, List<Point3D> point3Ds, double Thickness, Color color)
            {
                List<LinesVisual3D> linesVisual3DList_Out = new List<LinesVisual3D>();
                if (point3Ds.Count > 1)
                {
                    for (int i = 1; i < point3Ds.Count; i++)
                    {
                        List<Point3D> OneLine = new List<Point3D>();
                        OneLine.Add(point3Ds[i - 1]);
                        OneLine.Add(point3Ds[i]);

                        LinesVisual3D linesVisual3D = new LinesVisual3D
                        {
                            Points = new Point3DCollection(OneLine),
                            Thickness = Thickness,
                            Color = color
                        };

                        UserCtrl.MainViewPort.Children.Add(linesVisual3D);

                        linesVisual3DList_Out.Add(linesVisual3D);
                    }
                }

                return linesVisual3DList_Out;
            }

            public static List<TubeVisual3D> DrawPath_Tube(UserControl1 UserCtrl, List<Point3D> point3Ds, double TubeDiametor, Color color)
            {
                List<TubeVisual3D> tubeVisual3DList_Out = new List<TubeVisual3D>();
                if (point3Ds.Count > 1)
                {
                    for(int i = 1; i < point3Ds.Count; i++)
                    {
                        //TubeVisual3Dの直径を指定
                        double Diametor_Value = TubeDiametor;

                        TubeVisual3D tubeVisual3D = new TubeVisual3D();
                        tubeVisual3D.Fill = new SolidColorBrush(color);
                        tubeVisual3D.Path = new Point3DCollection();
                        tubeVisual3D.Path.Add(point3Ds[i - 1]);
                        tubeVisual3D.Path.Add(point3Ds[i]);
                        tubeVisual3D.Diameter = Diametor_Value;
                        tubeVisual3D.IsPathClosed = false;

                        tubeVisual3DList_Out.Add(tubeVisual3D);

                        //Add Tube
                        UserCtrl.MainViewPort.Children.Add(tubeVisual3D);
                    }
                }

                return tubeVisual3DList_Out;
            }

            public static void MoveRails(int MDLNum, Vector3D Pos, List<TubeVisual3D> TubeVisual3D_List)
            {
                if (MDLNum == 0)
                {
                    TubeVisual3D_List[MDLNum].Path[0] = (Point3D)Pos;
                }
                if (MDLNum > 0 && MDLNum < TubeVisual3D_List.Count)
                {
                    TubeVisual3D_List[MDLNum - 1].Path[1] = (Point3D)Pos;
                    TubeVisual3D_List[MDLNum].Path[0] = (Point3D)Pos;
                }
                if (MDLNum == TubeVisual3D_List.Count)
                {
                    TubeVisual3D_List[MDLNum - 1].Path[1] = (Point3D)Pos;
                }
            }

            public static void MoveRails(int MDLNum, Vector3D Pos, List<LinesVisual3D> LinesVisual3D_List)
            {
                if (MDLNum == 0)
                {
                    LinesVisual3D_List[MDLNum].Points[0] = (Point3D)Pos;
                }
                if (MDLNum > 0 && MDLNum < LinesVisual3D_List.Count)
                {
                    LinesVisual3D_List[MDLNum - 1].Points[1] = (Point3D)Pos;
                    LinesVisual3D_List[MDLNum].Points[0] = (Point3D)Pos;
                }
                if (MDLNum == LinesVisual3D_List.Count)
                {
                    LinesVisual3D_List[MDLNum - 1].Points[1] = (Point3D)Pos;
                }
            }

            public enum RailType
            {
                Line,
                Tube
            }

            public enum PointType
            {
                Model,
                Point3D
            }

            public static void DeleteRailPoint(UserControl1 UserCtrl, Rail rail, int SelectedIdx, double TubeDiametor, Color color, RailType railType)
            {
                Point3D? SelectedIndex_Next = null;
                Point3D? SelectedIndex_Current = null;
                Point3D? SelectedIndex_Prev = null;

                List<Point3D> point3Ds = new List<Point3D>();

                #region SelectedIndex_Next
                try
                {
                    SelectedIndex_Next = new Point3D(rail.MV3D_List[SelectedIdx + 1].Content.Transform.Value.OffsetX, rail.MV3D_List[SelectedIdx + 1].Content.Transform.Value.OffsetY, rail.MV3D_List[SelectedIdx + 1].Content.Transform.Value.OffsetZ);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    SelectedIndex_Next = null;
                }
                #endregion

                #region SelectedIndex_Current
                try
                {
                    SelectedIndex_Current = new Point3D(rail.MV3D_List[SelectedIdx].Content.Transform.Value.OffsetX, rail.MV3D_List[SelectedIdx].Content.Transform.Value.OffsetY, rail.MV3D_List[SelectedIdx].Content.Transform.Value.OffsetZ);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    SelectedIndex_Current = null;
                }
                #endregion

                #region SelectedIndex_Prev
                try
                {
                    SelectedIndex_Prev = new Point3D(rail.MV3D_List[SelectedIdx - 1].Content.Transform.Value.OffsetX, rail.MV3D_List[SelectedIdx - 1].Content.Transform.Value.OffsetY, rail.MV3D_List[SelectedIdx - 1].Content.Transform.Value.OffsetZ);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    SelectedIndex_Prev = null;
                }
                #endregion

                if (railType == RailType.Tube)
                {
                    if (SelectedIndex_Current != null)
                    {
                        if ((SelectedIndex_Next == null && SelectedIndex_Prev == null) == true)
                        {
                            UserCtrl.MainViewPort.Children.Remove(rail.MV3D_List[SelectedIdx]);
                            rail.MV3D_List.Remove(rail.MV3D_List[SelectedIdx]);

                            //MessageBox.Show("Point3D Only");
                        }
                        else if ((SelectedIndex_Next != null && SelectedIndex_Prev != null) == true)
                        {
                            point3Ds.Add(SelectedIndex_Prev.Value);
                            point3Ds.Add(SelectedIndex_Next.Value);

                            //Pointを削除
                            UserCtrl.MainViewPort.Children.Remove(rail.MV3D_List[SelectedIdx]);
                            rail.MV3D_List.Remove(rail.MV3D_List[SelectedIdx]);

                            //Pointの両端に存在するTubeVisual3Dを削除
                            UserCtrl.MainViewPort.Children.Remove(rail.TV3D_List[SelectedIdx]);
                            rail.TV3D_List.Remove(rail.TV3D_List[SelectedIdx]);

                            UserCtrl.MainViewPort.Children.Remove(rail.TV3D_List[SelectedIdx - 1]);
                            rail.TV3D_List.Remove(rail.TV3D_List[SelectedIdx - 1]);

                            for (int i = 0; i < rail.MV3D_List.Count; i++)
                            {
                                string[] MDLInfo = rail.MV3D_List[i].GetName().Split(' ');
                                string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
                                rail.MV3D_List[i].SetName(New_MDLInfo);
                            }

                            //TubeVisual3Dの直径を指定
                            double Diametor_Value = TubeDiametor;

                            TubeVisual3D tubeVisual3D = new TubeVisual3D();
                            tubeVisual3D.Fill = new SolidColorBrush(color);
                            tubeVisual3D.Path = new Point3DCollection();
                            tubeVisual3D.Path.Add(point3Ds[0]);
                            tubeVisual3D.Path.Add(point3Ds[1]);
                            tubeVisual3D.Diameter = Diametor_Value;
                            tubeVisual3D.IsPathClosed = false;

                            rail.TV3D_List.Insert(SelectedIdx - 1, tubeVisual3D);

                            //Add Tube
                            UserCtrl.MainViewPort.Children.Add(tubeVisual3D);

                            //MessageBox.Show("PrevPoint and NextPoint");
                        }
                        else if ((SelectedIndex_Next != null || SelectedIndex_Prev != null) == true)
                        {
                            if (SelectedIndex_Prev == null)
                            {
                                UserCtrl.MainViewPort.Children.Remove(rail.MV3D_List[SelectedIdx]);
                                rail.MV3D_List.Remove(rail.MV3D_List[SelectedIdx]);

                                UserCtrl.MainViewPort.Children.Remove(rail.TV3D_List[SelectedIdx]);
                                rail.TV3D_List.Remove(rail.TV3D_List[SelectedIdx]);

                                for (int i = 0; i < rail.MV3D_List.Count; i++)
                                {
                                    string[] MDLInfo = rail.MV3D_List[i].GetName().Split(' ');
                                    string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
                                    rail.MV3D_List[i].SetName(New_MDLInfo);
                                }

                                //MessageBox.Show("PrevPoint not found : FirstPoint");
                            }
                            if (SelectedIndex_Next == null)
                            {
                                UserCtrl.MainViewPort.Children.Remove(rail.MV3D_List[SelectedIdx]);
                                rail.MV3D_List.Remove(rail.MV3D_List[SelectedIdx]);

                                UserCtrl.MainViewPort.Children.Remove(rail.TV3D_List[SelectedIdx - 1]);
                                rail.TV3D_List.Remove(rail.TV3D_List[SelectedIdx - 1]);

                                for (int i = 0; i < rail.MV3D_List.Count; i++)
                                {
                                    string[] MDLInfo = rail.MV3D_List[i].GetName().Split(' ');
                                    string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
                                    rail.MV3D_List[i].SetName(New_MDLInfo);
                                }

                                //MessageBox.Show("NextPoint not found : EndPoint");
                            }
                        }
                    }
                }
                if (railType == RailType.Line)
                {
                    if (SelectedIndex_Current != null)
                    {
                        if ((SelectedIndex_Next == null && SelectedIndex_Prev == null) == true)
                        {
                            UserCtrl.MainViewPort.Children.Remove(rail.MV3D_List[SelectedIdx]);
                            rail.MV3D_List.Remove(rail.MV3D_List[SelectedIdx]);

                            //MessageBox.Show("Point3D Only");
                        }
                        else if ((SelectedIndex_Next != null && SelectedIndex_Prev != null) == true)
                        {
                            point3Ds.Add(SelectedIndex_Prev.Value);
                            point3Ds.Add(SelectedIndex_Next.Value);

                            //Pointを削除
                            UserCtrl.MainViewPort.Children.Remove(rail.MV3D_List[SelectedIdx]);
                            rail.MV3D_List.Remove(rail.MV3D_List[SelectedIdx]);

                            //Pointの両端に存在するLinesVisual3Dを削除
                            UserCtrl.MainViewPort.Children.Remove(rail.LV3D_List[SelectedIdx]);
                            rail.LV3D_List.Remove(rail.LV3D_List[SelectedIdx]);

                            UserCtrl.MainViewPort.Children.Remove(rail.LV3D_List[SelectedIdx - 1]);
                            rail.LV3D_List.Remove(rail.LV3D_List[SelectedIdx - 1]);

                            for (int i = 0; i < rail.MV3D_List.Count; i++)
                            {
                                string[] MDLInfo = rail.MV3D_List[i].GetName().Split(' ');
                                string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
                                rail.MV3D_List[i].SetName(New_MDLInfo);
                            }

                            List<Point3D> OneLine = new List<Point3D>();
                            OneLine.Add(point3Ds[0]);
                            OneLine.Add(point3Ds[1]);

                            LinesVisual3D linesVisual3D = new LinesVisual3D
                            {
                                Points = new Point3DCollection(OneLine),
                                Thickness = TubeDiametor,
                                Color = color
                            };

                            rail.LV3D_List.Insert(SelectedIdx - 1, linesVisual3D);

                            UserCtrl.MainViewPort.Children.Add(linesVisual3D);

                            //MessageBox.Show("PrevPoint and NextPoint");
                        }
                        else if ((SelectedIndex_Next != null || SelectedIndex_Prev != null) == true)
                        {
                            if (SelectedIndex_Prev == null)
                            {
                                UserCtrl.MainViewPort.Children.Remove(rail.MV3D_List[SelectedIdx]);
                                rail.MV3D_List.Remove(rail.MV3D_List[SelectedIdx]);

                                UserCtrl.MainViewPort.Children.Remove(rail.LV3D_List[SelectedIdx]);
                                rail.LV3D_List.Remove(rail.LV3D_List[SelectedIdx]);

                                for (int i = 0; i < rail.MV3D_List.Count; i++)
                                {
                                    string[] MDLInfo = rail.MV3D_List[i].GetName().Split(' ');
                                    string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
                                    rail.MV3D_List[i].SetName(New_MDLInfo);
                                }

                                //MessageBox.Show("PrevPoint not found : FirstPoint");
                            }
                            if (SelectedIndex_Next == null)
                            {
                                UserCtrl.MainViewPort.Children.Remove(rail.MV3D_List[SelectedIdx]);
                                rail.MV3D_List.Remove(rail.MV3D_List[SelectedIdx]);

                                UserCtrl.MainViewPort.Children.Remove(rail.LV3D_List[SelectedIdx - 1]);
                                rail.LV3D_List.Remove(rail.LV3D_List[SelectedIdx - 1]);

                                for (int i = 0; i < rail.MV3D_List.Count; i++)
                                {
                                    string[] MDLInfo = rail.MV3D_List[i].GetName().Split(' ');
                                    string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
                                    rail.MV3D_List[i].SetName(New_MDLInfo);
                                }

                                //MessageBox.Show("NextPoint not found : EndPoint");
                            }
                        }
                    }
                }
            }

            public static void DeleteRail(UserControl1 UserCtrl, Rail rail)
            {
                if (rail.TV3D_List != null)
                {
                    for (int TVCount = 0; TVCount < rail.TV3D_List.Count; TVCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(rail.TV3D_List[TVCount]);
                        UserCtrl.UpdateLayout();
                    }

                    rail.TV3D_List.Clear();
                }

                if (rail.LV3D_List != null)
                {
                    for (int LVCount = 0; LVCount < rail.LV3D_List.Count; LVCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(rail.LV3D_List[LVCount]);
                        UserCtrl.UpdateLayout();
                    }

                    rail.LV3D_List.Clear();
                }

                if (rail.MV3D_List != null)
                {
                    for (int MV3DCount = 0; MV3DCount < rail.MV3D_List.Count; MV3DCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(rail.MV3D_List[MV3DCount]);
                        UserCtrl.UpdateLayout();
                    }

                    rail.MV3D_List.Clear();
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="UserCtrl"></param>
            /// <param name="rail"></param>
            /// <param name="railType"></param>
            public static void ResetRail(UserControl1 UserCtrl, Rail rail, RailType railType)
            {
                if (railType == RailType.Line)
                {
                    for (int i = 0; i < rail.LV3D_List.Count; i++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(rail.LV3D_List[i]);
                    }

                    rail.LV3D_List.Clear();
                }
                if (railType == RailType.Tube)
                {
                    for (int i = 0; i < rail.TV3D_List.Count; i++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(rail.TV3D_List[i]);
                    }

                    rail.TV3D_List.Clear();
                }
            }

            public class SideWall
            {
                public List<ModelVisual3D> SideWallList { get; set; }
            }

            public static List<ModelVisual3D> DrawPath_SideWall(UserControl1 UserCtrl, List<Point3D> point3Ds, Color color)
            {
                List<ModelVisual3D> modelVisual3DList_Out = new List<ModelVisual3D>();
                if (point3Ds.Count > 1)
                {
                    for (int i = 1; i < point3Ds.Count; i++)
                    {
                        #region Memo
                        //OneLine.Add(point3Ds[i - 1]); //1
                        //OneLine.Add(point3Ds[i]); //3
                        //OneLine.Add(new Point3D(point3Ds[i - 1].X, 0, point3Ds[i - 1].Z)); //0
                        //OneLine.Add(new Point3D(point3Ds[i].X, 0, point3Ds[i].Z)); //2
                        #endregion

                        List<Point3D> OneSiideWallMDL = new List<Point3D>();
                        OneSiideWallMDL.Add(new Point3D(point3Ds[i - 1].X, 0, point3Ds[i - 1].Z)); //0
                        OneSiideWallMDL.Add(point3Ds[i - 1]); //1
                        OneSiideWallMDL.Add(new Point3D(point3Ds[i].X, 0, point3Ds[i].Z)); //2
                        OneSiideWallMDL.Add(point3Ds[i]); //3

                        ModelVisual3D SideWall_MV3D = CustomModelCreateHelper.CustomRectanglePlane3D(HTK_3DES.CustomModelCreateHelper.ToPoint3DCollection(OneSiideWallMDL), color, color);
                        HTK_3DES.TSRSystem.SetString_MV3D(SideWall_MV3D, "SideWall -1 -1");
                        UserCtrl.MainViewPort.Children.Add(SideWall_MV3D);
                        modelVisual3DList_Out.Add(SideWall_MV3D);
                    }
                }

                return modelVisual3DList_Out;
            }

            public static void MoveSideWalls(int MDLNum, Vector3D Pos, List<ModelVisual3D> ModelVisual3D_List)
            {
                if (MDLNum == 0)
                {
                    HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum].Content).Positions[0] = new Point3D(((Point3D)Pos).X, 0, ((Point3D)Pos).Z);
                    HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum].Content).Positions[1] = (Point3D)Pos;
                }
                if (MDLNum > 0 && MDLNum < ModelVisual3D_List.Count)
                {
                    HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum - 1].Content).Positions[2] = new Point3D(((Point3D)Pos).X, 0, ((Point3D)Pos).Z);
                    HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum - 1].Content).Positions[3] = (Point3D)Pos;

                    HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum].Content).Positions[0] = new Point3D(((Point3D)Pos).X, 0, ((Point3D)Pos).Z);
                    HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum].Content).Positions[1] = (Point3D)Pos;
                }
                if (MDLNum == ModelVisual3D_List.Count)
                {
                    HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum - 1].Content).Positions[2] = new Point3D(((Point3D)Pos).X, 0, ((Point3D)Pos).Z);
                    HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum - 1].Content).Positions[3] = (Point3D)Pos;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="UserCtrl"></param>
            /// <param name="rail"></param>
            /// <param name="railType"></param>
            public static void ResetSideWall(UserControl1 UserCtrl, SideWall sideWall)
            {
                for (int i = 0; i < sideWall.SideWallList.Count; i++) UserCtrl.MainViewPort.Children.Remove(sideWall.SideWallList[i]);
                sideWall.SideWallList.Clear();
            }
        }

        public class Point3DSystem : TSRSystem
        {
            /// <summary>
            /// List<DrawLine_Value>を使用してLinesVisual3Dを生成、ModelVisual3Dに変換する
            /// </summary>
            /// <param name="DrawPoint_Value_List">Point3D_List</param>
            /// <param name="colors">Set Color</param>
            /// <returns>List<ModelVisual3D>List<ModelVisual3D></returns>
            public ModelVisual3D DrawPointsVisual3D(Point3D DrawPoint3D, List<PointsVisual3D> PV3D_List, System.Windows.Media.Color colors, double PointSize)
            {
                List<Point3D> p3d = new List<Point3D>();
                p3d.Add(DrawPoint3D);
                PV3D_List.Add(new PointsVisual3D { Points = new Point3DCollection(p3d), Color = colors, Size = PointSize });
                return PV3D_List[0];
            }
        }

        public class KMP_3DCheckpointSystem : HTK_3DES.PathTools
        {
            public class Checkpoint
            {
                public Rail Checkpoint_Left { get; set; }
                public Rail Checkpoint_Right { get; set; }
                public SideWall SideWall_Left { get; set; }
                public SideWall SideWall_Right { get; set; }
                public List<LinesVisual3D> Checkpoint_Line { get; set; }
                public List<TubeVisual3D> Checkpoint_Tube { get; set; }
                public List<ModelVisual3D> Checkpoint_SplitWallMDL { get; set; }
            }

            public static void DeleteRailChk(UserControl1 UserCtrl, Checkpoint railChk)
            {
                if (railChk.Checkpoint_Left.MV3D_List != null)
                {
                    for (int ChkLeftCount = 0; ChkLeftCount < railChk.Checkpoint_Left.MV3D_List.Count; ChkLeftCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(railChk.Checkpoint_Left.MV3D_List[ChkLeftCount]);
                        UserCtrl.UpdateLayout();
                    }

                    railChk.Checkpoint_Left.MV3D_List.Clear();
                }
                if (railChk.Checkpoint_Right.MV3D_List != null)
                {
                    for (int ChkRightCount = 0; ChkRightCount < railChk.Checkpoint_Right.MV3D_List.Count; ChkRightCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(railChk.Checkpoint_Right.MV3D_List[ChkRightCount]);
                        UserCtrl.UpdateLayout();
                    }

                    railChk.Checkpoint_Right.MV3D_List.Clear();
                }
                if (railChk.Checkpoint_Line != null)
                {
                    for (int ChkLineCount = 0; ChkLineCount < railChk.Checkpoint_Line.Count; ChkLineCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(railChk.Checkpoint_Line[ChkLineCount]);
                        UserCtrl.UpdateLayout();
                    }

                    railChk.Checkpoint_Line.Clear();
                }
                if (railChk.Checkpoint_Tube != null)
                {
                    for (int ChkTubeCount = 0; ChkTubeCount < railChk.Checkpoint_Tube.Count; ChkTubeCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(railChk.Checkpoint_Tube[ChkTubeCount]);
                        UserCtrl.UpdateLayout();
                    }

                    railChk.Checkpoint_Tube.Clear();
                }
                if (railChk.Checkpoint_Left.LV3D_List != null)
                {
                    for (int ChkRLineLeftCount = 0; ChkRLineLeftCount < railChk.Checkpoint_Left.LV3D_List.Count; ChkRLineLeftCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(railChk.Checkpoint_Left.LV3D_List[ChkRLineLeftCount]);
                        UserCtrl.UpdateLayout();
                    }

                    railChk.Checkpoint_Left.LV3D_List.Clear();
                }
                if (railChk.Checkpoint_Right.LV3D_List != null)
                {
                    for (int ChkRLineRightCount = 0; ChkRLineRightCount < railChk.Checkpoint_Right.LV3D_List.Count; ChkRLineRightCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(railChk.Checkpoint_Right.LV3D_List[ChkRLineRightCount]);
                        UserCtrl.UpdateLayout();
                    }

                    railChk.Checkpoint_Right.LV3D_List.Clear();
                }
                if(railChk.Checkpoint_SplitWallMDL != null)
                {
                    for (int ChkSplitWallCount = 0; ChkSplitWallCount < railChk.Checkpoint_SplitWallMDL.Count; ChkSplitWallCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(railChk.Checkpoint_SplitWallMDL[ChkSplitWallCount]);
                        UserCtrl.UpdateLayout();
                    }

                    railChk.Checkpoint_SplitWallMDL.Clear();
                }
                if (railChk.SideWall_Left.SideWallList != null)
                {
                    for (int ChkSideWallLeftCount = 0; ChkSideWallLeftCount < railChk.SideWall_Left.SideWallList.Count; ChkSideWallLeftCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(railChk.SideWall_Left.SideWallList[ChkSideWallLeftCount]);
                        UserCtrl.UpdateLayout();
                    }

                    railChk.SideWall_Left.SideWallList.Clear();
                }
                if (railChk.SideWall_Right.SideWallList != null)
                {
                    for (int ChkSideWallRightCount = 0; ChkSideWallRightCount < railChk.SideWall_Right.SideWallList.Count; ChkSideWallRightCount++)
                    {
                        UserCtrl.MainViewPort.Children.Remove(railChk.SideWall_Right.SideWallList[ChkSideWallRightCount]);
                        UserCtrl.UpdateLayout();
                    }

                    railChk.SideWall_Right.SideWallList.Clear();
                }
            }
        }

        public class OBJData
        {
            public static Matrix3D ReScale(Matrix3D Matrix_3D, double ScaleFactor)
            {
                Matrix3D M = Matrix_3D;
                M.M11 = M.M11 / ScaleFactor;
                M.M22 = M.M22 / ScaleFactor;
                M.M33 = M.M33 / ScaleFactor;
                return M;
            }

            /// <summary>
            /// ModelVisual3D or ArrayList
            /// </summary>
            /// <param name="Path"></param>
            /// <returns></returns>
            public static Dictionary<string, ModelVisual3D> OBJReader_Dictionary(string Path)
            {
                Dictionary<string, ModelVisual3D> MV3D_Dictionary = new Dictionary<string, ModelVisual3D>();

                Model3DGroup M3D_Group = null;
                ObjReader OBJ_Reader = new ObjReader();
                M3D_Group = OBJ_Reader.Read(Path);

                for (int MDLChildCount = 0; MDLChildCount < M3D_Group.Children.Count; MDLChildCount++)
                {
                    Model3D NewM3D = M3D_Group.Children[MDLChildCount];
                    ModelVisual3D MV3D = new ModelVisual3D { Content = NewM3D };

                    MV3D.Transform = new MatrixTransform3D(ReScale(MV3D.Content.Transform.Value, 100));

                    GeometryModel3D GM3D = (GeometryModel3D)M3D_Group.Children[MDLChildCount];
                    string MatName = GM3D.Material.GetName();

                    //Give a name to ModelVisual3D
                    MV3D.SetName(MatName);

                    if (MV3D_Dictionary.Keys.Contains(MatName) && MV3D_Dictionary.Values.Contains(MV3D))
                    {
                        //マテリアルの名前が同じだった場合
                        MV3D_Dictionary.Add(MatName + MDLChildCount, MV3D);
                    }
                    else
                    {
                        MV3D_Dictionary.Add(MatName, MV3D);
                    }
                }

                return MV3D_Dictionary;
            }

            public static Dictionary<string, ArrayList> OBJReader_AryListDictionary(string Path)
            {
                Dictionary<string, ArrayList> MV3D_Dictionary = new Dictionary<string, ArrayList>();

                Model3DGroup M3D_Group = null;
                ObjReader OBJ_Reader = new ObjReader();
                M3D_Group = OBJ_Reader.Read(Path);

                for (int MDLChildCount = 0; MDLChildCount < M3D_Group.Children.Count; MDLChildCount++)
                {
                    Model3D NewM3D = M3D_Group.Children[MDLChildCount];
                    ModelVisual3D MV3D = new ModelVisual3D { Content = NewM3D };

                    //MV3D.Transform = new MatrixTransform3D(ReScale(MV3D.Content.Transform.Value, 100));

                    GeometryModel3D GM3D = (GeometryModel3D)M3D_Group.Children[MDLChildCount];
                    string MatName = GM3D.Material.GetName();

                    //ModelVisual3Dに名前をつける
                    MV3D.SetName(MatName + " -1 -1");

                    ArrayList arrayList = new ArrayList();
                    arrayList.Add(false);
                    arrayList.Add(MV3D);


                    if (MV3D_Dictionary.Keys.Contains(MatName) && MV3D_Dictionary.Values.Contains(arrayList))
                    {
                        //マテリアルの名前が同じだった場合
                        MV3D_Dictionary.Add(MatName + MDLChildCount, arrayList);
                    }
                    else
                    {
                        MV3D_Dictionary.Add(MatName, arrayList);
                    }
                }

                return MV3D_Dictionary;
            }

            public static GeometryModel3D GetGeometryModel3D(Model3D MV3D)
            {
                return (GeometryModel3D)MV3D;
            }

            public static Geometry3D GetGeometry3D(Model3D MV3D)
            {
                return ((GeometryModel3D)MV3D).Geometry;
            }

            public static MeshGeometry3D GetMeshGeometry3D(Model3D MV3D)
            {
                return (MeshGeometry3D)((GeometryModel3D)MV3D).Geometry;
            }
        }

        public class CustomModelCreateHelper
        {
            public static List<Point3D> DefaultBoxData()
            {
                List<Point3D> point3Ds = new List<Point3D>();

                #region d1
                point3Ds.Add(new Point3D(-0.5, -0.5, -0.5));
                point3Ds.Add(new Point3D(-0.5, 0.5, -0.5));

                point3Ds.Add(new Point3D(-0.5, 0.5, 0.5));
                point3Ds.Add(new Point3D(-0.5, -0.5, 0.5));

                point3Ds.Add(new Point3D(0.5, -0.5, 0.5));
                point3Ds.Add(new Point3D(0.5, 0.5, 0.5));

                point3Ds.Add(new Point3D(0.5, 0.5, -0.5));
                point3Ds.Add(new Point3D(0.5, -0.5, -0.5));
                #endregion

                #region d2
                point3Ds.Add(new Point3D(0.5, -0.5, -0.5));
                point3Ds.Add(new Point3D(-0.5, -0.5, -0.5));

                point3Ds.Add(new Point3D(-0.5, -0.5, 0.5));
                point3Ds.Add(new Point3D(0.5, -0.5, 0.5));

                point3Ds.Add(new Point3D(0.5, 0.5, 0.5));
                point3Ds.Add(new Point3D(-0.5, 0.5, 0.5));

                point3Ds.Add(new Point3D(-0.5, 0.5, -0.5));
                point3Ds.Add(new Point3D(0.5, 0.5, -0.5));
                #endregion

                #region d3
                point3Ds.Add(new Point3D(-0.5, -0.5, -0.5));
                point3Ds.Add(new Point3D(-0.5, -0.5, 0.5));

                point3Ds.Add(new Point3D(-0.5, 0.5, 0.5));
                point3Ds.Add(new Point3D(-0.5, 0.5, -0.5));

                point3Ds.Add(new Point3D(0.5, 0.5, 0.5));
                point3Ds.Add(new Point3D(0.5, 0.5, -0.5));

                point3Ds.Add(new Point3D(0.5, -0.5, 0.5));
                point3Ds.Add(new Point3D(0.5, -0.5, -0.5));
                #endregion

                return point3Ds;
            }

            public static Point3DCollection ToPoint3DCollection(List<Point3D> point3DList)
            {
                Point3DCollection P3DCollection = new Point3DCollection();
                for (int i = 0; i < point3DList.Count; i++) P3DCollection.Add(point3DList[i]);
                return P3DCollection;
            }

            public static List<Point3D> ToPoint3DList(Point3DCollection P3DCollection)
            {
                List<Point3D> point3DList = new List<Point3D>();
                for (int i = 0; i < P3DCollection.Count; i++) point3DList.Add(P3DCollection[i]);
                return point3DList;
            }

            public static MeshBuilder AddPoint3DList(List<Point3D> point3Ds)
            {
                var msb = new MeshBuilder();
                for (int i = 0; i < point3Ds.Count; i++) msb.Positions.Add(point3Ds[i]);
                return msb;
            }

            public static MeshBuilder AddPoint3DCollection(Point3DCollection P3DCollection)
            {
                var msb = new MeshBuilder();
                for (int i = 0; i < P3DCollection.Count; i++) msb.Positions.Add(P3DCollection[i]);
                return msb;
            }

            public static ModelVisual3D MeshGeometryToModelVisual3D(Point3DCollection point3Ds, Color color, Color Backcolor, string MDLName = null)
            {
                GeometryModel3D mesh = new GeometryModel3D
                {
                    Geometry = new MeshGeometry3D
                    {
                        Positions = point3Ds
                    },
                    Material = MaterialHelper.CreateMaterial(color),
                    BackMaterial = MaterialHelper.CreateMaterial(Backcolor)
                };

                ModelVisual3D modelVisual3D = new ModelVisual3D { Content = mesh };
                modelVisual3D.SetName(MDLName);

                return modelVisual3D;
            }

            public static ModelVisual3D MeshGeometryToModelVisual3D(Point3DCollection point3Ds, Int32Collection TriangleIndicesSetting, Color color, Color Backcolor, string MDLName = null)
            {
                GeometryModel3D mesh = new GeometryModel3D
                {
                    Geometry = new MeshGeometry3D
                    {
                        Positions = point3Ds,
                        TriangleIndices = TriangleIndicesSetting
                    },
                    Material = MaterialHelper.CreateMaterial(color),
                    BackMaterial = MaterialHelper.CreateMaterial(Backcolor)
                };

                ModelVisual3D modelVisual3D = new ModelVisual3D { Content = mesh };
                modelVisual3D.SetName(MDLName);

                return modelVisual3D;
            }

            public static ModelVisual3D CreateWireBoxLine()
            {
                LinesVisual3D linesVisual3D = new LinesVisual3D();
                linesVisual3D.Thickness = 5;
                linesVisual3D.Points = ToPoint3DCollection(DefaultBoxData());
                ModelVisual3D modelVisual3D = linesVisual3D;
                return modelVisual3D;
            }

            public static ModelVisual3D CreateWireBoxTube()
            {
                TubeVisual3D tubeVisual3D = new TubeVisual3D();
                tubeVisual3D.Diameter = 0.04;
                tubeVisual3D.Path = ToPoint3DCollection(DefaultBoxData());
                tubeVisual3D.IsPathClosed = false;
                ModelVisual3D modelVisual3D = tubeVisual3D;
                return modelVisual3D;
            }

            public static List<Point3D> Vector3DToPoint3DList(Vector3D vector3D)
            {
                double v1 = vector3D.X / 2;
                double v2 = -(vector3D.X / 2);

                double v3 = vector3D.Y / 2;
                double v4 = -(vector3D.Y / 2);

                double v5 = vector3D.Z / 2;
                double v6 = -(vector3D.Z / 2);

                List<Point3D> point3Ds = new List<Point3D>();

                #region d1
                point3Ds.Add(new Point3D(v2, v3, v6));
                point3Ds.Add(new Point3D(v2, v4, v6));

                point3Ds.Add(new Point3D(v2, v4, v6));
                point3Ds.Add(new Point3D(v1, v4, v6));

                point3Ds.Add(new Point3D(v1, v4, v6));
                point3Ds.Add(new Point3D(v1, v3, v6));

                point3Ds.Add(new Point3D(v1, v3, v6));
                point3Ds.Add(new Point3D(v2, v3, v6));
                #endregion

                #region d2
                point3Ds.Add(new Point3D(v2, v3, v5));
                point3Ds.Add(new Point3D(v2, v4, v5));

                point3Ds.Add(new Point3D(v2, v4, v5));
                point3Ds.Add(new Point3D(v1, v4, v5));

                point3Ds.Add(new Point3D(v1, v4, v5));
                point3Ds.Add(new Point3D(v1, v3, v5));

                point3Ds.Add(new Point3D(v1, v3, v5));
                point3Ds.Add(new Point3D(v2, v3, v5));
                #endregion

                #region d3
                point3Ds.Add(new Point3D(v2, v4, v6));
                point3Ds.Add(new Point3D(v2, v4, v5));

                point3Ds.Add(new Point3D(v1, v4, v5));
                point3Ds.Add(new Point3D(v1, v4, v6));

                point3Ds.Add(new Point3D(v1, v3, v6));
                point3Ds.Add(new Point3D(v1, v3, v5));

                point3Ds.Add(new Point3D(v2, v3, v6));
                point3Ds.Add(new Point3D(v2, v3, v5));
                #endregion

                return point3Ds;
            }

            public static ModelVisual3D CreateWireFrameMDLLine(List<Point3D> point3Ds)
            {
                LinesVisual3D linesVisual3D = new LinesVisual3D();
                linesVisual3D.Thickness = 4;
                linesVisual3D.Points = ToPoint3DCollection(point3Ds);
                ModelVisual3D modelVisual3D = linesVisual3D;
                return modelVisual3D;
            }

            public static ModelVisual3D CreateWireFrameMDLTube(List<Point3D> point3Ds)
            {
                TubeVisual3D tubeVisual3D = new TubeVisual3D();
                tubeVisual3D.Diameter = 0.05;
                tubeVisual3D.Path = ToPoint3DCollection(point3Ds);
                ModelVisual3D modelVisual3D = tubeVisual3D;
                return modelVisual3D;
            }

            public static ModelVisual3D CustomCylinderVisual3D(Color color, Color BackColor)
            {
                Point3DCollection point3Ds = new Point3DCollection();
                point3Ds.Add(new Point3D(0, -0.5, 0));
                point3Ds.Add(new Point3D(0, 0.5, 0));

                TubeVisual3D tubeVisual3D = new TubeVisual3D
                {
                    Diameter = 1,
                    Path = point3Ds,
                    AddCaps = true,
                    Material = MaterialHelper.CreateMaterial(color),
                    BackMaterial = MaterialHelper.CreateMaterial(BackColor)
                };

                Model3DGroup model3DGroup = new Model3DGroup();
                model3DGroup.Children.Add(tubeVisual3D.Content);

                ModelVisual3D modelVisual3D = new ModelVisual3D { Content = model3DGroup };

                return modelVisual3D;
            }

            public static ModelVisual3D CustomBoxVisual3D(Vector3D vector3D, Point3D center, Color Color, Color BackColor)
            {
                BoxVisual3D boxVisual3D = new BoxVisual3D
                {
                    Length = vector3D.X,
                    Width = vector3D.Y,
                    Height = vector3D.Z,
                    TopFace = true,
                    BottomFace = true,
                    Visible = true,
                    Center = center,
                    Material = MaterialHelper.CreateMaterial(Color),
                    BackMaterial = MaterialHelper.CreateMaterial(BackColor),
                };

                Model3DGroup model3DGroup = new Model3DGroup();
                model3DGroup.Children.Add(boxVisual3D.Content);

                ModelVisual3D modelVisual3D = new ModelVisual3D { Content = model3DGroup };

                //ModelVisual3D modelVisual3D = boxVisual3D;

                return modelVisual3D;
            }

            public static ModelVisual3D CustomSphereVisual3D(int ThetaDivValue, int PhiDivValue, double RadiusValue, Color Color, Color BackColor)
            {
                //int ThetaDivValue = 30, int PhiDivValue = 10, double RadiusValue = 0.5
                SphereVisual3D sphereVisual3D = new SphereVisual3D
                {
                    ThetaDiv = ThetaDivValue,
                    PhiDiv = PhiDivValue,
                    Radius = RadiusValue,
                    Material = MaterialHelper.CreateMaterial(Color),
                    BackMaterial = MaterialHelper.CreateMaterial(BackColor)
                };

                Model3DGroup model3DGroup = new Model3DGroup();
                model3DGroup.Children.Add(sphereVisual3D.Content);

                ModelVisual3D modelVisual3D = new ModelVisual3D { Content = model3DGroup };

                //ModelVisual3D modelVisual3D = sphereVisual3D;
                return modelVisual3D;
            }

            public static ModelVisual3D CustomRectanglePlane3D(Point3DCollection P3DCollection, Color color, Color Backcolor, string MDLName = "")
            {
                RectangleVisual3D rectangleVisual3D = new RectangleVisual3D
                {
                    Length = 1,
                    Width = 1,
                    DivLength = 1,
                    DivWidth = 1,
                    Origin = new Point3D(0, 0, 0)
                };

                ModelVisual3D modelVisual3D = rectangleVisual3D;
                HTK_3DES.OBJData.GetMeshGeometry3D(modelVisual3D.Content).Positions = P3DCollection;
                ((GeometryModel3D)modelVisual3D.Content).Material = MaterialHelper.CreateMaterial(color);
                ((GeometryModel3D)modelVisual3D.Content).BackMaterial = MaterialHelper.CreateMaterial(Backcolor);
                modelVisual3D.SetName(MDLName);

                return modelVisual3D;
            }

            public static ModelVisual3D CustomArrowVisual3D(double Diameter, int ThetaDiv, double HeadLength, Vector3D Direction, Point3D Origin, Color Color, Color BackColor)
            {
                ArrowVisual3D arrowVisual3D = new ArrowVisual3D
                {
                    Diameter = Diameter,
                    ThetaDiv = ThetaDiv,
                    HeadLength = HeadLength,
                    Direction = Direction,
                    Origin = Origin,
                    Material = MaterialHelper.CreateMaterial(Color),
                    BackMaterial = MaterialHelper.CreateMaterial(BackColor)
                };

                ModelVisual3D modelVisual3D = arrowVisual3D;

                return modelVisual3D;
            }

            public static ModelVisual3D CustomPointVector3D(Color BoxColor, Color BoxBackColor, Color ArrowColor, Color ArrowBackColor, Color SphereColor, Color SphereBackColor)
            {
                //BoxVisual3D boxVisual3D = (BoxVisual3D)CustomBoxVisual3D(new Vector3D(0.3, 0.3, 2.5), new Point3D(0, 0, 1.65), BoxColor, BoxBackColor);
                ArrowVisual3D arrowVisual3D = (ArrowVisual3D)CustomArrowVisual3D(0.3, 5, 1, new Vector3D(0, 1, 0), new Point3D(0, -0.5, 0), ArrowColor, ArrowBackColor);

                HTK_3DES.TSRSystem.Transform transform = new HTK_3DES.TSRSystem.Transform
                {
                    Rotate3D = new Vector3D(0, 0, 0),
                    Scale3D = new Vector3D(1, 1, 1),
                    Translate3D = new Vector3D(0, -0.1, 0)
                };

                HTK_3DES.TSRSystem.TransformSetting transformSetting = new TSRSystem.TransformSetting { InputMV3D = arrowVisual3D };

                HTK_3DES.TSRSystem.New_TransformSystem3D(transform, transformSetting);

                //HTK_3DES.TransformMV3D.Transform_MV3D(transform, arrowVisual3D, HTK_3DES.TSRSystem.RotationSetting.Angle);

                Model3DGroup model3DGroup = new Model3DGroup();
                model3DGroup.Children.Add(CustomBoxVisual3D(new Vector3D(0.3, 0.3, 5), new Point3D(0, 0, 2.65), BoxColor, BoxBackColor).Content);
                model3DGroup.Children.Add(arrowVisual3D.Content);
                model3DGroup.Children.Add(CustomSphereVisual3D(30, 10, 1, SphereColor, SphereBackColor).Content);

                ModelVisual3D modelVisual3D = new ModelVisual3D { Content = model3DGroup };

                return modelVisual3D;
            }

            public static CuttingPlaneGroup CreateCuttingPlaneGroup(List<Visual3D> visual3Ds, List<Plane3D> plane3Ds, CuttingOperation cuttingOperation, bool IsEnabled)
            {
                CuttingPlaneGroup cuttingPlaneGroup = new CuttingPlaneGroup
                {
                    CuttingPlanes = plane3Ds,
                    Operation = cuttingOperation,
                    IsEnabled = IsEnabled
                };

                for (int f = 0; f < visual3Ds.Count; f++) cuttingPlaneGroup.Children.Add(visual3Ds[f]);

                return cuttingPlaneGroup;
            }

            public static CuttingPlaneGroup CreateCuttingPlaneGroup(Visual3D visual3D, List<Plane3D> plane3Ds, CuttingOperation cuttingOperation, bool IsEnabled)
            {
                CuttingPlaneGroup cuttingPlaneGroup = new CuttingPlaneGroup
                {
                    CuttingPlanes = plane3Ds,
                    Operation = cuttingOperation,
                    IsEnabled = IsEnabled
                };

                cuttingPlaneGroup.Children.Add(visual3D);

                return cuttingPlaneGroup;
            }

            public enum Option
            {
                Setting1,
                Setting2,
                Setting3
            }

            public static ModelVisual3D CustomSphereHurf3D(int ThetaDivValue, int PhiDivValue, double RadiusValue, Color Color, Color BackColor, Option option)
            {
                //int ThetaDivValue = 30, int PhiDivValue = 10, double RadiusValue = 0.5
                SphereVisual3D sphereVisual3D = new SphereVisual3D
                {
                    ThetaDiv = ThetaDivValue,
                    PhiDiv = PhiDivValue,
                    Radius = RadiusValue,
                    Material = MaterialHelper.CreateMaterial(Color),
                    BackMaterial = MaterialHelper.CreateMaterial(BackColor)
                };

                List<Plane3D> plane3Ds = new List<Plane3D>();

                if (option == Option.Setting1) plane3Ds.Add(new Plane3D { Normal = new Vector3D(0, 1, 0), Position = new Point3D(0, 0, 0) });
                if (option == Option.Setting2) plane3Ds.Add(new Plane3D { Normal = new Vector3D(1, 0, 0), Position = new Point3D(0, 0, 0) });
                if (option == Option.Setting3) plane3Ds.Add(new Plane3D { Normal = new Vector3D(0, 0, 1), Position = new Point3D(0, 0, 0) });

                ModelVisual3D modelVisual3D = CreateCuttingPlaneGroup(sphereVisual3D, plane3Ds, CuttingOperation.Intersect, true);
                return modelVisual3D;
            }

            public static ModelVisual3D CustomPointModel3D()
            {
                List<Plane3D> plane3Ds = new List<Plane3D>();
                plane3Ds.Add(new Plane3D { Normal = new Vector3D(0, 1, 0), Position = new Point3D(0, 0, 0) });

                List<Plane3D> plane3Ds2 = new List<Plane3D>();
                plane3Ds2.Add(new Plane3D { Normal = new Vector3D(0, -1, 0), Position = new Point3D(0, 0, 0) });

                ModelVisual3D sp1 = CustomSphereVisual3D(30, 10, 0.5, Color.FromArgb(0x80, 0x00, 0xF0, 0x00), Color.FromArgb(0x80, 0x00, 0xF0, 0x00));
                ModelVisual3D sp2 = CustomSphereVisual3D(30, 10, 0.5, Color.FromArgb(0x80, 0xF0, 0x00, 0x00), Color.FromArgb(0x80, 0xF0, 0x00, 0x00));
                ModelVisual3D Box1 = CustomBoxVisual3D(new Vector3D(0.3, 0.3, 2.5), new Point3D(0, 0, 1), Color.FromArgb(0x80, 0xF0, 0x00, 0xF0), Color.FromArgb(0x80, 0xF0, 0x00, 0xF0));
                ModelVisual3D Box2 = CustomBoxVisual3D(new Vector3D(0.3, 0.3, 2.5), new Point3D(0, 0, 1), Color.FromArgb(0x80, 0x00, 0xF0, 0xF0), Color.FromArgb(0x80, 0x00, 0xF0, 0xF0));

                ModelVisual3D f1 = CreateCuttingPlaneGroup(sp1, plane3Ds, CuttingOperation.Intersect, true);
                ModelVisual3D f2 = CreateCuttingPlaneGroup(sp2, plane3Ds2, CuttingOperation.Intersect, true);
                ModelVisual3D f3 = CreateCuttingPlaneGroup(Box1, plane3Ds, CuttingOperation.Intersect, true);
                ModelVisual3D f4 = CreateCuttingPlaneGroup(Box2, plane3Ds2, CuttingOperation.Intersect, true);

                ModelVisual3D MV3D = new ModelVisual3D();
                MV3D.Children.Add(f1);
                MV3D.Children.Add(f2);
                MV3D.Children.Add(f3);
                MV3D.Children.Add(f4);

                return MV3D;
            }
        }
    }
}
