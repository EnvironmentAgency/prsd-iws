DECLARE @UserId UNIQUEIDENTIFIER;
SELECT @UserId = Id FROM [Identity].[AspNetUsers] WHERE [Email]='sunily@sfwltd.co.uk'
INSERT [Notification].[Notification] ([Id], [UserId], [NotificationType], [CompetentAuthority], [NotificationNumber], [CreatedDate], [IsPreconsentedRecoveryFacility], [ReasonForExport], [HasSpecialHandlingRequirements], [SpecialHandlingDetails], [MeansOfTransport], [IsRecoveryPercentageDataProvidedByImporter], [PercentageRecoverable], [MethodOfDisposal], [WasteGenerationProcess], [IsWasteGenerationProcessAttached]) VALUES (N'fb6b9288-a487-41d2-8601-a4d4013148aa', @UserId, 1, 1, N'GB 0001 003000', CAST(N'2015-07-13 18:31:30.0000000' AS DateTime2), 1, N'Reason of export is my business policy', 0, NULL, N'R', 1, NULL, NULL, NULL, 1)
INSERT [Business].[Facility] ([Id], [Name], [IsActualSiteOfTreatment], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [Address2], [TownOrCity], [PostalCode], [Region], [Country], [FirstName], [LastName], [Telephone], [Fax], [Email], [NotificationId], [OtherDescription]) VALUES (N'72fb7ec7-e4f7-4032-93a9-a4d40132ca71', N'Importer Facility', 1, N'Sole Trader', N'IMPORTER123', NULL, N'Southern House', N'Opp. ABCD', N'Near XYZ', N'Woking', N'GU22 7UY', N'Surrey', N'United Kingdom', N'John', N'Bob', N'44-2225557777', N'44-3336669999', N'test@importer.com', N'fb6b9288-a487-41d2-8601-a4d4013148aa', NULL)
INSERT [Business].[Facility] ([Id], [Name], [IsActualSiteOfTreatment], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [Address2], [TownOrCity], [PostalCode], [Region], [Country], [FirstName], [LastName], [Telephone], [Fax], [Email], [NotificationId], [OtherDescription]) VALUES (N'a13dd1a5-ac5a-4e8f-9481-a4d40132d4fc', N'Importer Facility1', 0, N'Sole Trader', N'IMPORTER123', NULL, N'Southern House', N'Opp. ABCD', N'Near XYZ', N'Woking', N'GU22 7UY', N'Surrey', N'United Kingdom', N'John', N'Bob', N'44-2225557777', N'44-3336669999', N'test@importer.com', N'fb6b9288-a487-41d2-8601-a4d4013148aa', NULL)
INSERT [Business].[Importer] ([Id], [Name], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [Address2], [TownOrCity], [PostalCode], [Region], [Country], [FirstName], [LastName], [Telephone], [Fax], [Email], [NotificationId], [OtherDescription]) VALUES (N'089c64c9-6685-4b15-bdbf-a4d40132bb5f', N'Importer', N'Sole Trader', N'IMPORTER123', NULL, N'Southern House', N'Opp. ABCD', N'Near XYZ', N'Woking', N'GU22 7UY', N'Surrey', N'United Kingdom', N'John', N'Bob', N'44-2225557777', N'44-3336669999', N'test@importer.com', N'fb6b9288-a487-41d2-8601-a4d4013148aa', NULL)
INSERT [Business].[Producer] ([Id], [Name], [IsSiteOfExport], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [Address2], [TownOrCity], [PostalCode], [Region], [Country], [CountryId], [FirstName], [LastName], [Telephone], [Fax], [Email], [NotificationId], [OtherDescription]) VALUES (N'bc7ebca5-4861-4f8f-8d0e-a4d40131fd4f', N'Exporter', 1, N'Sole Trader', N'Not applicable', NULL, N'Southern House', N'Station Approach', N'Opp. TESCO', N'Woking', N'GU22 7UY', N'Surrey', N'United Kingdom', NULL, N'John', N'Smith', N'44-7778889999', N'44-1112223333', N'test@exporter.com', N'fb6b9288-a487-41d2-8601-a4d4013148aa', NULL)
INSERT [Business].[Producer] ([Id], [Name], [IsSiteOfExport], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [Address2], [TownOrCity], [PostalCode], [Region], [Country], [CountryId], [FirstName], [LastName], [Telephone], [Fax], [Email], [NotificationId], [OtherDescription]) VALUES (N'23b8b9c5-35a0-4e62-ac84-a4d40132169c', N'New Producer', 0, N'Sole Trader', N'Not applicable', NULL, N'Southern House', N'Station Approach', N'Opp. TESCO', N'Woking', N'GU22 7UY', N'Surrey', N'United Kingdom', NULL, N'John', N'Smith', N'44-7778889999', N'44-1112223333', N'test@producer.com', N'fb6b9288-a487-41d2-8601-a4d4013148aa', NULL)
INSERT [Business].[Exporter] ([Id], [Name], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [Address2], [TownOrCity], [PostalCode], [Region], [Country], [FirstName], [LastName], [Telephone], [Fax], [Email], [NotificationId], [OtherDescription]) VALUES (N'5b58bc37-1a76-4640-9a23-a4d40131ef26', N'Exporter', N'Sole Trader', N'EXP12345', N'EXP12356', N'Southern House', N'Station Approach', N'Opp. TESCO', N'Woking', N'GU22 7UY', N'Surrey', N'United Kingdom', N'John', N'Smith', N'44-7778889999', N'44-1112223333', N'test@exporter.com', N'fb6b9288-a487-41d2-8601-a4d4013148aa', NULL)
INSERT [Business].[ShipmentInfo] ([Id], [NotificationId], [NumberOfShipments], [Quantity], [Units], [FirstDate], [LastDate]) VALUES (N'f300dc00-f20d-49d6-8871-a4d401343224', N'fb6b9288-a487-41d2-8601-a4d4013148aa', 1, CAST(10.0000 AS Decimal(18, 4)), 3, CAST(N'2015-12-31' AS Date), CAST(N'2016-12-31' AS Date))

