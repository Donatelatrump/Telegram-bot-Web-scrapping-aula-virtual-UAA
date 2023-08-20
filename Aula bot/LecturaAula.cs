using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;



namespace PerreVergueBot
{
    internal class LecturaAula
    {
        public static int tareas_detectadas = 0;
        public  string Aula(string path, string Usuario, string Password2, string fecha1, string tarea1, string update, BotClient bot)

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
                                    tareas_detectadas += Int32.Parse(Convert.ToString(item2.Text)[0].ToString()) + 1;
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
                File.WriteAllText(fecha1, Fechas_aula);
                File.WriteAllText(tarea1, dia);
            }
            catch (Exception Noabrio)
            {
                Console.WriteLine("No se pudo abrir o escribir en uno o ambos archivos de Fechas o Eventos, el código de error es:\n" + Noabrio);
            }

            int[] auxi = new int[20];
            auxiliar2 = 0;

            using (StreamReader Primer_evento = File.OpenText(fecha1))
            {
                string temporal = Primer_evento.ReadLine();
                while (temporal != null)
                {
                    if (!string.IsNullOrEmpty(temporal))
                    {
                        auxi[auxiliar2] += temporal[0];
                        auxiliar2 += 1;
                    }
                    temporal = Primer_evento.ReadLine();
                }
            }

            using (StreamReader lolcito = File.OpenText(tarea1))
            using (StreamReader Primer_evento = File.OpenText(fecha1))
            using (StreamWriter aiuda = new(path))
            {
                dia = "";

                for (int i = 0; i < auxiliar2; i++)
                {
                    var ase = Primer_evento.ReadLine();
                    if (ase != null)
                    {
                        dia = ase.ToString();
                    }
                    aiuda.WriteLine(dia);
                    dia = "";
                    int integer = auxi[i] - '0';
                    for (int j = 0; j < integer; j++)
                    {
                        var lo = lolcito.ReadLine();
                        if (lo != null)
                        {
                            dia = lo;
                        }
                        aiuda.WriteLine(dia);
                    }
                }
            }
            var menuToggle = driver.FindElement(By.Id("action-menu-toggle-0"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", menuToggle);
            menuToggle.Click();

           

            driver.Quit();

            int lineCount = File.ReadAllLines(path).Length;

            return "a";
        }

    }
}
