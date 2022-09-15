using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ConvexGen.Net
{
    using SimpleX;

    public partial class MainForm : Form
    {
        private Random random = new Random();
        private ConvexGenerator convex = null;
        private PointF[] vertics = null;
        private Pen pen = new Pen(Color.Black);

        public MainForm()
        {
            InitializeComponent();
        }

        private void OnLoadHandler(object sender, EventArgs e)
        {
            convex = new ConvexGenerator(random);
            vertics = GenConvex();
        }

        private void OnPaintHandler(object sender, PaintEventArgs e)
        {
            if (vertics != null)
            {
                var grap = e.Graphics;
                grap.SmoothingMode = SmoothingMode.HighQuality;

                var points = new PointF[vertics.Length + 1];
                for (int i=0; i<points.Length; i++)
                {
                    var v = vertics[i % vertics.Length];
                    points[i] = new PointF(v.X, v.Y);
                }
                grap.DrawPolygon(pen, points);
            }
        }

        private PointF[] GenConvex()
        {
            var w = this.ClientSize.Width-40;
            var h = this.ClientSize.Height-40;
            var c = random.Next(3, 16);

            return convex.Gen(20, 20, w, h, c);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                vertics = GenConvex();
                Refresh();
            }
        }
    }
}
