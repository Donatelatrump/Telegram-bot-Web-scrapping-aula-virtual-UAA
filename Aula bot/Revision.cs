using Telegram.BotAPI;
namespace PerreVergueBot
{
    internal class Revision
    {
        //Declaracion de las rutas de los archivos que almacenan la informacion (debo encontrar una mejor manera de guardarlo, una manera mas eficiente)

        static readonly string direct = Directory.GetCurrentDirectory();
        readonly string path_tareas = direct + "\\Archivos\\Eventos.txt";
        readonly string path_fecha = direct + "\\Archivos\\Fechas.txt";
        readonly string path_admins = direct + "\\Archivos\\Admins.txt";
        readonly string path_datosOr = direct + "\\Archivos\\DatosOr.txt";
        readonly string path_datosOrLDI = direct + "\\Archivos\\DatosOrLDI.txt";
        readonly string path_datosOrTemp = direct + "\\Archivos\\DatosOrTemp.txt";
        readonly string path_datosOrTempLDI = direct + "\\Archivos\\DatosOrTempLDI.txt";
        readonly string path_fecha_LDI = direct + "\\Archivos\\Fechas_LDI.txt";
        readonly string path_tareas_LDI = direct + "\\Archivos\\Eventos_LDI.txt";
        readonly string path_tareas_ici2 = direct + "\\Archivos\\EventosICI2.txt";
        readonly string path_fechas_ici2 = direct + "\\Archivos\\FechasICI2.txt";
        readonly string path_datosOrICI2 = direct + "\\Archivos\\DatosOrICI2.txt";
        readonly string path_datosOrTempICI2 = direct + "\\Archivos\\DatosOrTempICI2.txt";

        public bool Revision1(string lolo, string chavo, string update,BotClient bot)
        {
            _ = new LecturaAula();
            string lin = "";
            int auxiliar = File.ReadAllLines(lolo).Length;
            int auxiliar2 = File.ReadAllLines(chavo).Length;

            // Llamada a la función aula según el caso de revisión
            if (lolo == path_datosOr)
            {
                LecturaAula.Aula(path_datosOrTemp, "al283189", "Sayulita0506", path_fecha, path_tareas, update, bot);
            }
            else if (lolo == path_datosOrLDI)
            {
                LecturaAula.Aula(path_datosOrTempLDI, "al263887", "Wera060102", path_fecha_LDI, path_tareas_LDI, update, bot);
            }
            else if (lolo == path_datosOrICI2)
            {
                LecturaAula.Aula(path_datosOrTempICI2, "al261731", "SPjl3490", path_fechas_ici2, path_tareas_ici2, update, bot);
            }

            if (auxiliar2 > auxiliar)
            {
                using StreamReader aioch = File.OpenText(chavo);
                for (int j = 0; j < auxiliar2; j++)
                {
                    var gf = aioch.ReadLine();
                    if (j >= auxiliar2 && gf != null)
                    {
                        lin += gf;
                    }
                }
            }

            return !string.IsNullOrEmpty(lin);
        }
        //Lee las personas o sus chat Id que poseen permisos elevados para poder usar el bot
        public string Archivitos()
        {
            string Admins = "";
            //lee los id chat del documento administradores
            StreamReader Administradores;
            Administradores = File.OpenText(path_admins);
            if (Administradores.ToString() != null)
            {
                string? fz;
                if ((fz = Administradores.ReadLine()) != null)
                {
                    Admins = fz;
                }
                Administradores.Close();
                return Admins;
            }
            else
            {
                Administradores.Close();
                return "";
            }
        }
        //Guarda las suscripciones en sus debidos archivos para tener ese almacenaje
        public static string Suscritos(String datos, string path)
        {
            string cmp = "";
            //se lee si hay datos en el archivo
            StreamReader lectura21 = File.OpenText(path);
            bool verdad;
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
            return cmp;
        }
        //Funcion para Inicar un programa .exe o .bat , en este caso para iniciar el servidor de Minecraft
        
    }
}
