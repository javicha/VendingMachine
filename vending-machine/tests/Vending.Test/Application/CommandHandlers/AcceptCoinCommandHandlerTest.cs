using AutoMapper;
using Moq;
using Moq.AutoMock;
using System.Collections.Concurrent;
using Vending.Application.Contracts.Persistence;
using Vending.Application.Features.Catalog.Commands.AcceptCoin;
using Vending.Application.Mappings;
using Vending.Domain.Entities;

namespace Vending.Test.Application.CommandHandlers
{
    /// <summary>
    /// Test class to ensure the correct operation of AcceptCoinCommandHandler 
    /// </summary>
    public class AcceptCoinCommandHandlerTest
    {
        private readonly AutoMocker mocker;
        private readonly AcceptCoinCommandHandler handler;
        private readonly Mock<IVendingMachineRepository> vendingRepository;

        public AcceptCoinCommandHandlerTest()
        {
            mocker = new AutoMocker();
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
            mocker.Use(mapperConfig.CreateMapper());
            handler = mocker.CreateInstance<AcceptCoinCommandHandler>();
            vendingRepository = mocker.GetMock<IVendingMachineRepository>();
        }

        [Fact]
        public async Task AddCoinsHandler_OK()
        {
            //Variables
            ConcurrentQueue<Task<decimal>> _tasks = new ConcurrentQueue<Task<decimal>>();
            var vendingMachineUpdated = GetVendingMachine();
            List<decimal> amountList = new List<decimal> { 1M, 2M, 0.10M, 0.50M, 0.20M, 1M };

            //Stub
            this.vendingRepository.Setup(r => r.GetVendingMachineWithCoins("1234")).ReturnsAsync(vendingMachineUpdated);
            this.vendingRepository.Setup(r => r.UpdateAsync(It.IsAny<VendingMachine>(), It.IsAny<string>())).ReturnsAsync(vendingMachineUpdated);

            amountList.ForEach(a =>
            {
                var command = new AcceptCoinCommand("1234", a);
                //Execute
                _tasks.Enqueue(handler.Handle(command, new CancellationToken()));
            });

            var amounts = await Task.WhenAll(_tasks);

            //Assert
            Assert.Equal(amountList.Sum(a => a), amounts.Sum(a => a));

            //Verify
            vendingRepository.Verify(r => r.UpdateAsync(It.IsAny<VendingMachine>(), It.IsAny<string>()), Times.Exactly(amountList.Count), "Repository not called");
        }

        private VendingMachine GetVendingMachine()
        {
            string userCreated = "javier.val";
            var vendingMachine = new VendingMachine("1234", "Delicious Drinks", userCreated);
            foreach (var product in GetProducts())
            {
                vendingMachine.AddNewProduct(userCreated, product.Name, product.Price, product.Portions, product.MinStock);
            }
            foreach (var coin in GetCoins())
            {
                vendingMachine.AddNewCoin(userCreated, coin);
            }

            return vendingMachine;
        }

        private IEnumerable<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product("Tea", 1.30M, 10),
            };
        }
        private IEnumerable<Coin> GetCoins()
        {
            //10 cent, 1 coin
            //20 cent, 1 coin
            //50 cent, 1 coin
            //1 euro, 1 coin
            List<Coin> coins = new List<Coin>();
            coins.Add(new Coin(0.10M));
            coins.Add(new Coin(0.20M));
            coins.Add(new Coin(0.50M));
            coins.Add(new Coin(1M));
            return coins;
        }
    }
}