INSERT [Business].[Carrier] ([Id], [Name], [NotificationId], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [Address2], [TownOrCity], [PostalCode], [Region], [Country], [FirstName], [LastName], [Telephone], [Fax], [Email], [OtherDescription]) VALUES (N'26397716-26fc-451b-94e3-a4d401336477', N'Carrier', N'fb6b9288-a487-41d2-8601-a4d4013148aa', N'Sole Trader', N'CARRIER12345', NULL, N'Northern House', N'Near Moon', N'Opp Sun', N'Woking', N'GU22 7UY', N'Surrey', N'United Kingdom', N'Karen', N'Murrey', N'44-4445556666', N'44-1112223333', N'test@carrier.com', NULL)
INSERT [Business].[Carrier] ([Id], [Name], [NotificationId], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [Address2], [TownOrCity], [PostalCode], [Region], [Country], [FirstName], [LastName], [Telephone], [Fax], [Email], [OtherDescription]) VALUES (N'5a46baaa-d5dd-4c21-aac0-a4d401337a29', N'Carrier Two', N'fb6b9288-a487-41d2-8601-a4d4013148aa', N'Sole Trader', N'CAR98765', NULL, N'Northern House', N'Near Moon', N'Opp Sun', N'Woking', N'GU22 7UY', N'Surrey', N'United Kingdom', N'Karen', N'Murrey', N'44-4445556666', N'44-1112223333', N'test@carrier.com', NULL)
INSERT [Business].[PackagingInfo] ([Id], [PackagingType], [OtherDescription], [NotificationId]) VALUES (N'21393bf2-8cba-4d09-9fdb-a4d401338ccc', 4, NULL, N'fb6b9288-a487-41d2-8601-a4d4013148aa')
INSERT [Business].[PackagingInfo] ([Id], [PackagingType], [OtherDescription], [NotificationId]) VALUES (N'916bd103-265b-49bf-a619-a4d401338ccc', 5, NULL, N'fb6b9288-a487-41d2-8601-a4d4013148aa')

