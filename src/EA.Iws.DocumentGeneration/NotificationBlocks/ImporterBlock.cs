﻿namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;
    using Domain.Notification;
    using Mapper;
    using ViewModels;

    internal class ImporterBlock : INotificationBlock
    {
        private readonly ImporterViewModel data;

        public ImporterBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, "Importer");

            data = new ImporterViewModel(notification.Importer);
        }

        public string TypeName
        {
            get { return "Importer"; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(ImporterViewModel));

            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        public int OrdinalPosition
        {
            get { return 2; }
        }
    }
}