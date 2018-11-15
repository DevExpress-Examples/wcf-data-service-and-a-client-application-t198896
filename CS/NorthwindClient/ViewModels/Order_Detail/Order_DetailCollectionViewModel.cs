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
    /// Represents the Order_Details collection view model.
    /// </summary>
    public partial class Order_DetailCollectionViewModel : ReadOnlyCollectionViewModel<Order_Detail, INorthwind2012EntitiesUnitOfWork> {

        /// <summary>
        /// Creates a new instance of Order_DetailCollectionViewModel as a POCO view model.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        public static Order_DetailCollectionViewModel Create(IUnitOfWorkFactory<INorthwind2012EntitiesUnitOfWork> unitOfWorkFactory = null) {
            return ViewModelSource.Create(() => new Order_DetailCollectionViewModel(unitOfWorkFactory));
        }

        /// <summary>
        /// Initializes a new instance of the Order_DetailCollectionViewModel class.
        /// This constructor is declared protected to avoid undesired instantiation of the Order_DetailCollectionViewModel type without the POCO proxy factory.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        protected Order_DetailCollectionViewModel(IUnitOfWorkFactory<INorthwind2012EntitiesUnitOfWork> unitOfWorkFactory = null)
            : base(unitOfWorkFactory ?? UnitOfWorkSource.GetUnitOfWorkFactory(), x => x.Order_Details) {
        }
    }
}