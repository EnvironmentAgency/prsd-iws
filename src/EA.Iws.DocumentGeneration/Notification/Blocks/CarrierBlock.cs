﻿namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Domain.NotificationApplication;
    using Formatters;
    using Mapper;
    using ViewModels;

    internal class CarrierBlock : AnnexBlockBase, IDocumentBlock, IAnnexedBlock
    {
        private readonly IList<CarrierViewModel> data;

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public CarrierBlock(IList<MergeField> mergeFields, MeansOfTransport meansOfTransport, CarrierCollection carrierCollection)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);

            data = CarrierViewModel.CreateCarrierViewModelsForNotification(meansOfTransport, carrierCollection, new MeansOfTransportFormatter());

            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
        }

        public bool HasAnnex 
        {
            get { return data.Count > 1; }
        }

        public void GenerateAnnex(int annexNumber)
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(CarrierViewModel));

            MergeCarriersToMainDocument(CarrierViewModel.GetCarrierViewModelShowingSeeAnnexInstruction(annexNumber, data[0].MeansOfTransport), properties);

            MergeMultipleCarriersTable(properties);
            MergeAnnexNumber(annexNumber);

            TocText = "Annex " + annexNumber + " - Intended carriers";
        }

        private void MergeCarriersToMainDocument(CarrierViewModel carrier, PropertyInfo[] properties)
        {
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, carrier, properties);
            }
        }

        private void MergeMultipleCarriersTable(PropertyInfo[] properties)
        {
            var mergeTableRows = new TableRow[data.Count];

            // Find both the first row in the multiple carriers table and the table itself.
            var firstMergeFieldInTable = FindFirstMergeFieldInAnnexTable();
            var table = FindMultipleCarriersTable(firstMergeFieldInTable);

            // Get the table row containing the merge fields.
            mergeTableRows[0] = firstMergeFieldInTable.Run.Ancestors<TableRow>().First();

            // Create a row containing merge fields for each of the Carriers.
            for (var i = 1; i < data.Count; i++)
            {
                mergeTableRows[i] = (TableRow)mergeTableRows[0].CloneNode(true);
                table.AppendChild(mergeTableRows[i]);
            }

            // Merge the carriers into the table rows.
            for (var i = 0; i < mergeTableRows.Length; i++)
            {
                foreach (var field in MergeFieldLocator.GetMergeRuns(mergeTableRows[i]))
                {
                    MergeFieldDataMapper.BindCorrespondingField(
                        MergeFieldLocator.ConvertAnnexMergeFieldToRegularMergeField(field), data[i], properties);
                }
            }
        }

        public string TypeName
        {
            get { return "Carrier"; }
        }

        public int OrdinalPosition
        {
            get { return 8; }
        }

        public void Merge()
        {
            var carriers = PropertyHelper.GetPropertiesForViewModel(typeof(CarrierViewModel));

            if (!HasAnnex)
            {
                if (data.Count == 1)
                {
                    MergeCarriersToMainDocument(data[0], carriers);
                }

                RemoveAnnex();
            }
        }

        private MergeField FindFirstMergeFieldInAnnexTable()
        {
            return
                AnnexMergeFields.Single(
                    mf => mf.FieldName.OuterTypeName.Equals(TypeName, StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("RegistrationNumber",
                              StringComparison.InvariantCultureIgnoreCase));
        }

        private Table FindMultipleCarriersTable(MergeField firstMergeFieldInTable)
        {
            return firstMergeFieldInTable.Run.Ancestors<Table>().First();
        }
    }
}