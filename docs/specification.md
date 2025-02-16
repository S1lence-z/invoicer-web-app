# Generátor faktur – specifikace

## Název programu
**Invoicer**

## Popis
Invoicer je C# aplikace s webovým grafickým rozhraním navržená k automatizaci a zjednodušení procesu vytváření faktur. Primárním uživatelem je můj táta, který momentálně vytváří faktury ručně. Tato aplikace mu významně usnadní práci tím, že automatizuje proces generování faktur.

V programu si bude moct uživatel vybrat číslování faktur podle své preference, zadávat jednotlivé odběratele a poskytovatele jako entity a vyhledávat je podle IČA.

## Použité technologie a postupy
- **GUI:** Blazor
- **Backend:** ASP.NET
- **Práce s API (NPRG038 - síťování):** API ARES ([ARES API Swagger UI](https://ares.gov.cz/swagger-ui/))
- **Databáze (NPRG057 – ADO.net):** SQLite
- **PDF:** QuestPDF knihovna pro export faktur do PDF

## Hlavní funkcionalita
### Generování faktur
- Faktury mohou být předvyplněné po zadání IČA dané entity pomocí ARES API.
- Uživatel si bude moct přidávat subjekty (adresy, bankovní účty).
- Uživatel uvidí, jaké faktury vygeneroval a spravuje.

### Uložení faktur
- Faktury budou uloženy v lokální databázi SQLite.

### Export faktur
- Možnost exportovat faktury do PDF pro tisk nebo přeposlání.
