using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using OpenTK;

namespace Eto.Gl.Windows
{
    /// <summary>
    /// Interaction logic for WpfUserControl.xaml
    /// </summary>
    public partial class WpfUserControl : System.Windows.Controls.UserControl
    {
        public event EventHandler ShuttingDown = delegate { };
        private GraphicsMode mode;
        private int major;
        private int minor;
        private GraphicsContextFlags flags;
        public GLControl glc;
        private GLSurface Widget;

        public WpfUserControl(GraphicsMode mode, int major, int minor, GraphicsContextFlags flags, GLSurface Widget)
        {
            // TODO: Complete member initialization
            this.mode = mode;
            this.major = major;
            this.minor = minor;
            this.flags = flags;
            this.Widget = Widget;
            InitializeComponent();
            glc = new GLControl(mode, major, minor, flags);
            glc.Load += (sender, args) => {
                Widget.OnGLInitalized(args);
            };
            glControl.Child = glc;
        }

        public Drawing.Size GLSize
        {
            get
            {
                return glc.Size.ToEto();
            }
            set
            {
                glc.Size = value.ToSD();
            }
        }

        internal void MakeCurrent()
        {
            glc.MakeCurrent();
        }

        internal void SwapBuffers()
        {
            glc.SwapBuffers();
        }
    }
}
