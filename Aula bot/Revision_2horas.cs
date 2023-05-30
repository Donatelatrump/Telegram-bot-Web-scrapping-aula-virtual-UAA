using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.BotAPI;
using Telegram.BotAPI.GettingUpdates;
using Telegram.BotAPI.AvailableMethods;
using CsQuery.ExtensionMethods.Internal;

namespace PerreVergueBot
{
    internal class Revision_2horas
    {
        public static void Revision_2(string id, BotClient bot)
        {
            string tareas_diferencias = "";
            Revision rev = new();
            _ = Task.Run(() =>
            {
                try
                {

                    //si al hacer la revision de aula esta detecta nuevas tareas entra en este caso
                    tareas_diferencias = Revision.Revision1(Rutas.path_datosOr, Rutas.path_datosOrTemp, id, bot);
                    if (!tareas_diferencias.IsNullOrEmpty())
                    {
                        StreamReader lectura3 = File.OpenText(Rutas.path_suscritos);
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
                    tareas_diferencias = Revision.Revision1(Rutas.path_datosOrLDI, Rutas.path_datosOrTempLDI, id, bot);
                    if (!tareas_diferencias.IsNullOrEmpty())
                    {
                        StreamReader lectura3 = File.OpenText(Rutas.path_suscritosLDI);
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
                    tareas_diferencias = Revision.Revision1(Rutas.path_datosOrICI2, Rutas.path_datosOrTempICI2, id, bot);
                    if (!tareas_diferencias.IsNullOrEmpty())
                    {
                        StreamReader lectura3 = File.OpenText(Rutas.path_suscritos_ici2);
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


    }
}
