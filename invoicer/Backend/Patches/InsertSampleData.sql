-- Insert sample addresses
INSERT INTO Address (Id, Street, City, ZipCode, Country) VALUES
(1, '123 Main St', 'Prague', '11000', 'Czech Republic'),
(2, '456 Business Rd', 'Brno', '60200', 'Czech Republic');

-- Insert sample bank accounts
INSERT INTO BankAccount (Id, AccountNumber, BankCode, BankName, IBAN) VALUES
(1, '123456789', '0100', 'Komerèní banka', 'CZ650100000000123456789'),
(2, '987654321', '0300', 'Èeskoslovenská obchodní banka', 'CZ650300000000987654321');

-- Insert sample entities (companies)
INSERT INTO Entity (Id, Ico, Name, Email, PhoneNumber, BankAccountId, AddressId) VALUES
(1, '12345678', 'ABC s.r.o.', 'info@abc.cz', '+420123456789', 1, 1),
(2, '87654321', 'XYZ Ltd.', 'contact@xyz.com', '+420987654321', 2, 2);
