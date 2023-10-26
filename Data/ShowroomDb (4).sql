CREATE DATABASE ShowroomAuto
GO
use ShowroomAuto
go 


-- Tạo bảng Nhân viên
CREATE TABLE Employees (
    EmployeeId NVARCHAR(10) PRIMARY KEY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    DateBirth DATETIME,
    Gender BIT,
    CCCD NVARCHAR(12),
    Position NVARCHAR(100),
    StartDate DATETIME,
    Salary INT,
    Email NVARCHAR(100),
    SaleId NVARCHAR(10),
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
    Gender BIT,
    CCCD NVARCHAR(12),
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
    Status NVARCHAR(100)
);
GO

-- Tạo bảng Hóa đơn nhập
CREATE TABLE PurchaseInvoices (
    InEnterId NVARCHAR(10) PRIMARY KEY,
    SourceId NVARCHAR(10),
    ProductId NVARCHAR(100),
    Date DATETIME,
    QuantityPurchase INT,
    Status NVARCHAR(100)
);
GO
-- Tạo bảng Hóa đơn bán
CREATE TABLE SalesInvoices (
    InSaleId NVARCHAR(10) PRIMARY KEY,
    ClientId NVARCHAR(10),
    ProductId NVARCHAR(100),
    Date DATETIME,
    QuantitySale INT,
    Status NVARCHAR(100),
);
GO
-- Tạo bảng Doanh số
CREATE TABLE SalesTargets (
    SaleId NVARCHAR(10) PRIMARY KEY,
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
    EmployeeId NVARCHAR(10),
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
	ID INT IDENTITY(1,1) PRIMARY KEY,
    ProductSerial NVARCHAR(100),
    Url_image NVARCHAR(500)
)

------------------
------------------
----- INSERT -----
------------------
------------------

---------------------------TINH LUONG-------------------------------------
GO
CREATE OR ALTER TRIGGER Tinh_Luong
ON [dbo].[SalesTargets]
AFTER INSERT, UPDATE
AS 
BEGIN
    -- Cập nhật Salary trong bảng Employees dựa trên giá trị Total từ bảng inserted
    UPDATE E
    SET Salary =
        CASE 
            WHEN I.Total > I.Target THEN 9 + I.Reward*(I.Total - I.[Target])
        ELSE 9
        END
    FROM Employees AS E
    INNER JOIN inserted AS I ON E.SaleId = I.SaleId
END;

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
    WHERE Product_Images.ProductSerial = (
        SELECT Products.Serial
        FROM Products
        WHERE ProductName LIKE N'%' +@name +N'%'
    )
)
go
-- SELECT * FROM Products
-- SELECT * from SearchProductUrlByName(N'Toyota Camry')
go


