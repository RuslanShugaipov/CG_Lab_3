using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private List<Matrix> biLin(Matrix point00, Matrix point01, Matrix point10, Matrix point11)
        {
            List<Matrix> points = new List<Matrix>();

            float x = 0, y = 0, z = 0;

            for (float u = 0; u <= 1; u += 0.02f)
            {
                for (float w = 0; w <= 1; w += 0.02f)
                {
                    x = point00[0, 0] * (1 - u) * (1 - w) + point01[0, 0] * (1 - u) * w + point10[0, 0] * (1 - w) * u + point11[0, 0] * u * w;
                    y = point00[0, 1] * (1 - u) * (1 - w) + point01[0, 1] * (1 - u) * w + point10[0, 1] * (1 - w) * u + point11[0, 1] * u * w;
                    z = point00[0, 2] * (1 - u) * (1 - w) + point01[0, 2] * (1 - u) * w + point10[0, 2] * (1 - w) * u + point11[0, 2] * u * w;
                    points.Add(new Matrix(new float[1, 3] { { x, y, z } }));
                }
            }
            return points;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Matrix point00 = new Matrix(new float[1, 3] { { 0f, -300f, 0f } });
            Matrix point01 = new Matrix(new float[1, 3] { { 300f, 0f, 0f } });
            Matrix point10 = new Matrix(new float[1, 3] { { 0f, 0f, 300f } });
            Matrix point11 = new Matrix(new float[1, 3] { { 150f, -150f, 150f } });

            List<Matrix> points2 = biLin(point00, point01, point10, point11);

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;

            Pen red_pen = new Pen(Color.Red);
            Pen black_pen = new Pen(Color.Black);

            PointF point = new PointF(0, -height / 2);

            g.TranslateTransform(width / 2, height / 2);

            for (int i = 1; i < points2.Count(); ++i)
            {
                g.DrawLine(red_pen, toIsometric(new Matrix(new float[1, 3] { { points2[i - 1][0, 0], points2[i - 1][0, 1], points2[i - 1][0, 2] } })),
                   toIsometric(new Matrix(new float[1, 3] { { points2[i][0, 0], points2[i][0, 1], points2[i][0, 2] } })));
            }

            //Axis
            g.DrawLine(black_pen, new PointF(0, 0), point);
            g.DrawLine(black_pen, new PointF(0, 0), rotationZ(new Matrix(new float[1, 3] { { point.X, point.Y, 0 } }), 120));
            g.DrawLine(black_pen, new PointF(0, 0), rotationZ(new Matrix(new float[1, 3] { { point.X, point.Y, 0 } }), -120));

            g.DrawLine(red_pen, toIsometric(new Matrix(new float[1, 3] { { point00[0, 0], point00[0, 1], point00[0, 2] } })),
                toIsometric(new Matrix(new float[1, 3] { { point01[0, 0], point01[0, 1], point01[0, 2] } })));
            g.DrawLine(red_pen, toIsometric(new Matrix(new float[1, 3] { { point01[0, 0], point01[0, 1], point01[0, 2] } })),
                toIsometric(new Matrix(new float[1, 3] { { point11[0, 0], point11[0, 1], point11[0, 2] } })));
            g.DrawLine(red_pen, toIsometric(new Matrix(new float[1, 3] { { point11[0, 0], point11[0, 1], point11[0, 2] } })),
                toIsometric(new Matrix(new float[1, 3] { { point10[0, 0], point10[0, 1], point10[0, 2] } })));
            g.DrawLine(red_pen, toIsometric(new Matrix(new float[1, 3] { { point10[0, 0], point10[0, 1], point10[0, 2] } })),
                toIsometric(new Matrix(new float[1, 3] { { point00[0, 0], point00[0, 1], point00[0, 2] } })));
        }

        public PointF rotationX(Matrix point, double angle)
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
            return new PointF(new_point[0, 0], new_point[0, 1]);
        }

        public PointF rotationY(Matrix point, double angle)
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
            return new PointF(new_point[0, 0], new_point[0, 1]);
        }

        public PointF rotationZ(Matrix point, double angle)
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
            return new PointF(new_point[0, 0], new_point[0, 1]);
        }

        public PointF toIsometric(Matrix point)
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
            return new PointF(new_point[0, 0], new_point[0, 1]);
        }

        private static double angleX = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            if (angleX == 360)
            {
                angleX = 0;
            }
            else
            {
                angleX += 5;
            }

            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                g.Clear(pictureBox1.BackColor);
                pictureBox1.Invalidate();

                Matrix point00 = new Matrix(new float[1, 3] { { 0f, -300f, 0f } });
                Matrix point01 = new Matrix(new float[1, 3] { { 300f, 0f, 0f } });
                Matrix point10 = new Matrix(new float[1, 3] { { 0f, 0f, 300f } });
                Matrix point11 = new Matrix(new float[1, 3] { { 150f, -150f, 150f } });

                PointF point00R = rotationZ(point00, angleX);
                PointF point01R = rotationZ(point01, angleX);
                PointF point10R = rotationZ(point10, angleX);
                PointF point11R = rotationZ(point11, angleX);

                List<Matrix> points2 = biLin(
                    new Matrix(new float[1, 3] { { point00R.X, point00R.Y, 150f } }),
                    new Matrix(new float[1, 3] { { point01R.X, point01R.Y, 150f } }),
                    new Matrix(new float[1, 3] { { point10R.X, point10R.Y, 0f } }),
                    new Matrix(new float[1, 3] { { point11R.X, point11R.Y, 150f } }));

                int width = pictureBox1.Width;
                int height = pictureBox1.Height;

                Pen red_pen = new Pen(Color.Red);
                Pen black_pen = new Pen(Color.Black);

                PointF point = new PointF(0, -height / 2);

                g.TranslateTransform(width / 2, height / 2);

                for (int i = 1; i < points2.Count(); ++i)
                {
                    g.DrawLine(red_pen, toIsometric(new Matrix(new float[1, 3] { { points2[i - 1][0, 0], points2[i - 1][0, 1], points2[i - 1][0, 2] } })),
                       toIsometric(new Matrix(new float[1, 3] { { points2[i][0, 0], points2[i][0, 1], points2[i][0, 2] } })));
                }

                //Axis
                g.DrawLine(black_pen, new PointF(0, 0), point);
                g.DrawLine(black_pen, new PointF(0, 0), rotationZ(new Matrix(new float[1, 3] { { point.X, point.Y, 0 } }), 120));
                g.DrawLine(black_pen, new PointF(0, 0), rotationZ(new Matrix(new float[1, 3] { { point.X, point.Y, 0 } }), -120));

                //g.DrawLine(red_pen, toIsometric(new Matrix(new float[1, 3] { { point00[0, 0], point00[0, 1], point00[0, 2] } })),
                //    toIsometric(new Matrix(new float[1, 3] { { point01[0, 0], point01[0, 1], point01[0, 2] } })));
                //g.DrawLine(red_pen, toIsometric(new Matrix(new float[1, 3] { { point01[0, 0], point01[0, 1], point01[0, 2] } })),
                //    toIsometric(new Matrix(new float[1, 3] { { point11[0, 0], point11[0, 1], point11[0, 2] } })));
                //g.DrawLine(red_pen, toIsometric(new Matrix(new float[1, 3] { { point11[0, 0], point11[0, 1], point11[0, 2] } })),
                //    toIsometric(new Matrix(new float[1, 3] { { point10[0, 0], point10[0, 1], point10[0, 2] } })));
                //g.DrawLine(red_pen, toIsometric(new Matrix(new float[1, 3] { { point10[0, 0], point10[0, 1], point10[0, 2] } })),
                //    toIsometric(new Matrix(new float[1, 3] { { point00[0, 0], point00[0, 1], point00[0, 2] } })));
            }
        }
    }
}
