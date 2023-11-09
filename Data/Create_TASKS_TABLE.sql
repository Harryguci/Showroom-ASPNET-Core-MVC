﻿USE SHOWROOMAUTO;
GO

CREATE TABLE TASKS (
	ID NVARCHAR(10) PRIMARY KEY,
	EMPLOYEEID NVARCHAR(10),
	DATELINE DATETIME,
	CONTENT NVARCHAR(500),
	RESULT NVarchar(500)
);

GO

ALTER TABLE TASKS
ADD CONSTRAINT FK_Tasks_Employees FOREIGN KEY (EmployeeId)
REFERENCES Employees (EmployeeId);

Go

INSERT INTO TASKS ("ID", "EMPLOYEEID", "DATELINE", "CONTENT", "RESULT")
VALUES (
	N'T000000001',
	N'E001',
	'2023-11-15',
	N'Công việc 1',
	NULL
),(N'T000000002', N'E002', '2023-11-16', N'Công việc 2', NULL),
  (N'T000000003', N'E003', '2023-11-17', N'Công việc 3', NULL),
  (N'T000000004', N'E004', '2023-11-18', N'Công việc 4', NULL),
  (N'T000000005', N'E005', '2023-11-19', N'Công việc 5', NULL),
  (N'T000000006', N'E006', '2023-11-20', N'Công việc 6', NULL),
  (N'T000000007', N'E007', '2023-11-21', N'Công việc 7', NULL),
  (N'T000000008', N'E008', '2023-11-22', N'Công việc 8', NULL),
  (N'T000000009', N'E009', '2023-11-23', N'Công việc 9', NULL),
  (N'T000000010', N'E010', '2023-11-23', N'Công việc 9', NULL);