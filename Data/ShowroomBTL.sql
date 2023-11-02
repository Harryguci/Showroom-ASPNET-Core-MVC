CREATE DATABASE ShowroomAuto
GO
USE ShowroomAuto
GO 


-- Tạo bảng Nhân viên
CREATE TABLE Employees (
    EmployeeId NVARCHAR(10) PRIMARY KEY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    DateBirth DATETIME,
    PhoneNumber NVARCHAR(20),
    Gender BIT,
    CCCD NVARCHAR(12),
    Position NVARCHAR(100),
    StartDate DATETIME,
    Salary INT,
    Email NVARCHAR(100),
    Deleted BIT,
    Url_image NVARCHAR(500)
);
GO
-- Tạo bảng Khách hàng
CREATE TABLE Customers (
    ClientId NVARCHAR(10) PRIMARY KEY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    DateBirth DATETIME,
    PhoneNumber NVARCHAR(20),
    Gender BIT,
    CCCD NVARCHAR(20),
    Email NVARCHAR(100),
    Address NVARCHAR(200),
    Deleted BIT
);
go
-- Tạo bảng Sản phẩm
CREATE TABLE Products (
    ProductName NVARCHAR(200),
    Serial NVARCHAR(100) PRIMARY KEY,
    PurchasePrice INT,
    SalePrice INT,
    Quantity INT,
    Status NVARCHAR(100),
    Deleted BIT
);
GO

-- Tạo bảng Hóa đơn nhập
CREATE TABLE PurchaseInvoices (
    InEnterId NVARCHAR(10) PRIMARY KEY,
    SourceId NVARCHAR(10),
    ProductId NVARCHAR(100),
    Date DATETIME,
    QuantityPurchase INT,
    Status NVARCHAR(100),
    Deleted BIT
);
GO
-- Tạo bảng Hóa đơn bán
CREATE TABLE SalesInvoices (
    InSaleId NVARCHAR(10) PRIMARY KEY,
    ClientId NVARCHAR(10),
    EmployeeId NVARCHAR(10),
    ProductId NVARCHAR(100),
    Date DATETIME,
    QuantitySale INT,
    Status NVARCHAR(100),
    Deleted BIT
);

GO
-- Tạo bảng Doanh số
CREATE TABLE SalesTargets (
    SaleId NVARCHAR(10) PRIMARY KEY,
    EmployeeId NVARCHAR(10),
    StartDate DATETIME,
    EndDate DATETIME,
    Total INT,
    Target INT,
    Status NVARCHAR(100),
    Reward FLOAT
);
go
-- DROP TABLE SalesTargets
-- Tạo bảng Thiết bị
CREATE TABLE Devices (
    DeviceId NVARCHAR(10) PRIMARY KEY,
    Name NVARCHAR(200),
    DateEntry DATETIME,
    Price INT,
    Status NVARCHAR(100)
);
go
-- DROP TABLE Devices
-- Tạo bảng Lịch đăng ký lái thử
CREATE TABLE TestDrive (
    DriveId NVARCHAR(10) PRIMARY KEY,
    EmployeeId NVARCHAR(10),
    ClientId NVARCHAR(10),
    BookDate DATETIME,
    Note NVARCHAR(100),
    Status NVARCHAR(100)
);
GO
---- ACCOUNT -----
CREATE TABLE Account (
    Username VARCHAR(30) PRIMARY KEY,
    Password_foruser VARBINARY(500),
    Level_account INT,
    Deleted BIT,
    CreateAt DATETIME,
    DeleteAt DATETIME
)

go
----- SOURCE -----
CREATE TABLE Source(
    SourceId NVARCHAR(10) PRIMARY KEY,
    Name NVARCHAR(100)
)
go
---- PRODUCT  __ IMAGE -----
CREATE TABLE Product_Images(
    id int IDENTITY(1,1) PRIMARY KEY,
    Serial NVARCHAR(100),
    Url_image NVARCHAR(500)
)

