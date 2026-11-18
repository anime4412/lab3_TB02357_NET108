IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Authors] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [Biography] nvarchar(max) NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Authors] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [EmployeeCode] nvarchar(20) NOT NULL,
    [FullName] nvarchar(150) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Password] nvarchar(255) NOT NULL,
    [Phone] nvarchar(15) NULL,
    [Role] nvarchar(50) NOT NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Publishers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [Address] nvarchar(250) NULL,
    [Phone] nvarchar(20) NULL,
    [Email] nvarchar(100) NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Publishers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Readers] (
    [Id] int NOT NULL IDENTITY,
    [ReaderCode] nvarchar(20) NOT NULL,
    [FullName] nvarchar(150) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Password] nvarchar(255) NOT NULL,
    [Phone] nvarchar(15) NULL,
    [Address] nvarchar(250) NULL,
    [IdentityNumber] nvarchar(20) NULL,
    [RegisterDate] datetime2 NOT NULL,
    [ExpiryDate] datetime2 NOT NULL,
    [MaxBorrowLimit] int NOT NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Readers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Shelves] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Location] nvarchar(200) NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Shelves] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [BorrowSlips] (
    [Id] int NOT NULL IDENTITY,
    [SlipCode] nvarchar(20) NOT NULL,
    [ReaderId] int NOT NULL,
    [EmployeeId] int NULL,
    [BorrowDate] datetime2 NOT NULL,
    [DueDate] datetime2 NOT NULL,
    [ActualReturnDate] datetime2 NULL,
    [Status] nvarchar(50) NOT NULL,
    [DepositAmount] decimal(18,2) NOT NULL,
    [Notes] nvarchar(500) NULL,
    CONSTRAINT [PK_BorrowSlips] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BorrowSlips_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_BorrowSlips_Readers_ReaderId] FOREIGN KEY ([ReaderId]) REFERENCES [Readers] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Books] (
    [Id] int NOT NULL IDENTITY,
    [ISBN] nvarchar(20) NOT NULL,
    [Title] nvarchar(250) NOT NULL,
    [CategoryId] int NULL,
    [AuthorId] int NULL,
    [PublisherId] int NULL,
    [ShelfId] int NULL,
    [PublishYear] int NOT NULL,
    [PageCount] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Quantity] int NOT NULL,
    [AvailableQuantity] int NOT NULL,
    [CoverImage] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Books_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Authors] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Books_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Books_Publishers_PublisherId] FOREIGN KEY ([PublisherId]) REFERENCES [Publishers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Books_Shelves_ShelfId] FOREIGN KEY ([ShelfId]) REFERENCES [Shelves] ([Id]) ON DELETE SET NULL
);
GO

CREATE TABLE [Fines] (
    [Id] int NOT NULL IDENTITY,
    [BorrowSlipId] int NOT NULL,
    [ReaderId] int NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [PaidAmount] decimal(18,2) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [Notes] nvarchar(500) NULL,
    CONSTRAINT [PK_Fines] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Fines_BorrowSlips_BorrowSlipId] FOREIGN KEY ([BorrowSlipId]) REFERENCES [BorrowSlips] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Fines_Readers_ReaderId] FOREIGN KEY ([ReaderId]) REFERENCES [Readers] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [BorrowDetails] (
    [Id] int NOT NULL IDENTITY,
    [BorrowSlipId] int NOT NULL,
    [BookId] int NOT NULL,
    [Quantity] int NOT NULL,
    [RentalPrice] decimal(18,2) NOT NULL,
    [IsReturned] bit NOT NULL,
    [ReturnDate] datetime2 NULL,
    [ConditionStatus] nvarchar(100) NOT NULL,
    [FineAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_BorrowDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BorrowDetails_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_BorrowDetails_BorrowSlips_BorrowSlipId] FOREIGN KEY ([BorrowSlipId]) REFERENCES [BorrowSlips] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Biography', N'Name') AND [object_id] = OBJECT_ID(N'[Authors]'))
    SET IDENTITY_INSERT [Authors] ON;
INSERT INTO [Authors] ([Id], [Active], [Biography], [Name])
VALUES (1, CAST(1 AS bit), N'Chuyên gia kiến trúc phần mềm, tác giả Clean Code', N'Robert C. Martin (Uncle Bob)'),
(2, CAST(1 AS bit), N'Tác giả cuốn The Pragmatic Programmer', N'Andrew Hunt & David Thomas'),
(3, CAST(1 AS bit), N'Tác giả Đắc Nhân Tâm', N'Dale Carnegie'),
(4, CAST(1 AS bit), N'Nhà văn nổi tiếng Việt Nam', N'Nguyễn Nhật Ánh'),
(5, CAST(1 AS bit), N'Tác giả Sapiens: Lược sử loài người', N'Yuval Noah Harari');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Biography', N'Name') AND [object_id] = OBJECT_ID(N'[Authors]'))
    SET IDENTITY_INSERT [Authors] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] ON;
