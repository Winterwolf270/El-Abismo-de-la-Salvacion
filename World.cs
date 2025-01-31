using System.Collections.Generic;
using Spectre.Console;

public class world
{
    public int tamaño;
    public char paredes;
    public char vacio;
    public char salvacion;
    public char trampa;
    public char[,] Mapa;
    public Random random;
    public Dictionary<char, (int, int)> PosicionesJugadores;
    public Tortura[,] Trampas;

    public world(int n)
    {
        tamaño = n;
        vacio = '▒';
        salvacion = '☼'; // Nueva representación para Salvación
        paredes = '▓';
        trampa = '░';
        Mapa = new char[tamaño, tamaño];
        random = new Random();
        PosicionesJugadores = new Dictionary<char, (int, int)>();
        Trampas = new Tortura[tamaño, tamaño];
    }

    public void GenerarMapa()
    {
        for (int capas = 0; capas < (tamaño + 1) / 2; capas++)
        {
            for (int j = capas; j < tamaño - capas; j++)
            {
                if (capas % 2 == 0)
                {
                    Mapa[capas, j] = paredes;
                    Mapa[tamaño - capas - 1, j] = paredes;
                }
                else
                {
                    Mapa[capas, j] = vacio;
                    Mapa[tamaño - capas - 1, j] = vacio;
                }
            }

            for (int i = capas; i < tamaño - capas; i++)
            {
                if (capas % 2 == 0)
                {
                    Mapa[i, capas] = paredes;
                    Mapa[i, tamaño - capas - 1] = paredes;
                }
                else
                {
                    Mapa[i, capas] = vacio;
                    Mapa[i, tamaño - capas - 1] = vacio;
                }
            }
        }

        // Colocar la Salvación en el centro del mapa
        int centro = tamaño / 2;
        Mapa[centro, centro] = salvacion;

        int zonaSeguraInicio = centro - 1;
        int zonaSeguraFin = centro + 1;

        for (int i = 1; i < tamaño - 1; i++)
        {
            for (int j = 1; j < tamaño - 1; j++)
            {
                if (Mapa[i, j] == paredes &&
                    (i < zonaSeguraInicio || i > zonaSeguraFin || j < zonaSeguraInicio || j > zonaSeguraFin) &&
                    random.Next(0, 10) < 2) // Probabilidad del 20%
                {
                    Mapa[i, j] = vacio;
                }
            }
        }

        int hueco = random.Next(1, 5);
        switch (hueco)
        {
            case 1:
                Mapa[13, 14] = vacio;
                break;
            case 2:
                Mapa[13, 12] = vacio;
                break;
            case 3:
                Mapa[12, 13] = vacio;
                break;
            case 4:
                Mapa[14, 13] = vacio;
                break;
            default:
                AnsiConsole.MarkupLine("[cyan] Valor no esperado. [/]");
                break;
        }

        string[] nombresTrampas = { "Grieta", "El pescador", "Almas corruptas" };
        string[] efectosTrampas = {
            "Caiste en una grieta dimensional, seras transportado a otra zona",
            "Aparece Pyke y eres arrastrado 3 casillas atrás",
            "Tu pasado oscuro te persigue, 5 turnos con -1 de velocidad"
        };

        for (int i = 0; i < tamaño; i++)
        {
            for (int j = 0; j < tamaño; j++)
            {
                if (Mapa[i, j] == vacio && random.Next(0, 10) < 2) // Probabilidad del 20%
                {
                    Mapa[i, j] = trampa;
                    int indiceTrampa = random.Next(nombresTrampas.Length);
                    Trampas[i, j] = new Tortura(nombresTrampas[indiceTrampa], efectosTrampas[indiceTrampa]);
                }
            }
        }
    }

    public void ColocarJugadores(Mutaciones[] jugadores)
    {
        int[,] posiciones = new int[,]
        {
            { 1, 1 },
            { 1, tamaño - 2 },
            { tamaño - 2, 1 },
            { tamaño - 2, tamaño - 2 }
        };

        for (int i = 0; i < jugadores.Length; i++)
        {
            int x = posiciones[i, 0];
            int y = posiciones[i, 1];
            Mapa[x, y] = jugadores[i].Representacion;
            PosicionesJugadores[jugadores[i].Representacion] = (x, y);
        }
    }

    public void MostrarMapa()
    {
        for (int i = 0; i < tamaño; i++)
        {
            for (int j = 0; j < tamaño; j++)
            {
                switch (Mapa[i, j])
                {
                    case '▓': // Paredes
                        AnsiConsole.Markup("[green]▓[/]");
                        break;
                    case '▒': // Espacios vacíos
                        AnsiConsole.Markup("[red1]▒[/]");
                        break;
                    case '☼': // Salvación
                        AnsiConsole.Markup("[darkred]☼[/]");
                        break;
                    case '░': // Trampas
                        AnsiConsole.Markup("[red3]░[/]");
                        break;
                    default:
                        AnsiConsole.Markup("[white]" + Mapa[i, j] + "[/]");
                        break;
                }
                Console.Write(""); // Espacio entre los caracteres
            }
            Console.WriteLine();
        }
    }