CREATE or ALTER FUNCTION SearchProductUrlBySerial(@id NVARCHAR(100))
RETURNS TABLE
as 
RETURN(
    SELECT Url_image
    from Product_Images
    WHERE ProductSerial = @id
)
go
-- SELECT * from SearchProductUrlBySerial(N'P001')
--------------------- SELECT EMPLOYEE_ID -------
GO
-- SELECT Employees.EmployeeId,Employees.FirstName +' '+ Employees.LastName as HovaTen
-- from Employees
GO
--#### EMPLOYEEE
INSERT INTO Employees (EmployeeId, FirstName, LastName, DateBirth, Gender, CCCD, Position, StartDate, Salary, Email, SaleId,Deleted,Url_image)
VALUES
    (N'E001', N'Nguyễn Văn', N'Tuấn', '1990-05-15', 1, N'123456789', N'Sales Manager', '2022-01-10', 5000, N'nguyenvantuan@gmail.com', N'S001',0,N''),
    (N'E002', N'Phạm Thị', N'Hà', '1985-07-20', 0, N'987654321', N'Sales', '2021-11-05', 4500, N'phamthiha@gmail.com', N'S002',0,N''),
    (N'E003', N'Lê Văn', N'Phúc', '1992-03-30', 1, N'111222333', N'Sales', '2022-02-15', 4800, N'levanphuc@gmail.com', N'S003',0,N''),
    (N'E004', N'Nguyễn Thị', N'Hằng', '1987-08-25', 0, N'444555666', N'Sales', '2021-10-20', 5500, N'nguyenthihang@gmail.com', N'S004',0,N''),
    (N'E005', N'Hoàng Đức', N'Tài', '1995-04-05', 1, N'777888999', N'Sales', '2022-03-01', 6000, N'hoangductai@gmail.com', N'S005',0,N''),
    (N'E006', N'Lương Minh', N'Châu', '1983-12-10', 1, N'999888777', N'Sales', '2021-12-15', 4800, N'luongminhchau@gmail.com', N'S006',0,N''),
    (N'E007', N'Phan Thị', N'Anh', '1994-06-20', 0, N'123123123', N'Sales', '2022-04-05', 5200, N'phanthianh@gmail.com', N'S007',0,N''),
    (N'E008', N'Trần Văn', N'Tuấn', '1989-01-05', 1, N'456456456', N'Sales', '2022-01-25', 4900, N'tranvantuan@gmail.com', N'S008',0,N''),
    (N'E009', N'Huỳnh Thị', N'Hòa', '1991-09-12', 0, N'789789789', N'Sales', '2022-02-20', 5300, N'huynhthihoa@gmail.com', N'S009',0,N''),
    (N'E010', N'Võ Thanh', N'Nam', '1993-11-15', 1, N'135792468', N'Sales', '2022-03-15', 4700, N'vothanhnam@gmail.com', N'S010',0,N'');

---#### CUSTOMERS 
INSERT INTO Customers (ClientId, FirstName, LastName, DateBirth, Gender, CCCD, Email, Address,Deleted)
VALUES
    (N'C001', N'Nguyễn Thị', N'Hương', '1990-06-18', 0, N'111222333', N'nguyenthihuong@gmail.com', N'Hà Nội',0),
    (N'C002', N'Lê Văn', N'Hào', '1986-03-25', 1, N'444555666', N'levanhao@gmail.com', N'Hồ Chí Minh',0),
    (N'C003', N'Phạm Đức', N'Trọng', '1991-09-10', 1, N'777888999', N'phamductrong@gmail.com', N'Đà Nẵng',0),
    (N'C004', N'Trần Thị', N'Quỳnh', '1987-08-01', 0, N'123456789', N'tranthiquynh@gmail.com', N'Hải Phòng',0),
    (N'C005', N'Hoàng Minh', N'Hoàng', '1995-05-12', 1, N'555444333', N'hoangminhhoang@gmail.com', N'Hà Nội',0),
    (N'C006', N'Vũ Thị', N'Yến', '1984-11-30', 0, N'999888777', N'vuthiyen@gmail.com', N'Hồ Chí Minh',0),
    (N'C007', N'Đỗ Văn', N'Thắng', '1993-02-07', 1, N'135792468', N'dovanthang@gmail.com', N'Hải Phòng',0),
    (N'C008', N'Nguyễn Thị', N'Trang', '1989-07-19', 0, N'987654321', N'nguyenthitrang@gmail.com', N'Hà Nội',0),
    (N'C009', N'Huỳnh Thanh', N'Hòa', '1991-11-25', 0, N'888777666', N'huynhthanhhoa@gmail.com', N'Đà Nẵng',0),
    (N'C010', N'Lê Thị', N'An', '1994-04-04', 0, N'999999999', N'lethian@gmail.com', N'Hà Nội',0);


