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
int auxiliar = 0;
bool[] Coordinacion = new bool[100];
Revision rev = new();
while (true)
{
    //Un contador de tiempo ya que en cada ciclo tarda 1s pues con un contador de 1 en 1 podemos contar el tiempo en segundos
    auxiliar++;
 
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
                Revision.Archivitos();
                if (auxiliar % 7200 == 0)
                {
                    Revision_2horas a = new();
                    _ = Task.Run(() =>
                    {
                        Revision_2horas.Revision_2(update.Message.Chat.Id.ToString(), bot);
                    });
                }
                //de esta manera puedo recibir respuestas a preguntas simples
                if (Coordinacion[0])
                {
                    switch (update.Message.Text)
                    {
                        case "rojo":
                            bot.SendMessage(update.Message.Chat.Id, "excelente te gusta el rojo");
                            Coordinacion[0] = false;
                            break;
                        case "azul":
                            bot.SendMessage(update.Message.Chat.Id, "excelente te gusta el azul");
                            Coordinacion[0] = false;
                            break;
                    }


                }else if (Coordinacion[1])
                {
                    switch (update.Message.Text)
                    {
                        case "/ICI":
                            bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");

                            if (altiro != true)
                            {
                                lec.Aula(Rutas.path_datosOrICI2, "al283189", "Sayulita0506", Rutas.path_fechas_ici2, Rutas.path_tareas_ici2, update.Message.Chat.Id.ToString(), bot);
                                altiro = true;

                            }

                            StreamReader aiu = File.OpenText(Rutas.path_datosOrICI2);
                            if (aiu != null)
                            {
                                bot.SendMessage(update.Message.Chat.Id, "=============================\n               Tareas detectadas               \n");

                                for (int i = 0; i < LecturaAula.tareas_detectadas; i++) {

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

                            Coordinacion[1] = false;
                            LecturaAula.tareas_detectadas = 0;
                            break;
                        case "/LDI":
                            bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");
                            if (LDI != true)
                            {
                                lec.Aula(Rutas.path_datosOrLDI, "al263887", "Wera060102", Rutas.path_fecha_LDI, Rutas.path_tareas_LDI, update.Message.Chat.Id.ToString(), bot);
                                LDI = true;

                            }
                            StreamReader aiuda2 = File.OpenText(Rutas.path_datosOrLDI);

                            bot.SendMessage(update.Message.Chat.Id, "=============================\n               Tareas detectadas               \n");
                            if (aiuda2 != null) { 
                            for (int i = 0; i < LecturaAula.tareas_detectadas; i++)
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
                            Coordinacion[1] = false;
                            break;
                    }
                }else if (Coordinacion[2])
                {
                    switch (update.Message.Text)
                    {
                        case "/ICI":
                            StreamReader lect = new(Rutas.path_suscritos_ici2);
                            string sus = lect.ReadToEnd();
                            lect.Close();
                            StreamReader f = new(Rutas.path_suscritosLDI);
                            string av = f.ReadToEnd();
                            f.Close();
                            StreamReader g = new(Rutas.path_suscritos);
                            string av2 = g.ReadToEnd();
                            g.Close();
                            if (sus.Contains(update.Message.Chat.Id.ToString()))
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito en ICI");

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
                                Revision.Suscritos(update.Message.Chat.Id.ToString(), Rutas.path_suscritos_ici2);
                            }
                            Coordinacion[2] = false;
                            break;
                        case "/LDI":
                            StreamReader lect2 = new(Rutas.path_suscritosLDI);
                            string susc2 = lect2.ReadToEnd();
                            lect2.Close();
                            StreamReader lau = new(Rutas.path_suscritos_ici2);
                            string lak = lau.ReadToEnd();
                            lau.Close();
                            StreamReader iz = new(Rutas.path_suscritos);
                            string h = iz.ReadToEnd();
                            iz.Close();
                            if (susc2.Contains(update.Message.Chat.Id.ToString()))
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a LDI");
                            }
                            else if (lak.Contains(update.Message.Chat.Id.ToString()))
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a ICI");
                            }
                            else if (h.Contains(update.Message.Chat.Id.ToString()))
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Ya estabas suscrito a ICI");
                            }
                            else
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Listo ya estas suscrito a LDI");
                                Revision.Suscritos(update.Message.Chat.Id.ToString(), Rutas.path_suscritosLDI);
                            }
                            Coordinacion[2] = false;
                            break;
                    }
                }
                else{
                    _ = update.Message.Text switch
                    {
                        "/start" => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Hola yo soy un bot de ayuda con el recordatorio y vista de las tareas de aula virtual\nPor el momento soy una beta pero espero que con tu ayuda\nPueda mejorar para facilitarte el recordatorio de tus tareas\n Y proximamente añadir mas cosas útiles :)");
                            bot.SendAnimation(update.Message.Chat.Id, "http://68.media.tumblr.com/0c6c24139702399121af533ab6011237/tumblr_oqcj9ycnPO1w46s3lo1_540.gif");

                        }),
                        //Informa de todos los comandos accesibles
                        "/help" => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "========================\n\n/start - pequeña presentación del bot\r\n/Suscribirme - Te suscribe a recibir actualizaciones cada vez que suban una nueva tarea\n/Revisar_Tareas - Revisa manualmente las tareas activas de tu carrera");
                            bot.SendAnimation(update.Message.Chat.Id, "https://media.tenor.com/oim29qOLORkAAAAC/konosuba-dance.gif");
                        }),
                        "/Preguntas" => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Que prefieres, rojo u azul?");
                            Coordinacion[0] = true;
                        }),
                        "/Revisar_Tareas" => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Porfavor dime que carera quieres revisar\n\n\t\t/ICI\n\n\t\t/LDI");
                            Coordinacion[1] = true;
                            
                        }),
                        "/Suscribirme" => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Porfavor dime que carera quieres suscribirte\n\n\t\t/ICI\n\n\t\t/LDI");
                            Coordinacion[2] = true;

                        }),
                        //Si lo ingresado no esta en los comandos le dice que no lo detecto
                        _ => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Comando no encontrado, para mas informacion escriba /help");
                        }),
                    }; ; ;
                }








                //si el mensaje recibido viene de un administrador entra a un switch si no pasa a otro switch
               
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

