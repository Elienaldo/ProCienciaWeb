# ADR-002: Driver SQL Server com go-mssqldb

- **Data**: 2026-05-30
- **Status**: Aceito
- **Decisores**: Migração Go (MG-2)
- **Tags**: database, sqlserver, persistence

## Contexto e enunciado do problema

A API legada persiste em **SQL Server** (banco `ProCiencia`, connection string local e Azure App Settings). O Go deve usar **`database/sql`** com um driver mantido, compatível com Azure SQL / SQL Server on-prem e com o schema existente (sem reescrever para outro SGBD na V1).

O inventário MG-1 não confirmou DDL live (API Azure retornava 500); ainda assim a paridade exige o mesmo motor relacional.

## Drivers da decisão

- Reutilizar banco e tabelas existentes na MG-4 (Strangler Fig).
- Evitar ORM pesado na V1 — SQL explícito em `internal/repository`.
- Suporte a `DATABASE_URL` / connection string no formato ADO/SQL Server.
- Comunidade e manutenção ativas para SQL Server no ecossistema Go.

## Opções consideradas

- **microsoft/go-mssqldb** (driver `sqlserver`, sucessor do denisenkom)
- **Ent + GORM** com dialector SQL Server
- **Migração para PostgreSQL** no cutover

## Resultado da decisão

Adotar **`github.com/microsoft/go-mssqldb`** registrado como driver `sqlserver` em `database/sql`, com repositórios em `internal/repository` usando queries parametrizadas.

ORM (GORM/Ent) fica **fora do escopo V1** — pode ser reavaliado se o volume de queries justificar.

### Consequências positivas

- Alinhamento direto ao legado EF + SQL Server.
- Controle fino de `Include`-equivalentes (JOINs) para paridade GET lista de `Projeto` e `SubArea`.
- Pool de conexões gerenciado por `database/sql`.

### Consequências negativas

- Tipos T-SQL e `IDENTITY` exigem cuidado em inserts (SCOPE_IDENTITY / OUTPUT).
- Testes de integração precisam de SQL Server (container ou instância local) — ver MG-6.
- Sem migrations versionadas no repo legado; validação de nomes de tabela fica para MG-4.

## Prós e contras das opções

### go-mssqldb (escolhida)

- ✅ Driver oficial Microsoft; mantido
- ✅ Funciona com `database/sql` idiomático
- ❌ SQL manual para cada entidade

### ORM (GORM/Ent)

- ✅ Menos boilerplate em CRUD simples
- ❌ Mapeamento de navegações e convenções EF podem divergir silenciosamente
- ❌ Mais dependências e curva para paridade fina de JSON

### PostgreSQL

- ✅ Stack “cloud native” comum em exemplos Go
- ❌ **Breaking** para Azure atual e apps que apontam para SQL existente
- ❌ Migração de dados fora do escopo MG-2…MG-5

## Links

- [inventario-api-legado.md](../inventario-api-legado.md) §6 — Schema inferido
- [ADR-003](./003-database-migrations.md) — Ferramenta de migrations
- [architecture.md](../architecture.md) — Variável `DATABASE_URL`