--- #### PRODUCTS
INSERT INTO Products (ProductName, Serial, PurchasePrice, SalePrice, Quantity, Status)
VALUES
    (N'Toyota Camry', N'P001', 280, 350, 5, N'Available'),
    (N'Honda Civic', N'P002', 250, 320, 3, N'Available'),
    (N'Ford Mustang', N'P003', 350, 420, 2, N'Available'),
    (N'Chevrolet Silverado', N'P004', 320, 390, 4, N'Available'),
    (N'Audi Q5', N'P005', 420, 490, 3, N'Available'),
    (N'Mercedes-Benz E-Class', N'P006', 450, 520, 2, N'Available'),
    (N'BMW X5', N'P007', 480, 550, 3, N'Available'),
    (N'Tesla Model 3', N'P008', 500, 580, 2, N'Available'),
    (N'Nissan Altima', N'P009', 270, 340, 4, N'Available'),
    (N'Kia Optima', N'P010', 240, 310, 3, N'Available');

--- ### PURCHASE INVOICES

INSERT INTO PurchaseInvoices (InEnterId, SourceId, ProductId, Date, QuantityPurchase, Status)
VALUES
    (N'IE001', N'SU001', N'P001', '2022-01-15', 5, N'Đã nhập hàng'),
    (N'IE002', N'SU002', N'P002', '2022-02-20', 10, N'Đã nhập hàng'),
    (N'IE003', N'SU003', N'P003', '2022-03-25', 8, N'Đã nhập hàng'),
    (N'IE004', N'SU004', N'P004', '2022-04-30', 12, N'Đã nhập hàng'),
    (N'IE005', N'SU005', N'P005', '2022-05-05', 15, N'Đã nhập hàng'),
    (N'IE006', N'SU006', N'P006', '2022-06-10', 7, N'Đã nhập hàng'),
    (N'IE007', N'SU007', N'P007', '2022-07-15', 9, N'Đã nhập hàng'),
    (N'IE008', N'SU008', N'P008', '2022-08-20', 11, N'Đã nhập hàng'),
    (N'IE009', N'SU009', N'P009', '2022-09-25', 6, N'Đã nhập hàng'),
    (N'IE010', N'SU010', N'P010', '2022-10-30', 14, N'Đã nhập hàng');


--- ### SALE INVOICES 
INSERT INTO SalesInvoices (InSaleId, ClientId, ProductId, Date, QuantitySale, Status)
VALUES
    (N'IS001', N'C001', N'P001', '2023-01-15', 3, N'Đã bán hàng'),
    (N'IS002', N'C002', N'P002', '2023-02-20', 6, N'Đã bán hàng'),
    (N'IS003', N'C003', N'P003', '2023-03-25', 4, N'Đã bán hàng'),
    (N'IS004', N'C004', N'P004', '2023-04-30', 8, N'Đã bán hàng'),
    (N'IS005', N'C005', N'P005', '2023-05-05', 10, N'Đã bán hàng'),
    (N'IS006', N'C006', N'P006', '2023-06-10', 5, N'Đã bán hàng'),
    (N'IS007', N'C007', N'P007', '2023-07-15', 7, N'Đã bán hàng'),
    (N'IS008', N'C008', N'P008', '2023-08-20', 9, N'Đã bán hàng'),
    (N'IS009', N'C009', N'P009', '2023-09-25', 2, N'Đã bán hàng'),
    (N'IS010', N'C010', N'P010', '2023-10-30', 12, N'Đã bán hàng');

---### SALE TARGET 
INSERT INTO SalesTargets (SaleId, StartDate, EndDate, Total, Target, Status, Reward)
VALUES
    (N'S001', '2023-01-01', '2023-12-31', 220, 200, N'In Progress', 0.03),
    (N'S002', '2023-02-01', '2023-12-31', 230, 180, N'In Progress', 0.03),
    (N'S003', '2023-03-01', '2023-12-31', 180, 220, N'In Progress', 0.03),
    (N'S004', '2023-04-01', '2023-12-31', 200, 180, N'In Progress', 0.03),
    (N'S005', '2023-05-01', '2023-12-31', 190, 230, N'In Progress', 0.03),
    (N'S006', '2023-06-01', '2023-12-31', 140, 190, N'In Progress', 0.03),
    (N'S007', '2023-07-01', '2023-12-31', 130, 180, N'In Progress', 0.03),
    (N'S008', '2023-08-01', '2023-12-31', 170, 220, N'In Progress', 0.03),
    (N'S009', '2023-09-01', '2023-12-31', 155, 200, N'In Progress', 0.03),
    (N'S010', '2023-10-01', '2023-12-31', 145, 190, N'In Progress', 0.03);

