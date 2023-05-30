using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.BotAPI;
using Telegram.BotAPI.GettingUpdates;
using Telegram.BotAPI.AvailableMethods;
using System.Diagnostics;

namespace PerreVergueBot
{
    internal class LecturaAula
    {
 
        public static string Aula(string path, string Usuario, string Password2, string fecha1, string tarea1, string update, BotClient bot)
        {
            int auxiliar = 0,auxiliar2=0;
            string dia="";
            string Fechas_aula = "";
            //Abrir chomre en aula y enviarle los datos de acceso 
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Minimize();
            try
            {
                driver.Navigate().GoToUrl("https://aulavirtual.uaa.mx/login/index.php");
                var user = driver.FindElement(By.Name("username"));
                user.SendKeys(Usuario);
                var contra = driver.FindElement(By.Name("password"));
                contra.SendKeys(Password2);
                contra.Submit();
                driver.Navigate().GoToUrl("https://aulavirtual.uaa.mx/calendar/view.php?view=month");
            }
            catch (Exception inter)
            {
                Console.WriteLine(inter);
                try
                {
                    bot.SendMessage(update, "Aula esta caido");
                    bot.SendAnimation(update, "https://i.pinimg.com/originals/d1/d6/c0/d1d6c0fe9c91839b97e361387b505b97.gif");
                }
                catch (Exception ar)
                {
                    Console.WriteLine("No pudimos conectarnos con el remitente\n" + ar);
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
                        if (auxiliar == 40) //Condicional para que no de vueltas de mas inecesarias
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
                                if (Int32.Parse(dia) >= Int32.Parse(DateTime.Now.ToString("dd")))
                                {
                                    Fechas_aula += item2.Text + "\n";
                                }
                                else
                                {
                                    auxiliar2 = Int32.Parse(Convert.ToString(item2.Text)[0].ToString());
                                }
                            }

                        }
                        auxiliar += 1;
                    }
                }
                //reiniciamos variables
                auxiliar = 0;
                dia = "";
                foreach (var item in numero_de_tareas)
                {

                    if (!item.Text.Contains("Ocultar"))
                    {
                       auxiliar += 1;
                        //Decirle que mientras el contador de las tareas leidas sea mayor al contador de fechas ignoradas debe seguir leyendo tareas
                        if (auxiliar > auxiliar2)
                        {
                            dia += item.Text.ToString() + "\n";
                        }
                    }
                }
                //Para el texto de las tareas
                try
                {
                    StreamWriter dayo = new(fecha1);
                    dayo.Write(Fechas_aula);
                    dayo.Close();
                    // Eventos
                  Fechas_aula = "";
                    StreamWriter eventos = new(tarea1);
                    eventos.WriteLine(dia);
                    eventos.Close();
                }
                catch (Exception Noabrio)
                {
                    Console.WriteLine("No se pudo abrir o escribir en uno o ambos archivos de Fechas o Eventos, el código de error es:\n" + Noabrio);
                }
                int[] auxi = new int[20];
                auxiliar2 = 0;
                // Contador de líneas de eventos
                StreamReader Primer_evento = File.OpenText(fecha1);
                string temporal = Primer_evento.ReadLine(); // Leer la primera línea
                while (temporal != null)
                {
                    if (!string.IsNullOrEmpty(temporal))
                    {
                        auxi[auxiliar2]  += temporal[0];
                        auxiliar2 += 1;
                        
                    }
                    temporal = Primer_evento.ReadLine(); // Leer la siguiente línea
                }
                Primer_evento.Close();


                StreamReader lolcito = File.OpenText(tarea1);
                Primer_evento = File.OpenText(fecha1);
                StreamWriter aiuda = new(path);
                dia = "";
                for (int i = 0; i < auxiliar2; i++)
                {
                    var ase = "";
                    if ((ase = Primer_evento.ReadLine()) != null)
                    {
                        dia= ase.ToString();
                    }
                    aiuda.WriteLine(dia);
                    dia = "";
                    int integer = auxi[i] - '0';
                    for (int j = 0; j < integer; j++)
                    {
                        var lo = "";
                        if ((lo = lolcito.ReadLine()) != null)
                        {
                            dia = lo;
                        }
                        aiuda.WriteLine(dia);
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
                auxiliar = 0;
                while (ola.ReadLine() != null)
                {
                   auxiliar+= 1;
                }
                ola.Close();
                return "a";
            }
            else
            {
                try
                {
                    bot.SendMessage(update, "Al parecer alguna de nuestros accesos esta caido, reportalo con el desarrollador por favor :3");
                }
                catch (Exception al)
                {
                    Console.WriteLine("No se pudo encontrar al destinatario\n" + al);
                }
                return "a";
            }
        }

    }
}
