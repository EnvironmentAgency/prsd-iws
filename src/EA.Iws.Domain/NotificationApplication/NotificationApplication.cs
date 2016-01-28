﻿namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.MeansOfTransport;
    using Core.Shared;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using WasteRecovery;

    public partial class NotificationApplication : Entity
    {
        private const string NotificationNumberFormat = "GB 000{0} {1}";

        protected NotificationApplication()
        {
        }

        public NotificationApplication(Guid userId, NotificationType notificationType,
            UKCompetentAuthority competentAuthority, int notificationNumber)
        {
            UserId = userId;
            NotificationType = notificationType;
            CompetentAuthority = competentAuthority;
            NotificationNumber = CreateNotificationNumber(notificationNumber);

            OperationInfosCollection = new List<OperationInfo>();
            PackagingInfosCollection = new List<PackagingInfo>();
            PhysicalCharacteristicsCollection = new List<PhysicalCharacteristicsInfo>();
            WasteCodeInfoCollection = new List<WasteCodeInfo>();

            RaiseEvent(new NotificationCreatedEvent(this));
        }
        
        protected virtual ICollection<OperationInfo> OperationInfosCollection { get; set; }

        protected virtual ICollection<PackagingInfo> PackagingInfosCollection { get; set; }

        protected virtual ICollection<PhysicalCharacteristicsInfo> PhysicalCharacteristicsCollection { get; set; }

        protected virtual ICollection<WasteCodeInfo> WasteCodeInfoCollection { get; set; }

        public Guid UserId { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }

        public string NotificationNumber { get; private set; }

        public virtual WasteType WasteType { get; private set; }

        public virtual TechnologyEmployed TechnologyEmployed { get; private set; }

        public bool? WasteRecoveryInformationProvidedByImporter { get; private set; }

        public decimal? Charge { get; private set; }

        public void SetWasteRecoveryInformationProvider(ProvidedBy providedBy)
        {
            this.WasteRecoveryInformationProvidedByImporter = providedBy == ProvidedBy.Importer;

            RaiseEvent(new ProviderChangedEvent(this.Id, providedBy));
        }

        public void SetCharge(decimal charge)
        {
            Charge = charge;
        }

        public void ChangeUser(Guid newUserId)
        {
            var currentUser = UserId;
            UserId = newUserId;

            RaiseEvent(new NotificationUserChangedEvent(Id, currentUser, newUserId));
        }

        protected string MeansOfTransportInternal { get; set; }

        public IOrderedEnumerable<MeansOfTransport> MeansOfTransport
        {
            get
            {
                if (string.IsNullOrWhiteSpace(MeansOfTransportInternal))
                {
                    return new MeansOfTransport[] { }.OrderBy(m => m);
                }

                // OrderBy with a key of 0 returns the elements in their original order.
                return MeansOfTransportInternal
                    .Split(new[] { Core.MeansOfTransport.MeansOfTransport.Separator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(Core.MeansOfTransport.MeansOfTransport.GetFromToken).OrderBy(transport => 0);
            }
        }

        public DateTimeOffset CreatedDate { get; private set; }
                
        public IEnumerable<OperationInfo> OperationInfos
        {
            get { return OperationInfosCollection.ToSafeIEnumerable(); }
        }

        public IEnumerable<PackagingInfo> PackagingInfos
        {
            get { return PackagingInfosCollection.ToSafeIEnumerable(); }
        }

        public IEnumerable<PhysicalCharacteristicsInfo> PhysicalCharacteristics
        {
            get { return PhysicalCharacteristicsCollection.ToSafeIEnumerable(); }
        }

        public IEnumerable<WasteCodeInfo> WasteCodes
        {
            get { return WasteCodeInfoCollection.ToSafeIEnumerable(); }
        }

        private string CreateNotificationNumber(int notificationNumber)
        {
            return string.Format(NotificationNumberFormat, CompetentAuthority.Value, notificationNumber.ToString("D6"));
        }

        private string reasonForExport;
        public string ReasonForExport
        {
            get
            {
                return reasonForExport;
            }
            set
            {
                if (value != null && value.Length > 70)
                {
                    throw new InvalidOperationException(string.Format("Reason for export cannot be greater than 70 characters for notification {0}", Id));
                }
                reasonForExport = value;
            }
        }
    }
}