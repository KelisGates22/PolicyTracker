USE PolicyTracker;

CREATE TABLE Customers (
    Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    FirstName       NVARCHAR(50)     NOT NULL,
    LastName        NVARCHAR(50)     NOT NULL,
    SSN             NVARCHAR(11)     NOT NULL,
    Email           NVARCHAR(100)    NOT NULL,
    Phone           NVARCHAR(20)     NULL,
    State           NVARCHAR(2)      NOT NULL,
    CreatedDate     DATETIME2        NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Policies (
    Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    CustomerId      UNIQUEIDENTIFIER NOT NULL,
    PolicyNumber    NVARCHAR(20)     NOT NULL UNIQUE,
    PolicyType      NVARCHAR(20)     NOT NULL,
    StartDate       DATE             NOT NULL,
    EndDate         DATE             NOT NULL,
    PremiumAmount   DECIMAL(10,2)    NOT NULL,
    Status          NVARCHAR(20)     NOT NULL DEFAULT 'Active',
    CreatedDate     DATETIME2        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Policies_Customers
        FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

CREATE TABLE Claims (
    Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    PolicyId        UNIQUEIDENTIFIER NOT NULL,
    ClaimDate       DATE             NOT NULL,
    Description     NVARCHAR(500)    NOT NULL,
    Amount          DECIMAL(10,2)    NOT NULL,
    Status          NVARCHAR(20)     NOT NULL DEFAULT 'Open',
    CreatedDate     DATETIME2        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Claims_Policies
        FOREIGN KEY (PolicyId) REFERENCES Policies(Id)
);

-- Declare customer GUIDs so we can reuse them in Policies
DECLARE @c1 UNIQUEIDENTIFIER = NEWID();
DECLARE @c2 UNIQUEIDENTIFIER = NEWID();
DECLARE @c3 UNIQUEIDENTIFIER = NEWID();
DECLARE @c4 UNIQUEIDENTIFIER = NEWID();
DECLARE @c5 UNIQUEIDENTIFIER = NEWID();
DECLARE @c6 UNIQUEIDENTIFIER = NEWID();
DECLARE @c7 UNIQUEIDENTIFIER = NEWID();
DECLARE @c8 UNIQUEIDENTIFIER = NEWID();
DECLARE @c9 UNIQUEIDENTIFIER = NEWID();
DECLARE @c10 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Customers (Id, FirstName, LastName, SSN, Email, Phone, State) VALUES
(@c1,  'Maria',   'Johnson',  '111-22-3333', 'maria.johnson@fakeemail.com',   '555-0101', 'GA'),
(@c2,  'James',   'Williams', '222-33-4444', 'james.williams@fakeemail.com',  '555-0102', 'OH'),
(@c3,  'Linda',   'Brown',    '333-44-5555', 'linda.brown@fakeemail.com',     '555-0103', 'TX'),
(@c4,  'Robert',  'Davis',    '444-55-6666', 'robert.davis@fakeemail.com',    '555-0104', 'FL'),
(@c5,  'Sarah',   'Miller',   '555-66-7777', 'sarah.miller@fakeemail.com',    '555-0105', 'OH'),
(@c6,  'Michael', 'Wilson',   '666-77-8888', 'michael.wilson@fakeemail.com',  '555-0106', 'GA'),
(@c7,  'Emily',   'Taylor',   '777-88-9999', 'emily.taylor@fakeemail.com',    '555-0107', 'CA'),
(@c8,  'David',   'Anderson', '888-99-0000', 'david.anderson@fakeemail.com',  '555-0108', 'NY'),
(@c9,  'Jessica', 'Thomas',   '999-00-1111', 'jessica.thomas@fakeemail.com',  '555-0109', 'OH'),
(@c10, 'Chris',   'Martinez', '000-11-2222', 'chris.martinez@fakeemail.com',  '555-0110', 'TX');

-- Declare policy GUIDs so we can reuse them in Claims
DECLARE @p1 UNIQUEIDENTIFIER = NEWID();
DECLARE @p2 UNIQUEIDENTIFIER = NEWID();
DECLARE @p3 UNIQUEIDENTIFIER = NEWID();
DECLARE @p4 UNIQUEIDENTIFIER = NEWID();
DECLARE @p5 UNIQUEIDENTIFIER = NEWID();
DECLARE @p6 UNIQUEIDENTIFIER = NEWID();
DECLARE @p7 UNIQUEIDENTIFIER = NEWID();
DECLARE @p8 UNIQUEIDENTIFIER = NEWID();
DECLARE @p9 UNIQUEIDENTIFIER = NEWID();
DECLARE @p10 UNIQUEIDENTIFIER = NEWID();
DECLARE @p11 UNIQUEIDENTIFIER = NEWID();
DECLARE @p12 UNIQUEIDENTIFIER = NEWID();
DECLARE @p13 UNIQUEIDENTIFIER = NEWID();
DECLARE @p14 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Policies (Id, CustomerId, PolicyNumber, PolicyType, StartDate, EndDate, PremiumAmount, Status) VALUES
(@p1,  @c1,  'POL-AUTO-001', 'Auto', '2025-01-15', '2026-01-15', 1200.00, 'Active'),
(@p2,  @c1,  'POL-HOME-001', 'Home', '2025-03-01', '2026-03-01', 1800.00, 'Active'),
(@p3,  @c2,  'POL-AUTO-002', 'Auto', '2025-02-01', '2026-02-01',  950.00, 'Active'),
(@p4,  @c3,  'POL-AUTO-003', 'Auto', '2024-06-15', '2025-06-15', 1100.00, 'Expired'),
(@p5,  @c3,  'POL-HOME-002', 'Home', '2025-04-01', '2026-04-01', 2200.00, 'Active'),
(@p6,  @c4,  'POL-LIFE-001', 'Life', '2025-01-01', '2035-01-01',  450.00, 'Active'),
(@p7,  @c5,  'POL-AUTO-004', 'Auto', '2025-05-01', '2026-05-01', 1050.00, 'Active'),
(@p8,  @c6,  'POL-AUTO-005', 'Auto', '2025-03-15', '2026-03-15', 1300.00, 'Active'),
(@p9,  @c6,  'POL-HOME-003', 'Home', '2025-06-01', '2026-06-01', 1600.00, 'Active'),
(@p10, @c7,  'POL-AUTO-006', 'Auto', '2024-12-01', '2025-12-01',  980.00, 'Active'),
(@p11, @c8,  'POL-LIFE-002', 'Life', '2025-02-15', '2035-02-15',  550.00, 'Active'),
(@p12, @c9,  'POL-AUTO-007', 'Auto', '2025-07-01', '2026-07-01', 1150.00, 'Active'),
(@p13, @c10, 'POL-AUTO-008', 'Auto', '2025-01-01', '2026-01-01', 1400.00, 'Active'),
(@p14, @c10, 'POL-HOME-004', 'Home', '2025-05-15', '2026-05-15', 1900.00, 'Active');

INSERT INTO Claims (PolicyId, ClaimDate, Description, Amount, Status) VALUES
(@p1,  '2025-06-10', 'Rear-end collision at intersection',     3500.00, 'Approved'),
(@p1,  '2025-09-22', 'Windshield cracked by road debris',       800.00, 'Open'),
(@p3,  '2025-07-05', 'Hail damage to vehicle hood and roof',   2200.00, 'Under Review'),
(@p4,  '2025-03-18', 'Side mirror struck in parking lot',        450.00, 'Closed'),
(@p5,  '2025-08-30', 'Water damage from burst pipe',           8500.00, 'Open'),
(@p7,  '2025-10-01', 'Fender bender in grocery store lot',     1200.00, 'Open'),
(@p9,  '2025-09-15', 'Roof damage from fallen tree branch',    4200.00, 'Under Review'),
(@p10, '2025-11-02', 'Theft of personal items from vehicle',   1800.00, 'Open'),
(@p13, '2025-08-20', 'Deer collision on highway',              5600.00, 'Approved'),
(@p14, '2025-10-10', 'Kitchen fire - smoke and heat damage',  12000.00, 'Under Review');

SELECT FirstName, LastName, SSN FROM Customers;
SELECT Id, FirstName FROM Customers;