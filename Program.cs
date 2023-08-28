using Backend.Data;
using Backend.Services.Users;
using Backend.Services.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do Cors
ConfigureCors(builder);

// Deploy no IIS
ConfigureIIS(builder);

// Configura��es do contexto de banco de dados usando o provedor SQL Server
ConfigureDB(builder);

// Configura��es de autentica��o JWT
ConfigureJWT(builder);

// Configura��es do Swagger
ConfigureSwagger(builder);

// Configura��es de autoriza��o
builder.Services.AddAuthorization();

// Adiciona a configura��o de controladores ao servi�o
builder.Services.AddControllers();

// Adiciona os servi�os de escopo para inje��o de depend�ncia
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<TasksService>();

// Constr�i a aplica��o
var app = builder.Build();

// Habilita o CORS usando a pol�tica "AllowAllOrigins"
app.UseCors("AllowAllOrigins");

app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PlanejaA� - API v1");
    options.RoutePrefix = "";
});

app.UseAuthentication(); // Adiciona a autentica��o � pipeline da aplica��o
app.UseAuthorization(); // Adiciona a autoriza��o � pipeline da aplica��o

app.MapControllers(); // Mapeia os controladores da aplica��o

app.Run(); // Executa a aplica��o

void ConfigureCors(WebApplicationBuilder builder) {
    builder.Services.AddCors(options => {
        options.AddPolicy("AllowAllOrigins", builder => {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    });
}

void ConfigureIIS(WebApplicationBuilder builder) {
    builder.Services.Configure<IISServerOptions>(options => {
        options.AutomaticAuthentication = false;
    });
}

void ConfigureDB(WebApplicationBuilder builder) {
    builder.Services.AddDbContext<ApplicationDbContext>(options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
}

void ConfigureJWT(WebApplicationBuilder builder) {
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateAudience = false, // N�o valida a audi�ncia do token
            ValidateIssuer = false, // N�o valida o emissor do token
            ValidateLifetime = true, // Valida a expira��o do token
            ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"])) // Chave de assinatura do token
        };
    });
}

void ConfigureSwagger(WebApplicationBuilder builder) {
    builder.Services.AddSwaggerGen(options => {

        // Configura��o do documento Swagger
        options.SwaggerDoc("v1", new OpenApiInfo {
            Title = "PlanejaAi - Api",
            Version = "v1",
            Description = "Uma api de gest�o de tarefas simples para voc� se planejar di�riamente.",
            Contact = new OpenApiContact {
                Name = "M�rcio Henrique Lima de Oliveira",
                Email = "marcio@github.com.br",
                Url = new Uri("http://planejaai.com.br")
            }
        });

        // Defini��o do esquema de seguran�a "Bearer"
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
            Description = "Informe um token JWT v�lido apenas o Token sem o Prefixo Bearer.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });

        // Adi��o do requisito de seguran�a "Bearer" para todas as opera��es
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
}