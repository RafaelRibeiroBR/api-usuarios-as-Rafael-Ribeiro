using APIUsuarios.Application.DTOs;

namespace APIUsuarios.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioReadDto>> GetAllAsync(CancellationToken ct);
    Task<UsuarioReadDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<UsuarioReadDto> CreateAsync(UsuarioCreateDto usuarioCreateDto, CancellationToken ct);
    Task<UsuarioReadDto?> UpdateAsync(int id, UsuarioUpdateDto usuarioUpdateDto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
}
