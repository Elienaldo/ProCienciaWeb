# AGENTS.md — ProCiencia (Go)

Guia curto para o agente Cursor na migração .NET → Go.

**Branch de trabalho:** `feature/migracao-go`  
**Fase atual:** MG-2 concluída → próxima **MG-3** ([PLANO-MIGRACAO-GO.md](ProCienciaWeb/docs/PLANO-MIGRACAO-GO.md))

## Stack

- Go 1.22+, [chi](https://github.com/go-chi/chi), `html/template`, `database/sql`, `slog`
- Monólito modular: `cmd/prociencia` + `internal/*` (scaffold a partir de MG-3)

## Antes de codar

- [ProCienciaWeb/docs/PLANO-MIGRACAO-GO.md](ProCienciaWeb/docs/PLANO-MIGRACAO-GO.md) — fase atual (MG-0 … MG-8)
- [ProCienciaWeb/docs/go/inventario-api-legado.md](ProCienciaWeb/docs/go/inventario-api-legado.md) — contrato API legada (MG-1)
- [ProCienciaWeb/docs/go/architecture.md](ProCienciaWeb/docs/go/architecture.md) — após MG-2
- [ProCienciaWeb/docs/PRD.md](ProCienciaWeb/docs/PRD.md) — requisitos e backlog

## Comandos (quando existir scaffold Go)

```powershell
go run ./cmd/prociencia
go test ./...
```

E2E: ver `e2e/README.md` (MG-6).

## Regras

- Não evoluir `legacy/dotnet/` exceto remoção coordenada (MG-7)
- Manter contrato `/api/*` para apps móveis
- Sem secrets no Git; usar variáveis de ambiente
- Uma fase MG por sessão; preencher DoD antes de avançar
- Plano .NET [PLANO-EXECUCAO-POR-FASES.md](ProCienciaWeb/docs/PLANO-EXECUCAO-POR-FASES.md) está **pausado**

## MCPs

| MCP | Escopo | Fases |
|-----|--------|-------|
| GitHub | Global (Cursor) | MG-0+ |
| Playwright | Global (Cursor) | MG-6 |
| Azure | `.cursor/mcp.json` (raiz do repo) | MG-7 |
| SQL Server (MSSQL) | `.cursor/mcp.json` — requer `MSSQL_CONNECTION_STRING` no ambiente | MG-1, MG-4 |

## Skills recomendadas

Catálogo: [agent-skills.techleads.club/skills](https://agent-skills.techleads.club/skills/)

**Arquitetura e migração:** `legacy-migration-planner`, `modular-decomposition`, `modular-design-principles`, `domain-analysis`, `decomposition-planning-roadmap`, `the-fool`

**Design técnico (TDD/ADR):** `technical-design-doc-creator`, `create-adr`

**Desenvolvimento e docs:** `coding-guidelines`, `codenavi`, `best-practices`, `docs-writer`, `chrome-devtools`

**Cursor (built-in):** `systematic-debugging`, `test-driven-development`, `verification-before-completion`, `brainstorming`, `writing-plans`

Detalhes por fase MG-*: [ProCienciaWeb/docs/PLANO-MIGRACAO-GO.md](ProCienciaWeb/docs/PLANO-MIGRACAO-GO.md) — Anexo A.

Instalação local (Node ≥ 22 no PATH; ex.: Node do Cursor em `Local\Programs\cursor\...\helpers`):

```powershell
npx -y @tech-leads-club/agent-skills install --skill coding-guidelines
# … demais skills — lista completa em ProCienciaWeb/docs/go/MG-0-RESULTADO.md
```
