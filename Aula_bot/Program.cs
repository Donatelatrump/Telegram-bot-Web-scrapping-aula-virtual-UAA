using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using HtmlAgilityPack;
using System.Diagnostics;
using Telegram.BotAPI;
using Telegram.BotAPI.GettingUpdates;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.InlineMode;
using System.Threading.Tasks;
using ScrapySharp;
using ScrapySharp.Extensions;
using Telegram.BotAPI.AvailableTypes;
using File = System.IO.File;
using System.Globalization;
using ScrapySharp.Html;
using By = OpenQA.Selenium.By;
using OpenQA.Selenium.Interactions;
using static Microsoft.FSharp.Core.ByRefKinds;
using Microsoft.VisualBasic;
using System.Threading;
using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.DevTools.V106.Debugger;
using System.Security.Cryptography.X509Certificates;



//Declaracion de variables necesarias, (es posible rutilizar variables )
int contador_temporal = 0, contador_original = 0, contador2 = 0, contadores = 0, contadorcito = 0, supercontador = 0, saberes = 0, contador_uni = 0, ultracontador=0,megacontador =0,conta=0,sapo=0;
string texto2="",lineas="", temporal="", aveces = "", cmp = "", tareas_diferencias = "", texto = "", fechas_aula, fechas_aula2 = "", eventos_aula, fechaderemplazo = "", Admins = "", texto22 = "", fecha_actual = "";
//Declaracion de las rutas de los archivos que almacenan la informacion (debo encontrar una mejor manera de guardarlo, una manera mas eficiente)
 string direct = Directory.GetCurrentDirectory();
 string path_tareas = direct + "\\Eventos.txt";
 string path_ip = direct + "\\ips2.txt";
 string path_fecha = direct + "\\Fechas.txt";
 string path_suscritos = direct + "\\Suscritos2.txt";
 string path_admins = direct + "\\Admins.txt";
 string path_datosOr = direct + "\\DatosOr.txt";
 string path_datosOrLDI = direct + "\\DatosOrLDI.txt";
 string path_datosOrTemp = direct + "\\DatosOrTemp.txt";
 string path_suscritosLDI = direct + "\\SuscritosLDI.txt";
 string path_datosOrTempLDI = direct + "\\DatosOrTempLDI.txt";
 string path_fecha_LDI = direct + "\\Fechas_LDI.txt";
 string path_tareas_LDI = direct + "\\Eventos_LDI.txt";
 string path_tareas_ici2 = direct + "\\EventosICI2.txt";
 string path_fechas_ici2 = direct + "\\FechasICI2.txt";
 string path_suscritos_ici2 = direct + "\\SuscritosICI2.txt";
 string path_datosOrICI2 = direct + "\\DatosOrICI2.txt";
 string path_datosOrTempICI2 = direct + "\\DatosOrTempICI2.txt";
