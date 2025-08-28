using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NegocioVehicular
{
    // ----------------- ENTIDADES -----------------
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

        public Vehiculo(string nombre, string marca, string modelo)
        {
            Nombre = nombre;
            Marca = marca;
            Modelo = modelo;
        }

        public void MostrarServicios()
        {
            Console.WriteLine($"Vehículo: {Nombre} | Marca: {Marca} | Modelo: {Modelo}");
            if (Servicios.Count == 0)
            {
                Console.WriteLine("   - No hay servicios registrados.");
                return;
            }

            foreach (var s in Servicios)
                Console.WriteLine($"   - Servicio: {s.Nombre}");
        }
    }

    public class Servicio
    {
        public string Nombre { get; set; }
        public Servicio(string nombre) { Nombre = nombre; }
    }

    // ----------------- NUEVA CLASE AUDITORIA -----------------
    public class Auditoria
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Mensaje { get; set; }
        public string Usuario { get; set; }

        public Auditoria(int id, string mensaje, string usuario = "Sistema")
        {
            Id = id;
            Fecha = DateTime.Now;
            Mensaje = mensaje;
            Usuario = usuario;
        }

        public void MostrarInfo()
        {
            Console.WriteLine($"[AUDITORÍA] {Id} | {Fecha} | {Usuario} | {Mensaje}");
        }
    }

    // ----------------- PROGRAMA PRINCIPAL -----------------
    class Program
    {
        static List<Trabajador> trabajadores = new();
        static List<Cliente> clientes = new();
        static List<Vehiculo> Vehiculos = new();
        static List<Auditoria> auditorias = new();
        static int contadorAuditoria = 1;

        static void Main(string[] args)
        {
            Stopwatch reloj = Stopwatch.StartNew();
            int opcion;

            do
            {
                Console.WriteLine("\n=== MENÚ PRINCIPAL ===");
                Console.WriteLine("1. Agregar información");
                Console.WriteLine("2. Mostrar información");
                Console.WriteLine("3. Actualizar información");
                Console.WriteLine("4. Ver auditorías");
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
                    case 4:
                        MostrarAuditorias();
                        break;
                    case 0:
                        reloj.Stop();
                        TimeSpan duracion = reloj.Elapsed;
                        RegistrarAuditoria($"Sistema finalizado en {duracion.TotalSeconds:F2} segundos");
                        Console.WriteLine($"\nTiempo total de ejecución: {duracion.TotalSeconds:F2} segundos");
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

            } while (opcion != 0);
        }

        // ----------------- MÉTODOS AUDITORÍA -----------------
        static void RegistrarAuditoria(string mensaje, string usuario = "Sistema")
        {
            auditorias.Add(new Auditoria(contadorAuditoria++, mensaje, usuario));
        }

        static void MostrarAuditorias()
        {
            Console.WriteLine("\n--- AUDITORÍAS ---");
            if (auditorias.Count == 0)
                Console.WriteLine("No hay registros de auditoría.");
            else
                foreach (var a in auditorias) a.MostrarInfo();
        }

        // ----------------- SUBMENUS -----------------
        static void MenuAgregar()
        {
            Console.WriteLine("\n--- Agregar ---");
            Console.WriteLine("1. Trabajador");
            Console.WriteLine("2. Cliente");
            Console.WriteLine("3. Vehículo");
            Console.WriteLine("4. Servicio a un Vehículo");
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
                    RegistrarAuditoria($"Trabajador agregado: {nT}");
                    break;
                case 2:
                    Console.Write("Nombre: ");
                    string nC = LeerTextoSoloLetras();
                    Console.Write("ID: ");
                    int idC = LeerEnteroValido();
                    Console.Write("Correo: ");
                    string correo = LeerCorreoValido();
                    clientes.Add(new Cliente(nC, idC, correo));
                    RegistrarAuditoria($"Cliente agregado: {nC}");
                    break;
                case 3:
                    Console.Write("Nombre del Vehículo: ");
                    string Nv = LeerTextoSoloLetras();
                    Console.Write("Marca del Vehículo: ");
                    string Mv = LeerTextoSoloLetras();
                    Console.Write("Modelo del Vehículo: ");
                    string Mov = LeerTextoSoloLetras();
                    Vehiculos.Add(new Vehiculo(Nv, Mv, Mov));
                    RegistrarAuditoria($"Vehículo agregado: {Nv}");
                    break;
                case 4:
                    if (Vehiculos.Count == 0)
                    {
                        Console.WriteLine("No hay vehículos registrados.");
                        break;
                    }
                    Console.WriteLine("Seleccione un vehículo:");
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
                    RegistrarAuditoria($"Servicio agregado: {nS} al Vehículo {Vehiculos[idx - 1].Nombre}");
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }
        }

        static void MenuMostrar()
        {
            Console.WriteLine("\n--- Mostrar ---");
            Console.WriteLine("1. Trabajadores");
            Console.WriteLine("2. Clientes");
            Console.WriteLine("3. Vehículos y Servicios");
            Console.Write("Seleccione: ");
            int op = LeerEnteroValido();

            switch (op)
            {
                case 1:
                    if (trabajadores.Count == 0)
                        Console.WriteLine("No hay trabajadores registrados.");
                    else
                        foreach (var t in trabajadores) t.MostrarInfo();
                    break;
                case 2:
                    if (clientes.Count == 0)
                        Console.WriteLine("No hay clientes registrados.");
                    else
                        foreach (var c in clientes) c.MostrarInfo();
                    break;
                case 3:
                    if (Vehiculos.Count == 0)
                        Console.WriteLine("No hay vehículos registrados.");
                    else
                        foreach (var v in Vehiculos) v.MostrarServicios();
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
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
                    var trab = trabajadores.Find(t => t.Id == idT);
                    if (trab == null)
                    {
                        Console.WriteLine("Trabajador no encontrado.");
                        break;
                    }
                    Console.Write("Nuevo nombre: ");
                    trab.Nombre = LeerTextoSoloLetras();
                    Console.Write("Nuevo cargo: ");
                    trab.Cargo = LeerTextoSoloLetras();
                    RegistrarAuditoria($"Trabajador actualizado: {trab.Nombre} (ID {idT})");
                    break;
                case 2:
                    Console.Write("Ingrese ID del cliente: ");
                    int idC = LeerEnteroValido();
                    var cli = clientes.Find(c => c.Id == idC);
                    if (cli == null)
                    {
                        Console.WriteLine("Cliente no encontrado.");
                        break;
                    }
                    Console.Write("Nuevo nombre: ");
                    cli.Nombre = LeerTextoSoloLetras();
                    Console.Write("Nuevo correo: ");
                    cli.Correo = LeerCorreoValido();
                    RegistrarAuditoria($"Cliente actualizado: {cli.Nombre} (ID {idC})");
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
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
