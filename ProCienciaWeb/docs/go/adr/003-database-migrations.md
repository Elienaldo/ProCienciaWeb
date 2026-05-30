# ADR-003: Migrations de schema com golang-migrate

- **Data**: 2026-05-30
- **Status**: Aceito
- **Decisores**: Migração Go (MG-2)
- **Tags**: database, migrations, devops

## Contexto e enunciado do problema

O repositório **ServicoProCiencia** não versiona pasta `Migrations/` EF; o schema foi inferido dos models na MG-1. O monólito Go precisa de **migrations versionadas** para:

1. Documentar o schema alvo em `migrations/`.
2. Permitir ambientes reproduzíveis (dev, CI, staging).
3. Evoluir o banco após o cutover sem depender de EF Tools.

Na V1, espera-se **baseline** compatível com tabelas existentes (`Projeto`, `Area`, `SubArea`, `Instituicao`), não recriação do zero em produção.

## Drivers da decisão

- SQL Server como alvo (ver [ADR-002](./002-sql-server-driver.md)).
- Arquivos SQL versionados no Git, revisáveis em PR.
- Ferramenta CLI usada em CI e localmente (sem acoplamento ao binário da app).
- Comunidade Go familiar com **golang-migrate** ou **goose** — escolher uma e padronizar.

## Opções consideradas

- **golang-migrate/migrate** (CLI + biblioteca)
- **pressly/goose**
- **Continuar sem migrations** (apenas scripts ad hoc)

## Resultado da decisão

Padronizar **`github.com/golang-migrate/migrate/v4`** com arquivos em `migrations/` no formato `{version}_{nome}.up.sql` / `.down.sql`, executados via CLI no deploy (MG-7) e documentados em `docs/go/deploy.md`.

**Baseline MG-4:** primeira migration `000001_baseline` espelha o schema inferido; em produção com tabelas já existentes, usar estratégia **“baseline já aplicada”** (registrar versão sem DROP) documentada no TDD.

### Consequências positivas

- Histórico auditável de alterações de schema pós-cutover.
- Rollback de schema via `.down.sql` quando seguro.
- Independente do runtime da aplicação.

### Consequências negativas

- Dupla fonte de verdade temporária (EF legado vs SQL em `migrations/`) até descomissionar .NET.
- Baseline em banco legado exige procedimento manual de “stamp” da versão inicial.
- Equipe deve aprender CLI migrate (documentar em MG-7).

## Prós e contras das opções

### golang-migrate (escolhida)

- ✅ Suporte maduro a SQL Server
- ✅ Separação clara CLI vs app
- ✅ Amplamente referenciado em tutoriais Go
- ❌ Menos integração “Go puro” que goose embutido em `main`

### goose

- ✅ Pode rodar migrations embutidas em Go (`embed`)
- ❌ Menos padronizado no plano já citando golang-migrate como exemplo
- ❌ Equipe precisaria de ADR adicional para trocar depois

### Sem migrations

- ✅ Menos trabalho imediato
- ❌ Schema continua implícito; risco alto em MG-4/MG-7

## Links

- [architecture.md](../architecture.md) — Estratégia de dados e rollback
- [ADR-002](./002-sql-server-driver.md) — Driver SQL Server
- [inventario-api-legado.md](../inventario-api-legado.md) §6.2 — Tabelas inferidas
