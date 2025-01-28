# SKY MAZE

*Sky Maze* es un juego multijugador que acepta hasta cuatro jugadores, cada uno elije entre seis fichas disponibles -cada una con una habilidad diferente- y se mueve a traves de las casillas de un tablero de dimension nxn con el objetivo de esquivar las distintas trampas para llegar al centro del tablero. La dimension del tablero y la cantidad de trampas esta dado por el nivel de dificultad a elegir por el usuario: facil, intermedio, dificil.

En primera instancia el usuario tendra la opcion de comenzar el juego (presionando 1) o por el contrario cerrarlo si asi desea (presionando 0). Una vez se elija la opcion JUGAR se pasa al Menu de Seleccion, donde se introduce la cantidad de jugadores, asi como el nombre de cada uno, se eligen las fichas correspondientes y el nivel de dificultad. 

## OBSTACULOS
Los caminos del laberinto generado estaran delimitados por "paredes", las cuales se muestran como "⬜", un movimiento regular no permitira a la ficha avanzar si una casilla es pared, al menos que la habilidad inherente a esta sea capaz de evitarlos.

## FICHAS
Como fue mencionado anteriormente, cada ficha tiene una habilidad correspondiente, la cual puede ser activada cada cierta cantidad de turnos en dependencia de su respectivo cooldown, a continuacion cada habilidad:
Arcoiris (🌈):
Luna Nueva (🌑):
Viento (🍃):
Nube (⛅):
Estrella (✨):
Eclipse (🌘):

## NIVELES DE DIFICULTAD
Cada nivel de dificultad tienen una dimension de laberinto y probabilidad de generacion de trampa asociado:

| DIFICULTAD | DIMENSION | PROBABILIDAD DE TRAMPA |
|------------|-----------|------------------------|
| FACIL      | 13        | 4%                     |
| INTERMEDIO | 15        | 5%                     |
| DIFICIL    | 19        | 6%                     |

Asi, la cantidad de trampas se reparte proporcional a la cantidad de casillas que posee el tablero.

## TRAMPAS
Las trampas son generadas aleatoriamente a lo largo del tablero, con la condicion que no pueden ser continuas, tomando en cuenta lo frustrante que podria ser ser congelado por tres turnos, solo para ser transportado al inicio en el proximo turno. Cuando la posicion de la ficha coincide con la posicion de una trampa, esta se activa y desaparece.
Tienen dos clasificaciones: trampas de efecto duradero, es decir, que asignan a la ficha un estado que dura por una cantidad limitada de turnos, y las trampas de efecto inmediato cuyo efecto termina en un solo turno debido a su naturaleza. A continuacion, las trampas disponibles:
Copo de Nieve(⛄):
LLuvia(☔):
Rayo(⚡):
Skyhole(🌀):

## CONTROLES
Una vez se escogen los elementos necesarios, se puede iniciar la partida, el laberinto se genera y cada jugador tiene un turno para controlar su ficha con lon los controles:

+------------------+-----------------+
|    Descripción   |   Control       |
+------------------+-----------------+
| Mover arriba     | Flecha arriba   |
|                  | W               |
+------------------+-----------------+
| Mover abajo      | Flecha abajo    |
|                  | S               |
+------------------+-----------------+
| Mover a la izq.  | Flecha izquierda|
|                  | A               |
+------------------+-----------------+
| Mover a la der.  | Flecha derecha  |
|                  | D               |
+------------------+-----------------+

Para activar la habilidad de la ficha se utiliza la tecla X, solo es posible usarla antes de iniciar el movimiento.

Una vez se llega al centro del tablero, la condicion de victoria se cumple y el juego se culmina.



# ESTRUCTURA DEL PROYECTO

**Sky-Maze-Logic:** 

**Sky-Maze-UI:**

**Board:**

**Player:**

**Ficha:**

**Position:**

**Trampa:**

**Program:**






 