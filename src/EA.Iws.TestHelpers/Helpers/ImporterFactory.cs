﻿namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain;
    using Domain.Notification;

    public class ImporterFactory
    {
        public static Importer Create(Guid id, Business business = null, Address address = null, string name = "AnyName")
        {
            var importer = ObjectInstantiator<Importer>.CreateNew();

            EntityHelper.SetEntityId(importer, id);

            if (business == null)
            {
                business = ObjectInstantiator<Business>.CreateNew();
                ObjectInstantiator<Business>.SetProperty(x => x.Name, name, business);
            }

            if (address == null)
            {
                address = ObjectInstantiator<Address>.CreateNew();
            }

            ObjectInstantiator<Importer>.SetProperty(x => x.Business, business, importer);
            ObjectInstantiator<Importer>.SetProperty(x => x.Address, address, importer);

            return importer;
        }
    }
}
