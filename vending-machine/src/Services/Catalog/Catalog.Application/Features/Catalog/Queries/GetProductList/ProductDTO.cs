namespace Vending.Application.Features.Catalog.Queries.GetProductList
{
    /// <summary>
    /// DTO object with the Product information customized for the presentation layer
    /// </summary>
    public class ProductDTO
    {
        /// <summary>
        /// Product name
        /// </summary>
        public string? Name { get; private set; }

        /// <summary>
        /// Sale price
        /// </summary>
        public decimal Price { get; private set; }
    }
}
