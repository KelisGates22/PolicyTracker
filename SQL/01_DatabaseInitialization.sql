USE PolicyTracker;

CREATE TABLE Customers (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    FirstName       NVARCHAR(50)   NOT NULL,
    LastName        NVARCHAR(50)   NOT NULL,
    SSN             NVARCHAR(11)   NOT NULL,
    Email           NVARCHAR(100)  NOT NULL,
    Phone           NVARCHAR(20)   NULL,
    State           NVARCHAR(2)    NOT NULL,
    CreatedDate     DATETIME2      NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Policies (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId      INT            NOT NULL,
    PolicyNumber    NVARCHAR(20)   NOT NULL UNIQUE,
    PolicyType      NVARCHAR(20)   NOT NULL,
    StartDate       DATE           NOT NULL,
    EndDate         DATE           NOT NULL,
    PremiumAmount   DECIMAL(10,2)  NOT NULL,
    Status          NVARCHAR(20)   NOT NULL DEFAULT 'Active',
    CreatedDate     DATETIME2      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Policies_Customers
        FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

CREATE TABLE Claims (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    PolicyId        INT            NOT NULL,
    ClaimDate       DATE           NOT NULL,
    Description     NVARCHAR(500)  NOT NULL,
    Amount          DECIMAL(10,2)  NOT NULL,
    Status          NVARCHAR(20)   NOT NULL DEFAULT 'Open',
    CreatedDate     DATETIME2      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Claims_Policies
        FOREIGN KEY (PolicyId) REFERENCES Policies(Id)
);

USE PolicyTracker;

INSERT INTO Customers (FirstName, LastName, SSN, Email, Phone, State) VALUES
('Maria',   'Johnson',  '111-22-3333', 'maria.johnson@fakeemail.com',   '555-0101', 'GA'),
('James',   'Williams', '222-33-4444', 'james.williams@fakeemail.com',  '555-0102', 'OH'),
('Linda',   'Brown',    '333-44-5555', 'linda.brown@fakeemail.com',     '555-0103', 'TX'),
('Robert',  'Davis',    '444-55-6666', 'robert.davis@fakeemail.com',    '555-0104', 'FL'),
('Sarah',   'Miller',   '555-66-7777', 'sarah.miller@fakeemail.com',    '555-0105', 'OH'),
('Michael', 'Wilson',   '666-77-8888', 'michael.wilson@fakeemail.com',  '555-0106', 'GA'),
('Emily',   'Taylor',   '777-88-9999', 'emily.taylor@fakeemail.com',    '555-0107', 'CA'),
('David',   'Anderson', '888-99-0000', 'david.anderson@fakeemail.com',  '555-0108', 'NY'),
('Jessica', 'Thomas',   '999-00-1111', 'jessica.thomas@fakeemail.com',  '555-0109', 'OH'),
('Chris',   'Martinez', '000-11-2222', 'chris.martinez@fakeemail.com',  '555-0110', 'TX');


INSERT INTO Policies (CustomerId, PolicyNumber, PolicyType, StartDate, EndDate, PremiumAmount, Status) VALUES
(1,  'POL-AUTO-001', 'Auto', '2025-01-15', '2026-01-15', 1200.00, 'Active'),
(1,  'POL-HOME-001', 'Home', '2025-03-01', '2026-03-01', 1800.00, 'Active'),
(2,  'POL-AUTO-002', 'Auto', '2025-02-01', '2026-02-01',  950.00, 'Active'),
(3,  'POL-AUTO-003', 'Auto', '2024-06-15', '2025-06-15', 1100.00, 'Expired'),
(3,  'POL-HOME-002', 'Home', '2025-04-01', '2026-04-01', 2200.00, 'Active'),
(4,  'POL-LIFE-001', 'Life', '2025-01-01', '2035-01-01',  450.00, 'Active'),
(5,  'POL-AUTO-004', 'Auto', '2025-05-01', '2026-05-01', 1050.00, 'Active'),
(6,  'POL-AUTO-005', 'Auto', '2025-03-15', '2026-03-15', 1300.00, 'Active'),
(6,  'POL-HOME-003', 'Home', '2025-06-01', '2026-06-01', 1600.00, 'Active'),
(7,  'POL-AUTO-006', 'Auto', '2024-12-01', '2025-12-01',  980.00, 'Active'),
(8,  'POL-LIFE-002', 'Life', '2025-02-15', '2035-02-15',  550.00, 'Active'),
(9,  'POL-AUTO-007', 'Auto', '2025-07-01', '2026-07-01', 1150.00, 'Active'),
(10, 'POL-AUTO-008', 'Auto', '2025-01-01', '2026-01-01', 1400.00, 'Active'),
(10, 'POL-HOME-004', 'Home', '2025-05-15', '2026-05-15', 1900.00, 'Active');

INSERT INTO Claims (PolicyId, ClaimDate, Description, Amount, Status) VALUES
(1,  '2025-06-10', 'Rear-end collision at intersection',     3500.00, 'Approved'),
(1,  '2025-09-22', 'Windshield cracked by road debris',       800.00, 'Open'),
(2,  '2025-07-05', 'Hail damage to vehicle hood and roof',   2200.00, 'Under Review'),
(3,  '2025-03-18', 'Side mirror struck in parking lot',        450.00, 'Closed'),
(5,  '2025-08-30', 'Water damage from burst pipe',           8500.00, 'Open'),
(7,  '2025-10-01', 'Fender bender in grocery store lot',     1200.00, 'Open'),
(9,  '2025-09-15', 'Roof damage from fallen tree branch',    4200.00, 'Under Review'),
(10, '2025-11-02', 'Theft of personal items from vehicle',   1800.00, 'Open'),
(13, '2025-08-20', 'Deer collision on highway',              5600.00, 'Approved'),
(14, '2025-10-10', 'Kitchen fire — smoke and heat damage',  12000.00, 'Under Review');


SELECT * FROM Customers;
SELECT * FROM Policies;
SELECT * FROM Claims;



SELECT FirstName, LastName, SSN FROM Customers;