INSERT INTO [Categories] ([Id], [Active], [Description], [Name])
VALUES (1, CAST(1 AS bit), N'Sách lập trình, cơ sở dữ liệu, mạng máy tính, AI', N'Công nghệ thông tin'),
(2, CAST(1 AS bit), N'Quản trị kinh doanh, tài chính, khởi nghiệp', N'Kinh tế & Quản trị'),
(3, CAST(1 AS bit), N'Tiểu thuyết, truyện ngắn, tác phẩm kinh điển', N'Văn học & Nghệ thuật'),
(4, CAST(1 AS bit), N'Tư duy, giao tiếp, tâm lý học ứng dụng', N'Kỹ năng sống & Phát triển bản thân'),
(5, CAST(1 AS bit), N'Toán học, vật lý, thiên văn học', N'Khoa học cơ bản');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Email', N'EmployeeCode', N'FullName', N'Password', N'Phone', N'Role') AND [object_id] = OBJECT_ID(N'[Employees]'))
    SET IDENTITY_INSERT [Employees] ON;
INSERT INTO [Employees] ([Id], [Active], [Email], [EmployeeCode], [FullName], [Password], [Phone], [Role])
VALUES (1, CAST(1 AS bit), N'admin@thuvien.edu.vn', N'NV001', N'Nguyễn Quản Trị', N'Admin@123', N'0911223344', N'Admin'),
(2, CAST(1 AS bit), N'thuthu01@thuvien.edu.vn', N'NV002', N'Trần Thị Thủ Thư', N'Thuthu@123', N'0988776655', N'Librarian');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Email', N'EmployeeCode', N'FullName', N'Password', N'Phone', N'Role') AND [object_id] = OBJECT_ID(N'[Employees]'))
    SET IDENTITY_INSERT [Employees] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Address', N'Email', N'Name', N'Phone') AND [object_id] = OBJECT_ID(N'[Publishers]'))
    SET IDENTITY_INSERT [Publishers] ON;
INSERT INTO [Publishers] ([Id], [Active], [Address], [Email], [Name], [Phone])
VALUES (1, CAST(1 AS bit), N'Hà Nội', N'nxbtt@gmail.com', N'NXB Thông tin và Truyền thông', N'02435770000'),
(2, CAST(1 AS bit), N'TP. Hồ Chí Minh', N'nxbtre@hcm.fpt.vn', N'NXB Trẻ', N'02839316289'),
(3, CAST(1 AS bit), N'Hà Nội', N'info@nxbkimdong.com.vn', N'NXB Kim Đồng', N'02439434730'),
(4, CAST(1 AS bit), N'Hà Nội', N'nxbkhkt@hn.vnn.vn', N'NXB Khoa học và Kỹ thuật', N'02438253164');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Address', N'Email', N'Name', N'Phone') AND [object_id] = OBJECT_ID(N'[Publishers]'))
    SET IDENTITY_INSERT [Publishers] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Address', N'Email', N'ExpiryDate', N'FullName', N'IdentityNumber', N'MaxBorrowLimit', N'Password', N'Phone', N'ReaderCode', N'RegisterDate') AND [object_id] = OBJECT_ID(N'[Readers]'))
    SET IDENTITY_INSERT [Readers] ON;
