using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace CGFX_Viewer
{
    public partial class Form1 : Form
    {
        //UserControl1.xamlの初期化
        //ここは作成時の名前にも影響されるので必ず確認すること
        public UserControl1 render = new UserControl1();

        public Form1()
        {
            InitializeComponent();
        }

        CGFXFormat.CGFX CGFX { get; set; }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            elementHost1.Child = render;

            OpenFileDialog Open_CGFX = new OpenFileDialog()
            {
                Title = "Open(CGFX)",
                InitialDirectory = @"C:\Users\User\Desktop",
                Filter = "bcmdl file|*.bcmdl|All file|*.*"
            };

            if (Open_CGFX.ShowDialog() != DialogResult.OK) return;

            System.IO.FileStream fs1 = new FileStream(Open_CGFX.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br1 = new BinaryReader(fs1);

            CGFX = new CGFXFormat.CGFX();
            CGFX.ReadCGFX(br1);

            #region TreeView
            treeView1.HideSelection = false;
            List<string> SectionNameList = new List<string>(CGFX.Data.DictOffset_Dictionary.Keys);

            List<TreeNode> SectionNodeList = new List<TreeNode>();
            for (int i = 0; i < SectionNameList.Count; i++)
            {
                List<TreeNode> EntryNameList = new List<TreeNode>();
                foreach (var sw in CGFX.DICTAndSectionData.Keys.Where(x => x == SectionNameList[i]).ToList())
                {
                    //var yt = CGFX.DICTAndSectionData[sw].DICT_Entries.Select(x => new TreeNode(x.Name)).ToList();
                    //EntryNameList.AddRange(yt.ToArray());

                    foreach (var r in CGFX.DICTAndSectionData[sw].DICT_Entries)
                    {
                        if (sw == "Model")
                        {
                            var lt = r.CGFXData.CGFXSectionData.CMDLSection.meshDatas.Select(x => new TreeNode(x.SOBJData.Meshes.MeshName)).ToList();
                            //var nt = r.CGFXData.CGFXSectionData.CMDLSection.shapeDatas.Select(x => new TreeNode(x.SOBJData.Shapes.Name)).ToList();
                            var nt = r.CGFXData.CGFXSectionData.CMDLSection.shapeDatas.Select((x, Id) => new { Id, x }).Select(x => new TreeNode(x.Id.ToString())).ToList();

                            TreeNode treeNode = new TreeNode(r.Name);
                            treeNode.Nodes.Add(new TreeNode("Mesh", lt.ToArray()));
                            treeNode.Nodes.Add(new TreeNode("Shape", nt.ToArray()));

                            EntryNameList.Add(treeNode);
                        }
                        else
                        {
                            TreeNode treeNode = new TreeNode(r.Name);
                            EntryNameList.Add(treeNode);
                        }
                    }
                }

                TreeNode SectionNameNode = new TreeNode(SectionNameList[i], EntryNameList.ToArray());
                SectionNodeList.Add(SectionNameNode);
            }

            TreeNode RootNode = new TreeNode("CGFX_Root", SectionNodeList.ToArray());
            treeView1.Nodes.Add(RootNode);
            treeView1.TopNode.Expand();
            #endregion
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                treeView1.PathSeparator = ",";

                string[] Set = treeView1.SelectedNode.FullPath.Split(',');

                if (Set.Length == 1)
                {
                    EndianConvert endianConvert = new EndianConvert(CGFX.BOM);
                    textBox1.Text += endianConvert.EndianCheck() + "\r\n";

                    textBox1.Text += "Revision : " + CGFX.Revision + "\r\n" + "\r\n";

                    foreach (var SectionCount in CGFX.Data.DictOffset_Dictionary)
                    {
                        textBox1.Text += SectionCount.Key + ":" + SectionCount.Value.NumOfEntries + "\r\n";
                    }
                }
                if (Set.Length == 2)
                {
                    System.Windows.MessageBox.Show("Selected " + Set[1]);
                }
                if (Set.Length == 3)
                {
                    var ht = CGFX.DICTAndSectionData[Set[1]].DICT_Entries.Find(x => x.Name == Set[2]).CGFXData.CGFXSectionData;


                    if (Set[1] == "Model")
                    {
                        var Models = ht.CMDLSection;
                        propertyGrid3.SelectedObject = new CGFXPropertyGridSet.CMDL_PropertyGrid(Models);

                        List<ModelVisual3D> mList = new List<ModelVisual3D>();
                        foreach (var shape in Models.shapeDatas)
                        {
                            List<int> vs = new List<int>();
                            List<Face> faces = new List<Face>();
                            var Tr = shape.SOBJData.Shapes.primitiveSets;
                            foreach (var ny in Tr)
                            {
                                foreach (var z in ny.Primitives)
                                {
                                    foreach (var q in z.IndexStreamCtrList)
                                    {
                                        //vs.Add(q.FaceArray);

                                        vs = q.FaceArray;
                                        faces = q.Faces;
                                    }
                                }
                            }


                            var VertexAttr = shape.SOBJData.Shapes.VertexAttributes;
                            foreach (var Polygons in VertexAttr)
                            {
                                var t = Polygons.Streams.PolygonList;

                                ////FF 80 80 80
                                //double q = BitConverter.ToDouble(new byte[] { 0x80, 0x80, 0x80, 0xFF }, 0);

                                IList<Point3D> TrPosList = t.Select(x => x.Position).ToList();
                                IList<Vector3D> TrNorList = t.Select(x => x.Normal).ToList();
                                IList<System.Windows.Point> TrTexCoordList = t.Select(x => new System.Windows.Point(x.TexCoord.X, x.TexCoord.Y)).ToList();
                                //IList<int> TrIndicts = vs;

                                MeshBuilder meshBuilder = new MeshBuilder(true, true);

                                for (int y = 0; y < vs.Count; y++)
                                {
                                    int number = vs[y];
                                    meshBuilder.TriangleIndices.Add(number);
                                    meshBuilder.Positions.Add(t[number].Position);
                                    meshBuilder.Normals.Add(t[number].Normal);
                                    meshBuilder.TextureCoordinates.Add(new System.Windows.Point(t[number].TexCoord.X, t[number].TexCoord.Y));
                                }

                                //meshBuilder.AddTriangles(TrPosList, TrNorList, TrTexCoordList);
                                //meshBuilder.AddPolygon(TrIndicts);

                                MeshGeometry3D meshGeometry3D = meshBuilder.ToMesh(true);

                                //BitmapImage bitmapImage = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(ht.TXOBSection.TXOB_Bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                                //Material material = MaterialHelper.CreateImageMaterial(bitmapImage, 1, true);

                                Material material = MaterialHelper.CreateMaterial(System.Windows.Media.Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));

                                var m3dGrp = new Model3DGroup();
                                m3dGrp.Children.Add(new GeometryModel3D { Geometry = meshGeometry3D, Material = material, BackMaterial = material });

                                ModelVisual3D m = new ModelVisual3D { Content = m3dGrp };
                                mList.Add(m);

                                //表示
                                render.MainViewPort.Children.Add(m);

                                #region ch
                                //Int32Collection TriangleIndices = new Int32Collection();
                                //Point3DCollection point3Ds = new Point3DCollection();
                                //Vector3DCollection VNormals = new Vector3DCollection();
                                //PointCollection TexCoords = new PointCollection();

                                //for (int y = 0; y < vs.Count; y++)
                                //{
                                //    int number = vs[y];
                                //    TriangleIndices.Add(number);
                                //    point3Ds.Add(t[number].Position);
                                //    VNormals.Add(t[number].Normal);
                                //    TexCoords.Add(new System.Windows.Point(t[number].TexCoord.X, t[number].TexCoord.Y));
                                //}
                                #endregion

                                //Int32Collection TriangleIndices = new Int32Collection();
                                //Point3DCollection point3Ds = new Point3DCollection();
                                //Vector3DCollection VNormals = new Vector3DCollection();
                                //PointCollection TexCoords = new PointCollection();

                                //for (int y = 0; y < t.Count; y++)
                                //{
                                //    //int number = vs[y];
                                //    //TriangleIndices.Add(number);
                                //    //TriangleIndices.Add(f)
                                //    point3Ds.Add(t[y].Position);
                                //    VNormals.Add(t[y].Normal);
                                //    TexCoords.Add(new System.Windows.Point(t[y].TexCoord.X, t[y].TexCoord.Y));
                                //}

                                //TriangleIndices = new Int32Collection(vs);

                                //Material material = MaterialHelper.CreateMaterial(System.Windows.Media.Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));

                                //GeometryModel3D mesh = new GeometryModel3D
                                //{
                                //    Geometry = new MeshGeometry3D
                                //    {
                                //        TriangleIndices = TriangleIndices,
                                //        Positions = point3Ds,
                                //        Normals = VNormals,
                                //        TextureCoordinates = TexCoords
                                //    },
                                //    Material = material,
                                //    BackMaterial = material
                                //};

                                //ModelVisual3D modelVisual3D = new ModelVisual3D { Content = mesh };

                                ////表示
                                //render.MainViewPort.Children.Add(modelVisual3D);

                            }
                        }



                        //List<ModelVisual3D> mList = new List<ModelVisual3D>();
                        //foreach (var shape in Models.shapeDatas)
                        //{
                        //    //List<int> vs = new List<int>();


                        //    //var Tr = shape.SOBJData.Shapes.primitiveSets;
                        //    //foreach (var ny in Tr)
                        //    //{
                        //    //    foreach (var z in ny.Primitives)
                        //    //    {
                        //    //        foreach (var q in z.IndexStreamCtrList)
                        //    //        {
                        //    //            //vs.Add(q.FaceArray);

                        //    //            vs = q.FaceArray;
                        //    //        }
                        //    //    }
                        //    //}


                        //    var VertexAttr = shape.SOBJData.Shapes.VertexAttributes;
                        //    foreach (var Polygons in VertexAttr)
                        //    {
                        //        var t = Polygons.Streams.PolygonList;

                        //        IList<Point3D> TrPosList = t.Select(x => x.Position.ToPoint3D()).ToList();
                        //        IList<Vector3D> TrNorList = t.Select(x => x.Normal).ToList();
                        //        IList<System.Windows.Point> TrTexCoordList = t.Select(x => new System.Windows.Point(x.TexCoord.X, x.TexCoord.Y)).ToList();
                        //        //IList<int> TrIndicts = vs;

                        //        MeshBuilder meshBuilder = new MeshBuilder(true, true);
                        //        meshBuilder.AddTriangles(TrPosList, TrNorList, TrTexCoordList);
                        //        //meshBuilder.AddPolygon(TrIndicts);

                        //        MeshGeometry3D meshGeometry3D = meshBuilder.ToMesh(true);

                        //        //BitmapImage bitmapImage = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(ht.TXOBSection.TXOB_Bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        //        //Material material = MaterialHelper.CreateImageMaterial(bitmapImage, 1, true);

                        //        Material material = MaterialHelper.CreateMaterial(System.Windows.Media.Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));

                        //        var m3dGrp = new Model3DGroup();
                        //        m3dGrp.Children.Add(new GeometryModel3D { Geometry = meshGeometry3D, Material = material, BackMaterial = material });

                        //        ModelVisual3D m = new ModelVisual3D { Content = m3dGrp };
                        //        mList.Add(m);

                        //        //表示
                        //        render.MainViewPort.Children.Add(m);
                        //    }
                        //}


                        //List<ModelVisual3D> mList = new List<ModelVisual3D>();
                        //foreach (var shape in Models.shapeDatas)
                        //{
                        //	var VertexAttr = shape.SOBJData.Shapes.VertexAttributes;
                        //	foreach (var Polygons in VertexAttr)
                        //	{
                        //		var t = Polygons.Streams.PolygonList;

                        //		IList<Point3D> TrPosList = t.Select(x => x.Position.ToPoint3D()).ToList();
                        //		IList<Vector3D> TrNorList = t.Select(x => x.Normal).ToList();
                        //		IList<System.Windows.Point> TrTexCoordList = t.Select(x => new System.Windows.Point(x.TexCoord.X, x.TexCoord.Y)).ToList();

                        //		MeshBuilder meshBuilder = new MeshBuilder();
                        //		meshBuilder.AddTriangles(TrPosList, TrNorList, TrTexCoordList);

                        //		MeshGeometry3D meshGeometry3D = meshBuilder.ToMesh(true);

                        //		BitmapImage bitmapImage = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(ht.TXOBSection.TXOB_Bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        //		Material material = MaterialHelper.CreateImageMaterial(bitmapImage, 1, true);

                        //		var m3dGrp = new Model3DGroup();
                        //		m3dGrp.Children.Add(new GeometryModel3D { Geometry = meshGeometry3D, Material = material, BackMaterial = material });

                        //		ModelVisual3D m = new ModelVisual3D { Content = m3dGrp };
                        //		mList.Add(m);
                        //	}
                        //}

                    }
                    if (Set[1] == "Textures")
                    {
                        var Textures = ht.TXOBSection;
                        pictureBox1.Image = Textures.TXOB_Bitmap;
                        propertyGrid2.SelectedObject = new CGFXPropertyGridSet.TXOB_PropertyGrid(Textures);
                    }
                    if (Set[1] == "Fog")
                    {
                        var fogs = ht.CFOGSection;
                        propertyGrid1.SelectedObject = new CGFXPropertyGridSet.CFOG_PropertyGrid(fogs);
                    }
                }
                if (Set.Length == 4)
                {
                    var ht = CGFX.DICTAndSectionData[Set[1]].DICT_Entries.Find(x => x.Name == Set[2]).CGFXData.CGFXSectionData;
                }
                if (Set.Length == 5)
                {
                    var ht = CGFX.DICTAndSectionData[Set[1]].DICT_Entries.Find(x => x.Name == Set[2]).CGFXData.CGFXSectionData;

                    if (Set[1] == "Model")
                    {
                        var Models = ht.CMDLSection;
                        propertyGrid3.SelectedObject = null;
                        if (Set[3] == "Mesh")
                        {
                            propertyGrid3.SelectedObject = Models.meshDatas.Find(x => x.SOBJData.Meshes.MeshName == Set[4]).SOBJData.Meshes;
                        }
                        if (Set[3] == "Shape")
                        {
                            propertyGrid3.SelectedObject = Models.shapeDatas[Convert.ToInt32(Set[4])].SOBJData.Shapes;
                            var i = Models.shapeDatas[Convert.ToInt32(Set[4])].SOBJData.Shapes.VertexAttributes.Select(x => x.Streams.PolygonList).ToList();
                        }
                    }
                }
            }
        }
    }
}
