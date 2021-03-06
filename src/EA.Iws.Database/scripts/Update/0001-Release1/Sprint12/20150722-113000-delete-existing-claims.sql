PRINT 'Deleting existing claims';
GO

DELETE
FROM [Identity].[AspNetUserClaims]
WHERE ClaimType IN 
(
'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', 
'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name',
'organisation_id',
'internal_user_status'
);