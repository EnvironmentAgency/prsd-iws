﻿INSERT INTO [Lookup].[Country] ([Id], [Name], [IsoAlpha2Code], [IsEuropeanUnionMember])
VALUES ((SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'Saint Pierre and Miquelon',
		'PM',
		1);
GO

IF (SELECT COUNT(1) FROM [Notification].[StateOfExport]) > 0 
OR (SELECT COUNT(1) FROM [Notification].[StateOfImport]) > 0
OR (SELECT COUNT(1) FROM [Notification].[TransitState]) > 0
OR (SELECT COUNT(1) FROM [ImportNotification].[StateOfExport]) > 0
OR (SELECT COUNT(1) FROM [ImportNotification].[StateOfImport]) > 0
OR (SELECT COUNT(1) FROM [ImportNotification].[TransitState]) > 0
BEGIN
	PRINT 'Dependent data exists in the database, the competent authority update script will not be run';
	RETURN;
END


DECLARE @CompetentAuthorities TABLE
(
	Country NVARCHAR(500),
	CompetentAuthority NVARCHAR(3000),
	Code NVARCHAR(16),
	IsTransit NCHAR(1)
);

INSERT INTO @CompetentAuthorities 
(
	Country, 
	CompetentAuthority,
	Code,
	IsTransit
)
VALUES
(N'Austria', N'Bundesministerium für Land- und Forstwirtschaft, Umwelt und Wasserwirtschaft', N'AT', N'Y'),
(N'Belgium', N'Flanders OVAM', N'BE001', N'N'),
(N'Belgium', N'Brussels Capital IBGE-BIM Waste Shipment', N'BE002', N'N'),
(N'Belgium', N'Wallonia Service public de Wallonie Direction générale de l''Agriculture, des Ressources naturelles et de l''Environnement', N'BE003', N'N'),
(N'Belgium', N'Interregional Packaging Commission (IVC-CIE)', N'BE004', N'Y'),
(N'Bulgaria', N'Ministry of Environment and Water Waste Management and Soil Protection Directorate', N'BG', N'Y'),
(N'Croatia', N'Ministry of Environmental and Nature Protection', N'CR', N'Y'),
(N'Cyprus', N'Environment Service', N'CY', N'Y'),
(N'Czech Republic', N'Ministry of the Environment', N'CZ', N'Y'),
(N'Denmark', N'Danish Environmental Protection Agency, EPA', N'DK', N'Y'),
(N'Estonia', N'Ministry of the Environment', N'EE', N'Y'),
(N'Finland', N'Finnish Environment Institute', N'FIN', N'Y'),
(N'France', N'The National Office for Transfrontier Shipments of Waste', N'FR', N'Y'),
(N'Guadeloupe', N'PNTTD', N'FR971', N'N'),
(N'Martinique', N'PNTTD', N'FR972', N'N'),
(N'French Guiana', N'PNTTD', N'FR973', N'N'),
(N'Réunion', N'PNTTD', N'FR974', N'N'),
(N'Mayotte', N'PNTTD', N'FR976', N'N'),
(N'Saint Pierre and Miquelon', N'PNTTD', N'FR975', N'N'),
(N'New Caledonia', N'DIMENC', N'FR988', N'N'),
(N'French Polynesia', N'Gouvernement de la Polynésie française Direction de l''environnement BP', N'FR987', N'N'),
(N'Wallis and Futuna Islands', N'Monsieur le Préfet, Administrateur supérieur des Iles Wallis et Futuna Service de l''Environnement BP', N'FR986', N'N'),
(N'Saint-Martin (France)', N'PNTTD', N'FR978', N'N'),
(N'Saint Barthélemy', N'Collectivité de Saint-Barthélemy Hôtel de la collectivité La Pointe Gustavia BP', N'FR977', N'N'),
(N'Germany', N'Umweltbundesamt Anlaufstelle Basler Übereinkommen', N'DE005', N'Y'),
(N'Germany', N'Senatsverwaltung für Gesundheit, Umwelt und Verbraucherschutz', N'DE006', N'N'),
(N'Germany', N'Der Senator für Umwelt, Bau und Verkehr', N'DE007', N'N'),
(N'Germany', N'SAA Sonderabfallagentur Baden-Württemberg GmbH', N'DE008', N'N'),
(N'Germany', N'Behörde für Stadtentwicklung und Umwelt Abteilung Abfallwirtschaft', N'DE009', N'N'),
(N'Germany', N'Regierungspräsidium Darmstadt', N'DE010', N'N'),
(N'Germany', N'Regierungspräsidium Gießen', N'DE011', N'N'),
(N'Germany', N'Regierungspräsidium Kassel ', N'DE012', N'N'),
(N'Germany', N'Regierungspräsidium Kassel ', N'DE013', N'N'),
(N'Germany', N'Niedersächsische Gesellschaft zur Endablagerung von Sonderabfall mbH', N'DE014', N'N'),
(N'Germany', N'SBB Sonderabfallgesellschaft Brandenburg/Berlin mbH', N'DE015', N'N'),
(N'Germany', N'Bezirksregierung Arnsberg Postfach', N'DE016', N'N'),
(N'Germany', N'Bezirksregierung Detmold', N'DE017', N'N'),
(N'Germany', N'Bezirksregierung Düsseldorf', N'DE018', N'N'),
(N'Germany', N'Bezirksregierung Köln', N'DE019', N'N'),
(N'Germany', N'Bezirksregierung Münster', N'DE020', N'N'),
(N'Germany', N'Sonderabfall-Management-Gesellschaft mbH', N'DE021', N'N'),
(N'Germany', N'Landesdirektion Dresden', N'DE022', N'N'),
(N'Germany', N'Thüringer Landesverwaltungsamt', N'DE023', N'N'),
(N'Germany', N'Landesamt für Umwelt und Arbeitsschutz Saarland', N'DE024', N'N'),
(N'Germany', N'Gesellschaft für die Organisation der Entsorgung von Sonderabfällen mbH', N'DE025', N'N'),
(N'Germany', N'Regierung von Oberbayern', N'DE026', N'N'),
(N'Germany', N'Regierung von Niederbayern', N'DE027', N'N'),
(N'Germany', N'Regierung der Oberpfalz Postfach', N'DE028', N'N'),
(N'Germany', N'Regierung von Oberfranken', N'DE029', N'N'),
(N'Germany', N'Regierung von Mittelfranken', N'DE030', N'N'),
(N'Germany', N'Regierung von Unterfranken', N'DE031', N'N'),
(N'Germany', N'Regierung von Schwaben', N'DE032', N'N'),
(N'Germany', N'Landesverwaltungsamt Sachsen-Anhalt (LVwA)', N'DE040', N'N'),
(N'Germany', N'Landesamt für Geologie und Bergwesen (LAGB)', N'DE043', N'N'),
(N'Germany', N'Landwirtschaftskammer Niedersachsen', N'DE047', N'N'),
(N'Germany', N'Landesamt für Umwelt, Naturschutz und Geologie Mecklenburg-Vorpommern', N'DE049', N'N'),
(N'Greece', N'Hellenic Ministry for the Environment, Physical Planning and Public Works', N'GR', N'Y'),
(N'Hungary', N'National Inspectorate for Environment and Nature Attila Ábrahám Head of Section', N'HU', N'Y'),
(N'Ireland', N'National TFS Office, Dublin City Council', N'IE31', N'Y'),
(N'Italy', N'Ministero dell’Ambiente e della Tutela del Territorio', N'IT', N'Y'),
(N'Italy', N'Agenzia Provinciale per la protezione dell''ambiente e tutela del lavoro', N'IT', N'N'),
(N'Italy', N'Agenzia Provinciale per la Protezione dell''Ambiente Settore Tecnico Suolo - U.O. Tutela del Suolo', N'IT', N'N'),
(N'Italy', N'Direzione Turismo Ambiente Energia', N'IT', N'N'),
(N'Italy', N'Provincia di POTENZAAmbienteIng. A. Santoro', N'IT', N'N'),
(N'Italy', N'Ambiente', N'IT', N'N'),
(N'Italy', N'Dipartimento Politiche dell’Ambiente Dipartimento 14, Settore 54 Ufficio Ecologia Dipartimento 5,Servizio 2', N'IT', N'N'),
(N'Italy', N'Settore Tecnico Amministrativo Provinciale Ecologia AVELLINO', N'IT', N'N'),
(N'Italy', N'Settore Provinciale Ecologia BENEVENTO', N'IT', N'N'),
(N'Italy', N'Settore Tecnico Amministrativo Provinciale CASERTA', N'IT', N'N'),
(N'Italy', N'Settore Tecnico Amministrativo Provinciale Ecologia NAPOLI', N'IT', N'N'),
(N'Italy', N'Settore Tecnico Amministrativo Provinciale Ecologia SALERNO', N'IT', N'N'),
(N'Italy', N'Servizio Tutela del Territorio Funzione Ambiente', N'IT', N'N'),
(N'Italy', N'Settore Tutela Ambientale - Servizio gestione rifiuti', N'IT', N'N'),
(N'Italy', N'Direzione D’Area Ambiente - Servizio Risorse Ambientali', N'IT', N'N'),
(N'Italy', N'Direzione Territorio e Ambiente', N'IT', N'N'),
(N'Italy', N'Settore Ambiente Ufficio Impianti Rifiuti e BonificheDott.', N'IT', N'N'),
(N'Italy', N'Settore Ambiente', N'IT', N'N'),
(N'Italy', N'Servizio Ambiente e Sicurezza del Territorio Ufficio Pianif. E Gestione Rifiuti', N'IT', N'N'),
(N'Italy', N'Settore Difesa del Suolo e Tutela dell’Ambiente', N'IT', N'N'),
(N'Italy', N'Servizio Ambiente Difesa del Suolo e Forestazione', N'IT', N'N'),
(N'Italy', N'Valorizzazione e Tutela dell’Ambiente', N'IT', N'N'),
(N'Italy', N'Settore Ambiente e Suolo', N'IT', N'N'),
(N'Italy', N'Servizio Tutela Ambientale', N'IT', N'N'),
(N'Italy', N'Servizio Ambiente', N'IT', N'N'),
(N'Italy', N'Dipartimento Territorio Direzione Regionale Energia e Rifiuti', N'IT', N'N'),
(N'Italy', N'Area 8 - Ambiente-Ufficio Suolo', N'IT', N'N'),
(N'Italy', N'Amministrazione ambientale', N'IT', N'N'),
(N'Italy', N'Settore Tutela dell’Ambiente', N'IT', N'N'),
(N'Italy', N'Settore Tutela dell’Ambiente', N'IT', N'N'),
(N'Italy', N'Reti e Servizi di pubblica utilità', N'IT', N'N'),
(N'Italy', N'Servizio Ambiente e Paesaggio P.F. Salvaguardia, Sostenibilità e Cooperazione Ambientale', N'IT', N'N'),
(N'Italy', N'Servizio Prevenzione e Tutela Ambiente', N'IT', N'N'),
(N'Italy', N'Direzione Pianificazione Difesa del Suolo - VIA-Servizi Tecnici-SIT-Servizio Gestione Rifiuti', N'IT', N'N'),
(N'Italy', N'Servizio Ambiente', N'IT', N'N'),
(N'Italy', N'Settore Tutela Ambientale ed Agricoltura', N'IT', N'N'),
(N'Italy', N'Area funzionale del territorio', N'IT', N'N'),
(N'Italy', N'Tutela e sviluppo del territorio', N'IT', N'N'),
(N'Italy', N'Area Ambiente, Parchi, Risorse Idriche e Tutela della Fauna', N'IT', N'N'),
(N'Italy', N'Ambiente e Georisorse', N'IT', N'N'),
(N'Italy', N'Assessorato Ambiente', N'IT', N'N'),
(N'Italy', N'Assessorato Ambiente e Territorio', N'IT', N'N'),
(N'Italy', N'Servizio Ecologia', N'IT', N'N'),
(N'Italy', N'Servizio Ambiente', N'IT', N'N'),
(N'Italy', N'Servizio Rifiuti, Scarichi emissioni e controllo impianti', N'IT', N'N'),
(N'Italy', N'Settore Ecologia - Ambiente', N'IT', N'N'),
(N'Italy', N'15.1.1. Assessorato all’Ambiente e Difesa del Territorio Settore Ecologia e Protezione Civile', N'IT', N'N'),
(N'Italy', N'Settore Ambiente e Valorizzazione del Territorio', N'IT', N'N'),
(N'Italy', N'Assessorato Ambiente', N'IT', N'N'),
(N'Italy', N'Assessorato Ambiente', N'IT', N'N'),
(N'Italy', N'Servizio Ambiente', N'IT', N'N'),
(N'Italy', N'Settore VIII Ambiente e Agricoltura', N'IT', N'N'),
(N'Italy', N'Settore Ambiente', N'IT', N'N'),
(N'Italy', N'Assessorato Ambiente', N'IT', N'N'),
(N'Italy', N'Agenzia Regionale per i Rifiuti e le Acque - Settore Rifiuti e Bonifiche Avv. ', N'IT', N'N'),
(N'Italy', N'Area Territorio e Servizio Ambiente', N'IT', N'N'),
(N'Italy', N'Gestione rifiuti e bonifiche siti inquinati', N'IT', N'N'),
(N'Italy', N'Settore Sviluppo e Tutela del Territorio e Area Ambiente', N'IT', N'N'),
(N'Italy', N'Provincia di Livorno', N'IT', N'N'),
(N'Italy', N'Servizio Ambiente', N'IT', N'N'),
(N'Italy', N'Settore Ambiente e Trasporti Servizio Rifiuti', N'IT', N'N'),
(N'Italy', N'Servizio Ambiente', N'IT', N'N'),
(N'Italy', N'Settore Tutela dell’Ambiente', N'IT', N'N'),
(N'Italy', N'U.O.C. Tutela Ambiente', N'IT', N'N'),
(N'Italy', N'Amministrazione Provinciale di Siena Servizio Ambiente', N'IT', N'N'),
(N'Italy', N'Ufficio difesa del suolo, dell’ambiente e infrastrutture I Settore', N'IT', N'N'),
(N'Italy', N'Assessorato Regionale Territorio Ambiente e Opere Pubbliche Dipartimento Territorio, Ambiente e Risorse Idriche - Ufficio Tutela dell’Ambiente', N'IT', N'N'),
(N'Italy', N'Segreteria Regionale all’Ambiente', N'IT', N'N'),
(N'Latvia', N'State Environmental Service of Latvia', N'LV', N'Y'),
(N'Lithuania', N'Environmental Protection Agency', N'LT', N'Y'),
(N'Luxembourg', N'Administration de l''Environnement', N'LU', N'Y'),
(N'Malta', N'Malta Environment and Planning Authority', N'MT', N'Y'),
(N'The Netherlands', N'ILT/Handhavingsbeleid/EVOA en Besluiten vergunningverlening Inspectie Leefomgeving en Transport Ministerie van Infrastructuur en Milieu', N'NL', N'Y'),
(N'Poland', N'Chief Inspectorate for Environmental Protection Division of Transboundary Movement of Waste', N'PL', N'Y'),
(N'Portugal', N'Agência Portuguesa do Ambiente', N'PT', N'Y'),
(N'Romania', N'National Environmental Protection Agency', N'RO', N'Y'),
(N'Slovakia', N'Ministry of the Environmental of the Slovak Republic Waste Management Department', N'SK', N'Y'),
(N'Slovenia', N'Environmental Agency of the Republic of Slovenia', N'SL', N'Y'),
(N'Spain', N'Subdirección General de Producción y Consumo Sostenibles Dirección General de Calidad y Evaluación Ambiental Ministerio de Medio Ambiente, y Medio Rural y Marino', N'ES', N'Y'),
(N'Spain', N'Dirección General de Prevención y Calidad Ambiental Consejería de Medio Ambiente', N'ES', N'N'),
(N'Spain', N'Dirección General de Calidad Ambiental y Cambio Climático Departamento de Medio Ambiente', N'ES', N'N'),
(N'Spain', N'Dirección General de Agua y Calidad Ambiental Consejería de Medio Ambiente, Ordenación del Territorio e Infraestructuras ', N'ES', N'N'),
(N'Spain', N'Dirección General de Calidad Ambiental Consejería de Medio Ambiente', N'', N'N'),
(N'Spain', N'Servicio de Residuos y Control de la Contaminación Consejería de Medio Ambiente', N'ES', N'N'),
(N'Spain', N'Dirección General de Medio Ambiente Consejería de Medio Ambiente', N'ES', N'N'),
(N'Spain', N'Dirección General de Calidad y Sostenibilidad Ambiental Consejería de Industria, Energía y Medio Ambiente ', N'ES', N'N'),
(N'Spain', N'Servicio de Control de la Gestión de los Residuos Consejería de Medio Ambiente', N'ES', N'N'),
(N'Spain', N'Agencia de Residuos de Cataluña Departamento de Atención Ciudadana', N'ES', N'N'),
(N'Spain', N'Dirección General de Medio Ambiente Consejería de Agricultura y Medio Ambiente', N'ES', N'N'),
(N'Spain', N'Dirección General de Calidad y Evaluación Ambiental Consejería de Medio Ambiente', N'ES', N'N'),
(N'Spain', N'Dirección General de Calidad Ambiental Consejería de Turismo, Medio Ambiente y Política Territorial Gobierno de La Rioja', N'ES', N'N'),
(N'Spain', N'Dirección General de Calidad Ambiental Consejería de Turismo, Medio Ambiente y Política Territorial Gobierno de La Rioja', N'ES', N'N'),
(N'Spain', N'Dirección General de Medio Ambiente Área de Planificación y Gestión de Residuos Consejería de Medio Ambiente', N'ES', N'N'),
(N'Spain', N'Consejería de Agricultura y Agua Dirección General de Planificación, Evaluación y Control Ambiental', N'ES', N'N'),
(N'Spain', N'Servicio de Calidad Ambiental Departamento de Desarrollo Rural y Medio Ambient', N'ES', N'N'),
(N'Spain', N'Servicio de Residuos Peligrosos Viceconsejería de Medio Ambiente Departamento de Medio Ambiente y Ordenación del Territorio', N'ES', N'N'),
(N'Spain', N'Dirección General para el Cambio Climático Consejería de Medio Ambiente, Agua, Urbanismo y Vivienda', N'ES', N'N'),
(N'Sweden', N'Naturvårdsverket/ The Swedish Environmental Protection Agency', N'SE001', N'Y'),
(N'Iceland', N'Environment Agency of Iceland', N'IS', N'Y'),
(N'Liechtenstein', N'Office of Environmental Protection Waste Management Division', N'LI', N'Y'),
(N'Norway', N'The Norwegian Environment Agency', N'NO', N'Y'),
(N'Afghanistan', N'National Environmental Protection Agency', N'AF', N'N'),
(N'Albania', N'Ministry of Environment', N'AL', N'N'),
(N'Algeria', N'Ministère de l''Aménagement du Territoire et de l''Environnement', N'DZ', N'N'),
(N'Andorra', N'Ministry of Environment, Agriculture and Sustainability', N'AD', N'N'),
(N'Angola', N'Ministry of Urban Affairs and Environment', N'AO', N'N'),
(N'Antigua and Barbuda', N'Ministry of Tourism and Environment', N'Y', N'N'),
(N'Antigua and Barbuda', N'Ministry of Agriculture, Land, Fisheries and Barbuda Affairs', N'Y', N'N'),
(N'Argentina', N'Secretaría de Ambiente y Desarrollo Sustenable de la Jefatura de Gabinete de Ministros', N'AG', N'N'),
(N'Armenia', N'Ministry of Nature Protection', N'AG', N'N'),
(N'Australia', N'Department of the Environment', N'AU', N'N'),
(N'Azerbaijan', N'Ministry of Ecology and Natural Resources', N'AZ', N'N'),
(N'Bahamas', N'The Bahamas Environment, Science and Technology Commission', N'BS', N'N'),
(N'Bahrain', N'Supreme Council for the Environment', N'BH', N'N'),
(N'Bangladesh', N'Ministry of Environment and Forests', N'BD', N'N'),
(N'Barbados', N'Ministry of Environment, Water Resources Management and Drainage', N'BB', N'N'),
(N'Belarus', N'Ministry of Natural Resources and Environmental Protection', N'BY', N'N'),
(N'Belize', N'Ministry of Forestry, Fisheries and Sustainable Development', N'BZ', N'N'),
(N'Benin', N'Ministère de l''environnement, de l''habitat et de l''urbanisme', N'BJ', N'N'),
(N'Bhutan', N'National Environment Commission Secretariat', N'BT', N'N'),
(N'Bolivia', N'Ministerio de Medio Ambiente y Agua', N'BO', N'N'),
(N'Bosnia and Herzegovina', N'Ministry of Environment and Tourism', N'BA', N'N'),
(N'Bosnia and Herzegovina', N'Ministry of Physical Planning, Civil Engineering and Ecology', N'BA', N'N'),
(N'Botswana', N'Ministry of Environment Wildlife and Tourism', N'BW', N'N'),
(N'Brazil', N'Brazilian Institute of Environmental and Renewable Natural Resources', N'BR', N'N'),
(N'Brazil', N'Ministry of the Environment', N'BR', N'N'),
(N'Brunei Darussalam', N'Ministry of Development', N'BN', N'N'),
(N'Burkina Faso', N'Ministère de l''Environnement et des ressources halieutiques', N'BF', N'N'),
(N'Burundi', N'Ministère de l''Aménagement du Territoire, de l''Environnement et du Tourisme', N'BI', N'N'),
(N'Cambodia', N'Ministry of Environment', N'KH', N'N'),
(N'Cameroon', N'Ministère de l''Environnement, de la Protection de la Nature et du Développement Durable', N'CM', N'N'),
(N'Canada', N'Environment Canada', N'CA', N'N'),
(N'Cape Verde', N'Ministère de l''Environnement, de l''Habitation et de l''Aménagement du Territoire', N'CV', N'N'),
(N'Central African Republic', N'Ministère de l’Environnement de l''Écologie et du Developpement Durable', N'CF', N'N'),
(N'Chad', N'Ministère de l''Agriculture et de l''Environnement', N'TD', N'N'),
(N'Chile', N'Ministerio del Medio Ambiente', N'CL', N'N'),
(N'Chile', N'Ministerio del Medio Ambiente', N'CL', N'N'),
(N'Chile', N'Ministerio del Medio Ambiente', N'CL', N'N'),
(N'China', N'Ministry of Environmental Protection', N'CN', N'N'),
(N'China', N'Government of Hong Kong Special Administrative Region of the People''s Republic of China', N'CN', N'N'),
(N'China', N'Environment Protection Bureau', N'CN', N'N'),
(N'Colombia', N'Autoridad Nacional de Licencias Ambientales', N'CO', N'N'),
(N'Colombia', N'Ministerio del Ambiente y Desarrollo Sostenible', N'CO', N'N'),
(N'Comoros', N'Vice-Présidence en Charge du Ministère de l’Environnement', N'KM', N'N'),
(N'Congo', N'Ministère du Tourisme', N'CG', N'N'),
(N'Congo, Democratic Republic of', N'Ministère de l’Environnement, Conservation de la Nature et Tourisme', N'CD', N'N'),
(N'Cook Islands', N'National Environment Service', N'CK', N'N'),
(N'Costa Rica', N'Ministerio de Salud', N'CR', N'N'),
(N'Côte d''Ivoire', N'Ministry of Environmental and Nature Protection', N'CI', N'N'),
(N'Cuba', N'Ministerio de Ciencia Tecnología y Medio Ambiente', N'CU', N'N'),
(N'Cyprus', N'Ministry of Agriculture, Natural Resources and Environment', N'CY', N'N'),
(N'Cyprus', N'Ministry of Agriculture, Natural Resources and Environment  ', N'CY', N'N'),
(N'Djibouti', N'Ministère de l''Habitat de l''Urbanisme et de l''Environnement', N'DJ', N'N'),
(N'Djibouti', N'Ministère de l''Habitat, de l''Urbanisme et de l''Environnement', N'DJ', N'N'),
(N'Dominica', N'Ministry of Health', N'DM', N'N'),
(N'Dominican Republic', N'Ministerio de Medio Ambiente y Recursos Naturales', N'DO', N'N'),
(N'Ecuador', N'Ministerio del Ambiente (MAE)', N'EC', N'N'),
(N'Egypt', N'Ministry of Foreign Affairs', N'EG', N'N'),
(N'Egypt', N'Suez Canal Authority', N'EG', N'N'),
(N'Egypt', N'Ministry of Environment', N'EG', N'N'),
(N'El Salvador', N'Ministerio de medio ambiente y recursos naturales', N'SV', N'N'),
(N'Equatorial Guinea', N'Ministerio de Pesca y Medio Ambiente', N'GQ', N'N'),
(N'Eritrea', N'Ministry of Land, Water and Environment', N'ER', N'N'),
(N'Ethiopia', N'Private Environmental Consultant', N'ET', N'N'),
(N'Gabon', N'Ministère de la Foret de l''|Environment et de la Protection des Resources Naturelles', N'GA', N'N'),
(N'Gambia', N'National Environment Agency', N'GM', N'N'),
(N'Georgia', N'Ministry of Environment and Natural Resources Protection', N'GE', N'N'),
(N'Ghana', N'Environmental Protection Agency', N'GH', N'N'),
(N'Guatemala', N'Ministerio de Ambiente y Recursos Naturales', N'GT', N'N'),
(N'Guernsey', N'Health and Social Services', N'GG', N'N'),
(N'Guinea', N'Ministère de l''Environnement et du Développement Durable', N'GN', N'N'),
(N'Guinea-Bissau', N'Secrétariat d''Etat de l''Environment et Tourism', N'GW', N'N'),
(N'Guyana', N'Environmental Protection Agency', N'GY', N'N'),
(N'Haiti', N'Direction Technique du Ministère de l''Environnement', N'HT', N'N'),
(N'Honduras', N'Secretario de Energia, Recursos Naturales, Ambiente y Minas (SERNA)', N'HN', N'N'),
(N'Hong Kong', N'Government of Hong Kong Special Administrative Region of the People''s Republic of China', N'HK', N'N'),
(N'India', N'Ministry of Environment, Forests and Climate Change', N'IN', N'N'),
(N'Indonesia', N'Ministry of Environment', N'ID', N'N'),
(N'Iran', N'Department of the Environment', N'IR', N'N'),
(N'Iraq', N'Ministry of Health and Environment', N'IQ', N'N'),
(N'Isle of Man', N'Department of Environment, Food and Agriculture', N'IM', N'N'),
(N'Israel', N'Ministry of Environment', N'IL', N'N'),
(N'Jamaica', N'Ministry of Water, Land, Environment and Climate Change', N'JM', N'N'),
(N'Japan', N'Ministry of the Environment', N'JP', N'N'),
(N'Jersey', N'States of Jersey', N'JE', N'N'),
(N'Jordan', N'Ministry of Environment', N'JO', N'N'),
(N'Kazakhstan', N'Ministry of Energy', N'KZ', N'N'),
(N'Kenya', N'National Environment Management Authority', N'KE', N'N'),
(N'Kiribati', N'Ministry of Finance and Economic Development', N'KI', N'N'),
(N'Kuwait', N'Environment Public Authority', N'KW', N'N'),
(N'Kyrgyzstan', N'State Agency on Environment Protection and Forestry', N'KG', N'N'),
(N'Lao People''s Democratic Republic', N'Water Resources and Environment Administration', N'LA', N'N'),
(N'Lebanon', N'Ministry of Environment', N'LB', N'N'),
(N'Lesotho', N'Ministry of Tourism, Environment and Culture', N'LS', N'N'),
(N'Liberia', N'Environment Protection Agency', N'LR', N'N'),
(N'Libya', N'Environment General Authority (EGA)', N'LY', N'N'),
(N'Liechtenstein', N'Office de la Protection de l''Environnement', N'LI', N'N'),
(N'Macedonia', N'Ministry of Environment and Physical Planning', N'MK', N'N'),
(N'Madagascar', N'Ministère de l''Environnement, des Eaux et Forêts et du Tourisme', N'MG', N'N'),
(N'Malawi', N'Ministry of Natural Resources, Energy and Mining', N'MW', N'N'),
(N'Malaysia', N'Department of National Solid Waste Management', N'MY', N'N'),
(N'Malaysia', N'Ministry of Natural Resources and Environment', N'MY', N'N'),
(N'Maldives', N'Ministry of Housing, Transport and Environment', N'MV', N'N'),
(N'Mali', N'Ministère de l''Environnement et de l''Assainissement', N'ML', N'N'),
(N'Marshall Islands', N'Office of Environmental Planning and Policy Coordination', N'MH', N'N'),
(N'Mauritania', N'Ministry of Environment, Sustainable Development, Disaster and Beach Management', N'MR', N'N'),
(N'Mauritius', N'Ministry of Local Government', N'MU', N'N'),
(N'Mexico', N'Secretaría de Medio Ambiente y Recursos Naturales (SEMARNAT)', N'MX', N'N'),
(N'Micronesia, Federated States of', N'Office of Environment and Emergency Management', N'FM', N'N'),
(N'Moldova', N'Ministry of Environment', N'MD', N'N'),
(N'Monaco', N'Direction de l''Aménagement Urbain', N'MC', N'N'),
(N'Mongolia', N'Ministry of Nature, Environment and Tourism', N'MN', N'N'),
(N'Montenegro', N'Environmental Protection Agency', N'ME', N'N'),
(N'Morocco', N'Secrétariat d''Etat auprès du Ministère de l''Energie, des Mines, de l''Eau et de l''Environnement', N'MA', N'N'),
(N'Mozambique', N'Ministry for Coordination of Environmental Affairs', N'MZ', N'N'),
(N'Myanmar', N'Ministry of Environmental Conservation and Forestry', N'MM', N'N'),
(N'Namibia', N'Ministry of Environment and Tourism', N'NA', N'N'),
(N'Nauru', N'Department of Commerce Industry and Environment', N'NR', N'N'),
(N'Nepal', N'Ministry of Environment, Science and Technology', N'NP', N'N'),
(N'New Zealand', N'Environmental Protection Authority', N'NZ', N'N'),
(N'Nicaragua', N'Comisión Nacional de Registro y Control de Sustancias Tóxicas', N'NI', N'N'),
(N'Niger', N'Ministère de l''Hydraulique, de l''Environnement et de la Lutte Contre la Désertification', N'NE', N'N'),
(N'Nigeria', N'Federal Ministry of Environment', N'NG', N'N'),
(N'Oman', N'Ministry of Environment and Climate Affairs', N'OM', N'N'),
(N'Pakistan', N'Ministry of Climate Change', N'PK', N'N'),
(N'Palau', N'Environmental Quality Protection Board', N'PW', N'N'),
(N'Palestine, State of', N'Environment Quality Authority (EQA)', N'PS', N'N'),
(N'Panama', N'Ministerio de Salud', N'PA', N'N'),
(N'Papua New Guinea', N'Department of Environment and Conservation', N'PG', N'N'),
(N'Paraguay', N'Secretaría del Ambiente', N'PY', N'N'),
(N'Peru', N'Ministerio de Salud', N'PE', N'N'),
(N'Peru', N'Ministerio del Ambiente', N'PE', N'N'),
(N'Philippines', N'Department of Environment and Natural Resources (DENR)', N'PH', N'N'),
(N'Qatar', N'Ministry of Environment', N'QA', N'N'),
(N'Romania', N'National Environmental Protection Agency', N'RO', N'N'),
(N'Russian Federation', N'Federal Service for Supervision in the Field of Natural Resource Use', N'RU', N'N'),
(N'Rwanda', N'Ministry of Lands, Environment, Forestry, Water and Mines', N'RW', N'N'),
(N'Saint Kitts and Nevis', N'Ministry of Health', N'KN', N'N'),
(N'Saint Lucia', N'Ministry of Physical Development and the Environment', N'LC', N'N'),
(N'Saint Vincent and the Grenadines', N'Ministry of Health and the Environment', N'VC', N'N'),
(N'Samoa', N'Ministry of Natural Resources, Environment and Meteorology', N'WS', N'N'),
(N'Sao Tome and Principe', N'Direction Générale de l’Environnement', N'ST', N'N'),
(N'Saudi Arabia', N'Meteorology and Environmental Protection Administration (MEPA)', N'SA', N'N'),
(N'Senegal', N'Ministère de l''Environnement et des Etablissements Classés', N'SN', N'N'),
(N'Serbia', N'Ministry of Agriculture and Environmental Protection', N'RS', N'N'),
(N'Seychelles', N'Ministry of Industry', N'SC', N'N'),
(N'Singapore', N'National Environment Agency', N'SG', N'N'),
(N'Somalia', N'Ministry of State for Environment', N'SO', N'N'),
(N'South Africa', N'Department of Environmental Affairs and Tourism', N'ZA', N'N'),
(N'South Korea', N'Ministry of Land and Environment Protection', N'KR', N'N'),
(N'South Korea', N'National Coordinating Committee for Environment (NCCE)', N'KR', N'N'),
(N'Sri Lanka', N'Central Environment Authority', N'LK', N'N'),
(N'Sudan', N'Higher Council for Environment and Natural Resources', N'SD', N'N'),
(N'Suriname', N'Cabinet of the President of the Republic Suriname', N'SR', N'N'),
(N'Swaziland', N'Swaziland Environment Authority', N'SZ', N'N'),
(N'Switzerland', N'Federal Office for the Environment', N'CH', N'N'),
(N'Syria', N'Ministry of Local Administration and Environment', N'SY', N'N'),
(N'Tanzania', N'Vice President''s Office', N'TZ', N'N'),
(N'Thailand', N'Ministry of Industry', N'TH', N'N'),
(N'Togo', N'Ministère de l''Environnement et des Ressources Forestières', N'TG', N'N'),
(N'Tonga', N'Ministry of Meteorology, Energy, Information, Disaster Management, Environment, Climate Change and Communications (MEIDECC)', N'TO', N'N'),
(N'Trinidad and Tobago', N'Environmental Management Authority', N'TT', N'N'),
(N'Tunisia', N'Ministère de l''Environnement et du Développement Durable', N'TN', N'N'),
(N'Turkey', N'Ministry of Environment and Urbanization', N'TR', N'N'),
(N'Turkmenistan', N'Ministry of Foreign Affairs', N'TM', N'N'),
(N'Uganda', N'National Environment Management Authority (NEMA)', N'UG', N'N'),
(N'Ukraine', N'Ministry of Ecology and Natural Resources', N'UA', N'N'),
(N'United Arab Emirates', N'Chemical and Hazardous Waste Department', N'AE', N'N'),
(N'United States', N'U.S. Environmental Protection Agency', N'US', N'N'),
(N'Uruguay', N'Ministerio de Vivienda, Ordenamiento Territorial y Medio Ambiente', N'UY', N'N'),
(N'Uzbekistan', N'State Committee for Nature Protection', N'UZ', N'N'),
(N'Venezuela', N'Ministerio del Poder Popular para el Ambiente', N'VE', N'N'),
(N'Vietnam', N'Ministry of Natural Resources and Environment (MONRE)', N'VN', N'N'),
(N'Yemen', N'Environment Protection Authority (EPA)', N'YE', N'N'),
(N'Zambia', N'Zambia Environmental Management Authority (ZEMA)', N'ZM', N'N'),
(N'Zimbabwe', N'Environmental Management Agency', N'ZW', N'N');

PRINT 'Delete all existing competent authorities except the UK CAs';

DELETE FROM [Lookup].[CompetentAuthority] 
WHERE [Id] NOT IN (SELECT [CompetentAuthorityId] FROM [Lookup].[UnitedKingdomCompetentAuthority]);

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId], [IsTransitAuthority])
SELECT		(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
			CA.[CompetentAuthority],
			CA.[Code],
			C.[Id],
			CASE
				WHEN CA.[IsTransit] = 'Y' THEN 1
				ELSE 0
			END
FROM		@CompetentAuthorities AS CA

INNER JOIN	[Lookup].[Country] AS C
ON			C.[Name] = CA.[Country];