namespace sky_maze_game.GameLogic;

public static class GameLogic{
    public static void MainMenu(){
        while(true)
        {
            int state = int.Parse(Console.ReadLine());
            if (state == 1)
            {
                Console.WriteLine("Iniciando juego...");
            }
            else if (state == 0)
            {
                break;
            }
            else
            {
                Console.WriteLine("Ingrese 0 o 1");
            }
        }
    }
}

