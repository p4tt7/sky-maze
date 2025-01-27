namespace sky_maze_game.GameLogic;

public class Player
{
    public string? Nombre { get; set; }
    public Ficha? selectedFicha { get; set; }
    public static List<Player> jugadores = new List<Player>();

    public Player(string nombre)
    {
        Nombre = nombre;
    }
}













