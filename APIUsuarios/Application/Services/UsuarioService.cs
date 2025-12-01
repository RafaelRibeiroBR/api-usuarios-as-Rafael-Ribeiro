using APIUsuarios.Application.DTOs;
using APIUsuarios.Application.Interfaces;
using APIUsuarios.Domain.Entities;
using FluentValidation;

namespace APIUsuarios.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IValidator<UsuarioCreateDto> _createValidator;
    private readonly IValidator<UsuarioUpdateDto> _updateValidator;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IValidator<UsuarioCreateDto> createValidator,
        IValidator<UsuarioUpdateDto> updateValidator)
    {
        _usuarioRepository = usuarioRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<UsuarioReadDto>> GetAllAsync(CancellationToken ct)
    {
        var usuarios = await _usuarioRepository.GetAllAsync(ct);
        return usuarios.Select(MapToReadDto);
    }

    public async Task<UsuarioReadDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id, ct);
        return usuario is null ? null : MapToReadDto(usuario);
    }

    public async Task<UsuarioReadDto> CreateAsync(UsuarioCreateDto usuarioCreateDto, CancellationToken ct)
    {
        await _createValidator.ValidateAndThrowAsync(usuarioCreateDto, ct);

        if (await _usuarioRepository.EmailExistsAsync(usuarioCreateDto.Email.ToLower(), ct))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var usuario = new Usuario
        {
            Nome = usuarioCreateDto.Nome,
            Email = usuarioCreateDto.Email.ToLower(),
            Senha = usuarioCreateDto.Senha, // In production, hash the password
            DataNascimento = usuarioCreateDto.DataNascimento,
            Telefone = usuarioCreateDto.Telefone,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };

        await _usuarioRepository.AddAsync(usuario, ct);
        await _usuarioRepository.SaveChangesAsync(ct);

        return MapToReadDto(usuario);
    }

    public async Task<UsuarioReadDto?> UpdateAsync(int id, UsuarioUpdateDto usuarioUpdateDto, CancellationToken ct)
    {
        await _updateValidator.ValidateAndThrowAsync(usuarioUpdateDto, ct);

        var usuario = await _usuarioRepository.GetByIdAsync(id, ct);
        if (usuario is null)
        {
            return null;
        }

        usuario.Nome = usuarioUpdateDto.Nome;
        usuario.Email = usuarioUpdateDto.Email.ToLower();
        usuario.DataNascimento = usuarioUpdateDto.DataNascimento;
        usuario.Telefone = usuarioUpdateDto.Telefone;
        usuario.Ativo = usuarioUpdateDto.Ativo;
        usuario.DataAtualizacao = DateTime.UtcNow;

        await _usuarioRepository.UpdateAsync(usuario, ct);
        await _usuarioRepository.SaveChangesAsync(ct);

        return MapToReadDto(usuario);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id, ct);
        if (usuario is null)
        {
            return false;
        }

        usuario.Ativo = false;
        usuario.DataAtualizacao = DateTime.UtcNow;

        await _usuarioRepository.UpdateAsync(usuario, ct);
        await _usuarioRepository.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    {
        return await _usuarioRepository.EmailExistsAsync(email.ToLower(), ct);
    }

    private static UsuarioReadDto MapToReadDto(Usuario usuario)
    {
        return new UsuarioReadDto(
            usuario.Id,
            usuario.Nome,
            usuario.Email,
            usuario.DataNascimento,
            usuario.Telefone,
            usuario.Ativo,
            usuario.DataCriacao
        );
    }
}
