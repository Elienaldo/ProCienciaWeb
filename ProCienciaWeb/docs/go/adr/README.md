# Architecture Decision Records — ProCiencia Go

Registro imutável de decisões de arquitetura da migração .NET → Go (fase **MG-2**).

| ADR | Título | Status |
|-----|--------|--------|
| [001](./001-chi-http-router.md) | Router HTTP com chi | Aceito |
| [002](./002-sql-server-driver.md) | Driver SQL Server com go-mssqldb | Aceito |
| [003](./003-database-migrations.md) | Migrations com golang-migrate | Aceito |
| [004](./004-web-in-process-monolith.md) | Web SSR in-process | Aceito |

**Documento mestre:** [architecture.md](../architecture.md) (TDD).  
**Contrato API:** [inventario-api-legado.md](../inventario-api-legado.md).

Novas decisões: criar `00N-titulo-kebab.md` sem editar ADRs aceitos; superseder com novo ADR e atualizar status do anterior.
