using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Notes.Controllers;
using Notes.Models;
using Notes.Data;
using Xunit;

namespace Notes.Test
{
    public class NotesControllerTest
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILoggerFactory _loggerFactory;
        private readonly UserManager<ApplicationUser> _fakeUserManager;

        public NotesControllerTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Notes"));

            _dbContext = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            _loggerFactory = new LoggerFactory();
            _fakeUserManager = new FakeUserManager();
        }






        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }

        [Fact]
        public void CreateSimple()
        {
            NotesController controller = new NotesController(_dbContext, _loggerFactory, _fakeUserManager);
            Assert.NotNull(controller.Create());
        }

        [Fact]
        public async void GetNonExisting()
        {
            NotesController controller = new NotesController(_dbContext, _loggerFactory, _fakeUserManager);
            var result = await controller.Details(99);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetExisting()
        {
            NotesController controller = new NotesController(_dbContext, _loggerFactory, _fakeUserManager);
            var note = new Note();
            note.Title = "Test";
            note.ID = 15;
            note.UserId = "asdf-1234";
            await controller.Create(note);
            var result = await controller.Details(15);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void UpdateNote()
        {
            var note = new Note();
            var controller = new NotesController(_dbContext, _loggerFactory, _fakeUserManager);

            note.Title = "Old Title";
            note.CreationDate = DateTime.Now;
            note.UserId = "5asdf";
            await controller.Create(note);
            note.Title = "New Title";
            var result = await controller.Edit(note.ID, note);

            Assert.IsType<RedirectToActionResult>(result);
        }
    }

    public class FakeUserManager : UserManager<ApplicationUser>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
        {
        }
    }
}