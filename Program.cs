using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

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

            string[] roles = { "Administrador", "Almacén", "Caja" };

            Console.Clear();
            Console.WriteLine(" BIENVENIDO AL MINI MERCADO");
            Console.Write("Usuario: ");
            string usuarioIngresado = Console.ReadLine();

            Console.Write("Contraseña: ");
            string claveIngresada = LeerPassword(); 

            bool accesoConcedido = false;
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
                Console.WriteLine($"Bienvenido/a, {usuarioIngresado}. Rol: {rolAsignado}\n");

                if (rolAsignado == "Administrador")
                {
                    MenuAdministrador(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }
                else if (rolAsignado == "Almacén") 
                {
                    MenuProveedor(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }
                else if (rolAsignado == "Caja")
                {
                    MenuCaja(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                }
            }
            else
            {
                Console.WriteLine("Error: Usuario o contraseña incorrectos.");
                Console.ReadKey();
            }
        }

      

        static void MenuAdministrador(List<string> p, List<int> c, List<double> pr, string arc, Stack<double> pres)
        {
            Console.WriteLine(" MENÚ ADMINISTRADOR");
            Console.WriteLine("1. Ver Inventario");
            Console.WriteLine("2. Comprar Productos (Gestionar)");
            Console.WriteLine("3. Realizar Venta (Salida)");
            Console.WriteLine("4. Salir");
            Console.Write("Opción: ");
            string op = Console.ReadLine();

            if (op == "1") Inventario(p, c, pr, pres);
            else if (op == "2") CompraProducto(p, c, pr, arc, pres);
            else if (op == "3") VenderProductos(p, c, pr, arc, pres);
        }

        static void MenuProveedor(List<string> p, List<int> c, List<double> pr, string arc, Stack<double> pres)
        {
            Console.WriteLine(" MENÚ PROVEEDOR / ALMACÉN");
            Console.WriteLine("1. Ver Inventario (Pedidos)");
            Console.WriteLine("2. Cargar Stock de Productos");
            Console.WriteLine("3. Salir");
            Console.Write("Opción: ");
            string op = Console.ReadLine();

            if (op == "1") Inventario(p, c, pr, pres);
            else if (op == "2") CompraProducto(p, c, pr, arc, pres);
        }

        static void MenuCaja(List<string> p, List<int> c, List<double> pr, string arc, Stack<double> pres)
        {
            Console.WriteLine(" MENÚ DE CAJA ");
            Console.WriteLine("1. Realizar Venta");
            Console.WriteLine("2. Consultar Inventario y Precios");
            Console.WriteLine("3. Salir");
            Console.Write("Opción: ");
            string op = Console.ReadLine();

            if (op == "1") VenderProductos(p, c, pr, arc, pres);
            else if (op == "2") Inventario(p, c, pr, pres);
        }

        // ============ FUNCIONES ORIGINALES (SIN CAMBIOS) ===============

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

        static void CompraProducto(List<string> productos, List<int> cantidadProd, List<double> precioProd, string archivoInventario, Stack<double> presupuesto)
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
                    Console.WriteLine("SAlDO ACTUAL: " + saldoActual);

                    Console.Write("\nIngrese el producto: ");
                    producto = Console.ReadLine().ToLower();
                    int indice = productos.IndexOf(producto);
                    if (indice != -1)
                    {
                        Console.WriteLine($"\nEl producto ya se encuentra en el inventario {producto}. Precio actual: {precioProd[indice]}");
                        Console.Write("\nIngrese la cantidad de unidades que desee agregar: ");
                        cantidad = int.Parse(Console.ReadLine());
                        precio = precioProd[indice];
                        for (int i = 0; i < cantidad; i++) { compraTotal += precio; }

                        if (compraTotal >= saldoActual)
                        {
                            Console.WriteLine("El valor de la compra es igual o sobrepasa el presupuesto...");
                            compraTotal = 0; Console.ReadKey(); continue;
                        }
                        else
                        {
                            Console.WriteLine("\nla Compra ha sido exitosa.");
                            tipo = "Gasto"; saldoProvicional -= compraTotal;
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
                        for (int i = 0; i < cantidad; i++) { compraTotal += precio; }

                        if (compraTotal >= saldoActual)
                        {
                            Console.WriteLine("El valor de la compra sobrepasa el presupuesto...");
                            compraTotal = 0; Console.ReadKey(); continue;
                        }
                        else
                        {
                            Console.WriteLine("\nla Compra ha sido exitosa.");
                            saldoProvicional -= compraTotal; tipo = "Gasto";
                        }
                        Console.WriteLine("SALDO ACTUAL: " + saldoProvicional);
                        productos.Add(producto); precioProd.Add(precio); cantidadProd.Add(cantidad);
                    }

                    GuardarInventario(productos, precioProd, cantidadProd, archivoInventario);
                    RegistrarMovimiento(presupuesto, tipo, compraTotal);

                    do
                    {
                        Console.WriteLine("¿Desea comprar otro producto? (si/no)");
                        addProduct = Console.ReadLine().ToLower();
                    } while (!(addProduct == "si" || addProduct == "no"));
                } while (addProduct == "si");
                if (addProduct == "no") salir = true;
            }
            Console.Clear();
        }

        static void Inventario(List<string> productos, List<int> cantidadProd, List<double> precioProd, Stack<double> presupuesto)
        {
            Console.Clear();
            double saldo = presupuesto.Peek();
            Console.SetCursorPosition(5, 0);
            Console.WriteLine($"SALDO ACTUAL: {saldo}");
            for (int i = 0; i < productos.Count; i++)
            {
                int x = 5 + (20 * i); int y = 2;
                Console.SetCursorPosition(x, y); Console.Write($"PRODUCTO {i + 1}");
                Console.SetCursorPosition(x, y + 1); Console.Write($"Producto: {productos[i].ToUpper()}");
                Console.SetCursorPosition(x, y + 2); Console.Write($"Cantidad: {cantidadProd[i]}");
                Console.SetCursorPosition(x, y + 3); Console.Write($"Precio: {precioProd[i] / 0.8}");
            }
            Console.SetCursorPosition(5, 10);
            Console.Write("Presione cualquier tecla para volver...");
            Console.ReadKey();
            Console.Clear();
        }

        static void VenderProductos(List<string> productos, List<int> cantidadProd, List<double> precioProd, string archivoInventario, Stack<double> presupuesto)
        {
            string continuar = "";
            do
            {
                double saldoActual = presupuesto.Peek();
                Console.Clear();
                Console.WriteLine("========== Venta de productos ========");
                Console.WriteLine($"Saldo Actual: {saldoActual:C}");
                Console.Write("\nIngrese el nombre del producto: ");
                string buscarProd = Console.ReadLine().ToLower();
                int indice = productos.IndexOf(buscarProd);

                if (indice != -1)
                {
                    Console.WriteLine($"Producto: {productos[indice]} | Stock: {cantidadProd[indice]}");
                    Console.Write("Unidades a vender: ");
                    if (int.TryParse(Console.ReadLine(), out int cantVenta) && cantVenta <= cantidadProd[indice] && cantVenta > 0)
                    {
                        double totalVenta = cantVenta * precioProd[indice] / 0.8;
                        cantidadProd[indice] -= cantVenta;
                        RegistrarMovimiento(presupuesto, "Ingreso", totalVenta);
                        GuardarInventario(productos, precioProd, cantidadProd, archivoInventario);
                        Console.WriteLine($"\nVenta exitosa. Total: {totalVenta:C}");
                    }
                    else { Console.WriteLine("Cantidad no válida."); }
                }
                else { Console.WriteLine("El producto no existe."); }

                Console.Write("\n¿Desea registrar otro producto? (si/no): ");
                continuar = Console.ReadLine().ToLower();
            } while (continuar == "si" || continuar == "s");
        }

        static void GuardarInventario(List<string> producto, List<double> precio, List<int> cantidad, string archivoInventario)
        {
            List<string> lineas = new List<string> { "Producto;Precio;Cantidad" };
            for (int i = 0; i < producto.Count; i++) { lineas.Add($"{producto[i]};{precio[i]};{cantidad[i]}"); }
            File.WriteAllLines(archivoInventario, lineas);
        }

        static void RecargarProductos(List<string> producto, List<double> precio, List<int> cantidad, string archivoInventario)
        {
            if (!File.Exists(archivoInventario)) return;
            string[] lineas = File.ReadAllLines(archivoInventario);
            for (int i = 1; i < lineas.Length; i++)
            {
                string[] ProduDatos = lineas[i].Split(';');
                producto.Add(ProduDatos[0]); precio.Add(double.Parse(ProduDatos[1])); cantidad.Add(int.Parse(ProduDatos[2]));
            }
        }

        static string LeerPassword()
        {
            string pass = ""; ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass.Substring(0, (pass.Length - 1)); Console.Write("\b \b");
                }
                else if (key.Key != ConsoleKey.Enter && !char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar; Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine(); return pass;
        }

        static void CargarPresupuesto(Stack<double> pila)
        {
            string archivoCostos = "costos_e_ingresos.csv";
            if (!File.Exists(archivoCostos)) { pila.Push(100000); return; }
            string[] lineas = File.ReadAllLines(archivoCostos);
            foreach (string linea in lineas)
            {
                string[] datos = linea.Split(';');
                pila.Push(double.Parse(datos[2]));
            }
        }

        static void RegistrarMovimiento(Stack<double> pila, string tipo, double monto)
        {
            double saldoActual = pila.Peek();
            double nuevoSaldo = (tipo == "Ingreso") ? saldoActual + monto : saldoActual - monto;
            pila.Push(nuevoSaldo);
            File.AppendAllText("costos_e_ingresos.csv", $"{tipo};{monto};{nuevoSaldo}" + Environment.NewLine);
        }

        static string[] InicializarUsuarios()
        {
            string archivoUsuarios = "usuarios.txt";
            if (File.Exists(archivoUsuarios)) return File.ReadAllLines(archivoUsuarios);
            string[] contraseñas = RegistroUser();
            File.WriteAllLines(archivoUsuarios, contraseñas);
            return contraseñas;
        }
    }
}
