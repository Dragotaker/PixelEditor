using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PixelEditor.Services
{
    public static class DrawingService
    {
        public static void DrawShape(Graphics graphics, Point start, Point end, 
                                    Color color, int penSize, string shapeType, 
                                    bool fill = false, Color? fillColor = null)
        {
            if (graphics == null) 
                throw new ArgumentNullException(nameof(graphics));

            using (var pen = new Pen(color, penSize))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                int x = Math.Min(start.X, end.X);
                int y = Math.Min(start.Y, end.Y);
                int width = Math.Abs(end.X - start.X);
                int height = Math.Abs(end.Y - start.Y);

                switch (shapeType)
                {
                    case "Line":
                        graphics.DrawLine(pen, start, end);
                        break;

                    case "Rectangle":
                        if (fill && fillColor.HasValue)
                        {
                            using (var brush = new SolidBrush(fillColor.Value))
                                graphics.FillRectangle(brush, x, y, width, height);
                        }
                        graphics.DrawRectangle(pen, x, y, width, height);
                        break;

                    case "Ellipse":
                        if (fill && fillColor.HasValue)
                        {
                            using (var brush = new SolidBrush(fillColor.Value))
                                graphics.FillEllipse(brush, x, y, width, height);
                        }
                        graphics.DrawEllipse(pen, x, y, width, height);
                        break;

                    default:
                        throw new ArgumentException("Unknown shape type", nameof(shapeType));
                }
            }
        }

        public static void DrawBezier(Graphics graphics, Point[] points, 
                                     Color color, int penSize)
        {
            if (points == null || points.Length < 2)
                return;

            using (var pen = new Pen(color, penSize))
            {
                pen.LineJoin = LineJoin.Round;
                
                if (points.Length == 2)
                {
                    graphics.DrawLine(pen, points[0], points[1]);
                }
                else
                {
                    graphics.DrawCurve(pen, points);
                }
            }
        }

        public static void DrawPolyline(Graphics graphics, Point[] points, 
                                       Color color, int penSize, bool isBezier)
        {
            if (points == null || points.Length < 2)
                return;

            using (var pen = new Pen(color, penSize))
            {
                pen.LineJoin = LineJoin.Round;

                if (isBezier && points.Length >= 3)
                {
                    graphics.DrawCurve(pen, points);
                }
                else
                {
                    graphics.DrawLines(pen, points);
                }
            }
        }

        public static void DrawText(Graphics graphics, string text, Rectangle bounds, 
                                   Font font, Color textColor, Color backgroundColor, 
                                   bool useBackground)
        {
            if (graphics == null || string.IsNullOrEmpty(text))
                return;

            if (useBackground)
            {
                using (var brush = new SolidBrush(backgroundColor))
                    graphics.FillRectangle(brush, bounds);
            }

            using (var brush = new SolidBrush(textColor))
            {
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near,
                    FormatFlags = StringFormatFlags.NoWrap
                };

                graphics.DrawString(text, font, brush, bounds, format);
            }
        }

        public static void DrawPreview(Graphics graphics, Point start, Point current, 
                                       string toolType, bool isBezier = false)
        {
            if (graphics == null)
                return;

            using (var pen = new Pen(Color.Gray, 1))
            {
                pen.DashStyle = DashStyle.Dash;

                int x = Math.Min(start.X, current.X);
                int y = Math.Min(start.Y, current.Y);
                int width = Math.Abs(current.X - start.X);
                int height = Math.Abs(current.Y - start.Y);

                switch (toolType)
                {
                    case "Line":
                        graphics.DrawLine(pen, start, current);
                        break;

                    case "Rectangle":
                        graphics.DrawRectangle(pen, x, y, width, height);
                        break;

                    case "Ellipse":
                        graphics.DrawEllipse(pen, x, y, width, height);
                        break;

                    case "Bezier":
                        if (isBezier)
                            graphics.DrawCurve(pen, new[] { start, current });
                        else
                            graphics.DrawLine(pen, start, current);
                        break;

                    case "Polyline":
                        graphics.DrawLines(pen, new[] { start, current });
                        break;
                }
            }
        }

        public static void DrawControlPoints(Graphics graphics, Point[] points)
        {
            if (points == null)
                return;

            foreach (var point in points)
            {
                graphics.FillEllipse(Brushes.Red, point.X - 3, point.Y - 3, 6, 6);
                graphics.DrawEllipse(Pens.White, point.X - 3, point.Y - 3, 6, 6);
            }
        }
    }
}