---------------------------Tinh Tong Tien cua thang -----------------
go 
CREATE OR ALTER TRIGGER KPI 
ON [dbo].[SalesInvoices]
FOR INSERT,UPDATE 
AS 
BEGIN
    UPDATE s
    set Total =Total + Cast(i.QuantitySale* p.SalePrice as INT) 
    FROM inserted as i join SalesTargets as s on i.EmployeeId = s.EmployeeId
    join Products as p on i.ProductId = p.Serial
    WHERE i.EmployeeId = s.EmployeeId
END 
---------------------------TINH LUONG-------------------------------------
GO
CREATE or ALTER FUNCTION Tinh_Luong(@idemployee NVARCHAR(20)) 
RETURNS INT
AS 
BEGIN
    DECLARE @salary int 
    SELECT @Salary = 
        CASE 
            WHEN s.Total > s.Target THEN 9 + s.Reward * (s.Total - s.Target)
            ELSE 9
        END
    FROM Employees e
    INNER JOIN SalesTargets s ON e.EmployeeId = s.EmployeeId
    WHERE e.EmployeeId = @idemployee

    RETURN @salary 
end


-------------------- HASH PASSWORD -----------------
go
CREATE OR ALTER TRIGGER Hash_password on [dbo].[Account]
for INSERT,UPDATE
as 
BEGIN
    
    UPDATE Account
    SET Password_foruser = HASHBYTES('SHA2_512', i.Password_foruser)
    from Account as a 
    JOIN inserted as i on a.Username = i.Username
END


-- Chèn dữ liệu mẫu vào bảng Account
-------------------- FUNCTION LOGIN -----------------

GO
CREATE FUNCTION Login_check (@username VARCHAR(30),@password VARCHAR(30))
RETURNS TABLE
AS 
RETURN(
    SELECT *
    FROM Account
    WHERE @username = Account.Username AND Account.Password_foruser = HASHBYTES('SHA2_512',@password)
)
GO

-- TEST 
-- SELECT * FROM Login_check('john_doe2', 'password1');

------------------ Function Product search--------------
CREATE or alter  FUNCTION SearchProductUrlByName (@name NVARCHAR(100))
RETURNS TABLE 
AS 
RETURN(
    SELECT Product_Images.Url_image
    from Product_Images
    WHERE Product_Images.Serial = (
        SELECT Products.Serial
        FROM Products
        WHERE ProductName LIKE N'%' +@name +N'%'
    )
)
go

go


CREATE or ALTER FUNCTION SearchProductUrlBySerial(@id NVARCHAR(100))
RETURNS TABLE
as 
RETURN(
    SELECT Url_image
    from Product_Images
    WHERE Serial = @id
)
go

GO

ALTER TABLE SalesTargets
ADD CONSTRAINT FK_Employee_SaleTarget FOREIGN KEY(EmployeeId)
REFERENCES Employees(EmployeeId)
ON DELETE CASCADE;

ALTER TABLE PurchaseInvoices
ADD CONSTRAINT FK_PurchaseInvoices_Products FOREIGN KEY (ProductId)
REFERENCES Products(Serial)
ON DELETE CASCADE;

ALTER TABLE SalesInvoices
ADD CONSTRAINT FK_SalesInvoices_Customers FOREIGN KEY (ClientId)
REFERENCES Customers (ClientId)
ON DELETE CASCADE;

ALTER TABLE SalesInvoices
ADD CONSTRAINT FK_SalesInvoices_Products FOREIGN KEY (ProductId)
REFERENCES Products (Serial)
ON DELETE CASCADE;

ALTER TABLE SalesInvoices
ADD CONSTRAINT FK_SalesInvoices_Employees FOREIGN KEY (EmployeeId)
REFERENCES Employees (EmployeeId)
ON DELETE CASCADE;

ALTER TABLE TestDrive
ADD CONSTRAINT FK_TestDrive_Employees FOREIGN KEY (EmployeeId)
REFERENCES Employees (EmployeeId)
ON DELETE CASCADE;

