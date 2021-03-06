﻿namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core.WasteType;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Domain.NotificationApplication;
    using Formatters;
    using Mapper;
    using ViewModels;

    internal class WasteCompositionBlock : AnnexBlockBase, IDocumentBlock, IAnnexedBlock
    {
        private const string Parameters = "Parameters";
        private const string Constituents = "Constituents";
        private readonly WasteCompositionViewModel data;

        public WasteCompositionViewModel Data
        {
            get { return data; }
        }

        public WasteCompositionBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new WasteCompositionViewModel(notification.WasteType, new WasteCompositionFormatter());

            //Set annex merge fields
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
            ((List<MergeField>)AnnexMergeFields).AddRange(MergeFieldLocator.GetAnnexMergeFields(mergeFields, Parameters));
            ((List<MergeField>)AnnexMergeFields).AddRange(MergeFieldLocator.GetAnnexMergeFields(mergeFields, Constituents));
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public bool HasAnnex
        {
            get 
            { 
                return (data.HasAnnex || 
                          data.ChemicalComposition != ChemicalComposition.Other || 
                          data.WoodTypeDescription.Length > WasteCompositionViewModel.TextLength() ||
                          data.OtherTypeDescription.Length > WasteCompositionViewModel.TextLength() ||
                          data.OptionalInformation.Length > 0); 
            }
        }

        public virtual void GenerateAnnex(int annexNumber)
        {
            MergeToMainDocument(annexNumber);
            MergeAnnexNumber(annexNumber);

            TocText = "Annex " + annexNumber + " - Designation and composition of waste";

            if (data.HasAnnex)
            {
                InstructionsText = "Designation and composition of waste - annex " + annexNumber;
            }

            var tableproperties = PropertyHelper.GetPropertiesForViewModel(typeof(ChemicalCompositionPercentages));

            if (data.Compositions.Count > 0)
            {
                MergeToTable(tableproperties, data.Compositions, Constituents);
            }
            else
            {
                RemoveAnnexTable("Constituents");
            }

            if (data.AdditionalInfos.Count > 0)
            {
                MergeToTable(tableproperties, data.AdditionalInfos, Parameters);
            }
            else
            {
                RemoveAnnexTable("Parameters");
            }
        }

        public string TypeName
        {
            get { return "Comp"; }
        }

        public int OrdinalPosition
        {
            get { return 12; }
        }

        public virtual void Merge()
        {
            if (!HasAnnex)
            {
                var properties = PropertyHelper.GetPropertiesForViewModel(typeof(WasteCompositionViewModel));

                foreach (var field in CorrespondingMergeFields)
                {
                    MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
                }

                RemoveAnnex();
            }
        }

        protected void MergeToMainDocument(int annexNumber)
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(WasteCompositionViewModel));
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, new WasteCompositionViewModel(data, annexNumber), properties);
            }

            var nonTableAnnexMergeFields = AnnexMergeFields.Where(f => !(f.FieldName.ToString().Contains("Parameters") || f.FieldName.ToString().Contains("Constituents")));
            foreach (var annexMergeField in nonTableAnnexMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(annexMergeField, data, properties);
            }
        }

        private void MergeToTable(PropertyInfo[] properties, IList<ChemicalCompositionPercentages> list, string tableName)
        {
            var mergeTableRows = new TableRow[list.Count()];

            var firstMergeFieldInTable = FindFirstMergeFieldInTable(tableName);
            var table = FindTable(firstMergeFieldInTable);

            mergeTableRows[0] = firstMergeFieldInTable.Run.Ancestors<TableRow>().First();

            for (var i = 1; i < list.Count(); i++)
            {
                mergeTableRows[i] = (TableRow)mergeTableRows[0].CloneNode(true);
                table.AppendChild(mergeTableRows[i]);
            }

            for (var i = 0; i < mergeTableRows.Length; i++)
            {
                foreach (var field in MergeFieldLocator.GetMergeRuns(mergeTableRows[i]))
                {
                    MergeFieldDataMapper.BindCorrespondingField(
                        MergeFieldLocator.ConvertAnnexMergeFieldToRegularMergeField(field), list[i], properties);
                }
            }
        }

        private MergeField FindFirstMergeFieldInTable(string id)
        {
            var mergeField = AnnexMergeFields.Single(
                    mf => mf.FieldName.OuterTypeName.Equals(id, StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("Name",
                              StringComparison.InvariantCultureIgnoreCase));

            return mergeField;
        }

        private Table FindTable(MergeField firstMergeFieldInTable)
        {
            var fieldRun = firstMergeFieldInTable.Run;
            var tableAncestors = fieldRun.Ancestors<Table>();
            return tableAncestors.First();
        }

        private void RemoveAnnexTable(string id)
        {
            var mf = FindFirstMergeFieldInTable(id);

            mf.Run.Ancestors<Table>().First().Remove();
        }
    }
}