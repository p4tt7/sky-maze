# SKY MAZE

![Portada Juego Image](sky-maze-game/portada-juego.png)

_Sky Maze_ es un juego multijugador que acepta hasta cuatro jugadores, cada uno elije entre seis fichas disponibles -cada una con una habilidad diferente- y se mueve a traves de las casillas de un tablero de dimension nxn con el objetivo de esquivar las distintas trampas para llegar al centro del tablero. La dimension del tablero y la cantidad de trampas esta dado por el nivel de dificultad a elegir por el usuario: facil, intermedio, dificil.

En primera instancia el usuario tendra la opcion de comenzar el juego (presionando 1) o por el contrario cerrarlo si asi desea (presionando 0). Una vez se elija la opcion JUGAR se pasa al Menu de Seleccion, donde se introduce la cantidad de jugadores, asi como el nombre de cada uno, se eligen las fichas correspondientes y el nivel de dificultad. 

# OBSTACULOS
Los caminos del laberinto generado estaran delimitados por "paredes", las cuales se muestran como "⬜", un movimiento regular no permitira a la ficha avanzar si una casilla es pared, al menos que la habilidad inherente a esta sea capaz de evitarlos.


# NIVELES DE DIFICULTAD
Cada nivel de dificultad tienen una dimension de laberinto y probabilidad de generacion de trampa asociado:

| DIFICULTAD | DIMENSION | PROBABILIDAD DE TRAMPA |
|------------|-----------|------------------------|
| FACIL      | 13        | 4%                     |
| INTERMEDIO | 15        | 5%                     |
| DIFICIL    | 19        | 6%                     |

Asi, la cantidad de trampas se reparte proporcional a la cantidad de casillas que posee el tablero.


# CONTROLES
Una vez se escogen los elementos necesarios, se puede iniciar la partida, el laberinto se genera y cada jugador tiene un turno para controlar su ficha con lon los controles:








 
