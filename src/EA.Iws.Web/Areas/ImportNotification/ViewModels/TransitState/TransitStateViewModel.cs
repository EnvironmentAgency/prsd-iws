﻿namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.TransitState
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using Core.TransportRoute;

    public class TransitStateViewModel
    {
        public Guid TransitStateId { get; set; }

        public int OrdinalPosition { get; set; }

        [Display(Name = "CountryId", ResourceType = typeof(TransitStateViewModelResources))]
        public Guid? CountryId { get; set; }

        [Display(Name = "CompetentAuthorityId", ResourceType = typeof(TransitStateViewModelResources))]
        public Guid? CompetentAuthorityId { get; set; }

        [Display(Name = "EntryPointId", ResourceType = typeof(TransitStateViewModelResources))]
        public Guid? EntryPointId { get; set; }

        [Display(Name = "ExitPointId", ResourceType = typeof(TransitStateViewModelResources))]
        public Guid? ExitPointId { get; set; }

        public TransitStateViewModel()
        {
            TransitStateId = Guid.NewGuid();
            Countries = new List<CountryData>();
            CompetentAuthorities = new List<CompetentAuthorityData>();
            EntryOrExitPoints = new List<EntryOrExitPointData>();
        }

        public TransitStateViewModel(TransitState transitState)
        {
            TransitStateId = transitState.Id;
            OrdinalPosition = transitState.OrdinalPosition;
            CompetentAuthorityId = transitState.CompetentAuthorityId;
            CountryId = transitState.CountryId;
            EntryPointId = transitState.EntryPointId;
            ExitPointId = transitState.ExitPointId;
        }

        public IList<CompetentAuthorityData> CompetentAuthorities { get; set; }

        public IList<CountryData> Countries { get; set; }

        public IList<EntryOrExitPointData> EntryOrExitPoints { get; set; }

        public SelectList CountryList
        {
            get
            {
                return new SelectList(Countries, "Id", "Name");
            }
        }

        public SelectList EntryOrExitPointList
        {
            get
            {
                return new SelectList(EntryOrExitPoints, "Id", "Name");
            }
        }

        public bool IsCountrySelected
        {
            get { return CountryId.HasValue; }
        }

        public TransitState AsTransitState()
        {
            return new TransitState
            {
                Id = TransitStateId,
                EntryPointId = EntryPointId,
                CountryId = CountryId,
                OrdinalPosition = OrdinalPosition,
                ExitPointId = ExitPointId,
                CompetentAuthorityId = CompetentAuthorityId
            };
        }
    }
}