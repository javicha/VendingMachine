using Vending.Domain.Entities;

namespace Vending.Test.Domain
{
    public class ProductTest
    {
        [Fact]
        public void CheckMinimalStock_Ok()
        {
            //Given
            var product = new Product("ProductTest", 1.50M, 10, 1); //A product with 10 portions and a minimal stock of 1
            bool minStockReached = false;

            //When
            for (int i = 0; i < 8; i++)
            {
                minStockReached = product.DecreasePortion();
            }

            //Then
            Assert.False(minStockReached);
            minStockReached = product.DecreasePortion();
            Assert.True(minStockReached);
        }


        [Fact]
        public void CheckNotNegativeStock_Ok()
        {
            //Given
            var product = new Product("ProductTest", 1.50M, 10); //A product with 10 portions

            //When
            for (int i = 0; i < 1000; i++)
            {
                product.DecreasePortion();
            }

            //Then
            Assert.Equal(0, product.Portions);
        }
    }
}
