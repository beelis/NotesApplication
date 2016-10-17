using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notes.Controllers;
using Notes.Data;
using Notes.Models;
using Xunit;

namespace Notes.Test
{
    public class OrderControllerTest
    {
        private readonly IServiceProvider _serviceProvider;
        private ApplicationDbContext _dbContext;
        private readonly ILogger _logger;


        public OrderControllerTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Notes.Test"));
            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<ApplicationDbContext>();

        }


        [Fact]
        public void CreateSimple()
        {
            var controller = new NotesController(null, null, null);
            Assert.NotNull(controller.Create());
        }
    }
}
