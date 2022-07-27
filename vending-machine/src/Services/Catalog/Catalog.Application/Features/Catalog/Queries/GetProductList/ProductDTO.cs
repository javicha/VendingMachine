namespace Vending.Application.Features.Catalog.Queries.GetProductList
{
    /// <summary>
    /// DTO object with the Product information customized for the presentation layer
    /// </summary>
    public class ProductDTO
    {
        /// <summary>
        /// Product identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Sale price
        /// </summary>
        public decimal Price { get; set; }
    }
}
