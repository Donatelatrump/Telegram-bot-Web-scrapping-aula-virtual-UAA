using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Telegram.BotAPI;
using Telegram.BotAPI.GettingUpdates;
using Telegram.BotAPI.AvailableMethods;
using File = System.IO.File;
using By = OpenQA.Selenium.By;
using PerreVergueBot;



//Declaracion de variables necesarias
int[] contador = new int[11];
string tareas_diferencias = "";
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
char[] numeros = new char[100];
char[] FECHa = new char[3];
char[] fecha = new char[2];
bool veces = false,  LDI = false, altiro = false;
//Declaracion de los objetos del bot
var bot = new BotClient("5681430643:AAGs0-yVSMuFnjix8YCzEqOg29JZlqy1W98");
var updates = bot.GetUpdates();
//Funcion para detectar programas activos en windows -> en este caso detecta minecraft para evitar que el bot se cierre mientras el usuario juega
DateTime thisDay = DateTime.Today;
//Ciclo infinito de ante busqueda de actualizaciones ( mensajes )
contador[3] = 0;
Revision rev = new();
while (true)
{
    //Un contador de tiempo ya que en cada ciclo tarda 1s pues con un contador de 1 en 1 podemos contar el tiempo en segundos
    contador[3]++;
    //si el contador llega a 2 horas se hace una revision automatica

    if (contador[3] % 7200 == 0)
    {
        _ = Task.Run(() =>
        {
            try
            {
                //se reinicia el contador para que vuelva a comenzar
                contador[3] = 0;
                //si al hacer la revision de aula esta detecta nuevas tareas entra en este caso
                if (rev.Revision1(path_datosOr, path_datosOrTemp, "000000",bot) == true)
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
                if (rev.Revision1(path_datosOrLDI, path_datosOrTempLDI, "000000",bot) == true)
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
                if (rev.Revision1(path_datosOrICI2, path_datosOrTempICI2, "000000",bot) == true)
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
    LecturaAula lec = new();
    //si el contador aun no llega al tiempo de 2 horas se pasa directamente a la comprobacion de mensajes 
    if (updates.Length > 0)
    {
        Console.WriteLine("Hay updates");
        foreach (var update in updates)
        {
            if (update.Message != null)
            {
                //lee erl archivo de admins
                rev.Archivitos();
                //si el mensaje recibido viene de un administrador entra a un switch si no pasa a otro switch
                _ = update.Message.Text switch
                {
                    //Casos de la carrera de LDI 5to semestre
                    "/cmp_LDI" => Task.Run(() =>
                                            {
                                                bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de comparacion manual\n");
                                                rev .Revision1(path_datosOrLDI, path_datosOrTempLDI, update.Message.Chat.Id.ToString(),bot);
                                                if (tareas_diferencias.Length > 0)
                                                {
                                                    bot.SendMessage(update.Message.Chat.Id, tareas_diferencias);
                                                }
                                                else
                                                {
                                                    bot.SendMessage(update.Message.Chat.Id, "No se detecto una nueva tarea\n");
                                                }
                                            }),
                    "/aula_LDI" => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");
                            if (LDI != true)
                            {
                                LecturaAula.Aula(path_datosOrLDI, "al263887", "Wera060102", path_fecha_LDI, path_tareas_LDI, update.Message.Chat.Id.ToString(),bot);
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
                        }),
                    "/Sucribirme_LDI" => Task.Run(() =>
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
                                Revision.Suscritos(update.Message.Chat.Id.ToString(), path_suscritosLDI);
                            }
                        }),
                    //Casos de la segunda mitad de ICI 3er semestre
                    "/cmp_ICI2" => Task.Run(() =>
                                            {
                                                bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de comparacion manual\n");
                                                rev.Revision1(path_datosOrICI2, path_datosOrTempICI2, update.Message.Chat.Id.ToString(),bot);
                                                if (tareas_diferencias.Length > 0)
                                                {
                                                    bot.SendMessage(update.Message.Chat.Id, tareas_diferencias);
                                                }
                                                else
                                                {
                                                    bot.SendMessage(update.Message.Chat.Id, "No se detecto una nueva tarea\n");
                                                }
                                            }),
                    "/aula_ICI2" => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");

                            if (altiro != true)
                            {
                                LecturaAula.Aula(path_datosOrICI2, "al261731", "SPjl3490", path_fechas_ici2, path_tareas_ici2, update.Message.Chat.Id.ToString(), bot);
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
                        }),
                    "/Suscribirme_ICI2" => Task.Run(() =>
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
                                Revision.Suscritos(update.Message.Chat.Id.ToString(), path_suscritos_ici2);
                            }
                        }),
                    //casos de la carrera de ICI 1ra mitad 3er semestre
                    "/cmp_ICI" => Task.Run(() =>
                                            {
                                                bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de comparacion manual\n");
                                                rev.Revision1(path_datosOr, path_datosOrTemp, update.Message.Chat.Id.ToString(),bot);
                                                if (tareas_diferencias.Length > 0)
                                                {
                                                    bot.SendMessage(update.Message.Chat.Id, tareas_diferencias);
                                                }
                                                else
                                                {
                                                    bot.SendMessage(update.Message.Chat.Id, "No se detecto una nueva tarea\n");
                                                }
                                            }),
                    "/aula_ICI" => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");

                            if (veces != true)
                            {
                                LecturaAula.Aula(path_datosOr, "al283189", "Sayulita0506", path_fecha, path_tareas, update.Message.Chat.Id.ToString(), bot);
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
                        }),
                    "/Suscribirme_ICI" => Task.Run(() =>
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
                                Revision.Suscritos(update.Message.Chat.Id.ToString(), path_suscritos);
                            }
                        }),
                    "/start" => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Hola yo soy un bot de ayuda con el recordatorio y vista de las tareas de aula virtual\nPor el momento soy una beta pero espero que con tu ayuda\nPueda mejorar para facilitarte el recordatorio de tus tareas\n Y proximamente añadir mas cosas útiles :)");
                            bot.SendAnimation(update.Message.Chat.Id, "http://68.media.tumblr.com/0c6c24139702399121af533ab6011237/tumblr_oqcj9ycnPO1w46s3lo1_540.gif");

                        }),
                    //Informa de todos los comandos accesibles
                    "/help" => Task.Run(() =>
                                            {
                                                bot.SendMessage(update.Message.Chat.Id, "========================\n\n/start - pequeña presentación del bot\r\n/cmp_LDI - compara manualmente las tareas de aula de la carrera de LDI\r\n/aula_LDI - consigue que tareas tienes activas para la carrera de LDI\r\n/Sucribirme_LDI - Si detecta una nueva tarea, te lo hace saber en un mensaje dandote la tarea y fecha de entrega \r\n/cmp_ICI - compara manualmente las tareas de aula de la carrera de ICI\r\n/aula_ICI - consigue que tareas tienes activas para la carrera de ICI\r\n/Suscribirme_ICI - Si detecta una nueva tarea, te lo hace saber en un mensaje dandote la tarea y fecha de entrega\r\n/cmp_ICI2 - compara manualmente las tareas de aula de la carrera de ICI\r\n/aula_ICI2 - consigue que tareas tienes activas para la carrera de ICI\r\n/Suscribirme_ICI2 - Si detecta una nueva tarea, te lo hace saber en un mensaje dandote la tarea y fecha de entrega\n\n========================\n");
                                                bot.SendAnimation(update.Message.Chat.Id, "https://media.tenor.com/oim29qOLORkAAAAC/konosuba-dance.gif");
                                            }),
                    //Si lo ingresado no esta en los comandos le dice que no lo detecto
                    _ => Task.Run(() =>
                                            {
                                                bot.SendMessage(update.Message.Chat.Id, "Comando no encontrado, para mas informacion escriba /help");
                                            }),
                };
                //fin de los casos de comandos
                //revisa que valla aumentando de 1 en 1 en la lectura de los mensajes (por si recibe varios mensajes)
                try
                {
                    updates = bot.GetUpdates(updates.Max(x => x.UpdateId) + 1);
                }catch(Exception e)
                {
                      Console.WriteLine(e.Message);
                }
               
            }
        }
    }
    else
    {
        //se llama asi mismo para continuar el bucle 
        try
        {
            updates = bot.GetUpdates();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }
}

