# ADR-001: Router HTTP com chi

- **Data**: 2026-05-30
- **Status**: Aceito
- **Decisores**: Migração Go (MG-2)
- **Tags**: architecture, http, go

## Contexto e enunciado do problema

O monólito Go expõe rotas **REST** (`/api/*`) para apps móveis e rotas **SSR** (`/`, `/listaprojetos`, …) para o navegador. É necessário um roteador com grupos de middleware, parâmetros de path (`{id}`) e composição clara entre API e web no mesmo processo (`cmd/prociencia`).

O plano de migração já indica **chi** como padrão; esta ADR formaliza a escolha em relação ao roteamento da biblioteca padrão.

## Drivers da decisão

- Paridade de paths com ASP.NET (`/api/Projetos/{id}`) sem ambiguidade.
- Middleware encadeável (logging, recovery, timeout) compartilhado entre API e web.
- Ecossistema maduro e alinhado ao [PLANO-MIGRACAO-GO.md](../../PLANO-MIGRACAO-GO.md).
- Equipe/agente com documentação abundante; curva de aprendizado baixa para monólito pequeno.

## Opções consideradas

- **chi** (`github.com/go-chi/chi/v5`)
- **net/http** + `http.ServeMux` (Go 1.22+ routing)
- **echo** / **gin** (frameworks completos)

## Resultado da decisão

Escolhida a opção **chi**, por composição leve de middleware, rotas explícitas compatíveis com o contrato legado e ausência de camadas desnecessárias (ORM, binding mágico) para um monólito modular.

### Consequências positivas

- Rotas agrupadas: `r.Route("/api", …)` e `r.Route("/", webHandlers)`.
- Middleware padrão da comunidade (logger, recoverer, timeout).
- Dependência única e estável; sem “framework opinionado” além do HTTP.

### Consequências negativas

- Dependência externa além da stdlib (aceitável; chi é amplamente adotado).
- Não traz validação/serialização JSON — permanecem em handlers + `encoding/json`.

## Prós e contras das opções

### chi (escolhida)

- ✅ Middleware composável; sub-routers
- ✅ Compatível com `net/http` (`http.Handler`)
- ❌ Mais uma dependência no `go.mod`

### net/http ServeMux

- ✅ Zero dependências
- ❌ Middleware menos ergonômico; histórico de limitações em rotas complexas
- ❌ Menos convenção documentada no projeto para grupos `/api` vs `/`

### echo / gin

- ✅ Produtividade em APIs grandes
- ❌ Acoplamento a binding/validação do framework
- ❌ Desalinhado ao princípio “monólito fino” do plano Go

## Links

- [architecture.md](../architecture.md) — Solução técnica, diagramas C4
- [inventario-api-legado.md](../inventario-api-legado.md) — Matriz de rotas `/api/*`
- [PLANO-MIGRACAO-GO.md](../../PLANO-MIGRACAO-GO.md) — Decisão de stack
