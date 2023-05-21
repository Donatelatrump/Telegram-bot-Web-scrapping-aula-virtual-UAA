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
using OpenQA.Selenium.DevTools.V113.Debugger;
using CsQuery.ExtensionMethods.Internal;



//Declaracion de variables necesarias
int[] contador = new int[11];
string lineas="", aveces = "", cmp = "", tareas_diferencias = "",  fechas_aula2 = "", Admins = "", texto22 = "", fecha_actual = "";
//Declaracion de las rutas de los archivos que almacenan la informacion (debo encontrar una mejor manera de guardarlo, una manera mas eficiente)
 string direct = Directory.GetCurrentDirectory();
 string path_tareas = direct + "\\Archivos\\Eventos.txt";
 string path_ip = direct + "\\Archivos\\ips2.txt";
 string path_fecha = direct + "\\Archivos\\Fechas.txt";
 string path_suscritos = direct + "\\Archivos\\Suscritos2.txt";
 string path_admins = direct + "\\Archivos\\Admins.txt";
 string path_datosOr = direct + "\\Archivos\\DatosOr.txt";
 string path_datosOrLDI = direct + "\\Archivos\\DatosOrLDI.txt";
 string path_datosOrTemp = direct + "\\Archivos\\DatosOrTemp.txt";
 string path_suscritosLDI = direct + "\\Archivos\\SuscritosLDI.txt";
 string path_datosOrTempLDI = direct + "\\Archivos\\DatosOrTempLDI.txt";
 string path_fecha_LDI = direct + "\\Archivos\\Fechas_LDI.txt";
 string path_tareas_LDI = direct + "\\Archivos\\Eventos_LDI.txt";
 string path_tareas_ici2 = direct + "\\Archivos\\EventosICI2.txt";
 string path_fechas_ici2 = direct + "\\Archivos\\FechasICI2.txt";
 string path_suscritos_ici2 = direct + "\\Archivos\\SuscritosICI2.txt";
 string path_datosOrICI2 = direct + "\\Archivos\\DatosOrICI2.txt";
 string path_datosOrTempICI2 = direct + "\\Archivos\\DatosOrTempICI2.txt";
