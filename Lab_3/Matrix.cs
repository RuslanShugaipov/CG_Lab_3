using System;

namespace Lab_3
{
    public class Matrix
    {
        private float[,] data;

        private int m;
        public int M
        {
            get { return m; }
        }

        private int n;
        public int N
        {
            get { return n; }
        }

        public Matrix(float[,] data)
        {
            m = data.GetUpperBound(0) + 1;
            n = data.Length / m;
            this.data = data;
        }

        public Matrix(int m, int n)
        {
            this.m = m;
            this.n = n;
            data = new float[m, n];
        }

        public float this[int x, int y]
        {
            get { return data[x, y]; }
            set { data[x, y] = value; }
        }

        public static Matrix operator *(Matrix matrixA, Matrix matrixB)
        {
            if (matrixA.N != matrixB.M)
            {
                throw new ArgumentException("Matrixes cannot be multiplied!");
            }
            var result = new Matrix(matrixA.M, matrixB.N);
            for (int i = 0; i < matrixA.M; ++i)
            {
                for (int k = 0; k < matrixB.N; ++k)
                {
                    for (int j = 0; j < matrixB.N; ++j)
                    {
                        result[i, k] += matrixA[i, j] * matrixB[j, k];
                    }
                }
            }
            return result;
        }
    }
}