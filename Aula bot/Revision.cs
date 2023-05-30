using Telegram.BotAPI;
namespace PerreVergueBot
{
    internal class Revision
    {

        public static string Revision1(string lolo, string chavo, string update,BotClient bot)
        {
            LecturaAula lec = new();
            string lin = "";
            int auxiliar = File.ReadAllLines(lolo).Length;
            int auxiliar2 = File.ReadAllLines(chavo).Length;

            // Llamada a la función aula según el caso de revisión
            if (lolo == Rutas.path_datosOr)
            {
                lec.Aula(Rutas.path_datosOrTemp, "al283189", "Sayulita0506", Rutas.path_fecha, Rutas.path_tareas, update, bot);
            }
            else if (lolo == Rutas.path_datosOrLDI)
            {
                lec.Aula(Rutas.path_datosOrTempLDI, "al263887", "Wera060102", Rutas.path_fecha_LDI, Rutas.path_tareas_LDI, update, bot);
            }
            else if (lolo == Rutas.path_datosOrICI2)
            {
                lec.Aula(Rutas.path_datosOrTempICI2, "al261731", "SPjl3490", Rutas.path_fechas_ici2, Rutas.path_tareas_ici2, update, bot);
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

            return lin;
        }
        //Lee las personas o sus chat Id que poseen permisos elevados para poder usar el bot
        public static string Archivitos()
        {
            string Admins = "";
            //lee los id chat del documento administradores
            StreamReader Administradores;
            Administradores = File.OpenText(Rutas.path_admins);
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
    
        
    }
}
