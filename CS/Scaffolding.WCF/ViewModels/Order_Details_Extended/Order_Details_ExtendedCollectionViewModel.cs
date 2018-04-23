﻿using System;
using System.Linq;
using DevExpress.Mvvm.POCO;
using Scaffolding.WCF.Common.Utils;
using Scaffolding.WCF.NorthwindEntitiesDataModel;
using Scaffolding.WCF.Common.DataModel;
using Scaffolding.WCF.NorthwindEntities;
using Scaffolding.WCF.Common.ViewModel;

namespace Scaffolding.WCF.ViewModels {
    /// <summary>
    /// Represents the Order_Details_Extendeds collection view model.
    /// </summary>
    public partial class Order_Details_ExtendedCollectionViewModel : CollectionViewModel<Order_Details_Extended, Tuple<float, int, int, string, short, decimal>, INorthwindEntitiesUnitOfWork> {

        /// <summary>
        /// Creates a new instance of Order_Details_ExtendedCollectionViewModel as a POCO view model.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        public static Order_Details_ExtendedCollectionViewModel Create(IUnitOfWorkFactory<INorthwindEntitiesUnitOfWork> unitOfWorkFactory = null) {
            return ViewModelSource.Create(() => new Order_Details_ExtendedCollectionViewModel(unitOfWorkFactory));
        }

        /// <summary>
        /// Initializes a new instance of the Order_Details_ExtendedCollectionViewModel class.
        /// This constructor is declared protected to avoid undesired instantiation of the Order_Details_ExtendedCollectionViewModel type without the POCO proxy factory.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        protected Order_Details_ExtendedCollectionViewModel(IUnitOfWorkFactory<INorthwindEntitiesUnitOfWork> unitOfWorkFactory = null)
            : base(unitOfWorkFactory ?? UnitOfWorkSource.GetUnitOfWorkFactory(), x => x.Order_Details_Extendeds) {
        }
    }
}