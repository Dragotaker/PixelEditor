using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace PixelEditor.Controls
{
    public class TextInputBox : TextBox
    {
        private bool _isActive;
        private Point _startPoint;
        private Rectangle _previewRect;
        private float _rotationAngle = 0f;
        
        public event EventHandler<TextConfirmedEventArgs> TextConfirmed;
        public event EventHandler<Rectangle> PreviewChanged;

        public Color TextColor { get; set; } = Color.Black;
        public Color BackgroundColor { get; set; } = Color.White;
        public Font TextFont { get; set; } = new Font("Arial", 12);
        public bool UseBackground { get; set; } = true;
        public float RotationAngle 
        { 
            get => _rotationAngle;
            set
            {
                _rotationAngle = value;
                Invalidate();
            }
        }

        public TextInputBox()
        {
            this.Visible = false;
            this.BorderStyle = BorderStyle.None;
            this.Multiline = true;
            this.AcceptsReturn = true;
            this.KeyDown += OnKeyDown;
            this.LostFocus += OnLostFocus;
        }

        public void RotateLeft()
        {
            RotationAngle -= 15f;
        }

        public void RotateRight()
        {
            RotationAngle += 15f;
        }

        public void StartInput(Point startPoint, Graphics graphics)
        {
            _startPoint = startPoint;
            _isActive = true;
            this.Location = startPoint;
            this.Size = new Size(1, 1);
            this.Visible = true;
            this.Focus();
        }

        public void UpdatePreview(Point currentPoint)
        {
            int x = Math.Min(_startPoint.X, currentPoint.X);
            int y = Math.Min(_startPoint.Y, currentPoint.Y);
            int width = Math.Abs(currentPoint.X - _startPoint.X);
            int height = Math.Abs(currentPoint.Y - _startPoint.Y);

            if (width < 10) width = 10;
            if (height < 10) height = 10;

            _previewRect = new Rectangle(x, y, width, height);
            PreviewChanged?.Invoke(this, _previewRect);
        }

        public void CommitText(Graphics graphics)
        {
            if (string.IsNullOrWhiteSpace(this.Text)) 
            {
                CancelInput();
                return;
            }

            var bounds = new Rectangle(this.Location, this.Size);
            TextConfirmed?.Invoke(this, new TextConfirmedEventArgs
            {
                Text = this.Text,
                Bounds = bounds,
                Font = this.TextFont,
                TextColor = this.TextColor,
                BackgroundColor = this.UseBackground ? this.BackgroundColor : Color.Transparent,
                RotationAngle = this.RotationAngle
            });

            Reset();
        }

        public void CancelInput()
        {
            Reset();
        }

        private void Reset()
        {
            this.Text = string.Empty;
            this.Visible = false;
            _isActive = false;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true;
                CommitText(null);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                CancelInput();
            }
        }

        private void OnLostFocus(object sender, EventArgs e)
        {
            if (_isActive)
            {
                CommitText(null);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            if (DesignMode)
            {
                using (var pen = new Pen(Color.Gray, 1))
                {
                    pen.DashStyle = DashStyle.Dash;
                    e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                }
            }
        }
    }

    public class TextConfirmedEventArgs : EventArgs
    {
        public string Text { get; set; }
        public Rectangle Bounds { get; set; }
        public Font Font { get; set; }
        public Color TextColor { get; set; }
        public Color BackgroundColor { get; set; }
        public float RotationAngle { get; set; }
    }
}