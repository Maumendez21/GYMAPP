using BackendGymStar.Data;
using BackendGymStar.Services;
using BackendGymStar.Services.Asocios;
using BackendGymStar.Services.Caja;
using BackendGymStar.Services.Membresia;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";




var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      builder =>
//                      {
//                          builder.WithOrigins("http://localhost").AllowAnyHeader().AllowAnyMethod();
//                      });
//});


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsRule", rule =>
    {
        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
    });
});

//private readonly string _MyCors = "MyCors";

// Se agrega servicio para que los json puedan ser serializados
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Se agrega servicio para que no afecte las peticiones del cliente Instalar el packete Microsoft.AspNetCore.Mvc.NewtonsoftJson
builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<DataBaseContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Servicio para hacer mapeos de sql al cliente con AutoMapper
builder.Services.AddAutoMapper(typeof(Program));


// Autenticación
//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = true;
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//})
//.AddEntityFrameworkStores<AlmacenContext>()
//.AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"])),
                    ClockSkew = System.TimeSpan.Zero
                });


// Se configuran cors para que pudieran hacer peticiónes
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "MyCors", builder =>
//    {
//        builder
//        .AllowAnyOrigin()
//        .AllowAnyHeader()
//        .AllowAnyMethod();
//    });
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "MyCors", builder =>
//    {
//        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost:4200")
//        .AllowAnyHeader().AllowAnyMethod();
//    });
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsRule", rule =>
//    {
//        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
//    });
//});


builder.Services.AddScoped<IGeneralRepository, GeneralRepository>();
builder.Services.AddScoped<IMembresiaRepository, MembresiaRepository>();
builder.Services.AddScoped<ICajaRepository, CajaRepository>();
builder.Services.AddScoped<IAsocioService, AsocioService>();


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
app.UseRouting();
app.UseCors("CorsRule");
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();
//app.UseCors("MyCors");


app.Run();
