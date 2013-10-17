using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using StUtil.Extensions;

namespace StUtil.UI.Controls
{
    public partial class DrawingCanvas : UserControl
    {
        public event EventHandler<DrawingAddedEventArgs> DrawingAdded;

        /// <summary>
        /// The background image
        /// </summary>
        public override Image BackgroundImage
        {
            get
            {
                return this.pbBackground.Image;
            }
            set
            {
                if (value != null)
                {
                    this.pbDrawing.Image = new Bitmap(value.Width, value.Height, PixelFormat.Format32bppArgb);
                    this.pbShapes.Image = new Bitmap(value.Width, value.Height, PixelFormat.Format32bppArgb);
                }
                this.pbBackground.Image = value;
            }
        }

        /// <summary>
        /// The background image
        /// </summary>
        public Image Image
        {
            get
            {
                return this.pbBackground.Image;
            }
            set
            {
                if (value != null)
                {
                    this.pbDrawing.Image = new Bitmap(value.Width, value.Height, PixelFormat.Format32bppArgb);
                    this.pbShapes.Image = new Bitmap(value.Width, value.Height, PixelFormat.Format32bppArgb);
                }
                this.pbBackground.Image = value;
            }
        }

        /// <summary>
        /// The background picturebox
        /// </summary>
        public PictureBox BackgroundPictureBox
        {
            get
            {
                return this.pbBackground;
            }
        }

        /// <summary>
        /// The shapes picturebox
        /// </summary>
        public PictureBox ShapesPictureBox
        {
            get
            {
                return this.pbShapes;
            }
        }

        /// <summary>
        /// The location the mouse was clicked down at
        /// </summary>
        private Point? MouseDownLocation { get; set; }

        /// <summary>
        /// The current location of the mouse
        /// </summary>
        private Point MouseLocation { get { return this.PointToClient(Cursor.Position); } }

        /// <summary>
        /// Shapes
        /// </summary>
        private List<GraphicsPath> Drawings = new List<GraphicsPath>();

        /// <summary>
        /// Readonly list of drawings
        /// </summary>
        public IEnumerable<GraphicsPath> Paths
        {
            get
            {
                return Drawings;
            }
        }

        /// <summary>
        /// The last created object
        /// </summary>
        private GraphicsPath lastPath;

        /// <summary>
        /// List of points making up a polygon
        /// </summary>
        private List<Point> points;

        private SolidBrush lineBrush;
        private Pen linePen;
        public Color LineColor
        {
            get
            {
                return lineBrush.Color;
            }
            set
            {
                if (value != lineBrush.Color)
                {
                    lineBrush.Dispose();
                    lineBrush = new SolidBrush(value);
                    linePen.Dispose();
                    linePen = new Pen(value);
                }
            }
        }

        private SolidBrush highlightLineBrush;
        public Color HighlightLineColor
        {
            get
            {
                return highlightLineBrush.Color;
            }
            set
            {
                if (value != highlightLineBrush.Color)
                {
                    highlightLineBrush.Dispose();
                    highlightLineBrush = new SolidBrush(value);
                }
            }
        }

        private SolidBrush fillBrush;
        public Color FillColor
        {
            get
            {
                return fillBrush.Color;
            }
            set
            {
                if (value != fillBrush.Color)
                {
                    fillBrush.Dispose();
                    fillBrush = new SolidBrush(value);
                }
            }
        }

        /// <summary>
        /// If the mouse is over the first poly point
        /// </summary>
        private bool overPolyFirst = false;

        /// <summary>
        /// List of possible shapes
        /// </summary>
        public enum Shapes
        {
            Polygon,
            Ellipse,
            Rectangle
        }

        /// <summary>
        /// Current shape to draw
        /// </summary>
        public Shapes CurrentShape { get; set; }

        /// <summary>
        /// If any drawings have been created
        /// </summary>
        public bool HasDrawing
        {
            get
            {
                return this.Drawings.Count > 0;
            }
        }

        /// <summary>
        /// If drawing is enabled
        /// </summary>
        public bool CanDraw { get; set; }

