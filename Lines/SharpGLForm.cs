using System;
using System.Drawing;
using System.Windows.Forms;
using SharpGL;
using GlmNet;

namespace Lines
{
    public partial class SharpGLForm : Form
    {
        private readonly Scene scene = new Scene();
        // Rotation/Zoom/Pan
        private System.Object matrixLock = new System.Object();
        private arcball arcBall = new arcball(640.0f, 480.0f);
        private float[] matrix = new float[16];
        private Matrix4f LastTransformation = new Matrix4f();
        private Matrix4f ThisTransformation = new Matrix4f();
        // mouse 
        private Point mouseStartDrag;
        private static bool isLeftDrag = false;
        private static bool isRightDrag = false;
        private static bool isMiddleDrag = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenderEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            lock (matrixLock)
            {
                ThisTransformation.get_Renamed(matrix);
            }
            scene.SetViewMatrix(matrix);
            scene.Draw(openGLControl.OpenGL);
        }

        /// <summary>
        /// Calling when the event of dragging is active just at the beginning
        /// </summary>
        /// <param name="MousePt">Position of the mouse.</param>
        private void startDrag(Point MousePt)
        {
            lock (matrixLock)
            {
                LastTransformation.set_Renamed(ThisTransformation); // Set Last Static Rotation To Last Dynamic One
            }
            arcBall.click(MousePt); // Update Start Vector And Prepare For Dragging
            mouseStartDrag = MousePt;
        }

        /// <summary>
        /// Calling when the event of dragging is active all the time.
        /// </summary>
        /// <param name="MousePt">Position of the mouse.</param>
        private void drag(Point MousePt)
        {
            Quat4f ThisQuat = new Quat4f();
            arcBall.drag(MousePt, ThisQuat); // Update End Vector And Get Rotation As Quaternion
            lock (matrixLock)
            {
                if (isMiddleDrag) //zoom
                {
                    double len = Math.Sqrt(mouseStartDrag.X * mouseStartDrag.X + mouseStartDrag.Y * mouseStartDrag.Y)
                        / Math.Sqrt(MousePt.X * MousePt.X + MousePt.Y * MousePt.Y);

                    ThisTransformation.Scale = (float)len;
                    ThisTransformation.Pan = new Vector3f(0, 0, 0);
                    ThisTransformation.Rotation = new Quat4f();
                    ThisTransformation.MatrixMultiply(ThisTransformation, LastTransformation);// Accumulate Last Rotation Into This One
                }
                else if (isRightDrag) //pan
                {
                    float x = (float)(MousePt.X - mouseStartDrag.X) / (float)this.openGLControl.Width;
                    float y = (float)(mouseStartDrag.Y - MousePt.Y) / (float)this.openGLControl.Height;
                    float z = 0.0f;
                    
                    ThisTransformation.Pan = new Vector3f(x, y, z);
                    ThisTransformation.Scale = 1.0f;
                    ThisTransformation.Rotation = new Quat4f();
                    ThisTransformation.MatrixMultiply(ThisTransformation, LastTransformation);
                }
                else if (isLeftDrag) //rotate
                {
                    ThisTransformation.Pan = new Vector3f(0, 0, 0);
                    ThisTransformation.Scale = 1.0f;
                    ThisTransformation.Rotation = ThisQuat;
                    ThisTransformation.MatrixMultiply(ThisTransformation, LastTransformation);
                }
            }
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            // Initialise OpenGL here.
            scene.Initialise(openGLControl.OpenGL, Width, Height);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            //setting the new values of the windows
            scene.Reshape(openGLControl.OpenGL, control.Size.Width, control.Size.Height);
            arcBall.setBounds((float)control.Size.Width, (float)control.Size.Height); // Update mouse bounds for arcball
        }

        /// <summary>
        /// This method is NOT invoked yet, but if is required clean all the rotation/translation/scale must be invoked
        /// </summary>
        public void reset()
        {
            lock (matrixLock)
            {
                LastTransformation.SetIdentity();      // Reset Rotation
                ThisTransformation.SetIdentity();      // Reset Rotation
            }
        }

        #region mouse control
        private void openGLControl_MouseHover(object sender, System.EventArgs e)
        {
            this.Focus();
        }

        private void openGLControl_MouseWheel(object sender, MouseEventArgs e)
        {
            int iValue = e.Delta * SystemInformation.MouseWheelScrollLines / 60;
            mouseStartDrag = new Point(e.X, e.Y);
            this.startDrag(mouseStartDrag);
            Point tempAux = new Point(e.X, e.Y + iValue);
            isMiddleDrag = true;
            this.drag(tempAux);
            isMiddleDrag = false;
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                isLeftDrag = true;
                mouseStartDrag = new Point(e.X, e.Y);
                this.startDrag(mouseStartDrag);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) 
            {
                Cursor.Current = Cursors.SizeAll;
                isRightDrag = true;
                mouseStartDrag = new Point(e.X, e.Y);
                this.startDrag(mouseStartDrag);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle) 
            {
                Cursor.Current = Cursors.NoMove2D;
                isMiddleDrag = true;
                mouseStartDrag = new Point(e.X, e.Y);
                this.startDrag(mouseStartDrag);
            }
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point tempAux = new Point(e.X, e.Y);
            this.drag(tempAux);
        }
        
        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
                isLeftDrag = false;
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Cursor.Current = Cursors.Default;
                isRightDrag = false;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle) 
            {
                Cursor.Current = Cursors.Default;
                isMiddleDrag = false;
            }
        }

        #endregion
    }
}
