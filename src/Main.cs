using System;
using Gtk;
using System.IO;
using System.Resources;
using Gdk;
using System.Reflection;

namespace TestResource
{
	class MainClass : Gtk.Window
	{
		DrawingArea darea = null;
		Label label1 = null;
		Button btnLoad = null;
		Pixmap pixmap;
		Pixbuf pngbuf;
		public MainClass():base("Test Resources"){
			BorderWidth = 8;
			this.DeleteEvent += new DeleteEventHandler(OnWindowDelete);
			Frame frame = new Frame("Load");
			Add(frame);
			VBox MainPanel = new VBox (false, 8);
			label1 = new Label("Query is: ");
			darea = new DrawingArea();
			btnLoad = new Button("Load resources");
			btnLoad.Clicked += AddResource_Clicked;
			darea.SetSizeRequest (200, 200);
			darea.ExposeEvent += Expose_Event;
			darea.ConfigureEvent += Configure_Event;
			MainPanel.Add(label1);
			MainPanel.PackStart(darea);
			MainPanel.Add(btnLoad);
			frame.Add (MainPanel);
			SetDefaultSize (320, 233);
			Resizable = false;
			ShowAll();
		}
		public void OnWindowDelete(object o, DeleteEventArgs args) {
			Application.Quit();	}
		
		public static void Main (string[] args)
		{
			Application.Init();
			new MainClass();
			Application.Run();
		}
		
		void PlacePixbuf (Gdk.Pixbuf buf)
  		{
    	pixmap.DrawPixbuf (darea.Style.BlackGC,buf, 0, 0, 0, 0,buf.Width, buf.Height,RgbDither.None, 0, 0);
    	darea.QueueDrawArea (0, 0, buf.Width, buf.Height);
  		}
		
		void LoadResources(){
			try{
				//find the assembly
				string assem = "demo.resources.dll";
				Assembly assembly = Assembly.LoadFrom(assem);
				if(File.Exists(assem))
				{
					//Instance for resourcemanager
			    ResourceManager rm = new ResourceManager("demo",assembly);
					//get the string for the resource
				label1.Text += rm.GetString("query1");	
					//get the image for the resource
				System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)rm.GetObject("pugme");
				bitmap.Save("pugme.png",System.Drawing.Imaging.ImageFormat.Png);
				pngbuf = new Pixbuf("pugme.png");
				}
			}catch(Exception e){
				Console.WriteLine(e.Message);
			}
		}
		
		void Configure_Event (object obj, ConfigureEventArgs args)
  		{
    		Gdk.EventConfigure ev = args.Event;
    		Gdk.Window window = ev.Window;
    		Gdk.Rectangle allocation = darea.Allocation;
    		pixmap = new Gdk.Pixmap (window, allocation.Width,allocation.Height, -1);
    		pixmap.DrawRectangle (darea.Style.WhiteGC, true, 0, 0,allocation.Width, allocation.Height);
  		}

		void Expose_Event (object obj, ExposeEventArgs args)
  		{
    		Gdk.Rectangle area = args.Event.Area;
    		args.Event.Window.DrawDrawable (darea.Style.WhiteGC, pixmap,area.X, area.Y,area.X, area.Y,area.Width, area.Height);
  		}
		
		void AddResource_Clicked (object obj, EventArgs args)
  		{
			LoadResources();
    		PlacePixbuf (pngbuf);
  		}
	}
}
