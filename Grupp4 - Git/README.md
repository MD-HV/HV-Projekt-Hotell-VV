Git info för HV-Projekt-Hotell-VV

Skapa en sida med information om GIT och dess användning för Hotellet.

ladda ned: https://git-scm.com/downloads för att kunna använda GIT, VSCode/VS/Rider kan ge info om hur man laddar ned Git och GitExtensions.
https://desktop.github.com/download/ eller https://www.gitkraken.com/ som GUI för er som vill leka med GIT och GITHub

git init (skapar Git repo)

git status (kollar vad som finns i repo)

git add _fil.namn_ (lägger till filen i repo, ej commit)

git commit -m "text för commit" (sparar och lägger in filen i repo)

git log (visar commits och vem som har genomför dem, konto, tid och namn)

git clone _länk till repo_ (skapar en egen lokal version till din dator, efter clone ska en länk som: https://github.com/MD-HV/HV-Projekt-Hotell-VV.git existera)

git pull (drar ned senaste uppdateringarna av GitKoden till din kod)

git push (sparar din lokala förändringar till GitHub eller den online versionen)

git branch (skapar en ny branch)

git switch (byter till en annan branch)

git checkout (annat sätt att byta branch på, uppdaterar huvud branchen?)

git merge (lägger in de nya funktionerna från en branch till main eller en annan branch)

GIT kan sparas på en egen server, GitHub, GitLab, BitBucket, Azure, AWS, Cloud m.m.

Rebase, Cherrypick, Gitflow, GitHub Actions - Värt att kolla på för mer avancerade delar och funktioner.

För att dölja filer med Git så går det att antingen ha det i en egen "branch" där man skapar filer som inte syns i "main/master" delen och om filerna ej sparas genom en "commit" så går det att gömma det även på det sättet, eller så går det att lägga till specifika filer i en _.gitignore_ fil.
