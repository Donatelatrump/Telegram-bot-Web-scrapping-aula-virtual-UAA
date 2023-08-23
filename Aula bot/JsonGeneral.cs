using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
namespace PerreVergueBot
{
    internal class JsonGeneral
    {

        public
        static void AgregarNuevoCliente(string jsonFilePath, string nuevoCodigo, string nuevoUsuario, string nuevaContrasena)
        {
            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Verificar si la lista de clientes existe y crearla si es necesario
            if (datosClientes.Clientes == null)
            {
                datosClientes.Clientes = new List<Cliente>();
            }

            // Agregar un nuevo cliente
            Cliente nuevoCliente = new Cliente
            {
                Codigo = nuevoCodigo,
                Usuario = nuevoUsuario,
                Contrasena = nuevaContrasena,
                Tareas = new List<string>(),
                Fechas = new List<string>(),
                TareasOrdenadas = new List<string>()
            };

            datosClientes.Clientes.Add(nuevoCliente);

            // Serializar el objeto DatosClientes actualizado de vuelta al texto JSON
            string nuevoJsonText = JsonSerializer.Serialize(datosClientes, new JsonSerializerOptions { WriteIndented = true });

            // Guardar el JSON actualizado en el archivo
            File.WriteAllText(jsonFilePath, nuevoJsonText);
        }

        // Clase para representar el objeto JSON completo
        class DatosClientes
        {
            public List<Cliente> Clientes { get; set; }
        }

        // Clase para representar un cliente en el JSON
        class Cliente
        {
            public string Codigo { get; set; }
            public string Usuario { get; set; }
            public string Contrasena { get; set; }
            public List<string> Tareas { get; set; }
            public List<string> Fechas { get; set; }
            public List<string> TareasOrdenadas { get; set; }
        }

        public static string LeerCodigosClientes(string jsonFilePath)
        {
            // Leer el JSON desde el archivo
            string jsonText = File.Exists(jsonFilePath) ? File.ReadAllText(jsonFilePath) : "{}";

            // Deserializar el JSON a un objeto JsonDocument
            using (JsonDocument doc = JsonDocument.Parse(jsonText))
            {
                JsonElement root = doc.RootElement;

                // Verificar si existe la propiedad "clientes" y si no está vacía
                if (root.TryGetProperty("clientes", out JsonElement clientesElement) && clientesElement.ValueKind == JsonValueKind.Array)
                {
                    List<Cliente> clientes = new List<Cliente>();

                    // Convertir la lista de clientes a la clase Cliente
                    foreach (var clienteElement in clientesElement.EnumerateArray())
                    {
                        Cliente cliente = JsonSerializer.Deserialize<Cliente>(clienteElement.GetRawText());
                        clientes.Add(cliente);
                    }

                    // Crear una lista de códigos de clientes
                    List<string> codigos = new List<string>();
                    foreach (Cliente cliente in clientes)
                    {
                        codigos.Add(cliente.Codigo);
                    }

                    return codigos.Count > 0 ? string.Join(", ", codigos) : "nosta";
                }
                else
                {
                    return "nosta";
                }
            }
        }
        public static void Reseteo(string jsonFilePath, string clienteCodigo)
        {
            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Buscar el cliente por su código
            Cliente cliente = datosClientes.Clientes.FirstOrDefault(c => c.Codigo == clienteCodigo);

            // Verificar si se encontró el cliente
            if (cliente != null)
            {
                // Borrar todas las tareas, fechas y tareas ordenadas del cliente
                cliente.Tareas.Clear();
                cliente.Fechas.Clear();
                cliente.TareasOrdenadas.Clear();
            }
            else
            {
                Console.WriteLine($"No se encontró el cliente con código: {clienteCodigo}");
                return;
            }

            // Serializar el objeto DatosClientes actualizado de vuelta al texto JSON
            string nuevoJsonText = JsonSerializer.Serialize(datosClientes, new JsonSerializerOptions { WriteIndented = true });

            // Guardar el JSON actualizado en el archivo
            File.WriteAllText(jsonFilePath, nuevoJsonText);

            Console.WriteLine($"Se han reseteado las tareas, fechas y tareas ordenadas del cliente con código: {clienteCodigo}");
        }
        public static void RellenarTareas(string jsonFilePath, string clienteCodigo, string[] nuevasTareas)
        {
            // Filtrar las nuevas tareas para eliminar elementos vacíos o nulos
            nuevasTareas = nuevasTareas.Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();

            // Verificar si no quedaron tareas válidas
            if (nuevasTareas.Length == 0)
            {
                Console.WriteLine("No hay tareas válidas para agregar.");
                return;
            }

            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Buscar el cliente por su código
            Cliente cliente = datosClientes.Clientes.FirstOrDefault(c => c.Codigo == clienteCodigo);

            // Verificar si se encontró el cliente
            if (cliente != null)
            {
                // Agregar las nuevas tareas al espacio de tareas del cliente
                cliente.Tareas.AddRange(nuevasTareas);
            }
            else
            {
                Console.WriteLine($"No se encontró el cliente con código: {clienteCodigo}");
                return;
            }

            // Serializar el objeto DatosClientes actualizado de vuelta al texto JSON
            string nuevoJsonText = JsonSerializer.Serialize(datosClientes, new JsonSerializerOptions { WriteIndented = true });

            // Guardar el JSON actualizado en el archivo
            File.WriteAllText(jsonFilePath, nuevoJsonText);
        }

