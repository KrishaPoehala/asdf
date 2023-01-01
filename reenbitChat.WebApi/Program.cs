using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using reenbitChat.BLL.Auth;
using reenbitChat.BLL.Hubs;
using reenbitChat.BLL.Services;
using reenbitChat.DAL.Context;
using reenbitChat.DAL.Entities;
using reenbitChat.Domain.Jwt;
using reenbitChat.Domain.Services;
using reenbitChat.WebApi.Extentions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var migrationAssebly = typeof(ChatContext).Assembly.GetName().Name;
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
   // builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

// Add services to the container.
builder.Services.AddDbContext<ChatContext>(x => 
x.UseSqlServer(builder.Configuration.GetConnectionString("ChatDb"), 
opt => opt.MigrationsAssembly(migrationAssebly)));
builder.Services.AddAutoMapper(typeof(reenbitChat.Common.Profiles.MapperProfile));
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddSignalR();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IChatService, ChatsService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<ChatContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors("corsapp");
app.MapControllers();
app.MapHub<ChatHub>("/chat");
InitializeDb(app);

app.Run();

void InitializeDb(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<ChatContext>();
    context.Database.Migrate();
}