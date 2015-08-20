﻿namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Core.NotificationAssessment;
    using NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;

    public class NotificationAssessment : Entity
    {
        private enum Trigger
        {
            Submit,
            NotificationReceived
        }

        private readonly StateMachine<NotificationStatus, Trigger> stateMachine;

        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> receivedTrigger;

        public Guid NotificationApplicationId { get; private set; }
        
        public NotificationStatus Status { get; private set; }

        protected virtual ICollection<NotificationStatusChange> StatusChangeCollection { get; set; }

        public IEnumerable<NotificationStatusChange> StatusChanges
        {
            get { return StatusChangeCollection.ToSafeIEnumerable(); }
        }

        public virtual NotificationDates Dates { get; set; }

        protected NotificationAssessment()
        {
            stateMachine = CreateStateMachine();
        }

        public NotificationAssessment(Guid notificationApplicationId)
        {
            NotificationApplicationId = notificationApplicationId;
            Status = NotificationStatus.NotSubmitted;
            StatusChangeCollection = new List<NotificationStatusChange>();
            stateMachine = CreateStateMachine();
            Dates = new NotificationDates();
        }

        private StateMachine<NotificationStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<NotificationStatus, Trigger>(() => Status, s => Status = s);

            receivedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.NotificationReceived);

            stateMachine.OnTransitioned(OnTransitionAction);

            stateMachine.Configure(NotificationStatus.NotSubmitted)
                .Permit(Trigger.Submit, NotificationStatus.Submitted);

            stateMachine.Configure(NotificationStatus.Submitted)
                .OnEntryFrom(Trigger.Submit, OnSubmit)
                .Permit(Trigger.NotificationReceived, NotificationStatus.NotificationReceived);

            stateMachine.Configure(NotificationStatus.NotificationReceived)
                .OnEntryFrom(receivedTrigger, OnReceived);

            return stateMachine;
        }

        private void OnSubmit()
        {
            RaiseEvent(new NotificationSubmittedEvent(NotificationApplicationId));
        }

        private void OnReceived(DateTime receivedDate)
        {
            Dates.NotificationReceivedDate = receivedDate;
        }

        private void OnTransitionAction(StateMachine<NotificationStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new NotificationStatusChangeEvent(this, transition.Destination));
        }

        public void Submit(INotificationProgressService progressService)
        {
            if (!progressService.IsComplete(NotificationApplicationId))
            {
                throw new InvalidOperationException(string.Format("Cannot submit an incomplete notification: {0}", NotificationApplicationId));
            }

            stateMachine.Fire(Trigger.Submit);
        }

        public void AddStatusChangeRecord(NotificationStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            StatusChangeCollection.Add(statusChange);
        }

        public void SetNotificationReceived(DateTime receivedDate)
        {
            stateMachine.Fire(receivedTrigger, receivedDate);
        }

        public void SetPaymentReceived(DateTime paymentDate)
        {
            if (!stateMachine.IsInState(NotificationStatus.NotificationReceived))
            {
                throw new InvalidOperationException(string.Format("Cannot set payment received on this notification {0} in status {1}", NotificationApplicationId, Status));
            }

            Dates.PaymentReceivedDate = paymentDate;
        }
    }
}