        public static void RellenarFechas(string jsonFilePath, string clienteCodigo, string[] nuevasFechas)
        {
            // Filtrar las nuevas fechas para eliminar elementos vacíos o nulos
            nuevasFechas = nuevasFechas.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();

            // Verificar si no quedaron fechas válidas
            if (nuevasFechas.Length == 0)
            {
                Console.WriteLine("No hay fechas válidas para agregar.");
                return;
            }

            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Buscar el cliente por su código
            Cliente cliente = datosClientes.Clientes.FirstOrDefault(c => c.Codigo == clienteCodigo);

            // Verificar si se encontró el cliente
            if (cliente != null)
            {
                // Agregar las nuevas fechas al espacio de fechas del cliente
                cliente.Fechas.AddRange(nuevasFechas);
            }
            else
            {
                Console.WriteLine($"No se encontró el cliente con código: {clienteCodigo}");
                return;
            }

            // Serializar el objeto DatosClientes actualizado de vuelta al texto JSON
            string nuevoJsonText = JsonSerializer.Serialize(datosClientes, new JsonSerializerOptions { WriteIndented = true });

            // Guardar el JSON actualizado en el archivo
            File.WriteAllText(jsonFilePath, nuevoJsonText);
        }

        public static void RellenarTareasOrdenadas(string jsonFilePath, string clienteCodigo, string[] nuevasTareasOrdenadas)
        {
            // Filtrar las nuevas tareas ordenadas para eliminar elementos vacíos o nulos
            nuevasTareasOrdenadas = nuevasTareasOrdenadas.Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();

            // Verificar si no quedaron tareas ordenadas válidas
            if (nuevasTareasOrdenadas.Length == 0)
            {
                Console.WriteLine("No hay tareas ordenadas válidas para agregar.");
                return;
            }

            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Buscar el cliente por su código
            Cliente cliente = datosClientes.Clientes.FirstOrDefault(c => c.Codigo == clienteCodigo);

            // Verificar si se encontró el cliente
            if (cliente != null)
            {
                // Agregar las nuevas tareas ordenadas al espacio de tareas ordenadas del cliente
                cliente.TareasOrdenadas.AddRange(nuevasTareasOrdenadas);
            }
            else
            {
                Console.WriteLine($"No se encontró el cliente con código: {clienteCodigo}");
                return;
            }

            // Serializar el objeto DatosClientes actualizado de vuelta al texto JSON
            string nuevoJsonText = JsonSerializer.Serialize(datosClientes, new JsonSerializerOptions { WriteIndented = true });

            // Guardar el JSON actualizado en el archivo
            File.WriteAllText(jsonFilePath, nuevoJsonText);
        }

        public  static string[] LeerFechas(string jsonFilePath, string clienteCodigo)
        {
            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Buscar el cliente por su código
            Cliente cliente = datosClientes.Clientes.FirstOrDefault(c => c.Codigo == clienteCodigo);

            // Verificar si se encontró el cliente
            if (cliente != null)
            {
                // Devolver las fechas del cliente como arreglo de strings
                return cliente.Fechas.ToArray();
            }
            else
            {
                Console.WriteLine($"No se encontró el cliente con código: {clienteCodigo}");
                return new string[0]; // Retorna un arreglo vacío si no se encuentra el cliente
            }
        }

