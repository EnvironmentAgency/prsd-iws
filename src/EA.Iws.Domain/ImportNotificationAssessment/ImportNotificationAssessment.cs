﻿namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Core.ImportNotificationAssessment;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;

    public class ImportNotificationAssessment : Entity
    {
        private enum Trigger
        {
            Receive = 1
        }

        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTimeOffset> receivedTrigger;

        private readonly StateMachine<ImportNotificationStatus, Trigger> stateMachine;

        public Guid NotificationApplicationId { get; private set; }

        public ImportNotificationStatus Status { get; private set; }

        public virtual ImportNotificationDates Dates { get; private set; }

        protected virtual ICollection<ImportNotificationStatusChange> StatusChangeCollection { get; private set; }

        public IEnumerable<ImportNotificationStatusChange> StatusChanges
        {
            get { return StatusChangeCollection.ToSafeIEnumerable(); }
        }

        protected ImportNotificationAssessment()
        {
            stateMachine = CreateStateMachine();
        }

        public ImportNotificationAssessment(Guid notificationId)
        {
            NotificationApplicationId = notificationId;
            Dates = new ImportNotificationDates();
            Status = ImportNotificationStatus.New;
            stateMachine = CreateStateMachine();
        }

        private StateMachine<ImportNotificationStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<ImportNotificationStatus, Trigger>(() => Status, s => Status = s);

            stateMachine.OnTransitioned(OnTransition);

            receivedTrigger = stateMachine.SetTriggerParameters<DateTimeOffset>(Trigger.Receive);

            stateMachine.Configure(ImportNotificationStatus.New)
                .Permit(Trigger.Receive, ImportNotificationStatus.NotificationReceived);

            stateMachine.Configure(ImportNotificationStatus.NotificationReceived)
                .OnEntryFrom(receivedTrigger, OnReceived);

            return stateMachine;
        }

        private void OnTransition(StateMachine<ImportNotificationStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new ImportNotificationStatusChangeEvent(this, transition.Source, transition.Destination));
        }

        public void AddStatusChangeRecord(ImportNotificationStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            StatusChangeCollection.Add(statusChange);
        }

        public void Receive(DateTimeOffset receivedDate)
        {
            stateMachine.Fire(receivedTrigger, receivedDate);
        }

        private void OnReceived(DateTimeOffset receivedDate)
        {
            Dates.NotificationReceivedDate = receivedDate;
        }
    }
}
