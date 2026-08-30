# Controller-Auth (ApiAutoMapper)

API REST desenvolvida em **.NET 10** para gerenciamento de **Filmes**, utilizando **Entity Framework Core**, **AutoMapper** e autenticação/autorização via **JWT (JSON Web Token)**.

## ✨ Funcionalidades

- CRUD completo de Filmes (criar, listar, buscar por id, atualizar total/parcial e remover)
- Autenticação via login com geração de token JWT
- Autorização baseada em roles (ex: `Admin`) e em usuários autenticados
- Mapeamento automático entre entidades e DTOs com AutoMapper
- Suporte a atualização parcial via `JSON Patch`
- Persistência de dados com Entity Framework Core e MySQL

## 🛠️ Tecnologias

- [.NET 10](https://dotnet.microsoft.com/)
- ASP.NET Core Web API
- Entity Framework Core + `MySql.EntityFrameworkCore`
- AutoMapper
- JWT Bearer Authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Newtonsoft.Json (suporte a `JsonPatchDocument`)
- OpenAPI (Swagger)

## 📁 Estrutura do Projeto

```
ApiAutoMapper/
├── Controllers/
│   ├── Auth/AuthController.cs      # Endpoint de login e geração de token JWT
│   └── FilmeController.cs          # Endpoints CRUD de Filmes
├── Data/
│   ├── DTOs/                       # CreateFilmeDto, ReadFilmeDto, UpdateFilmeDto
│   └── FilmeContext.cs             # DbContext do Entity Framework
├── Extensions/
│   ├── AuthenticationExtention.cs  # Configuração da autenticação JWT
│   └── ConnectionStringExtension.cs# Configuração da conexão com o banco de dados
├── Migrations/                     # Migrations do Entity Framework Core
├── Models/
│   └── Filme.cs                    # Entidade Filme
├── Profiles/
│   └── FilmeProfile.cs             # Perfis de mapeamento do AutoMapper
└── Program.cs                      # Configuração e inicialização da aplicação
```

## ⚙️ Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL Server em execução
- Visual Studio 2022/2026 ou VS Code (opcional)

## 🔧 Configuração

1. Clone o repositório:

   ```powershell
   git clone https://github.com/Wellington-cardosinhoX/Controller-Auth.git
   cd Controller-Auth
   ```

2. Configure a string de conexão com o banco de dados MySQL em `appsettings.json` (ou via `dotnet user-secrets`):

   ```json
   {
	 "ConnectionStrings": {
	   "FilmeConnection": "Server=localhost;Database=FilmeDb;User=root;Password=SuaSenha;"
	 },
	 "Jwt": {
	   "Key": "SuaChaveSecretaComPeloMenos32Caracteres",
	   "Issuer": "ApiAutoMapper",
	   "Audience": "ApiAutoMapperUsers"
	 }
   }
   ```

   > ⚠️ Recomenda-se armazenar `Jwt:Key` e a connection string usando `dotnet user-secrets` ou variáveis de ambiente, evitando expor segredos no `appsettings.json`.

3. Aplique as migrations para criar o banco de dados:

   ```powershell
   dotnet ef database update --project ApiAutoMapper
   ```

## ▶️ Executando a aplicação

```powershell
dotnet run --project ApiAutoMapper
```

A aplicação exibirá a URL local no console (ex: `https://localhost:xxxx`). Em ambiente de desenvolvimento, a documentação OpenAPI fica disponível em `/openapi/v1.json`.

## 🔐 Autenticação

Para autenticar, realize um `POST` no endpoint de login:

```
POST /Auth/login
Content-Type: application/json

{
  "usuario": "admin",
  "senha": "1234"
}
```

A resposta contém o token JWT que deve ser enviado no header `Authorization` das demais requisições:

```
Authorization: Bearer {token}
```

> ⚠️ As credenciais atuais estão fixas no código (`admin` / `1234`) apenas para fins de teste/demonstração. Substitua por uma validação real (ex: consulta a um banco de usuários com senha criptografada via `BCrypt.Net-Next`) antes de usar em produção.

## 📚 Endpoints principais

| Método | Rota              | Autorização      | Descrição                              |
|--------|-------------------|-------------------|-----------------------------------------|
| POST   | `/Auth/login`      | Anônimo           | Realiza login e retorna o token JWT     |
| GET    | `/Filme`           | Autenticado       | Lista filmes com paginação (`skip`, `take`) |
| GET    | `/Filme/{id}`      | Autenticado       | Busca um filme pelo id                  |
| POST   | `/Filme`           | Role `Admin`      | Cadastra um novo filme                  |
| PUT    | `/Filme/{id}`      | Autenticado       | Atualiza um filme por completo          |
| PATCH  | `/Filme/{id}`      | Autenticado       | Atualiza parcialmente um filme (JSON Patch) |
| DELETE | `/Filme/{id}`      | Autenticado       | Remove um filme                         |

### Exemplo de payload - Criar Filme

```json
{
  "titulo": "Interestelar",
  "genero": "Ficção Científica",
  "duracao": 169
}
```

> Regras de validação: `titulo` e `genero` são obrigatórios; `duracao` deve ser maior ou igual a 70 minutos.

## 🧪 Testando a API

Um arquivo [`WebApplication1.http`](ApiAutoMapper/WebApplication1.http) está disponível com exemplos de requisições prontos para uso com o REST Client do Visual Studio/VS Code.

## 📄 Licença

Este projeto é de uso educacional/demonstrativo.
