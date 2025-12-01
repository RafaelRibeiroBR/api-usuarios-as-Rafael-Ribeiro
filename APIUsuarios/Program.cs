using APIUsuarios.Application.DTOs;
using APIUsuarios.Application.Interfaces;
using APIUsuarios.Application.Services;
using APIUsuarios.Application.Validators;
using APIUsuarios.Domain.Entities;
using APIUsuarios.Infrastructure.Persistence;
using APIUsuarios.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddValidatorsFromAssemblyContaining<UsuarioCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioUpdateDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Middleware for validation and error handling
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (ValidationException ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
    }
});

// Endpoints
app.MapGet("/usuarios", async (IUsuarioService usuarioService, CancellationToken ct) =>
{
    var usuarios = await usuarioService.GetAllAsync(ct);
    return Results.Ok(usuarios);
});

app.MapGet("/usuarios/{id:int}", async (int id, IUsuarioService usuarioService, CancellationToken ct) =>
{
    var usuario = await usuarioService.GetByIdAsync(id, ct);
    return usuario is not null ? Results.Ok(usuario) : Results.NotFound();
});

app.MapPost("/usuarios", async (UsuarioCreateDto dto, IUsuarioService usuarioService, IValidator<UsuarioCreateDto> validator, CancellationToken ct) =>
{
    var validationResult = await validator.ValidateAsync(dto, ct);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
    }

    var usuario = await usuarioService.CreateAsync(dto, ct);
    return Results.Created($"/usuarios/{usuario.Id}", usuario);
});

app.MapPut("/usuarios/{id:int}", async (int id, UsuarioUpdateDto dto, IUsuarioService usuarioService, IValidator<UsuarioUpdateDto> validator, CancellationToken ct) =>
{
    var validationResult = await validator.ValidateAsync(dto, ct);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
    }

    var usuario = await usuarioService.UpdateAsync(id, dto, ct);
    return usuario is not null ? Results.NoContent() : Results.NotFound();
});

app.MapDelete("/usuarios/{id:int}", async (int id, IUsuarioService usuarioService, CancellationToken ct) =>
{
    var result = await usuarioService.DeleteAsync(id, ct);
    return result ? Results.NoContent() : Results.NotFound();
});

app.Run();
