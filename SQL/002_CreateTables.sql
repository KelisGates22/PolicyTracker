USE PolicyTracker;
GO

DROP TABLE IF EXISTS Claims;
DROP TABLE IF EXISTS Policies;
DROP TABLE IF EXISTS Customers;
GO

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
GO