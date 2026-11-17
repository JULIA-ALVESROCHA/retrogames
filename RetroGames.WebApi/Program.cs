using RetroGames.WebApi.Configuration;

// Carrega o .env da raiz do repositório (procura subindo as pastas a partir da atual).
// Cada linha vira variável de ambiente: ConnectionStrings__Default vira ConnectionStrings:Default.
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string não encontrada. Crie o arquivo .env na raiz do repositório " +
        "(copie o .env.example) com a linha ConnectionStrings__Default.");
}

// Serviços (injeção de dependência)
builder.Services.AddRepositorios(connectionString);
builder.Services.AddServicosDeNegocio();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Pipeline: por onde toda requisição passa
if (app.Environment.IsDevelopment())
{
    // Descrição da API em /openapi/v1.json e página de testes em /swagger
    app.MapOpenApi();
    app.UseSwaggerUI(opcoes => opcoes.SwaggerEndpoint("/openapi/v1.json", "RetroGames API"));
}

app.MapControllers();

app.Run();
