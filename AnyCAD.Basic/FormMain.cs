using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AnyCAD.Platform;

namespace AnyCAD.Basic
{
    public partial class FormMain : Form
    {
        // Render Control
        private Presentation.RenderWindow3d renderView;
        private int shapeId = 100;
        public FormMain()
        {
            InitializeComponent();

            // 
            // Create renderView
            // 
            this.renderView = new AnyCAD.Presentation.RenderWindow3d();
            this.renderView.Location = new System.Drawing.Point(0, 27);
            this.renderView.Size = this.Size;
            this.renderView.TabIndex = 1;
            this.Controls.Add(this.renderView);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            renderView.Size = this.Size;
        }

        private void sphereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopoShape sphere = renderView.ShapeMaker.MakeSphere(new Vector3(0, 0, 0), 40);
            renderView.ShowGeometry(sphere, ++shapeId);
        }

        private void boxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopoShape box = renderView.ShapeMaker.MakeBox(new Vector3(40, -20, 0), new Vector3(0, 0, 1), new Vector3(30, 40, 60));

            SceneNode sceneNode = renderView.ShowGeometry(box, ++shapeId);

            FaceStyle style = new FaceStyle();
            style.SetColor(new ColorValue(0.5f, 0.3f, 0, 1));
            sceneNode.SetFaceStyle(style);
        }

        private void cylinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopoShape cylinder = renderView.ShapeMaker.MakeCylinder(new Vector3(80, 0, 0), new Vector3(0, 0, 1), 20, 100, 315);
            SceneNode sceneNode = renderView.ShowGeometry(cylinder, ++shapeId);
            FaceStyle style = new FaceStyle();
            style.SetColor(new ColorValue(0.1f, 0.3f, 0.8f, 1));
            sceneNode.SetFaceStyle(style);
        }

        private void coneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopoShape cylinder = renderView.ShapeMaker.MakeCone(new Vector3(120, 0, 0), new Vector3(0, 0, 1), 20, 100, 40, 315);
            renderView.ShowGeometry(cylinder, ++shapeId);
        }

        private void extrudeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int size = 20;
            // Create the outline edge
            TopoShape arc = renderView.ShapeMaker.MakeArc3Pts(new Vector3(-size, 0, 0), new Vector3(size, 0, 0), new Vector3(0, size, 0));
            TopoShape line1 = renderView.ShapeMaker.MakeLine(new Vector3(-size, -size, 0), new Vector3(-size, 0, 0));
            TopoShape line2 = renderView.ShapeMaker.MakeLine(new Vector3(size, -size, 0), new Vector3(size, 0, 0));
            TopoShape line3 = renderView.ShapeMaker.MakeLine(new Vector3(-size, -size, 0), new Vector3(size, -size, 0));

            TopoShapeGroup shapeGroup = new TopoShapeGroup();
            shapeGroup.Add(line1);
            shapeGroup.Add(arc);
            shapeGroup.Add(line2);
            shapeGroup.Add(line3);

            TopoShape wire = renderView.ShapeMaker.MakeWire(shapeGroup);
            TopoShape face = renderView.ShapeMaker.MakeFace(wire);

            // Extrude
            TopoShape extrude = renderView.ShapeMaker.Extrude(face, 100, new Vector3(0, 0, 1));
            renderView.ShowGeometry(extrude, ++shapeId);

            // Check find....
            SceneNode findNode = renderView.View3d.GetSceneManager().FindNode(++shapeId);
            renderView.View3d.GetSceneManager().SelectNode(findNode);
        }

        private void revoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int size = 10;
            // Create the outline edge
            TopoShape arc = renderView.ShapeMaker.MakeArc3Pts(new Vector3(-size, 0, 0), new Vector3(size, 0, 0), new Vector3(0, size, 0));
            TopoShape line1 = renderView.ShapeMaker.MakeLine(new Vector3(-size, -size, 0), new Vector3(-size, 0, 0));
            TopoShape line2 = renderView.ShapeMaker.MakeLine(new Vector3(size, -size, 0), new Vector3(size, 0, 0));
            TopoShape line3 = renderView.ShapeMaker.MakeLine(new Vector3(-size, -size, 0), new Vector3(size, -size, 0));

            TopoShapeGroup shapeGroup = new TopoShapeGroup();
            shapeGroup.Add(line1);
            shapeGroup.Add(arc);
            shapeGroup.Add(line2);
            shapeGroup.Add(line3);

            TopoShape wire = renderView.ShapeMaker.MakeWire(shapeGroup);

            TopoShape revole = renderView.ShapeMaker.Revol(wire, new Vector3(size * 3, 0, 0), new Vector3(0, 1, 0), 145);

            renderView.ShowGeometry(revole, ++shapeId);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderView.ClearScene();
        }

        private void sTLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "STL (*.stl)|*.stl|IGES (*.igs)|*.igs|STEP (*.stp)|*.stp|All Files(*.*)|*.*";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                TopoShape shape = renderView.ShapeMaker.LoadFile(dlg.FileName);
                if( shape != null)
                    renderView.ShowGeometry(shape, ++shapeId);

            }

            renderView.View3d.FitAll();
        }

        private void shadeWithEdgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderView.ExecuteCommand("ShadeWithEdgeMode");
            renderView.RequestDraw();
        }

        private void shadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderView.ExecuteCommand("ShadeMode");
            renderView.RequestDraw();
        }

        private void edgeWithPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderView.ExecuteCommand("EdgeMode");
            renderView.RequestDraw();
        }

        private void splitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopoShape cylinder = renderView.ShapeMaker.MakeCylinder(new Vector3(80, 0, 0), new Vector3(0, 0, 1), 20, 100, 315);

            TopoShape planeFace = renderView.ShapeMaker.MakePlaneFace(new Vector3(80, 0, 50), new Vector3(0, 0, 1), -100, 100, -100, 100);

            TopoShape rest = renderView.ShapeMaker.MakeSplit(cylinder, planeFace);
            renderView.ShowGeometry(rest, ++shapeId);
        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopoShapeConvert con = new TopoShapeConvert();
            float[] vb ={0,0,0,100,0,0,100,100,0};
            uint[] ib = { 0, 1, 2 };
            float[] cb = { 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1 };
            float[] nb = {};
            RenderableEntity entity = con.CreateColoredFaceEntity(vb, ib, nb, cb, new AABox(Vector3.ZERO, new Vector3(100, 100, 1)));

            EntitySceneNode node = new EntitySceneNode();
            node.SetEntity(entity);

            renderView.SceneManager.AddNode(node);
            renderView.RequestDraw();
        }

    }
}
