// Adicione estas linhas no topo do arquivo
using WebApplication1.Services;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddControllers();

// Adicione a linha do seu serviço. O erro deve desaparecer agora.
builder.Services.AddSingleton<ContatoService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adicione estas duas linhas para servir seus arquivos HTML, CSS e JS
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();