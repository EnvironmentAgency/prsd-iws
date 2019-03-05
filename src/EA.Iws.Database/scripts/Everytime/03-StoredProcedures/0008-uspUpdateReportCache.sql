﻿IF OBJECT_ID('[Reports].[uspUpdateReportCache]') IS NULL
    EXEC('CREATE PROCEDURE [Reports].[uspUpdateReportCache] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [Reports].[uspUpdateReportCache]
AS
BEGIN

    DELETE FROM [Reports].[FreedomOfInformationCache];
    DELETE FROM [Reports].[ShipmentsCache];

    INSERT INTO [Reports].[FreedomOfInformationCache] (
           [NotificationNumber]
          ,[ImportOrExport]
          ,[IsInterim]
          ,[ReceivedDate]
          ,[CompetentAuthorityId]
          ,[NotifierName]
          ,[NotifierAddress]
          ,[NotifierPostalCode]
          ,[ProducerName]
          ,[ProducerAddress]
          ,[ProducerPostalCode]
          ,[PointOfExport]
          ,[PointOfEntry]
          ,[ImportCountryName]
          ,[ChemicalCompositionTypeId]
          ,[NameOfWaste]
          ,[EWC]
          ,[YCode]
          ,[HCode]
          ,[OperationCodes]
          ,[ImporterName]
          ,[ImporterAddress]
          ,[ImporterPostalCode]
          ,[FacilityName]
          ,[FacilityAddress]
          ,[FacilityPostalCode]
          ,[IntendedQuantity]
          ,[IntendedQuantityUnit]
          ,[IntendedQuantityUnitId]
          ,[ConsentFrom]
          ,[ConsentTo]
          ,[LocalArea]
          ,[MovementNumber]
          ,[MovementReceivedDate]
          ,[MovementCompletedDate]
          ,[MovementQuantityReceviedUnitId]
          ,[MovementQuantityReceived]
          ,[BaselOecdCode]
          ,[ExportCountryName] )
    SELECT [NotificationNumber]
          ,[ImportOrExport]
          ,[IsInterim]
          ,[ReceivedDate]
          ,[CompetentAuthorityId]
          ,[NotifierName]
          ,[NotifierAddress]
          ,[NotifierPostalCode]
          ,[ProducerName]
          ,[ProducerAddress]
          ,[ProducerPostalCode]
          ,[PointOfExport]
          ,[PointOfEntry]
          ,[ImportCountryName]
          ,[ChemicalCompositionTypeId]
          ,[NameOfWaste]
          ,[EWC]
          ,[YCode]
          ,[HCode]
          ,[OperationCodes]
          ,[ImporterName]
          ,[ImporterAddress]
          ,[ImporterPostalCode]
          ,[FacilityName]
          ,[FacilityAddress]
          ,[FacilityPostalCode]
          ,[IntendedQuantity]
          ,[IntendedQuantityUnit]
          ,[IntendedQuantityUnitId]
          ,[ConsentFrom]
          ,[ConsentTo]
          ,[LocalArea]
          ,[MovementNumber]
          ,[MovementReceivedDate]
          ,[MovementCompletedDate]
          ,[MovementQuantityReceviedUnitId]
          ,[MovementQuantityReceived]
          ,[BaselOecdCode]
          ,[ExportCountryName]
      FROM [Reports].[FreedomOfInformation];

    INSERT INTO [Reports].[ShipmentsCache] (
           [NotificationId]
          ,[NotificationNumber]
          ,[CompetentAuthorityId]
          ,[Exporter]
          ,[Importer]
          ,[Facility]
          ,[ShipmentNumber]
          ,[ActualDateOfShipment]
          ,[ConsentFrom]
          ,[ConsentTo]
          ,[PrenotificationDate]
          ,[ReceivedDate]
          ,[CompletedDate]
          ,[QuantityReceived]
          ,[QuantityReceivedUnit]
          ,[QuantityReceivedUnitId]
          ,[ChemicalCompositionTypeId]
          ,[ChemicalComposition]
          ,[LocalArea]
          ,[TotalQuantity]
          ,[TotalQuantityUnits]
          ,[TotalQuantityUnitsId]
          ,[EntryPort]
          ,[DestinationCountry]
          ,[ExitPort]
          ,[OriginatingCountry]
          ,[Status]
          ,[NotificationReceivedDate]
          ,[EwcCodes]
          ,[ImportOrExport]
          ,[BaselOecdCode]
          ,[OperationCodes]
          ,[YCode]
          ,[HCode]
          ,[UNClass]
		  ,[SiteOfExportName]
		  ,[RejectedShipmentDate] )
    SELECT [NotificationId]
          ,[NotificationNumber]
          ,[CompetentAuthorityId]
          ,[Exporter]
          ,[Importer]
          ,[Facility]
          ,[ShipmentNumber]
          ,[ActualDateOfShipment]
          ,[ConsentFrom]
          ,[ConsentTo]
          ,[PrenotificationDate]
          ,[ReceivedDate]
          ,[CompletedDate]
          ,[QuantityReceived]
          ,[QuantityReceivedUnit]
          ,[QuantityReceivedUnitId]
          ,[ChemicalCompositionTypeId]
          ,[ChemicalComposition]
          ,[LocalArea]
          ,[TotalQuantity]
          ,[TotalQuantityUnits]
          ,[TotalQuantityUnitsId]
          ,[EntryPort]
          ,[DestinationCountry]
          ,[ExitPort]
          ,[OriginatingCountry]
          ,[Status]
          ,[NotificationReceivedDate]
          ,[EwcCodes]
          ,[ImportOrExport]
          ,[BaselOecdCode]
          ,[OperationCodes]
          ,[YCode]
          ,[HCode]
          ,[UNClass]
		  ,[SiteOfExportName]
		  ,[RejectedShipmentDate]
      FROM [Reports].[Shipments]

END
GO