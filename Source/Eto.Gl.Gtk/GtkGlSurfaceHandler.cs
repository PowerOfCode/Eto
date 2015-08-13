using Eto.Drawing;
using Eto.GtkSharp.Forms;
using OpenTK.Graphics;

namespace Eto.Gl.Gtk
{
	public class GtkGlSurfaceHandler : GtkControl<GLDrawingArea, GLSurface, GLSurface.ICallback>, GLSurface.IHandler
	{
		private GraphicsMode mode;
		private int major;
		private int minor;
		private GraphicsContextFlags flags;

		protected override void Initialize()
		{
			var c = new GLDrawingArea(mode, major, minor, flags);
			this.Control = c;

			base.Initialize();
		}

		public override void AttachEvent(string id)
		{
			switch (id)
			{
				case GLSurface.GLInitializedEvent:
					Control.Initialized += (sender, e) => Callback.OnInitialized(Widget, e);
					break;
				case GLSurface.GLDrawNowEvent:
					Control.Resize += (sender, e) => Callback.OnDrawNow(Widget, e);
					break;
				case GLSurface.GLShuttingDownEvent:
					Control.ShuttingDown += (sender, e) => Callback.OnShuttingDown(Widget, e);
					break;
				default:
					base.AttachEvent(id);
					break;
			}
		}

		public void CreateWithParams(GraphicsMode mode, int major, int minor, GraphicsContextFlags flags)
		{
			this.mode = mode;
			this.major = major;
			this.minor = minor;
			this.flags = flags;
		}

		public Size GLSize
		{
			get { return this.Control.GLSize; }
			set { this.Control.GLSize = value; }
		}

		public bool IsInitialized
		{
			get { return this.Control.IsInitialized; }
		}

		public void MakeCurrent()
		{
			this.Control.MakeCurrent();
		}

		public void SwapBuffers()
		{
			this.Control.SwapBuffers();
		}

		public void Refresh()
		{
			this.Control.SendExpose(new Gdk.Event(System.IntPtr.Zero));
		}

		protected override void Dispose(bool disposing)
		{
			this.Control.Dispose(disposing);
		}
	}

}
