using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;



namespace PerreVergueBot
{
    internal class LecturaAula
    {
        public static int tareas_detectadas = 0;
        public  string Aula(string Usuario, string Password2, string update, BotClient bot)

        {

            ChromeOptions options = new();
            options.AddArguments("--headless");
            string chromeDriverPath = @"C:\Program Files\Google\Chrome\Application\chromedriver.exe";
            IWebDriver driver = new ChromeDriver(chromeDriverPath,options);
            int auxiliar = 0, auxiliar2 = 0;
            string dia = "";
            string Fechas_aula = "";

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
                    bot.SendMessage(update, "Aula está caído");
                    bot.SendAnimation(update, "https://i.pinimg.com/originals/d1/d6/c0/d1d6c0fe9c91839b97e361387b505b97.gif");
                }
                catch (Exception ar)
                {
                    Console.WriteLine("No pudimos conectarnos con el remitente\n" + ar);
                }
                return "a";
            }

            if (driver.Url != "https://aulavirtual.uaa.mx/calendar/view.php?view=month")
            {
                try
                {
                    bot.SendMessage(update, "Al parecer alguna de nuestros accesos está caído, repórtalo con el desarrollador por favor :3");
                }
                catch (Exception al)
                {
                    Console.WriteLine("No se pudo encontrar al destinatario\n" + al);
                }
                return "a";
            }

            var numero_de_tareas = driver.FindElements(By.ClassName("eventname"));
            var fecha_tareas = driver.FindElements(By.ClassName("sr-only"));

            foreach (var item2 in fecha_tareas)
            {

                if (item2.Text.ToString().Length != 0)
                {
                    if (auxiliar == 40)
                    {
                        break;
                    }
                    if (!item2.Text.Contains("Sin eventos") && !item2.Text.Contains("Omitir"))
                    {
                        if (item2.Text.Any(x => char.IsDigit(x)))
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
                                auxiliar2 += Int32.Parse(Convert.ToString(item2.Text)[0].ToString());
                            }
                        }
                    }
                    auxiliar += 1;
                }
            }

            auxiliar = 0;
            dia = "";

            foreach (var item in numero_de_tareas)
            {
                if (!item.Text.Contains("Ocultar"))
                {
                    auxiliar += 1;
                    if (auxiliar > auxiliar2)
                    {
                        dia += item.Text.ToString() + "\n";
                     
                    }
                }
            }

            try
            {
                string[] Fechas = Fechas_aula.Split('\n');
                JsonGeneral.RellenarFechas(Rutas.Jzon, update, Fechas);
                string[] Tareas = dia.Split("\n");
                JsonGeneral.RellenarTareas(Rutas.Jzon, update, Tareas);
            }
            catch (Exception Noabrio)
            {
                Console.WriteLine("No se pudo abrir o escribir en uno o ambos archivos de Fechas o Eventos, el código de error es:\n" + Noabrio);
            }

            int[] auxi = new int[20];
            auxiliar2 = 0;

                string[] temporal = JsonGeneral.LeerFechas(Rutas.Jzon,update);
            string[] temporal2 = JsonGeneral.LeerTareas(Rutas.Jzon,update);
               for(int i = 0; i < temporal.Length; i++) 
                {
                    if (!string.IsNullOrEmpty(temporal[i]))
                    {
                        auxi[auxiliar2] += temporal[i][0];
                        auxiliar2 += 1;
                    }
                }
            string[] ordenado = new string[200];

        
                dia = "";

                for (int i = 0; i < auxiliar2; i++)
                {
                    var ase = temporal[i];
                    if (ase != null)
                    {
                        dia = ase.ToString();
                    }
                    ordenado[i] = dia;
                    dia = "";
                    int integer = auxi[i] - '0';
                    for (int j = 0; j < integer; j++)
                    {
                        var lo = temporal2[j];
                        if (lo != null)
                        {
                            dia = lo;
                        }
                        ordenado[j+1]=dia;
                    }
                }
            JsonGeneral.RellenarTareasOrdenadas(Rutas.Jzon, update, ordenado);
            
            var menuToggle = driver.FindElement(By.Id("action-menu-toggle-0"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", menuToggle);
            menuToggle.Click();
            driver.Quit();
            tareas_detectadas = ordenado.Count(t => !string.IsNullOrWhiteSpace(t));
            return "a";
        }

    }
}