        public DrawingCanvas()
        {
            InitializeComponent();

            this.highlightLineBrush = new SolidBrush(Color.Green);
            this.fillBrush = new SolidBrush(Color.FromArgb(100, 255, 0, 0));
            this.lineBrush = new SolidBrush(Color.Red);
            this.linePen = new Pen(Color.Red);

            this.pbBackground.LoadCompleted += pbBackground_LoadCompleted;

            this.CanDraw = true;
            this.CurrentShape = Shapes.Rectangle;

            this.pbDrawing.Parent = this.pbShapes;
            this.pbShapes.Parent = this.pbBackground;

            //this.Resize += DrawingCanvas_Resize;

            this.pbDrawing.MouseDown += pbDrawing_MouseDown;
            this.pbDrawing.MouseMove += pbDrawing_MouseMove;
            this.pbDrawing.MouseUp += pbDrawing_MouseUp;
            this.pbDrawing.Click += pbDrawing_Click;
            this.pbDrawing.DoubleClick += pbDrawing_DoubleClick;
            this.KeyDown += DrawingCanvas_KeyDown;
        }

        void pbBackground_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DrawingCanvas_Resize(object sender, EventArgs e)
        {
            this.pbDrawing.Image = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);
            this.pbShapes.Image = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);
            Draw();
        }

        private void DrawingCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                using (Graphics g = Graphics.FromImage(this.pbDrawing.Image))
                {
                    g.Clear(Color.Transparent);
                }
                this.pbDrawing.Refresh();
                lastPath = new GraphicsPath();
                if (points != null)
                {
                    points.Clear();
                }
            }
        }

        private void pbDrawing_DoubleClick(object sender, EventArgs e)
        {
            if (this.CurrentShape == Shapes.Polygon && CanDraw)
            {
                if (points != null)
                {
                    lastPath.CloseAllFigures();
                    points = null;
                    Store();
                }
            }
        }

        private void pbDrawing_Click(object sender, EventArgs e)
        {
            if (this.CurrentShape == Shapes.Polygon && CanDraw)
            {
                if (points == null)
                {
                    points = new List<Point>();
                }

                if (points.Count > 0)
                {
                    if (Math.Abs(points[0].Distance(this.MouseLocation)) < 5)
                    {
                        lastPath.CloseAllFigures();
                        points = null;
                        Store();
                        return;
                    }
                }

                points.Add(this.MouseLocation);
                lastPath = new GraphicsPath();
                lastPath.AddLines(points.ToArray());

                DrawPoly(false);
            }
        }

        private void pbDrawing_MouseUp(object sender, MouseEventArgs e)
        {
            if (lastPath != null && CurrentShape != Shapes.Polygon && CanDraw)
            {
                Store();
            }
            this.MouseDownLocation = null;
        }

        private void pbDrawing_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.MouseDownLocation.HasValue)
            {
                if (CanDraw)
                {
                    //if (Control.ModifierKeys.HasFlag(Keys.Shift))
                    //{
                    //    Point pt = this.MouseLocation;
                    //    foreach (GraphicsPath gp in this.Drawings)
                    //    {
                    //        if (gp.IsVisible(pt))
                    //        {
                    //            Matrix translateMatrix = new Matrix();
                    //            translateMatrix.Translate(100, 0);
                    //            gp.Transform(translateMatrix);
                    //            Draw();
                    //            return;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    Draw();
                    //}
                }
            }
            else if (Control.ModifierKeys.HasFlag(Keys.Shift))
            {
                //Point pt = this.MouseLocation;
                //foreach (GraphicsPath gp in this.Drawings)
                //{
                //    if (gp.IsVisible(pt))
                //    {
                //        Cursor.Current = Cursors.SizeAll;
                //        return;
                //    }
                //    //bool valid = false;
                //    //foreach (PointF ptf in gp.PathPoints)
                //    //{
                //    //    if (IsValidPoint(pt, ptf))
                //    //    {
                //    //        valid = true;
                //    //        Cursor.Current = Cursors.SizeAll;
                //    //        break;
                //    //    }
                //    //}
                //    //if (valid)
                //    //    break;
                //}
            }
            else if (CurrentShape == Shapes.Polygon && points != null && points.Count > 0 && Math.Abs(points[0].Distance(this.MouseLocation)) < 5)
            {
                if (!overPolyFirst)
                {
                    overPolyFirst = true;
                    DrawPoly(true);
                }
            }
            else if (overPolyFirst)
            {
                overPolyFirst = false;
                DrawPoly(false);
            }
        }

        private void DrawPoly(bool first = false)
        {
            if (points == null)
                return;

            using (Graphics g = Graphics.FromImage(this.pbDrawing.Image))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                foreach (Point pt in points)
                {
                    g.FillEllipse(first ? highlightLineBrush : lineBrush, pt.X - 2, pt.Y - 2, 4, 4);
                    first = false;
                }
                g.DrawPath(linePen, lastPath);
            }
            this.pbDrawing.Refresh();
        }

        private void Store(bool raise = true)
        {
            lastPath.Flatten();
            if (lastPath.PointCount > 0)
            {
                using (Graphics g = Graphics.FromImage(this.pbDrawing.Image))
                {
                    g.Clear(Color.Transparent);
                }
                using (Graphics g = Graphics.FromImage(this.pbShapes.Image))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillPath(fillBrush, lastPath);
                }
                this.pbDrawing.Refresh();
                this.pbShapes.Refresh();
                this.Drawings.Add(lastPath);
                if (raise)
                {
                    DrawingAdded.RaiseEvent(this, new DrawingAddedEventArgs(lastPath));
                }
            }
            lastPath = null;
        }

        private bool IsValidPoint(Point pt1, PointF pt2)
        {
            float dX = pt1.X - pt2.X;
            float dY = pt1.Y - pt2.Y;
            return dX < 8 && dX > -8 && dY < 8 && dY > -8;
        }

        private void Draw()
        {
            using (Graphics g = Graphics.FromImage(this.pbDrawing.Image))
            {
                g.Clear(Color.Transparent);

                switch (this.CurrentShape)
                {
                    case Shapes.Ellipse:
                        HandleEllipse();
                        break;
                    case Shapes.Rectangle:
                        HandleRectangle();
                        break;
                    default:
                        return;
                }
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(fillBrush, lastPath);
            }
            this.pbDrawing.Refresh();
        }


        private Rectangle CreateRectangle(Point pt1, Point pt2, bool square)
        {
            int x;
            int y;
            int w;
            int h;
            if (pt1.X < pt2.X)
            {
                x = pt1.X;
                w = pt2.X - pt1.X;
            }
            else
            {
                x = pt2.X;
                w = pt1.X - pt2.X;
            }

            if (pt1.Y < pt2.Y)
            {
                y = pt1.Y;
                if (square)
                {
                    h = w;
                }
                else
                {
                    h = pt2.Y - pt1.Y;
                }
            }
            else
            {
                y = pt2.Y;
                if (square)
                {
                    h = w;
                }
                else
                {
                    h = pt1.Y - pt2.Y;
                }
            }

            return new Rectangle(x, y, w, h);
        }

        private void HandleRectangle()
        {
            lastPath = new GraphicsPath();
            if (!this.MouseDownLocation.HasValue)
                return;
            if (Control.ModifierKeys.HasFlag(Keys.Shift))
            {
                lastPath.AddRectangle(CreateRectangle(this.MouseDownLocation.Value, this.MouseLocation, true));
            }
            else
            {

                lastPath.AddRectangle(CreateRectangle(this.MouseDownLocation.Value, this.MouseLocation, false));
            }
        }

        private void HandleEllipse()
        {
            lastPath = new GraphicsPath();
            if (!this.MouseDownLocation.HasValue)
                return;
            if (Control.ModifierKeys.HasFlag(Keys.Shift))
            {
                lastPath.AddEllipse(this.MouseDownLocation.Value.X, this.MouseDownLocation.Value.Y, this.MouseLocation.X - this.MouseDownLocation.Value.X, this.MouseLocation.X - this.MouseDownLocation.Value.X);
            }
            else
            {
                lastPath.AddEllipse(this.MouseDownLocation.Value.X, this.MouseDownLocation.Value.Y, this.MouseLocation.X - this.MouseDownLocation.Value.X, this.MouseLocation.Y - this.MouseDownLocation.Value.Y);
            }
        }

        private void pbDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            this.MouseDownLocation = this.MouseLocation;
        }

        public void ClearShapes()
        {
            using (Graphics g = Graphics.FromImage(this.pbDrawing.Image))
            {
                g.Clear(Color.Transparent);
            }
            using (Graphics g = Graphics.FromImage(this.pbShapes.Image))
            {
                g.Clear(Color.Transparent);
            }
            this.Drawings.Clear();
            this.pbDrawing.Refresh();
            this.pbShapes.Refresh();
            lastPath = null;
        }

        public void AddShape(GraphicsPath graphicsPath)
        {
            lastPath = graphicsPath;
            Store(false);
        }
    }
}
