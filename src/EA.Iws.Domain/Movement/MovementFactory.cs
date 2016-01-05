﻿namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using NotificationAssessment;

    public class MovementFactory
    {
        private readonly MovementNumberGenerator numberGenerator;
        private readonly NumberOfMovements numberOfMovements;
        private readonly NotificationMovementsQuantity movementsQuantity;
        private readonly INotificationAssessmentRepository assessmentRepository;

        public MovementFactory(NumberOfMovements numberOfMovements,
            NotificationMovementsQuantity movementsQuantity,
            INotificationAssessmentRepository assessmentRepository,
            MovementNumberGenerator numberGenerator)
        {
            this.numberOfMovements = numberOfMovements;
            this.movementsQuantity = movementsQuantity;
            this.assessmentRepository = assessmentRepository;
            this.numberGenerator = numberGenerator;
        }

        public async Task<Movement> Create(Guid notificationId, DateTime actualMovementDate)
        {
            var hasMaximumMovements = await numberOfMovements.HasMaximum(notificationId);

            if (hasMaximumMovements)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create new movement for notification {0} which has used all available movements",
                        notificationId));
            }

            var quantityRemaining = await movementsQuantity.Remaining(notificationId);

            if (quantityRemaining <= 0)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create new movement for notification {0} as the quantity has been reached or exceeded.", 
                        notificationId));
            }

            var notificationStatus = (await assessmentRepository.GetByNotificationId(notificationId)).Status;

            if (notificationStatus != NotificationStatus.Consented)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create a movement for notification {0} because it's status is {1}",
                        notificationId, notificationStatus));
            }

            var newNumber = await numberGenerator.Generate(notificationId);

            return new Movement(newNumber, notificationId, actualMovementDate);
        }
    }
}