---### DEVICES 
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



--- ### TEST DRIVE 
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


---### ACCOUNT 
INSERT INTO Account (EmployeeId, Username, Password_foruser,Level_account, Deleted, CreateAt, DeleteAt)
VALUES 
    (N'E001', 'john_doe2', CONVERT(VARBINARY(500), 'password1'),2, 0, GETDATE(), NULL),
    (N'E002', 'jane_smith', CONVERT(VARBINARY(500), 'password2'),1, 0, GETDATE(), NULL),
    (N'E003', 'bob_jones', CONVERT(VARBINARY(500), 'password3'),1, 0, GETDATE(), NULL),
    (N'E004', 'susan_wilson', CONVERT(VARBINARY(500), 'password4'),1, 0, GETDATE(), NULL),
    (N'E005', 'mike_adams', CONVERT(VARBINARY(500), 'password5'),1, 0, GETDATE(), NULL),
    (N'E006', 'lisa_miller', CONVERT(VARBINARY(500), 'password6'),1, 0, GETDATE(), NULL),
    (N'E007', 'david_brown', CONVERT(VARBINARY(500), 'password7'),1, 0, GETDATE(), NULL),
    (N'E008', 'emily_white', CONVERT(VARBINARY(500), 'password8'),1, 0, GETDATE(), NULL),
    (N'E009', 'steve_green', CONVERT(VARBINARY(500), 'password9'),1, 0, GETDATE(), NULL),
    (N'E010', 'jennifer_black', CONVERT(VARBINARY(500), 'password10'),1, 0, GETDATE(), NULL);

---### source
-- Chèn 10 bản ghi vào bảng Source với SourceId thay đổi thành định dạng N'SU' và Name là N'Tên Nguồn'
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


---##### PRODUCT___IMAGE
INSERT INTO Product_Images (ProductSerial, Url_image)
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
------- set salary bang 0 de phuc vu cho viec tinh luong -------
UPDATE Employees 
set Salary = 0

-------------------
-------------------
-----END INSERT----
-------------------
-------------------



-------------------
-------------------
--REFERENCES KEY---
-------------------
-------------------
ALTER TABLE Employees
ADD CONSTRAINT FK_Employee_SaleTarget FOREIGN KEY(SaleId)
REFERENCES SalesTargets(SaleId)

ALTER TABLE PurchaseInvoices
ADD CONSTRAINT FK_PurchaseInvoices_Products FOREIGN KEY (ProductId)
REFERENCES Products(Serial);

ALTER TABLE SalesInvoices
ADD CONSTRAINT FK_SalesInvoices_Customers FOREIGN KEY (ClientId)
REFERENCES Customers (ClientId);

ALTER TABLE SalesInvoices
ADD CONSTRAINT FK_SalesInvoices_Products FOREIGN KEY (ProductId)
REFERENCES Products (Serial);

ALTER TABLE TestDrive
ADD CONSTRAINT FK_TestDrive_Employees FOREIGN KEY (EmployeeId)
REFERENCES Employees (EmployeeId);

ALTER TABLE TestDrive
ADD CONSTRAINT FK_TestDrive_Customers FOREIGN KEY (ClientId)
REFERENCES Customers (ClientId);

ALTER TABLE Account
ADD CONSTRAINT FK_Account_Employees FOREIGN KEY (EmployeeId)
REFERENCES Employees (EmployeeId);

ALTER TABLE PurchaseInvoices
ADD CONSTRAINT FK_PurchaseInvoices_Source FOREIGN KEY (SourceId)
REFERENCES Source (SourceId);


-------------------
-------------------
------END ---------
-------------------
-------------------

