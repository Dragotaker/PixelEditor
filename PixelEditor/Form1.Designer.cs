using System;
using System.Drawing;
using System.Windows.Forms;
using PixelEditor.Controls;

namespace PixelEditor
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLine = new System.Windows.Forms.ToolStripButton();
            this.btnRectangle = new System.Windows.Forms.ToolStripButton();
            this.btnCircle = new System.Windows.Forms.ToolStripButton();
            this.btnPencil = new System.Windows.Forms.ToolStripButton();
            this.btnEraser = new System.Windows.Forms.ToolStripButton();
            this.btnBezier = new System.Windows.Forms.ToolStripButton();
            this.btnPolyline = new System.Windows.Forms.ToolStripButton();
            this.btnText = new System.Windows.Forms.ToolStripButton();
            this.btnFont = new System.Windows.Forms.ToolStripButton();
            this.btnTextColor = new System.Windows.Forms.ToolStripButton();
            this.btnBgColor = new System.Windows.Forms.ToolStripButton();
            this.btnColor = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.btnApplyRotation = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnLoad = new System.Windows.Forms.ToolStripButton();
            this.trackBarSize = new System.Windows.Forms.TrackBar();
            this.lblSize = new System.Windows.Forms.Label();
            this.panelSegmentType = new System.Windows.Forms.Panel();
            this.radioCurve = new System.Windows.Forms.RadioButton();
            this.radioStraight = new System.Windows.Forms.RadioButton();
            this.textEntryBox = new TextInputBox();
            this.panelRotation = new System.Windows.Forms.Panel();
            this.lblRotation = new System.Windows.Forms.Label();
            this.txtRotation = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).BeginInit();
            this.panelSegmentType.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLine,
            this.btnRectangle,
            this.btnCircle,
            this.btnPencil,
            this.btnEraser,
            this.btnBezier,
            this.btnPolyline,
            this.btnText,
            this.btnFont,
            this.btnTextColor,
            this.btnBgColor,
            this.btnColor,
            this.btnClear,
            this.btnApplyRotation,
            this.btnSave,
            this.btnLoad});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnLine
            // 
            this.btnLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(32, 22);
            this.btnLine.Text = "Line";
            // 
            // btnRectangle
            // 
            this.btnRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRectangle.Name = "btnRectangle";
            this.btnRectangle.Size = new System.Drawing.Size(60, 22);
            this.btnRectangle.Text = "Rectangle";
            // 
            // btnCircle
            // 
            this.btnCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Size = new System.Drawing.Size(39, 22);
            this.btnCircle.Text = "Circle";
            // 
            // btnPencil
            // 
            this.btnPencil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPencil.Name = "btnPencil";
            this.btnPencil.Size = new System.Drawing.Size(42, 22);
            this.btnPencil.Text = "Pencil";
            // 
            // btnEraser
            // 
            this.btnEraser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnEraser.Name = "btnEraser";
            this.btnEraser.Size = new System.Drawing.Size(42, 22);
            this.btnEraser.Text = "Eraser";
            // 
            // btnBezier
            // 
            this.btnBezier.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBezier.Name = "btnBezier";
            this.btnBezier.Size = new System.Drawing.Size(41, 22);
            this.btnBezier.Text = "Bezier";
            // 
            // btnPolyline
            // 
            this.btnPolyline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPolyline.Name = "btnPolyline";
            this.btnPolyline.Size = new System.Drawing.Size(50, 22);
            this.btnPolyline.Text = "Polyline";
            // 
            // btnText
            // 
            this.btnText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(31, 22);
            this.btnText.Text = "Text";
            // 
            // btnFont
            // 
            this.btnFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(35, 22);
            this.btnFont.Text = "Font";
            // 
            // btnTextColor
            // 
            this.btnTextColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTextColor.Name = "btnTextColor";
            this.btnTextColor.Size = new System.Drawing.Size(64, 22);
            this.btnTextColor.Text = "Text Color";
            // 
            // btnBgColor
            // 
            this.btnBgColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBgColor.Name = "btnBgColor";
            this.btnBgColor.Size = new System.Drawing.Size(56, 22);
            this.btnBgColor.Text = "Bg Color";
            // 
            // btnColor
            // 
            this.btnColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(38, 22);
            this.btnColor.Text = "Color";
            // 
            // btnClear
            // 
            this.btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(38, 22);
            this.btnClear.Text = "Clear";
            // 
            // btnApplyRotation
            // 
            this.btnApplyRotation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnApplyRotation.Name = "btnApplyRotation";
            this.btnApplyRotation.Size = new System.Drawing.Size(60, 22);
            this.btnApplyRotation.Text = "Apply";
            this.btnApplyRotation.Click += new System.EventHandler(this.BtnApplyRotation_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(35, 22);
            this.btnSave.Text = "Save";
            // 
            // btnLoad
            // 
            this.btnLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(35, 22);
            this.btnLoad.Text = "Load";
            // 
            // trackBarSize
            // 
            this.trackBarSize.Location = new System.Drawing.Point(700, 30);
            this.trackBarSize.Maximum = 50;
            this.trackBarSize.Minimum = 1;
            this.trackBarSize.Name = "trackBarSize";
            this.trackBarSize.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarSize.Size = new System.Drawing.Size(45, 100);
            this.trackBarSize.TabIndex = 1;
            this.trackBarSize.Value = 3;
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(700, 5);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(44, 13);
            this.lblSize.TabIndex = 2;
            this.lblSize.Text = "Size: 3";
            // 
            // panelSegmentType
            // 
            this.panelSegmentType.Controls.Add(this.radioCurve);
            this.panelSegmentType.Controls.Add(this.radioStraight);
            this.panelSegmentType.Location = new System.Drawing.Point(120, 28);
            this.panelSegmentType.Name = "panelSegmentType";
            this.panelSegmentType.Size = new System.Drawing.Size(200, 30);
            this.panelSegmentType.TabIndex = 3;
            this.panelSegmentType.Visible = false;
            // 
            // radioCurve
            // 
            this.radioCurve.AutoSize = true;
            this.radioCurve.Location = new System.Drawing.Point(100, 7);
            this.radioCurve.Name = "radioCurve";
            this.radioCurve.Size = new System.Drawing.Size(52, 17);
            this.radioCurve.TabIndex = 1;
            this.radioCurve.TabStop = true;
            this.radioCurve.Text = "Curve";
            this.radioCurve.UseVisualStyleBackColor = true;
            // 
            // radioStraight
            // 
            this.radioStraight.AutoSize = true;
            this.radioStraight.Checked = true;
            this.radioStraight.Location = new System.Drawing.Point(3, 7);
            this.radioStraight.Name = "radioStraight";
            this.radioStraight.Size = new System.Drawing.Size(61, 17);
            this.radioStraight.TabIndex = 0;
            this.radioStraight.TabStop = true;
            this.radioStraight.Text = "Straight";
            this.radioStraight.UseVisualStyleBackColor = true;
            // 
            // textEntryBox
            // 
            this.textEntryBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textEntryBox.Multiline = true;
            this.textEntryBox.Visible = false;
            this.textEntryBox.Location = new System.Drawing.Point(0, 0);
            this.textEntryBox.Name = "textEntryBox";
            this.textEntryBox.Size = new System.Drawing.Size(100, 20);
            this.textEntryBox.TabIndex = 4;
            this.textEntryBox.Leave += new System.EventHandler(this.TextEntryBox_Leave);
            // 
            // panelRotation
            // 
            this.panelRotation.Controls.Add(this.txtRotation);
            this.panelRotation.Controls.Add(this.lblRotation);
            this.panelRotation.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRotation.Location = new System.Drawing.Point(700, 0);
            this.panelRotation.Name = "panelRotation";
            this.panelRotation.Size = new System.Drawing.Size(100, 25);
            this.panelRotation.TabIndex = 5;
            // 
            // lblRotation
            // 
            this.lblRotation.AutoSize = true;
            this.lblRotation.Location = new System.Drawing.Point(3, 5);
            this.lblRotation.Name = "lblRotation";
            this.lblRotation.Size = new System.Drawing.Size(50, 13);
            this.lblRotation.Text = "Rotation:";
            // 
            // txtRotation
            // 
            this.txtRotation.Location = new System.Drawing.Point(53, 2);
            this.txtRotation.Name = "txtRotation";
            this.txtRotation.Size = new System.Drawing.Size(45, 20);
            this.txtRotation.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.panelRotation);
            this.Controls.Add(this.textEntryBox);
            this.Controls.Add(this.panelSegmentType);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.trackBarSize);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Pixel Editor";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawCanvas);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownHandler);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMoveHandler);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpHandler);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).EndInit();
            this.panelSegmentType.ResumeLayout(false);
            this.panelSegmentType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnLine;
        private System.Windows.Forms.ToolStripButton btnRectangle;
        private System.Windows.Forms.ToolStripButton btnCircle;
        private System.Windows.Forms.ToolStripButton btnPencil;
        private System.Windows.Forms.ToolStripButton btnEraser;
        private System.Windows.Forms.ToolStripButton btnBezier;
        private System.Windows.Forms.ToolStripButton btnPolyline;
        private System.Windows.Forms.ToolStripButton btnText;
        private System.Windows.Forms.ToolStripButton btnFont;
        private System.Windows.Forms.ToolStripButton btnTextColor;
        private System.Windows.Forms.ToolStripButton btnBgColor;
        private System.Windows.Forms.ToolStripButton btnColor;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripButton btnApplyRotation;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnLoad;
        private System.Windows.Forms.TrackBar trackBarSize;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Panel panelSegmentType;
        private System.Windows.Forms.RadioButton radioCurve;
        private System.Windows.Forms.RadioButton radioStraight;
        private TextInputBox textEntryBox;
        private System.Windows.Forms.Panel panelRotation;
        private System.Windows.Forms.Label lblRotation;
        private System.Windows.Forms.TextBox txtRotation;
    }
}