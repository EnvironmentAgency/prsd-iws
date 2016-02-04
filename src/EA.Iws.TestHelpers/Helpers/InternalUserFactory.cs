﻿namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public static class InternalUserFactory
    {
        public static InternalUser Create(Guid id, User user)
        {
            var internalUser = ObjectInstantiator<InternalUser>.CreateNew();

            ObjectInstantiator<InternalUser>.SetProperty(x => x.Id, id, internalUser);
            ObjectInstantiator<InternalUser>.SetProperty(x => x.User, user, internalUser);
            ObjectInstantiator<InternalUser>.SetProperty(x => x.UserId, user.Id, internalUser);

            ObjectInstantiator<InternalUser>.SetProperty(x => x.CompetentAuthority, CompetentAuthorityEnum.England, internalUser);

            return internalUser;
        }
    }
}