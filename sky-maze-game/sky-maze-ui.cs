using sky_maze_game.GameLogic;

namespace sky_maze_game.GameUI;
using Spectre.Console;


public class GameUI
{


    public static void Start(){

        Console.WriteLine(@"                                                                                     
                         .--.                     .--.                 .--.                 
                      .-(    ).               .-(    ).            .-(    ).              
                     (___.__)__)           (___.__)__)          (___.__)__)               
                                                                                            
         .--.     ███████╗██╗  ██╗██╗   ██╗   ███╗   ███╗ █████╗ ███████╗███████╗      .--. 
        (    )    ██╔════╝██║ ██╔╝╚██╗ ██╔╝   ████╗ ████║██╔══██╗╚══███╔╝██╔════╝    (    )
        (___(__   ███████╗█████╔╝  ╚████╔╝    ██╔████╔██║███████║  ███╔╝ █████╗     (___.__)
                  ╚════██║██╔═██╗   ╚██╔╝     ██║╚██╔╝██║██╔══██║ ███╔╝  ██╔══╝              
          .--.    ███████║██║  ██╗   ██║      ██║ ╚═╝ ██║██║  ██║███████╗███████╗     .--.  
         (    ).  ╚══════╝╚═╝  ╚═╝   ╚═╝       ╚═╝     ╚═╝╚═╝  ╚═╝╚══════╝╚══════╝   (    ). 
        (___.__)                   .--.                 .--.                     .--.(___.__)
                                  (    ).             .-(    ).               .-(    ).      
                               (___.__)__)            (___.__)__)            (___.__)__)  
        ");

            
        AnsiConsole.MarkupLine("[cyan]1- PLAY\n0- EXIT[/]");
            
    }


public static void PrintBoard(){
    Console.OutputEncoding = System.Text.Encoding.UTF8; // Configurar codificación UTF-8

    for(int i = 0; i < Board.dimension; i++)
    {
        for(int j = 0; j < Board.dimension; j++)
        {
            string symbol = "";
            
            if (i == Board.center-1 && j == Board.center-1) // Posición ganadora 
            {
                symbol = "🌞";
            }
            else if(Board.board[i, j] == "c") // Camino
            {
                symbol = "⬛";
            }
            else if(Board.board[i, j] == "w") // Pared
            {
                symbol = "☁️";
            }
            else // Ficha, trampa y obstáculo
            {
                symbol = Board.board[i, j];
            }

            // Usando AnsiConsole para mostrar el símbolo
            AnsiConsole.Write(symbol);
        }
        AnsiConsole.WriteLine(); // Nueva línea después de cada fila
    }
}

    public static void WinArt(){
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



                 