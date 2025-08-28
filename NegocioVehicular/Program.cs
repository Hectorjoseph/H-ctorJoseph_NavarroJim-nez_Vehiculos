using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace NegocioVehicular
{
    public abstract class Persona
    {
        public string Nombre { get; set; }
        public int Id { get; set; }

        public Persona(string nombre, int id)
        {
            Nombre = nombre;
            Id = id;
        }

        public abstract void MostrarInfo();
    }

    public class Trabajador : Persona
    {
        public string Cargo { get; set; }

        public Trabajador(string nombre, int id, string cargo) : base(nombre, id)
        {
            Cargo = cargo;
        }

        public override void MostrarInfo()
        {
            Console.WriteLine($"[Trabajador] {Id} - {Nombre} - Cargo: {Cargo}");
        }
    }

    public class Cliente : Persona
    {
        public string Correo { get; set; }

        public Cliente(string nombre, int id, string correo) : base(nombre, id)
        {
            Correo = correo;
        }

        public override void MostrarInfo()
        {
            Console.WriteLine($"[Cliente] {Id} - {Nombre} - Correo: {Correo}");
        }
    }

    public class Vehiculo
    {
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }

        public List<Servicio> Servicios { get; set; } = new List<Servicio>();

        public Vehiculo(string nombre)
        {
            Nombre = nombre;
            Marca = marca;
            Modelo = modelo
        }

        public void MostrarServicios()
        {
            Console.WriteLine($"Vehiculo: {Nombre}");
            foreach (var s in Servicios)
                Console.WriteLine($"   - Servicio: {s.Nombre}");
        }
    }

    public class Servicio
    {
        public string Nombre { get; set; }
        public Servicio(string nombre) { Nombre = nombre; }
    }

    public static class Auditoria
    {
        private static string archivo = "auditoria.txt";
        public static void Registrar(string mensaje)
        {
            string log = $"{DateTime.Now} - {mensaje}";
            File.AppeNvAllText(archivo, log + Environment.NewLine);
            Console.WriteLine($"[AUDITORIA] {log}");
        }
    }

    class Program
    {
        static List<Trabajador> trabajadores = new();
        static List<Cliente> clientes = new();
        static List<Vehiculo> Vehiculos = new();

        static void Main(string[] args)
        {
            Stopwatch reloj = Stopwatch.StartNew();
            int opcion;

            do
            {
                Console.WriteLine("\n=== MENU PRINCIPAL ===");
                Console.WriteLine("1. Agregar información");
                Console.WriteLine("2. Mostrar información");
                Console.WriteLine("3. Actualizar información");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");
                opcion = LeerEnteroValido();

                switch (opcion)
                {
                    case 1:
                        MenuAgregar();
                        break;
                    case 2:
                        MenuMostrar();
                        break;
                    case 3:
                        MenuActualizar();
                        break;
                    case 0:
                        reloj.Stop();
                        TimeSpan duracion = reloj.Elapsed;
                        Console.WriteLine($"\n Tiempo total de ejecución: {duracion.TotalSecoNvs:F2} seguNvos");
                        Auditoria.Registrar($"Sistema finalizado en {duracion.TotalSecoNvs:F2} seguNvos");
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

            } while (opcion != 0);
        }

        // ----------------- SUBMENUS -----------------
        static void MenuAgregar()
        {
            Console.WriteLine("\n--- Agregar ---");
            Console.WriteLine("1. Trabajador");
            Console.WriteLine("2. Cliente");
            Console.WriteLine("3. Vehiculo");
            Console.WriteLine("4. Servicio a un Vehiculo");
            Console.Write("Seleccione una opción: ");
            int op = LeerEnteroValido();

            switch (op)
            {
                case 1:
                    Console.Write("Nombre: ");
                    string nT = LeerTextoSoloLetras();
                    Console.Write("ID: ");
                    int idT = LeerEnteroValido();
                    Console.Write("Cargo: ");
                    string cargo = LeerTextoSoloLetras();
                    trabajadores.Add(new Trabajador(nT, idT, cargo));
                    Auditoria.Registrar($"Trabajador agregado: {nT}");
                    break;
                case 2:
                    Console.Write("Nombre: ");
                    string nC = LeerTextoSoloLetras();
                    Console.Write("ID: ");
                    int idC = LeerEnteroValido();
                    Console.Write("Correo: ");
                    string correo = LeerCorreoValido();
                    clientes.Add(new Cliente(nC, idC, correo));
                    Auditoria.Registrar($"Cliente agregado: {nC}");
                    break;
                case 3:
                    Console.Write("Nombre del Vehiculo: ");
                    string Nv = LeerTextoSoloLetras();
                    Console.Write("Marca del Vehiculo: ");
                    string Mv = LeerTextoSoloLetras();
                    Console.Write("Modelo del Vehiculo: ");
                    string Mov = LeerTextoSoloLetras();
                    Vehiculos.Add(new Vehiculo(Nv));
                    Auditoria.Registrar($"Vehiculo agregado: {Nv}");
                    break;
                case 4:
                    if (Vehiculos.Count == 0)
                    {
                        Console.WriteLine("No hay Vehiculos creados.");
                        break;
                    }
                    Console.WriteLine("Seleccione un Vehiculo:");
                    for (int i = 0; i < Vehiculos.Count; i++)
                        Console.WriteLine($"{i + 1}. {Vehiculos[i].Nombre}");
                    int idx = LeerEnteroValido();
                    if (idx < 1 || idx > Vehiculos.Count)
                    {
                        Console.WriteLine("Opción inválida.");
                        break;
                    }
                    Console.Write("Nombre del Servicio: ");
                    string nS = LeerTextoSoloLetras();
                    Vehiculos[idx - 1].Servicios.Add(new Servicio(nS));
                    Auditoria.Registrar($"Servicio agregado: {nS} al Vehiculo {Vehiculos[idx - 1].Nombre}");
                    break;
            }
        }

        static void MenuMostrar()
        {
            Console.WriteLine("\n--- Mostrar ---");
            Console.WriteLine("1. Trabajadores");
            Console.WriteLine("2. Clientes");
            Console.WriteLine("3. Vehiculos y Servicios");
            Console.Write("Seleccione: ");
            int op = LeerEnteroValido();

            switch (op)
            {
                case 1:
                    foreach (var t in trabajadores) t.MostrarInfo();
                    break;
                case 2:
                    foreach (var c in clientes) c.MostrarInfo();
                    break;
                case 3:
                    foreach (var d in Vehiculos) d.MostrarServicios();
                    break;
            }
        }

        static void MenuActualizar()
        {
            Console.WriteLine("\n--- Actualizar ---");
            Console.WriteLine("1. Actualizar Trabajador (por ID)");
            Console.WriteLine("2. Actualizar Cliente (por ID)");
            Console.Write("Seleccione: ");
            int op = LeerEnteroValido();

            switch (op)
            {
                case 1:
                    Console.Write("Ingrese ID del trabajador: ");
                    int idT = LeerEnteroValido();
                    var trab = trabajadores.FiNv(t => t.Id == idT);
                    if (trab == null) { Console.WriteLine("No encontrado."); break; }
                    Console.Write("Nuevo nombre: ");
                    trab.Nombre = LeerTextoSoloLetras();
                    Console.Write("Nuevo cargo: ");
                    trab.Cargo = LeerTextoSoloLetras();
                    Auditoria.Registrar($"Trabajador actualizado: {trab.Nombre} (ID {idT})");
                    break;
                case 2:
                    Console.Write("Ingrese ID del cliente: ");
                    int idC = LeerEnteroValido();
                    var cli = clientes.FiNv(c => c.Id == idC);
                    if (cli == null) { Console.WriteLine("No encontrado."); break; }
                    Console.Write("Nuevo nombre: ");
                    cli.Nombre = LeerTextoSoloLetras();
                    Console.Write("Nuevo correo: ");
                    cli.Correo = LeerCorreoValido();
                    Auditoria.Registrar($"Cliente actualizado: {cli.Nombre} (ID {idC})");
                    break;
            }
        }

        // ----------------- VALIDACIONES -----------------
        static string LeerTextoSoloLetras()
        {
            string? entrada;
            do
            {
                entrada = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(entrada) || !Regex.IsMatch(entrada, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                {
                    Console.Write("Entrada inválida (solo letras y espacios). Intente de nuevo: ");
                    entrada = null;
                }
            } while (entrada == null);
            return entrada.Trim();
        }

        static int LeerEnteroValido()
        {
            int numero;
            string? entrada;
            do
            {
                entrada = Console.ReadLine();
                if (!int.TryParse(entrada, out numero) || numero <= 0)
                {
                    Console.Write("Número inválido, ingrese un número positivo: ");
                    entrada = null;
                }
            } while (entrada == null);
            return numero;
        }

        static string LeerCorreoValido()
        {
            string? entrada;
            do
            {
                entrada = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(entrada) || !Regex.IsMatch(entrada, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    Console.Write("Correo inválido, intente de nuevo: ");
                    entrada = null;
                }
            } while (entrada == null);
            return entrada.Trim();
        }
    }
}