DECLARE @CountryId UNIQUEIDENTIFIER;
SELECT @CountryId = Id from [Lookup].[Country] where [Name] = 'United Kingdom';
DECLARE @CAId UNIQUEIDENTIFIER;
SELECT @CAId = Id from [Lookup].[CompetentAuthority] where [Code] = 'GB01';
DECLARE @EntryId UNIQUEIDENTIFIER;
DECLARE @ExitId UNIQUEIDENTIFIER;
SELECT @EntryId = Id from [Lookup].[EntryOrExitPoint] where [Name] = 'Dover';
INSERT [Notification].[StateOfExport] ([Id], [NotificationId], [CountryId], [CompetentAuthorityId], [ExitPointId]) VALUES (N'0990a856-f001-45fe-a3cd-a4d40133a0dc', N'fb6b9288-a487-41d2-8601-a4d4013148aa', @CountryId, @CAId, @EntryId)

SELECT @CountryId = Id from [Lookup].[Country] where [Name] = 'Germany';
SELECT @CAId = Id from [Lookup].[CompetentAuthority] where [Code] = 'DE 023';
SELECT @ExitId = Id from [Lookup].[EntryOrExitPoint] where [Name] = 'Aachen';
INSERT [Notification].[StateOfImport] ([Id], [NotificationId], [CountryId], [CompetentAuthorityId], [EntryPointId]) VALUES (N'a7e76c08-1939-4e56-88d8-a4d40133bd27', N'fb6b9288-a487-41d2-8601-a4d4013148aa', @CountryId, @CAId, @ExitId)

SELECT @CountryId = Id from [Lookup].[Country] where [Name] = 'France';
SELECT @CAId = Id from [Lookup].[CompetentAuthority] where [Code] = 'FR999';
SELECT @EntryId = Id from [Lookup].[EntryOrExitPoint] where [Name] = 'Calais';
SELECT @ExitId = Id from [Lookup].[EntryOrExitPoint] where [Name] = 'Lille';
INSERT [Notification].[TransitState] ([Id], [NotificationId], [CountryId], [CompetentAuthorityId], [EntryPointId], [ExitPointId], [OrdinalPosition]) VALUES (N'c0fb971c-c7f1-490a-a7b1-a4d40133db93', N'fb6b9288-a487-41d2-8601-a4d4013148aa', @CountryId, @CAId, @EntryId, @ExitId, 1)

