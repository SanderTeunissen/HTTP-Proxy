# HTTP Proxyserver

De HTTP Proxy was een opdracht voor de course Web Infrastructures van het semester .Net on the Server aan de Hogeschool van Arnhem en Nijmegen. De opdracht was om in C# .Net een proxy server te bouwen, hiervoor mocht geen gebruik worden gemaakt van de standaard http classes van .Net. Alle communicatie moest vanaf het TCP niveau worden opgebouwd. 

-----

The HTTP Proxy was an assignment for the course Web Infrastructures of the semester .Net on the Server at the Hogeschool of Arnhem and Nijmegen. The assignment was to build a proxy server in C # .Net, for this it was not allowed to use the standard http classes of .Net. All communication had to be built up from the TCP level.


## Requirements

- De proxy kan zowel met HTTP1.0 als HTTP1.1 overweg.
- De proxy kan de identiteit van de client beschermen t.o.v. de server, dit gebeurt door middel van het verwijderen van de user-agent header uit het request.
- De proxy verhoogt de response snelheid door objecten te cachen. Dit wordt standaard gedaan wanneer een item vaker als 5 keer gebruikt wordt of altijd wanneer deze optie is ingeschakeld in de UI. 
- De tijd dat de objecten gecached worden is ook instelbaar vanuit de UI.
- De proxy kan afbeeldingen vervangen door een standaard afbeelding. Deze optie is in te schakelen in de UI.
- De proxy heeft de mogelijkheid om in te stellen dat clients in moeten loggen met een gebruikersnaam en wachtwoord. Het is niet nodig om een database aan te leggen. Een functie die username/password controleert is voldoende.

-----

- The proxy can handle both HTTP1.0 and HTTP1.1.
- The proxy can protect the identity of the client with respect to the server, this is done by removing the user-agent header from the request.
- The proxy increases the response speed by caching objects. This is done by default when an item is used more than 5 times or always when this option is enabled in the UI.
- The time that the objects are cached is also adjustable from the UI.
- The proxy can replace images with a standard image. This option can be enabled in the UI.
- The proxy has the ability to set clients to log in with a user name and password. There is no need to create a database. A function that checks username / password is sufficient.


## Class diagram

![Class diagram](./ProxyClassDiagram.png)



## Flow chart

![Flow chart](./flowchart.jpg)
