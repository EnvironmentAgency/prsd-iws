ALTER TABLE [Notification].[NotificationDates]
ADD ConsentWithdrawnDate DATE NULL;
GO

ALTER TABLE [Notification].[NotificationDates]
ADD ConsentWithdrawnReasons NVARCHAR(4000) NULL;
GO