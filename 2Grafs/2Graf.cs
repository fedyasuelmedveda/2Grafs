using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace _2Grafs
{
    struct Vertex : IComparable
    {
        public int x;
        public int y;
        public double weight;
        public double wayweight;
        public Vertex(int x, int y, double weight, double wayweight)
        {
            this.x = x;
            this.y = y;
            this.weight = weight;
            this.wayweight = wayweight;
        }
        public static Vertex operator<(Vertex v1, Vertex v2)
        {
            if (v1.wayweight > v2.wayweight)
                return v2;
            return v1;
        }
        public static Vertex operator >(Vertex v1, Vertex v2)
        {
            if (v1.wayweight < v2.wayweight)
                return v2;
            return v1;
        }
        public int CompareTo(object o)
        {
            if (o is Vertex v)
            {
                if (v.wayweight < wayweight)
                    return 1;
                else
                if (v.wayweight > wayweight)
                    return -1;
                else
                if (v.x < x)
                    return 1;
                else
                if (v.x > x)
                    return -1;
                else
                if (v.y < y)
                    return 1;
                else
                if (v.y > y)
                    return -1;
                else
                    return 0;
            }
            else
                return -1; 
        }

    }
    internal class _2Graf
    {
        private Vertex[,] Vertexes;
        private bool[,] Allowed;
        private int[,] W;
        private int[] I;
        private int sizex,sizey;
        private Random r;
        
        public _2Graf(int sizex, int sizey) 
        {
            this.sizex = sizex;
            this.sizey = sizey;
            W = new int[sizex, sizey];
            Vertexes = new Vertex[sizex,sizey];
            Allowed = new bool[sizex,sizey];
            ResetWeights();
            r = new Random();
            for(int i =0; i< sizex; i++)
            {
                for(int j = 0;j < sizey; j++)
                {
                    Allowed[i, j] = true;
                    Vertexes[i, j] = new Vertex(i, j,1/*r.Next(1,6)*/,sizex*sizey*5);

                }
            }
        }
        public int GetSizeX()
        {
            return sizex;
        }
        public int GetSizeY()
        {
            return sizey;
        }
        public void SetWeight(int i, int j, int value)
        {
            W[i, j] = value;
            Vertexes[i, j].weight = value;
        }
        public void ResetWeights()
        {
            for(int i = 0; i < sizex; i++)
            {
                for(int j = 0; j < sizey; j++)
                {
                    W[i, j] = sizex * sizey;

                }
            }
        }
        public void Wall(int i,int j) {
            Allowed[i, j] = false;
        }
        public void BFS(int i, int j)
        {
            SortedSet<Vertex> s = new SortedSet<Vertex>();
            Queue<Vertex> q = new Queue<Vertex>();
            Vertexes[i, j].wayweight = 0;
            q.Enqueue(Vertexes[i, j]);
            s.Add(Vertexes[i, j]);
            W[i, j] = 0;
            while(s.Count > 0)
            {
                Vertex v = s.Min;
                s.Remove(v);

                AddNeighbours(v, s, 0);
            }
        }
        private void AddNeighbours(Vertex v, SortedSet<Vertex> s, int mode)
        {
            if (mode == 0)
            {
                if (v.x < sizex - 1)
                    if (Vertexes[v.x + 1, v.y].wayweight == sizex * sizey * 5 && Allowed[v.x + 1, v.y] == true)
                    {
                        Vertexes[v.x+1, v.y].wayweight = v.wayweight + Vertexes[v.x + 1, v.y].weight;
                        s.Add(Vertexes[v.x+1,v.y]);
                        W[v.x + 1, v.y] = W[v.x, v.y] + 1;
                    }
                if (v.x > 0)
                    if (Vertexes[v.x - 1, v.y].wayweight == sizex * sizey * 5 && Allowed[v.x - 1, v.y] == true)
                    {
                        Vertexes[v.x - 1, v.y].wayweight = v.wayweight + Vertexes[v.x - 1, v.y].weight;
                        s.Add(Vertexes[v.x - 1, v.y]);
                        W[v.x - 1, v.y] = W[v.x, v.y] + 1;
                    }
                if (v.y < sizey - 1)
                    if (Vertexes[v.x, v.y + 1].wayweight == sizex * sizey * 5 && Allowed[v.x, v.y + 1] == true)
                    {
                        Vertexes[v.x, v.y + 1].wayweight = v.wayweight + Vertexes[v.x, v.y + 1].weight;
                        s.Add(Vertexes[v.x, v.y + 1]);
                        W[v.x, v.y + 1] = W[v.x, v.y] + 1;
                    }
                if (v.y > 0)
                    if (Vertexes[v.x, v.y - 1].wayweight == sizex * sizey * 5 && Allowed[v.x, v.y - 1] == true)
                    {
                        Vertexes[v.x, v.y - 1].wayweight = v.wayweight + Vertexes[v.x, v.y - 1].weight;
                        s.Add(Vertexes[v.x, v.y - 1]);
                        W[v.x, v.y - 1] = W[v.x, v.y] + 1;
                    }
            }
        }

        public void DrawGraf(double l, double r, double u, double d, int mode)
        {
            double max = 0;
            for(int i = 0; i < sizex; i++)
            {
                for(int j = 0;j < sizey; j++)
                {
                    if (Vertexes[i,j].wayweight!= sizex*sizey*5 && Vertexes[i,j].wayweight > max)
                        max = Vertexes[i,j].wayweight;
                }
            }
            GL.Begin(PrimitiveType.Quads);
            double deltax = (r - l) / sizex;
            double deltay = (u - d) / sizey;
            for(int i = 0; i< sizex; i++)
            {
                for(int j = 0; j < sizey; j++)
                {
                    if (Allowed[i, j] == false)
                        GL.Color3(Color.Black);
                    else
                    {
                        if (mode == 0)
                            GL.Color3(Math.Sqrt((double)Vertexes[i, j].wayweight / max), 1.0, 1.0);
                        if (mode == 1)
                            GL.Color3(Math.Sqrt((double)Vertexes[i, j].weight / 5), 1.0, 1.0);
                    }
                        GL.Vertex2(l + i * deltax, d + j * deltay);
                    GL.Vertex2(l + (i + 1) * deltax, d +j * deltay);
                    GL.Vertex2(l + (i + 1) * deltax, d + (j + 1) * deltay);
                    GL.Vertex2(l + i * deltax, d + (j + 1) * deltay);
                }
            }
            GL.End();
        }
    }
}
