using System;
using GlmNet;
using SharpGL;
using SharpGL.Shaders;
using SharpGL.VertexBuffers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Lines
{
    public class Scene
    {
        public const String DLL_PATH = "DLLNative.dll";

        [DllImport(DLL_PATH, EntryPoint = "getLineData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe static extern void getLineData(float[] pColor, float[] pLine, ref int nLines, float[] fCenter, ref float fScale);
        #region parameters
        //fixed parameters
        const float angle = 45.0f;
        const float NCP = 0.01f;
        const float FCP = 500.0f;
        //window parameters
        int iWidth = 0;
        int iHeight = 0;
        //  The projection, view and model matrices.
        mat4 projectionMatrix;
        mat4 viewMatrix;
        mat4 modelMatrix;
        //object info
        float fScale;
        vec3 center;
        int nLines = 0; //number of lines read
       

        //  Constants that specify the attribute indexes.
        const uint attributeIndexPosition = 0;
        const uint attributeIndexColour = 1;

        //  The vertex buffer array which contains the vertex and colour buffers.
        VertexBufferArray vertexBufferArray;

        //  The shader program for our vertex and fragment shader.
        private ShaderProgram shaderProgram;
        #endregion parameters

        /// <summary>
        /// Initialises the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public void Initialise(OpenGL gl, float width, float height)
        {
            //  Set a blue clear colour.
            gl.ClearColor(0.2f, 0.2f, 0.2f, 0.0f);

            //  Create the shader program.
            var vertexShaderSource = ManifestResourceLoader.LoadTextFile("Shader.vert");
            var fragmentShaderSource = ManifestResourceLoader.LoadTextFile("Shader.frag");
            shaderProgram = new ShaderProgram();
            shaderProgram.Create(gl, vertexShaderSource, fragmentShaderSource, null);
            shaderProgram.BindAttributeLocation(gl, attributeIndexPosition, "in_Position");
            shaderProgram.BindAttributeLocation(gl, attributeIndexColour, "in_Color");
            shaderProgram.AssertValid(gl);

            iWidth = Convert.ToInt32(width);
            iHeight = Convert.ToInt32(height);
            //  Create a perspective projection matrix.
            projectionMatrix = new mat4(1.0f); //it will be change after in resize

            //  Now create the geometry for the set of lines.
            CreateVerticesForLines(gl);

            //  Create a view matrix to move us back a bit.
            viewMatrix = glm.translate(new mat4(1.0f), new vec3(0.0f, 0.0f, -0.5f));

            //  Create a model matrix to make the model a little bigger.
            center.x = -center.x;
            center.y = -center.y;
            center.z = -center.z;
            modelMatrix = glm.translate(new mat4(1.0f), center);
            modelMatrix = glm.scale(new mat4(1.0f), new vec3(fScale, fScale, fScale)) * modelMatrix;
        }

        /// <summary>
        /// Is invoked when the window change of size.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public void Reshape(OpenGL gl, int iWidth, int iHeight) 
        {
            this.iWidth = iWidth;
            this.iHeight = iHeight;
            if(iHeight == 0) iHeight = 1;
	        float ratio = iWidth / (float)iHeight;
            gl.Viewport(0, 0, iWidth, iHeight);
            projectionMatrix = GlmNet.glm.perspective(angle, ratio, NCP, FCP);
        }

        /// <summary>
        /// Updates the view matrix in the pipeline
        /// </summary>
        /// <param name="mMatrix">The new view matrix as a float array.</param>
        public void SetViewMatrix(float[] mMatrix)
        {
            
            vec4 c1, c2, c3, c4;
            c1.x = mMatrix[0];
            c2.x = mMatrix[1];
            c3.x = mMatrix[2];
            c4.x = mMatrix[3];

            c1.y = mMatrix[4];
            c2.y = mMatrix[5];
            c3.y = mMatrix[6];
            c4.y = mMatrix[7];

            c1.z = mMatrix[8];
            c2.z = mMatrix[9];
            c3.z = mMatrix[10];
            c4.z = mMatrix[11];

            c1.w = mMatrix[12];
            c2.w = mMatrix[13];
            c3.w = mMatrix[14];
            c4.w = mMatrix[15];
            mat4 ma = new mat4(c1, c2, c3, c4);
            modelMatrix = ma * viewMatrix;
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Draw(OpenGL gl)
        {
            //  Clear the scene.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);

            //  Bind the shader, set the matrices.
            shaderProgram.Bind(gl);
            shaderProgram.SetUniformMatrix4(gl, "projectionMatrix", projectionMatrix.to_array());
            shaderProgram.SetUniformMatrix4(gl, "viewMatrix", viewMatrix.to_array());
            shaderProgram.SetUniformMatrix4(gl, "modelMatrix", modelMatrix.to_array());

            //  Bind the out vertex array.
            vertexBufferArray.Bind(gl);

            //  Draw the lines.
            gl.DrawArrays(OpenGL.GL_LINES, 0, nLines*2);

            //  Unbind our vertex array and shader.
            vertexBufferArray.Unbind(gl);
            shaderProgram.Unbind(gl);
        }

        /// <summary>
        /// Creates the geometry for lines, also creating the vertex buffer array.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        private void CreateVerticesForLines(OpenGL gl)
        {    
            float[] pLines = null;
            float[] pColor = null;
            float[] theCenter = new float[3];
            float some = 0;
            unsafe 
            {
                getLineData(null, null, ref nLines, theCenter, ref some);
                pLines = new float[nLines*3*2];
                pColor = new float[nLines*4*2];
                getLineData(pColor, pLines, ref nLines, theCenter, ref fScale);
            }
            //copy the center
            center.x = theCenter[0];
            center.y = theCenter[1];
            center.z = theCenter[2];
            //  Create the vertex array object.
            vertexBufferArray = new VertexBufferArray();
            vertexBufferArray.Create(gl);
            vertexBufferArray.Bind(gl);

            //  Create a vertex buffer for the vertex data.
            var vertexDataBuffer = new VertexBuffer();
            vertexDataBuffer.Create(gl);
            vertexDataBuffer.Bind(gl);
            //vertexDataBuffer.SetData(gl, 0, vertices, false, 3);
            vertexDataBuffer.SetData(gl, 0, pLines, false, 3);

            //  Now do the same for the colour data.
            var colourDataBuffer = new VertexBuffer();
            colourDataBuffer.Create(gl);
            colourDataBuffer.Bind(gl);
            colourDataBuffer.SetData(gl, 1, pColor, false, 4);

            //  Unbind the vertex array, we've finished specifying data for it.
            vertexBufferArray.Unbind(gl);
        }

    }
}
