# Usando Satellite Assemblies con GTK# - (parte 2)

<p align="justify">
En este programa se mostrara un ejemplo que nos mostrara los pasos de como consumir los ensamblados satelite o ensamblados de recursos desde una aplicacion GTK#.
    </p>
    <p align="justify">
    Toda esta funcionalidad se encuentra en el metodo <tt>LoadResources()</tt> , este metodo comienza primeramente con la carga en tiempo de ejecucion del ensamblado que contiene los recursos utilizando las lineas siguientes:
    </p>
    <pre>
    string assem = "demo.resources.dll";
    Assembly assembly = Assembly.LoadFrom(assem);
    </pre>
    <p align="justify">
    A continuacion creamos una instancia de la clase <a href="http://msdn.microsoft.com/en-us/library/system.resources.resourcemanager.aspx">ResourceManager</a> en la cual se encuentran los metodos para obtener los recursos del ensamblado, en este ejemplo obtenemos un recurso de tipo cadena y otro de tipo imagen, con el codigo de las li­neas siguientes
    </p>
    <p>
    <pre>
    label1.Text += rm.GetString("query1"); 
    System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)rm.GetObject("pugme");
    </pre>
    </p>
    <p align="justify">
    Por ultimo unicamente se guarda la imagen en el directorio de la aplicacion para crear un objeto <i>Pixbuf</i> el cual se dibujara en un control <i>DrawingArea</i>, esto ocurre en las siguientes li­neas: 
    </p>
    <pre>
    bitmap.Save("pugme.png",System.Drawing.Imaging.ImageFormat.Png);
    pngbuf = new Pixbuf("pugme.png");
    </pre>
    <p align="justify">
    Los metodos presentados por este programa en GTK# aplican para cualquier otra aplicacion .NET incluso si el lenguaje de programacion utilizado no es C#.<br/>Compilamos la aplicacion y ejecutamos la aplicacion con los siguientes comandos desde una terminal:
    </p>
    <p>
    <pre>
    <tt>$ mcs -pkg:gtk-sharp-2.0 -r:System.Drawing Main.cs</tt>
    <tt>$ mono Main.exe</tt>
    </pre>
    </p>
    <div>
<img src="images/testres3.png" />
</div>
<br />
<div>Al ejecutar la aplicacion se mostrara como en la siguiente imagen:</div><br>
<div>
<img src="images/testres1.png" />
</div>
    <p align="justify">
    Al presionar el boton <i>Load resources</i> deberan de cargarse los recursos de cadena e imagen respectivamente, antes de ejecutar la aplicacion es importante verificar que el ensamblado <i>demo.resources.dll</i> se encuentre en el mismo directorio de la aplicacion.<br />
    <br />El resultado final se mostrara como en la siguiente imagen:
    </p>
    <div>
    <img src="images/testres2.png" /></div>
    <p align="justify">
    Parte del codigo de este programa se derivo del ejemplo 4-8 del capitulo 4 del libro <a href="http://books.google.com.mx/books/about/Mono.html?id=HyszoedfP3MC&redir_esc=y">Mono: A Developer's Notebook</a> de Niel M. Bornstein y Edd Dumbill
    </p>
