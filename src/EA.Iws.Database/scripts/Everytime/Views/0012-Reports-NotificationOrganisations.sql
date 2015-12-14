IF OBJECT_ID('[Reports].[NotificationOrganisations]') IS NULL
    EXEC('CREATE VIEW [Reports].[NotificationOrganisations] AS SELECT 1 AS [NOTHING];')
GO

/*
*	Returns the organisation details for each of the businesses involved in an export notification.
*	Facility: Takes actual site if available, any facility if not, or null where none available
*	Producer: Takes site of export if available, any producer if not, or null where none available
*/
ALTER VIEW [Reports].[NotificationOrganisations]
AS
	SELECT	N.[Id],
			N.[NotificationNumber],
			E.[Name] AS [Exporter],
			E.[FirstName] + ' ' + E.[LastName] AS [ExporterContactName],
			E.[RegistrationNumber] AS [ExporterRegistrationNumber],
			I.[Name] AS [Importer],
			I.[FirstName] + ' ' + I.[LastName] AS [ImporterContactName],
			I.[RegistrationNumber] AS [ImporterRegistrationNumber],
			P.[Name] AS [Producer],
			P.[FirstName] + ' ' + P.[LastName] AS [ProducerContactName],
			P.[RegistrationNumber] AS [ProducerRegistrationNumber],
			F.[Name] AS [Facility],
			F.[FirstName] + ' ' + F.[LastName] AS [FacilityContactName],
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
	)

	UNION

	SELECT	N.[Id],
			N.[NotificationNumber],
			E.[Name] AS [Exporter],
			E.[ContactName] AS [ExporterContactName],
			NULL AS [ExporterRegistrationNumber],
			I.[Name] AS [Importer],
			I.[ContactName] AS [ImporterContactName],
			I.[RegistrationNumber] AS [ImporterRegistrationNumber],
			P.[Name] AS [Producer],
			P.[ContactName] AS [ProducerContactName],
			NULL AS [ProducerRegistrationNumber],
			F.[Name] AS [Facility],
			F.[ContactName] AS [FacilityContactName],
			F.[RegistrationNumber] AS [FacilityRegistrationNumber]

	FROM		[ImportNotification].[Notification] AS N

	LEFT JOIN	[ImportNotification].[Exporter] AS E
	ON			E.[ImportNotificationId] = N.[Id]

	LEFT JOIN	[ImportNotification].[Importer] AS I
	ON			I.[ImportNotificationId] = N.[Id]

	LEFT JOIN	[ImportNotification].[Facility] AS F
	ON F.Id = 
	(
		SELECT TOP 1 F1.Id

		FROM		[ImportNotification].[Facility] AS F1

		INNER JOIN	[ImportNotification].[FacilityCollection] AS FC
		ON			FC.ImportNotificationId = N.Id

		WHERE		ImportNotificationId = N.Id
		ORDER BY	F.ContactName DESC	-- TODO: Order by actual site
	)
	LEFT JOIN [ImportNotification].[Producer] AS P
	ON P.Id = 
	(
		SELECT TOP 1 Id
		FROM [ImportNotification].[Producer]
		WHERE ImportNotificationId = N.Id
	)
GO