IF OBJECT_ID('[Reports].[ExportNotificationOrganisations]') IS NULL
    EXEC('CREATE VIEW [Reports].[ExportNotificationOrganisations] AS SELECT 1 AS [NOTHING];')
GO

/*
*	Returns the organisation details for each of the businesses involved in an export notification.
*	Facility: Takes actual site if available, any facility if not, or null where none available
*	Producer: Takes site of export if available, any producer if not, or null where none available
*/
ALTER VIEW [Reports].[ExportNotificationOrganisations]
AS
	SELECT	N.[Id],
			N.[NotificationNumber],
			E.[Name] AS [Exporter],
			E.[FirstName] + ' ' + E.[LastName] AS [ExporterName],
			E.[RegistrationNumber] AS [ExporterRegistrationNumber],
			I.[Name] AS [Importer],
			I.[FirstName] + ' ' + I.[LastName] AS [ImporterName],
			I.[RegistrationNumber] AS [ImporterRegistrationNumber],
			P.[Name] AS [Producer],
			P.[FirstName] + ' ' + P.[LastName] AS [ProducerName],
			P.[RegistrationNumber] AS [ProducerRegistrationNumber],
			F.[Name] AS [Facility],
			F.[FirstName] + ' ' + F.[LastName] AS [FacilityName],
			F.[RegistrationNumber] AS [FacilityRegistrationNumber]

	FROM [Notification].[Notification] AS N
	LEFT JOIN [Notification].[Exporter] AS E
	ON E.[NotificationId] = N.[Id]
	LEFT JOIN [Notification].[Importer] AS I
	ON I.[NotificationId] = N.[Id]
	LEFT JOIN [Notification].[Facility] AS F
	ON F.Id = 
	(
		SELECT TOP 1 Id
		FROM [Notification].[Facility]
		WHERE NotificationId = N.Id
		ORDER BY [IsActualSiteOfTreatment] DESC
	)
	LEFT JOIN [Notification].[Producer] AS P
	ON P.Id = 
	(
		SELECT TOP 1 Id
		FROM [Notification].[Producer]
		WHERE NotificationId = N.Id
		ORDER BY [IsSiteOfExport] DESC
	);
GO