ALTER TABLE TestDrive
ADD CONSTRAINT FK_TestDrive_Customers FOREIGN KEY (ClientId)
REFERENCES Customers (ClientId)
ON DELETE CASCADE;

ALTER TABLE PurchaseInvoices
ADD CONSTRAINT FK_PurchaseInvoices_Source FOREIGN KEY (SourceId)
REFERENCES Source (SourceId)
ON DELETE CASCADE;
GO

-----------------------------------------------------------------
INSERT INTO Source (SourceId, Name)
VALUES
    (N'SU001', N'Trình duyệt web'),
    (N'SU002', N'Mạng xã hội'),
    (N'SU003', N'Đến cửa hàng trực tiếp'),
    (N'SU004', N'Thông qua giới thiệu'),
    (N'SU005', N'Email Marketing'),
    (N'SU006', N'Trưng bày sản phẩm'),
    (N'SU007', N'Quảng cáo trên radio'),
    (N'SU008', N'Quảng cáo trên truyền hình'),
    (N'SU009', N'Tin thư trực tiếp'),
    (N'SU010', N'Khác')


INSERT INTO Employees (EmployeeId, LastName, FirstName, DateBirth,PhoneNumber, Gender, CCCD, Position, StartDate, Salary, Email,Deleted,Url_image)
VALUES
    (N'E001', N'Nguyễn Văn', N'Tuấn', '1990-05-15','0987654321', 1, N'123456789', N'Sales Manager', '2022-01-10', 0, N'nguyenvantuan@gmail.com',0,N''),
    (N'E002', N'Phạm Thị', N'Hà', '1985-07-20','0123456789', 0, N'987654321', N'Sales', '2021-11-05', 0, N'phamthiha@gmail.com',0,N''),
    (N'E003', N'Lê Văn', N'Phúc', '1992-03-30','0456789123', 1, N'111222333', N'Sales', '2022-02-15', 0, N'levanphuc@gmail.com',0,N''),
    (N'E004', N'Nguyễn Thị', N'Hằng', '1987-08-25','0789012345', 0, N'444555666', N'Sales', '2021-10-20', 0, N'nguyenthihang@gmail.com',0,N''),
    (N'E005', N'Hoàng Đức', N'Tài', '1995-04-05','0234567890', 1, N'777888999', N'Sales', '2022-03-01', 0, N'hoangductai@gmail.com',0,N''),
    (N'E006', N'Lương Minh', N'Châu', '1983-12-10','0678901234', 1, N'999888777', N'Sales', '2021-12-15', 0, N'luongminhchau@gmail.com',0,N''),
    (N'E007', N'Phan Thị', N'Anh', '1994-06-20','0567890123', 0, N'123123123', N'Sales', '2022-04-05', 0, N'phanthianh@gmail.com',0,N''),
    (N'E008', N'Trần Văn', N'Tuấn', '1989-01-05','0345678901', 1, N'456456456', N'Sales', '2022-01-25', 0, N'tranvantuan@gmail.com',0,N''),
    (N'E009', N'Huỳnh Thị', N'Hòa', '1991-09-12','0890123456', 0, N'789789789', N'Sales', '2022-02-20', 0, N'huynhthihoa@gmail.com',0,N''),
    (N'E010', N'Võ Thanh', N'Nam', '1993-11-15','0012345678', 1, N'135792468', N'Sales', '2022-03-15', 0, N'vothanhnam@gmail.com',0,N'');

