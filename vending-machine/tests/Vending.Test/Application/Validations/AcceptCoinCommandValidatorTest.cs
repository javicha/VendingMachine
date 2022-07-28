using Moq.AutoMock;
using Vending.Application.Features.Catalog.Commands.AcceptCoin;

namespace Vending.Test.Application.Validations
{
    /// <summary>
    /// Test class to ensure the correct operation of AcceptCoinCommandValidator 
    /// </summary>
    public class AcceptCoinCommandValidatorTest
    {
        private readonly AutoMocker mocker;
        private readonly AcceptCoinCommandValidator validator;

        public AcceptCoinCommandValidatorTest()
        {
            mocker = new AutoMocker();
            validator = mocker.CreateInstance<AcceptCoinCommandValidator>();
        }

        [Fact]
        public async Task AcceptCoinCommandValidator_InvalidCoin()
        {
            // Variables 
            var command = new AcceptCoinCommand("1234", 0.30M);

            // Execute
            var result = await this.validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(0.20)]
        [InlineData(0.50)]
        [InlineData(0.10)]
        public async Task AcceptCoinCommandValidator_ValidCoin(decimal amount)
        {
            // Variables 
            var command = new AcceptCoinCommand("1234", amount);

            // Execute
            var result = await this.validator.ValidateAsync(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
