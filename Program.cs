using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabajo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Llamamos al método y guardamos el resultado
            string[] contraseñas = RegistroUser();

            Console.WriteLine("=== PROCESO FINALIZADO ===");
            Console.WriteLine("Todos los usuarios han sido registrados.");
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }

        static string[] RegistroUser()
        {
            string[] usuarios = { "Administrador", "Proveedor", "Caja" };
            string[] contraseñasGuardadas = new string[usuarios.Length];

            for (int i = 0; i < usuarios.Length; i++)
            {
                string ingreso1 = "";
                string ingreso2 = "";
                bool coinciden = false;

                while (!coinciden)
                {
                    Console.WriteLine($"=== REGISTRO DE USUARIOS ({usuarios[i]}) ===");

                    Console.Write("Cree la contraseña: ");
                    ingreso1 = LeerPassword();

                    Console.Write("\nConfirme la contraseña: ");
                    ingreso2 = LeerPassword();

                    // Validación: Que no sea espacio en blanco y que ambas sean iguales
                    if (!string.IsNullOrWhiteSpace(ingreso1) && ingreso1 == ingreso2)
                    {
                        contraseñasGuardadas[i] = ingreso1;
                        coinciden = true;
                        Console.WriteLine("\n\n[✔] Contraseña guardada correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("\n\n[✘] Error: Las contraseñas no coinciden o están vacías.");
                        Console.WriteLine("Intente de nuevo.");
                    }

                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            return contraseñasGuardadas;
        }

        /// <summary>
        /// Captura caracteres de consola ocultándolos con asteriscos.
        /// </summary>
        static string LeerPassword()
        {
            string pass = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true); // El 'true' evita que la tecla se vea en pantalla

                // Si no es Enter ni Borrar, agregamos el caracter
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                // Si es Borrar y hay texto, eliminamos el último caracter
                else if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass.Substring(0, (pass.Length - 1));
                    Console.Write("\b \b"); // Mueve el cursor atrás, escribe espacio y vuelve atrás
                }
            } while (key.Key != ConsoleKey.Enter);

            return pass;
        }
    }
}