INSERT INTO SalesTargets (SaleId,EmployeeId, StartDate, EndDate, Total, Target, Status, Reward)
VALUES
    (N'S001',N'E001', '2023-01-01', '2023-12-31', 0, 200, N'In Progress', 0.03),
    (N'S002',N'E002', '2023-02-01', '2023-12-31', 0, 180, N'In Progress', 0.03),
    (N'S003',N'E003', '2023-03-01', '2023-12-31', 0, 220, N'In Progress', 0.03),
    (N'S004',N'E004', '2023-04-01', '2023-12-31', 0, 180, N'In Progress', 0.03),
    (N'S005',N'E005', '2023-05-01', '2023-12-31', 0, 230, N'In Progress', 0.03),
    (N'S006',N'E006', '2023-06-01', '2023-12-31', 0, 190, N'In Progress', 0.03),
    (N'S007',N'E007', '2023-07-01', '2023-12-31', 0, 180, N'In Progress', 0.03),
    (N'S008',N'E008', '2023-08-01', '2023-12-31', 0, 220, N'In Progress', 0.03),
    (N'S009',N'E009', '2023-09-01', '2023-12-31', 0, 200, N'In Progress', 0.03),
    (N'S010',N'E010', '2023-10-01', '2023-12-31', 0, 190, N'In Progress', 0.03);


INSERT INTO Customers (ClientId, LastName, FirstName, DateBirth,PhoneNumber, Gender, CCCD, Email, Address,Deleted)
VALUES
    (N'C001', N'Nguyễn Thị', N'Hương', '1990-06-18','0321987654', 0, N'111222333', N'nguyenthihuong@gmail.com', N'Hà Nội',0),
    (N'C002', N'Lê Văn', N'Hào', '1986-03-25','0897654321', 1, N'444555666', N'levanhao@gmail.com', N'Hồ Chí Minh',0),
    (N'C003', N'Phạm Đức', N'Trọng', '1991-09-10','0765432198', 1, N'777888999', N'phamductrong@gmail.com', N'Đà Nẵng',0),
    (N'C004', N'Trần Thị', N'Quỳnh', '1987-08-01','0011111111', 0, N'123456789', N'tranthiquynh@gmail.com', N'Hải Phòng',0),
    (N'C005', N'Hoàng Minh', N'Hoàng', '1995-05-12','0123456000', 1, N'555444333', N'hoangminhhoang@gmail.com', N'Hà Nội',0),
    (N'C006', N'Vũ Thị', N'Yến', '1984-11-30','0789000000', 0, N'999888777', N'vuthiyen@gmail.com', N'Hồ Chí Minh',0),
    (N'C007', N'Đỗ Văn', N'Thắng', '1993-02-07','0999901234', 1, N'135792468', N'dovanthang@gmail.com', N'Hải Phòng',0),
    (N'C008', N'Nguyễn Thị', N'Trang', '1989-07-19','0432109898', 0, N'987654321', N'nguyenthitrang@gmail.com', N'Hà Nội',0),
    (N'C009', N'Huỳnh Thanh', N'Hòa', '1991-11-25','0555555555', 0, N'888777666', N'huynhthanhhoa@gmail.com', N'Đà Nẵng',0),
    (N'C010', N'Lê Thị', N'An', '1994-04-04','0456789999', 0, N'999999999', N'lethian@gmail.com', N'Hà Nội',0);


INSERT INTO Products (ProductName, Serial, PurchasePrice, SalePrice, Quantity, Status,Deleted)
VALUES
    (N'Toyota Camry', N'P001', 280, 350, 5, N'Available',0),
    (N'Honda Civic', N'P002', 250, 320, 3, N'Available',0),
    (N'Ford Mustang', N'P003', 350, 420, 2, N'Available',0),
    (N'Chevrolet Silverado', N'P004', 320, 390, 4, N'Available',0),
    (N'Audi Q5', N'P005', 420, 490, 3, N'Available',0),
    (N'Mercedes-Benz E-Class', N'P006', 450, 520, 2, N'Available',0),
    (N'BMW X5', N'P007', 480, 550, 3, N'Available',0),
    (N'Tesla Model 3', N'P008', 500, 580, 2, N'Available',0),
    (N'Nissan Altima', N'P009', 270, 340, 4, N'Available',0),
    (N'Kia Optima', N'P010', 240, 310, 3, N'Available',0);

