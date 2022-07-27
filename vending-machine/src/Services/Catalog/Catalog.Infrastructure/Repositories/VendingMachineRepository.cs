using Microsoft.EntityFrameworkCore;
using Vending.Application.Contracts.Persistence;
using Vending.Domain.Entities;
using Vending.Infrastructure.Persistence;

namespace Vending.Infrastructure.Repositories
{
    /// <summary>
    /// Class containing the implementation of the specific methods for VendingMachineRepository
    /// </summary>
    public class VendingMachineRepository : RepositoryBase<VendingMachine>, IVendingMachineRepository
    {
        public VendingMachineRepository(VendingContext vendingContext) : base(vendingContext) { }


        /// <summary>
        /// Get vending machine by serial number
        /// </summary>
        /// <param name="serialNumber">Unique serial number identifier</param>
        /// <returns>The vending machine with the given serial number</returns>
        public Task<VendingMachine> GetBySerialNumberAsync(string serialNumber)
        {
            string serial = serialNumber.Trim().ToUpper();
            return _vendingContext.VendingMachines.Where(v => v.SerialNumber == serialNumber).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the vending machine information and the list of all available products
        /// </summary>
        /// <param name="serialNumber">Vending machine serial number</param>
        /// <returns>The product catalog of the vending machine</returns>
        public Task<VendingMachine> GetVendingMachineWithProduct(string serialNumber)
        {
            string serial = serialNumber.Trim().ToUpper();
            return _vendingContext.VendingMachines.Where(v => v.SerialNumber == serial).Include(v => v.Products.Where(p => p.DateDeleted == null && p.Portions > 0)).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Get the vending machine information and the coins inserted
        /// </summary>
        /// <param name="serialNumber">Vending machine serial number</param>
        /// <returns>The vending machine and coins inserted</returns>
        public Task<VendingMachine> GetVendingMachineWithCoins(string serialNumber)
        {
            string serial = serialNumber.Trim().ToUpper();
            return _vendingContext.VendingMachines.Where(v => v.SerialNumber == serial).Include(v => v.Coins.Where(c => c.DateDeleted == null)).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Get the vending machine information and all related information
        /// </summary>
        /// <param name="serialNumber">Vending machine serial number</param>
        /// <returns>The product catalog of the vending machine and coins inserted</returns>
        public Task<VendingMachine> GetVendingMachineWithProductsAndCoins(string serialNumber)
        {
            string serial = serialNumber.Trim().ToUpper();
            return _vendingContext.VendingMachines.Where(v => v.SerialNumber == serial).Include(v => v.Products.Where(p => p.DateDeleted == null && p.Portions > 0))
                .Include(v => v.Coins.Where(c => c.DateDeleted == null)).FirstOrDefaultAsync();
        }
    }
}