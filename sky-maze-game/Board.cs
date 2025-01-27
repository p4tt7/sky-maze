namespace sky_maze_game.GameLogic;

public class Board
{
    public static int dimension;
    public static int center = dimension / 2;
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

        int[,] distancias = new int[dimension, dimension];

        board[center, center] = "c";

        foreach (Position Posicion in Position.InitialPositionFichas)
        {
            board[Posicion.x, Posicion.y] = "c";
        }

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                if (board[i, j] != "w")
                {
                    distancias[i, j] = -1;

                }
            }
        }

        distancias[firstRow, firstColumn] = 1;

        int[] dr = { -1, 1, 0, 0, -1, 1, -1, 1 };
        int[] dc = { 0, 0, 1, -1, -1, -1, 1, 1 };
        bool change;

        do
        {
            change = false;

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (distancias[i, j] > 0)
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

                        if (Range(dimension, vr, vc) && distancias[vr, vc] == -1 && board[vr, vc] == "c")
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
        bool change;

        do
        {
            change = false;
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (distancias[i, j] == -1)
                    {
                        int minDist = int.MaxValue;
                        for (int d = 0; d < dr.Length; d++)
                        {
                            int vr = i + dr[d];
                            int vc = j + dc[d];

                            if (Range(dimension, vr, vc) && distancias[vr, vc] > 0)
                            {
                                minDist = Math.Min(minDist, distancias[vr, vc] + 1);
                                change = true;
                            }
                        }
                        if (minDist != int.MaxValue)
                        {
                            board[i, j] = "c";
                            distancias[i, j] = minDist;
                        }
                    }
                }
            }
        } while (change);
    }
}