INSERT INTO PurchaseInvoices (InEnterId, SourceId, ProductId, Date, QuantityPurchase, Status,Deleted)
VALUES
    (N'IE001', N'SU001', N'P001', '2022-01-15', 5, N'Đã nhập hàng',0),
    (N'IE002', N'SU002', N'P002', '2022-02-20', 10, N'Đã nhập hàng',0),
    (N'IE003', N'SU003', N'P003', '2022-03-25', 8, N'Đã nhập hàng',0),
    (N'IE004', N'SU004', N'P004', '2022-04-30', 12, N'Đã nhập hàng',0),
    (N'IE005', N'SU005', N'P005', '2022-05-05', 15, N'Đã nhập hàng',0),
    (N'IE006', N'SU006', N'P006', '2022-06-10', 7, N'Đã nhập hàng',0),
    (N'IE007', N'SU007', N'P007', '2022-07-15', 9, N'Đã nhập hàng',0),
    (N'IE008', N'SU008', N'P008', '2022-08-20', 11, N'Đã nhập hàng',0),
    (N'IE009', N'SU009', N'P009', '2022-09-25', 6, N'Đã nhập hàng',0),
    (N'IE010', N'SU010', N'P010', '2022-10-30', 14, N'Đã nhập hàng',0);


INSERT INTO SalesInvoices (InSaleId, ClientId,EmployeeId, ProductId, Date, QuantitySale, Status,Deleted)
VALUES
    (N'IS001', N'C001',N'E001', N'P001', '2023-01-15', 1, N'Đã bán hàng',0),
    (N'IS002', N'C002',N'E002', N'P002', '2023-02-20', 1, N'Đã bán hàng',0),
    (N'IS003', N'C003',N'E003', N'P003', '2023-03-25', 1, N'Đã bán hàng',0),
    (N'IS004', N'C004',N'E004', N'P004', '2023-04-30', 1, N'Đã bán hàng',0),
    (N'IS005', N'C005',N'E005', N'P005', '2023-05-05', 1, N'Đã bán hàng',0),
    (N'IS006', N'C006',N'E006', N'P006', '2023-06-10', 1, N'Đã bán hàng',0),
    (N'IS007', N'C007',N'E007', N'P007', '2023-07-15', 1, N'Đã bán hàng',0),
    (N'IS008', N'C008',N'E008', N'P008', '2023-08-20', 1, N'Đã bán hàng',0),
    (N'IS009', N'C009',N'E009', N'P009', '2023-09-25', 1, N'Đã bán hàng',0),
    (N'IS010', N'C010',N'E010', N'P010', '2023-10-30', 1, N'Đã bán hàng',0);


INSERT INTO TestDrive (DriveId, EmployeeId, ClientId, BookDate, Note, Status)
VALUES
    (N'TD001', N'E001', N'C001', '2023-01-15', N'Lên lịch lái thử cho khách hàng', N'Đã lên lịch'),
    (N'TD002', N'E002', N'C002', '2023-02-20', N'Lên lịch lái thử cho khách hàng', N'Đã lên lịch'),
    (N'TD003', N'E003', N'C003', '2023-03-25', N'Lên lịch lái thử cho khách hàng', N'Đã lên lịch'),
    (N'TD004', N'E004', N'C004', '2023-04-30', N'Lên lịch lái thử cho khách hàng', N'Đã lên lịch'),
    (N'TD005', N'E005', N'C005', '2023-05-05', N'Lên lịch lái thử cho khách hàng', N'Đã lên lịch'),
    (N'TD006', N'E006', N'C006', '2023-06-10', N'Lên lịch lái thử cho khách hàng', N'Đã lên lịch'),
    (N'TD007', N'E007', N'C007', '2023-07-15', N'Lên lịch lái thử cho khách hàng', N'Đã lên lịch'),
    (N'TD008', N'E008', N'C008', '2023-08-20', N'Lên lịch lái thử cho khách hàng', N'Đã lên lịch'),
    (N'TD009', N'E009', N'C009', '2023-09-25', N'Lên lịch lái thử cho khách hàng', N'Đã lên lịch'),
    (N'TD010', N'E010', N'C010', '2023-10-30', N'Lên lịch lái thử cho khách hàng', N'Đã lên lịch');

