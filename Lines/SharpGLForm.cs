using System;
using System.Drawing;
using System.Windows.Forms;
using SharpGL;
using GlmNet;

namespace Lines
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        private readonly Scene scene = new Scene();

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
            scene.Draw(openGLControl.OpenGL);
            ////  Get the OpenGL object.
            //OpenGL gl = openGLControl.OpenGL;

            ////  Clear the color and depth buffer.
            //gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            ////  Load the identity matrix.
            //gl.LoadIdentity();

            ////  Rotate around the Y axis.
            //gl.Rotate(rotation, 0.0f, 1.0f, 0.0f);

            ////  Draw a coloured pyramid.
            //gl.Begin(OpenGL.GL_TRIANGLES);
            //gl.Color(1.0f, 0.0f, 0.0f);
            //gl.Vertex(0.0f, 1.0f, 0.0f);
            //gl.Color(0.0f, 1.0f, 0.0f);
            //gl.Vertex(-1.0f, -1.0f, 1.0f);
            //gl.Color(0.0f, 0.0f, 1.0f);
            //gl.Vertex(1.0f, -1.0f, 1.0f);
            //gl.Color(1.0f, 0.0f, 0.0f);
            //gl.Vertex(0.0f, 1.0f, 0.0f);
            //gl.Color(0.0f, 0.0f, 1.0f);
            //gl.Vertex(1.0f, -1.0f, 1.0f);
            //gl.Color(0.0f, 1.0f, 0.0f);
            //gl.Vertex(1.0f, -1.0f, -1.0f);
            //gl.Color(1.0f, 0.0f, 0.0f);
            //gl.Vertex(0.0f, 1.0f, 0.0f);
            //gl.Color(0.0f, 1.0f, 0.0f);
            //gl.Vertex(1.0f, -1.0f, -1.0f);
            //gl.Color(0.0f, 0.0f, 1.0f);
            //gl.Vertex(-1.0f, -1.0f, -1.0f);
            //gl.Color(1.0f, 0.0f, 0.0f);
            //gl.Vertex(0.0f, 1.0f, 0.0f);
            //gl.Color(0.0f, 0.0f, 1.0f);
            //gl.Vertex(-1.0f, -1.0f, -1.0f);
            //gl.Color(0.0f, 1.0f, 0.0f);
            //gl.Vertex(-1.0f, -1.0f, 1.0f);
            //gl.End();

            ////  Nudge the rotation.
            //rotation += 3.0f;
        }



        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  TODO: Initialise OpenGL here.
            scene.Initialise(openGLControl.OpenGL, Width, Height);
            //  Get the OpenGL object.
            //OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            //gl.ClearColor(0, 0, 0, 0);
            SetBounds(width, height);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            ////  TODO: Set the projection matrix here.
            
            ////  Get the OpenGL object.
            //OpenGL gl = openGLControl.OpenGL;

            ////  Set the projection matrix.
            //gl.MatrixMode(OpenGL.GL_PROJECTION);

            ////  Load the identity.
            //gl.LoadIdentity();

            ////  Create a perspective transformation.
            //gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 100.0);

            ////  Use the 'look at' helper function to position and aim the camera.
            //gl.LookAt(-5, 5, -5, 0, 0, 0, 0, 1, 0);

            ////  Set the modelview matrix.
            //gl.MatrixMode(OpenGL.GL_MODELVIEW);
            Control control = (Control)sender;

            SetBounds(control.Size.Width, control.Size.Height);
            scene.Reshape(openGLControl.OpenGL, control.Size.Width, control.Size.Height);
        }

        /// <summary>
        /// The current rotation.
        /// </summary>
        private float rotation = 0.0f;
        private Point pInitial= new Point(0);
        private Point pCurrent = new Point(0);
        Boolean bIsDragging = false;
        mat4 mTransformation = new mat4(0);
        vec3 lastPoint = new vec3(0);

        private vec3 crossProduct(vec3 a, vec3 b)
        {
            vec3 c = new vec3( a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x );
            return c;
        }
        public float getLength(vec3 v)
        {
            return (float)Math.Sqrt((v.x * v.x) + (v.y * v.y) + (v.z * v.z));
        }
        float angle, length, radiusRadius;
        Double[] currentTransform = new Double[16], lastTransform = new Double[16] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
        vec3 startPosition, endPosition, normalVector;
        Point translate = new Point(0);

        private int width;
        private int height;
        public void SetBounds(int width, int height)
        {
            this.width = width; this.height = height;
            length = width > height ? width : height;
            radiusRadius = (float)(Math.Pow((width / length) / 2, 2) + Math.Pow((height / length) / 2, 2));
            if (radiusRadius < 0)
            {
                radiusRadius = 0;
            }
        }
        public void setClick(int x, int y)
        {
            double zz = radiusRadius
                - Math.Pow((x - width / 2) / length, 2)
                - Math.Pow((height / 2 - y) / length, 2);
            if (zz >= 0)
            {
                startPosition = new vec3(
                   (x - width / 2) / length,
                   (height / 2 - y) / length,
                   (float)Math.Sqrt(zz));
            }
            else
            {
                startPosition = new vec3(
                   (x - width / 2) / length,
                   (height / 2 - y) / length,
                   0);
            }
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            pInitial.X = e.X;
            pInitial.Y = e.Y;
            bIsDragging = true;
            if (e.Button == MouseButtons.Left) 
            {
                setClick(e.X, e.Y);
                
            }
        }
        
        float getScalarProduct(vec3 a, vec3 b) 
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (bIsDragging && e.Button == MouseButtons.Left) 
            {
                Point pNew = new Point(e.X, e.Y);
                vec3 curPoint = scene.trackBallMapping(pNew);
                vec3 direction = curPoint - lastPoint;
                float rot_angle, zoom_factor;
                float velocity = scene.getLength(direction);
                if (velocity > 0.0001) // If little movement - do nothing.
                {
                    //vec3 rotAxis = new vec3(0);
                    //rotAxis = crossProduct(lastPoint, curPoint);
                    //rot_angle = velocity * 0.05f;   //ROT_SCALE
                    //mTransformation = glm.rotate(rot_angle, rotAxis);
                    //scene.SetModelMatrix(mTransformation);
                    
                }
                //Point pCurrent = new Point (pInitial.X - e.X, pInitial.Y - e.Y);
                //vec3 vectRot = new vec3(pCurrent.X, pCurrent.Y, 0);
                //mTransformation = glm.rotate(0.03f, vectRot);
                //scene.SetModelMatrix(mTransformation);
                //pInitial.X = e.X;
                //pInitial.Y = e.Y;
                //lastPoint.x = e.X;
                //lastPoint.y = e.Y;
                endPosition = new vec3(
                    (e.X - width / 2) / length,
                    (height / 2 - e.Y) / length,
                    (float)Math.Sqrt(
                        radiusRadius
                        - Math.Pow((e.X - width / 2) / length, 2)
                        - Math.Pow((height / 2 - e.Y) / length, 2))
                            );
                vec3 p = new vec3(0);
                //p = crossProduct(startPosition, )
                float l = getScalarProduct(startPosition, endPosition) / (getLength(startPosition) * getLength(endPosition));
                float newV = getLength(startPosition) * getLength(endPosition);
                angle = 10 * (float)(Math.Acos(newV)) / (float)(Math.PI * 180.0);
                //normalVector = startPosition.VectorProduct(endPosition);
                //normalVector = crossProduct(startPosition, endPosition);
                normalVector = crossProduct(endPosition, startPosition);
                startPosition = endPosition;
                mTransformation = glm.rotate(2*angle, normalVector);
                scene.SetModelMatrix(mTransformation);
            }
            else if (bIsDragging && e.Button == MouseButtons.Right)
            {
                vec3 t = new vec3(0);
                t.x = 0.01f*(pInitial.X - e.X);
                t.y = 0.01f*(pInitial.Y - e.Y);
                mTransformation = glm.translate(new mat4(1), t);
                scene.SetModelMatrix(mTransformation);
                pInitial.X = e.X;
                pInitial.Y = e.Y;
            }
        }
        
        private void openGLControl_MouseWheel(object sender, MouseEventArgs e) 
        {
        
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            bIsDragging = false;
        }

        private void openGLControl_MouseHover(object sender, EventArgs e)
        {
            openGLControl.Focus();
        }
    }
}
