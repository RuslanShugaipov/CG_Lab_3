using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3
{
    class Transformation
    {
        private static Transformation instance;

        private Transformation()
        {

        }

        public static Transformation getInstance()
        {
            if (instance == null)
                instance = new Transformation();
            return instance;
        }

        public Matrix rotationX(Matrix point, double angle)
        {
            angle = -angle * (Math.PI / 180);

            Matrix _point = new Matrix(new float[1, 4] { { point[0, 0], point[0, 1], point[0, 2], 0 } });
            Matrix new_point = new Matrix(new float[1, 4] { { 0, 0, 0, 0 } });

            Matrix rotationX = new Matrix(new float[4, 4]{
                {    1,                         0,                         0, 0 },
                {    0,    (float)Math.Cos(angle),    (float)Math.Sin(angle), 0 },
                {    0, -((float)Math.Sin(angle)),    (float)Math.Cos(angle), 0 },
                {    0,                         0,                         0, 1 }
            });
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    new_point[0, i] += _point[0, j] * rotationX[j, i];
                }
            }
            return new_point;
        }

        public Matrix rotationY(Matrix point, double angle)
        {
            angle = -angle * (Math.PI / 180);

            Matrix _point = new Matrix(new float[1, 4] { { point[0, 0], point[0, 1], point[0, 2], 0 } });
            Matrix new_point = new Matrix(new float[1, 4] { { 0, 0, 0, 0 } });

            Matrix rotationY = new Matrix(new float[4, 4]{
                {    (float)Math.Cos(angle), 0, -((float)Math.Sin(angle)), 0 },
                {                         0, 1,                         0, 0 },
                {    (float)Math.Sin(angle), 0,    (float)Math.Cos(angle), 0 },
                {                         0, 0,                         0, 1 }
            });
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    new_point[0, i] += _point[0, j] * rotationY[j, i];
                }
            }
            return new_point;
        }

        public Matrix rotationZ(Matrix point, double angle)
        {
            angle = -angle * (Math.PI / 180);

            Matrix _point = new Matrix(new float[1, 4] { { point[0, 0], point[0, 1], point[0, 2], 0 } });
            Matrix new_point = new Matrix(new float[1, 4] { { 0, 0, 0, 0 } });

            Matrix rotationZ = new Matrix(new float[4, 4]{
                {    (float)Math.Cos(angle), (float)Math.Sin(angle), 0, 0 },
                { -((float)Math.Sin(angle)), (float)Math.Cos(angle), 0, 0 },
                {                         0,                      0, 1, 0 },
                {                         0,                      0, 0, 1 }
            });
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    new_point[0, i] += _point[0, j] * rotationZ[j, i];
                }
            }
            return new_point;
        }

        public Matrix toIsometric(Matrix point)
        {
            double angleY = -45 * (Math.PI / 180);
            double angleX = -35.264 * (Math.PI / 180);

            Matrix _point = new Matrix(new float[1, 4] { { point[0, 0], point[0, 1], point[0, 2], 0 } });
            Matrix new_point = new Matrix(new float[1, 4] { { 0, 0, 0, 0 } });

            Matrix rotationYX = new Matrix(new float[4, 4]{
                {    (float)Math.Cos(angleY),  (float)Math.Sin(angleX) *(float)Math.Sin(angleY), -(float)Math.Sin(angleY) *(float)Math.Cos(angleX), 0 },
                {                          0,                           (float)Math.Cos(angleX),                           (float)Math.Sin(angleX), 0 },
                {    (float)Math.Sin(angleY), -(float)Math.Sin(angleX) *(float)Math.Cos(angleY),  (float)Math.Cos(angleX) *(float)Math.Cos(angleY), 0 },
                {                          0,                                                 0,                                                 0, 1 }
            });
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    new_point[0, i] += _point[0, j] * rotationYX[j, i];
                }
            }
            return new_point;
        }
    }
}