go 
INSERT INTO Devices (DeviceId, Name, DateEntry, Price, Status)
VALUES
    (N'D001', N'Hệ thống định vị', '2023-01-05', 500, N'Có sẵn'),
    (N'D002', N'Cảm biến đỗ xe', '2023-02-10', 300, N'Có sẵn'),
    (N'D003', N'Camera lùi', '2023-03-15', 400, N'Có sẵn'),
    (N'D004', N'Cửa sổ trời', '2023-04-20', 600, N'Có sẵn'),
    (N'D005', N'Kết nối Bluetooth', '2023-05-25', 200, N'Có sẵn'),
    (N'D006', N'Cửa số không khóa', '2023-06-30', 250, N'Có sẵn'),
    (N'D007', N'Ghế nóng', '2023-07-05', 350, N'Có sẵn'),
    (N'D008', N'Camera lùi phía sau', '2023-08-10', 400, N'Có sẵn'),
    (N'D009', N'Hệ thống cảnh báo điểm mù', '2023-09-15', 450, N'Có sẵn'),
    (N'D010', N'Tích hợp điện thoại thông minh', '2023-10-20', 300, N'Có sẵn');


INSERT INTO Account ( Username, Password_foruser,Level_account, Deleted, CreateAt, DeleteAt)
VALUES 
    ('john_doe2', CONVERT(VARBINARY(500), 'password1'),2, 0, GETDATE(), NULL),
    ('jane_smith', CONVERT(VARBINARY(500), 'password2'),1, 0, GETDATE(), NULL),
    ('bob_jones', CONVERT(VARBINARY(500), 'password3'),1, 0, GETDATE(), NULL),
    ('susan_wilson', CONVERT(VARBINARY(500), 'password4'),1, 0, GETDATE(), NULL),
    ('mike_adams', CONVERT(VARBINARY(500), 'password5'),1, 0, GETDATE(), NULL),
    ('lisa_miller', CONVERT(VARBINARY(500), 'password6'),1, 0, GETDATE(), NULL),
    ('david_brown', CONVERT(VARBINARY(500), 'password7'),1, 0, GETDATE(), NULL),
    ('emily_white', CONVERT(VARBINARY(500), 'password8'),1, 0, GETDATE(), NULL),
    ('steve_green', CONVERT(VARBINARY(500), 'password9'),1, 0, GETDATE(), NULL),
    ('jennifer_black', CONVERT(VARBINARY(500), 'password10'),1, 0, GETDATE(), NULL);

---##### PRODUCT___IMAGE
INSERT INTO Product_Images (Serial, Url_image)
VALUES
    (N'P001', N'URL_1'),
    (N'P002', N'URL_2'),
    (N'P003', N'URL_3'),
    (N'P004', N'URL_4'),
    (N'P005', N'URL_5'),
    (N'P006', N'URL_6'),
    (N'P007', N'URL_7'),
    (N'P008', N'URL_8'),
    (N'P009', N'URL_9'),
    (N'P010', N'URL_10');

-------------------
-------------------
-----END INSERT----
-------------------
-------------------

----------------------------------

GO
------############################----
------############################----
----------------------------------
-- CREATE or ALTER TRIGGER Product_deleteImage
-- on Products
-- INSTEAD of DELETE
-- as 
-- BEGIN
--     DELETE FROM Product_Images
--     WHERE Serial in (Select deleted.Serial from deleted)

--     DELETE from 
-- END

-- go
-- CREATE OR ALTER TRIGGER Custormer_DeleteTrigger
-- on Customers
-- INSTEAD OF DELETE 
-- AS 
-- BEGIN
--     DELETE  TestDrive 
--     WHERE ClientId in (Select deleted.ClientId from deleted)

