using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace PixelEditor
{
    public partial class Form1 : Form
    {
        // Основные переменные редактора
        private Bitmap canvas;
        private Graphics graphics;
        private Point startPoint;
        private bool drawing = false;
        private string mode = null;
        private Color drawColor = Color.Black;
        private int penSize = 3;
        private ToolStripButton currentActiveButton;
        private Point previousPoint;

        // Для фигур и кривых
        private List<Point> bezierPoints = new List<Point>();
        private List<Point> polylinePoints = new List<Point>();
        private bool isBezierSegment = false;
        private bool isBuildingPolyline = false;
        private int clicksCount = 0;

        // Для текстового инструмента
        private Font textFont = new Font("Arial", 12);
        private Color textColor = Color.Black;
        private Color textBgColor = Color.White;
        private bool useBackground = true;

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
            btnPolyline.Click += BtnPolyline_Click;
            btnText.Click += BtnText_Click;
            btnColor.Click += BtnColor_Click;
            btnClear.Click += BtnClear_Click;
            btnFont.Click += BtnFont_Click;
            btnTextColor.Click += BtnTextColor_Click;
            btnBgColor.Click += BtnBgColor_Click;

            trackBarSize.Scroll += TrackBarSize_Scroll;
            radioStraight.CheckedChanged += RadioSegmentType_Changed;
            radioCurve.CheckedChanged += RadioSegmentType_Changed;

            // Настройка текстового поля
            textEntryBox.Visible = false;
            textEntryBox.BorderStyle = BorderStyle.None;
            textEntryBox.Multiline = true;
            textEntryBox.Leave += TextEntryBox_Leave;
        }

        private void InitializeCanvas()
        {
            canvas = new Bitmap(ClientSize.Width, ClientSize.Height);
            graphics = Graphics.FromImage(canvas);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.Clear(Color.White);
        }

        private void DrawCanvas(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            e.Graphics.DrawImage(canvas, 0, 0);
            
            // Предпросмотр для инструментов
            if (drawing && startPoint != Point.Empty)
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
                        case "Text":
                            e.Graphics.DrawRectangle(previewPen, x, y, width, height);
                            break;
                    }
                }
            }
            
            // Для полилинии
            if (mode == "Polyline" && isBuildingPolyline && polylinePoints.Count > 0)
            {
                foreach (var point in polylinePoints)
                {
                    e.Graphics.FillEllipse(Brushes.Red, point.X - 3, point.Y - 3, 6, 6);
                }

                Point cursorPos = this.PointToClient(Cursor.Position);
                
                if (!isBezierSegment)
                {
                    if (polylinePoints.Count >= 1)
                    {
                        e.Graphics.DrawLine(Pens.Gray, polylinePoints[polylinePoints.Count - 1], cursorPos);
                    }
                }
                else
                {
                    if (polylinePoints.Count == 1)
                    {
                        e.Graphics.DrawLine(Pens.LightGray, polylinePoints[0], cursorPos);
                    }
                    else if (polylinePoints.Count == 2)
                    {
                        e.Graphics.DrawCurve(Pens.LightGray, new[] { polylinePoints[0], polylinePoints[1], cursorPos });
                    }
                }
            }
            
            // Для кривой Безье
            if (mode == "Bezier" && bezierPoints.Count > 0)
            {
                Point cursorPos = this.PointToClient(Cursor.Position);
                
                foreach (var point in bezierPoints)
                {
                    e.Graphics.FillEllipse(Brushes.Red, point.X - 3, point.Y - 3, 6, 6);
                }
                
                if (bezierPoints.Count == 1)
                {
                    e.Graphics.DrawLine(Pens.LightGray, bezierPoints[0], cursorPos);
                }
                else if (bezierPoints.Count == 2)
                {
                    e.Graphics.DrawCurve(Pens.LightGray, new[] { bezierPoints[0], bezierPoints[1], cursorPos });
                }
            }
        }

        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            if (mode == null || e.Button != MouseButtons.Left) return;
            
            if (mode == "Bezier")
            {
                if (bezierPoints.Count < 3)
                {
                    bezierPoints.Add(e.Location);
                    
                    if (bezierPoints.Count == 3)
                    {
                        using (Graphics g = Graphics.FromImage(canvas))
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.DrawCurve(new Pen(drawColor, penSize), bezierPoints.ToArray());
                        }
                        bezierPoints.Clear();
                        Invalidate();
                    }
                }
                return;
            }
            
            if (mode == "Polyline")
            {
                if (!isBuildingPolyline)
                {
                    isBuildingPolyline = true;
                    polylinePoints.Clear();
                    clicksCount = 0;
                }

                clicksCount++;
                polylinePoints.Add(e.Location);

                if (!isBezierSegment && clicksCount == 2)
                {
                    using (Graphics g = Graphics.FromImage(canvas))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLine(new Pen(drawColor, penSize), polylinePoints[0], polylinePoints[1]);
                    }
                    
                    polylinePoints.Clear();
                    polylinePoints.Add(e.Location);
                    clicksCount = 1;
                }
                else if (isBezierSegment && clicksCount == 3)
                {
                    using (Graphics g = Graphics.FromImage(canvas))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawCurve(new Pen(drawColor, penSize), polylinePoints.ToArray());
                    }
                    
                    polylinePoints.Clear();
                    polylinePoints.Add(e.Location);
                    clicksCount = 1;
                }

                Invalidate();
                return;
            }
            
            // Для текстового инструмента
            if (mode == "Text")
            {
                textEntryBox.Visible = false;
                drawing = true;
                startPoint = e.Location;
                return;
            }
            
            // Для других инструментов
            drawing = true;
            startPoint = e.Location;
            previousPoint = e.Location;
            
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
            
            if (mode == "Text" && drawing && e.Button == MouseButtons.Left)
            {
                int x = Math.Min(startPoint.X, e.X);
                int y = Math.Min(startPoint.Y, e.Y);
                int width = Math.Abs(startPoint.X - e.X);
                int height = Math.Abs(startPoint.Y - e.Y);
                
                textEntryBox.Location = new Point(x, y);
                textEntryBox.Size = new Size(width, height);
                Invalidate();
            }
            else if (drawing && e.Button == MouseButtons.Left)
            {
                Invalidate();
            }
            
            if ((mode == "Polyline" && isBuildingPolyline) || (mode == "Bezier" && bezierPoints.Count > 0))
                Invalidate();
        }

        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            if (!drawing && mode != "Polyline" && mode != "Bezier") return;
            
            if (mode == "Pencil" || mode == "Eraser")
            {
                drawing = false;
                return;
            }
            
            if (mode == "Polyline" && e.Button == MouseButtons.Right)
            {
                isBuildingPolyline = false;
                polylinePoints.Clear();
                clicksCount = 0;
                Invalidate();
                return;
            }
            
            if (mode == "Text")
            {
                if (textEntryBox.Width > 10 && textEntryBox.Height > 10)
                {
                    textEntryBox.Font = textFont;
                    textEntryBox.ForeColor = textColor;
                    textEntryBox.BackColor = useBackground ? textBgColor : Color.Transparent;
                    textEntryBox.Visible = true;
                    textEntryBox.Focus();
                }
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

        private void TextEntryBox_Leave(object sender, EventArgs e)
        {
            if (textEntryBox.Visible && !string.IsNullOrEmpty(textEntryBox.Text))
            {
                using (Graphics g = Graphics.FromImage(canvas))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;

                    if (useBackground)
                    {
                        using (Brush bgBrush = new SolidBrush(textBgColor))
                        {
                            g.FillRectangle(bgBrush, 
                                textEntryBox.Left, 
                                textEntryBox.Top, 
                                textEntryBox.Width, 
                                textEntryBox.Height);
                        }
                    }

                    using (Brush textBrush = new SolidBrush(textColor))
                    {
                        StringFormat format = new StringFormat();
                        g.DrawString(textEntryBox.Text, textFont, textBrush, 
                            textEntryBox.Left, 
                            textEntryBox.Top, 
                            format);
                    }
                }
                textEntryBox.Visible = false;
                textEntryBox.Text = "";
                Invalidate();
            }
        }

        private void SetActiveButton(ToolStripButton activeButton)
        {
            if (currentActiveButton != null)
                currentActiveButton.BackColor = SystemColors.Control;
            
            currentActiveButton = activeButton;
            currentActiveButton.BackColor = Color.LightBlue;
            
            textEntryBox.Visible = false;
            panelSegmentType.Visible = (mode == "Polyline");
        }

        // Обработчики кнопок инструментов
        private void BtnLine_Click(object sender, EventArgs e) { mode = "Line"; SetActiveButton((ToolStripButton)sender); }
        private void BtnRectangle_Click(object sender, EventArgs e) { mode = "Rectangle"; SetActiveButton((ToolStripButton)sender); }
        private void BtnCircle_Click(object sender, EventArgs e) { mode = "Circle"; SetActiveButton((ToolStripButton)sender); }
        private void BtnPencil_Click(object sender, EventArgs e) { mode = "Pencil"; SetActiveButton((ToolStripButton)sender); }
        private void BtnEraser_Click(object sender, EventArgs e) { mode = "Eraser"; SetActiveButton((ToolStripButton)sender); }
        private void BtnBezier_Click(object sender, EventArgs e) { mode = "Bezier"; SetActiveButton((ToolStripButton)sender); }
        private void BtnPolyline_Click(object sender, EventArgs e) { mode = "Polyline"; SetActiveButton((ToolStripButton)sender); }
        private void BtnText_Click(object sender, EventArgs e) { mode = "Text"; SetActiveButton((ToolStripButton)sender); }

        // Обработчики текстовых настроек
        private void BtnFont_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                fontDialog.Font = textFont;
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    textFont = fontDialog.Font;
                }
            }
        }

        private void BtnTextColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = textColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    textColor = colorDialog.Color;
                }
            }
        }

        private void BtnBgColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = textBgColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    textBgColor = colorDialog.Color;
                    useBackground = true;
                }
            }
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = drawColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    drawColor = colorDialog.Color;
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            bezierPoints.Clear();
            polylinePoints.Clear();
            isBuildingPolyline = false;
            clicksCount = 0;
            textEntryBox.Visible = false;
            Invalidate();
        }

        private void TrackBarSize_Scroll(object sender, EventArgs e)
        {
            penSize = trackBarSize.Value;
            lblSize.Text = $"Size: {penSize}";
        }

        private void RadioSegmentType_Changed(object sender, EventArgs e)
        {
            isBezierSegment = radioCurve.Checked;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (canvas != null && ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                Bitmap newCanvas = new Bitmap(ClientSize.Width, ClientSize.Height);
                using (Graphics g = Graphics.FromImage(newCanvas))
                {
                    g.Clear(Color.White);
                    if (canvas != null)
                    {
                        g.DrawImage(canvas, Point.Empty);
                    }
                }
                canvas = newCanvas;
                graphics = Graphics.FromImage(canvas);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Invalidate();
            }
        }
    }
}