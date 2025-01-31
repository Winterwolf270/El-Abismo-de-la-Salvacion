using Spectre.Console;

public class Mutaciones
{
    public string Historia; // Nueva propiedad para la historia del personaje
    public string Nombre;
    public int Velocidad;
    public int VelocidadOriginal; // Para restaurar la velocidad original
    public string Habilidad;
    public int TiempoDeEnfriamiento;
    public char Representacion;
    public int TurnosReduccionVelocidad; // Para la trampa "Almas corruptas"
    public int CooldownRestante; // Para manejar el tiempo de enfriamiento

    public Mutaciones(string nombre, int velocidad, string habilidad, int tiempoDeEnfriamiento, char representacion, string hisotria)
    {
        Nombre = nombre;
        Velocidad = velocidad;
        VelocidadOriginal = velocidad;
        Habilidad = habilidad;
        TiempoDeEnfriamiento = tiempoDeEnfriamiento;
        Representacion = representacion;
        Historia = hisotria;
        TurnosReduccionVelocidad = 0;
        CooldownRestante = 0;
    }

    public void MostrarInfo()
    {
        AnsiConsole.MarkupLine($"[cyan] Nombre: {Nombre} [/]");
        AnsiConsole.MarkupLine($"[cyan] Velocidad: {Velocidad} [/]");
        AnsiConsole.MarkupLine($"[cyan] Habilidad: {Habilidad} (Cooldown: {TiempoDeEnfriamiento} turnos) [/]");
        AnsiConsole.MarkupLine($"[cyan] Representación: {Representacion} [/]");
    }
 
    // Método para mostrar la historia del personaje
    public void MostrarHistoria()
    {
        AnsiConsole.MarkupLine($"[yellow]{Historia}[/]");
    }

    // Método para reducir la velocidad temporalmente
    public void ReducirVelocidadTemporalmente(int turnos)
    {
        TurnosReduccionVelocidad = turnos;
        Velocidad--;
    }

    // Método para restaurar la velocidad original
    public void RestaurarVelocidad()
    {
        Velocidad = VelocidadOriginal;
        TurnosReduccionVelocidad = 0;
    }

    // Métodos para activar habilidades manuales
    public void RomperMurosAdjacentes(char[,] mapa, int x, int y)
    {
        if (CooldownRestante == 0)
        {
            // Romper muros en las posiciones adyacentes horizontal y vertical
            int[,] direcciones = new int[,]
            {
                {-1, 0}, // Arriba
                {1, 0},  // Abajo
                {0, -1}, // Izquierda
                {0, 1}   // Derecha
            };

            for (int i = 0; i < direcciones.GetLength(0); i++)
            {
                int nuevoX = x + direcciones[i, 0];
                int nuevoY = y + direcciones[i, 1];

                if (nuevoX >= 0 && nuevoX < mapa.GetLength(0) && nuevoY >= 0 && nuevoY < mapa.GetLength(1) && mapa[nuevoX, nuevoY] == '▓')
                {
                    mapa[nuevoX, nuevoY] = '▒'; // Cambiar muro a vacío
                }
            }

            AnsiConsole.MarkupLine($"[bold green] {Nombre} ha activado su habilidad 'Romper muros adjacentes'. [/]");
            CooldownRestante = TiempoDeEnfriamiento;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red] La habilidad de {Nombre} está en enfriamiento por {CooldownRestante} turnos más. [/]");
        }
    }

    public void AumentarVelocidad()
    {
        if (CooldownRestante == 0)
        {
            Velocidad++;
            CooldownRestante = TiempoDeEnfriamiento;
            AnsiConsole.MarkupLine($"[bold green] {Nombre} ha activado su habilidad 'Aumentar su velocidad de movimiento'. [/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[red] La habilidad de {Nombre} está en enfriamiento por {CooldownRestante} turnos más. [/]");
        }
    }

    // Habilidades automáticas para trampas
    public bool EvitarTrampa(string nombreTrampa)
    {
        if (CooldownRestante == 0)
        {
            if ((Nombre == "Katarina" && nombreTrampa == "Grieta") ||
                (Nombre == "Lee Sin" && nombreTrampa == "El pescador") ||
                (Nombre == "Dr Mundo" && nombreTrampa == "Almas corruptas"))
            {
                CooldownRestante = TiempoDeEnfriamiento;
                AnsiConsole.MarkupLine($"[bold green] {Nombre} ha evitado la trampa '{nombreTrampa}' gracias a su habilidad. [/]");
                return true;
            }
        }
        return false;
    }

    // Método para actualizar el cooldown
    public void ActualizarCooldown()
    {
        if (CooldownRestante > 0)
        {
            CooldownRestante--;
        }
    }
}
