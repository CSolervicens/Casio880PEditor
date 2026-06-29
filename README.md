## Editor simple para CASIO FX-880P (No se si servirá para otros modelos) ##
Está basado en el editor Mónaco (La base sobre la que está construido Visual Studio Code)
por lo que es bastante cómodo ya que cuenta con muchas de las opciones básicas como F7 para
buscar la palabra que está bajo el cursor, marca la anidación de paréntesis,
Ctrl + F para buscar.

Identifica los puertos COM activos, y puede cargar y descargar los programas con los comandos
* LOAD "COM0:5,N,8,1"   // Cargar un programa a la Casio (*Upload* en el programa)
* SAVE "COM0:5,N,8,1"   // Enviar un programa desde la Casio al Editor (*Download* en el Programa)
