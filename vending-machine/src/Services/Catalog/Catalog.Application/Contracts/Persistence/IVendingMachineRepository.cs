using Vending.Domain.Entities;

namespace Vending.Application.Contracts.Persistence
{
    /// <summary>
    /// Specific contracts for the VendingMachine entity
    /// </summary>
    public interface IVendingMachineRepository : IAsyncRepository<VendingMachine>
    {
        /// <summary>
        /// Get vending machine by serial number
        /// </summary>
        /// <param name="serialNumber">Unique serial number identifier</param>
        /// <returns>The vending machine with the given serial number</returns>
        Task<VendingMachine> GetBySerialNumberAsync(string serialNumber);

        /// <summary>
        /// Obtain information from the vending machine and its list of available products
        /// </summary>
        /// <param name="serialNumber">Vending machine unique serial number</param>
        /// <returns>The product catalog of the vending machine</returns>
        Task<VendingMachine> GetVendingMachineWithProduct(string serialNumber);

        /// <summary>
        /// Get the vending machine information and the coins inserted
        /// </summary>
        /// <param name="serialNumber">Vending machine serial number</param>
        /// <returns>The vending machine and coins inserted</returns>
        Task<VendingMachine> GetVendingMachineWithCoins(string serialNumber);

        /// <summary>
        /// Get the vending machine information and all related information
        /// </summary>
        /// <param name="serialNumber">Vending machine serial number</param>
        /// <returns>The product catalog of the vending machine and coins inserted</returns>
        Task<VendingMachine> GetVendingMachineWithProductsAndCoins(string serialNumber);
    }
}
