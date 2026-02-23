# Stock Flow

## Projectbeschrijving

Momenteel kan je inloggen, registreren op Stock Flow. <br> 
De google implementatie werkt enkel nogmaar op Android. <br>
Ik zal later kijken om dit volledig bij te werken eenmaal het project verder is. <br>

Je komt terecht op de dashboard, waarbij je dan de eerste drie zaken kan bezichten en dit is Scanning,Products,Warehouses en een klein deeltje van Operations. <br>
De andere modules zijn nog in progress. <br> 

Dit is verbonden met een api waarbij je de dev tunnel moet gebruiken. Dit kan je vinden in `Mde.Project.Mobile.Core` in het bestand `Constants.cs`. <br>

Uiteindelijk zal het grootste werk gebeuren in `Operations` waar ik dan zal proberen om een robust systeem te maken zodat de stock altijd voorzien is van genoeg producten alvolgens de configuratie van de warehouse zelf.

## Extra info
Plaats hier de nodig informatie om het
project te kunnen uitvoeren:

- Api key zit momenteel in `Constants.cs` en ook hier zal u dde devtunnel moeten aanpassen.
- In `Mde.Project.Api.Core` zit de database migratie die je kan laten uitvoeren. En ddit zal natuurlijk noddig zijn om de app te testen.
