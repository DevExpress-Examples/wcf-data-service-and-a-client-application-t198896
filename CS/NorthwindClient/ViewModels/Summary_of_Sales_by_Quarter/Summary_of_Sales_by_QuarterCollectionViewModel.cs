using System;
using System.Linq;
using DevExpress.Mvvm.POCO;
using NorthwindClient.Common.Utils;
using NorthwindClient.Northwind2012EntitiesDataModel;
using NorthwindClient.Common.DataModel;
using NorthwindClient.ServiceReference1;
using NorthwindClient.Common.ViewModel;

namespace NorthwindClient.ViewModels {
    /// <summary>
    /// Represents the Summary_of_Sales_by_Quarters collection view model.
    /// </summary>
    public partial class Summary_of_Sales_by_QuarterCollectionViewModel : CollectionViewModel<Summary_of_Sales_by_Quarter, int, INorthwind2012EntitiesUnitOfWork> {

        /// <summary>
        /// Creates a new instance of Summary_of_Sales_by_QuarterCollectionViewModel as a POCO view model.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        public static Summary_of_Sales_by_QuarterCollectionViewModel Create(IUnitOfWorkFactory<INorthwind2012EntitiesUnitOfWork> unitOfWorkFactory = null) {
            return ViewModelSource.Create(() => new Summary_of_Sales_by_QuarterCollectionViewModel(unitOfWorkFactory));
        }

        /// <summary>
        /// Initializes a new instance of the Summary_of_Sales_by_QuarterCollectionViewModel class.
        /// This constructor is declared protected to avoid undesired instantiation of the Summary_of_Sales_by_QuarterCollectionViewModel type without the POCO proxy factory.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        protected Summary_of_Sales_by_QuarterCollectionViewModel(IUnitOfWorkFactory<INorthwind2012EntitiesUnitOfWork> unitOfWorkFactory = null)
            : base(unitOfWorkFactory ?? UnitOfWorkSource.GetUnitOfWorkFactory(), x => x.Summary_of_Sales_by_Quarters) {
        }
    }
}