--     DELETE SalesInvoices
--     WHERE ClientId in (Select deleted.ClientId from deleted)
    
--     DELETE Customers
--     WHERE ClientId in (Select deleted.ClientId from deleted)
-- END 
-- ----------------------------------
-- GO
-- CREATE OR ALTER TRIGGER Employee_DeleteTrigger
-- on Employees
-- INSTEAD of DELETE
-- as 
-- BEGIN
--     DELETE from TestDrive 
--     WHERE EmployeeId in (select deleted.EmployeeId from deleted)

--     delete SalesTargets
--     WHERE EmployeeId in (Select deleted.EmployeeId from deleted)

--     DELETE Employees
--     WHERE EmployeeId in (select deleted.EmployeeId from deleted)
-- end 
-- GO
-- -----------------------------------
-- CREATE or ALTER TRIGGER Product_DeleteTrigger
-- ON Products
-- INSTEAD OF DELETE
-- AS
-- BEGIN
--     DELETE FROM SalesInvoices
--     WHERE ProductId IN (SELECT DELETED.Serial FROM DELETED);

--     DELETE FROM PurchaseInvoices
--     WHERE ProductId IN (SELECT DELETED.Serial FROM DELETED);

--     -- Xoá giá trị trong bảng Products
--     DELETE FROM Products
--     WHERE Serial IN (SELECT DELETED.Serial FROM DELETED);
-- END;
---------#################################
---------##################################
----------- update quantity in product after delete sale and purchase invoice ----
GO
CREATE OR ALTER TRIGGER Update_quantityafter_purchase
on PurchaseInvoices
for DELETE
as 
BEGIN
    UPDATE P
    set p.Quantity = p.Quantity - d.QuantityPurchase
    from deleted AS d join Products as p on d.ProductId = p.Serial
end 


 -------- cho sale invoice ----
go 
CREATE OR ALTER TRIGGER Update_quantityafter_sale 
on SalesInvoices
for DELETE
as
BEGIN
    update p
    set p.Quantity = p.Quantity + d.QuantitySale
    FROM deleted as d join Products as p on d.ProductId = p.Serial
END

-------- cap nhat sau khi insert , update ----
go
CREATE OR ALTER TRIGGER Update_quantityafter_insertupdate 
on SalesInvoices
for INSERT,update
as 
BEGIN
    UPDATE p
    set p.Quantity = p.Quantity + d.QuantitySale
    from deleted as d join Products as p on d.ProductId = p.Serial

    UPDATE p
    set p.Quantity = p.Quantity -i.QuantitySale
    from inserted as i join Products as p on i.ProductId = p.Serial
END
go
-- Tạo trigger sau khi INSERT hoặc UPDATE hóa đơn mua (PurchaseInvoices)
CREATE OR ALTER TRIGGER UpdatePurchaseQuantity
ON PurchaseInvoices
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE p
    SET p.Quantity = p.Quantity + i.QuantityPurchase
    FROM inserted AS i
    INNER JOIN Products AS p ON i.ProductId = p.Serial;

    -- Tăng số lượng cho các bản ghi trong bảng deleted
    UPDATE p
    SET p.Quantity = p.Quantity - d.QuantityPurchase
    FROM deleted AS d
    INNER JOIN Products AS p ON d.ProductId = p.Serial;
END;
--- Tao 

GO
-------##########################################-------
------- Ham tinh so luong xe tung thang trong nam ----------
CREATE or ALTER FUNCTION Get_ToTal_Quantity (@year int)
RETURNS TABLE
as 
RETURN(
    SELECT MONTH(SalesInvoices.[Date]) AS Thang , Sum(SalesInvoices.QuantitySale) as TongSoLuong
    from SalesInvoices
    WHERE YEAR([Date]) = @year
    GROUP by [Date]
)
go

use showroomauto;
go
SELECT * FROM Get_ToTal_Quantity(2023)