//Variables de acceso a la pagina de la Uni
string usuarioICI = "al283189", usuarioLDI = "al263887", usuarioICI2 = "al261731";
string contrasenaICI = "Donnet0708", contrasenaLDI = "Wera060102", contrasenaICI2 = "SPjl3490";
char[] fechita = new char[2];
char[] numeros = new char[50];
char[] FECHa = new char[3];
char[] fecha = new char[2];
bool bandera,veces = false, verdad = false,minecraft,LDI = false, altiro =false;
//Declaracion de los objetos del bot
var bot = new BotClient("5681430643:AAGs0-yVSMuFnjix8YCzEqOg29JZlqy1W98");
var updates = bot.GetUpdates();
//Funcion para detectar programas activos en windows -> en este caso detecta minecraft para evitar que el bot se cierre mientras el usuario juega
void revisar_minecraft_activo()
{
    Process[] actividad = Process.GetProcessesByName("java");
    if (actividad.Length == 0)
    {
        minecraft = false;
    }
    else
    {
        minecraft = true;
    }
}
//Funcion de web scrapping para sacar las tareas y fechas de la uni dependiendo de que carrera sea
string aula(string path, string Usuario, string Password2, string fecha1,string tarea1,string update)
{
    
    saberes = 0;
    contador_uni = 0;
    texto22 = "";
    //Tomar solo los dias de la fecha actual 
    DateTime thisDay = DateTime.Today;
    fecha[0] = thisDay.ToString()[0];
    fecha[1] = thisDay.ToString()[1];
    fecha_actual += fecha[0];
    fecha_actual += fecha[1];
    //Abrir chomre en aula y enviarle los datos de acceso 
    IWebDriver driver = new ChromeDriver();
   driver.Manage().Window.Minimize();
    try { 
        driver.Navigate().GoToUrl("https://aulavirtual.uaa.mx/login/index.php");
        var user = driver.FindElement(By.Name("username"));
        user.SendKeys(Usuario);
        var contra = driver.FindElement(By.Name("password"));
        contra.SendKeys(Password2);
        contra.Submit();
        driver.Navigate().GoToUrl("https://aulavirtual.uaa.mx/calendar/view.php?view=month");
    } catch(Exception inter) { Console.WriteLine(inter);
        try
        {
            bot.SendMessage(update, "Aula esta caido");
        }catch(Exception ar)
        {
            Console.WriteLine(ar);
        }
       
        driver.Quit();
        return "a";
    }
    //Despues de ingresar dirigirse a la pagina de calendario
    if (driver.Url == "https://aulavirtual.uaa.mx/calendar/view.php?view=month")
    {
        //Con esta variable tomar todos los nombres de las tareas 
        var numero_de_tareas = driver.FindElements(By.ClassName("eventname"));
        //Con esta variable tomar todas las fechas que tengan una tarea en su interior
        var fecha_tareas = driver.FindElements(By.ClassName("sr-only"));
        //Consigue y limpia todas las tareas
        foreach (var item2 in fecha_tareas)
        {
            if (sapo == 72) { break;}//Condicional para que no de vueltas de mas inecesarias
            if (new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' }.Any(x => item2.Text.Contains(x)))
            {
                if (!item2.Text.Contains("Sin eventos") && !item2.Text.Contains("Omitir"))
                {
                    //En estas revisiones lo que hago es conseguir el dia exacto en que son las entregas, como los dias de la semana cambian cada mes su posicion
                    //lo que hago es contar los caracteres de cada caso del dia de la semana para saber en qu posicion del string esta la fecha, asi sea de 2 o 1 digito
                    //asi como si las tareas son mayores a 1 una palabra del arreglo cambia de evento a eventos lo que hace que la posicion de la fecha cambie
                    //despues la comparo con la fecha actual y si es igual o mayor las guardo en un archivo de texto
                    fechaderemplazo = "";
                    //Para Evento
                    //Para el lunes PA
                    if (item2.Text.Contains("lunes") && item2.Text.Contains("evento"))
                    {
                        FECHa[0] = item2.Text.ToString()[17];
                        FECHa[1] = item2.Text.ToString()[18];
                    }

                    //Para Martes Jueves y Sabado PA                                                  
                    if (new[] { "martes", "sábado", "jueves", "evento" }.Any(x => item2.Text.Contains(x)))
                    {
                        FECHa[0] = item2.Text.ToString()[18];
                        FECHa[1] = item2.Text.ToString()[19];
                    }
                    //Para Miercoles PA
                    if (item2.Text.Contains("miércoles") && item2.Text.Contains("evento"))
                    {
                        FECHa[0] = item2.Text.ToString()[21];
                        FECHa[1] = item2.Text.ToString()[22];
                    }
                    //Para Viernes y domingo PA
                    if (item2.Text.Contains("viernes") || item2.Text.Contains("domingo") && item2.Text.Contains("evento"))
                    {
                        FECHa[0] = item2.Text.ToString()[19];
                        FECHa[1] = item2.Text.ToString()[20];
                    }
                    //Para Eventos multiples
                    //Para Martes, Jueves y Sabado PA
                    if (item2.Text.Contains("sábado") && item2.Text.Contains("eventos"))
                    {
                        FECHa[0] = item2.Text.ToString()[19];
                        FECHa[1] = item2.Text.ToString()[20];
                    }
                    if (new[] { "martes", "lunes", "eventos" }.Any(z => item2.Text.Contains(z)))
                    {
                        FECHa[0] = item2.Text.ToString()[18];
                        FECHa[1] = item2.Text.ToString()[19];
                    }
                    if (item2.Text.Contains("jueves") && item2.Text.Contains("eventos"))
                    {
                        FECHa[0] = item2.Text.ToString()[19];
                        FECHa[1] = item2.Text.ToString()[20];
                    }
                    //Para domingo y viernes PA
                    if (item2.Text.Contains("domingo") && item2.Text.Contains("eventos"))
                    {
                        FECHa[0] = item2.Text.ToString()[20];
                        FECHa[1] = item2.Text.ToString()[21];
                    }
                    if (item2.Text.Contains("viernes") && item2.Text.Contains("eventos"))
                    {
                        FECHa[0] = item2.Text.ToString()[20];
                        FECHa[1] = item2.Text.ToString()[21];
                    }

                    //Para miercoles PA
                    if (item2.Text.Contains("miércoles") && item2.Text.Contains("eventos"))
                    {
                        FECHa[0] = item2.Text.ToString()[22];
                        FECHa[1] = item2.Text.ToString()[23];
                    }
                    if (FECHa[0] != ' ')
                    {
                        fechaderemplazo += FECHa[0];
                    }
                    if (FECHa[1] != ' ')
                    {
                        fechaderemplazo += FECHa[1];
                    }
                    if (Int32.Parse(fechaderemplazo) >= Int32.Parse(fecha_actual))
                    {
                        fechas_aula2 += item2.Text.ToString() + "\n";
                    }
                    else
                    {
                        FECHa[2] = item2.Text.ToString()[0];
                        saberes += Int32.Parse(FECHa[2].ToString());
                    }
                }
                fechaderemplazo = "";
            }
            sapo++;
        }
        sapo = 0;
        foreach (var item in numero_de_tareas)
        { 
                if (sapo == (numero_de_tareas.Count - 6)){break; }//Condicional para que no de vueltas de mas inecesarias
                //Limpiar info basura que arroja la pagina
                if (!item.Text.Contains("Ocultar"))
                {
                    contador_uni += 1;

                    //Decirle que mientras el contador de las tareas leidas sea mayor al contador de fechas ignoradas debe seguir leyendo tareas
                    if (contador_uni > saberes)
                    {
                        texto22 = texto22 + item.Text.ToString() + "\n";
                    }
                }
            sapo++;
        }
        sapo = 0;
        saberes = 0;
        contador_uni = 0;
        //Para el texto de las tareas
        StreamReader Nanda = File.OpenText(fecha1);
        StreamReader Even = File.OpenText(tarea1);
        if (Nanda != null && Even != null)
        {
            //Fechas
            fechas_aula = Nanda.ReadToEnd();
            Nanda.Close();
            StreamWriter dayo = new(fecha1);
            if (fechas_aula2 != "")
            {
                if (fechas_aula != fechas_aula2)
                {
                    dayo.Write(fechas_aula2);
                    dayo.Close();
                }
                else
                {
                    dayo.Write(fechas_aula2);
                    dayo.Close();
                }
            }
            else
            {
                dayo.Write(fechas_aula);
                dayo.Close();
            }
            //Eventos
            fechas_aula2 = "";
            eventos_aula = Even.ReadToEnd();
            Even.Close();
            StreamWriter eventos = new(tarea1);
            if (texto22 != "")
            {
                if (eventos_aula != texto22)
                {
                    eventos.WriteLine(texto22);
                    eventos.Close();
                }
                else
                {
                    eventos.WriteLine(texto22);
                    eventos.Close();
                }
            }
            else
            {
                eventos.WriteLine(eventos_aula);
                eventos.Close();
            }
        }
        else
        {
            Console.WriteLine("No se pudo abrir el archivo");
        }
        //Contador de lineas de eventos
        StreamReader Primer_evento = File.OpenText(fecha1);
        contador2 = 0;
        while (Primer_evento.ReadLine() != null)
        {
            contador2++;
        }
        Primer_evento.Close();
        //Contador de Eventos de las Fechas 
        StreamReader Primer_evento1 = File.OpenText(fecha1);
        if (Primer_evento1 != null)
        {
            for (int j = 0; j < contador2; j++)
            {
                var xc = "";
                if((xc=Primer_evento1.ReadLine())!= null)
                {
                    temporal = xc;
                }
                numeros[j] = temporal[0];
            }
            Primer_evento1.Close();
        } //Ciclo de guardado de los datos ordenados tipo baraja, primero la fecha y se cuenta el numero de eventos que este tiene 
          //este numero de eventos se envia a un segundo for anidado como parametro y se toman las primeras tareas y asi hasta que el contador de 
          //las fechas lleguen a 0 y los datos esten bien ordenados
        StreamReader lolcito = File.OpenText(tarea1);
        Primer_evento = File.OpenText(fecha1);
        StreamWriter aiuda = new(path);
        for (int i = 0; i < contador2; i++)
        {
            var ase = "";
            if ((ase = Primer_evento.ReadLine()) != null)
            {
                lineas = ase;
            }
            aiuda.WriteLine(lineas);
            int integer = numeros[i] - '0';
            for (int j = 0; j < integer; j++)
            {
                var lo = "";
                if ((lo = lolcito.ReadLine()) != null)
                {
                    aveces = lo;
                }
                aiuda.WriteLine(aveces);
            }
            contadores += contador2;
        }
        //se cierran todos los archivos y el metodo de chrome
        aiuda.Close();
        Primer_evento.Close();
        lolcito.Close();
        var a = driver.FindElement(By.XPath("//*[@id=\"action-menu-0-menu\"]/a[6]"));
        var e = a.GetAttribute("href");
        driver.Navigate().GoToUrl(e);
        driver.Quit();

        //contamos cuantas lineas tiene el documento final ordenado y lo guardamos en una variable 
        StreamReader ola = File.OpenText(path);
        supercontador = 0;
        while (ola.ReadLine() != null)
        {
            supercontador += 1;
        }
        ola.Close();
        return "a";
    }
    else
    {
        try
        {
            bot.SendMessage(update, "Al parecer alguna de nuestros accesos esta caido, reportalo con el desarrollador por favor :3");
        }catch(Exception al)
        {
            Console.WriteLine(al);
        }
        return "a";
    }
}
//Funcion para revisar si las tareas que tenemos son las mismas que en la pagina de la uni(basicamente funciona como la funcion de web scrapping pero aqui se revisa 
bool revision(string lolo, string chavo,string update)
{
    tareas_diferencias = "";
    contador_original = 0;
    contador_temporal = 0;
    if(lolo == path_datosOr)
    {
        aula(path_datosOrTemp, usuarioICI, contrasenaICI, path_fecha, path_tareas, update);
    }else if(lolo == path_datosOrLDI)
    {
        aula(path_datosOrTempLDI, usuarioLDI, contrasenaLDI, path_fecha_LDI, path_tareas_LDI, update);
    }else if(lolo == path_datosOrICI2)
    {
        aula(path_datosOrTempICI2, usuarioICI2, contrasenaICI2, path_fechas_ici2, path_tareas_ici2, update);
    }
    //Contador de lineas original
    StreamReader temporalci2 = File.OpenText(lolo);
    //se cuenta el numero de lineas del archivo inicial de tareas de aula 
    while (temporalci2.ReadLine() != null)
    {
        contador_original += 1;
    }
    temporalci2.Close();
    //se cuentan las lienas del nuevo archivo de tareas de aula
    StreamReader temporalci = File.OpenText(chavo);
    while (temporalci.ReadLine() != null)
    {
        contador_temporal += 1;
    }
    temporalci.Close();

    //Si el numero de lienas del nuevo archivo es menor al numero de lineas del original o mayor
    if (contador_temporal > contador_original)
    {
        //se lee el nuervo archivo y se envian las lineas que tenga este 
        StreamReader aioch = File.OpenText(chavo);
        for (int j = 0; j < contador_temporal; j++)
        {
            var gf = "";
            if ((gf = aioch.ReadLine()) != null)
            {
                tareas_diferencias += gf;
            }
        }
        aioch.Close();

    }
    //mientras que el string tareas_diferencias no este vacia (significa que si hubo nuevas tareas)
    if (tareas_diferencias != "")
    {
        //se retorna un verdadero que dice que si hubo cambios
        return true;
    }
    else
    {
        //si no se retorna un false que no hubo cambios
        return false;
    }
}
//Funcion para conocer la ip del servidor de Minecraft
void direccion_ip()
{
    //hace webscraping por consola hacia un sitio que marca la ip del equipo
    HtmlWeb oWeb = new();
    HtmlDocument doc = oWeb.Load("https://cual-es-mi-ip-publica.com");
    foreach (var Node in doc.DocumentNode.CssSelect(".rojo"))
    {
        var NodoAncho = Node.CssSelect("strong").First();
        texto = NodoAncho.InnerHtml;
    }
    //lee el archivo de ip que ya teniamos 
    StreamReader ipes234 = File.OpenText(path_ip);
    if (ipes234.ToString() != null)
    {
        var jh = "";
        if ((jh = ipes234.ReadLine()) != null)
        {
            aveces = jh;
        }
    }
    ipes234.Close();
    StreamWriter ipes = new(path_ip);
    //si la ip conseguida es diferente a la ip guardada en el archivo txt, la nueva remplaza a la vieja y el booleano se activa para mandar la nueva ip a los suscritos
    if (aveces != texto)
    {
        bandera = true;
        ipes.Write(texto);
        ipes.Close();
    }
    else
    {
        //si no es distinta esta no se guarda y el booleano se desactiva
        bandera = false;
        ipes.Close();
    }
}
//Lee las personas o sus chat Id que poseen permisos elevados para poder usar el bot
void archivitos()
{
    //lee los id chat del documento administradores
    StreamReader Administradores;
    Administradores = File.OpenText(path_admins);
    if (Administradores.ToString() != null)
    {
        var fz = "";
        if ((fz = Administradores.ReadLine()) != null)
        {
            Admins = fz;
        }
        Administradores.Close();
    }
    else
    {
        Administradores.Close();
    }
}
//Guarda las suscripciones en sus debidos archivos para tener ese almacenaje
void suscritos(String datos,string path)
{
    cmp = "";
    //se lee si hay datos en el archivo
    StreamReader lectura21 = File.OpenText(path);
    if (lectura21.ToString() != null)
    {
        cmp = lectura21.ReadToEnd();
        verdad = true;
        lectura21.Close();
    }
    else
    {
        verdad = false;
        lectura21.Close();
    }
    StreamWriter suscritos = new(path);
    //si no se encuentran datos en el archivo pues los nuevos datos remplazan a todo lo que contenga el archivo
    if (verdad == false)
    {
        suscritos.WriteLine(datos);
        suscritos.Close();
    }
    else
    {
        //si si encontro datos estos se toman y se concatenan
        suscritos.WriteLine(cmp + datos);
        suscritos.Close();
    }
}
//Funcion para Inicar un programa .exe o .bat , en este caso para iniciar el servidor de Minecraft
ProcessStartInfo psi = new()
{
    //Funcion para correr un .exe o un .bat en este caso que es el server de  Minecraft
    UseShellExecute = false,
    WorkingDirectory = "D:\\Documentos\\Server nuevo",
    CreateNoWindow = false,
    FileName = "D:\\Documentos\\Server nuevo\\iniciar.bat"
};
//Ciclo infinito de ante busqueda de actualizaciones ( mensajes )
while (true)
{
    //Un contador de tiempo ya que en cada ciclo tarda 1s pues con un contador de 1 en 1 podemos contar el tiempo en segundos
    contadorcito++;
    //si el contador llega a 2 horas se hace una revision automatica
    if (contadorcito % 7200 == 0)
    {
        try
        {
            //se reinicia el contador para que vuelva a comenzar
            contadorcito = 0;
            //si al hacer la revision de aula esta detecta nuevas tareas entra en este caso
            if (revision(path_datosOr, path_datosOrTemp, "0000") == true)
            {
                StreamReader lectura3 = File.OpenText(path_suscritos);
                //se leen todos los usuarios registrados y se les envia las nuevas tareas detectadas
                while (lectura3.ToString() != null)
                {
                    var tem = "";
                    if ((tem = lectura3.ReadLine()) != null)
                    {
                        bot.SendMessage(chatId: tem, text: tareas_diferencias);
                    }
                }
                lectura3.Close();
            }
            if (revision(path_datosOrLDI, path_datosOrTempLDI, "0000") == true)
            {
                StreamReader lectura3 = File.OpenText(path_suscritosLDI);
                //se leen todos los usuarios registrados y se les envia las nuevas tareas detectadas
                while (lectura3.ToString() != null)
                {
                    var tema = "";
                    if ((tema = lectura3.ReadLine()) != null)
                    {
                        bot.SendMessage(chatId: tema, text: tareas_diferencias);
                    }
                }

                lectura3.Close();
            }
            if (revision(path_datosOrICI2, path_datosOrTempICI2, "0000") == true)
            {
                StreamReader lectura3 = File.OpenText(path_suscritos_ici2);
                //se leen todos los usuarios registrados y se les envia las nuevas tareas detectadas
                while (lectura3.ToString() != null)
                {
                    var hg = "";
                    if ((hg = lectura3.ReadLine()) != null)
                    {
                        bot.SendMessage(chatId: hg, text: tareas_diferencias);
                    }
                }

                lectura3.Close();

            }
            //se revisa la direccion ip si esta es diferente a la almacenada, bandera el boleano se vuelve true 
            direccion_ip();
            if (bandera == true)
            {
                //si la ip es distinta vuelve a leer a todos los registrados y les envia la nueva ip
                StreamReader lectura = File.OpenText(path_suscritos);
                if (lectura.ToString() != null)
                {
                    while (lectura.ReadLine() != null)
                    {
                        var gh = "";
                        if ((gh = lectura.ReadLine()) != null)
                        {
                            bot.SendMessage(chatId: gh, text: "La direccion ip del servidor ha cambiado a: " + texto2 + ":25565");
                        }
                    }
                    lectura.Close();
                }
                else
                {

                    lectura.Close();

                }
            }
            else
            {
                Console.WriteLine("No hay cambios de Ip\n");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    //si el contador aun no llega al tiempo de 2 horas se pasa directamente a la comprobacion de mensajes 
    if (updates.Length > 0)
    {
        Console.WriteLine("Hay updates");
        foreach (var update in updates)
        {
            if (update.Message != null)
            {
                //lee erl archivo de admins
                archivitos();
                //si el mensaje recibido viene de un administrador entra a un switch si no pasa a otro switch
                switch (update.Message.Text)
                {
                    //Casos de la carrera de LDI 5to semestre
                    case "/cmp_LDI":
                        bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de comparacion manual\n");
                        revision(path_datosOrLDI, path_datosOrTempLDI,update.Message.Chat.Id.ToString());
                            if (tareas_diferencias.Length > 0)
                            {
                                bot.SendMessage(update.Message.Chat.Id, tareas_diferencias);
                            }
                            else
                            {
                                bot.SendMessage(update.Message.Chat.Id, "No se detecto una nueva tarea\n");
                            }
                        break;
                    case "/aula_LDI":
                        bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");
                        if (LDI != true )
                        {
                            aula(path_datosOrLDI,usuarioLDI,contrasenaLDI,path_fecha_LDI,path_tareas_LDI, update.Message.Chat.Id.ToString());
                            LDI = true;
                            megacontador = supercontador;
                        }
                        StreamReader aiuda2 = File.OpenText(path_datosOrLDI);
                        if (megacontador!=0)
                        { 
                            bot.SendMessage(update.Message.Chat.Id, "=============================\n               Tareas detectadas               \n");

                            for (int i = 0; i < megacontador; i++)
                            {
                                var aj = "";
                                if((aj =aiuda2.ReadLine())!= null){
                                    bot.SendMessage(update.Message.Chat.Id, aj);
                                }
                            }
                            bot.SendMessage(update.Message.Chat.Id, "=============================\n");
                            aiuda2.Close();
                        }
                        else
                        {
                            aiuda2.Close();
                            bot.SendMessage(update.Message.Chat.Id, "=============================\n              Sin tareas detectadas a descansar :3           \n");
                        }
                        break;
                    case "/Sucribirme_LDI":
                        StreamReader lect2 = new(path_suscritosLDI);
                        string susc2 = lect2.ReadToEnd();
                        lect2.Close();
                        StreamReader lau = new(path_suscritos_ici2);
                        string lak = lau.ReadToEnd();
                        lau.Close();
                        StreamReader iz = new(path_suscritos);
                        string h = iz.ReadToEnd();
                        iz.Close();
                        if (susc2.Contains(update.Message.Chat.Id.ToString()))
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a LDI");
                        }else if(lak.Contains(update.Message.Chat.Id.ToString()))
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a ICI2");
                        }else if(h.Contains(update.Message.Chat.Id.ToString()))
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a ICI");
                        }
                        else
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Listo ya estas suscrito a LDI");
                            suscritos(update.Message.Chat.Id.ToString(),path_suscritosLDI);
                        }
                        break;


                        //Casos de la segunda mitad de ICI 3er semestre
                    case "/cmp_ICI2":
                        bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de comparacion manual\n");
                        revision(path_datosOrICI2, path_datosOrTempICI2, update.Message.Chat.Id.ToString());
                            if (tareas_diferencias.Length > 0)
                            {
                                bot.SendMessage(update.Message.Chat.Id, tareas_diferencias);
                            }
                            else
                            {
                                bot.SendMessage(update.Message.Chat.Id, "No se detecto una nueva tarea\n");
                            }
                        
                        break;
                    case "/aula_ICI2":
                        bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");

                        if (altiro != true)
                        {
                            aula(path_datosOrICI2, usuarioICI2, contrasenaICI2, path_fechas_ici2, path_tareas_ici2, update.Message.Chat.Id.ToString());
                            altiro = true;
                            conta = supercontador;
                        }

                        StreamReader aiu = File.OpenText(path_datosOrICI2);
                        if (conta!=0)
                        {
                            bot.SendMessage(update.Message.Chat.Id, "=============================\n               Tareas detectadas               \n");

                            for (int i = 0; i < conta; i++)
                            {
                                var ak = "";
                                if((ak=aiu.ReadLine())!= null)
                                {
                                    bot.SendMessage(update.Message.Chat.Id, ak);
                                }
                            }
                            bot.SendMessage(update.Message.Chat.Id, "=============================\n");
                            aiu.Close();
                        }
                        else
                        {
                            aiu.Close();
                            bot.SendMessage(update.Message.Chat.Id, "=============================\n              Sin tareas detectadas a descansar :3           \n");
                        }

                        break;
                    case "/Suscribirme_ICI2":
                        StreamReader lect = new(path_suscritos_ici2);
                        string sus = lect.ReadToEnd();
                        lect.Close();
                        StreamReader f = new(path_suscritosLDI);
                        string av = f.ReadToEnd();
                        f.Close();
                        StreamReader g = new(path_suscritos);
                        string av2 = g.ReadToEnd();
                        g.Close();
                        if (sus.Contains(update.Message.Chat.Id.ToString()))
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito en ICI2");
                            
                        }else if(av.Contains(update.Message.Chat.Id.ToString()))
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito en LDI");
                        }else if(av2.Contains(update.Message.Chat.Id.ToString()))
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscritos en ICI");
                        }
                        else
                        {
                            
                            bot.SendMessage(update.Message.Chat.Id, "Listo ya estas suscrito en ICI2");
                            suscritos(update.Message.Chat.Id.ToString(), path_suscritos_ici2);
                        }
                        break;
                    //casos de la carrera de ICI 1ra mitad 3er semestre
                    case "/cmp_ICI":
                        bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de comparacion manual\n");
                        revision(path_datosOr, path_datosOrTemp, update.Message.Chat.Id.ToString());
                            if (tareas_diferencias.Length > 0)
                            {
                                bot.SendMessage(update.Message.Chat.Id, tareas_diferencias);
                            }
                            else
                            {
                                bot.SendMessage(update.Message.Chat.Id, "No se detecto una nueva tarea\n");
                            }
                        
                        break;
                    case "/aula_ICI":
                        bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");
                        
                        if (veces != true)
                        {
                            aula(path_datosOr,usuarioICI, contrasenaICI, path_fecha, path_tareas, update.Message.Chat.Id.ToString());
                            veces = true;
                            ultracontador = supercontador;

                        }
                        StreamReader aiuda = File.OpenText(path_datosOr);
                        if (ultracontador!=0)
                        {
                            bot.SendMessage(update.Message.Chat.Id, "=============================\n               Tareas detectadas               \n");

                            for (int i = 0; i < ultracontador; i++)
                            {
                                var ag = "";
                                if((ag = aiuda.ReadLine())!= null)
                                {
                                    bot.SendMessage(update.Message.Chat.Id, text: ag);
                                }
                            }
                            bot.SendMessage(update.Message.Chat.Id, "=============================\n");
                            aiuda.Close();
                        }
                        else
                        {
                            aiuda.Close();
                            bot.SendMessage(update.Message.Chat.Id, "=============================\n              Sin tareas detectadas a descansar :3           \n");
                        }
                        break;
                    case "/Suscribirme_ICI":
                        StreamReader lect34 = new(path_suscritos);
                        string susc = lect34.ReadToEnd();
                        lect34.Close();
                        StreamReader o = new(path_suscritosLDI);
                        string j = o.ReadToEnd();
                        o.Close();
                        StreamReader l = new(path_suscritos_ici2);
                        string n = l.ReadToEnd();
                        l.Close();
                        if (susc.Contains(update.Message.Chat.Id.ToString()))
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a ICI");
                        }
                        else if (j.Contains(update.Message.Chat.Id.ToString()))
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a LDI");
                        }
                        else if (n.Contains(update.Message.Chat.Id.ToString()))
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a ICI2");
                        }
                        else
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Listo ya estas suscrito a ICI");
                            suscritos(update.Message.Chat.Id.ToString(), path_suscritos);
                        }
                        break;
                        
                     //Inicio de comandos para el bot de Minecraft
                    // Da una intro del bot , el que hace
                    case "/start":
                        bot.SendMessage(update.Message.Chat.Id, "Hola yo soy un bot de ayuda con el recordatorio y vista de las tareas de aula virtual\nPor el momento soy una beta pero espero que con tu ayuda\nPueda mejorar para facilitarte el recordatorio de tus tareas\n Y proximamente añadir mas cosas útiles :)");
                        break;
                    //descarga las tareas de aula y las envia
                    
                    //Enciende el servidor de minecraft
                    case "/Encender":
                        revisar_minecraft_activo();
                        if (minecraft == false)  //revisa si el server de minecraft esta encendido , si no lo esta lo enciende
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Encendiendo Guap@!!\n");
                            Process.Start(psi);
                            Thread.Sleep(30000);
                            revisar_minecraft_activo();
                            if (minecraft == true)
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Listo ya esta encendido :3\n");
                            }
                            else
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Algo fallo en la configuracion de nuevos mods OnO\n");
                            }
                        }
                        else
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya se encuentra encendido\n");
                        }
                        break;
                    //Envia la ip del servidor 
                    case "/Ip":
                        direccion_ip();
                        bot.SendMessage(update.Message.Chat.Id, texto + ":25565");
                        break;
                    //comprueba si el servidor se encuentra en ejecucion y con internet
                    case "/Estado":
                        revisar_minecraft_activo();
                        if (minecraft == true)
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Servidor de minecraft Online");
                        }
                        else
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Servidor de minecraft Ofline");
                        }
                        break;
                    //Apaga el servidor de minecraft
                    case "/Apagar":
                        revisar_minecraft_activo();
                        if (minecraft == true)
                        {
                            if (Admins == update.Message.Chat.Id.ToString())
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Apagando servidor");
                                Process[] procs = Process.GetProcessesByName("java"); foreach (Process proc in procs) proc.Kill();
                                Thread.Sleep(5000);
                                revisar_minecraft_activo();
                                if (minecraft == false)
                                {
                                    bot.SendMessage(update.Message.Chat.Id, "Listo ya esta apagado\n");
                                }
                                else
                                {
                                    bot.SendMessage(update.Message.Chat.Id, "No se que ocurrio pero este no se ha apagado OwO\n");
                                }
                            }
                        }
                        else
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya se encuentra apagado");
                        }
                        break;
                    //Informa de todos los comandos accesibles
                    case "/help":
                        bot.SendMessage(update.Message.Chat.Id, "========================\n\n/start - pequeña presentación del bot\r\n/cmp_LDI - compara manualmente las tareas de aula de la carrera de LDI\r\n/aula_LDI - consigue que tareas tienes activas para la carrera de LDI\r\n/Sucribirme_LDI - Si detecta una nueva tarea, te lo hace saber en un mensaje dandote la tarea y fecha de entrega \r\n/cmp_ICI - compara manualmente las tareas de aula de la carrera de ICI\r\n/aula_ICI - consigue que tareas tienes activas para la carrera de ICI\r\n/Suscribirme_ICI - Si detecta una nueva tarea, te lo hace saber en un mensaje dandote la tarea y fecha de entrega\r\n/cmp_ICI2 - compara manualmente las tareas de aula de la carrera de ICI\r\n/aula_ICI2 - consigue que tareas tienes activas para la carrera de ICI\r\n/Suscribirme_ICI2 - Si detecta una nueva tarea, te lo hace saber en un mensaje dandote la tarea y fecha de entrega\n\n========================\n");
                        break;
                    //Si lo ingresado no esta en los comandos le dice que no lo detecto
                    default:
                        bot.SendMessage(update.Message.Chat.Id, "Comando no encontrado, para mas informacion escriba /help");
                        break;
                }
                //fin de los casos de comandos
                //revisa que valla aumentando de 1 en 1 en la lectura de los mensajes (por si recibe varios mensajes)
                updates = bot.GetUpdates(updates.Max(x => x.UpdateId) + 1);
            }
        }
    }
    else
    {
        //se llama asi mismo para continuar el bucle 
        updates = bot.GetUpdates();
    }
}
