# ADR-004: Web SSR in-process (sem HTTP interno para API)

- **Data**: 2026-05-30
- **Status**: Aceito
- **Decisores**: Migração Go (MG-2)
- **Tags**: architecture, web, monolith

## Contexto e enunciado do problema

O legado **ProCienciaWeb** (Blazor) chama a API Azure via `HttpClient` — dois processos e latência de rede. No monólito Go, **webhandlers** e **apihandlers** coexistem no mesmo binário.

Deve-se decidir se páginas SSR chamam `GET /api/Projetos` em loopback HTTP ou compartilham a camada **`internal/service`** diretamente.

## Drivers da decisão

- Monólito modular: um deploy, um processo (MG-3…MG-7).
- Apps móveis **continuam** usando REST `/api/*` — contrato externo inalterado.
- Reduzir latência e falhas de configuração de URL interna na web.
- Manter **uma** implementação de regras de negócio (DRY entre web e API).

## Opções consideradas

- **In-process:** `webhandlers` → `service` → `repository`
- **Loopback HTTP:** `webhandlers` → `http://127.0.0.1:PORT/api/...`
- **Dois binários** (web + api) — fora do escopo do plano V1

## Resultado da decisão

**Web in-process:** handlers SSR injetam (via `main` / struct `App`) interfaces do pacote `internal/service`. Rotas `/api/*` permanecem em `internal/apihandlers` para clientes externos, também delegando ao mesmo `service`.

### Consequências positivas

- Paridade de validação (BL-001…005) sem duplicar lógica HTTP.
- Sem `API_BASE_URL` obrigatório para o servidor renderizar páginas.
- Testes de `service` cobrem comportamento usado pela web e pela API.

### Consequências negativas

- `webhandlers` não exercita serialização JSON da API — testes de contrato REST continuam necessários (MG-6).
- Acoplamento em tempo de compilação entre pacotes `webhandlers` e `service` (aceitável no monólito).
- Refatorar para microserviços no futuro exigiria extrair API ou introduzir cliente HTTP.

## Prós e contras das opções

### In-process (escolhida)

- ✅ Simples, rápido, idiomático para monólito Go
- ✅ Um ponto para transações futuras (se necessário)
- ❌ Web não valida automaticamente o wire format JSON da API

### Loopback HTTP

- ✅ Web e móvel passam pelo mesmo stack HTTP
- ❌ Overhead, portas, timeouts e erros de configuração
- ❌ Duplica caminho de erro (handler API vs cliente HTTP)

### Dois binários

- ✅ Escala independente (irrelevante no estágio atual)
- ❌ Contradiz PLANO-MIGRACAO-GO e aumenta custo operacional

## Links

- [architecture.md](../architecture.md) — Fluxo de dados e diagrama C4
- [ADR-001](./001-chi-http-router.md) — Montagem de rotas no mesmo router
- [architecture.md legado](../../architecture.md) — Blazor como thin client HTTP (padrão antigo)