    public bool MoverJugador(Mutaciones jugador, char direccion)
    {
        (int x, int y) = PosicionesJugadores[jugador.Representacion];
        int nuevoX = x, nuevoY = y;

        switch (direccion)
        {
            case 'W':
            case 'w':
                nuevoX--;
                break;
            case 'S':
            case 's':
                nuevoX++;
                break;
            case 'A':
            case 'a':
                nuevoY--;
                break;
            case 'D':
            case 'd':
                nuevoY++;
                break;
            default:
                return false;
        }

        if (nuevoX >= 0 && nuevoX < tamaño && nuevoY >= 0 && nuevoY < tamaño && Mapa[nuevoX, nuevoY] != paredes)
        {
            // Verificar si el jugador ha llegado a la Salvación
            if (Mapa[nuevoX, nuevoY] == salvacion)
            {
                MostrarMensajeFinal(jugador); // Mostrar el diálogo al llegar a la Salvación
                return false; // Terminar el juego
            }

            // Verificar colisión con otro jugador
            if (PosicionesJugadores.ContainsValue((nuevoX, nuevoY)))
            {
                char otroJugador = Mapa[nuevoX, nuevoY];

                // Intercambiar posiciones
                PosicionesJugadores[jugador.Representacion] = (nuevoX, nuevoY);
                PosicionesJugadores[otroJugador] = (x, y);

                // Actualizar el mapa
                Mapa[x, y] = otroJugador;
                Mapa[nuevoX, nuevoY] = jugador.Representacion;

                AnsiConsole.MarkupLine($"[yellow] ¡{jugador.Nombre} ha colisionado con {otroJugador} y han intercambiado posiciones! [/]");
            }
            else
            {
                if (Mapa[nuevoX, nuevoY] == trampa && Trampas[nuevoX, nuevoY] != null)
                {
                    Tortura trampaActivada = Trampas[nuevoX, nuevoY];

                    if (!jugador.EvitarTrampa(trampaActivada.Nombre))
                    {
                        trampaActivada.Activar(jugador);

                        switch (trampaActivada.Nombre)
                        {
                            case "Grieta":
                                Mapa[x, y] = vacio;
                                PosicionesJugadores.Remove(jugador.Representacion);
                                TeletransportarJugadorCapa(jugador, nuevoX, nuevoY);
                                AnsiConsole.MarkupLine($"[red] {jugador.Nombre} ha caído en la trampa 'Grieta' y ha sido teletransportado a una casilla aleatoria de la misma capa. [/]");
                                return true; // Terminar el turno del jugador
                            case "El pescador":
                                for (int i = 0; i < 3; i++)
                                {
                                    int retrocederX = nuevoX;
                                    int retrocederY = nuevoY;

                                    switch (direccion)
                                    {
                                        case 'W':
                                        case 'w':
                                            retrocederX++;
                                            break;
                                        case 'S':
                                        case 's':
                                            retrocederX--;
                                            break;
                                        case 'A':
                                        case 'a':
                                            retrocederY++;
                                            break;
                                        case 'D':
                                        case 'd':
                                            retrocederY--;
                                            break;
                                    }

                                    if (retrocederX >= 0 && retrocederX < tamaño && retrocederY >= 0 && retrocederY < tamaño && Mapa[retrocederX, retrocederY] != paredes)
                                    {
                                        Mapa[nuevoX, nuevoY] = vacio;
                                        nuevoX = retrocederX;
                                        nuevoY = retrocederY;
                                        Mapa[nuevoX, nuevoY] = jugador.Representacion;
                                        PosicionesJugadores[jugador.Representacion] = (nuevoX, nuevoY);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                break;
                            case "Almas corruptas":
                                jugador.ReducirVelocidadTemporalmente(5);
                                AnsiConsole.MarkupLine($"[red] {jugador.Nombre} ha caído en la trampa 'Almas corruptas' y su velocidad se reducirá en -1 durante 5 turnos. [/]");
                                break;
                        }
                    }
                }

                Mapa[x, y] = vacio;
                Mapa[nuevoX, nuevoY] = jugador.Representacion;
                PosicionesJugadores[jugador.Representacion] = (nuevoX, nuevoY);
            }

            return true;
        }

        return false;
    }

    private void TeletransportarJugadorCapa(Mutaciones jugador, int x, int y)
    {
        List<(int, int)> posicionesDisponibles = new List<(int, int)>();

        int capa = Math.Min(x, y);
        int limiteInferior = capa;
        int limiteSuperior = tamaño - capa - 1;

        for (int i = limiteInferior; i <= limiteSuperior; i++)
        {
            if (Mapa[limiteInferior, i] == vacio || Mapa[limiteInferior, i] == trampa) posicionesDisponibles.Add((limiteInferior, i));
            if (Mapa[limiteSuperior, i] == vacio || Mapa[limiteSuperior, i] == trampa) posicionesDisponibles.Add((limiteSuperior, i));
            if (Mapa[i, limiteInferior] == vacio || Mapa[i, limiteInferior] == trampa) posicionesDisponibles.Add((i, limiteInferior));
            if (Mapa[i, limiteSuperior] == vacio || Mapa[i, limiteSuperior] == trampa) posicionesDisponibles.Add((i, limiteSuperior));
        }

        if (posicionesDisponibles.Count > 0)
        {
            var nuevaPosicion = posicionesDisponibles[random.Next(posicionesDisponibles.Count)];

            // Remover jugador de la posición actual
            Mapa[x, y] = vacio;
            PosicionesJugadores.Remove(jugador.Representacion);
            
            // Colocar jugador en la nueva posición
            Mapa[nuevaPosicion.Item1, nuevaPosicion.Item2] = jugador.Representacion;
            PosicionesJugadores[jugador.Representacion] = nuevaPosicion;
        }
    }

    public void MostrarMensajeFinal(Mutaciones ganador)
    {
        switch (ganador.Nombre)
        {
            case "Orn":
                AnsiConsole.MarkupLine($"[green] {ganador.Nombre} ha llegado a la Salvación y ha liberado su poder ancestral! Después de alcanzar la Salvación, Ornn regresó a las montañas volcánicas de Freljord. Usando los materiales raros y poderosos que encontró en el Abismo de la Evolución, forjó armas y armaduras impenetrables para los guerreros de su tierra natal. Con su ayuda, Freljord pudo resistir las invasiones y las catástrofes naturales, convirtiéndose en una región más fuerte y unida. Ornn, aunque siguió prefiriendo la soledad, fue venerado como el protector silencioso de Freljord. Los cantos de los bardos narraban sus hazañas, y su nombre se convirtió en sinónimo de resistencia y esperanza.[/]");
                break;
            case "Katarina":
                AnsiConsole.MarkupLine($"[green] {ganador.Nombre} Has alcanzado la Salvación. Con tu agilidad y destreza, asegurarás tu lugar entre los mejores asesinos de Noxus. Al salir del Abismo de la Evolución, Katarina fue recibida como una heroína en Noxus. Su habilidad y coraje en la arena le ganaron respeto y admiración, incluso de aquellos que antes la subestimaban. Utilizó su posición para liderar misiones críticas, eliminando amenazas para Noxus y consolidando su reputación como la asesina más letal de su tiempo. Bajo su liderazgo, Noxus se fortaleció, y Katarina demostró que la grandeza se mide no solo por la habilidad, sino también por la capacidad de inspirar a otros. Su nombre se convirtió en leyenda, temido por sus enemigos y respetado por sus aliados [/]");
                break;
            case "Singed":
                AnsiConsole.MarkupLine($"[green] {ganador.Nombre} Has encontrado la Salvación. Con los nuevos conocimientos y recursos, continuarás tus experimentos sin límites. Al regresar a Zaun, Singed utilizó los recursos obtenidos en el Abismo para llevar a cabo experimentos aún más audaces. Sus descubrimientos revolucionaron el campo de la alquimia y la ciencia, aunque a un costo humano significativo. Zaun se convirtió en un centro de innovación y progreso, aunque con un toque de peligro constante debido a las creaciones de Singed. Aunque muchos le temían, otros lo veían como un visionario que desafió los límites del conocimiento. Su laboratorio se convirtió en un lugar de maravillas y horrores, y su legado fue el de un genio incomprendido[/]");
                break;
            case "Lee Sin":
                AnsiConsole.MarkupLine($"[green] {ganador.Nombre} Has llegado a la Salvación. Tu equilibrio y disciplina restaurarán la paz en Ionia. Al alcanzar la Salvación, Lee Sin regresó a Ionia con una nueva visión de paz y equilibrio. Utilizó su sabiduría y habilidades mejoradas para entrenar a una nueva generación de monjes guerreros, asegurando que su tierra natal estuviera preparada para cualquier amenaza futura. Bajo su guía, Ionia floreció como un bastión de paz y armonía, y Lee Sin fue reverenciado como un maestro y protector que restauró el equilibrio y la serenidad en su mundo. Su legado fue inmortalizado en los templos y escuelas de artes marciales, inspirando a generaciones futuras.[/]");
                break;
            case "Dr Mundo":
                AnsiConsole.MarkupLine($"[green] {ganador.Nombre} Has alcanzado la Salvación. Con tu poder y resistencia, dominas el caos y te conviertes en una leyenda en Zaun. Después de alcanzar la Salvación, Dr Mundo regresó a Zaun como una fuerza imparable. Utilizó su fuerza y resistencia para dominar el submundo criminal de la ciudad, imponiendo su voluntad con puño de hierro. Su presencia se convirtió en una leyenda aterradora, y nadie se atrevía a desafiarlo. Aunque su camino fue uno de caos y destrucción, Dr Mundo encontró en su dominio un propósito y un sentido de poder absoluto, consolidando su lugar como una figura temida y respetada en Zaun. Su nombre se convirtió en sinónimo de terror y autoridad en las calles de la ciudad[/]");
                break;
            default:
                AnsiConsole.MarkupLine($"[green] {ganador.Nombre} ha llegado a la Salvación y ganado el juego! [/]");
                break;
        }

        Environment.Exit(0); // Terminar el programa
    }
}
