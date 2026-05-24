using System;
using System.Collections.Generic;
using System.IO;

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Stack<double> presupuesto = new Stack<double>();
        CargarPresupuesto(presupuesto);

        string archivoInventario = "inventario.csv";
        List<string> productos = new List<string>();
        List<int> cantidadProd = new List<int>();
        List<double> precioProd = new List<double>();
        RecargarProductos(productos, precioProd, cantidadProd, archivoInventario);

        // NUEVAS LISTAS PARALELAS DE USUARIOS
        string archivoUsuarios = "usuarios.txt";
        List<string> usuarios = new List<string>();
        List<string> contraseñas = new List<string>();
        List<string> roles = new List<string>();


        CargarUsuarios(usuarios, contraseñas, roles, archivoUsuarios);
        MenuPrincipal(usuarios, contraseñas, roles, productos, cantidadProd, precioProd, archivoInventario, presupuesto, archivoUsuarios);
    }

    //----------------------------------------------------------------------------------------
    static void MenuPrincipal(List<string> usuarios, List<string> contraseñas, List<string> roles, List<string> productos,
                              List<int> cantidadProd, List<double> precioProd, string archivoInventario, Stack<double> presupuesto, string archivoUsuarios)
    {
        Console.Clear();
        bool accesoConcedido = false;
        bool esCorrecto = true;

        while (!accesoConcedido)
        {
            int ancho = 40;
            int alto = 20;
            int rangex = (Console.WindowWidth / 2) - (ancho / 2);
            int rangey = (Console.WindowHeight / 2) - (alto / 2);


            for (int x = 1; x < ancho; x++)
            {
                Console.SetCursorPosition(rangex + x, rangey);
                Console.Write("─"); // Línea superior

                Console.SetCursorPosition(rangex + x, rangey + alto);
                Console.Write("─"); // Línea inferior
            }


            for (int y = 1; y < alto; y++)
            {
                Console.SetCursorPosition(rangex, rangey + y);
                Console.Write("│"); // Línea izquierda

                Console.SetCursorPosition(rangex + ancho, rangey + y);
                Console.Write("│"); // Línea derecha
            }

            Console.SetCursorPosition(rangex, rangey);
            Console.Write("┌"); // Esquina superior izquierda

            Console.SetCursorPosition(rangex + ancho, rangey);
            Console.Write("┐"); // Esquina superior derecha

            Console.SetCursorPosition(rangex, rangey + alto);
            Console.Write("└"); // Esquina inferior izquierda

            Console.SetCursorPosition(rangex + ancho, rangey + alto);
            Console.Write("┘"); // Esquina inferior derecha


            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(rangex + 8, rangey + 2);
            Console.Write("Bienvenidos al MiniMercado");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(rangex + 17, rangey + 5);
            Console.Write("Usuario");
            for (int x = 1; x < ancho / 2; x++)
            {
                Console.SetCursorPosition(rangex + 10 + x, rangey + 8);
                Console.Write("─"); // Línea usuario
            }
            Console.SetCursorPosition(rangex + 15, rangey + 10);
            Console.Write("Contraseña");
            for (int x = 1; x < ancho / 2; x++)
            {
                Console.SetCursorPosition(rangex + 10 + x, rangey + 13);
                Console.Write("─"); // Línea contraseña
            }
            if (!esCorrecto)
            {
                Console.SetCursorPosition(rangex + 10, rangey + 15);
                Console.Write("Dato(s) incorrecto(s).");
            }
            Console.SetCursorPosition(rangex + 11, rangey + 7);
            string usuarioIngresado = Console.ReadLine().Trim();

            Console.SetCursorPosition(rangex + 11, rangey + 12);
            string claveIngresada = LeerPassword();

            string rolAsignado = "";
            int indiceUsuario = usuarios.FindIndex(u => u.ToLower() == usuarioIngresado.ToLower());

            // Validamos si el usuario existe y si la contraseña coincide en el mismo índice
            if (indiceUsuario != -1 && contraseñas[indiceUsuario] == claveIngresada)
            {
                accesoConcedido = true;
                rolAsignado = roles[indiceUsuario];
                usuarioIngresado = usuarios[indiceUsuario]; // Para mantener el formato original de mayúsculas/minúsculas
            }

            if (accesoConcedido)
            {
                Console.Clear();
                if (rolAsignado == "Administrador")
                {
                    MenuAdministrador(usuarios, contraseñas, roles, productos, cantidadProd, precioProd, archivoInventario, presupuesto, archivoUsuarios);
                }
                else if (rolAsignado == "Almacen")
                {
                    MenuProveedor(usuarios, contraseñas, roles, productos, cantidadProd, precioProd, archivoInventario, presupuesto, archivoUsuarios);
                }
                else if (rolAsignado == "Caja")
                {
                    MenuCaja(usuarios, contraseñas, roles, productos, cantidadProd, precioProd, archivoInventario, presupuesto, archivoUsuarios);
                }
            }
            else
            {
                esCorrecto = false;
                Console.Clear();

            }
        }
    }

    //----------------------------------------------------------------------------------------
    static void MenuAdministrador(List<string> usuarios, List<string> contraseñas, List<string> roles, List<string> productos,
                                  List<int> cantidadProd, List<double> precioProd, string archivoInventario, Stack<double> presupuesto, string archivoUsuarios)
    {
        bool volver = false;
        int ancho = 32;
        int alto = 20;

        while (!volver)
        {
            Console.Clear();
            if (productos.Count != 0)
            {
                for (int i = 0; i < productos.Count; i++)
                {
                    if (cantidadProd[i] <= 5)
                    {
                        AlertaInventario(cantidadProd);
                        break;
                    }
                }
            }

            int rangex = (Console.WindowWidth / 2) - (ancho / 2);
            int rangey = (Console.WindowHeight / 2) - (alto / 2);

            Console.SetCursorPosition(rangex, rangey);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" === MENÚ DE ADMINISTRADOR === ");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(rangex, rangey + 1);
            Console.Write("1. Ver Inventario");

            Console.SetCursorPosition(rangex, rangey + 2);
            Console.Write("2. Comprar Productos");

            Console.SetCursorPosition(rangex, rangey + 3);
            Console.Write("3. Realizar Venta");

            Console.SetCursorPosition(rangex, rangey + 4);
            Console.Write("4. Editar Inventario");

            Console.SetCursorPosition(rangex, rangey + 5);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("5. Crear un usuario nuevo");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(rangex, rangey + 6);
            Console.Write("6. Eliminar un usuario");
            Console.ForegroundColor = ConsoleColor.Red;

            Console.SetCursorPosition(rangex, rangey + 7);
            Console.Write("7. Salir");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(rangex + 10, rangey + 9);
            Console.Write("Opción");
            for (int x = 0; x < 16; x++)
            {
                Console.SetCursorPosition(rangex + 5 + x, rangey + 11);
                Console.Write("─"); // Línea opcion
            }
            Console.SetCursorPosition(rangex + 5, rangey + 10);
            string op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    Inventario(productos, cantidadProd, precioProd, presupuesto);
                    break;
                case "2":
                    CompraProducto(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                    break;
                case "3":
                    VenderProductos(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
                    break;
                case "4":
                    EditarInventario(productos, cantidadProd, precioProd, presupuesto, archivoInventario);
                    break;
                case "5":
                    RegistrarNuevoUsuario(usuarios, contraseñas, roles, archivoUsuarios);
                    break;
                case "6":
                    EliminarUsuario(usuarios, contraseñas, roles, archivoUsuarios);
                    break;
                case "7":
                    volver = true;
                    Salir(usuarios, contraseñas, roles, productos, cantidadProd, precioProd,
                        archivoInventario, presupuesto, archivoUsuarios);
                    break;
                default:
                    Console.Clear();
                    break;
            }
        }
    }

    //----------------------------------------------------------------------------------------
    static void MenuProveedor(List<string> usuarios, List<string> contraseñas, List<string> roles, List<string> productos,
                              List<int> cantidadProd, List<double> precioProd, string archivoInventario, Stack<double> presupuesto, string archivoUsuarios)
    {
        bool volver = false;
        int ancho = 28;
        int alto = 20;

        while (!volver)
        {
            Console.Clear();
            if (productos.Count != 0)
            {
                for (int i = 0; i < productos.Count; i++)
                {
                    if (cantidadProd[i] <= 5)
                    {
                        AlertaInventario(cantidadProd);
                        break;
                    }
                }
            }

            int rangex = (Console.WindowWidth / 2) - (ancho / 2);
            int rangey = (Console.WindowHeight / 2) - (alto / 2);

            Console.SetCursorPosition(rangex, rangey);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" === MENÚ DE ALMACÉN === ");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(rangex, rangey + 1);
            Console.WriteLine("1. Ver Inventario ");

            Console.SetCursorPosition(rangex, rangey + 2);
            Console.WriteLine("2. Cargar Stock de Productos");

            Console.SetCursorPosition(rangex, rangey + 3);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("3. Salir");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(rangex + 10, rangey + 5);
            Console.Write("Opción");
            for (int x = 0; x < 16; x++)
            {
                Console.SetCursorPosition(rangex + 5 + x, rangey + 7);
                Console.Write("─"); // Línea opcion
            }
            Console.SetCursorPosition(rangex + 5, rangey + 6);
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
                Salir(usuarios, contraseñas, roles, productos, cantidadProd, precioProd, archivoInventario, presupuesto, archivoUsuarios);
            }
            else
            {
                Console.Clear();
            }
        }
    }

    //----------------------------------------------------------------------------------------
    static void MenuCaja(List<string> usuarios, List<string> contraseñas, List<string> roles, List<string> productos,
                         List<int> cantidadProd, List<double> precioProd, string archivoInventario, Stack<double> presupuesto, string archivoUsuarios)
    {
        bool volver = false;
        int ancho = 24;
        int alto = 20;
        while (!volver)
        {
            Console.Clear();
            if (productos.Count != 0)
            {
                for (int i = 0; i < productos.Count; i++)
                {
                    if (cantidadProd[i] <= 5)
                    {
                        AlertaInventario(cantidadProd);
                        break;
                    }
                }
            }
            int rangex = (Console.WindowWidth / 2) - (ancho / 2);
            int rangey = (Console.WindowHeight / 2) - (alto / 2);

            Console.SetCursorPosition(rangex, rangey);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" === MENÚ DE CAJA === ");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(rangex, rangey + 1);
            Console.WriteLine("1. Realizar Venta");

            Console.SetCursorPosition(rangex, rangey + 2);
            Console.WriteLine("2. Consultar Inventario");

            Console.SetCursorPosition(rangex, rangey + 3);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("3. Salir");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(rangex + 10, rangey + 5);
            Console.Write("Opción");
            for (int x = 0; x < 16; x++)
            {
                Console.SetCursorPosition(rangex + 5 + x, rangey + 7);
                Console.Write("─"); // Línea opcion
            }
            Console.SetCursorPosition(rangex + 5, rangey + 6);
            string op = Console.ReadLine();

            if (op == "1")
            {
                VenderProductos(productos, cantidadProd, precioProd, archivoInventario, presupuesto);
            }
            else if (op == "2")
            {
                Inventario(productos, cantidadProd, precioProd, presupuesto);
            }
            else if (op == "3")
            {
                volver = true;
                Salir(usuarios, contraseñas, roles, productos, cantidadProd, precioProd, archivoInventario, presupuesto, archivoUsuarios);
            }
            else
            {
                Console.Clear();
            }
        }
    }

    // ============ FUNCIONES PRINCIPALES ===============
    static void CompraProducto(List<string> productos, List<int> cantidadProd,
                                List<double> precioProd, string archivoInventario, Stack<double> presupuesto)
    {
        double precio, compraTotal = 0, saldoActual = presupuesto.Peek();
        string addProduct = "", producto, tipo = "Gasto";
        int cantidad;
        bool salir = false;
        double saldoProvicional = saldoActual;
        List<string> compraNombres = new List<string>();
        List<int> compraCantidades = new List<int>();
        List<double> compraPrecios = new List<double>();
        while (!salir)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("=== COMPRA DE PRODUCTOS ===");
                Console.WriteLine($"SALDO ACTUAL: {saldoProvicional:C}");

                Console.Write("\nIngrese el producto: ");
                producto = Console.ReadLine().ToLower();
                int indice = productos.IndexOf(producto);
                if (indice != -1)
                {
                    
                    Console.WriteLine($"\nEl producto ya se encuentra en el inventario {producto}. Precio actual: {precioProd[indice]}");
                    Console.Write("\nIngrese la cantidad de unidades que desee agregar: ");
                    while (!int.TryParse(Console.ReadLine(), out cantidad) || cantidad < 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Error: Ingrese una cantidad válida (número entero positivo): ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    precio = precioProd[indice];
                    compraTotal = cantidad * precio;

                    if (compraTotal >= saldoProvicional)
                    {
                        Console.WriteLine("El valor de la compra es igual o sobrepasa el presupuesto...");
                        compraTotal = 0;
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("\nLa compra ha sido exitosa.");
                        tipo = "Gasto";
                        saldoProvicional -= compraTotal;
                        compraNombres.Add(producto);
                        compraCantidades.Add(cantidad);
                        compraPrecios.Add(precio);
                    }
                    Console.WriteLine($"SALDO ACTUAL: {saldoProvicional:C}");
                    cantidadProd[indice] += cantidad;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(producto))
                    {
                        Console.Write("\n Error: No puede estar vacío.\n...");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                    Console.WriteLine("\n¡¡PRODUCTO NUEVO!!");
                    Console.Write("\nIngrese el precio por unidad: ");
                    while (!double.TryParse(Console.ReadLine(), out precio) || precio < 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Error: Ingrese una cantidad válida (número entero positivo): ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write("\nIngrese la cantidad de unidades que desee comprar: ");
                    while (!int.TryParse(Console.ReadLine(), out cantidad) || cantidad < 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Error: Ingrese una cantidad válida (número entero positivo): ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    compraTotal = cantidad * precio;

                    if (compraTotal >= saldoProvicional)
                    {
                        Console.WriteLine("El valor de la compra sobrepasa el presupuesto...");
                        compraTotal = 0;
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("\nLa compra ha sido exitosa.");
                        saldoProvicional -= compraTotal;
                        tipo = "Gasto";
                        compraNombres.Add(producto);
                        compraCantidades.Add(cantidad);
                        compraPrecios.Add(precio);
                    }
                    Console.WriteLine($"SALDO ACTUAL: {saldoProvicional:C}");
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
        
        if (compraNombres.Count > 0)
        {
            GenerarFacturaCompra(compraNombres, compraCantidades, compraPrecios);
        }

        return;
    }
    // ============================================================================
    static void GenerarFacturaCompra(List<string> nombres, List<int> cantidades, List<double> precios)
    {
        Console.Clear();
        double totalInversion = 0;
        

        Console.WriteLine("============================================");
        Console.WriteLine("          FACTURA DE COMPRA       ");
        Console.WriteLine("============================================");
        Console.WriteLine("{0,-20} {1,-7} {2,-12}", "PRODUCTO", "CANT.", "COSTO UNIT.");
        Console.WriteLine("--------------------------------------------");

        for (int i = 0; i < nombres.Count; i++)
        {
            double subtotal = cantidades[i] * precios[i];
            totalInversion += subtotal;
            Console.WriteLine("{0,-20} {1,-7} {2,-12:C}", nombres[i].ToUpper(), cantidades[i], precios[i]);
        }

        Console.WriteLine("--------------------------------------------");
        Console.WriteLine($"TOTAL INVERTIDO:              {totalInversion,12:C}");
        Console.WriteLine("============================================");
        Console.WriteLine("\n¡Inventario reabastecido con éxito!");
        Console.WriteLine("\nPresione cualquier tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }



    //----------------------------------------------------------------------------------------
    static void Inventario(List<string> productos, List<int> cantidadProd, List<double> precioProd, Stack<double> presupuesto)
    {
        Console.Clear();
        double saldo = presupuesto.Peek();
        Console.SetCursorPosition(5, 0);
        Console.WriteLine($"SALDO ACTUAL: {saldo:C}");

        int productosPorFila = 4;

        for (int i = 0; i < productos.Count; i++)
        {
            int columna = i % productosPorFila;
            int fila = i / productosPorFila;

            int x = 5 + (25 * columna);
            int y = 2 + (fila * 6);

            Console.SetCursorPosition(x, y);
            Console.Write($"--- PRODUCTO {i + 1} ---");
            Console.SetCursorPosition(x, y + 1);

            Console.Write($"Nombre: {productos[i].ToUpper()}");
            Console.SetCursorPosition(x, y + 2);
            if (cantidadProd[i] <= 5)
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
            Console.WriteLine("\nERROR: No es posible realizar ventas. Inventario vacío.");
            Console.ReadKey();
            return;
        }

        List<string> carritoNombres = new List<string>();
        List<int> carritoCantidades = new List<int>();
        List<double> carritoPreciosUnitarios = new List<double>();

        string continuar = "";
        Console.Clear();
        Console.WriteLine("========== REGISTRO DE VENTA ========");

        do
        {
            Console.Write("\nIngrese el nombre del producto: ");
            string buscarProd = Console.ReadLine().ToLower();
            int indice = productos.IndexOf(buscarProd);

            if (indice != -1)
            {
                double priceVentaUnitario = precioProd[indice] / 0.8;
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
                Console.WriteLine($"-> Precio unidad: {priceVentaUnitario:C}");

                Console.Write("   Unidades a vender: ");
                if (int.TryParse(Console.ReadLine(), out int cantVenta) && cantVenta > 0)
                {
                    if (cantVenta <= cantidadProd[indice])
                    {
                        carritoNombres.Add(productos[indice]);
                        carritoCantidades.Add(cantVenta);
                        carritoPreciosUnitarios.Add(priceVentaUnitario);

                        cantidadProd[indice] -= cantVenta;

                        double subtotal = cantVenta * priceVentaUnitario;
                        RegistrarMovimiento(presupuesto, "Ingreso", subtotal);
                        GuardarInventario(productos, precioProd, cantidadProd, archivoInventario);

                        Console.WriteLine($">> OK: {productos[indice].ToUpper()} añadido al carrito.");
                        Console.WriteLine($">> Agregado: {cantVenta} x {productos[indice]} = {subtotal:C}");
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Stock insuficiente.");
                    }
                }
            }
            else
            {
                Console.WriteLine("ERROR: El producto no existe.");
            }

            while (true)
            {
                Console.Write("\n¿Desea agregar otro producto al carrito? (si/no): ");
                continuar = Console.ReadLine().ToLower();
                if (continuar == "si" || continuar == "s" || continuar == "no" || continuar == "n") break;
                Console.WriteLine("Respuesta no válida.");
            }

        } while (continuar == "si" || continuar == "s");

        if (carritoNombres.Count > 0)
        {
            GenerarFacturaFinal(carritoNombres, carritoCantidades, carritoPreciosUnitarios);
        }
    }

    static void GenerarFacturaFinal(List<string> nombres, List<int> cantidades, List<double> precios)
    {
        Console.Clear();
        double totalFinal = 0;

        Console.WriteLine("============================================");
        Console.WriteLine("              FACTURA DE VENTA               ");
        Console.WriteLine("============================================");
        Console.WriteLine("{0,-20} {1,-7} {2,-12}", "PRODUCTO", "CANT.", "SUBTOTAL");
        Console.WriteLine("--------------------------------------------");

        for (int i = 0; i < nombres.Count; i++)
        {
            double subtotal = cantidades[i] * precios[i];
            totalFinal += subtotal;
            Console.WriteLine("{0,-20} {1,-7} {2,-12:C}", nombres[i].ToUpper(), cantidades[i], subtotal);
        }

        Console.WriteLine("--------------------------------------------");
        Console.WriteLine($"TOTAL A PAGAR:                {totalFinal,12:C}");
        Console.WriteLine("============================================");
        Console.WriteLine("\n¡Gracias por su compra!");
        Console.WriteLine("\nPresione cualquier tecla para finalizar...");
        Console.ReadKey();
        Console.Clear();
    }

    
    

    // ============ FUNCIONES AUXILIARES ============
    static void GuardarInventario(List<string> producto, List<double> precio, List<int> cantidad, string archivoInventario)
    {
        List<string> lineas = new List<string> { "Producto;Precio Compra;Precio Caja;Cantidad" };
        for (int i = 0; i < producto.Count; i++)
        {
            lineas.Add($"{producto[i]};{precio[i]};{precio[i] / 0.8};{cantidad[i]}");
        }
        File.WriteAllLines(archivoInventario, lineas);
    }

    //----------------------------------------------------------------------------------------
    static void RecargarProductos(List<string> producto, List<double> precio, List<int> cantidad, string archivoInventario)
    {
        if (!File.Exists(archivoInventario)) return;
        string[] lineas = File.ReadAllLines(archivoInventario);
        for (int i = 1; i < lineas.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lineas[i])) continue;
            string[] ProduDatos = lineas[i].Split(';');
            if (ProduDatos.Length >= 4)
            {
                producto.Add(ProduDatos[0]);
                precio.Add(double.Parse(ProduDatos[1]));
                cantidad.Add(int.Parse(ProduDatos[3]));
            }
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
            if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
            {
                pass = pass.Substring(0, (pass.Length - 1));
                Console.Write("\b \b");
            }
            else if (key.Key != ConsoleKey.Enter && !char.IsControl(key.KeyChar))
            {
                pass += key.KeyChar;
                Console.Write("*");
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
            for (int i = 1; i < lineas.Length; i++)
            {
                string[] datos = lineas[i].Split(';');
                if (datos.Length >= 3)
                {
                    pila.Push(double.Parse(datos[2]));
                }
            }
        }
        else
        {
            File.AppendAllText(archivoCostos, "Tipo;Monto;Saldo Nuevo" + Environment.NewLine);
        }

        if (pila.Count == 0) pila.Push(800000);
    }

    //----------------------------------------------------------------------------------------
    static void RegistrarMovimiento(Stack<double> pila, string tipo, double monto)
    {
        double saldoActual = pila.Peek();
        double nuevoSaldo = (tipo == "Ingreso") ? saldoActual + monto : saldoActual - monto;
        pila.Push(nuevoSaldo);
        File.AppendAllText("costos_e_ingresos.csv", $"{tipo};{monto};{nuevoSaldo}" + Environment.NewLine);
    }

    //----------------------------------------------------------------------------------------
    static void GuardarUsuariosEnArchivo(List<string> usuarios, List<string> contraseñas, List<string> roles, string archivoUsuarios)
    {
        List<string> lineas = new List<string>();
        for (int i = 0; i < usuarios.Count; i++)
        {
            lineas.Add($"{usuarios[i]};{contraseñas[i]};{roles[i]}");
        }
        File.WriteAllLines(archivoUsuarios, lineas);
    }

    //----------------------------------------------------------------------------------------
    static void CargarUsuarios(List<string> usuarios, List<string> contraseñas, List<string> roles, string archivoUsuarios)
    {
        if (!File.Exists(archivoUsuarios))
        {
            // Inicialización por defecto en listas paralelas si el archivo no existe
            usuarios.Add("admin");
            contraseñas.Add("1234");
            roles.Add("Administrador");

            usuarios.Add("almacen1");
            contraseñas.Add("1234");
            roles.Add("Almacen");

            usuarios.Add("caja1");
            contraseñas.Add("1234");
            roles.Add("Caja");
            GuardarUsuariosEnArchivo(usuarios, contraseñas, roles, archivoUsuarios);
            return;
        }

        string[] lineas = File.ReadAllLines(archivoUsuarios);
        foreach (string linea in lineas)
        {
            if (string.IsNullOrWhiteSpace(linea)) continue;
            string[] datos = linea.Split(';');
            if (datos.Length == 3)
            {
                usuarios.Add(datos[0]);
                contraseñas.Add(datos[1]);
                roles.Add(datos[2]);
            }
        }
    }

    //----------------------------------------------------------------------------------------
    static void Salir(List<string> usuarios, List<string> contraseñas, List<string> roles, List<string> productos,
                      List<int> cantidadProd, List<double> precioProd, string archivoInventario, Stack<double> presupuesto, string archivoUsuarios)
    {
        Console.Clear();
        string salir;
        do
        {
            Console.WriteLine("¿DESEA CERRAR SESIÓN Y VOLVER AL LOGIN? (si o no)");
            salir = Console.ReadLine().ToLower();
            if (salir == "si")
            {
                // Cerramos sesión devolviendo al Login
                MenuPrincipal(usuarios, contraseñas, roles, productos, cantidadProd, precioProd, archivoInventario, presupuesto, archivoUsuarios);
                return;
            }
            else if (salir != "no")
            {
                Console.Write("Ingrese un valor válido (si/no)\n...");
                Console.ReadKey();
                Console.Clear();
            }
        } while (salir != "si" && salir != "no");
    }

    //----------------------------------------------------------------------------------------
    static void RegistrarNuevoUsuario(List<string> usuarios, List<string> contraseñas, List<string> roles, string archivoUsuarios)
    {
        Console.Clear();
        Console.WriteLine("========== REGISTRO DE NUEVO USUARIO ========");

        Console.Write("Ingrese el nombre del nuevo usuario (ej: pepito22): ");
        string nuevoUsuario = Console.ReadLine().Trim();

        if (string.IsNullOrWhiteSpace(nuevoUsuario))
        {
            Console.WriteLine("\nError: El nombre de usuario no puede estar vacío.");
            Console.ReadKey();
            return;
        }

        int existe = usuarios.FindIndex(u => u.ToLower() == nuevoUsuario.ToLower());
        if (existe != -1)
        {
            Console.WriteLine("\nError: El nombre de usuario ya está en uso. Intente con otro.");
            Console.ReadKey();
            return;
        }

        string rolSeleccionado = "";
        bool rolValido = false;
        while (!rolValido)
        {
            Console.WriteLine("\nSeleccione el rol para este usuario:");
            Console.WriteLine("1. Administrador");
            Console.WriteLine("2. Almacen");
            Console.WriteLine("3. Caja");
            Console.Write("Opción (1-3): ");
            string opRol = Console.ReadLine();

            if (opRol == "1") { rolSeleccionado = "Administrador"; rolValido = true; }
            else if (opRol == "2") { rolSeleccionado = "Almacen"; rolValido = true; }
            else if (opRol == "3") { rolSeleccionado = "Caja"; rolValido = true; }
            else { Console.WriteLine("Opción inválida. Seleccione un número del 1 al 3."); }
        }

        string pass1 = "", pass2 = "";
        bool claveValida = false;
        while (!claveValida)
        {
            Console.Write($"\nAsigne una contraseña para {nuevoUsuario}: ");
            pass1 = LeerPassword();
            Console.Write("Confirme la contraseña: ");
            pass2 = LeerPassword();

            if (!string.IsNullOrWhiteSpace(pass1) && pass1 == pass2)
            {
                claveValida = true;
            }
            else
            {
                Console.WriteLine("\nLas contraseñas no coinciden o están vacías. Intente de nuevo.");
            }
        }

        usuarios.Add(nuevoUsuario);
        contraseñas.Add(pass1);
        roles.Add(rolSeleccionado);

        GuardarUsuariosEnArchivo(usuarios, contraseñas, roles, archivoUsuarios);

        Console.WriteLine($"\nUsuario '{nuevoUsuario}' creado con éxito con el rol de '{rolSeleccionado}'.");
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    //----------------------------------------------------------------------------------------
    static void EliminarUsuario(List<string> usuarios, List<string> contraseñas, List<string> roles, string archivoUsuarios)
    {
        Console.Clear();
        Console.WriteLine("========== ELIMINAR USUARIO ========");

        Console.WriteLine("Usuarios actuales en el sistema:");
        for (int i = 0; i < usuarios.Count; i++)
        {
            Console.WriteLine($"- {usuarios[i]} (Rol: {roles[i]})");
        }
        Console.WriteLine("-------------------------------------");

        Console.Write("\nIngrese el nombre del usuario a eliminar: ");
        string usuarioAEliminar = Console.ReadLine().Trim();

        int indice = usuarios.FindIndex(u => u.ToLower() == usuarioAEliminar.ToLower());

        if (indice != -1)
        {
            if (usuarios[indice].ToLower() == "admin")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError crítico: No se puede eliminar al Administrador maestro ('admin').");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
                return;
            }

            Console.Write($"¿Está seguro de que desea eliminar a '{usuarios[indice]}'? (si/no): ");
            string confirmar = Console.ReadLine().ToLower();

            if (confirmar == "si" || confirmar == "s")
            {
                string usuarioBorrado = usuarios[indice];

                usuarios.RemoveAt(indice);
                contraseñas.RemoveAt(indice);
                roles.RemoveAt(indice);

                GuardarUsuariosEnArchivo(usuarios, contraseñas, roles, archivoUsuarios);

                Console.WriteLine($"\nEl usuario '{usuarioBorrado}' ha sido removido del sistema.");
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }
        }
        else
        {
            Console.WriteLine("\n El usuario no existe en el sistema.");
        }

        Console.WriteLine("\nPresione cualquier tecla para regresar...");
        Console.ReadKey();
    }

    //----------------------------------------------------------------------------------------
    static void AlertaInventario(List<int> cantidadProd)
    {
        int ancho = 30;
        int alto = 10;
        int rangex = (Console.WindowWidth / 2) - (ancho / 2);
        int rangey = (Console.WindowHeight / 2) - (alto / 2);
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        for (int x = 1; x < ancho; x++)
        {
            Console.SetCursorPosition(rangex + x, rangey);
            Console.Write("─"); // Línea superior

            Console.SetCursorPosition(rangex + x, rangey + alto);
            Console.Write("─"); // Línea inferior
        }


        for (int y = 1; y < alto; y++)
        {
            Console.SetCursorPosition(rangex, rangey + y);
            Console.Write("│"); // Línea izquierda

            Console.SetCursorPosition(rangex + ancho, rangey + y);
            Console.Write("│"); // Línea derecha
        }

        Console.SetCursorPosition(rangex, rangey);
        Console.Write("┌"); // Esquina superior izquierda

        Console.SetCursorPosition(rangex + ancho, rangey);
        Console.Write("┐"); // Esquina superior derecha

        Console.SetCursorPosition(rangex, rangey + alto);
        Console.Write("└"); // Esquina inferior izquierda

        Console.SetCursorPosition(rangex + ancho, rangey + alto);
        Console.Write("┘"); // Esquina inferior derecha

        Console.SetCursorPosition(rangex + 10, rangey + 2);
        Console.Write("¡¡¡ALERTA!!!");
        Console.SetCursorPosition(rangex + 5, rangey + 4);
        Console.Write("Hay productos que estan");
        Console.SetCursorPosition(rangex + 10, rangey + 5);
        Console.Write("por acabarse");

        Console.SetCursorPosition(rangex + 9, rangey + 7);
        Console.Write("VOLVER AL MENÚ");


        Console.ReadKey();
        Console.Clear();
        Console.CursorVisible = true;
        return;

    }
    //----------------------------------------------------------------------------------------
    static void EditarInventario(List<string> productos, List<int> cantidadProd, List<double> precioProd,
        Stack<double> presupuesto, string archivoInventario)
    {
        Console.Clear();
        if (productos.Count == 0)
        {
            Console.Write("Inventario vacio...");
            Console.ReadKey();

            return;
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("EDICIÓN DE INVENTARIO");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Ingrese el nombre del producto que desee cambiar: ");
        string produc = Console.ReadLine().ToLower();
        int indice = productos.IndexOf(produc);
        double nvPrecio;
        string nvNombre;
        int nvCantidad;
        if (indice != -1)
        {
            Console.WriteLine($"El producto {produc} Está en el inventario");
            Console.WriteLine("1) Cambiar precio\n" +
                              "2) Cambiar Nombre\n" +
                              "3) Cambiar Cantidad");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("4) Eliminar Producto");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Opcion: ");
            string op = Console.ReadLine();
            Console.ReadKey();
            Console.Clear();


            if (op == "1")
            {
                Console.WriteLine($"Producto: {productos[indice]} ");
                Console.WriteLine($"Precio de compra: {precioProd[indice]} ");
                Console.WriteLine($"Precio en caja: {precioProd[indice] / 0.8} ");
                Console.Write("Ingrese el nuevo precio: ");
                nvPrecio = double.Parse(Console.ReadLine());
                precioProd[indice] = nvPrecio;
                GuardarInventario(productos, precioProd, cantidadProd, archivoInventario);
            }
            else if (op == "2")
            {
                Console.WriteLine($"Producto: {productos[indice]} ");
                Console.Write("Ingrese el nuevo Nombre: ");
                nvNombre = Console.ReadLine();
                productos[indice] = nvNombre;
                GuardarInventario(productos, precioProd, cantidadProd, archivoInventario);
            }
            else if (op == "3")
            {
                Console.WriteLine($"Producto: {productos[indice]} ");
                Console.WriteLine($"Cantidad: {cantidadProd[indice]} ");
                Console.Write("Ingrese el nuevo Nombre: ");
                while (!int.TryParse(Console.ReadLine(), out nvCantidad) || nvCantidad >= 0)
                {
                    Console.WriteLine("Error, ingre un número");
                }
                cantidadProd[indice] = nvCantidad;
                GuardarInventario(productos, precioProd, cantidadProd, archivoInventario);
            }
            else if (op == "4")
            {
                Console.WriteLine($"Producto: {productos[indice]} ");
                Console.WriteLine($"Cantidad: {cantidadProd[indice]} ");
                Console.WriteLine($"Precio de compra: {precioProd[indice]} ");
                Console.WriteLine($"Precio en caja: {precioProd[indice] / 0.8} ");
                productos.RemoveAt(indice);
                cantidadProd.RemoveAt(indice);
                precioProd.RemoveAt(indice);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Producto Eliminado");
                Console.ForegroundColor = ConsoleColor.White;
                GuardarInventario(productos, precioProd, cantidadProd, archivoInventario);
            }
            else
            {
                Console.Clear();
            }
        }
        else
        {
            Console.WriteLine("El Producto no existe");
        }
        Console.ReadKey();
        Console.Clear();
        return;

    }
}
