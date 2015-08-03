﻿namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using Core.FinancialGuarantee;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;

    public class FinancialGuarantee : Entity
    {
        private const int WorkingDaysUntilDecisionRequired = 20;
        
        public Guid NotificationApplicationId { get; private set; }

        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime> receivedTrigger;
        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime> completedTrigger;
        private readonly StateMachine<FinancialGuaranteeStatus, Trigger> stateMachine;

        protected FinancialGuarantee()
        {
            stateMachine = CreateStateMachine();
        }

        protected FinancialGuarantee(Guid notificationApplicationId)
        {
            CreatedDate = SystemTime.UtcNow;
            NotificationApplicationId = notificationApplicationId;
            StatusChangeCollection = new List<FinancialGuaranteeStatusChange>();
            stateMachine = CreateStateMachine();
            Status = FinancialGuaranteeStatus.AwaitingApplication;
        }

        public DateTime? ReceivedDate { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public DateTime? GetDecisionRequiredDate(IWorkingDayCalculator workingDayCalculator, UKCompetentAuthority competentAuthority)
        {
            DateTime? returnDate = null;

            if (CompletedDate.HasValue)
            {
                returnDate = workingDayCalculator.AddWorkingDays(CompletedDate.Value, WorkingDaysUntilDecisionRequired, false);
            }

            return returnDate;
        }

        public DateTime CreatedDate { get; private set; }

        public FinancialGuaranteeStatus Status { get; private set; }

        protected virtual ICollection<FinancialGuaranteeStatusChange> StatusChangeCollection { get; set; }

        public IEnumerable<FinancialGuaranteeStatusChange> StatusChanges
        {
            get { return StatusChangeCollection.ToSafeIEnumerable(); }
        }

        public void Received(DateTime date)
        {
            stateMachine.Fire(receivedTrigger, date);
        }

        public void Completed(DateTime date)
        {
            stateMachine.Fire(completedTrigger, date);
        }

        public void AddStatusChangeRecord(FinancialGuaranteeStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            StatusChangeCollection.Add(statusChange);
        }

        public static FinancialGuarantee Create(Guid notificationApplicationId)
        {
            var financialGuarantee = new FinancialGuarantee(notificationApplicationId);

            return financialGuarantee;
        }

        private void OnReceived(DateTime date)
        {
            ReceivedDate = date;
        }

        private void OnCompleted(DateTime date)
        {
            if (date < ReceivedDate.Value)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot set FG completed date before received date for financial guarantee {0}.", Id));
            }

            CompletedDate = date;
        }

        private StateMachine<FinancialGuaranteeStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<FinancialGuaranteeStatus, Trigger>(() => Status, s => Status = s);

            receivedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Received);
            completedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Completed);

            stateMachine.Configure(FinancialGuaranteeStatus.AwaitingApplication)
                .Permit(Trigger.Received, FinancialGuaranteeStatus.ApplicationReceived);

            stateMachine.Configure(FinancialGuaranteeStatus.ApplicationReceived)
                .OnEntryFrom(receivedTrigger, OnReceived)
                .Permit(Trigger.Completed, FinancialGuaranteeStatus.ApplicationComplete);

            stateMachine.Configure(FinancialGuaranteeStatus.ApplicationComplete)
                .OnEntryFrom(completedTrigger, OnCompleted);

            stateMachine.OnTransitioned(OnTransitionAction);

            return stateMachine;
        }

        private void OnTransitionAction(StateMachine<FinancialGuaranteeStatus, Trigger>.Transition transition)
        {
            DomainEvents.Raise(new FinancialGuaranteeStatusChangeEvent(this, transition.Destination));
        }

        private enum Trigger
        {
            Received,
            Completed
        }

        public void UpdateReceivedDate(DateTime value)
        {
            if (ReceivedDate.HasValue && (!CompletedDate.HasValue || value <= CompletedDate))
            {
                ReceivedDate = value;
            }
            else
            {
                throw new InvalidOperationException("Received date must have value and value to set must be less than received date.");
            }
        }

        public void UpdateCompletedDate(DateTime value)
        {
            if (CompletedDate.HasValue && ReceivedDate.HasValue && value >= ReceivedDate)
            {
                CompletedDate = value;
            }
            else
            {
                throw new InvalidOperationException("Completed date must have value and value to set must be greater than received date.");
            }
        }
    }
}