ALTER TABLE [Notification].[ImportMovement]
ADD [ActualShipmentDate] DATETIMEOFFSET NOT NULL;
GO

ALTER TABLE [Notification].[ImportMovement]
ADD [PrenotificationDate] DATETIMEOFFSET NULL;
GO