## Editor simple para CASIO FX-880P Windows 10 ##

*(No se si servirá para otros modelos)*

Está basado en el editor Mónaco (La base sobre la que está construido Visual Studio Code)
por lo que es bastante cómodo ya que cuenta con muchas de las opciones básicas como F7 para
buscar la palabra que está bajo el cursor, marca la anidación de paréntesis,
Ctrl + F para buscar.

Identifica los puertos COM activos, y puede cargar y descargar los programas con los comandos
* LOAD "COM0:5,N,8,1"   // Cargar un programa a la Casio (*Upload* en el programa)
* SAVE "COM0:5,N,8,1"   // Enviar un programa desde la Casio al Editor (*Download* en el Programa)

Hice este editor para contar con uno que funcionara bien con un adaptador muy básico que construí basado
en una pequeña placa que venden en aliexpress y que se usa para los arduino y esp32.

Pueden preguntarme libremente por los detalles, pero es muy simple de armar.

Si alguien más llega a usar este programa le agradeceré un correo, también agradeceré sugerencias de
mejoras ;)

Saludos,

Cristian Solervicéns C.
cscornely@gmail.com
