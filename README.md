# Agendamento de Salas de Reunião

Este é um projeto ASP.NET Core Web API para gerenciar o agendamento de salas de reunião, desenvolvido utilizando as melhores práticas e tecnologias modernas.

---

## Tecnologias Utilizadas

- **ASP.NET Core 8.0**: Framework principal para a construção da API.
- **Entity Framework Core**: ORM para manipulação do banco de dados.
- **SQL Server**: Banco de dados utilizado.
- **Kendo UI Templates**: Para componentes e interfaces dinâmicas (quando aplicável).
- **Docker**: Para conteinerização do banco de dados SQL Server.
- **SendGrid**: Para envio de e-mails de notificação.

---

## Funcionalidades Principais

1. **Gerenciamento de Salas**:
   - Cadastro, edição e exclusão de salas.
   - Listagem de salas disponíveis com base em filtros de horários e recursos.

2. **Gerenciamento de Reservas**:
   - Agendamento de salas.
   - Validação de conflitos de horário.
   - Envio de notificação por e-mail em caso de recusa ou confirmação da reserva.

3. **Autenticação e Histórico de Reservas**:
   - Listagem de reservas feitas por um usuário.

---

## Configuração do Ambiente

### Requisitos

- .NET 8.0 SDK instalado.
- Docker instalado (opcional, para rodar o banco de dados).
- SQL Server instalado (caso não utilize Docker).
- Conta SendGrid configurada para envio de e-mails.

#### Configuração do Connection String

Atualize o arquivo `appsettings.json`:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProjetoAgendamento;User Id=sa;Password=YourStrongPassword123;"
}
```

### Configuração do SendGrid

1. No arquivo `appsettings.json`, adicione:
   ```json
   "SendGrid": {
       "ApiKey": "SEU_API_KEY"
   }
   ```

2. Certifique-se de usar o `IConfiguration` para recuperar a chave na aplicação.

---

## Execução do Projeto

1. Restaure os pacotes NuGet:
   ```bash
   dotnet restore
   ```

2. Aplique as migrações do banco de dados:
   ```bash
   dotnet ef database update
   ```

3. Inicie a aplicação:
   ```bash
   dotnet run
   ```

---

## Endpoints da API

### Salas
- **GET /api/salas/disponiveis**: Lista salas disponíveis com base nos filtros enviados.

### Reservas
- **POST /api/reservas**: Cria uma nova reserva.
- **GET /api/reservas/historico**: Lista o histórico de reservas de um usuário.

---

## Testes

1. Use ferramentas como **Postman** ou **Insomnia** para testar os endpoints.

---

## Contribuições

Sinta-se à vontade para abrir issues ou enviar pull requests com melhorias e correções.


