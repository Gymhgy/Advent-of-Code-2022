using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCUtils {
    public static class Vector {

        public static int ManhattanDist(this (int x, int y) p1, (int x, int y) p2) => Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);

        public static int Dot(this (int x, int y) p1, (int x, int y) p2) => (p1.x * p2.x + p1.y * p2.y);

        public static (int x, int y) Add(this (int x, int y) p1, (int x, int y) p2) => (p1.x + p2.x, p1.y + p2.y);

        public static (int x, int y) Subtract(this (int x, int y) p1, (int x, int y) p2) => (p1.x - p2.x, p1.y - p2.y);

        public static int ChebyshevDist(this (int x, int y) p1, (int x, int y) p2) => Math.Max(Math.Abs(p1.x - p2.x), Math.Abs(p1.y - p2.y));

    }
}
