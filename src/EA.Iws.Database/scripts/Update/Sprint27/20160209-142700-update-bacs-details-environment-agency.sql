UPDATE	[Lookup].[UnitedKingdomCompetentAuthority]

SET		[BacsAccountName] = 'EA Receipts',
		[BacsBank] = 'Royal Bank of Scotland',
		[BacsBankAddress] = 'Royal Bank of Scotland plc,London Corporate Service Centre, CPB Services 2nd Floor, 280 Bishopsgate, London, EC2M 4RB',
		[BacsAccountNumber] = '10014411',
		[BacsSortCode] = '60-70-80',
		[BacsIban] = 'GB23NWBK60708010014411',
		[BacsSwiftBic] = 'NWBKGB2L',
		[BacsEmail] = 'ea_fsc_ar@sscl.gse.gov.uk'

WHERE	UnitedKingdomCountry = 'England';
