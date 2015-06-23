﻿namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Core;
    using Core.CustomsOffice;
    using Domain.Notification;
    using TestHelpers.Helpers;
    using TransportRoute;
    using Xunit;

    public class NotificationCustomsOfficeTests
    {
        private readonly NotificationApplication notification;
        private readonly Country europeanCountry1;
        private readonly Country europeanCountry2;
        private readonly Country europeanCountry3;
        private readonly Country nonEuCountry1;
        private readonly Country nonEuCountry2;
        
        private readonly CompetentAuthority europeanCompetentAuthority1;
        private readonly CompetentAuthority europeanCompetentAuthority2;
        private readonly CompetentAuthority europeanCompetentAuthority3;
        private readonly CompetentAuthority nonEuCompetentAuthority1;
        private readonly CompetentAuthority nonEuCompetentAuthority2;
        
        private EntryOrExitPoint[] europeanEntryOrExitPoints1;
        private EntryOrExitPoint[] europeanEntryOrExitPoints2;
        private EntryOrExitPoint[] europeanEntryOrExitPoints3;
        private EntryOrExitPoint[] nonEuEntryOrExitPoints1;
        private EntryOrExitPoint[] nonEuEntryOrExitPoints2;

        private readonly ExitCustomsOffice exitCustomsOffice;
        private readonly EntryCustomsOffice entryCustomsOffice;

        public NotificationCustomsOfficeTests()
        {
            notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 520);

            europeanCountry1 = CreateCountry(new Guid("FB908C44-7AF1-4894-A0A5-860338468DFA"), true);
            europeanCountry2 = CreateCountry(new Guid("8057CD56-A12C-4C7D-BED4-B12D98DCADAB"), true);
            europeanCountry3 = CreateCountry(new Guid("BC0F57D1-6E22-4A15-A39E-1F34C91E49C6"), true);

            nonEuCountry1 = CreateCountry(new Guid("E9738924-B1EC-46E8-B45F-67D990E29D67"), false);
            nonEuCountry2 = CreateCountry(new Guid("04113907-94AF-41DB-A239-10803E067F68"), false);

            europeanCompetentAuthority1 = CreateCompetentAuthority(new Guid("532051C7-145E-4BA5-B950-CFC199B358A7"), europeanCountry1);
            europeanCompetentAuthority2 = CreateCompetentAuthority(new Guid("D274D346-E0F3-48EF-A54E-872319E37A97"), europeanCountry2);
            europeanCompetentAuthority3 = CreateCompetentAuthority(new Guid("02D11536-8564-4BAF-87A6-36CBAE7F6163"),
                europeanCountry3);

            nonEuCompetentAuthority1 = CreateCompetentAuthority(new Guid("2842CAAE-B34D-4311-A1DE-286EEF492AD6"), nonEuCountry1);
            nonEuCompetentAuthority2 = CreateCompetentAuthority(new Guid("F84CF9EF-A98A-4B8F-B4EC-C3185833226B"), nonEuCountry2);

            exitCustomsOffice = new ExitCustomsOffice("test", "test", europeanCountry1);
            entryCustomsOffice = new EntryCustomsOffice("test", "test", europeanCountry1);

            SetTransitStates();
        }

        [Fact]
        public void CustomsOfficesRequired_TransitStatesEmpty_TransitStatesNotSet()
        {
            var result = notification.GetCustomsOfficesRequired();

            Assert.Equal(CustomsOffices.TransitStatesNotSet, result);
        }

        [Fact]
        public void CustomsOfficesRequired_TransitStatesInEU_None()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1, europeanEntryOrExitPoints1[0]);
            var stateOfImport = new StateOfImport(europeanCountry2, europeanCompetentAuthority2, europeanEntryOrExitPoints2[0]);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);

            var result = notification.GetCustomsOfficesRequired();

            Assert.Equal(CustomsOffices.None, result);
        }

        [Fact]
        public void CustomsOfficesRequired_StateOfImportOutsideEU_Exit()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);
            var stateOfImport = new StateOfImport(nonEuCountry1, nonEuCompetentAuthority1, nonEuEntryOrExitPoints1[0]);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);

            var result = notification.GetCustomsOfficesRequired();

            Assert.Equal(CustomsOffices.Exit, result);
        }

        [Fact]
        public void CustomsOfficesRequired_StateOfTransitOutsideEUImportInsideEU_EntryAndExit()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);
            var stateOfImport = new StateOfImport(europeanCountry2, europeanCompetentAuthority2,
                europeanEntryOrExitPoints2[0]);

            var transitState = new TransitState(nonEuCountry1, nonEuCompetentAuthority1, nonEuEntryOrExitPoints1[0],
                nonEuEntryOrExitPoints1[1], 1);

            notification.AddTransitStateToNotification(transitState);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);

            var result = notification.GetCustomsOfficesRequired();

            Assert.Equal(CustomsOffices.EntryAndExit, result);
        }

        [Fact]
        public void CustomsOfficesRequired_StateOfTransitMixedImportInsideEU_EntryAndExit()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);
            var stateOfImport = new StateOfImport(europeanCountry2, europeanCompetentAuthority2,
                europeanEntryOrExitPoints2[0]);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);

            var europeanTransitState = new TransitState(europeanCountry3, europeanCompetentAuthority3,
                europeanEntryOrExitPoints3[0],
                europeanEntryOrExitPoints3[1], 1);
            var nonEuropeantransitState = new TransitState(nonEuCountry1, nonEuCompetentAuthority1, nonEuEntryOrExitPoints1[0],
                nonEuEntryOrExitPoints1[1], 2);

            notification.AddTransitStateToNotification(europeanTransitState);
            notification.AddTransitStateToNotification(nonEuropeantransitState);

            var result = notification.GetCustomsOfficesRequired();

            Assert.Equal(CustomsOffices.EntryAndExit, result);
        }

        [Fact]
        public void CustomsOfficesRequired_StateOfTransitInsideEUImportOutside_Exit()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);
            var stateOfImport = new StateOfImport(nonEuCountry2, nonEuCompetentAuthority2,
                nonEuEntryOrExitPoints2[0]);

            var transitState = new TransitState(europeanCountry2, europeanCompetentAuthority2, europeanEntryOrExitPoints2[0],
                europeanEntryOrExitPoints2[1], 1);

            notification.AddTransitStateToNotification(transitState);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);

            var result = notification.GetCustomsOfficesRequired();

            Assert.Equal(CustomsOffices.Exit, result);
        }

        [Fact]
        public void CustomsOfficesRequired_ImportAndExportInsideEU_None()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);
            var stateOfImport = new StateOfImport(europeanCountry2, europeanCompetentAuthority2,
                europeanEntryOrExitPoints2[0]);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);

            var result = notification.GetCustomsOfficesRequired();

            Assert.Equal(CustomsOffices.None, result);
        }

        [Fact]
        public void SetExitCustomsOffice_NotRequired_Throws()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);

            var stateOfImport = new StateOfImport(europeanCountry2, europeanCompetentAuthority2,
                europeanEntryOrExitPoints2[0]);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);

            Assert.Throws<InvalidOperationException>(
                () =>
                    notification.SetExitCustomsOffice(new ExitCustomsOffice("test", "test", europeanCountry1)));
        }

        [Fact]
        public void SetExitCustomsOffice_Required_Sets()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);

            var stateOfImport = new StateOfImport(nonEuCountry1, nonEuCompetentAuthority1,
                nonEuEntryOrExitPoints1[0]);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);

            notification.SetExitCustomsOffice(new ExitCustomsOffice("test", "test", europeanCountry1));

            Assert.Equal("test", notification.ExitCustomsOffice.Name);
            Assert.Equal("test", notification.ExitCustomsOffice.Address);
            Assert.Equal(europeanCountry1.Id, notification.ExitCustomsOffice.Country.Id);
        }

        [Fact]
        public void SetExitCustomsOffice_Required_OverwritesPrevious()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);

            var stateOfImport = new StateOfImport(nonEuCountry1, nonEuCompetentAuthority1,
                nonEuEntryOrExitPoints1[0]);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);

            notification.SetExitCustomsOffice(new ExitCustomsOffice("test", "test", europeanCountry1));

            notification.SetExitCustomsOffice(new ExitCustomsOffice("test2", "test2", europeanCountry2));

            Assert.Equal("test2", notification.ExitCustomsOffice.Name);
            Assert.Equal("test2", notification.ExitCustomsOffice.Address);
            Assert.Equal(europeanCountry2.Id, notification.ExitCustomsOffice.Country.Id);
        }

        [Fact]
        public void SetEntryCustomsOffice_NotRequired_Throws()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);

            var stateOfImport = new StateOfImport(europeanCountry2, europeanCompetentAuthority2,
                europeanEntryOrExitPoints2[0]);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);

            Assert.Throws<InvalidOperationException>(
                () =>
                    notification.SetEntryCustomsOffice(new EntryCustomsOffice("test", "test", europeanCountry1)));
        }

        [Fact]
        public void SetEntryCustomsOffice_Required_Sets()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);

            var transitState = new TransitState(nonEuCountry1, nonEuCompetentAuthority1, nonEuEntryOrExitPoints1[0],
                nonEuEntryOrExitPoints1[1], 1);

            var stateOfImport = new StateOfImport(europeanCountry2, europeanCompetentAuthority2,
                europeanEntryOrExitPoints2[0]);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);
            notification.AddTransitStateToNotification(transitState);

            notification.SetEntryCustomsOffice(new EntryCustomsOffice("test", "test", europeanCountry1));

            Assert.Equal("test", notification.EntryCustomsOffice.Name);
            Assert.Equal("test", notification.EntryCustomsOffice.Address);
            Assert.Equal(europeanCountry1.Id, notification.EntryCustomsOffice.Country.Id);
        }

        [Fact]
        public void SetEntryCustomsOffice_Required_OverwritesPrevious()
        {
            var stateOfExport = new StateOfExport(europeanCountry1, europeanCompetentAuthority1,
                europeanEntryOrExitPoints1[0]);

            var transitState = new TransitState(nonEuCountry1, nonEuCompetentAuthority1, nonEuEntryOrExitPoints1[0],
                nonEuEntryOrExitPoints1[1], 1);

            var stateOfImport = new StateOfImport(europeanCountry2, europeanCompetentAuthority2,
                europeanEntryOrExitPoints2[0]);

            SetNotificationStateOfExport(stateOfExport);
            SetNotificationStateOfImport(stateOfImport);
            notification.AddTransitStateToNotification(transitState);

            notification.SetEntryCustomsOffice(new EntryCustomsOffice("test", "test", europeanCountry1));

            notification.SetEntryCustomsOffice(new EntryCustomsOffice("test2", "test2", europeanCountry2));

            Assert.Equal("test2", notification.EntryCustomsOffice.Name);
            Assert.Equal("test2", notification.EntryCustomsOffice.Address);
            Assert.Equal(europeanCountry2.Id, notification.EntryCustomsOffice.Country.Id);
        }

        [Fact]
        public void GetCustomsOfficesCompleted_NeitherCompleted_ReturnsNone()
        {
            var result = notification.GetCustomsOfficesCompleted();

            Assert.Equal(CustomsOffices.None, result);
        }

        [Fact]
        public void GetCustomsOfficesCompleted_ExitCompleted_ReturnsExit()
        {
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.ExitCustomsOffice, exitCustomsOffice, notification);
            var result = notification.GetCustomsOfficesCompleted();

            Assert.Equal(CustomsOffices.Exit, result);
        }

        [Fact]
        public void GetCustomsOfficesCompleted_BothCompleted_ReturnsEntryAndExit()
        {
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.ExitCustomsOffice, exitCustomsOffice, notification);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.EntryCustomsOffice, entryCustomsOffice, notification);
            var result = notification.GetCustomsOfficesCompleted();

            Assert.Equal(CustomsOffices.EntryAndExit, result);
        }

        [Fact]
        public void GetCustomsOfficesCompleted_EntryCompleted_ReturnsEntry()
        {
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.EntryCustomsOffice, entryCustomsOffice, notification);
            var result = notification.GetCustomsOfficesCompleted();

            Assert.Equal(CustomsOffices.Entry, result);
        }

        private void SetNotificationStateOfExport(StateOfExport stateOfExport)
        {
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.StateOfExport, stateOfExport, notification);
        }

        private void SetNotificationStateOfImport(StateOfImport stateOfImport)
        {
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.StateOfImport, stateOfImport, notification);
        }

        private Country CreateCountry(Guid id, bool isEuMember)
        {
            return CountryFactory.Create(id, "test", isEuMember);
        }

        private CompetentAuthority CreateCompetentAuthority(Guid id, Country country)
        {
            return CompetentAuthorityFactory.Create(id, country);
        }

        private EntryOrExitPoint CreateEntryOrExitPoint(Guid id, Country country)
        {
            return EntryOrExitPointFactory.Create(id, country);
        }

        private void SetTransitStates()
        {
            europeanEntryOrExitPoints1 = new[]
            {
                CreateEntryOrExitPoint(new Guid("97F580C5-CB72-4036-A974-C9E775AF2F80"), europeanCountry1),
                CreateEntryOrExitPoint(new Guid("4ABBAD17-B36A-4D90-958B-54D80BB49CCC"), europeanCountry1)
            };

            europeanEntryOrExitPoints2 = new[]
            {
                CreateEntryOrExitPoint(new Guid("3E9BD815-0B7A-4216-BC4F-602CB399E287"), europeanCountry2),
                CreateEntryOrExitPoint(new Guid("81DB2211-8F3E-4935-8363-D9AE510EAADF"), europeanCountry2)
            };

            europeanEntryOrExitPoints3 = new[]
            {
                CreateEntryOrExitPoint(new Guid("CAD647EB-EE40-4FAC-A978-89AC3FEC8F8D"), europeanCountry3),
                CreateEntryOrExitPoint(new Guid("F1E176A5-E365-4FF0-BE05-2081967B8818"), europeanCountry3)
            };

            nonEuEntryOrExitPoints1 = new[]
            {
                CreateEntryOrExitPoint(new Guid("22AF2B53-46EA-451E-A579-741D4A848102"), nonEuCountry1),
                CreateEntryOrExitPoint(new Guid("FBF10A8B-E191-459F-97F2-47FD9EAAFEC5"), nonEuCountry1)
            };

            nonEuEntryOrExitPoints2 = new[]
            {
                CreateEntryOrExitPoint(new Guid("A8C827F4-96B0-473B-81DB-47300A5D2719"), nonEuCountry2),
                CreateEntryOrExitPoint(new Guid("C5E54EB6-B1DC-4692-BB0C-07F018F6DF5D"), nonEuCountry2)
            };
        }
    }
}
