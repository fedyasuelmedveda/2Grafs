using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace _2Grafs
{
    public partial class Form1 : Form
    {
        _2Graf g;
        int mode = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-2, 2, -2, 2, -2, 2);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            g = new _2Graf(100, 100);
            Draw(0);
        }
        void Draw(int mode)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            g.DrawGraf(-2, 2, 2, -2, mode);
            glControl1.SwapBuffers();
        }
        private void glControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (mode == 2)
            {
                int sx = g.GetSizeX();
                int sy = g.GetSizeY();
                Point p = new Point(e.Location.X * sx / glControl1.Width, sy - e.Location.Y * sy / glControl1.Height - 1);
                g.BFS(p.X, p.Y);
                Draw(0);
            }
            else
            if (mode == 1)
                mode = 0;
            else
            if (mode == 0)
                mode = 1;
            //g.DrawGraf(-2,2,-2,2);
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mode == 0 && trackBar1.Value == 5)
            {
                int sx = g.GetSizeX();
                int sy = g.GetSizeY();
                Point p = new Point(e.Location.X * sx / glControl1.Width, sy - e.Location.Y * sy / glControl1.Height - 1);
                g.Wall(p.X, p.Y);
                Draw(1);
            }
            else
            if(mode == 0)
            {
                int sx = g.GetSizeX();
                int sy = g.GetSizeY();
                Point p = new Point(e.Location.X * sx / glControl1.Width, sy - e.Location.Y * sy / glControl1.Height - 1);
                g.SetWeight(p.X, p.Y, trackBar1.Value);
                Draw(1);

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            mode = 2;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }
    }
}
