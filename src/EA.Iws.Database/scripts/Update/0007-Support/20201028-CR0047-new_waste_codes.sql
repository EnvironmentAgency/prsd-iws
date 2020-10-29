GO
PRINT N'ALTER [Lookup].[WasteCode]...';

DECLARE @baselcodetype int = (SELECT TOP 1 Id FROM Lookup.CodeType WHERE [Name] = 'Basel');
DECLARE @oecdcodetype int = (SELECT TOP 1 Id FROM Lookup.CodeType WHERE [Name] = 'OECD');
DECLARE @ycodetype int = (SELECT TOP 1 Id FROM Lookup.CodeType WHERE [Name] = 'Y-Code');

insert into [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('5F423A53-74EA-4A36-908B-A5D9D808715F','B3011','Plastic waste listed, provided it is destined for recycling in an environmentally sound manner and almost free from contamination and other types of wastes: Plastic waste almost exclusively consisting of one non-halogenated polymer, Plastic waste almost exclusively consisting of one cured resin or condensation product, including but not limited to the following resins, Plastic waste almost exclusively consisting of one of the following fluorinated polymers - Mixtures of plastic waste, consisting of polyethylene (PE), polypropylene (PP) and/or polyethylene terephthalate (PET), provided they are destined for separate recycling of each material and in an environmentally sound manner, and almost free from contamination and other types of wastes.', @baselcodetype);
insert into [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('F8DB6971-7A98-44E4-877F-5A63B3815AFF','EU3011','Plastic waste listed, provided it is destined for recycling in an environmentally sound manner and almost free from contamination and other types of wastes: Plastic waste almost exclusively consisting of one non-halogenated polymer, Plastic waste almost exclusively consisting of one cured resin or condensation product, including but not limited to the following resins, Plastic waste almost exclusively consisting of one of the following fluorinated polymers - Mixtures of plastic waste, consisting of polyethylene (PE), polypropylene (PP) and/or polyethylene terephthalate (PET), provided they are destined for separate recycling of each material and in an environmentally sound manner, and almost free from contamination and other types of wastes.', @baselcodetype);

insert into [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('3A8CB059-4FB2-4DA9-B631-EE727C387F6B','AC300','Plastic waste, including mixtures of such waste, containing or contaminated with Annex I constituents, to an extent that it exhibits an Appendix 2 characteristic.', @oecdcodetype);
insert into [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('0EBD6BA3-FCA6-4DF9-93FC-FD633D24A9BB','BEU01','Pressure sensitive adhesive label laminate waste containing raw materials used in label material production not covered by Basel entry B3020', @oecdcodetype);
insert into [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('C9B8083B-E5E8-4469-8D8F-C13B4C53D0AD','BEU02','Non-separable plastic fraction from the pre-treatment of used liquid packages.', @oecdcodetype);
insert into [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('9DA84FE5-2FD9-4261-9E4D-85ED56590B42','BEU03','Non-separable plastic-aluminium fraction from the pre-treatment of used liquid packages.', @oecdcodetype);
insert into [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('AA2F0251-B4E3-4CD0-8F59-B4E85F3A3D07','BEU04','Composite packaging consisting of mainly paper and some plastic, not containing residues and not covered by Basel entry B3020.', @oecdcodetype);
insert into [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('D1A36AD3-4E29-42B4-AD51-89A5D167AEEE','BEU05','Clean biodegradable waste from agriculture, horticulture, forestry, gardens, parks and cemeteries.', @oecdcodetype);

insert into [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('D31791F1-EA5E-4E0F-B4EE-F1BC38474F74','Y48','Plastic waste, including mixtures of such waste, with the exception of the following: Plastic waste that is hazardous waste pursuant to paragraph 1 (a) of Article 1. Plastic waste listed below, provided it is destined for recycling in an environmentally sound manner and almost free from contamination and other types of wastes: Plastic waste almost exclusively consisting of one non-halogenated polymer, Plastic waste almost exclusively consisting of one cured resin or condensation product, Plastic waste almost exclusively consisting of one of the listed fluorinated polymers. Mixtures of plastic waste, consisting of polyethylene (PE), polypropylene (PP) and/or polyethylene terephthalate (PET), provided they are destined for separate recycling of each material and in an environmentally sound manner and almost free from contamination and other types of wastes.', @ycodetype);

insert into [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('149E5AAD-84FC-4A69-B716-C3E2D393FFBD','EU48','Plastic waste, including mixtures of such waste, with the exception of the following: Plastic waste that is hazardous waste pursuant to paragraph 1 (a) of Article 1. Plastic waste listed below, provided it is destined for recycling in an environmentally sound manner and almost free from contamination and other types of wastes: Plastic waste almost exclusively consisting of one non-halogenated polymer, Plastic waste almost exclusively consisting of one cured resin or condensation product, Plastic waste almost exclusively consisting of one of the listed fluorinated polymers. Mixtures of plastic waste, consisting of polyethylene (PE), polypropylene (PP) and/or polyethylene terephthalate (PET), provided they are destined for separate recycling of each material and in an environmentally sound manner and almost free from contamination and other types of wastes. ', @ycodetype);

GO
PRINT N'Update complete.';