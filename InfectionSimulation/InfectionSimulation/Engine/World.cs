﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfectionSimulation
{
    class World
    {
        private Random rnd = new Random();

        private const int width = 300;
        private const int height = 300;
        private Size size = new Size(width, height);
        private List<GameObject> objects = new List<GameObject>();
        private List<GameObject>[,] spatialObjects = new List<GameObject>[width, height];

        public IEnumerable<GameObject> GameObjects {
            get
            {
                return objects.ToArray();
            }
        }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Point Center { get { return new Point(width / 2, height / 2); } }

        public bool IsInside(Point p)
        {
            return p.X >= 0 && p.X < width
                && p.Y >= 0 && p.Y < height;
        }
        
        public Point RandomPoint()
        {
            return new Point(rnd.Next(width), rnd.Next(height));
        }

        public float Random()
        {
            return (float)rnd.NextDouble();
        }

        public int Random(int min, int max)
        {
            return rnd.Next(min, max);
        }

        private List<GameObject> GetBucketAt(Point position)
        {
            var bucket = spatialObjects[position.X, position.Y];
            if (bucket == null)
            {
                bucket = new List<GameObject>();
                spatialObjects[position.X, position.Y] = bucket;
            }
            return bucket;
        }

        public void Add(GameObject obj)
        {
            objects.Add(obj);
            GetBucketAt(obj.Position).Add(obj);
        }

        public void Remove(GameObject obj)
        {
            objects.Remove(obj);
            GetBucketAt(obj.Position).Remove(obj);
        }

        public void Update()
        {
            foreach (GameObject obj in GameObjects)
            {
                var oldPosition = obj.Position;

                obj.InternalUpdateOn(this);
                obj.Position = Mod(obj.Position, size);

                if (obj.Position != oldPosition)
                {
                    GetBucketAt(oldPosition).Remove(obj);
                    GetBucketAt(obj.Position).Add(obj);
                }
            }
        }

        public void DrawOn(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Black, 0, 0, width, height);

            foreach (GameObject obj in GameObjects)
            {
                graphics.FillRectangle(new Pen(obj.Color).Brush, obj.Bounds);
            }
        }

        public double Dist(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public double Dist(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        // http://stackoverflow.com/a/10065670/4357302
        private static int Mod(int a, int n)
        {
            int result = a % n;
            if ((a < 0 && n > 0) || (a > 0 && n < 0))
                result += n;
            return result;
        }
        private static Point Mod(Point p, Size s)
        {
            return new Point(Mod(p.X, s.Width), Mod(p.Y, s.Height));
        }

        public IEnumerable<GameObject> ObjectsAt(Point pos)
        {
            return GetBucketAt(pos);
        }

    }
}
