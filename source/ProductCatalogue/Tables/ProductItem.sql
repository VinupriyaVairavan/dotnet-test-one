CREATE TABLE ProductItem (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    [ProductId] INT,
    [Name] NVARCHAR(50),
    [Description] NVARCHAR(100),
    [Price] DECIMAL(18, 2),
    [Stock] INT,
    [Status] NVARCHAR(50),
    [CreatedDate] DATETIME,
    [ModifiedDate] DATETIME,
    [DeletedDate] DATETIME,
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);