INSERT [Business].[OperationCodes] ([Id], [NotificationId], [OperationCode]) VALUES (N'477e8c48-478a-4f76-9417-a4d40132e6b4', N'fb6b9288-a487-41d2-8601-a4d4013148aa', 2)
INSERT [Business].[OperationCodes] ([Id], [NotificationId], [OperationCode]) VALUES (N'eef1eb16-8bc1-4af0-957b-a4d40132e6b4', N'fb6b9288-a487-41d2-8601-a4d4013148aa', 3)
INSERT [Business].[OperationCodes] ([Id], [NotificationId], [OperationCode]) VALUES (N'e9f2a0ac-b776-4c78-b1ff-a4d40132e6b4', N'fb6b9288-a487-41d2-8601-a4d4013148aa', 1)
INSERT [Business].[WasteType] ([Id], [ChemicalCompositionType], [ChemicalCompositionName], [ChemicalCompositionDescription], [NotificationId], [HasAnnex], [OtherWasteTypeDescription], [EnergyInformation], [WoodTypeDescription], [OptionalInformation]) VALUES (N'd8797ce3-c58f-4faf-9db9-a4d401346416', 3, NULL, N'Wooden blocks', N'fb6b9288-a487-41d2-8601-a4d4013148aa', 0, NULL, NULL, N'Wooden blocks', NULL)
INSERT [Business].[WasteComposition] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [ChemicalCompositionType]) VALUES (N'0a6e0fc3-2d2d-4612-8829-a4d401346416', N'Food', CAST(1.00 AS Decimal(5, 2)), CAST(3.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 3)
INSERT [Business].[WasteComposition] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [ChemicalCompositionType]) VALUES (N'1f67c730-4bd2-43fe-8a17-a4d401346416', N'Plastics', CAST(1.00 AS Decimal(5, 2)), CAST(3.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 2)
INSERT [Business].[WasteComposition] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [ChemicalCompositionType]) VALUES (N'c780661f-853d-4a5a-8c05-a4d401346416', N'Wood', CAST(1.00 AS Decimal(5, 2)), CAST(3.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 4)
INSERT [Business].[WasteComposition] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [ChemicalCompositionType]) VALUES (N'a748dbf7-2b70-4e27-8ca1-a4d401346416', N'Paper', CAST(1.00 AS Decimal(5, 2)), CAST(3.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 1)
INSERT [Business].[WasteComposition] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [ChemicalCompositionType]) VALUES (N'e095e4d2-f908-48ca-948e-a4d401346416', N'Textiles', CAST(1.00 AS Decimal(5, 2)), CAST(3.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 5)
INSERT [Business].[WasteComposition] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [ChemicalCompositionType]) VALUES (N'44040e45-2f23-4cd8-a717-a4d401346416', N'Metals', CAST(1.00 AS Decimal(5, 2)), CAST(3.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 6)
INSERT [Business].[PhysicalCharacteristicsInfo] ([Id], [PhysicalCharacteristicType], [OtherDescription], [NotificationId]) VALUES (N'52d690f1-7002-49ff-8f13-a4d40134869d', 4, NULL, N'fb6b9288-a487-41d2-8601-a4d4013148aa')
INSERT [Business].[PhysicalCharacteristicsInfo] ([Id], [PhysicalCharacteristicType], [OtherDescription], [NotificationId]) VALUES (N'2210c309-8429-443c-ac7f-a4d40134869d', 3, NULL, N'fb6b9288-a487-41d2-8601-a4d4013148aa')
INSERT [Notification].[TechnologyEmployed] ([Id], [AnnexProvided], [Details], [NotificationId], [FurtherDetails]) VALUES (N'272e1409-ebc2-439b-87b5-a4d40132ebc0', 0, 'Test details less than 70 chars', N'fb6b9288-a487-41d2-8601-a4d4013148aa', 'This is furthe details for technology employed')

DECLARE @WasteCodeId UNIQUEIDENTIFIER;
SELECT @WasteCodeId = Id from [Lookup].[WasteCode] where [Code] = 'A1030';
INSERT [Business].[WasteCodeInfo] ([Id], [WasteCodeId], [CustomCode], [NotificationId]) VALUES (N'9e23dda3-283b-436a-98f7-a4d40134b271', @WasteCodeId, NULL, N'fb6b9288-a487-41d2-8601-a4d4013148aa')
SELECT @WasteCodeId = Id from [Lookup].[WasteCode] where [Code] = '01 01 01';
INSERT [Business].[WasteCodeInfo] ([Id], [WasteCodeId], [CustomCode], [NotificationId]) VALUES (N'cd07b7de-2aea-4c67-9bb9-a4d40134b271', @WasteCodeId, NULL, N'fb6b9288-a487-41d2-8601-a4d4013148aa')
SELECT @WasteCodeId = Id from [Lookup].[WasteCode] where [Description] = 'Optional import code';
INSERT [Business].[WasteCodeInfo] ([Id], [WasteCodeId], [CustomCode], [NotificationId]) VALUES (N'59566772-1305-4f6c-861d-a4d40134cf11', @WasteCodeId, N'NA', N'fb6b9288-a487-41d2-8601-a4d4013148aa')
SELECT @WasteCodeId = Id from [Lookup].[WasteCode] where [Description] = 'Optional export code';
INSERT [Business].[WasteCodeInfo] ([Id], [WasteCodeId], [CustomCode], [NotificationId]) VALUES (N'377581f6-e570-4fa6-979d-a4d40134cf11', @WasteCodeId, N'NA', N'fb6b9288-a487-41d2-8601-a4d4013148aa')
SELECT @WasteCodeId = Id from [Lookup].[WasteCode] where [Description] = 'Explosives';
INSERT [Business].[WasteCodeInfo] ([Id], [WasteCodeId], [CustomCode], [NotificationId]) VALUES (N'bd32d603-f5c6-46f9-99f4-a4d40134e397', @WasteCodeId, NULL, N'fb6b9288-a487-41d2-8601-a4d4013148aa')
SELECT @WasteCodeId = Id from [Lookup].[WasteCode] where [Code] = 'Y1';
INSERT [Business].[WasteCodeInfo] ([Id], [WasteCodeId], [CustomCode], [NotificationId]) VALUES (N'9a6aee8d-6e4a-47c9-9e98-a4d40134e397', @WasteCodeId, NULL, N'fb6b9288-a487-41d2-8601-a4d4013148aa')
SELECT @WasteCodeId = Id from [Lookup].[WasteCode] where [Code] = 'H1';
INSERT [Business].[WasteCodeInfo] ([Id], [WasteCodeId], [CustomCode], [NotificationId]) VALUES (N'9e9d509d-a612-4974-a431-a4d40134e397', @WasteCodeId, NULL, N'fb6b9288-a487-41d2-8601-a4d4013148aa')
SELECT @WasteCodeId = Id from [Lookup].[WasteCode] where [Code] = 'UN 0004';
INSERT [Business].[WasteCodeInfo] ([Id], [WasteCodeId], [CustomCode], [NotificationId]) VALUES (N'8cbe99ba-dd85-4393-bcee-a4d40134f506', @WasteCodeId, NULL, N'fb6b9288-a487-41d2-8601-a4d4013148aa')

INSERT [Business].[WasteAdditionalInformation] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [WasteInformationType]) VALUES (N'a8556283-753d-48fe-9714-a4d4013479ee', N'Chlorine', CAST(2.00 AS Decimal(5, 2)), CAST(5.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 5)
INSERT [Business].[WasteAdditionalInformation] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [WasteInformationType]) VALUES (N'a8ac0c58-850a-44cd-99ae-a4d4013479ee', N'Moisture content', CAST(2.00 AS Decimal(5, 2)), CAST(5.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 2)
INSERT [Business].[WasteAdditionalInformation] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [WasteInformationType]) VALUES (N'93bb0f25-ef6d-4f1c-ab0b-a4d4013479ee', N'Ash content', CAST(2.00 AS Decimal(5, 2)), CAST(5.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 3)
INSERT [Business].[WasteAdditionalInformation] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [WasteInformationType]) VALUES (N'a6d0fce1-b876-4e15-aca1-a4d4013479ee', N'Net calorific value (Megajoules per kg)', CAST(2.00 AS Decimal(5, 2)), CAST(5.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 1)
INSERT [Business].[WasteAdditionalInformation] ([Id], [Constituent], [MinConcentration], [MaxConcentration], [WasteTypeId], [WasteInformationType]) VALUES (N'f5daeffa-f220-4a51-b83b-a4d4013479ee', N'Heavy metals (Megajoules per kg)', CAST(2.00 AS Decimal(5, 2)), CAST(5.00 AS Decimal(5, 2)), N'd8797ce3-c58f-4faf-9db9-a4d401346416', 4)

INSERT INTO [Notification].[NotificationAssessment]
(
    [Id],
    [NotificationApplicationId],
    [Status]
)
VALUES
(
    (SELECT CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
    N'fb6b9288-a487-41d2-8601-a4d4013148aa',
    1
)