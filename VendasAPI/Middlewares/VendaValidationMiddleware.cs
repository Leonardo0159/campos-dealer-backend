using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace VendasAPI.Middlewares
{
    public class VendaValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public VendaValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
            {
                if (context.Request.Path.StartsWithSegments("/api/vendas"))
                {
                    context.Request.EnableBuffering();

                    using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                    var body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    try
                    {
                        var jsonElement = JsonDocument.Parse(body).RootElement;
                        var missingFields = new List<string>();

                        if (!jsonElement.TryGetProperty("idCliente", out _))
                        {
                            missingFields.Add("idCliente");
                        }
                        if (!jsonElement.TryGetProperty("idProduto", out _))
                        {
                            missingFields.Add("idProduto");
                        }
                        if (!jsonElement.TryGetProperty("qtdVenda", out _))
                        {
                            missingFields.Add("qtdVenda");
                        }

                        if (missingFields.Count > 0)
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            var response = new
                            {
                                message = "JSON inválido. Campos obrigatórios estão faltando.",
                                missingFields = missingFields
                            };
                            await context.Response.WriteAsJsonAsync(response);
                            return;
                        }
                    }
                    catch (JsonException ex)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync($"JSON inválido: {ex.Message}");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
