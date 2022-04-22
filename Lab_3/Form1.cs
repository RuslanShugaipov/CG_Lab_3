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
        Graphics g;
        Transformation t;
        private static double angleX = 0, angleY = 0;

        private static Matrix point00 = new Matrix(new float[1, 3] { { 0f, -300f, 0f } });
        private static Matrix point01 = new Matrix(new float[1, 3] { { 300f, 0f, 0f } });
        private static Matrix point10 = new Matrix(new float[1, 3] { { 0f, 0f, 300f } });
        private static Matrix point11 = new Matrix(new float[1, 3] { { 150f, -150f, 150f } });

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            t = Transformation.getInstance();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            draw_surface(0, t.rotationX);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            angleX = angleX == 360 ? 0 : angleX + 20;
            draw_surface(angleX, t.rotationX);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            angleY = angleY == 360 ? 0 : angleY + 20;
            draw_surface(angleY, t.rotationY);
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

        private void draw_surface(double angle, Rotation func)
        {
            g = Graphics.FromImage(pictureBox1.Image);

            g.Clear(pictureBox1.BackColor);
            pictureBox1.Invalidate();

            PointF point00R = t.toIsometric(point00);
            PointF point01R = t.toIsometric(point01);
            PointF point10R = t.toIsometric(point10);
            PointF point11R = t.toIsometric(point11);

            point00R = func(point00, angle);
            point01R = func(point01, angle);
            point10R = func(point10, angle);
            point11R = func(point11, angle);

            List<Matrix> points = biLin(
                new Matrix(new float[1, 3] { { point00R.X, point00R.Y, point00[0, 2] } }),
                new Matrix(new float[1, 3] { { point01R.X, point01R.Y, point01[0, 2] } }),
                new Matrix(new float[1, 3] { { point10R.X, point10R.Y, point10[0, 2] } }),
                new Matrix(new float[1, 3] { { point11R.X, point11R.Y, point11[0, 2] } }));

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;

            Pen red_pen = new Pen(Color.Red);
            Pen black_pen = new Pen(Color.Black);

            PointF axis_point = new PointF(0, -height / 2);
            g.TranslateTransform(width / 2, height / 2);

            for (int i = 1; i < points.Count(); ++i)
            {
                g.DrawLine(red_pen, t.toIsometric(new Matrix(new float[1, 3] { { points[i - 1][0, 0], points[i - 1][0, 1], points[i - 1][0, 2] } })),
                   t.toIsometric(new Matrix(new float[1, 3] { { points[i][0, 0], points[i][0, 1], points[i][0, 2] } })));
            }

            //Axis
            g.DrawLine(black_pen, new PointF(0, 0), axis_point);
            g.DrawLine(black_pen, new PointF(0, 0), t.rotationZ(new Matrix(new float[1, 3] { { axis_point.X, axis_point.Y, 0 } }), 120));
            g.DrawLine(black_pen, new PointF(0, 0), t.rotationZ(new Matrix(new float[1, 3] { { axis_point.X, axis_point.Y, 0 } }), -120));

            g.DrawLine(red_pen, t.toIsometric(new Matrix(new float[1, 3] { { point00R.X, point00R.Y, 0f } })),
               t.toIsometric(new Matrix(new float[1, 3] { { point01R.X, point01R.Y, 0f } })));
            g.DrawLine(red_pen, t.toIsometric(new Matrix(new float[1, 3] { { point01R.X, point01R.Y, 0f } })),
               t.toIsometric(new Matrix(new float[1, 3] { { point11R.X, point11R.Y, 150f } })));
            g.DrawLine(red_pen, t.toIsometric(new Matrix(new float[1, 3] { { point11R.X, point11R.Y, 150f } })),
               t.toIsometric(new Matrix(new float[1, 3] { { point10R.X, point10R.Y, 300f } })));
            g.DrawLine(red_pen, t.toIsometric(new Matrix(new float[1, 3] { { point10R.X, point10R.Y, 300f } })),
               t.toIsometric(new Matrix(new float[1, 3] { { point00R.X, point00R.Y, 0f } })));

            point00 = new Matrix(new float[1, 3] { { func(point00, angle).X, func(point00, angle).Y, point00[0, 2] } });
            point01 = new Matrix(new float[1, 3] { { func(point01, angle).X, func(point01, angle).Y, point01[0, 2] } });
            point10 = new Matrix(new float[1, 3] { { func(point10, angle).X, func(point10, angle).Y, point10[0, 2] } });
            point11 = new Matrix(new float[1, 3] { { func(point11, angle).X, func(point11, angle).Y, point11[0, 2] } });
        }


        public delegate PointF Rotation(Matrix axis_point, double angle);
    }
}
