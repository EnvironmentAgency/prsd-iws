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
            NotificationReceived,
            AssessmentCommenced,
            NotificationComplete,
            Transmit,
            Acknowledged,
            DecisionRequiredBySet
        }

        private readonly StateMachine<NotificationStatus, Trigger> stateMachine;

        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> receivedTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime, string> commencedTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> completeTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> transmitTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> acknowledgedTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> decisionRequiredByTrigger;

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
            commencedTrigger = stateMachine.SetTriggerParameters<DateTime, string>(Trigger.AssessmentCommenced);
            completeTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.NotificationComplete);
            transmitTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Transmit);
            acknowledgedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Acknowledged);
            decisionRequiredByTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.DecisionRequiredBySet);

            stateMachine.OnTransitioned(OnTransitionAction);

            stateMachine.Configure(NotificationStatus.NotSubmitted)
                .Permit(Trigger.Submit, NotificationStatus.Submitted);

            stateMachine.Configure(NotificationStatus.Submitted)
                .OnEntryFrom(Trigger.Submit, OnSubmit)
                .Permit(Trigger.NotificationReceived, NotificationStatus.NotificationReceived);

            stateMachine.Configure(NotificationStatus.NotificationReceived)
                .OnEntryFrom(receivedTrigger, OnReceived)
                .PermitIf(Trigger.AssessmentCommenced, NotificationStatus.InAssessment, () => Dates.PaymentReceivedDate.HasValue);

            stateMachine.Configure(NotificationStatus.InAssessment)
                .OnEntryFrom(commencedTrigger, OnInAssessment)
                .Permit(Trigger.NotificationComplete, NotificationStatus.ReadyToTransmit);

            stateMachine.Configure(NotificationStatus.ReadyToTransmit)
                .OnEntryFrom(completeTrigger, OnCompleted)
                .Permit(Trigger.Transmit, NotificationStatus.Transmitted);

            stateMachine.Configure(NotificationStatus.Transmitted)
                .OnEntryFrom(transmitTrigger, OnTransmitted)
                .Permit(Trigger.Acknowledged, NotificationStatus.Acknowledged);

            stateMachine.Configure(NotificationStatus.Acknowledged)
                .OnEntryFrom(acknowledgedTrigger, OnAcknowledged)
                .Permit(Trigger.DecisionRequiredBySet, NotificationStatus.DecisionRequiredBy);

            stateMachine.Configure(NotificationStatus.DecisionRequiredBy)
                .OnEntryFrom(decisionRequiredByTrigger, OnDecisionRequiredBy);

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

        private void OnInAssessment(DateTime commencementDate, string nameOfOfficer)
        {
            Dates.CommencementDate = commencementDate;
            Dates.NameOfOfficer = nameOfOfficer;
        }

        private void OnCompleted(DateTime completedDate)
        {
            Dates.CompleteDate = completedDate;
        }

        private void OnTransmitted(DateTime transmittedDate)
        {
            Dates.TransmittedDate = transmittedDate;
        }

        private void OnTransitionAction(StateMachine<NotificationStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new NotificationStatusChangeEvent(this, transition.Destination));
        }

        private void OnAcknowledged(DateTime acknowledgedDate)
        {
            Dates.AcknowledgedDate = acknowledgedDate;
        }

        private void OnDecisionRequiredBy(DateTime decisionByDate)
        {
            Dates.DecisionDate = decisionByDate;
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

        public void NotificationReceived(DateTime receivedDate)
        {
            stateMachine.Fire(receivedTrigger, receivedDate);
        }

        public void PaymentReceived(DateTime paymentDate)
        {
            if (!stateMachine.IsInState(NotificationStatus.NotificationReceived))
            {
                throw new InvalidOperationException(string.Format("Cannot set payment received on this notification {0} in status {1}", NotificationApplicationId, Status));
            }

            Dates.PaymentReceivedDate = paymentDate;
        }

        public void Commenced(DateTime commencementDate, string nameOfOfficer)
        {
            Guard.ArgumentNotNullOrEmpty(() => nameOfOfficer, nameOfOfficer);
            stateMachine.Fire(commencedTrigger, commencementDate, nameOfOfficer);
        }

        public void Complete(DateTime completedDate)
        {
            stateMachine.Fire(completeTrigger, completedDate);
        }

        public void Transmit(DateTime transmittedDate)
        {
            stateMachine.Fire(transmitTrigger, transmittedDate);
        }

        public void Acknowledge(DateTime acknowledgedDate)
        {
            stateMachine.Fire(acknowledgedTrigger, acknowledgedDate);
        }

        public void DecisionRequiredBy(DateTime decisionRequiredByDate)
        {
            stateMachine.Fire(decisionRequiredByTrigger, decisionRequiredByDate);
        }
    }
}