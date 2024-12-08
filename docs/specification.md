# Gener�tor faktur � specifikace

## N�zev programu
**Invoicer**

## Popis
Invoicer je C# aplikace s webov�m grafick�m rozhran�m navr�en� k automatizaci a zjednodu�en� procesu vytv��en� faktur. Prim�rn�m u�ivatelem je m�j t�ta, kter� moment�ln� vytv��� faktury ru�n�. Tato aplikace mu v�znamn� usnadn� pr�ci t�m, �e automatizuje proces generov�n� faktur.

V programu si bude moct u�ivatel vybrat ��slov�n� faktur podle sv� preference, zad�vat jednotliv� odb�ratele a poskytovatele jako ekonomick� subjekty a vyhled�vat je podle I�A.

## Pou�it� technologie a postupy
- **GUI:** Vue.js (nebo Blazor)
- **Backend:** ASP.net
- **Pr�ce s API (NPRG038 - s�ov�n�):** API ARES ([ARES API Swagger UI](https://ares.gov.cz/swagger-ui/))
- **Datab�ze (NPRG057 � ADO.net):** SQLite
- **PDF:** QuestPDF knihovna pro export faktur do PDF

## Hlavn� funkcionalita
### Generov�n� faktur
- Faktury budou obsahovat automaticky vypln�n� �daje po zad�n� I�O pomoc� API ARES.
- U�ivatel si bude moct p�id�vat subjekty (adresy, bankovn� ��ty).
- U�ivatel uvid�, jak� faktury vygeneroval a spravuje.

### Ulo�en� faktur
- Faktury budou ulo�eny v lok�ln� datab�zi SQLite.

### Export faktur
- Mo�nost exportovat faktury do PDF pro tisk nebo p�eposl�n�.
