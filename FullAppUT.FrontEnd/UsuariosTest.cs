using Moq;
using System.Net.Http;
using System.Text.Json;
using System.Net;
using Bunit;
using Bunit.TestDoubles;
using FullApp.Client.Pages;
using FullApp.Shared;
using Microsoft.Extensions.DependencyInjection;
using Moq.Protected;
using System.Text;
using Xunit;

namespace FullAppUT.Frontend
{ 
    public class UsuariosTest
    {
        [Fact]
        public async Task UsuariosComponent_ShouldRenderUsersTable()
        {
            // Preparación
            using var ctx = new Bunit.TestContext();
            var usuarios = new[]
            {
        new Usuario { Id = 1, Nombre = "Randall", Apellido1 = "Vargas", Apellido2 = "L", TelefonoMovil = "123456789" },
        new Usuario { Id = 2, Nombre = "Sandra", Apellido1 = "Molina", Apellido2 = "A", TelefonoMovil = "987654321" }
    };

            // Creamos un HttpMessageHandler simulado que devolverá nuestros datos de prueba
            var messageHandler = new Mock<HttpMessageHandler>();
            messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(usuarios), Encoding.UTF8, "application/json"),
                });

            // Creamos un HttpClient usando nuestro HttpMessageHandler simulado
            var httpClient = new HttpClient(messageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com")  // Aquí establecemos la dirección base
            };

            // Registramos nuestro HttpClient simulado en el proveedor de servicios de prueba
            ctx.Services.AddSingleton(httpClient);

            // Acción
            var cut = ctx.RenderComponent<Usuarios>();

            // Afirmación
            var expectedMarkup = @"
<h1>Lista de usuarios (dummy data en base de datos)</h1>
<p>Aquí se muestra cómo obtener datos en base de datos del servidor.</p>
<table class=""table"">
  <thead>
    <tr>
      <th>Id</th>
      <th>Nombre</th>
      <th>Primer apellido</th>
      <th>Segundo apellido</th>
      <th>Teléfono móvil</th>
      <th></th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>1</td>
      <td>Randall</td>
      <td>Vargas</td>
      <td>L</td>
      <td>123456789</td>
      <td>
        <button class=""btn btn-primary btn-sm"">Editar</button>
        <button class=""btn btn-danger btn-sm"">Borrar</button>
      </td>
    </tr>
    <tr>
      <td>2</td>
      <td>Sandra</td>
      <td>Molina</td>
      <td>A</td>
      <td>987654321</td>
      <td>
        <button class=""btn btn-primary btn-sm"">Editar</button>
        <button class=""btn btn-danger btn-sm"">Borrar</button>
      </td>
    </tr>
  </tbody>
</table>
<button class=""btn btn-success"">Crear Usuario</button>
<div class=""modal"" tabindex=""-1"" role=""dialog"" style=""display: none;"">
  <div class=""modal-dialog"" role=""document"">
    <div class=""modal-content"">
      <div class=""modal-header"">
        <h5 class=""modal-title"">Crear/Editar Usuario</h5>
        <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Cerrar"">
          <span aria-hidden=""true"">&times;</span>
        </button>
      </div>
      <div class=""modal-body""></div>
      <div class=""modal-footer"">
        <button type=""button"" class=""btn btn-secondary"">Cerrar</button>
        <button type=""button"" class=""btn btn-primary"">Guardar</button>
      </div>
    </div>
  </div>
</div>";

            cut.MarkupMatches(expectedMarkup);

        }

        private interface IUsuarioService
        {
            Task<Usuario[]> GetUsuariosAsync();
        }

        private class FakeUsuarioService : IUsuarioService
        {
            private readonly Usuario[] _usuarios;

            public FakeUsuarioService(Usuario[] usuarios)
            {
                _usuarios = usuarios;
            }

            public Task<Usuario[]> GetUsuariosAsync()
            {
                return Task.FromResult(_usuarios);
            }
        }
    }
}