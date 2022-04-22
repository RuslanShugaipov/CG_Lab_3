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

        private void Draw_Line(Matrix point1, Matrix point2, Graphics g, Pen pen)
        {
            g.DrawLine(pen, point1[0, 0], point1[0, 1], point2[0, 0], point2[0, 1]);
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

            Matrix point00R = t.toIsometric(point00);
            Matrix point01R = t.toIsometric(point01);
            Matrix point10R = t.toIsometric(point10);
            Matrix point11R = t.toIsometric(point11);

            point00R = func(point00, angle);
            point01R = func(point01, angle);
            point10R = func(point10, angle);
            point11R = func(point11, angle);

            List<Matrix> points = biLin(
                point00R,
                point01R,
                point10R,
                point11R);

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;

            Pen red_pen = new Pen(Color.Red);
            Pen black_pen = new Pen(Color.Black);

            Matrix axis_point = new Matrix(new float[1, 3] { { 0, -height / 2, 0 } });
            g.TranslateTransform(width / 2, height / 2);

            for (int i = 1; i < points.Count(); ++i)
            {
                Draw_Line(t.toIsometric(points[i - 1]), t.toIsometric(points[i]), g, red_pen);
            }

            //Axis Y
            Draw_Line(new Matrix(new float[1, 2] { { 0, 0 } }), axis_point, g, black_pen);
            //Axis Z
            Draw_Line(new Matrix(new float[1, 2] { { 0, 0 } }), t.rotationZ(axis_point, 120), g, black_pen);
            //Axis X
            Draw_Line(new Matrix(new float[1, 2] { { 0, 0 } }), t.rotationZ(axis_point, -120), g, black_pen);

            Draw_Line(t.toIsometric(point00R), t.toIsometric(point01R), g, red_pen);
            Draw_Line(t.toIsometric(point01R), t.toIsometric(point11R), g, red_pen);
            Draw_Line(t.toIsometric(point11R), t.toIsometric(point10R), g, red_pen);
            Draw_Line(t.toIsometric(point10R), t.toIsometric(point00R), g, red_pen);
        }

        public delegate Matrix Rotation(Matrix axis_point, double angle);
    }
}
