using System.Text.Json;

namespace VendasAPI.Middlewares
{
    public class ClienteValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ClienteValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
            {
                if (context.Request.Path.StartsWithSegments("/api/clientes"))
                {
                    context.Request.EnableBuffering();

                    using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                    var body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    try
                    {
                        var jsonElement = JsonDocument.Parse(body).RootElement;
                        var issues = new List<string>();

                        if (!jsonElement.TryGetProperty("nome", out var nomeProperty))
                        {
                            issues.Add("Campo 'nome' está faltando.");
                        }
                        else if (string.IsNullOrWhiteSpace(nomeProperty.GetString()))
                        {
                            issues.Add("Campo 'nome' não pode ser vazio.");
                        }

                        if (!jsonElement.TryGetProperty("cidade", out var cidadeProperty))
                        {
                            issues.Add("Campo 'cidade' está faltando.");
                        }
                        else if (string.IsNullOrWhiteSpace(cidadeProperty.GetString()))
                        {
                            issues.Add("Campo 'cidade' não pode ser vazio.");
                        }

                        if (issues.Count > 0)
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            var response = new
                            {
                                message = "JSON inválido. Há problemas nos seguintes campos:",
                                issues = issues
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
