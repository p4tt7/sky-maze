namespace sky_maze_game.GameLogic;
using Spectre.Console;

public class Position
{
    public int x { get; set; }
    public int y { get; set; }

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static List<Position> InitialPositionFichas = new List<Position>{
        new Position(0, 0),
        new Position(0, Board.dimension - 1),
        new Position(Board.dimension - 1, 0),
        new Position(Board.dimension - 1, Board.dimension - 1),
    };


    public static bool Movement(Ficha ficha)
    {
        Console.Clear();
        GameUI.GameUI.PrintBoard();
        ConsoleKeyInfo teclaPresionada = Console.ReadKey(true);

        int lastX = ficha.Posicion.x;
        int lastY = ficha.Posicion.y;
        bool moverse = false;
        int newX = lastX;
        int newY = lastY;

        if (teclaPresionada.Key == ConsoleKey.W || teclaPresionada.Key == ConsoleKey.UpArrow)
        {
            if (ficha.Posicion.x > 0 && !IsWall(ficha, ficha.Posicion.x - 1, ficha.Posicion.y))
            {
                newX = ficha.Posicion.x - 1;
                moverse = true;
            }
        }
        else if (teclaPresionada.Key == ConsoleKey.A || teclaPresionada.Key == ConsoleKey.LeftArrow)
        {
            if (ficha.Posicion.y > 0 && !IsWall(ficha, ficha.Posicion.x, ficha.Posicion.y - 1))
            {
                newY = ficha.Posicion.y - 1;
                moverse = true;
            }
        }
        else if (teclaPresionada.Key == ConsoleKey.S || teclaPresionada.Key == ConsoleKey.DownArrow)
        {
            if (ficha.Posicion.x < Board.dimension - 1 && !IsWall(ficha, ficha.Posicion.x + 1, ficha.Posicion.y))
            {
                newX = ficha.Posicion.x + 1;
                moverse = true;
            }
        }
        else if (teclaPresionada.Key == ConsoleKey.D || teclaPresionada.Key == ConsoleKey.RightArrow)
        {
            if (ficha.Posicion.y < Board.dimension - 1 && !IsWall(ficha, ficha.Posicion.x, ficha.Posicion.y + 1))
            {
                newY = ficha.Posicion.y + 1;
                moverse = true;
            }
        }

        if (moverse)
        {
            if (IsFicha(newX, newY))
            {
                Ficha fichaDestino = null;
                foreach (Player jugador in Player.jugadores)
                {
                    if (jugador.selectedFicha.Posicion.x == newX && jugador.selectedFicha.Posicion.y == newY)
                    {
                        fichaDestino = jugador.selectedFicha;
                        break;
                    }
                }

                string tempSimbolo = Board.board[newX, newY];

                Board.board[newX, newY] = ficha.Simbolo;
                Board.board[lastX, lastY] = tempSimbolo;

                fichaDestino.Posicion.x = lastX;
                fichaDestino.Posicion.y = lastY;
                ficha.Posicion.x = newX;
                ficha.Posicion.y = newY;
            }

            else
            {

                ficha.Posicion.x = newX;
                ficha.Posicion.y = newY;

                if (Board.board[newX, newY] == "🕸️") { ficha.Estado = Ficha.State.Slower; ficha.StateDuration = 3; ficha.CurrentVelocidad = Math.Max(1, ficha.Velocidad - 2); AnsiConsole.MarkupLine("[red]¡Has sido ralentizado por una telaraña![/]"); }

                Position nuevaPosicion = new Position(newX, newY);

                string trampaTipo = Board.board[newX, newY];
                if (trampaTipo == "⚡")
                {
                    nuevaPosicion = Trampa.Rayo(ficha);
                }
                else if (trampaTipo == "🌀")
                {
                    nuevaPosicion = Trampa.Skyhole(ficha);
                }
                else
                {
                    Trampa.DetectarTrampa(ficha);
                }

                Board.board[lastX, lastY] = "c";

                if (trampaTipo == "⚡" || trampaTipo == "🌀")
                {
                    Board.board[ficha.Posicion.x, ficha.Posicion.y] = "c";
                    ficha.Posicion = nuevaPosicion;
                    newX = nuevaPosicion.x;
                    newY = nuevaPosicion.y;
                }

                Board.board[newX, newY] = ficha.Simbolo;
            }

            return true;
        }

        return false;
    }



    public static bool IsWall(Ficha ficha, int newX, int newY)
    {
        if (Board.board[newX, newY] == "w")
        {
            return true;
        }
        return false;
    }


    public static bool IsWinning()
    {

        foreach (Player jugador in Player.jugadores)
        {
            if (Board.board[Board.center, Board.center] == jugador.selectedFicha.Simbolo)
            {
                return true;
            }

        }
        return false;
    }

    public static bool IsFicha(int x, int y)
    {
        foreach (Player jugador in Player.jugadores)
        {

            if (jugador.selectedFicha.Posicion.x == x && jugador.selectedFicha.Posicion.y == y)
            {
                return true;
            }

        }

        return false;
    }

}













