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
    /// Represents the Invoices collection view model.
    /// </summary>
    public partial class InvoiceCollectionViewModel : ReadOnlyCollectionViewModel<Invoice, INorthwindEntitiesUnitOfWork> {

        /// <summary>
        /// Creates a new instance of InvoiceCollectionViewModel as a POCO view model.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        public static InvoiceCollectionViewModel Create(IUnitOfWorkFactory<INorthwindEntitiesUnitOfWork> unitOfWorkFactory = null) {
            return ViewModelSource.Create(() => new InvoiceCollectionViewModel(unitOfWorkFactory));
        }

        /// <summary>
        /// Initializes a new instance of the InvoiceCollectionViewModel class.
        /// This constructor is declared protected to avoid undesired instantiation of the InvoiceCollectionViewModel type without the POCO proxy factory.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        protected InvoiceCollectionViewModel(IUnitOfWorkFactory<INorthwindEntitiesUnitOfWork> unitOfWorkFactory = null)
            : base(unitOfWorkFactory ?? UnitOfWorkSource.GetUnitOfWorkFactory(), x => x.Invoices) {
        }
    }
}