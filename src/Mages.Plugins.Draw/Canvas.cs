namespace Mages.Plugins.Draw
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    public class Canvas
    {
        private readonly Bitmap _bmp;
        private readonly Graphics _g;
        private readonly Pen _pen;
        private Brush _brush;
        private GraphicsPath _path;
        private PointF _current;

        public Canvas(Double width, Double height)
        {
            _bmp = new Bitmap((Int32)width, (Int32)height);
            _g = Graphics.FromImage(_bmp);
            _pen = new Pen(ColorTranslator.FromHtml("black"), 1.0f);
            _current = new PointF(0f, 0f);
            _path = new GraphicsPath();
            _brush = new SolidBrush(_pen.Color);
        }

        public Canvas Color(String color)
        {
            _pen.Color = ColorTranslator.FromHtml(color);
            return this;
        }

        public Canvas Thickness(Double thickness)
        {
            _pen.Width = (Single)thickness;
            return this;
        }

        public Canvas SolidBrush(String color)
        {
            _brush = new SolidBrush(ColorTranslator.FromHtml(color));
            return this;
        }

        public Canvas StartPath()
        {
            _path.StartFigure();
            return this;
        }

        public Canvas EndPath()
        {
            _path.CloseFigure();
            return this;
        }

        public Canvas Stroke()
        {
            _g.DrawPath(_pen, _path);
            return this;
        }

        public Canvas Fill()
        {
            _g.FillPath(_brush, _path);
            return this;
        }

        public Canvas MoveTo(Double x, Double y)
        {
            _current = new PointF((Single)x, (Single)y);
            return this;
        }

        public Canvas LineTo(Double x, Double y)
        {
            var previous = _current;
            MoveTo(x, y);
            _path.AddLine(previous, _current);
            return this;
        }

        public Byte[] Content()
        {
            using (var ms = new MemoryStream())
            {
                _bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
