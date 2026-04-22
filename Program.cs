using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] contraseñas = RegistroUser();
        }

        static string[] RegistroUser()
        {
            string[] usuarios = { "Administrador", "Proveedor", "Caja" };
            string[] contraseña = new string[usuarios.Length];
            bool userNoRegis = true;
            int contador = 1;
            
            while (userNoRegis)
            {
                for (int i = 0; i < usuarios.Length; i++)
                {
                    string ingresoContraseña;
                    string validarContraseña;
                    do
                    {
                        Console.WriteLine("=== REGISTRO DE USUARIOS ===");
                        Console.Write("Cree la contraseña del usuario " + usuarios[i] + ": ");
                        ingresoContraseña = Console.ReadLine();
                        Console.Write("Ingrese la contraseña nuevamente: ");
                        validarContraseña = Console.ReadLine();
                        if (ingresoContraseña != validarContraseña)
                        {
                            Console.Write("No hay similitud en el ingreso de contraseñas.\n...");
                            Console.ReadKey();
                            Console.Clear();

                        }
                        else
                        {
                            Console.Write("Contraseña Guardada.\n...");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    } while (ingresoContraseña != validarContraseña);
                    contador++;
                    if (contador == usuarios.Length)
                    {
                        userNoRegis = false;
                    }
                }
                
            }
            return contraseña;
        }
    }
}
