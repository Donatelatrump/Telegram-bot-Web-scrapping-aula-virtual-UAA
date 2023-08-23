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
           
                /*if (auxiliar % 7200 == 0)
                {
                    Revision_2horas a = new();
                    _ = Task.Run(() =>
                    {
                        Revision_2horas.Revision_2(update.Message.Chat.Id.ToString(), bot);
                    });
                }*/
                //de esta manera puedo recibir respuestas a preguntas simples
                if (Coordinacion[0])
                {
                    Coordinacion[0] = false;

                }
                else if (Coordinacion[1])
                {
                    string[] info = update.Message.Text.Split(',');
                    if (info.Count() >= 1)
                    {

                        JsonGeneral.AgregarNuevoCliente(Rutas.Jzon, update.Message.Chat.Id.ToString(), info[0], info[1]);
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");
                            lec.Aula(JsonGeneral.LeerUsuario(Rutas.Jzon, update.Message.Chat.Id.ToString()), JsonGeneral.LeerContrasena(Rutas.Jzon, update.Message.Chat.Id.ToString()), update.Message.Chat.Id.ToString(), bot);
                            string[] tareas = JsonGeneral.LeerTareasOrdenadas(Rutas.Jzon, update.Message.Chat.Id.ToString());
                            if (tareas != null)
                            {
                                bot.SendMessage(update.Message.Chat.Id, "=============================\n               Tareas detectadas               \n");

                                for (int i = 0; i < LecturaAula.tareas_detectadas; i++)
                                {

                                    if (tareas[i] != null)
                                    {
                                        bot.SendMessage(update.Message.Chat.Id, tareas[i]);
                                    }
                                }


                                bot.SendMessage(update.Message.Chat.Id, "=============================\n");
                                bot.SendAnimation(update.Message.Chat.Id, "https://thumbs.gfycat.com/MeaslyJaggedBrontosaurus-size_restricted.gif");

                            }
                            else
                            {

                                bot.SendMessage(update.Message.Chat.Id, "=============================\n              Sin tareas detectadas a descansar :3\n");
                                bot.SendAnimation(update.Message.Chat.Id, "https://i.pinimg.com/originals/ca/39/9e/ca399e41629b0bc8d91f8d6507b15707.gif");
                            }
                            JsonGeneral.EliminarCliente(Rutas.Jzon, update.Message.Chat.Id.ToString());
                        }

                    }
                    else
                    {
                        bot.SendMessage(update.Message.Chat.Id, "Usuario y contraseña en formato invalido");
                    }
                    Coordinacion[1] = false;
                }

                else if (Coordinacion[2])
                {
                    string[] info = update.Message.Text.Split(',');
                    if (info.Count() >= 1)
                    {
                        var e = JsonGeneral.LeerCodigosClientes(Rutas.Jzon);
                        if (e.Contains(update.Message.Chat.Id.ToString()))
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Ya estas suscrito!!");

                        }
                        else
                        {
                            JsonGeneral.AgregarNuevoCliente(Rutas.Jzon, update.Message.Chat.Id.ToString(), info[0], info[1]);
                            bot.SendMessage(update.Message.Chat.Id, "Ya estas suscrito!!");

                        }
                    }
                    else
                    {
                        bot.SendMessage(update.Message.Chat.Id, "Usuario y contraseña en formato invalido");

                    }
                    Coordinacion[2] = false;
                }
                else
                {
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

                            bool existe = JsonGeneral.ExisteCodigoCliente(Rutas.Jzon, update.Message.Chat.Id.ToString());
                            if (existe)
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Esta en proceso de la captura de los datos!\n");
                                lec.Aula(JsonGeneral.LeerUsuario(Rutas.Jzon, update.Message.Chat.Id.ToString()), JsonGeneral.LeerContrasena(Rutas.Jzon, update.Message.Chat.Id.ToString()), update.Message.Chat.Id.ToString(), bot);
                                string[] tareas = JsonGeneral.LeerTareasOrdenadas(Rutas.Jzon, update.Message.Chat.Id.ToString());
                                if (tareas != null)
                                {
                                    bot.SendMessage(update.Message.Chat.Id, "=============================\n               Tareas detectadas               \n");

                                    for (int i = 0; i < LecturaAula.tareas_detectadas; i++)
                                    {

                                        if (tareas[i] != null)
                                        {
                                            bot.SendMessage(update.Message.Chat.Id, tareas[i]);
                                        }
                                    }


                                    bot.SendMessage(update.Message.Chat.Id, "=============================\n");
                                    bot.SendAnimation(update.Message.Chat.Id, "https://thumbs.gfycat.com/MeaslyJaggedBrontosaurus-size_restricted.gif");

                                }
                                else
                                {

                                    bot.SendMessage(update.Message.Chat.Id, "=============================\n              Sin tareas detectadas a descansar :3\n");
                                    bot.SendAnimation(update.Message.Chat.Id, "https://i.pinimg.com/originals/ca/39/9e/ca399e41629b0bc8d91f8d6507b15707.gif");
                                }
                                JsonGeneral.Reseteo(Rutas.Jzon, update.Message.Chat.Id.ToString());
                            }
                            else
                            {
                                bot.SendMessage(update.Message.Chat.Id, "Porfavor dame tu Id empezando con al y tu contraseña separadas por una coma \n Ejemplo: al111111,Contraseña \n\n Esta informacion no sera guardada a menos que te registres ");
                                Coordinacion[1] = true;
                            }

                        }),
                        "/Suscribirme" => Task.Run(() =>
                        {
                            bot.SendMessage(update.Message.Chat.Id, "Porfavor dame tu ID y Contraseña, separadas con una coma :)");
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

