# Entendiendo Satellite Assemblies usando MonoDevelop - (parte 2)

En la primera parte de este tutorial, se mostró como crear Satellite Assemblies, ahora en este segunda parte se mostrará un listado donde se muestra el código que nos mostrará los pasos de como consumir los ensamblados satélite o ensamblados de recursos desde una aplicación GTK#.

<pre>
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
Application.Quit(); }
  
public static void Main (string[] args)
{
Application.Init();
new MainClass();
Application.Run();
}
  
void PlacePixbuf (Gdk.Pixbuf buf)
{
pixmap.DrawPixbuf (darea.Style.BlackGC,buf, 0, 0, 0, 0,buf.Width,
buf.Height,RgbDither.None, 0, 0);
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
pixmap.DrawRectangle (darea.Style.WhiteGC, true, 0, 0,allocation.Width,
allocation.Height);
}

void Expose_Event (object obj, ExposeEventArgs args)
{
Gdk.Rectangle area = args.Event.Area;
args.Event.Window.DrawDrawable (darea.Style.WhiteGC, 
pixmap,area.X, area.Y,area.X, area.Y,area.Width, area.Height);
}
  
void AddResource_Clicked (object obj, EventArgs args)
{
LoadResources();
PlacePixbuf (pngbuf);
}
}
}
</pre>

Toda esta funcionalidad se encuentra en el método LoadResources() , este método comienza primeramente con la carga en tiempo de ejecución del ensamblado que contiene los recursos utilizando las líneas siguientes:

string assem = "demo.resources.dll";
Assembly assembly = Assembly.LoadFrom(assem);
A continuación creamos una instancia de la clase ResourceManager en la cual se encuentran los métodos para obtener los recursos del ensamblado, en este ejemplo obtenemos un recurso de tipo cadena y otro de tipo imagen, con el código de las líneas siguientes

label1.Text += rm.GetString("query1"); 
System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)rm.GetObject("pugme");
Por último únicamente se guarda la imagen en el directorio de la aplicación para crear un objeto Pixbuf el cual se dibujará en un control DrawingArea, esto ocurre en las siguientes líneas:

bitmap.Save("pugme.png",System.Drawing.Imaging.ImageFormat.Png);
pngbuf = new Pixbuf("pugme.png");
Los métodos presentados por este programa en GTK# aplican para cualquier otra aplicación .NET incluso si el lenguaje de programación utilizado no es C#.
>Compilamos la aplicación y ejecutamos la aplicación con los siguientes comandos desde una terminal:

$ mcs –pkg:gtk-sharp-2.0 –r:System.Drawing Main.cs
$ mono Main.exe



Al ejecutar la aplicación se mostrará como en la siguiente imagen:




Al presionar el botón Load resources deberán de cargarse los recursos de cadena e imagen respectivamente, antes de ejecutar la aplicación es importante verificar que el ensamblado demo.resources.dll se encuentre en el mismo directorio de la aplicación.

El resultado final se mostrará como en la siguiente imagen:



Parte del código de este programa se derivo del ejemplo 4-8 del capítulo 4 del libro Mono: A Developer's Notebook de Niel M. Bornstein y Edd Dumbill

