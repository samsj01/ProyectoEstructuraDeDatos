using System;
using System.Collections.Generic;
using System.IO;


namespace trabajo
{
    internal class Program
    {
        static void Main()
        {
            string archivoInventario = "inventario.csv";
            List<string> productos = new List<string>();
            List<int> cantidadProd = new List<int>();
            List<double> precioProd = new List<double>();
            double presupuesto = 100000;
            RecargarProductos(productos, precioProd, cantidadProd,  archivoInventario, presupuesto);

            string[] contraseñas;
            string archivoUsuarios = "usuarios.txt";
            if (File.Exists(archivoUsuarios))
            {
                File.ReadAllLines(archivoUsuarios);
            }
            else
            {
                // Llamamos al método y guardamos el resultado
                contraseñas = RegistroUser();
                File.WriteAllLines(archivoUsuarios, contraseñas);
                Console.WriteLine("Las contraseñas se han guardado correctamente. ");
            }
            Console.WriteLine("=== PROCESO FINALIZADO ===");
            Console.WriteLine("Todos los usuarios han sido registrados.");
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
            // Acá se debe de llamar la función del menu de los usuarios.
            // MenuUsuarios();
            CompraProducto(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
        }

        // ============ FUNCIONES PRINCIPALES ===============
        static string[] RegistroUser()
        {
            string[] usuarios = { "Administrador", "Almacén", "Caja" };
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
                        Console.WriteLine("\n\nContraseña guardada correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("\n\nError: Las contraseñas no coinciden o están vacías.");
                        Console.WriteLine("Intente de nuevo.");
                    }

                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            return contraseñasGuardadas;
        }

       
        static void CompraProducto(List<string> productos, List<int> cantidadProd, 
            List<double> precioProd, string archivoInventario, double presupuesto)
        {
            
            //string archivoInventario = "Inventario.txt";
            double precio, compraTotal = 0;
            string addProduct = "", producto;
            int cantidad;
            bool salir = false;
            while (!salir)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("=== COMPRA DE PRODUCTOS ===");
                    Console.WriteLine("SAlDO ACTUAL: " + presupuesto);

                    Console.Write("\nIngrese el producto: ");
                    producto = Console.ReadLine().ToLower();
                    int indice = productos.IndexOf(producto);
                    bool productoExiste = false;
                    if(indice != -1)
                    {
                        productoExiste=true;
                        Console.WriteLine($"\nEl producto ya se encuentra en el inventario {producto}. Precio actual: {precioProd[indice]}");
                        Console.Write("\nIngrese la cantidad de unidades que desee agregar: ");
                        cantidad = int.Parse(Console.ReadLine());
                        precio = precioProd[indice];

                        for (int i = 0; i < cantidad; i++)
                        {
                            compraTotal += precio;
                        }

                        if (compraTotal >= presupuesto)
                        {
                            Console.WriteLine("El valor de la compra es igual o sobrepasa el presupuesto.\n" +
                                "compre menos unidades o compre otro producto\n...");
                            compraTotal = 0;
                            Console.ReadKey();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("\nla Compra ha sido exitosa, guardando en factura.");
                            presupuesto -= compraTotal;

                        }
                        Console.WriteLine("SALDO ACTUAL: " + presupuesto);
                        
                        cantidadProd[indice] += cantidad;
                    }
                    else
                    {
                        Console.WriteLine("\n¡¡PRODUCTO NUEVO!!");
                        Console.Write("\nIngrese el precio por unidad: ");
                        precio = double.Parse(Console.ReadLine());
                        Console.Write("\nIngrese la cantidad de unidades que desee comprar: ");
                        cantidad = int.Parse(Console.ReadLine());
                        for (int i = 0; i < cantidad; i++)
                        {
                            compraTotal += precio;
                        }

                        if (compraTotal >= presupuesto)
                        {
                            Console.WriteLine("El valor de la compra es igual o sobrepasa el presupuesto.\n" +
                                "compre menos unidades o compre otro producto\n...");
                            compraTotal = 0;
                            Console.ReadKey();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("\nla Compra ha sido exitosa, guardando en factura.");
                            presupuesto -= compraTotal;

                        }
                        Console.WriteLine("SALDO ACTUAL: " + presupuesto);
                        productos.Add(producto);
                        precioProd.Add(precio);
                        cantidadProd.Add(cantidad);

                    }

                    GuardarInventario(productos, precioProd, cantidadProd, productoExiste, archivoInventario, presupuesto);

                    Console.WriteLine("Producto Ingresado al inventario.");
                    do
                    {
                        Console.WriteLine("¿Desea comprar otro producto? (si/no)");
                        addProduct = Console.ReadLine().ToLower();
                        if (!(addProduct == "si" || addProduct == "no"))
                        {
                            Console.WriteLine("Respuesta no válida: debes ser (si o no)");
                        }
                    } while (!(addProduct == "si" || addProduct == "no"));
                } while (addProduct == "si");
                if (addProduct == "no")
                {
                    salir = true;
                }
            }

            Console.WriteLine("Final de la compra. Generando Factura.");
            //FacturaCompra();
            Console.WriteLine("presione cualquier tecla para volver al menú\n...");
            Console.ReadKey();
            Console.Clear();
            //Menu();
        }

        // =============== FUNCIONES AUXILIARES Y CONTROL =====================
        static void GuardarInventario(List<string> producto, List<double> precio, List<int> cantidad, 
            bool existe, string archivoInventario, double presupuesto)
        {
            
            List<string> lineas = new List<string>();
            lineas.Add("Producto;Precio;Cantidad");

            for (int i = 0; i < producto.Count; i++)
            {
                lineas.Add($"{producto[i]};{precio[i]};{cantidad[i]}");
            }

            File.WriteAllLines(archivoInventario, lineas);
        }

        static void RecargarProductos(List<string> producto, List<double> precio, 
            List<int> cantidad, string archivoInventario, double presupuesto)
        {
            if (!File.Exists(archivoInventario)) return;

            string[] lineas = File.ReadAllLines(archivoInventario);

            for(int i =1; i<lineas.Length; i++)
            {
                string[] ProduDatos = lineas[i].Split(';');

                producto.Add(ProduDatos[0]);
                precio.Add(double.Parse(ProduDatos[1]));
                cantidad.Add(int.Parse(ProduDatos[2]));
            }
        }
        static string LeerPassword()
        {
            string pass = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // pa que borre
                if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass.Substring(0, (pass.Length - 1));
                    Console.Write("\b \b");
                }
                //  solo deja ingresar caracteres
                else if (key.Key != ConsoleKey.Enter && !char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }

            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return pass;
        }
    }
}
