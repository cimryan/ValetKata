USE [master]
GO

CREATE DATABASE [Valet]
GO

USE [Valet]
GO

CREATE TABLE [dbo].[ParkedCarInformation](
	[TicketNumber] [int] NOT NULL,
	[Make] [nvarchar](50) NOT NULL,
	[Model] [nvarchar](50) NOT NULL,
	[Color] [nvarchar](50) NOT NULL,
	[LotRow] [nvarchar](50) NULL,
	[LotColumn] [nvarchar](50) NULL
) ON [PRIMARY]

GO

CREATE PROCEDURE [dbo].[ClearTheLot] 
AS
BEGIN
	DELETE FROM dbo.ParkedCarInformation

	WAITFOR DELAY '00:00:02'
END

GO

CREATE PROCEDURE [dbo].[RecordCarAccepted]
	@make nvarchar(50),
	@model nvarchar(50),
	@color nvarchar(50)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	BEGIN TRANSACTION

	DECLARE @ticketNumber int

	SELECT @ticketNumber = (SELECT min(TicketNumber) + 1 FROM dbo.ParkedCarInformation WHERE TicketNumber + 1 NOT IN (SELECT TicketNumber FROM dbo.ParkedCarInformation))

	IF @ticketNumber IS NULL SELECT @ticketNumber = 1

	INSERT INTO dbo.ParkedCarInformation (TicketNumber, Make, Model, Color) VALUES (@ticketNumber, @make, @model, @color)

	COMMIT

	WAITFOR DELAY '00:00:02'

	SELECT @ticketNumber
END

GO

CREATE PROCEDURE [dbo].[RecordCarParked]
	@ticketNumber int,
	@lotRow nvarchar(50),
	@lotColumn nvarchar(50)
AS
BEGIN
	IF @ticketNumber IS NULL
	BEGIN;
		THROW 51001, 'Specify the ticket number', 1
	END

	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	BEGIN TRANSACTION
	
	IF EXISTS(SELECT * FROM ParkedCarInformation WHERE LotRow = @lotColumn AND LotColumn = @lotColumn)
	BEGIN;
		THROW 51001, 'There is already a car parked there.', 1
	END	

	UPDATE
		dbo.ParkedCarInformation
	SET
		LotRow = @lotRow,
		LotColumn = @lotColumn
	WHERE
		TicketNumber = @ticketNumber

	COMMIT

	WAITFOR DELAY '00:00:02'
END

GO

CREATE PROCEDURE [dbo].[RetrieveParkedCarInformation]
	@ticketNumber int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Make, Model, Color, LotRow, LotColumn from dbo.ParkedCarInformation WHERE TicketNumber = @ticketNumber

	WAITFOR DELAY '00:00:02'
END

GO

USE [master]
GO
ALTER DATABASE [Valet] SET  READ_WRITE 
GO
