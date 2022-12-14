using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace AOCUtils {

    //X is row, Y is column number
    public static class Matrix {

        public static T[] Col<T>(this T[,] matrix, int y) =>
            Enumerable.Range(0, matrix.Rows()).Select(x => matrix[x, y]).ToArray();

        public static T[] Row<T>(this T[,] matrix, int x) =>
            Enumerable.Range(0, matrix.Cols()).Select(y => matrix[x, y]).ToArray();

        public static int Cols<T>(this T[,] matrix) => matrix.GetLength(1);

        public static int Rows<T>(this T[,] matrix) => matrix.GetLength(0);

        public static T[,] ToMatrix<T>(this T[][] data) {
            var height = data.Length;
            var width = data[0].Length;
            var matrix = new T[height, width];

            for(int x = 0; x < height; x++) {
                for(int y = 0; y < width; y++) {
                    matrix[x, y] = data[x][y];
                }
            }
            return matrix;
        }

        public static IEnumerable<(int, int)> MooreNeighbors<T>(this T[,] matrix, int x, int y) {
            var offsets = new[] { (-1, 0), (-1, -1), (0, -1), (1,-1), (1, 0), (1,1), (0, 1), (-1, 1) };
            List<(int, int)> n = new();

            foreach (var (xx, yy) in offsets) {
                if (x + xx < 0 || x + xx >= matrix.Rows() || y + yy < 0 || y + yy >= matrix.Cols())
                    n.Add((x + xx, y + yy));
            }
            return n;
        }

        public static IEnumerable<(int, int)> VonNeumannNeighbors<T>(this T[,] matrix, int x, int y) {
            List<(int,int)> n = new();
            foreach(var (xx, yy) in new[] {(-1,0), (0,-1), (1,0), (0,1)}) {
                if(x + xx < 0 || x + xx >= matrix.Rows() || y + yy < 0 || y + yy >= matrix.Cols())
                    n.Add((x+xx, y+yy));
            }
            return n;
        }


        public static (int[,] dist, (int,int)[,] prev) Dijkstra<T>(this T[,] matrix, int sx, int sy, Func<T[,], int, int, IEnumerable<(int, int)>>? neighbors = null, Func<T[,], int, int, bool>? end = null, Func<T[,], int, int, int, int, int>? edge = null) {

            PriorityQueue<(int, int), int> pq = new();

            neighbors ??= (m, x, y) => m.VonNeumannNeighbors(x, y);
            edge ??= (m, x1, y1, x2, y2) => 1;
            end ??= (m, x, y) => pq.Count == 0;


            int[,] dist = new int[matrix.Rows(), matrix.Cols()];
            (int, int)[,] prev = new (int, int)[matrix.Rows(), matrix.Cols()];
            bool[,] visited = new bool[matrix.Rows(), matrix.Cols()];
            for (int i = 0; i < matrix.Rows(); i++) {
                for(int j = 0; j < matrix.Cols(); j++) {
                    dist[i,j] = int.MaxValue;
                    prev[i,j] = (-1, -1);
                }

            }

            dist[sx, sy] = 0;

            pq.Enqueue((sx, sy), 0);

            int x = sx, y = sy;

            while(!end(matrix, x, y)) {
                (x, y) = pq.Dequeue();

                if (visited[x, y]) continue;
                visited[x, y] = true;

                foreach(var (nx, ny) in neighbors(matrix, x, y)) {
                    if (visited[nx, ny]) continue;

                    int alt = dist[x, y] + edge(matrix, x, y, nx, ny);
                    if (alt < dist[x, y]) {
                        dist[x, y] = alt;
                        prev[nx, ny] = (x, y);

                        pq.Enqueue((nx, ny), alt);
                    }
                }
            }

            return (dist, prev);
        }
    }
}