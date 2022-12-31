using HelixToolkit.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using static CGFX_Viewer.CGFXFormat.SOBJ.Shape.VertexAttribute;
using static CGFX_Viewer.CGFXFormat.Transform;
using static CGFX_Viewer.HTK_3DES.TSRSystem.TSRSystem3D;
using static CGFX_Viewer.VertexAttribute;

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
                            var mt = r.CGFXData.CGFXSectionData.CMDLSection.MTOB_DICT.DICT_Entries.Select(x => new TreeNode(x.CGFXData.CGFXSectionData.MTOBSection.Name)).ToList();
                            //var nt = r.CGFXData.CGFXSectionData.CMDLSection.shapeDatas.Select(x => new TreeNode(x.SOBJData.Shapes.Name)).ToList();
                            var sh = r.CGFXData.CGFXSectionData.CMDLSection.shapeDatas.Select((x, Id) => new { Id, x }).Select(x => new TreeNode(x.Id.ToString())).ToList();

                            var mtName = r.CGFXData.CGFXSectionData.CMDLSection.UnknownDICT.DICT_Entries.Select(x => new TreeNode(x.CGFXData.NativeDataSections.CMDL_Native.MaterialName_Set.Name)).ToList();

                            TreeNode treeNode = new TreeNode(r.Name);
                            treeNode.Nodes.Add(new TreeNode("Mesh", lt.ToArray()));
                            treeNode.Nodes.Add(new TreeNode("Material", mt.ToArray()));
                            treeNode.Nodes.Add(new TreeNode("Shape", sh.ToArray()));
                            treeNode.Nodes.Add(new TreeNode("LinkedMaterial", mtName.ToArray()));

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
                    // "Model", "Textures", "LUTS", "Materials", "Shaders", "Cameras", "Lights", "Fog", "Environments", "Skeleton_Animations", "Texture_Animations", "Visibility_Animations", "Camera_Animations", "Light_Animations", "Fog_Animations", "Emitters" 

                    var ht = CGFX.DICTAndSectionData[Set[1]].DICT_Entries.Find(x => x.Name == Set[2]).CGFXData.CGFXSectionData;

                    if (Set[1] == "Model")
                    {
                        var Models = ht.CMDLSection;
                        propertyGrid3.SelectedObject = new CGFXPropertyGridSet.CMDL_PropertyGrid(Models);

                        //Get TextureName
                        Dictionary<int, ArrayList> MaterialDictionary = new Dictionary<int, ArrayList>();
                        foreach (var MTOB in Models.MTOB_DICT.DICT_Entries.Select((value, i) => new { Value = value, Index = i }))
                        {
                            var DICTName = MTOB.Value.Name;

                            var MTOBSectionData = MTOB.Value.CGFXData.CGFXSectionData.MTOBSection;
                            ArrayList arrayList = new ArrayList();
                            arrayList.AddRange(new object[] { MTOBSectionData.Name, MTOBSectionData.UnknownDataAreas, MTOBSectionData.GetMaterialInfoSet() });
                            MaterialDictionary.Add(MTOB.Index, arrayList);
                        }

                        //Get Texture (Bitmap)
                        Dictionary<string, CGFXFormat.CGFXSection.TXOB.Texture> CMDL_BitmapDictionary = new Dictionary<string, CGFXFormat.CGFXSection.TXOB.Texture>();
                        foreach (var df in CGFX.DICTAndSectionData["Textures"].DICT_Entries)
                        {
                            string s = df.Name;

                            string TextureName = df.CGFXData.CGFXSectionData.TXOBSection.TextureSection.Name;
                            CMDL_BitmapDictionary.Add(TextureName, df.CGFXData.CGFXSectionData.TXOBSection.TextureSection);
                        }

                        foreach (var qs in Models.meshDatas)
                        {
                            int MtlId = qs.SOBJData.Meshes.MaterialIndex;
                            string MaterialName = (string)MaterialDictionary[MtlId][0];
                            CGFXFormat.CGFXSection.MTOB.UnknownDataArea unknownDataArea = (CGFXFormat.CGFXSection.MTOB.UnknownDataArea)MaterialDictionary[MtlId][1];
                            List<CGFXFormat.CGFXSection.MTOB.MaterialInfoSet> MaterialInfoSetList = MaterialDictionary[MtlId][2] as List<CGFXFormat.CGFXSection.MTOB.MaterialInfoSet>;

                            int ShapeID = qs.SOBJData.Meshes.ShapeIndex;
                            var Shape = Models.shapeDatas[ShapeID].SOBJData.Shapes;
                            var indexStreamCtrs = Shape.primitiveSets.Select(x => x.GetIndexStreamCtrPrimitive()).ToList();

                            foreach (var VertexAttr in Shape.VertexAttributes.Select((value, i) => new { Value = value, Index = i }))
                            {
                                if (VertexAttr.Value.Flag.IdentFlag.SequenceEqual(new byte[] { 0x02, 0x00, 0x00, 0x40 }))
                                {
                                    HTK_3DES.CustomMeshBuildHelper.Mesh mesh = new HTK_3DES.CustomMeshBuildHelper.Mesh(true, true, true);
                                    for (int q = 0; q < indexStreamCtrs[0][0].Count; q++) mesh.AddTriangleIndicesArray(indexStreamCtrs[0][0][q].FaceArray.ToArray());
                                    foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                                    {
                                        var Vertex = ym.Scaled<Point3D>(Polygon.DataType.Vt);
                                        var Normal = ym.Scaled<Vector3D>(Polygon.DataType.Nr);
                                        var TexCoord = ym.Scaled<Polygon.TextureCoordinate>(Polygon.DataType.TexCoord0).ToPoint();
                                        mesh.Add(Vertex, Normal, TexCoord);
                                    }

                                    MeshGeometry3D meshGeometry3D = mesh.ToMeshGeometry3D(true);

                                    MaterialGroup material = new MaterialGroup();
                                    material.SetName(MaterialName);

                                    foreach (var i in MaterialInfoSetList)
                                    {
                                        var MatName = i.TXOBDataSection.TXOB.MaterialInfoSection.MTOB_MaterialName;

                                        if (MatName != null)
                                        {
                                            if (CMDL_BitmapDictionary[MatName].TXOB_Bitmap != null)
                                            {
                                                //Create Texture
                                                HTK_3DES.CustomMeshBuildHelper.Texture texture = new HTK_3DES.CustomMeshBuildHelper.Texture(CMDL_BitmapDictionary[MatName].TXOB_Bitmap);
                                                var imageBrush = texture.ToImageBrush(1, 1, unknownDataArea.CalculateTextureCoordinateTypeValue);

                                                if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.LA8)
                                                {
                                                    SpecularMaterial specularMaterial = new SpecularMaterial(imageBrush, 1.0);
                                                    specularMaterial.SetName((string)CMDL_BitmapDictionary[MatName].TXOB_Bitmap.Tag);
                                                    material.Children.Add(specularMaterial);
                                                }
                                                else if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.HILO8)
                                                {
                                                    SpecularMaterial specularMaterial = new SpecularMaterial(imageBrush, 1.0);
                                                    specularMaterial.SetName((string)CMDL_BitmapDictionary[MatName].TXOB_Bitmap.Tag);
                                                    material.Children.Add(specularMaterial);
                                                }
                                                else
                                                {
                                                    DiffuseMaterial Mtl = new DiffuseMaterial(imageBrush);
                                                    Mtl.SetName((string)CMDL_BitmapDictionary[MatName].TXOB_Bitmap.Tag);
                                                    material.Children.Add(Mtl);
                                                }
                                            }
                                        }
                                    }

                                    var m3dGrp = new Model3DGroup();
                                    m3dGrp.Children.Add(new GeometryModel3D { Geometry = meshGeometry3D, Material = material, BackMaterial = material });

                                    ModelVisual3D m = new ModelVisual3D { Content = m3dGrp };
                                    //mList.Add(m);

                                    //表示
                                    render.MainViewPort.Children.Add(m);
                                    render.UpdateLayout();

                                    #region GetPointColor (Test)
                                    //foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                                    //{
                                    //    PointsVisual3D pointsVisual3D = new PointsVisual3D();
                                    //    pointsVisual3D.Points = new Point3DCollection();
                                    //    pointsVisual3D.Points.Add(ym.Vertex);
                                    //    pointsVisual3D.Color = System.Windows.Media.Color.FromArgb(ym.ColorData.A, ym.ColorData.R, ym.ColorData.G, ym.ColorData.B);
                                    //    pointsVisual3D.Size = 5;

                                    //    render.MainViewPort.Children.Add(pointsVisual3D);
                                    //    render.UpdateLayout();
                                    //}
                                    #endregion

                                    #region Point3D Only (Test)
                                    //foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                                    //{
                                    //    //List<Point3D> point3Ds = new List<Point3D>();
                                    //    //point3Ds.Add(ym.Vertex);

                                    //    PointsVisual3D pointsVisual3D = new PointsVisual3D();
                                    //    pointsVisual3D.Points = new Point3DCollection();
                                    //    pointsVisual3D.Points.Add(ym.Vertex);
                                    //    pointsVisual3D.Color = System.Windows.Media.Color.FromArgb(ym.ColorData.A, ym.ColorData.R, ym.ColorData.G, ym.ColorData.B);
                                    //    pointsVisual3D.Size = 5;

                                    //    render.MainViewPort.Children.Add(pointsVisual3D);
                                    //    render.UpdateLayout();


                                    //    //List<Point3D> point3Ds = new List<Point3D>();
                                    //    //point3Ds.Add(ym.Vertex);

                                    //    //PointsVisual3D pointsVisual3D = new PointsVisual3D();
                                    //    //pointsVisual3D.Points = new Point3DCollection(point3Ds);
                                    //    //pointsVisual3D.Color = System.Windows.Media.Color.FromArgb(ym.ColorData.A, ym.ColorData.R, ym.ColorData.G, ym.ColorData.B);
                                    //    //pointsVisual3D.Size = 5;

                                    //    //render.MainViewPort.Children.Add(pointsVisual3D);
                                    //    //render.UpdateLayout();
                                    //}
                                    #endregion
                                }
                                if (VertexAttr.Value.Flag.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x80 }))
                                {

                                }
                            }
                        }
                    }
                    if (Set[1] == "Textures")
                    {
                        var Textures = ht.TXOBSection.TextureSection;
                        pictureBox1.Image = Textures.TXOB_Bitmap;
                        propertyGrid2.SelectedObject = new CGFXPropertyGridSet.TXOB_PropertyGrid(Textures);
                    }
                    if (Set[1] == "LUTS") return;
                    if (Set[1] == "Materials") return;
                    if (Set[1] == "Shaders") return;
                    if (Set[1] == "Cameras") return;
                    if (Set[1] == "Lights") return;
                    if (Set[1] == "Fog")
                    {
                        var fogs = ht.CFOGSection;
                        propertyGrid1.SelectedObject = new CGFXPropertyGridSet.CFOG_PropertyGrid(fogs);
                    }
                    if (Set[1] == "Environments")
                    {

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
                            //propertyGrid3.SelectedObject = Models.meshDatas.Find(x => x.SOBJData.Meshes.MeshName == Set[4]).SOBJData.Meshes;
                            propertyGrid3.SelectedObject = new CGFXPropertyGridSet.CMDL_MeshData_PropertyGrid(Models.meshDatas.Find(x => x.SOBJData.Meshes.MeshName == Set[4]));
                        }
                        if (Set[3] == "Material")
                        {
                            //propertyGrid3.SelectedObject = Models.MTOB_DICT.DICT_Entries.Find(x => x.Name == Set[4]).CGFXData.CGFXSectionData.MTOBSection;

                            propertyGrid3.SelectedObject = new CGFXPropertyGridSet.MTOB_PropertyGrid(Models.MTOB_DICT.DICT_Entries.Find(x => x.Name == Set[4]).CGFXData.CGFXSectionData.MTOBSection);
                        }
                        if (Set[3] == "Shape")
                        {
                            //propertyGrid3.SelectedObject = Models.shapeDatas[Convert.ToInt32(Set[4])].SOBJData.Shapes;
                            propertyGrid3.SelectedObject = new PropertyGridForms.Section.CMDL.ShapeData.ShapeData_PropertyGrid(Models.shapeDatas[Convert.ToInt32(Set[4])].SOBJData.Shapes);

                            //var i = Models.shapeDatas[Convert.ToInt32(Set[4])].SOBJData.Shapes.VertexAttributes.Select(x => x.Streams.PolygonList).ToList();
                        }
                        if (Set[3] == "LinkedMaterial")
                        {
                            propertyGrid3.SelectedObject = Models.UnknownDICT.DICT_Entries.Find(x => x.Name == Set[4]).CGFXData.NativeDataSections.CMDL_Native.MaterialName_Set.Name;
                        }
                    }
                }
            }
        }
    }
}
