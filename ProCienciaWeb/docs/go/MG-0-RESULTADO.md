# MG-0 — Resultado (Pausa .NET + setup agente)

**Data início:** 2026-05-27  
**Data fim:** 2026-05-27  
**Status:** Concluída  
**Branch:** `feature/migracao-go`  
**Fase:** MG-0 do [PLANO-MIGRACAO-GO.md](../PLANO-MIGRACAO-GO.md)

---

## O que foi feito

1. Banner **PAUSADO** em [PLANO-EXECUCAO-POR-FASES.md](../PLANO-EXECUCAO-POR-FASES.md) — Fases 6–8 .NET não executar; link para plano Go; branch ativa documentada.
2. [AGENTS.md](../../../AGENTS.md) na raiz — stack Go, regras, MCPs, skills (catálogo v1.1), branch `feature/migracao-go`.
3. Branch **`feature/migracao-go`** criada a partir de `feature/docs-mcp-planejamento` (alterações MG-0 nesta branch).
4. MCPs revisados (sem secrets) — ver tabela abaixo.
5. Painel e seção MG-0 em [PLANO-MIGRACAO-GO.md](../PLANO-MIGRACAO-GO.md) marcados como concluídos (DoD `[x]`).
6. **11 skills Tech Leads Club** instaladas em `.cursor/skills/` (escopo projeto — Cursor detecta automaticamente; **não** vão para o PATH).
7. **MCP Azure** unificado em `.cursor/mcp.json` na raiz do repo (antes em `ProCienciaWeb/.cursor/`).
8. **Não** criado `go.mod`, `cmd/`, `internal/` — reservado para MG-3.
9. **Não** movido projeto .NET para `legacy/dotnet/` — reservado para MG-7.

---

## Arquivos gerados / alterados

| Arquivo / pasta | Ação |
|-----------------|------|
| `ProCienciaWeb/docs/PLANO-EXECUCAO-POR-FASES.md` | Banner PAUSADO + referência branch Go |
| `ProCienciaWeb/docs/PLANO-MIGRACAO-GO.md` | Painel MG-0 `[x]`, DoD, resultado obtido, skills v1.1 |
| `AGENTS.md` | Criado/atualizado (raiz) |
| `.cursor/mcp.json` | MCP Azure (movido de `ProCienciaWeb/.cursor/`) |
| `.cursor/skills/*` | 11 skills TLC (cópia local para Cursor) |
| `ProCienciaWeb/docs/go/MG-0-RESULTADO.md` | Este arquivo |

---

## MCPs (sem secrets)

| MCP | Status | Observação |
|-----|--------|------------|
| **GitHub** | Habilitado (global Cursor) | PRs, explorar `ServicoProCiencia` |
| **Playwright** | Habilitado (global Cursor) | E2E na MG-6 |
| **Azure** | Projeto — `.cursor/mcp.json` (raiz do repo) | `@azure/mcp` — deploy MG-7 |
| **MSSQL** | Pendente **MG-1** | Adicionar em `.cursor/mcp.json` com `MSSQL_CONNECTION_STRING` via env (sem commitar secret) |

Configuração unificada em `.cursor/` na raiz: `mcp.json` + `skills/` (sem connection strings no repositório).

---

## Ambiente local

| Item | Valor |
|------|--------|
| **Go** | `go1.24.2` (≥ 1.22 — OK) |
| **Node** | `v24.16.0` (`C:\Program Files\nodejs`) — no PATH |
| **Branch Git** | `feature/migracao-go` |
| **Código Go app** | Ausente (correto para MG-0) |
| **.NET** | Em `ProCienciaWeb/` (inalterado) |

---

## Skills Tech Leads Club

**Status:** instaladas (2026-05-27)  
**Destino:** `.cursor/` na raiz do repositório (`mcp.json` + `skills/`)

| Skill | Pasta |
|-------|--------|
| coding-guidelines | `.cursor/skills/coding-guidelines/` |
| codenavi | `.cursor/skills/codenavi/` |
| best-practices | `.cursor/skills/best-practices/` |
| chrome-devtools | `.cursor/skills/chrome-devtools/` |
| docs-writer | `.cursor/skills/docs-writer/` |
| legacy-migration-planner | `.cursor/skills/legacy-migration-planner/` |
| modular-decomposition | `.cursor/skills/modular-decomposition/` |
| modular-design-principles | `.cursor/skills/modular-design-principles/` |
| domain-analysis | `.cursor/skills/domain-analysis/` |
| decomposition-planning-roadmap | `.cursor/skills/decomposition-planning-roadmap/` |
| the-fool | `.cursor/skills/the-fool/` |

**Método:** `npx @tech-leads-club/agent-skills install` falhou aqui com `Failed to fetch registry` (CDN indisponível no ambiente do agente). Skills copiadas a partir do repositório oficial [tech-leads-club/agent-skills](https://github.com/tech-leads-club/agent-skills) (`main`), equivalente ao que o CLI instalaria para Cursor.

**Atualizar no futuro** (com internet no seu terminal):

```powershell
npx -y @tech-leads-club/agent-skills install --skill <nome> -a cursor
```

Catálogo e mapeamento por fase: [PLANO-MIGRACAO-GO.md](../PLANO-MIGRACAO-GO.md) — Anexo A.

**Skills Cursor (built-in):** `systematic-debugging`, `test-driven-development`, `verification-before-completion`, `brainstorming`, `writing-plans`.

---

## DoD MG-0

| Item | Status |
|------|--------|
| Banner de pausa no plano .NET | [x] |
| Branch `feature/migracao-go` criada | [x] |
| `AGENTS.md` na raiz, versionado | [x] |
| MCPs documentados | [x] |
| Nenhum código Go de aplicação | [x] |
| Painel MG-0 atualizado no plano Go | [x] |
| Instalação skills TLC (opcional) | [x] |

---

## Pendências remanescentes (fora do escopo MG-0)

| Item | Fase | Nota |
|------|------|------|
| MCP MSSQL | MG-1 | Após confirmar connection string de dev |
| Commit/push da branch | Dev | Incluir `.cursor/skills/` se quiser versionar no repo |
| `git push -u origin feature/migracao-go` | Dev | Branch só local até push |

---

## Próximo passo

**MG-1** — Descoberta API legada (`ServicoProCiencia`)  
**Entregável:** `docs/go/inventario-api-legado.md`  
**Skills:** `codenavi`, `legacy-migration-planner`, `domain-analysis`, `modular-decomposition`
