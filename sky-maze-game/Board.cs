namespace sky_maze_game.GameLogic;

public class Board
{
    public static int dimension;
    public static int center = dimension/2;
    public static string[,] board;
    public static int[,] direcciones = { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 } };


    public static void BoardInitializer()
    {
        board = new string[dimension, dimension];
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                board[i, j] = "w";
            }
        }
    }

    public static string[,] BoardGenerator() //recursive backtrack
    {
        board[0, 0] = "c"; //marca el inicio

        GenerateMaze(board, 0, 0); //genera el laberunto
        return board;
    }

    private static void GenerateMaze(string[,] board, int currentRow, int currentCol)
    {
        int[] indices = { 0, 1, 2, 3 };
        ShuffleDirections(indices);

        for (int i = 0; i < 4; i++)
        {
            int directionIndex = indices[i];
            int newRow = currentRow + direcciones[directionIndex, 0] * 2;
            int newCol = currentCol + direcciones[directionIndex, 1] * 2;

            if (newRow >= 0 && newRow < board.GetLength(0) && newCol >= 0 && newCol < board.GetLength(1) && board[newRow, newCol] == "w")
            {

                board[currentRow + direcciones[directionIndex, 0], currentCol + direcciones[directionIndex, 1]] = "c";
                board[newRow, newCol] = "c";

                GenerateMaze(board, newRow, newCol);
            }
        }
    }

    private static void ShuffleDirections(int[] indices)
    {
        int n = indices.Length;
        Random rand = new Random();

        for (int i = n - 1; i > 0; i--)
        {

            int j = rand.Next(0, i + 1);

            int temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;
        }
    }


    public static int[,] DistanceValidator(string[,] board, int firstRow, int firstColumn) //algoritmo de lee
    {
        board[center, center] = "c";
        foreach (Position Posicion in Position.InitialPositionFichas)
        {
            board[Posicion.x, Posicion.y] = "c";
        }
        int[,] distancias = new int[dimension, dimension];
        distancias[firstRow, firstColumn] = 1;
        int[] dr = { -1, 1, 0, 0, -1, 1, -1, 1 };
        int[] dc = { 0, 0, 1, -1, -1, -1, 1, 1 };
        bool change;

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                distancias[i, j] = -1;
            }
        }

        do
        {
            change = false;

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (distancias[i, j] == 0)
                    {
                        continue;
                    }
                    if (board[i, j] == "w")
                    {
                        continue;
                    }
                    for (int d = 0; d < dr.Length; d++)
                    {
                        int vr = i + dr[d];
                        int vc = j + dc[d];

                        if (Range(dimension, vr, vc) && distancias[vr, vc] == 0 && board[vr, vc] == "c")
                        {
                            distancias[vr, vc] = distancias[i, j] + 1;
                            change = true;
                        }
                    }
                }
            }
        } while (change);

        return distancias;
    }

    public static bool Range(int dimension, int row, int column)
    {
        if (row >= 0 && row < dimension && column >= 0 && column < dimension)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ValidatedBoard(string[,] board, int[,] distancias)
    {

        int[] dr = { -1, 1, 0, 0, -1, 1, -1, 1 };
        int[] dc = { 0, 0, 1, -1, -1, -1, 1, 1 };

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                if (distancias[i, j] == -1)
                {

                    for (int d = 0; d < dr.Length; d++)
                    {
                        int vr = i + dr[d];
                        int vc = j + dc[d];

                        if (Range(dimension, vr, vc) && distancias[vr, vc] != -1)
                        {
                            board[i, j] = "c";
                            distancias[i, j] = distancias[vr, vc] + 1;
                            break;
                        }
                    }

                }
            }
        }
    }
}













