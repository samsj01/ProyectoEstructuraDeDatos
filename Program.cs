using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace trabajo
{
    internal class Program
    {
        static void Main()
        {

            Stack<double> presupuesto = new Stack<double>();
            CargarPresupuesto(presupuesto);

            string archivoInventario = "inventario.csv";
            List<string> productos = new List<string>();
            List<int> cantidadProd = new List<int>();
            List<double> precioProd = new List<double>();
            RecargarProductos(productos, precioProd, cantidadProd, archivoInventario);

            string[] contraseñas = InicializarUsuarios();
            string[] roles = { "Administrador", "Almacen", "Caja" };
            MenuPrincipal(roles, contraseñas, productos, cantidadProd, precioProd, archivoInventario, presupuesto);

        }
        //----------------------------------------------------------------------------------------
        static void MenuPrincipal(string[] roles, string[] contraseñas, List<string> productos, List<int> cantidadProd,
            List<double> precioProd, string archivoInventario, Stack<double> presupuesto)
        {
            Console.Clear();
            bool accesoConcedido = false;
            while (!accesoConcedido)
            {
                Console.Clear();
                Console.WriteLine("BIENVENIDO AL MINI MERCADO");
                Console.Write("Usuario: ");
                string usuarioIngresado = Console.ReadLine();

                Console.Write("Contraseña: ");
                string claveIngresada = LeerPassword();
                string rolAsignado = "";

                for (int i = 0; i < roles.Length; i++)
                {

                    if (roles[i].ToLower() == usuarioIngresado.ToLower() && contraseñas[i] == claveIngresada)
                    {
                        accesoConcedido = true;
                        rolAsignado = roles[i];
                        break;
                    }
                }

                if (accesoConcedido)
                {
                    Console.Clear();

                    if (rolAsignado == "Administrador")
                    {
                        MenuAdministrador(roles, contraseñas, productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                    }
                    else if (rolAsignado == "Almacen")
                    {
                        MenuProveedor(roles, contraseñas, productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                    }
                    else if (rolAsignado == "Caja")
                    {
                        MenuCaja(roles, contraseñas, productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                    }
                }
                else
                {
                    Console.Write("Error: Usuario o contraseña incorrectos.\n...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }




        }
        //----------------------------------------------------------------------------------------
        static void MenuAdministrador(string[] roles, string[] contraseñas, List<string> productos, List<int> cantidadProd,
            List<double> precioProd, string archivoInventario, Stack<double> presupuesto)
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine(" MENÚ ADMINISTRADOR");
                Console.WriteLine("1. Ver Inventario");
                Console.WriteLine("2. Comprar Productos");
                Console.WriteLine("3. Realizar Venta ");
                Console.WriteLine("4. Salir");
                Console.Write("Opción: ");
                string op = Console.ReadLine();

                if (op == "1")
                {
                    Inventario(productos, cantidadProd, precioProd, presupuesto);
                }
                else if (op == "2")
                {
                    CompraProducto(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }

                else if (op == "3")
                {
                    VenderProductos(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }

                else if (op == "4")
                {
                    volver = true;
                    Salir(roles, contraseñas, productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }
            }

        }
        //----------------------------------------------------------------------------------------
        static void MenuProveedor(string[] roles, string[] contraseñas, List<string> productos, List<int> cantidadProd,
            List<double> precioProd, string archivoInventario, Stack<double> presupuesto)
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine(" MENÚ PROVEEDOR / ALMACÉN");
                Console.WriteLine("1. Ver Inventario ");
                Console.WriteLine("2. Cargar Stock de Productos");
                Console.WriteLine("3. Salir");
                Console.Write("Opción: ");
                string op = Console.ReadLine();

                if (op == "1")
                {
                    Inventario(productos, cantidadProd, precioProd, presupuesto);
                }
                else if (op == "2")
                {
                    CompraProducto(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }
                else if (op == "3")
                {
                    volver = true;
                    Salir(roles, contraseñas, productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }
            }


        }

        //----------------------------------------------------------------------------------------
        static void MenuCaja(string[] roles, string[] contraseñas, List<string> productos, List<int> cantidadProd, List<double> precioProd, string archivoInventario, Stack<double> presupuesto)
        {
            bool volver = false;
            while (!volver) // --- Este bucle mantiene el menú vivo
            {
                Console.Clear();
                Console.WriteLine(" === MENÚ DE CAJA === ");
                Console.WriteLine("1. Realizar Venta");
                Console.WriteLine("2. Consultar Inventario");
                Console.WriteLine("3. Cerrar Sesión");
                Console.Write("Opción: ");
                string op = Console.ReadLine();

                if (op == "1")
                {
                    // Al terminar esta función, el "return" nos devuelve aquí
                    VenderProductos(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }
                else if (op == "2")
                {
                    Inventario(productos, cantidadProd, precioProd, presupuesto);
                }
                else if (op == "3")
                {
                    volver = true;
                    Salir(roles, contraseñas, productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }
            }

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
        //----------------------------------------------------------------------------------------
        static void CompraProducto(List<string> productos, List<int> cantidadProd,
            List<double> precioProd, string archivoInventario, Stack<double> presupuesto)
        {
            double precio, compraTotal = 0, saldoActual = presupuesto.Peek();
            string addProduct = "", producto, tipo;
            int cantidad;
            bool salir = false;
            double saldoProvicional = saldoActual;
            while (!salir)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("=== COMPRA DE PRODUCTOS ===");
                    Console.WriteLine("SAlDO ACTUAL: " + saldoProvicional);

                    Console.Write("\nIngrese el producto: ");
                    producto = Console.ReadLine().ToLower();
                    int indice = productos.IndexOf(producto);
                    if (indice != -1)
                    {
                        Console.WriteLine($"\nEl producto ya se encuentra en el inventario {producto}. Precio actual: {precioProd[indice]}");
                        Console.Write("\nIngrese la cantidad de unidades que desee agregar: ");
                        cantidad = int.Parse(Console.ReadLine());
                        precio = precioProd[indice];
                        for (int i = 0; i < cantidad; i++)
                        {
                            compraTotal += precio;
                        }

                        if (compraTotal >= saldoProvicional)
                        {
                            Console.WriteLine("El valor de la compra es igual o sobrepasa el presupuesto...");
                            compraTotal = 0;
                            Console.ReadKey();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("\nla Compra ha sido exitosa.");
                            tipo = "Gasto";
                            saldoProvicional -= compraTotal;
                        }
                        Console.WriteLine("SALDO ACTUAL: " + saldoProvicional);
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

                        if (compraTotal >= saldoProvicional)
                        {
                            Console.WriteLine("El valor de la compra sobrepasa el presupuesto...");
                            compraTotal = 0;
                            Console.ReadKey();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("\nla Compra ha sido exitosa.");
                            saldoProvicional -= compraTotal;
                            tipo = "Gasto";
                        }
                        Console.WriteLine("SALDO ACTUAL: " + saldoProvicional);
                        productos.Add(producto);
                        precioProd.Add(precio);
                        cantidadProd.Add(cantidad);
                    }

                    GuardarInventario(productos, precioProd, cantidadProd, archivoInventario);
                    RegistrarMovimiento(presupuesto, tipo, compraTotal);
                    compraTotal = 0;

                    do
                    {
                        Console.WriteLine("¿Desea comprar otro producto? (si/no)");
                        addProduct = Console.ReadLine().ToLower();
                    } while (!(addProduct == "si" || addProduct == "no"));
                } while (addProduct == "si");
                if (addProduct == "no") salir = true;
            }
            Console.Clear();
            return;
        }
        //----------------------------------------------------------------------------------------
        static void Inventario(List<string> productos, List<int> cantidadProd, List<double> precioProd, Stack<double> presupuesto)
        {
            Console.Clear();
            double saldo = presupuesto.Peek();
            Console.SetCursorPosition(5, 0);
            Console.WriteLine($"SALDO ACTUAL: {saldo:C}"); // :C le da formato de moneda

            int productosPorFila = 4;

            for (int i = 0; i < productos.Count; i++)
            {
                // El residuo (%) nos da la columna (0, 1, 2, 3)
                int columna = i % productosPorFila;

                // La división (/) nos da la fila (0, 0, 0, 0, luego 1, 1, 1, 1...)
                int fila = i / productosPorFila;

                int x = 5 + (25 * columna);
                int y = 2 + (fila * 6); // Baja 6 renglones por cada nueva fila

                Console.SetCursorPosition(x, y);
                Console.Write($"--- PRODUCTO {i + 1} ---");
                Console.SetCursorPosition(x, y + 1);
               
                Console.Write($"Nombre: {productos[i].ToUpper()}");
                Console.SetCursorPosition(x, y + 2);
                if (cantidadProd[i]<=5)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"Stock: {cantidadProd[i]}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"Stock: {cantidadProd[i]}");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(x, y + 3);
                Console.Write($"Precio: {precioProd[i] / 0.8:F2}");
            }

            // Calculamos dinámicamente dónde poner el mensaje final para que no quede encima de los productos
            int filasTotales = (int)Math.Ceiling((double)productos.Count / productosPorFila);
            Console.SetCursorPosition(5, 2 + (filasTotales * 6));
            Console.Write("Presione cualquier tecla para volver...");
            Console.ReadKey();
            Console.Clear();
        }
        //----------------------------------------------------------------------------------------

        static void VenderProductos(List<string> productos, List<int> cantidadProd, List<double> precioProd, string archivoInventario, Stack<double> presupuesto)
        {
            if (productos.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("========== Venta de productos ========");
                Console.WriteLine("\nERROR: No es posible realizar ventas.");
                Console.WriteLine("No se pueden realizar ventas, el inventario está vacío.");
                Console.WriteLine("\nPresione cualquier tecla para regresar...");
                Console.ReadKey();
                return;
            }

            string continuar = "";
            double totalVentaGeneral = 0; // Acumulador para el total de la sesión

            Console.Clear();
            Console.WriteLine("========== Venta de productos ========");

            do
            {
                Console.Write("\nIngrese el nombre del producto: ");
                string buscarProd = Console.ReadLine().ToLower();
                int indice = productos.IndexOf(buscarProd);

                if (indice != -1)
                {
                    // Mostramos info del producto
                    double precioVentaUnitario = precioProd[indice] / 0.8;
                    
                    if (cantidadProd[indice] <= 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"-> Producto: {productos[indice]} | Stock actual: {cantidadProd[indice]}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"-> Producto: {productos[indice]} | Stock actual: {cantidadProd[indice]}");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"-> Precio unidad: {precioVentaUnitario:C}");

                    Console.Write("   Unidades a vender: ");
                    if (int.TryParse(Console.ReadLine(), out int cantVenta) && cantVenta > 0)
                    {
                        if (cantVenta <= cantidadProd[indice])
                        {
                            double subtotal = cantVenta * precioVentaUnitario;

                            // Actualización de datos
                            cantidadProd[indice] -= cantVenta;
                            totalVentaGeneral += subtotal; // Sumamos al total de la sesión

                            RegistrarMovimiento(presupuesto, "Ingreso", subtotal);
                            GuardarInventario(productos, precioProd, cantidadProd, archivoInventario);

                            Console.WriteLine($">> Agregado: {cantVenta} x {productos[indice]} = {subtotal:C}");

                        }
                        else
                        {
                            Console.WriteLine("   ERROR: Stock insuficiente.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("   ERROR: Cantidad no válida.");
                    }
                }
                else
                {
                    Console.WriteLine("   ERROR: El producto no existe en el inventario.");
                }


                // ... (dentro del método VenderProductos, al final del ciclo do)

                while (true)
                {
                    Console.Write("\n¿Desea registrar otro producto? (si/no): ");
                    continuar = Console.ReadLine().ToUpper(); // Convertimos a mayúsculas para facilitar la comparación

                    if (continuar == "SI" || continuar == "S" || continuar == "NO" || continuar == "N")
                    {
                        break; // Salimos del bucle de validación porque la respuesta es correcta
                    }
                    else
                    {
                        Console.WriteLine("Respuesta no válida. Por favor, ingrese 'SI' o 'NO'.");
                    }
                }

            } while (continuar == "SI" || continuar == "S" || continuar=="si" || continuar == "s");

            // Al salir del bucle principal, mostramos el total
            Console.WriteLine("\n======================================");
            Console.WriteLine($" TOTAL A PAGAR: {totalVentaGeneral:C}");
            Console.WriteLine("======================================");
            Console.WriteLine("\nPresione cualquier tecla para finalizar...");
            Console.ReadKey();

           
        }


        // ============ FUNCIONES AUXILIARES ============
        static void GuardarInventario(List<string> producto, List<double> precio, List<int> cantidad, string archivoInventario)
        {
            List<string> lineas = new List<string>();
            lineas.Add("Producto;Precio Compra;Precio Caja;Cantidad");
            for (int i = 0; i < producto.Count; i++)
            {
                lineas.Add($"{producto[i]};{precio[i]};{precio[i] / 0.8};{cantidad[i]}");
            }
            File.WriteAllLines(archivoInventario, lineas);
        }
        //----------------------------------------------------------------------------------------
        static void RecargarProductos(List<string> producto, List<double> precio, List<int> cantidad, string archivoInventario)
        {
            if (!File.Exists(archivoInventario))
            {
                return;
            }
            string[] lineas = File.ReadAllLines(archivoInventario);
            for (int i = 1; i < lineas.Length; i++)// se empieza desde 1 para ignorar el titulo en el archivo
            {
                string[] ProduDatos = lineas[i].Split(';');
                producto.Add(ProduDatos[0]);
                precio.Add(double.Parse(ProduDatos[1]));
                cantidad.Add(int.Parse(ProduDatos[3]));
            }
        }
        //----------------------------------------------------------------------------------------
        static string LeerPassword()
        {
            string pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                //Borra el caracter
                if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass.Substring(0, (pass.Length - 1));
                    Console.Write("\b \b");
                }
                //solo permite ingresar caracteres
                else if (key.Key != ConsoleKey.Enter && !char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");//
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return pass;
        }
        //----------------------------------------------------------------------------------------
        static void CargarPresupuesto(Stack<double> pila)
        {
            string archivoCostos = "costos_e_ingresos.csv";

            if (File.Exists(archivoCostos))
            {
                string[] lineas = File.ReadAllLines(archivoCostos);
                for (int i = 1; i < lineas.Length; i++)// inicia en 1 para evitar el titulo en el archivo
                {
                    string[] datos = lineas[i].Split(';');
                    if (datos.Length >= 3)
                    {
                        double saldo = double.Parse(datos[2]);
                        pila.Push(saldo);
                    }
                }
            }
            else
            {
                File.AppendAllText(archivoCostos, "Tipo;Monto;Saldo Nuevo" + Environment.NewLine);
            }

            // Si la pila sigue vacía (archivo no existía o no tenía datos), valor inicial
            if (pila.Count == 0)
                pila.Push(800000);
        }
        //----------------------------------------------------------------------------------------
        static void RegistrarMovimiento(Stack<double> pila, string tipo, double monto)
        {
            double saldoActual = pila.Peek();
            double nuevoSaldo;
            if (tipo == "Ingreso")
            {
                nuevoSaldo = saldoActual + monto;
            }
            else
            {
                nuevoSaldo = saldoActual - monto;
            }
            pila.Push(nuevoSaldo);
            File.AppendAllText("costos_e_ingresos.csv", $"{tipo};{monto};{nuevoSaldo}" + Environment.NewLine);
        }
        //----------------------------------------------------------------------------------------
        static string[] InicializarUsuarios()
        {
            string archivoUsuarios = "usuarios.txt";
            if (File.Exists(archivoUsuarios))
            {
                return File.ReadAllLines(archivoUsuarios);
            }
            string[] contraseñas = RegistroUser();
            File.WriteAllLines(archivoUsuarios, contraseñas);
            return contraseñas;
        }
        //----------------------------------------------------------------------------------------
        static void Salir(string[] roles, string[] contraseñas, List<string> productos, List<int> cantidadProd,
            List<double> precioProd, string archivoInventario, Stack<double> presupuesto)
        {
            Console.Clear();
            string salir;
            do
            {
                Console.WriteLine("¿DESEA SALIR DEL PROGRAMA? (si o no)");
                salir = Console.ReadLine().ToLower();
                if (salir == "si")
                {
                    return;
                }
                else if (salir != "si" & salir != "no")
                {
                    Console.Write("Ingrese un valor valido (si/no)\n...");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    MenuPrincipal(roles, contraseñas, productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }
            } while (salir != "si" & salir != "no");
        }
    }
}
