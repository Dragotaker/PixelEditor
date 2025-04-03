using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PixelEditor
{
    public partial class Form1 : Form
    {
        private Bitmap canvas;
        private Graphics graphics;
        private Point startPoint;
        private bool drawing = false;
        private string mode = null;
        private Color drawColor = Color.Black;
        private int penSize = 3;
        private ToolStripButton currentActiveButton;
        private Point previousPoint;
        
        private List<Point> bezierPoints = new List<Point>();

        public Form1()
        {
            InitializeComponent();
            InitializeCanvas();
            
            // Привязка обработчиков
            btnLine.Click += BtnLine_Click;
            btnRectangle.Click += BtnRectangle_Click;
            btnCircle.Click += BtnCircle_Click;
            btnPencil.Click += BtnPencil_Click;
            btnEraser.Click += BtnEraser_Click;
            btnBezier.Click += BtnBezier_Click;
            btnColor.Click += BtnColor_Click;
            btnClear.Click += BtnClear_Click;
            trackBarSize.Scroll += TrackBarSize_Scroll;
        }

        private void InitializeCanvas()
        {
            canvas = new Bitmap(ClientSize.Width, ClientSize.Height);
            graphics = Graphics.FromImage(canvas);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);
        }

        private void DrawCanvas(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(canvas, 0, 0);
            
            // Предпросмотр для инструментов
            if (drawing && startPoint != Point.Empty && 
                (mode == "Line" || mode == "Rectangle" || mode == "Circle"))
            {
                using (Pen previewPen = new Pen(Color.Gray, 1))
                {
                    previewPen.DashStyle = DashStyle.Dash;
                    Point currentPos = this.PointToClient(Cursor.Position);
                    
                    int x = Math.Min(startPoint.X, currentPos.X);
                    int y = Math.Min(startPoint.Y, currentPos.Y);
                    int width = Math.Abs(currentPos.X - startPoint.X);
                    int height = Math.Abs(currentPos.Y - startPoint.Y);
                    
                    switch (mode)
                    {
                        case "Line":
                            e.Graphics.DrawLine(previewPen, startPoint, currentPos);
                            break;
                        case "Rectangle":
                            e.Graphics.DrawRectangle(previewPen, x, y, width, height);
                            break;
                        case "Circle":
                            e.Graphics.DrawEllipse(previewPen, x, y, width, height);
                            break;
                    }
                }
            }
            
            // Для кривой Безье
            if (mode == "Bezier")
            {
                // Рисуем точки
                foreach (var point in bezierPoints)
                {
                    e.Graphics.FillEllipse(Brushes.Red, point.X - 3, point.Y - 3, 6, 6);
                }
                
                // Рисуем вспомогательные линии
                if (bezierPoints.Count > 1)
                {
                    for (int i = 0; i < bezierPoints.Count - 1; i++)
                    {
                        e.Graphics.DrawLine(Pens.LightGray, bezierPoints[i], bezierPoints[i + 1]);
                    }
                }
                
                // Рисуем кривую когда есть 4 точки
                if (bezierPoints.Count == 4)
                {
                    using (Pen pen = new Pen(Color.Red, 1))
                    {
                        pen.DashStyle = DashStyle.Dash;
                        e.Graphics.DrawBezier(pen, 
                            bezierPoints[0], 
                            bezierPoints[1], 
                            bezierPoints[2], 
                            bezierPoints[3]);
                    }
                }
            }
        }

        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            if (mode == null || e.Button != MouseButtons.Left) return;
            
            if (mode == "Bezier")
            {
                if (bezierPoints.Count < 4)
                {
                    bezierPoints.Add(e.Location);
                    
                    if (bezierPoints.Count == 4)
                    {
                        using (Graphics g = Graphics.FromImage(canvas))
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.DrawBezier(new Pen(drawColor, penSize), 
                                bezierPoints[0], 
                                bezierPoints[1], 
                                bezierPoints[2], 
                                bezierPoints[3]);
                        }
                        bezierPoints.Clear();
                        Invalidate();
                    }
                }
                return;
            }
            
            // Для других инструментов
            drawing = true;
            startPoint = e.Location;
            previousPoint = e.Location;
            
            // Для карандаша и ластика сразу рисуем первую точку
            if (mode == "Pencil" || mode == "Eraser")
            {
                using (Graphics g = Graphics.FromImage(canvas))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    if (mode == "Pencil")
                    {
                        using (Pen pen = new Pen(drawColor, penSize))
                        {
                            g.DrawEllipse(pen, e.X, e.Y, 1, 1);
                        }
                    }
                    else
                    {
                        g.FillEllipse(Brushes.White, e.X - penSize/2, e.Y - penSize/2, penSize, penSize);
                    }
                }
                Invalidate();
            }
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (mode == null) return;
            
            if ((mode == "Pencil" || mode == "Eraser") && drawing && e.Button == MouseButtons.Left)
            {
                using (Graphics g = Graphics.FromImage(canvas))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    if (mode == "Pencil")
                    {
                        using (Pen pen = new Pen(drawColor, penSize))
                        {
                            g.DrawLine(pen, previousPoint, e.Location);
                        }
                    }
                    else
                    {
                        using (Pen pen = new Pen(Color.White, penSize))
                        {
                            g.DrawLine(pen, previousPoint, e.Location);
                        }
                    }
                }
                previousPoint = e.Location;
                Invalidate();
                return;
            }
            
            // Для Line, Rectangle, Circle - только предпросмотр
            if (drawing && e.Button == MouseButtons.Left)
                Invalidate();
        }

        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            if (!drawing || mode == null || e.Button != MouseButtons.Left) return;
            
            if (mode == "Pencil" || mode == "Eraser")
            {
                drawing = false;
                return;
            }
            
            // Для Line, Rectangle, Circle
            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(drawColor, penSize))
                {
                    int x = Math.Min(startPoint.X, e.X);
                    int y = Math.Min(startPoint.Y, e.Y);
                    int width = Math.Abs(e.X - startPoint.X);
                    int height = Math.Abs(e.Y - startPoint.Y);
                    
                    switch (mode)
                    {
                        case "Line":
                            g.DrawLine(pen, startPoint, e.Location);
                            break;
                        case "Rectangle":
                            g.DrawRectangle(pen, x, y, width, height);
                            break;
                        case "Circle":
                            g.DrawEllipse(pen, x, y, width, height);
                            break;
                    }
                }
            }
            
            drawing = false;
            startPoint = Point.Empty;
            Invalidate();
        }

        private void SetActiveButton(ToolStripButton activeButton)
        {
            if (currentActiveButton != null)
                currentActiveButton.BackColor = SystemColors.Control;
            
            currentActiveButton = activeButton;
            currentActiveButton.BackColor = Color.LightBlue;
            
            // Сброс состояния Безье при смене инструмента
            if (mode != "Bezier")
                bezierPoints.Clear();
        }

        private void BtnLine_Click(object sender, EventArgs e)
        {
            mode = "Line";
            SetActiveButton((ToolStripButton)sender);
        }

        private void BtnRectangle_Click(object sender, EventArgs e)
        {
            mode = "Rectangle";
            SetActiveButton((ToolStripButton)sender);
        }

        private void BtnCircle_Click(object sender, EventArgs e)
        {
            mode = "Circle";
            SetActiveButton((ToolStripButton)sender);
        }

        private void BtnPencil_Click(object sender, EventArgs e)
        {
            mode = "Pencil";
            SetActiveButton((ToolStripButton)sender);
        }

        private void BtnEraser_Click(object sender, EventArgs e)
        {
            mode = "Eraser";
            SetActiveButton((ToolStripButton)sender);
        }

        private void BtnBezier_Click(object sender, EventArgs e)
        {
            mode = "Bezier";
            SetActiveButton((ToolStripButton)sender);
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                drawColor = colorDialog.Color;
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            bezierPoints.Clear();
            Invalidate();
        }

        private void TrackBarSize_Scroll(object sender, EventArgs e)
        {
            penSize = trackBarSize.Value;
            lblSize.Text = $"Size: {penSize}";
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (ClientSize.Width <= 0 || ClientSize.Height <= 0)
                return;

            Bitmap newCanvas = new Bitmap(ClientSize.Width, ClientSize.Height);
            using (Graphics g = Graphics.FromImage(newCanvas))
            {
                g.Clear(Color.White);
                if (canvas != null)
                {
                    g.DrawImage(canvas, Point.Empty);
                }
            }

            if (graphics != null)
                graphics.Dispose();
            if (canvas != null)
                canvas.Dispose();

            canvas = newCanvas;
            graphics = Graphics.FromImage(canvas);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Invalidate();
        }
    }
}