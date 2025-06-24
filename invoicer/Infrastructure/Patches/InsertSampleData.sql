-- Address
INSERT INTO "Address" ("Id","Street","City","ZipCode","Country") VALUES (1,'Pod vinicí 1432','Praha',14300,'Èeská republika'),
 (2,'Karolinská 654','Praha',18600,'Èeská republika'),
 (3,'Jankovcova 1522','Praha',17000,'Èeská republika'),
 (4,'Námìstí Republiky 1079','Praha',11000,'Èeská republika');

-- BankAccount
INSERT INTO "BankAccount" ("Id","AccountNumber","BankCode","BankName","IBAN") VALUES (1,'3555000022','6363','Partners',''),
 (2,'3555000021','0100','KB',''),
 (3,'3500000212','0800','Èeská spoøitelna',''),
 (4,'1234000005','0770','Think Bank','');

-- Entity
INSERT INTO "Entity" ("Id","Ico","Name","Email","PhoneNumber","BankAccountId","AddressId","CurrentNumberingSchemeId") VALUES (1,'28915232','PERFECT SOUND GROUP s.r.o.','test@seznam.cz','777777778',1,1,1),
 (2,'09960676','Rohlik Group a.s.','rohlik@gmail.com','607493066',2,2,1),
 (3,'27082440','Alza.cz a.s.','alza@seznam.cz','604456123',3,3,1),
 (4,'26503808','WOOD & Company Financial Services, a.s.','wood@gmail.com','604896325',4,4,1);

-- EntityInvoiceNumberSchemeStates
INSERT INTO "EntityInvoiceNumberingSchemeState" ("EntityId","LastSequenceNumber","LastGenerationYear","LastGenerationMonth","NumberingSchemeId") VALUES (1,0,2025,5,1), (2,0,2025,5,1), (3,0,2025,5,1), (4,0,2025,5,1);

