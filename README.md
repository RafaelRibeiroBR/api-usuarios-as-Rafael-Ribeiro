# API de Gerenciamento de Usuários

## Descrição
Esta é uma API REST completa para gerenciamento de usuários, desenvolvida em ASP.NET Core com Minimal APIs, seguindo os princípios de Clean Architecture. A API permite operações CRUD (Create, Read, Update, Delete) sobre usuários, com validações rigorosas, persistência em SQLite e implementação de padrões de projeto como Repository e Service.

## Tecnologias Utilizadas
- .NET 8.0
- ASP.NET Core Minimal APIs
- Entity Framework Core 8.0
- SQLite
- FluentValidation.AspNetCore 11.3.0
- C# 12.0

## Padrões de Projeto Implementados
- Repository Pattern
- Service Pattern
- DTO Pattern
- Dependency Injection
- Clean Architecture (Domain, Application, Infrastructure)

## Como Executar o Projeto

### Pré-requisitos
- .NET SDK 8.0 ou superior

### Passos
1. Clone o repositório
2. Navegue até a pasta APIUsuarios
3. Execute as migrações do banco de dados:
   ```
   dotnet ef database update
   ```
4. Execute a aplicação:
   ```
   dotnet run
   ```
5. A API estará disponível em `http://localhost:5000`

### Exemplos de Requisições

#### Criar usuário (POST /usuarios)
```json
{
  "nome": "João Silva",
  "email": "joao.silva@email.com",
  "senha": "senha123",
  "dataNascimento": "1990-01-01",
  "telefone": "(11) 99999-9999"
}
```

#### Listar usuários (GET /usuarios)
Resposta:
```json
[
  {
    "id": 1,
    "nome": "João Silva",
    "email": "joao.silva@email.com",
    "dataNascimento": "1990-01-01T00:00:00",
    "telefone": "(11) 99999-9999",
    "ativo": true,
    "dataCriacao": "2024-01-01T00:00:00"
  }
]
```

#### Buscar usuário por ID (GET /usuarios/{id})
Resposta:
```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao.silva@email.com",
  "dataNascimento": "1990-01-01T00:00:00",
  "telefone": "(11) 99999-9999",
  "ativo": true,
  "dataCriacao": "2024-01-01T00:00:00"
}
```

#### Atualizar usuário (PUT /usuarios/{id})
```json
{
  "nome": "João Silva Atualizado",
  "email": "joao.silva@email.com",
  "dataNascimento": "1990-01-01",
  "telefone": "(11) 99999-9999",
  "ativo": true
}
```

#### Remover usuário (DELETE /usuarios/{id})
Resposta: 204 No Content

## Estrutura do Projeto
```
APIUsuarios/
├── Domain/
│   └── Entities/
│       └── Usuario.cs
├── Application/
│   ├── DTOs/
│   │   ├── UsuarioCreateDto.cs
│   │   ├── UsuarioReadDto.cs
│   │   └── UsuarioUpdateDto.cs
│   ├── Interfaces/
│   │   ├── IUsuarioRepository.cs
│   │   └── IUsuarioService.cs
│   ├── Services/
│   │   └── UsuarioService.cs
│   └── Validators/
│       ├── UsuarioCreateDtoValidator.cs
│       └── UsuarioUpdateDtoValidator.cs
├── Infrastructure/
│   ├── Persistence/
│   │   └── AppDbContext.cs
│   └── Repositories/
│       └── UsuarioRepository.cs
├── Migrations/
├── Program.cs
├── appsettings.json
└── APIUsuarios.csproj
```

## Validações Implementadas
- Nome: obrigatório, 3-100 caracteres
- Email: obrigatório, formato válido, único
- Senha: obrigatória, mínimo 6 caracteres
- Data de Nascimento: obrigatória, idade >= 18 anos
- Telefone: opcional, formato brasileiro válido
- Soft Delete: remoção lógica (Ativo = false)

## Códigos de Status HTTP
- 200 OK: Operação bem-sucedida
- 201 Created: Recurso criado
- 204 No Content: Recurso removido
- 400 Bad Request: Dados inválidos
- 404 Not Found: Recurso não encontrado
- 409 Conflict: Email já cadastrado
- 500 Internal Server Error: Erro interno

## Autor
Nome: Rafael Ribeiro do Santos
RA: 2024009663
Curso: Análise e Desenvolvimento de Sistemas

## Vídeo Demonstrativo


<!-- https://youtu.be/iBFVWJZKZxo -->
