# Generátor faktur – specifikace

## Název programu
**Invoicer**

## Popis
Invoicer je C# aplikace s webovım grafickım rozhraním navrená k automatizaci a zjednodušení procesu vytváøení faktur. Primárním uivatelem je mùj táta, kterı momentálnì vytváøí faktury ruènì. Tato aplikace mu vıznamnì usnadní práci tím, e automatizuje proces generování faktur.

V programu si bude moct uivatel vybrat èíslování faktur podle své preference, zadávat jednotlivé odbìratele a poskytovatele jako ekonomické subjekty a vyhledávat je podle IÈA.

## Pouité technologie a postupy
- **GUI:** Vue.js (nebo Blazor)
- **Backend:** ASP.net
- **Práce s API (NPRG038 - síování):** API ARES ([ARES API Swagger UI](https://ares.gov.cz/swagger-ui/))
- **Databáze (NPRG057 – ADO.net):** SQLite
- **PDF:** QuestPDF knihovna pro export faktur do PDF

## Hlavní funkcionalita
### Generování faktur
- Faktury budou obsahovat automaticky vyplnìné údaje po zadání IÈO pomocí API ARES.
- Uivatel si bude moct pøidávat subjekty (adresy, bankovní úèty).
- Uivatel uvidí, jaké faktury vygeneroval a spravuje.

### Uloení faktur
- Faktury budou uloeny v lokální databázi SQLite.

### Export faktur
- Monost exportovat faktury do PDF pro tisk nebo pøeposlání.