//Variables de acceso a la pagina de la Uni
string usuarioICI = "al283189", usuarioLDI = "al263887", usuarioICI2 = "al261731";
string contrasenaICI = "Sayulita0506", contrasenaLDI = "Wera060102", contrasenaICI2 = "SPjl3490";
char[] numeros = new char[100];
char[] FECHa = new char[3];
char[] fecha = new char[2];
bool veces = false, verdad = false,LDI = false, altiro =false;
//Declaracion de los objetos del bot
var bot = new BotClient("5681430643:AAGs0-yVSMuFnjix8YCzEqOg29JZlqy1W98");
var updates = bot.GetUpdates();
//Funcion para detectar programas activos en windows -> en este caso detecta minecraft para evitar que el bot se cierre mientras el usuario juega
DateTime thisDay = DateTime.Today;
//Funcion de web scrapping para sacar las tareas y fechas de la uni dependiendo de que carrera sea
string aula(string path, string Usuario, string Password2, string fecha1,string tarea1,string update)
{
    contador[5] = 0;
    contador[6] = 0;
    contador[10] = 0;
    texto22 = "";
    string dia="";
    //Tomar solo los dias de la fecha actual 
    fecha[0] = thisDay.ToString()[0];
    fecha[1] = thisDay.ToString()[1];
    fecha_actual = new string(fecha);
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
            bot.SendAnimation(update, "https://i.pinimg.com/originals/d1/d6/c0/d1d6c0fe9c91839b97e361387b505b97.gif");
        }
        catch(Exception ar)
        {
            Console.WriteLine("No pudimos conectarnos con el remitente\n"+ar);
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
            if (item2.Text.ToString().Length != 0)
            {
                if (contador[10] == 40) //Condicional para que no de vueltas de mas inecesarias
                {
                    break; //Para romper el foreach
                }
                if (!item2.Text.Contains("Sin eventos") && !item2.Text.Contains("Omitir"))
                {
                    if (new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' }.Any(x => item2.Text.Contains(x)))
                    {

                        string[] partes = item2.Text.Split(',');


                        if (partes.Length >= 3)
                        {
                            string fecha = partes[2].Trim();


                            string[] fechaPartes = fecha.Split(' ');
                            if (fechaPartes.Length >= 2)
                            {
                                dia = fechaPartes[^2];
                            }
                        } 
                        if (Int32.Parse(dia) >= Int32.Parse(fecha_actual))
                        {
                            fechas_aula2 += item2.Text+ "\n";
                        }
                        else
                        {
                            FECHa[2] = item2.Text[0];
                            contador[5]+= Int32.Parse(FECHa[2].ToString());
                        }
                    }

                }
                contador[10]+=1;
            }
        }
        contador[10] = 0;
        
        foreach (var item in numero_de_tareas)
        {

                if (!item.Text.Contains("Ocultar"))
                {
                contador[6] += 1;
                //Decirle que mientras el contador de las tareas leidas sea mayor al contador de fechas ignoradas debe seguir leyendo tareas
                if (contador[6] > contador[5])
                    {
                    texto22 += item.Text.ToString() + "\n";
                    }
            }
        }
        //Para el texto de las tareas
        try
        {
            StreamWriter dayo = new(fecha1);
            dayo.Write(fechas_aula2);
            dayo.Close();
            // Eventos
            fechas_aula2 = "";
            StreamWriter eventos = new(tarea1);
            eventos.WriteLine(texto22);
            eventos.Close();
        }
        catch (Exception Noabrio)
        {
            Console.WriteLine("No se pudo abrir o escribir en uno o ambos archivos de Fechas o Eventos, el código de error es:\n" + Noabrio);
        }

        // Contador de líneas de eventos
        StreamReader Primer_evento = File.OpenText(fecha1);
        contador[2] = 0;
        string temporal = Primer_evento.ReadLine(); // Leer la primera línea
        while (temporal != null)
        {
            if (!string.IsNullOrEmpty(temporal))
            {
                numeros[contador[2]] = temporal[0];
                contador[2] += 1;
            }
            temporal = Primer_evento.ReadLine(); // Leer la siguiente línea
        }
        Primer_evento.Close();


        StreamReader lolcito = File.OpenText(tarea1);
        Primer_evento = File.OpenText(fecha1);
        StreamWriter aiuda = new(path);
        for (int i = 0; i < contador[2]; i++)
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
        }
        //se cierran todos los archivos
        aiuda.Close();
        Primer_evento.Close();
        lolcito.Close();
        //Se cierra la sesion de aula
        var a = driver.FindElement(By.XPath("//*[@id=\"action-menu-0-menu\"]/a[6]"));
        var e = a.GetAttribute("href");
        driver.Navigate().GoToUrl(e);
        driver.Quit();
        //contamos cuantas lineas tiene el documento final ordenado y lo guardamos en una variable 
        StreamReader ola = File.OpenText(path);
        contador[4] = 0;
        while (ola.ReadLine() != null)
        {
            contador[4] += 1;
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
            Console.WriteLine("No se pudo encontrar al destinatario\n"+al);
        }
        return "a";
    }
}
//Funcion para revisar si las tareas que tenemos son las mismas que en la pagina de la uni(basicamente funciona como la funcion de web scrapping pero aqui se revisa 
bool revision(string lolo, string chavo, string update)
{
    tareas_diferencias = "";
    contador[1] = File.ReadAllLines(lolo).Length;
    contador[0] = File.ReadAllLines(chavo).Length;

    // Llamada a la función aula según el caso de revisión
    if (lolo == path_datosOr)
    {
        aula(path_datosOrTemp, usuarioICI, contrasenaICI, path_fecha, path_tareas, update);
    }
    else if (lolo == path_datosOrLDI)
    {
        aula(path_datosOrTempLDI, usuarioLDI, contrasenaLDI, path_fecha_LDI, path_tareas_LDI, update);
    }
    else if (lolo == path_datosOrICI2)
    {
        aula(path_datosOrTempICI2, usuarioICI2, contrasenaICI2, path_fechas_ici2, path_tareas_ici2, update);
    }

    if (contador[0] > contador[1])
    {
        using StreamReader aioch = File.OpenText(chavo);
        for (int j = 0; j < contador[0]; j++)
        {
            var gf = aioch.ReadLine();
            if (j >= contador[1] && gf != null)
            {
                tareas_diferencias += gf;
            }
        }
    }

    return !string.IsNullOrEmpty(tareas_diferencias);
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
//Ciclo infinito de ante busqueda de actualizaciones ( mensajes )
contador[3] = 0;
while (true)
{
    //Un contador de tiempo ya que en cada ciclo tarda 1s pues con un contador de 1 en 1 podemos contar el tiempo en segundos
    contador[3]++;
    //si el contador llega a 2 horas se hace una revision automatica

    if (contador[3] % 7200 == 0)
    {
        Task.Run(() =>
        {
            try
            {
                //se reinicia el contador para que vuelva a comenzar
                contador[3] = 0;
                //si al hacer la revision de aula esta detecta nuevas tareas entra en este caso
                if (revision(path_datosOr, path_datosOrTemp, "000000") == true)
                {
                    StreamReader lectura3 = File.OpenText(path_suscritos);
                    //se leen todos los usuarios registrados y se les envia las nuevas tareas detectadas
                    while (lectura3.ToString() != null)
                    {
                        var tem = "";//al hacer esto quitamos los avisos de posible null
                        if ((tem = lectura3.ReadLine()) != null)
                        {
                            bot.SendMessage(chatId: tem, text: tareas_diferencias);
                        }
                    }
                    lectura3.Close();
                }
                if (revision(path_datosOrLDI, path_datosOrTempLDI, "000000") == true)
                {
                    StreamReader lectura3 = File.OpenText(path_suscritosLDI);
                    //se leen todos los usuarios registrados y se les envia las nuevas tareas detectadas
                    while (lectura3.ToString() != null)
                    {
                        var tema = "";//al hacer esto quitamos los avisos de posible null
                        if ((tema = lectura3.ReadLine()) != null)
                        {
                            bot.SendMessage(chatId: tema, text: tareas_diferencias);
                        }
                    }
                    lectura3.Close();
                }
                if (revision(path_datosOrICI2, path_datosOrTempICI2, "000000") == true)
                {
                    StreamReader lectura3 = File.OpenText(path_suscritos_ici2);
                    //se leen todos los usuarios registrados y se les envia las nuevas tareas detectadas
                    while (lectura3.ToString() != null)
                    {
                        var hg = "";//al hacer esto quitamos los avisos de posible null
                        if ((hg = lectura3.ReadLine()) != null)
                        {
                            bot.SendMessage(chatId: hg, text: tareas_diferencias);
                        }
                    }
                    lectura3.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fallo la revision automatica, codigo de error:\n" + e);
            }
        });
      
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
                        Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de comparacion manual\n");
                            revision(path_datosOrLDI, path_datosOrTempLDI, update.Message.Chat.Id.ToString());
                            if (tareas_diferencias.Length > 0)
                            {
                                bot.SendMessage(update.Message.Chat.Id, tareas_diferencias);
                            }
                            else
                            {
                                bot.SendMessage(update.Message.Chat.Id, "No se detecto una nueva tarea\n");
                            }
                        });
                        break;
                    case "/aula_LDI":
                        Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");
                            if (LDI != true)
                            {
                                aula(path_datosOrLDI, usuarioLDI, contrasenaLDI, path_fecha_LDI, path_tareas_LDI, update.Message.Chat.Id.ToString());
                                LDI = true;
                                contador[8] = contador[4];
                            }
                            StreamReader aiuda2 = File.OpenText(path_datosOrLDI);
                            if (contador[8] != 0)
                            {
                                bot.SendMessage(update.Message.Chat.Id, "=============================\n               Tareas detectadas               \n");

                                for (int i = 0; i < contador[8]; i++)
                                {
                                    var aj = "";
                                    if ((aj = aiuda2.ReadLine()) != null)
                                    {
                                        bot.SendMessage(update.Message.Chat.Id, aj);
                                    }
                                }
                                bot.SendMessage(update.Message.Chat.Id, "=============================\n");
                                bot.SendAnimation(update.Message.Chat.Id, "https://thumbs.gfycat.com/MeaslyJaggedBrontosaurus-size_restricted.gif");
                                aiuda2.Close();
                            }
                            else
                            {
                                aiuda2.Close();
                                bot.SendMessage(update.Message.Chat.Id, "=============================\n              Sin tareas detectadas a descansar :3\n");
                                bot.SendAnimation(update.Message.Chat.Id, "https://i.pinimg.com/originals/e0/03/69/e00369b162f3b91e05b2198efcf8f73f.gif");
                            }
                        });
                       
                        break;
                    case "/Sucribirme_LDI":
                        Task.Run(() =>
                        {
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
                            }
                            else if (lak.Contains(update.Message.Chat.Id.ToString()))
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a ICI2");
                            }
                            else if (h.Contains(update.Message.Chat.Id.ToString()))
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a ICI");
                            }
                            else
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Listo ya estas suscrito a LDI");
                                suscritos(update.Message.Chat.Id.ToString(), path_suscritosLDI);
                            }
                        });
                        break;
                        //Casos de la segunda mitad de ICI 3er semestre
                    case "/cmp_ICI2":
                        Task.Run(() =>
                        {
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
                        });
                        break;
                    case "/aula_ICI2":
                        Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");

                            if (altiro != true)
                            {
                                aula(path_datosOrICI2, usuarioICI2, contrasenaICI2, path_fechas_ici2, path_tareas_ici2, update.Message.Chat.Id.ToString());
                                altiro = true;
                                contador[9] = contador[4];
                            }

                            StreamReader aiu = File.OpenText(path_datosOrICI2);
                            if (contador[9] != 0)
                            {
                                bot.SendMessage(update.Message.Chat.Id, "=============================\n               Tareas detectadas               \n");

                                for (int i = 0; i < contador[9]; i++)
                                {
                                    var ak = "";
                                    if ((ak = aiu.ReadLine()) != null)
                                    {
                                        bot.SendMessage(update.Message.Chat.Id, ak);
                                    }
                                }
                                bot.SendMessage(update.Message.Chat.Id, "=============================\n");
                                bot.SendAnimation(update.Message.Chat.Id, "https://thumbs.gfycat.com/MeaslyJaggedBrontosaurus-size_restricted.gif");
                                aiu.Close();
                            }
                            else
                            {
                                aiu.Close();
                                bot.SendMessage(update.Message.Chat.Id, "=============================\n              Sin tareas detectadas a descansar :3\n");
                                bot.SendAnimation(update.Message.Chat.Id, "https://i.pinimg.com/originals/ca/39/9e/ca399e41629b0bc8d91f8d6507b15707.gif");
                            }
                        });
                        break;
                    case "/Suscribirme_ICI2":
                        Task.Run(() =>
                        {
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

                            }
                            else if (av.Contains(update.Message.Chat.Id.ToString()))
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito en LDI");
                            }
                            else if (av2.Contains(update.Message.Chat.Id.ToString()))
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscritos en ICI");
                            }
                            else
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Listo ya estas suscrito en ICI2");
                                suscritos(update.Message.Chat.Id.ToString(), path_suscritos_ici2);
                            }
                        });
                        break;
                    //casos de la carrera de ICI 1ra mitad 3er semestre
                    case "/cmp_ICI":
                        Task.Run(() =>
                        {
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
                        });
                          break;
                    case "/aula_ICI":
                        Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");

                            if (veces != true)
                            {
                                aula(path_datosOr, usuarioICI, contrasenaICI, path_fecha, path_tareas, update.Message.Chat.Id.ToString());
                                veces = true;
                                contador[7] = contador[4];
                            }
                            StreamReader aiuda = File.OpenText(path_datosOr);
                            if (contador[7] != 0)
                            {
                                bot.SendMessage(update.Message.Chat.Id, "=============================\n               Tareas detectadas               \n");

                                for (int i = 0; i < contador[7]; i++)
                                {
                                    var ag = "";
                                    if ((ag = aiuda.ReadLine()) != null)
                                    {
                                        bot.SendMessage(update.Message.Chat.Id, text: ag);
                                    }
                                }
                                bot.SendMessage(update.Message.Chat.Id, "=============================\n");
                                bot.SendAnimation(update.Message.Chat.Id, "https://thumbs.gfycat.com/MeaslyJaggedBrontosaurus-size_restricted.gif");
                                aiuda.Close();
                            }
                            else
                            {
                                aiuda.Close();
                                bot.SendMessage(update.Message.Chat.Id, "===================================\n              Sin tareas detectadas a descansar :3\n");
                                bot.SendAnimation(update.Message.Chat.Id, "https://i.pinimg.com/originals/b8/47/7b/b8477b8f1cf8fcb00e37fbec31c2a22e.gif");
                            }
                        });
                        break;
                    case "/Suscribirme_ICI":
                        Task.Run(() =>
                        {
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
                        });
                        break;
                    case "/start":
                        Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Hola yo soy un bot de ayuda con el recordatorio y vista de las tareas de aula virtual\nPor el momento soy una beta pero espero que con tu ayuda\nPueda mejorar para facilitarte el recordatorio de tus tareas\n Y proximamente añadir mas cosas útiles :)");
                            bot.SendAnimation(update.Message.Chat.Id, "http://68.media.tumblr.com/0c6c24139702399121af533ab6011237/tumblr_oqcj9ycnPO1w46s3lo1_540.gif");

                        });
                     
                        break;
                    //Informa de todos los comandos accesibles
                    case "/help":
                        Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "========================\n\n/start - pequeña presentación del bot\r\n/cmp_LDI - compara manualmente las tareas de aula de la carrera de LDI\r\n/aula_LDI - consigue que tareas tienes activas para la carrera de LDI\r\n/Sucribirme_LDI - Si detecta una nueva tarea, te lo hace saber en un mensaje dandote la tarea y fecha de entrega \r\n/cmp_ICI - compara manualmente las tareas de aula de la carrera de ICI\r\n/aula_ICI - consigue que tareas tienes activas para la carrera de ICI\r\n/Suscribirme_ICI - Si detecta una nueva tarea, te lo hace saber en un mensaje dandote la tarea y fecha de entrega\r\n/cmp_ICI2 - compara manualmente las tareas de aula de la carrera de ICI\r\n/aula_ICI2 - consigue que tareas tienes activas para la carrera de ICI\r\n/Suscribirme_ICI2 - Si detecta una nueva tarea, te lo hace saber en un mensaje dandote la tarea y fecha de entrega\n\n========================\n");
                            bot.SendAnimation(update.Message.Chat.Id, "https://media.tenor.com/oim29qOLORkAAAAC/konosuba-dance.gif");
                        });
                     
                        break;
                    //Si lo ingresado no esta en los comandos le dice que no lo detecto
                    default:
                        Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Comando no encontrado, para mas informacion escriba /help");
                        });
                      
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
