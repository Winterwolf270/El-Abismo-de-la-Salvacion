using Spectre.Console;

public class Tortura
{
    public string Nombre;
    public string Efecto;

    public Tortura(string nombre, string efecto)
    {
        Nombre = nombre;
        Efecto = efecto;
    }

    public void Activar(Mutaciones jugador)
    {
        AnsiConsole.MarkupLine($"[cyan]¡Has activado la trampa '{Nombre}'! {Efecto}[/]");

        switch (Nombre)
        {
            case "Grieta":
                jugador.ReducirVelocidadTemporalmente(5); // Ejemplo de efecto
                switch (jugador.Nombre)
                {
                    case "Orn":
                        AnsiConsole.MarkupLine("[red]Ornn: ¡El suelo se abre bajo mis pies! Debo usar toda mi fuerza para evitar caer en el abismo.[/]");
                        break;
                    case "Katarina":
                        AnsiConsole.MarkupLine("[red]Katarina: ¡Una grieta! Por suerte, mis reflejos me salvarán.[/]");
                        break;
                    case "Singed":
                        AnsiConsole.MarkupLine("[red]Singed: La grieta me atrapará... a menos que mi velocidad aumente justo a tiempo.[/]");
                        break;
                    case "Lee Sin":
                        AnsiConsole.MarkupLine("[red]Lee Sin: Siento el suelo ceder. Debo confiar en mis otros sentidos para salvarme.[/]");
                        break;
                    case "Dr Mundo":
                        AnsiConsole.MarkupLine("[red]Dr Mundo: Grieta no detener a Dr Mundo. Seguir adelante.[/]");
                        break;
                }
                break;

            case "El pescador":
                jugador.ReducirVelocidadTemporalmente(5); // Ejemplo de efecto
                switch (jugador.Nombre)
                {
                    case "Orn":
                        AnsiConsole.MarkupLine("[red]Ornn: ¡Arpones lanzados desde las sombras! Necesito toda mi destreza para esquivarlos.[/]");
                        break;
                    case "Katarina":
                        AnsiConsole.MarkupLine("[red]Katarina: ¡Arpones! Pero soy más rápida que ellos.[/]");
                        break;
                    case "Singed":
                        AnsiConsole.MarkupLine("[red]Singed: Arpones dirigidos hacia mí... pero mi velocidad será mi salvación.[/]");
                        break;
                    case "Lee Sin":
                        AnsiConsole.MarkupLine("[red]Lee Sin: Los arpones no pueden retrocederme. Mi entrenamiento me protege.[/]");
                        break;
                    case "Dr Mundo":
                        AnsiConsole.MarkupLine("[red]Dr Mundo: Arpones insignificantes. No detener a Dr Mundo.[/]");
                        break;
                }
                break;

            case "Almas corruptas":
                jugador.ReducirVelocidadTemporalmente(5); // Ejemplo de efecto
                switch (jugador.Nombre)
                {
                    case "Orn":
                        AnsiConsole.MarkupLine("[red]Ornn: Espíritus malignos intentan ralentizarme. No cederé a su influencia.[/]");
                        break;
                    case "Katarina":
                        AnsiConsole.MarkupLine("[red]Katarina: Las almas me persiguen... Debo mantenerme ágil.[/]");
                        break;
                    case "Singed":
                        AnsiConsole.MarkupLine("[red]Singed: Espíritus oscuros, intentarán frenarme, pero soy más rápido de lo que creen.[/]");
                        break;
                    case "Lee Sin":
                        AnsiConsole.MarkupLine("[red]Lee Sin: Espíritus malignos, pero no podrán perturbar mi equilibrio interior.[/]");
                        break;
                    case "Dr Mundo":
                        AnsiConsole.MarkupLine("[red]Dr Mundo: Espíritus inútiles. No ralentizar a Dr Mundo.[/]");
                        break;
                }
                break;
        }
    }

}
