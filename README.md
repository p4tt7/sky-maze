# **SKY MAZE**

![Portada Juego Image](sky-maze-game/portada-juego.png)

_Sky Maze_ es un juego multijugador que acepta hasta cuatro jugadores, cada uno elije entre seis fichas disponibles -cada una con una habilidad diferente- y se mueve a través de las casillas de un tablero de dimensión n x n con el objetivo de esquivando trampas, llegar al centro del tablero, y escapar.

## **INSTALACION**

1. Requisitos para su instalación
   - Verificar tener [DOTNET SDK](https://dotnet.microsoft.com/en-us/download) previamente instalado

2. Clonar el repositorio:

   - ```git clone https://github.com/p4tt7/sky-maze.git```

3. Build repositiorio:

   - ```dotnet build```

4. Correr el juego:

   - ```dotnet run```

## **INSTRUCCIONES**

### _MENU PRINCIPAL_
En primera instancia los jugadores se encontrarán con el Menú Princpal como punto de inicio, donde tendrán las opciones:


 - _PLAY:_ Inicia el juego, y pasa al Menú de Selección

 - _EXIT:_ Cierra el juego por completo.


### _MENU DE SELECCION_
Al seleccionar _PLAY_ ingresando su índice correspondiente _1_, se brindan instrucciones basicas para el uso del juego, los cuales pueden ser omitidos presionando _ENTER_ y asi pasar al siguiente menu, el Menu de Seleccion, aqui los jugadores podran seleccionaar:

 - Cantidad de jugadores: Se pueden ingresar hasta 4, e inmediatamente se introduce el nombre de cada uno

 - Ficha: Cada jugador elige la ficha que usara en la partida, cada una tiene una habilidad asociada que puede ser activada al inicio de la partida:



### Arcoiris 🌈
| Propiedad     | Valor         |
|---------------|---------------|
| Velocidad     | 4             |
| CoolingTime   | 2             |
| Habilidad     |  Iris Healing       |

### Luna Nueva 🌑
| Propiedad     | Valor         |
|---------------|---------------|
| Velocidad     | 3             |
| CoolingTime   | 4             |
| Habilidad     | Slowing Shadow       |

### Viento 🍃
| Propiedad     | Valor         |
|---------------|---------------|
| Velocidad     | 2             |
| CoolingTime   | 3             |
| Habilidad     | Rafaga Acelerada  |

### Nube ⛅
| Propiedad     | Valor         |
|---------------|---------------|
| Velocidad     | 3             |
| CoolingTime   | 5             |
| Habilidad     | Piercing Cloud           |

### Estrella ✨
| Propiedad     | Valor         |
|---------------|---------------|
| Velocidad     | 3             |
| CoolingTime   | 4             |
| Habilidad     | Star Daze         |

### Eclipse 🌘
| Propiedad     | Valor         |
|---------------|---------------|
| Velocidad     | 4             |
| CoolingTime   | 5             |
| Habilidad     | Mimic Eclipse       |




**Iris Healing:** Es capaz de curarse del estado impuesto por una trampa.



**Slowing Shadow:** Genera casillas aleatorias sobre el laberinto capaz de reducir la velocidad de los jugadores y evitar que lleguen antes a la meta.



**Rafaga Acelerada:** Aumenta la velocidad de la ficha en 10, siendo capaz de moverse hasta 12 casillas por turno.



**Piercing Cloud:** La nube puede volar sobre las casillas, evitando obstaculos y trampas.



**Star Daze:** La estrella entra en frenesi y rompe todo en su camino (excepto fichas)



**Mimic Eclipse:** Puede copiar cualquier habilidad de las fichas que se encuentren en el tablero.



_NOTA: Cada jugador puede elegir una unica ficha y no puede ser repetida por los demas, una vez un jugador elija una ficha, sera inmediatamente retirada de la lista de fichas disponibles._



 - Nivel de dificultad: Afecta directamente la dimension del tablero y probabilidad de generacion de trampas:


| DIFICULTAD | DIMENSION | PROBABILIDAD DE TRAMPA |
|------------|-----------|------------------------|
| FACIL      | 13        | 7%                     |
| INTERMEDIO | 15        | 8%                     |
| DIFICIL    | 19        | 9%                     |


Asi, la cantidad de trampas se reparte proporcional a la cantidad de casillas que posee el tablero.


### _FLUJO DEL JUEGO_ 


#### Partes del laberinto


 1. Fichas: Las fichas se inicializan en las respectivas esquinas del laberinto, se garantiza que no seran bloqueadas durante la generacion de este.


   
 2. Centro del tablero: Es la condicion de victoria, representado por un "🟦" que representa la puerta de salida del cielo.


   
 3. Obstaculos: Delimitan el laberinto, representados por un "⬜" Un movimiento regular no permitira a la ficha avanzar si una casilla es pared, al menos que la habilidad inherente a esta sea capaz de evitarlos.
 4. 
_NOTA: Si en el movimiento, los jugadores chocan con un obstaculo, no se contara como un  movimiento invalido_



 5. Trampas: Se generan aleatoriamente por el tablero, se clasifican en dos tipos: _Instantaneas y Continuas_, donde la principal diferencia es la duracion de su efecto sobre el jugador.

### Copo de Nieve
| Propiedad | Valor        |
|-----------|--------------|
| Nombre    | Copo de Nieve|
| Símbolo   | ⛄            |
| Habilidad | Congelar     |
| Efecto    | No puede moverse por 3 turnos, velocidad reducida a 0|

### Lluvia
| Propiedad | Valor        |
|-----------|--------------|
| Nombre    | Lluvia       |
| Símbolo   | ☔            |
| Habilidad | Mojar        |
| Efecto    | Estado "Mojado" por 5 turnos, potencial de resbalar |

### Rayo
| Propiedad | Valor        |
|-----------|--------------|
| Nombre    | Rayo         |
| Símbolo   | ⚡            |
| Habilidad | Electrificar |
| Efecto    | Devuelve al jugador al inicio del juego |

### Skyhole
| Propiedad | Valor        |
|-----------|--------------|
| Nombre    | Skyhole      |
| Símbolo   | 🌀            |
| Habilidad | Teletransportar |
| Efecto    | Mueve al jugador a una posición aleatoria disponible |

    

### Informaciones dadas bajo el laberinto

 1. Nombre del jugador al que corresponde el turno.
    
 2. Teclas escenciales para comenzar a moverse (_ENTER_) o seleccionar la habilidad (_X_)
    
_NOTA: La habilidad de cada ficha solo puede ser seleccionada al inicio del turno del jugador, una vez comience a moverse, debera esperar a su proximo turno_

 3. Por cada paso, se mostrara al jugador:
    
 - Cantidad de pasos restantes que puede avanzar.
   
 - Cooldown de su habilidad.
   
 - Estado de la ficha (se explicara mas adelante en la definicion de cada trampa)


| Ejemplo           |
|-----------------------|
| Movimientos restantes: 5 |
| Cooldown de habilidad: 0 |
| Estado: Congelado       |




## CONTROLES
En _Sky Maze_, tienes varias opciones para mover tu ficha y utilizar habilidades:


**Flechas del Teclado:**
Flecha Arriba: Mueve tu ficha hacia arriba.
Flecha Abajo: Mueve tu ficha hacia abajo.
Flecha Izquierda: Mueve tu ficha hacia la izquierda.
Flecha Derecha: Mueve tu ficha hacia la derecha.


**Teclas WASD:**
W: Equivalente a la flecha arriba.
A: Equivalente a la flecha izquierda.
S: Equivalente a la flecha abajo.
D: Equivalente a la flecha derecha.


**X:**
Activa la habilidad especial asociada a tu ficha.


**Enter:**
Confirma el movimiento. Después de seleccionar la dirección con las flechas o WASD, presiona Enter para que tu ficha se mueva en esa dirección.



# ARQUITECTURA DEL JUEGO

## LOGICA

## sky-maze-logic.cs
Contiene la logica del Menu de Seleccion, crucial para el resto de configuracion del laberinto.

## Board.cs
Maneja la generacion, configuracion, y validacion del laberinto.

Con el valor de dimension que toma del menu de seleccion por el nivel de dificultad, se crea un array bidimensional tipo string, que se inicializa en primera instancia como paredes (_BoardInitializer()_). Esta clase se encarga de generar el laberinto con el algoritmo de Recursive Backtrack(_BoardGenerator()_). Una vez generado, para garantizar que la inicializacion de las fichas y el no sellado del centro, se crea un array bidimencional tipo int que detecta que casillas han sido inalcanzables (_DistanceValidator_). Por ultimo, un metodo corrije los errores de generacion en base a dicho array de enteros (_ValidatedBoard_), y esta listo.


## Ficha.cs
Define las fichas preestablecidas, y se definen sus propiedades Nombre, Simbolo. Posicion, Velocidad, CoolingTime, State, StateDuration, CurrentCoolingTime

Esta clase es quien se encarga de inicializar las fichas en el tablero, ademas que define los metodos de cada habilidad, asi como su modo de activacion


## Player.cs
Asigna a cada jugador un nombre, la ficha seleccionada, la posicion inicial de su ficha; ademas crea la lista de los jugadores en la partida, y su index respectivo para manejarlos. 


## Position.cs
Con las variables enteras x, y, es que se permite el movimiento y la localizacion de los objetos en el tablero. Con el metodo _Movement()_ es que cada jugador es capaz de manejar el movimiento de la ficha, ademas, detecta cuando una casilla es pared, es otr ficha, o si se cumple la condicion de victoria.


## Trampa.cs
Parecido a la clase Ficha.cs, se definen las trampas prestablecidas y se generan aleatoriamente en el laberinto (solo donde hay camino). La clase contiene los metodos DetectarTrampa() y ActivateTrampa() para controlar el funcionamiento de estas, y por ultimo se encuentran los metodos con las habilidades de cada una.


## sky-maze-ui.cs
Se encarga de la interfaz visual basica, utiliza principalmente la .NET library Sprectre Console para añadir colores y elementos visualmente atractivos.


## Program.cs
Controla el flujo principal del juego, los turnos, cooldown, movimiento, activacion de habilidades son todas manejadas aqui, desde el punto inicial llamando al inicio, hasta la revision de la condicion de victoria que culmina el juego. La clase Program coordina todos los aspectos del juego, desde la configuración inicial hasta la lógica de turnos y la interacción con el usuario, asegurando un flujo de juego coherente y manejable. Utiliza métodos de otras clases (GameUI, GameLogic, Board, Trampa, Ficha, Position, Player) para ejecutar las diferentes fases y funcionalidades del juego.

 
