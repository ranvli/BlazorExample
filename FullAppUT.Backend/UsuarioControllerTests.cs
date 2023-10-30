using FullApp.Server.Controllers;
using FullApp.Server.Data;
using FullApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullAppUT.Backend
{
    [TestFixture]
    public class UsuarioControllerTests
    {
        private UsuarioController _controller;
        private FullApp.Server.Data.DbContext _dbContext;
        private IConfiguration _configuration;

        public UsuarioControllerTests()
        {
            // Configurar la configuración de prueba en lugar de utilizar IConfiguration
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();
        }

        [OneTimeSetUp]
        public void Setup()
        {
            // Configurar el contexto de la base de datos en memoria para cada prueba
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .EnableSensitiveDataLogging()
                .Options;

            _dbContext = new FullApp.Server.Data.DbContext((DbContextOptions<FullApp.Server.Data.DbContext>)options, _configuration);

            // Agregar datos de prueba al contexto de la base de datos en memoria
            _dbContext.Usuarios.AddRange(
                new Usuario
                {
                    Id = 1,
                    Nombre = "Usuario 1",
                    Apellido1 = "Apellido 1",
                    Apellido2 = "Apellido 2",
                    TelefonoMovil = "123456789"
                },
                new Usuario
                {
                    Id = 2,
                    Nombre = "Usuario 2",
                    Apellido1 = "Apellido 1",
                    Apellido2 = "Apellido 2",
                    TelefonoMovil = "987654321"
                }
            );

            _dbContext.SaveChanges();

            // Crear una nueva instancia del controlador de usuarios con el contexto en memoria
            _controller = new UsuarioController(_dbContext);
        }



        [Test]
        public async Task GetUsuarios_ReturnsOkResultWithUsuarios()
        {
            // Act
            var result = await _controller.GetUsuarios();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var usuarios = okResult.Value as IEnumerable<Usuario>;
            Assert.IsNotNull(usuarios);
            Assert.AreEqual(2, usuarios.Count());
        }

        [Test]
        public async Task GetUsuario_ExistingId_ReturnsOkResultWithUsuario()
        {
            // Arrange
            var usuarioId = 2;

            // Act
            var result = await _controller.GetUsuario(usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ActionResult<Usuario>>(result);

            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);

            var usuario = okResult.Value as Usuario;
            Assert.NotNull(usuario);

            Assert.AreEqual(usuarioId, usuario.Id);
        }


        [Test]
        public async Task GetUsuario_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var nonExistingId = 999;

            // Act
            var result = await _controller.GetUsuario(nonExistingId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }


        [Test]
        public async Task CreateUsuario_ValidUsuario_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var usuario = new Usuario
            {
                Id = 3,
                Nombre = "Usuario 3",
                Apellido1 = "Apellido 1",
                Apellido2 = "Apellido 2",
                TelefonoMovil = "123456789"
            };

            // Act
            var result = await _controller.CreateUsuario(usuario);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            Assert.AreEqual("GetUsuario", createdAtActionResult.ActionName);

            var createdUsuario = createdAtActionResult.Value as Usuario;
            Assert.IsNotNull(createdUsuario);
            Assert.AreEqual(3, createdUsuario.Id);
            Assert.AreEqual("Usuario 3", createdUsuario.Nombre);
        }

        [Test]
        public async Task UpdateUsuario_ValidUsuario_ReturnsNoContentResult()
        {
            // Arrange
            var usuarioId = 2;
            var updatedUsuario = new Usuario
            {
                Id = usuarioId,
                Nombre = "Nuevo Nombre",
                Apellido1 = "Nuevo Apellido 1",
                Apellido2 = "Nuevo Apellido 2",
                TelefonoMovil = "Nuevo Teléfono Móvil"
            };

            // Habilitar el rastreo de entidades
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            // Act
            var result = await _controller.UpdateUsuario(usuarioId, updatedUsuario);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteUsuario_ExistingId_ReturnsNoContentResult()
        {
            // Act
            var result = await _controller.DeleteUsuario(1);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task DeleteUsuario_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var nonExistingId = 9999;

            // Act
            var result = await _controller.DeleteUsuario(nonExistingId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

    }
}
