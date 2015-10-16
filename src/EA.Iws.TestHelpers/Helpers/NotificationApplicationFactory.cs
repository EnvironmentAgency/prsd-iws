﻿namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core.MeansOfTransport;
    using Core.Shared;
    using Core.WasteCodes;
    using Core.WasteType;
    using Domain;
    using Domain.NotificationApplication;
    using OI = ObjectInstantiator<Domain.NotificationApplication.NotificationApplication>;
    using PhysicalCharacteristicType = Domain.NotificationApplication.PhysicalCharacteristicType;

    public class NotificationApplicationFactory
    {
        public static readonly string ProducerBusinessName1 = "business1";
        public static readonly string ProducerBusinessName2 = "business2";
        public static readonly string CarrierBusinessName1 = "carrier1";
        public static readonly string CarrierBusinessName2 = "carrier2";
        public static readonly string FacilityBusinessName1 = "facility1";
        public static readonly string FacilityBusinessName2 = "facility2";
        public static readonly string OtherWasteCode = "baubles";

        public static NotificationApplication Create(Guid id, int number = 250)
        {
            var notificationApplication = new NotificationApplication(Guid.Empty, NotificationType.Recovery,
                UKCompetentAuthority.England, number);

            EntityHelper.SetEntityId(notificationApplication, id);

            return notificationApplication;
        }

        public static NotificationApplication CreateCompleted(Guid id,
            Guid userId,
            IList<Country> countries,
            IList<WasteCode> wasteCodes,
            int number = 250)
        {
            var notification = Create(id, number);

            OI.SetProperty(x => x.UserId, userId, notification);

            OI.SetProperty(x => x.Exporter, ExporterFactory.Create(Guid.NewGuid()), notification);
            OI.SetProperty(x => x.Importer, ImporterFactory.Create(Guid.NewGuid()), notification);

            notification.AddProducer(ComplexTypeFactory.Create<ProducerBusiness>(ProducerBusinessName1),
                ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());
            notification.AddProducer(ComplexTypeFactory.Create<ProducerBusiness>(ProducerBusinessName2),
                ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());
            notification.AddCarrier(ComplexTypeFactory.Create<Business>(CarrierBusinessName1),
                ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());
            notification.AddCarrier(ComplexTypeFactory.Create<Business>(CarrierBusinessName2),
                ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());
            notification.AddFacility(ComplexTypeFactory.Create<Business>(FacilityBusinessName1),
                ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());
            notification.AddFacility(ComplexTypeFactory.Create<Business>(FacilityBusinessName2),
                ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());

            notification.SetMeansOfTransport(new List<MeansOfTransport>
            {
                MeansOfTransport.Air,
                MeansOfTransport.Road,
                MeansOfTransport.Train
            });

            notification.SetPhysicalCharacteristics(new List<PhysicalCharacteristicsInfo>
            {
                PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType.Sludgy)
            });

            notification.SetWasteType(WasteType.CreateRdfWasteType(new[]
            {
                WasteComposition.CreateWasteComposition("boulder", 5, 10, ChemicalCompositionCategory.Other),
                WasteComposition.CreateWasteComposition("notes", 6, 9, ChemicalCompositionCategory.Paper)
            }));

            SetProperty("WasteAdditionalInformationCollection", new List<WasteAdditionalInformation>(),
                notification.WasteType);
            notification.SetWasteAdditionalInformation(new[]
            {
                WasteAdditionalInformation.CreateWasteAdditionalInformation("Rubik's cubes", 1, 10,
                    WasteInformationType.AshContent)
            });

            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails("cheddar", "cheese"));

            notification.SetOperationCodes(new[] { OperationCode.R1, OperationCode.R7 });
            notification.ReasonForExport = "recovery";
            notification.SetEwcCodes(new[]
            {
                WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.Ewc))
            });
            notification.SetHCodes(new[]
            {
                WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.H))
            });
            notification.SetYCodes(new[]
            {
                WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.Y))
            });
            notification.SetUnClasses(new[]
            {
                WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.Un))
            });
            notification.SetUnNumbers(new[]
            {
                WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.UnNumber))
            });
            notification.SetCustomsCode(
                WasteCodeInfo.CreateCustomWasteCodeInfo(CodeType.CustomsCode,
                    "olives"));
            notification.SetImportCode(
                WasteCodeInfo.CreateCustomWasteCodeInfo(CodeType.ImportCode,
                    "cardboard boxes"));
            notification.SetExportCode(
                WasteCodeInfo.CreateCustomWasteCodeInfo(CodeType.ExportCode,
                    "gravel"));
            notification.SetPreconsentedRecoveryFacility(false);

            return notification;
        }

        private static void SetProperty<T>(string name, object value, T target)
        {
            var prop = typeof(T).GetProperty(name,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            prop.SetValue(target, value, null);
        }
    }
}