.\launch.ps1

Docker skal køre! Kør kun ovenstående kommando en gang.

Hver gang launch køres bliver password opdateret. Kig i user secrets og opdater i azura. (Kun hvis man vil se database i azura, ellers ligegyldigt)

Migrations:
dotnet ef migrations add Added_something  --verbose -p .\Assignment3.Entities\ -s .\Assignment3\
FACTORY SKAL VÆRE DER FOR AT DET VIRKER!! De er tilføjet pt, kommando behøves ikke køres. 


dotnet run --project .\Assignment3\


I context kan vi fjerne entities requirements, fx stringlenght og gøre det i klasserne i stedet. Det er mindre kludret.
Vi skal ikke lave det der many-to-many i context either, det holder databasen selv styr på, så længe vi har de der collections i klasserne. 
