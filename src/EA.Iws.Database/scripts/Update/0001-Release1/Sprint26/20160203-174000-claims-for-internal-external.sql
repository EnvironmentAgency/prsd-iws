INSERT INTO [Identity].[AspNetUserClaims]
([UserId]
,[ClaimType]
,[ClaimValue])
SELECT 
	u.Id as UserId,
	'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' as ClaimType,
	'internal' as Value
FROM [Identity].[AspNetUsers] u
INNER JOIN [Person].[InternalUser] p on u.Id = p.UserId

GO 

INSERT INTO [Identity].[AspNetUserClaims]
([UserId]
,[ClaimType]
,[ClaimValue])
SELECT 
	u.Id as UserId,
	'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' as ClaimType,
	'external' as Value
FROM [Identity].[AspNetUsers] u
LEFT JOIN [Person].[InternalUser] p on u.Id = p.UserId
WHERE p.Id IS NULL

GO