INSERT INTO [Readers] ([Id], [Active], [Address], [Email], [ExpiryDate], [FullName], [IdentityNumber], [MaxBorrowLimit], [Password], [Phone], [ReaderCode], [RegisterDate])
VALUES (1, CAST(1 AS bit), N'Cầu Giấy, Hà Nội', N'tuan.tm@gmail.com', '2027-01-01T00:00:00.0000000', N'Trần Minh Tuấn', N'001202001122', 5, N'123456', N'0933112233', N'DG001', '2026-01-01T00:00:00.0000000'),
(2, CAST(1 AS bit), N'Bắc Từ Liêm, Hà Nội', N'yen.lh@gmail.com', '2027-01-01T00:00:00.0000000', N'Lê Hoàng Yến', N'001202003344', 5, N'123456', N'0944223344', N'DG002', '2026-01-01T00:00:00.0000000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Address', N'Email', N'ExpiryDate', N'FullName', N'IdentityNumber', N'MaxBorrowLimit', N'Password', N'Phone', N'ReaderCode', N'RegisterDate') AND [object_id] = OBJECT_ID(N'[Readers]'))
    SET IDENTITY_INSERT [Readers] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Location', N'Name') AND [object_id] = OBJECT_ID(N'[Shelves]'))
    SET IDENTITY_INSERT [Shelves] ON;
INSERT INTO [Shelves] ([Id], [Active], [Location], [Name])
VALUES (1, CAST(1 AS bit), N'Tầng 2 - Dãy A bên trái', N'Kệ A1 - CNTT'),
(2, CAST(1 AS bit), N'Tầng 2 - Dãy A bên phải', N'Kệ A2 - Lập trình .NET'),
(3, CAST(1 AS bit), N'Tầng 1 - Dãy B trung tâm', N'Kệ B1 - Kinh tế'),
(4, CAST(1 AS bit), N'Tầng 1 - Dãy C', N'Kệ C1 - Văn học');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Location', N'Name') AND [object_id] = OBJECT_ID(N'[Shelves]'))
    SET IDENTITY_INSERT [Shelves] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'AuthorId', N'AvailableQuantity', N'CategoryId', N'CoverImage', N'Description', N'ISBN', N'PageCount', N'Price', N'PublishYear', N'PublisherId', N'Quantity', N'ShelfId', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] ON;
INSERT INTO [Books] ([Id], [Active], [AuthorId], [AvailableQuantity], [CategoryId], [CoverImage], [Description], [ISBN], [PageCount], [Price], [PublishYear], [PublisherId], [Quantity], [ShelfId], [Title])
VALUES (1, CAST(1 AS bit), 1, 8, 1, N'cleancode.jpg', N'Cuốn sách kinh điển về kỹ nghệ viết mã sạch, tối ưu và dễ bảo trì.', N'9780132350884', 464, 250000.0, 2021, 1, 10, 2, N'Clean Code: A Handbook of Agile Software Craftsmanship'),
(2, CAST(1 AS bit), 2, 7, 1, N'pragmatic.jpg', N'Hành trình từ lập trình viên nghiệp dư trở thành bậc thầy phát triển phần mềm.', N'9780201616224', 352, 280000.0, 2022, 1, 8, 2, N'The Pragmatic Programmer: Your Journey To Mastery'),
(3, CAST(1 AS bit), 3, 14, 4, N'dacnhantam.jpg', N'Nghệ thuật thu phục lòng người và xây dựng mối quan hệ bền vững.', N'9786045882885', 320, 95000.0, 2023, 2, 15, 3, N'Đắc Nhân Tâm (How to Win Friends and Influence People)'),
(4, CAST(1 AS bit), 4, 11, 3, N'matbiec.jpg', N'Tác phẩm văn học lãng mạn đầy cảm xúc của nhà văn Nguyễn Nhật Ánh.', N'9786042171120', 300, 110000.0, 2020, 3, 12, 4, N'Mắt Biếc'),
(5, CAST(1 AS bit), 5, 10, 5, N'sapiens.jpg', N'Khám phá toàn cảnh lịch sử tiến hóa và văn minh nhân loại.', N'9780062316097', 512, 185000.0, 2022, 4, 10, 3, N'Sapiens: Lược Sử Loài Người');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'AuthorId', N'AvailableQuantity', N'CategoryId', N'CoverImage', N'Description', N'ISBN', N'PageCount', N'Price', N'PublishYear', N'PublisherId', N'Quantity', N'ShelfId', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ActualReturnDate', N'BorrowDate', N'DepositAmount', N'DueDate', N'EmployeeId', N'Notes', N'ReaderId', N'SlipCode', N'Status') AND [object_id] = OBJECT_ID(N'[BorrowSlips]'))
    SET IDENTITY_INSERT [BorrowSlips] ON;
