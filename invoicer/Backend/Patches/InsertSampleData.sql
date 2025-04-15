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
INSERT INTO "Entity" ("Id","Ico","Name","Email","PhoneNumber","BankAccountId","AddressId","InvoiceNumberSchemeId") VALUES (1,'28915232','PERFECT SOUND GROUP s.r.o.','test@seznam.cz','777777778',1,1,1),
 (2,'09960676','Rohlik Group a.s.','rohlik@gmail.com','607493066',2,2,1),
 (3,'27082440','Alza.cz a.s.','alza@seznam.cz','604456123',3,3,1),
 (4,'26503808','WOOD & Company Financial Services, a.s.','wood@gmail.com','604896325',4,4,1);

-- EntityInvoiceNumberSchemeStates
INSERT INTO "EntityInvoiceNumberSchemeStates" ("EntityId","InvoiceNumberSchemeId","LastSequenceNumber","LastGenerationYear","LastGenerationMonth") VALUES (1,1,2,2025,4),
 (2,1,1,2025,4);

-- Invoice
INSERT INTO "Invoice" ("Id","SellerId","BuyerId","InvoiceNumber","IssueDate","DueDate","VatDate","Currency","PaymentMethod","DeliveryMethod","Status","InvoiceNumberSchemeId") VALUES (1,1,3,'INV001-2025-04','2025-04-15 12:14:00.831','2025-04-29 12:14:00.831','2025-04-15 12:14:00.831','CZK','BankTransfer','Courier','Paid',1),
 (2,1,3,'INV002-2025-04','2025-04-15 12:18:21.55','2025-04-29 12:18:21.55','2025-04-15 12:18:21.55','CZK','BankTransfer','Courier','Pending',1),
 (3,2,1,'INV001-2025-04','2025-04-15 12:19:27.217','2025-04-29 12:19:27.217','2025-04-15 12:19:27.217','CZK','BankTransfer','Courier','Overdue',1);

-- InvoiceItem
INSERT INTO "InvoiceItem" ("Id","InvoiceId","Unit","Quantity","Description","UnitPrice","VatRate") VALUES (1,1,'kg','4.0','Test','3.0','0.21'),
 (2,2,'Ks','5.0','Test 3','1000.0','0.21'),
 (3,3,'Ks','54.0','Banán','100.0','0.21');

-- InvoiceNumberScheme (it is inserted by default when starting the application)
-- INSERT INTO "InvoiceNumberScheme" ("Id","Prefix","UseSeperator","Seperator","SequencePosition","SequencePadding","InvoiceNumberYearFormat","IncludeMonth","ResetFrequency","IsDefault") VALUES (1,'INV',1,'-','Start',3,'FourDigit',1,'Yearly',1);
