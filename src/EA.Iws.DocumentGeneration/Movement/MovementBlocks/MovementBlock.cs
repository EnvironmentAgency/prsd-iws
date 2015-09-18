﻿namespace EA.Iws.DocumentGeneration.Movement.MovementBlocks
{
    using System.Collections.Generic;
    using Domain.Movement;
    using Formatters;
    using Mapper;
    using ViewModels;

    public class MovementBlock : IDocumentBlock
    {
        private readonly MovementViewModel data;

        public MovementBlock(IList<MergeField> mergeFields, Movement movement)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new MovementViewModel(movement,
                new DateTimeFormatter(),
                new QuantityFormatter(),
                new PhysicalCharacteristicsFormatter());
        }

        public string TypeName
        {
            get { return "Movement"; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; set; }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(MovementViewModel));

            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        public int OrdinalPosition
        {
            get { return 0; }
        }
    }
}