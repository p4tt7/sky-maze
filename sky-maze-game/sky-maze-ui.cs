using sky_maze_game.GameLogic;

namespace sky_maze_game.GameUI;
using Spectre.Console;


public class GameUI
{

    public static void IntroAnimation()
    {
        Console.Clear();
        AnsiConsole.Markup("[cyan]El cielo es hermoso, pero...[/]");
        System.Threading.Thread.Sleep(2000);
        AnsiConsole.Clear();
        AnsiConsole.Markup("[cyan]¿Quién iba a decir que las diversas formas que tomaban las nubes no eran más que un laberinto?[/]");
        System.Threading.Thread.Sleep(3000);
        AnsiConsole.Clear();
    }



    public static void Start()
    {

        AnsiConsole.MarkupLine(@"[cyan]                                                                                     
                         .--.                     .--.                 .--.                 
                      .-(    ).               .-(    ).            .-(    ).              
                     (___.__)__)           (___.__)__)          (___.__)__)               
                                                                                            
         .--.     ███████╗██╗  ██╗██╗   ██╗   ███╗   ███╗ █████╗ ███████╗███████╗      .--. 
        (    )    ██╔════╝██║ ██╔╝╚██╗ ██╔╝   ████╗ ████║██╔══██╗╚══███╔╝██╔════╝    (    )
        (___(__   ███████╗█████╔╝  ╚████╔╝    ██╔████╔██║███████║  ███╔╝ █████╗     (___.__)
                  ╚════██║██╔═██╗   ╚██╔╝     ██║╚██╔╝██║██╔══██║ ███╔╝  ██╔══╝              
          .--.    ███████║██║  ██╗   ██║      ██║ ╚═╝ ██║██║  ██║███████╗███████╗     .--.  
         (    ).  ╚══════╝╚═╝  ╚═╝   ╚═╝      ╚═╝     ╚═╝╚═╝  ╚═╝╚══════╝╚══════╝   (    ). 
        (___.__)                   .--.                 .--.                     .--.(___.__)
                                  (    ).             .-(    ).               .-(    ).      
                               (___.__)__)            (___.__)__)            (___.__)__)  
        [/]");

        AnsiConsole.MarkupLine("\n[cyan]1- PLAY\n0- EXIT[/]");

    }

    public static void Presentation()
    {
        var markup = new Markup(@"[cyan] 
        ██████╗ ██╗███████╗███╗   ██╗██╗   ██╗███████╗███╗   ██╗██╗██████╗  ██████╗ 
        ██╔══██╗██║██╔════╝████╗  ██║██║   ██║██╔════╝████╗  ██║██║██╔══██╗██╔═══██╗
        ██████╔╝██║█████╗  ██╔██╗ ██║██║   ██║█████╗  ██╔██╗ ██║██║██║  ██║██║   ██║
        ██╔══██╗██║██╔══╝  ██║╚██╗██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║██║██║  ██║██║   ██║
        ██████╔╝██║███████╗██║ ╚████║ ╚████╔╝ ███████╗██║ ╚████║██║██████╔╝╚██████╔╝
        ╚═════╝ ╚═╝╚══════╝╚═╝  ╚═══╝  ╚═══╝  ╚══════╝╚═╝  ╚═══╝╚═╝╚═════╝  ╚═════╝ 
        [/]");
        AnsiConsole.Write(new Align(markup, HorizontalAlignment.Center));

        AnsiConsole.MarkupLine("[cyan]Has sido atrapado por los caprichos de seres celestiales, y queriendo retornar a tu mundo, debes emprender una aventura hasta la salida que ubicas en el centro para escapar de él.[/]\n");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Jugadores y Fichas:\n");
        AnsiConsole.MarkupLine("[cyan]- Hasta cuatro valientes pueden emprender esta aventura, cada uno eligiendo hasta tres de las siguientes fichas:[/]");
        AnsiConsole.MarkupLine("[cyan]  - Arcoiris (🌈):                      [/]");
        AnsiConsole.MarkupLine("[cyan]  - Luna Nueva (🌑):                    [/]");
        AnsiConsole.MarkupLine("[cyan]  - Viento (🍃):                        [/]");
        AnsiConsole.MarkupLine("[cyan]  - Nube (⛅):                          [/]");
        AnsiConsole.MarkupLine("[cyan]  - Estrella (✨):                      [/]");
        AnsiConsole.MarkupLine("[cyan]  - Eclipse (🌘):                       [/]\n");

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Trampas y Obstáculos:");
        AnsiConsole.MarkupLine("[cyan]- A lo largo del laberinto, los jugadores encontrarán trampas que congelan a las fichas por 2 turnos, lluvias torrenciales que hacen que las fichas resbalen, o agujeros en el cielo que los trasladaran a otro punto del tablero.[/]\n");

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Jugabilidad:");
        AnsiConsole.MarkupLine("[cyan]- Cada jugador, por turno, puede mover sus fichas una cantidad de casillas equivalente a su velocidad, optando por usar su habilidad especial cuando esté disponible.[/]\n");

        AnsiConsole.MarkupLine("Controles:");
        AnsiConsole.MarkupLine("[cyan]W y flecha superior: Arriba\nA y flecha izquierda: Izquierda\nS y flecha inferior: Abajo\nD y flecha derecha: Derecha[/]");

        AnsiConsole.MarkupLine("\nPRESIONA ENTER PARA CONTINUAR");
    }


    public static void PrintBoard()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        for (int i = 0; i < Board.dimension; i++)
        {
            for (int j = 0; j < Board.dimension; j++)
            {
                string symbol = "";

                foreach (Player jugador in Player.jugadores)
                {
                    if (jugador.selectedFicha.Posicion.x == i && jugador.selectedFicha.Posicion.y == j)
                    {
                        symbol = jugador.selectedFicha.Simbolo;
                        break;
                    }
                }

                if (i == Board.center && j == Board.center) // centro
                {
                    symbol = "🟦";
                }

                else if (Board.board[i, j] == "c") // camino
                {
                    symbol = "  ";
                }
                else if (Board.board[i, j] == "w") // pared
                {
                    symbol = "⬜";
                }
                else // ficha, trampa 
                {
                    symbol = Board.board[i, j];
                }

                AnsiConsole.Write(symbol);
            }
            AnsiConsole.WriteLine();
        }
    }

    public static void WinArt()
    {
        AnsiConsole.MarkupLine(
        "'     |         [cyan]  | [/]                                 |   \n" +
        "      o         [cyan] -o- [/]       -+-       +~~     .     -o-  \n" +
        "      |   o     [cyan]  | [/]  [cyan]     __'   [/].. *       |   \n" +
        "    -+-      '              [cyan]     .' .-'`  [/]                + o\n" +
        "    o |   ' '               [cyan]     /  /     [/]  +   '             \n" +
        "      o  .     o           '[cyan]     |  |    [/].+         .        *\n" +
        "                    +    .  [cyan]     |  |    [/]    .    *           \n" +
        "                            [cyan]  o   '._'-._ [/]                  ' \n" +
        "    *                       [cyan]         ```  [/]                    \n" +
        "                      [cyan] HAS GANADO!!![/]            *             \n" +
        "        *        +               *                                . \n" +
        " +                         '                '                   o   \n" +
        "   '              .                   .        o .                .\n" +
        "   o   *           .   +   [cyan]':.       [/].                        \n" +
        "             |       . o   [cyan]  '::._ * [/]     |  |  +             \n" +
        "    ':.    - o - o         [cyan]    '._)  [/]    -+--+-     *       ' \n" +
        "      '::._  |             o              |  |                \n" +
        "    '.  '._)     +               .   .   o   '        .       \n" +
        "   .                 .           . '            .'        .   \n" +
        "                      .        .     +    '       + +."
        );
    }
}



