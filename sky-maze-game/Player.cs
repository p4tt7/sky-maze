namespace sky_maze_game.GameLogic;

public class Player
{
    public string? Nombre { get; set; }
    public Position PosicionInicial { get; set; }

    public Ficha? selectedFicha { get; set; }
    public static List<Player> jugadores = new List<Player>();
    public int Index { get; set; }

    public Player(string nombre, Position posicion)
    {
        Nombre = nombre;
        PosicionInicial = posicion;
   
    }

    public static List<Position> posicionesIniciales = new List<Position>
    {
    new Position(0, 0),
    new Position(0, Board.dimension - 1),
    new Position(Board.dimension - 1, 0),
    new Position(Board.dimension - 1, Board.dimension - 1)
    };




}













