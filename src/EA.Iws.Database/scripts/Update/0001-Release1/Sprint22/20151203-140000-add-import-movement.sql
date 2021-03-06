CREATE TABLE [Notification].[ImportMovement]
(
	[Id] UNIQUEIDENTIFIER NOT NULL, 
    [Number] INT NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL, 
	[RowVersion] ROWVERSION NOT NULL,
    CONSTRAINT [PK_ImportMovement] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ImportMovement_ImportNotification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[ImportNotification] (Id)
);
GO