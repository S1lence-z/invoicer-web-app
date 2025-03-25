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

-- Insert sample invoices
INSERT INTO Invoice (Id, SellerId, BuyerId, InvoiceNumber, IssueDate, DueDate, Currency, PaymentMethod, DeliveryMethod) VALUES
(1, 1, 2, 'INV-2025001', '2025-03-13', '2025-03-20', 'CZK', 'BankTransfer', 'PersonalPickUp'),
(2, 2, 1, 'INV-2025002', '2025-03-14', '2025-03-21', 'CZK', 'Cash', 'Courier');

-- Insert sample invoice items
INSERT INTO InvoiceItem (Id, InvoiceId, Unit, Quantity, Description, UnitPrice) VALUES
(1, 1, 'pcs', 5, 'Office Chairs', 1500.00),
(2, 1, 'pcs', 2, 'Desks', 3000.00),
(3, 2, 'pcs', 10, 'Monitors', 5000.00),
(4, 2, 'pcs', 4, 'Keyboards', 800.00);
