# MG-1 — Resultado (Descoberta API legada)

**Data início / fim:** 2026-05-30  
**Status:** Concluída  
**Entregável:** [inventario-api-legado.md](./inventario-api-legado.md)

---

## DoD

| Item | Status |
|------|--------|
| Endpoints `/api/*` documentados | [x] |
| Schema / diagrama ER | [x] |
| NuGet / .NET 3.1 registrados | [x] |
| Riscos de paridade | [x] |
| MCP SQL ou decisão manual | [x] — MCP em `.cursor/mcp.json`; schema inferido sem DB live |

---

## Destaques

- **20 rotas REST** (CRUD × 4 entidades) no código e no Swagger Azure.
- **Sem autenticação** na API legada.
- **Azure:** Swagger OK; dados `/api/*` com **500** no teste de 2026-05-30.
- **Front:** só GET/POST Projetos + GET SubAreas; PUT/DELETE existem na API mas não no `ApiService`.

---

## Próximo passo

**MG-2** — `docs/go/architecture.md` + `docs/go/adr/`