INSERT INTO [BorrowSlips] ([Id], [ActualReturnDate], [BorrowDate], [DepositAmount], [DueDate], [EmployeeId], [Notes], [ReaderId], [SlipCode], [Status])
VALUES (1, NULL, '2026-11-01T00:00:00.0000000', 100000.0, '2026-11-15T00:00:00.0000000', 2, N'Mượn 2 cuốn sách CNTT kỳ 1', 1, N'PM0001', N'Borrowing'),
(2, NULL, '2026-11-02T00:00:00.0000000', 50000.0, '2026-11-16T00:00:00.0000000', 2, N'Mượn sách văn học', 2, N'PM0002', N'Borrowing');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ActualReturnDate', N'BorrowDate', N'DepositAmount', N'DueDate', N'EmployeeId', N'Notes', N'ReaderId', N'SlipCode', N'Status') AND [object_id] = OBJECT_ID(N'[BorrowSlips]'))
    SET IDENTITY_INSERT [BorrowSlips] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BookId', N'BorrowSlipId', N'ConditionStatus', N'FineAmount', N'IsReturned', N'Quantity', N'RentalPrice', N'ReturnDate') AND [object_id] = OBJECT_ID(N'[BorrowDetails]'))
    SET IDENTITY_INSERT [BorrowDetails] ON;
INSERT INTO [BorrowDetails] ([Id], [BookId], [BorrowSlipId], [ConditionStatus], [FineAmount], [IsReturned], [Quantity], [RentalPrice], [ReturnDate])
VALUES (1, 1, 1, N'Nguyên vẹn', 0.0, CAST(0 AS bit), 1, 10000.0, NULL),
(2, 2, 1, N'Nguyên vẹn', 0.0, CAST(0 AS bit), 1, 10000.0, NULL),
(3, 4, 2, N'Nguyên vẹn', 0.0, CAST(0 AS bit), 1, 5000.0, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BookId', N'BorrowSlipId', N'ConditionStatus', N'FineAmount', N'IsReturned', N'Quantity', N'RentalPrice', N'ReturnDate') AND [object_id] = OBJECT_ID(N'[BorrowDetails]'))
    SET IDENTITY_INSERT [BorrowDetails] OFF;
GO

CREATE INDEX [IX_Books_AuthorId] ON [Books] ([AuthorId]);
GO

CREATE INDEX [IX_Books_CategoryId] ON [Books] ([CategoryId]);
GO

CREATE UNIQUE INDEX [IX_Books_ISBN] ON [Books] ([ISBN]);
GO

CREATE INDEX [IX_Books_PublisherId] ON [Books] ([PublisherId]);
GO

CREATE INDEX [IX_Books_ShelfId] ON [Books] ([ShelfId]);
GO

CREATE INDEX [IX_BorrowDetails_BookId] ON [BorrowDetails] ([BookId]);
GO

CREATE INDEX [IX_BorrowDetails_BorrowSlipId] ON [BorrowDetails] ([BorrowSlipId]);
GO

CREATE INDEX [IX_BorrowSlips_EmployeeId] ON [BorrowSlips] ([EmployeeId]);
GO

CREATE INDEX [IX_BorrowSlips_ReaderId] ON [BorrowSlips] ([ReaderId]);
GO

CREATE UNIQUE INDEX [IX_BorrowSlips_SlipCode] ON [BorrowSlips] ([SlipCode]);
GO

CREATE UNIQUE INDEX [IX_Employees_Email] ON [Employees] ([Email]);
GO

CREATE UNIQUE INDEX [IX_Employees_EmployeeCode] ON [Employees] ([EmployeeCode]);
GO

CREATE INDEX [IX_Fines_BorrowSlipId] ON [Fines] ([BorrowSlipId]);
GO

CREATE INDEX [IX_Fines_ReaderId] ON [Fines] ([ReaderId]);
GO

CREATE UNIQUE INDEX [IX_Readers_Email] ON [Readers] ([Email]);
GO

CREATE UNIQUE INDEX [IX_Readers_ReaderCode] ON [Readers] ([ReaderCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261118024616_InitialCreate', N'8.0.11');
GO

COMMIT;
GO