       public static string[] LeerTareas(string jsonFilePath, string clienteCodigo)
        {
            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Buscar el cliente por su código
            Cliente cliente = datosClientes.Clientes.FirstOrDefault(c => c.Codigo == clienteCodigo);

            // Verificar si se encontró el cliente
            if (cliente != null)
            {
                // Devolver las tareas del cliente como arreglo de strings
                return cliente.Tareas.ToArray();
            }
            else
            {
                Console.WriteLine($"No se encontró el cliente con código: {clienteCodigo}");
                return new string[0]; // Retorna un arreglo vacío si no se encuentra el cliente
            }
        }


       public static string LeerUsuario(string jsonFilePath, string clienteCodigo)
        {
            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Buscar el cliente por su código
            Cliente cliente = datosClientes.Clientes.FirstOrDefault(c => c.Codigo == clienteCodigo);

            // Verificar si se encontró el cliente
            if (cliente != null)
            {
                // Devolver el usuario del cliente
                return cliente.Usuario;
            }
            else
            {
                Console.WriteLine($"No se encontró el cliente con código: {clienteCodigo}");
                return ""; // Retorna un string vacío si no se encuentra el cliente
            }
        }

       public static string LeerContrasena(string jsonFilePath, string clienteCodigo)
        {
            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Buscar el cliente por su código
            Cliente cliente = datosClientes.Clientes.FirstOrDefault(c => c.Codigo == clienteCodigo);

            // Verificar si se encontró el cliente
            if (cliente != null)
            {
                // Devolver la contraseña del cliente
                return cliente.Contrasena;
            }
            else
            {
                Console.WriteLine($"No se encontró el cliente con código: {clienteCodigo}");
                return ""; // Retorna un string vacío si no se encuentra el cliente
            }
        }

      public  static string[] LeerTareasOrdenadas(string jsonFilePath, string clienteCodigo)
        {
            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Buscar el cliente por su código
            Cliente cliente = datosClientes.Clientes.FirstOrDefault(c => c.Codigo == clienteCodigo);

            // Verificar si se encontró el cliente
            if (cliente != null)
            {
                // Devolver las tareas ordenadas del cliente como arreglo de strings
                return cliente.TareasOrdenadas.ToArray();
            }
            else
            {
                Console.WriteLine($"No se encontró el cliente con código: {clienteCodigo}");
                return new string[0]; // Retorna un arreglo vacío si no se encuentra el cliente
            }
        }

        public static bool ExisteCodigoCliente(string jsonFilePath, string clienteCodigo)
        {
            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Verificar si existe un cliente con el código proporcionado
            bool existeCliente = datosClientes.Clientes.Any(c => c.Codigo == clienteCodigo);

            return existeCliente;
        }

        public static void EliminarCliente(string jsonFilePath, string clienteCodigo)
        {
            // Leer el JSON desde el archivo
            string jsonText = File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una instancia de DatosClientes
            DatosClientes datosClientes = JsonSerializer.Deserialize<DatosClientes>(jsonText);

            // Buscar el índice del cliente por su código
            int indiceCliente = datosClientes.Clientes.FindIndex(c => c.Codigo == clienteCodigo);

            // Verificar si se encontró el cliente
            if (indiceCliente != -1)
            {
                // Eliminar el cliente de la lista
                datosClientes.Clientes.RemoveAt(indiceCliente);

                // Serializar el objeto DatosClientes actualizado de vuelta al texto JSON
                string nuevoJsonText = JsonSerializer.Serialize(datosClientes, new JsonSerializerOptions { WriteIndented = true });

                // Guardar el JSON actualizado en el archivo
                File.WriteAllText(jsonFilePath, nuevoJsonText);

                Console.WriteLine($"Se ha eliminado el cliente con código: {clienteCodigo}");
            }
            else
            {
                Console.WriteLine($"No se encontró el cliente con código: {clienteCodigo}");
            }
        }
    }
}









