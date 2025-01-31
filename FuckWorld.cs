using System;
using Spectre.Console;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[bold green]En un mundo devastado por la guerra y la contaminación, la humanidad se encuentra al borde de la extinción. Los paisajes antes verdes y fértiles ahora son desiertos inhóspitos llenos de ruinas. Sin embargo, en medio de la desesperación, surge una nueva esperanza: los mutantes. Estos seres, producto de mutaciones genéticas inducidas por la radiación y los químicos tóxicos, poseen habilidades especiales que los hacen únicos.[/]");
        Console.ReadLine();
        AnsiConsole.MarkupLine("[bold green]El Abismo de la Evolución es un coliseo subterráneo, oculto en las profundidades de la tierra, donde los mutantes más poderosos se reúnen para competir y demostrar su supremacía. Cada mutante debe navegar un laberinto mortal lleno de trampas antiguas y enemigos feroces para alcanzar la Salvación, el centro del coliseo, donde se dice que reside el secreto para restaurar el mundo.[/]");
        Console.ReadLine();
        AnsiConsole.MarkupLine("[bold green]Solo aquellos con habilidades excepcionales y la voluntad de sacrificarse por el bien mayor lograrán sobrevivir a los desafíos del Abismo. Cada paso en el coliseo no es solo una prueba de fuerza y destreza, sino también un test de carácter y determinación. La lucha no es solo por la propia supervivencia, sino por el futuro de toda la humanidad.[/]");
        Console.ReadLine();

        // Crear una lista de Mutaciones predefinidas
        Mutaciones[] mutantes = new Mutaciones[]
        {
            new Mutaciones("Orn", 4, "Romper un muro", 10, 'O',"Orn fue un arquitecto brillante antes de la gran catástrofe. Cuando el mundo se derrumbó, perdió a su familia y a su hogar. Atrapado bajo los escombros, Orn emergió como un mutante con una fuerza descomunal, capaz de romper cualquier barrera física. Ahora, Orn busca la Salvación en el Abismo para obtener el poder de reconstruir el mundo y recuperar lo que una vez fue. Su viaje es uno de redención y esperanza."),
            
            new Mutaciones("Katarina", 6, "Saltar las trampas de agujero", 2, 'K',"Katarina era una corredora de élite, ganadora de numerosas competiciones de velocidad, hasta que la radiación la transformó en una mutante. Su agilidad y rapidez fueron aumentadas a niveles sobrehumanos. Katarina se enteró de que su familia había sido capturada por un grupo de mutantes hostiles. Cree que alcanzando la Salvación, podrá salvar a su familia y encontrar un refugio seguro para todos ellos. Su historia está llena de amor y determinación."),
            
            new Mutaciones("Singed", 3, "Aumentar su velocidad de movimiento", 10, 'S',"Singed fue un científico brillante que se experimentó a sí mismo con compuestos químicos, lo que resultó en sus habilidades mutantes. La constante exposición a químicos le ha dejado mentalmente inestable, pero también le ha dado la capacidad de aumentar su velocidad cuando más lo necesita. Singed busca la Salvación para encontrar un antídoto que cure su mente y le permita vivir en paz. Su viaje es uno de autodescubrimiento y redención."),
            
            new Mutaciones("Lee Sin", 5, "Inmune al retroceso", 1, 'L',"Lee Sin era un monje que se dedicaba a la meditación y a las artes marciales. Durante la gran catástrofe, sus ojos fueron dañados, dejándolo ciego. Sin embargo, la radiación le otorgó la capacidad de sentir su entorno y ser inmune al retroceso. Lee Sin desea alcanzar la Salvación para restaurar el equilibrio en el mundo y redescubrir su propósito espiritual. Su viaje es uno de búsqueda de la verdad y la iluminación."),
            
            new Mutaciones("Dr Mundo", 4, "Inmune a los debuffos", 1, 'M',"Dr Mundo era un médico en una pequeña aldea antes de la catástrofe. Cuando el desastre golpeó, fue expuesto a químicos que lo transformaron en un ser gigante e inmune a los debuffos. Ahora, busca la Salvación para encontrar el secreto de su origen y propósito. Dr Mundo está impulsado por un deseo de entender quién es y por qué fue elegido para esta transformación. Su historia es una de identidad y propósito.")
        };

        // Mostrar las estadísticas de los mutantes antes de que los jugadores elijan
        AnsiConsole.MarkupLine("[darkred] Veamos los campeones disponibles: [/]");
        for (int j = 0; j < mutantes.Length; j++)
        {
            mutantes[j].MostrarInfo();
            mutantes[j].MostrarHistoria();
            Console.WriteLine();
        }

        // Preguntar al usuario cuántos personajes han mutado
        AnsiConsole.MarkupLine("[bold blue] ¡Los mutantes están listos para la batalla! ¿Cuántos héroes competirán en esta ronda épica? [/]");
        int cantidad = int.Parse(Console.ReadLine());
        while (cantidad <= 1 || cantidad >= 5)
        {
            AnsiConsole.MarkupLine("[darkred] Que sea entre dos y cuatro, animal de granja [/]");
            cantidad = int.Parse(Console.ReadLine());
        }

        Mutaciones[] jugadores = new Mutaciones[cantidad];
        bool[] mutantesSeleccionados = new bool[mutantes.Length];

        for (int i = 0; i < cantidad; i++)
        {
            AnsiConsole.MarkupLine($"[bold yellow] Elige tu personaje: Player {i + 1}: [/]");
            for (int j = 0; j < mutantes.Length; j++)
            {
                if (!mutantesSeleccionados[j])
                {
                    AnsiConsole.MarkupLine($"[bold cyan] {j + 1}. {mutantes[j].Nombre} [/]");
                }
            }

            int eleccion;
            while (true)
            {
                AnsiConsole.MarkupLine("[bold yellow] Introduce el número de tu personaje: [/]");
                if (int.TryParse(Console.ReadLine(), out eleccion) && eleccion >= 1 && eleccion <= mutantes.Length && !mutantesSeleccionados[eleccion - 1])
                {
                    jugadores[i] = mutantes[eleccion - 1];
                    mutantesSeleccionados[eleccion - 1] = true;
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red] Selección inválida o personaje ya elegido. Inténtalo de nuevo. [/]");
                }
            }
        }

        AnsiConsole.MarkupLine("[bold magenta] \nMutaciones seleccionados: [/]");

        for (int i = 0; i < cantidad; i++)
        {
            jugadores[i].MostrarInfo();
        }

        // Crear y mostrar el mapa del mundo
        world abismo = new world(27);
        abismo.GenerarMapa();
        abismo.ColocarJugadores(jugadores);
        abismo.MostrarMapa();

        // Lógica para mover a los jugadores
        bool juegoEnCurso = true;
        while (juegoEnCurso)
        {
            for (int i = 0; i < jugadores.Length; i++)
            {
                // Actualizar cooldown de habilidades
                jugadores[i].ActualizarCooldown();

                int velocidadActual = jugadores[i].Velocidad;
                if (jugadores[i].TurnosReduccionVelocidad > 0)
                {
                    velocidadActual--;
                    jugadores[i].TurnosReduccionVelocidad--;
                    if (jugadores[i].TurnosReduccionVelocidad == 0)
                    {
                        jugadores[i].RestaurarVelocidad();
                    }
                }

                for (int pasos = 0; pasos < velocidadActual; pasos++)
                {
                    AnsiConsole.MarkupLine($"[bold yellow]Turno de {jugadores[i].Nombre}. Usa [bold cyan]W, A, S, D[/] para moverte ({velocidadActual - pasos} movimientos restantes): \nPara activar tu habilidad, presiona la tecla [bold magenta]H[/].[/]");

                    char movimiento = Console.ReadKey().KeyChar;
                    Console.Clear();

                    if (movimiento == 'H' || movimiento == 'h')
                    {
                        // Activar habilidad del jugador
                        switch (jugadores[i].Nombre)
                        {
                            case "Orn":
                                var posicionOrn = abismo.PosicionesJugadores[jugadores[i].Representacion];
                                jugadores[i].RomperMurosAdjacentes(abismo.Mapa, posicionOrn.Item1, posicionOrn.Item2);
                                abismo.MostrarMapa();
                                break;
                            case "Singed":
                                jugadores[i].AumentarVelocidad();
                                break;
                        }
                    }
                    else if (abismo.MoverJugador(jugadores[i], movimiento))
                    {
                        abismo.MostrarMapa();

                        // Verificar si algún jugador ha llegado a la Salvación (centro)
                        int centro = abismo.tamaño / 2;
                        var posicionJugador = abismo.PosicionesJugadores[jugadores[i].Representacion];
                        if (posicionJugador.Item1 == centro && posicionJugador.Item2 == centro)
                        {
                            // Mostrar mensaje final y detener el juego
                            abismo.MostrarMensajeFinal(jugadores[i]);
                            juegoEnCurso = false;
                            break;
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red] Movimiento inválido. Inténtalo de nuevo. [/]");
                        pasos--; // No contamos el movimiento inválido como un paso
                        abismo.MostrarMapa();
                    }
                }

                if (!juegoEnCurso)
                {
                    break;
                }
            }
        }
    }
}