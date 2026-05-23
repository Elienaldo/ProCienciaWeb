# Inventário técnico — ProCienciaWeb

Documento gerado na **Fase 1** do plano de execução. Mapeia estrutura, dependências, entrypoints, rotas, integrações HTTP e código legado.

**Repositório:** `d:\Projetos\ProCiencia\ProCienciaWeb`  
**Branch:** `feature/docs-mcp-planejamento`  
**Data:** 2026-05-22

---

## 1. Visão geral

| Item | Valor |
|------|--------|
| Solução | `ProCienciaWeb.sln` (1 projeto) |
| Projeto web | `ProCienciaWeb/ProCienciaWeb.csproj` |
| Target Framework | `netcoreapp3.1` (.NET Core 3.1 — EOL) |
| Modelo de hospedagem | Blazor Server |
| API externa | `https://apiprociencia.azurewebsites.net` |
| Cliente HTTP | `ApiService` (singleton) + `HttpClient` estático |
| Autenticação local | Não implementada |
| Testes automatizados | Nenhum projeto de teste na solução |

---

## 2. Árvore de diretórios

Níveis 2–3, excluindo `bin/`, `obj/`, `.git/`, `.vs/`.

```
ProCienciaWeb/                          ← raiz do repositório Git
├── .cursor/
│   └── mcp.json                        ← MCP Azure (projeto)
├── .gitignore
├── LICENSE
├── README.md
├── ProCienciaWeb.sln
└── ProCienciaWeb/                      ← projeto ASP.NET Core
    ├── API/
    │   └── ApiService.cs               ← cliente REST da API Azure
    ├── Controller/                     ← EXCLUÍDO da compilação (.csproj)
    │   └── ProjetoController.cs
    ├── Data/                           ← template Blazor (legado)
    │   ├── WeatherForecast.cs
    │   └── WeatherForecastService.cs
    ├── docs/
    │   ├── PLANO-EXECUCAO-POR-FASES.md
    │   ├── FASE-0-RESULTADO.md
    │   ├── FASE-0-GUIA-PENDENCIAS.md
    │   └── inventario-tecnico.md       ← este arquivo
    ├── Models/
    │   ├── Area.cs
    │   ├── Instituicao.cs
    │   ├── Projeto.cs
    │   └── SubArea.cs
    ├── Pages/
    │   ├── _Host.cshtml                ← host Blazor Server (fallback)
    │   ├── Component.razor             ← EXCLUÍDO do build (Content Remove)
    │   ├── Counter.razor               ← template (legado)
    │   ├── EditarProjeto.razor
    │   ├── Error.razor
    │   ├── IncluirProjeto.cshtml       ← EXCLUÍDO do build
    │   ├── IncluirProjeto.cshtml.cs    ← EXCLUÍDO da compilação
    │   ├── IncluirProjeto.razor        ← página ativa de inclusão
    │   ├── Index.razor
    │   └── ListaProjetos.razor
    ├── Properties/
    │   └── launchSettings.json
    ├── Shared/
    │   ├── MainLayout.razor
    │   ├── NavMenu.razor
    │   └── SurveyPrompt.razor          ← template (não referenciado)
    ├── wwwroot/
    │   ├── css/                        ← Bootstrap, open-iconic, site.css
    │   └── favicon.ico
    ├── _Imports.razor
    ├── App.razor                       ← roteador Blazor
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── Program.cs
    ├── ProCienciaWeb.csproj
    └── Startup.cs
```

---

## 3. Dependências (`ProCienciaWeb.csproj`)

### 3.1 Target e SDK

| Propriedade | Valor |
|-------------|--------|
| SDK | `Microsoft.NET.Sdk.Web` |
| `TargetFramework` | `netcoreapp3.1` |

### 3.2 Pacotes NuGet explícitos

| Pacote | Versão solicitada | Versão resolvida | Uso |
|--------|-------------------|------------------|-----|
| `Microsoft.VisualStudio.Web.CodeGeneration.Design` | 3.1.3 | 3.1.3 | Scaffolding (dev) |
| `Newtonsoft.Json` | 12.0.3 | 12.0.3 | Serialização JSON em `ApiService` |

### 3.3 Pacotes implícitos (via SDK Web)

O SDK `Microsoft.NET.Sdk.Web` para 3.1 inclui transitivamente, entre outros:

- `Microsoft.AspNetCore.App` (metapacote — Blazor Server, MVC, Razor Pages)
- Componentes Blazor Server (`MapBlazorHub`, SignalR)

Não há referências explícitas a Entity Framework, autenticação ou `IHttpClientFactory`.

### 3.4 Itens excluídos do build

| Caminho | Tipo de exclusão | Motivo provável |
|---------|------------------|-----------------|
| `Controller/**` | Compile, Content, EmbeddedResource, None | Código duplicado/obsoleto (`ProjetoController` vs `ApiService`) |
| `Pages/IncluirProjeto.cshtml.cs` | Compile | Substituído por `IncluirProjeto.razor` |
| `Pages/IncluirProjeto.cshtml` | Content | Idem |
| `Pages/Component.razor` | Content | Componente vazio/não usado |

---

## 4. Entrypoints e pipeline HTTP

### 4.1 `Program.cs`

| Elemento | Descrição |
|----------|-----------|
| `Main` | `CreateHostBuilder(args).Build().Run()` |
| `CreateHostBuilder` | `Host.CreateDefaultBuilder` + `ConfigureWebHostDefaults` → `UseStartup<Startup>()` |

Ponto de entrada único da aplicação.

### 4.2 `Startup.cs` — serviços (`ConfigureServices`)

| Registro | Lifetime | Namespace |
|----------|----------|-----------|
| `AddRazorPages()` | — | Infra ASP.NET Core |
| `AddServerSideBlazor()` | — | Blazor Server |
| `AddSingleton<WeatherForecastService>()` | Singleton | `ProCienciaWeb.Data` |
| `AddSingleton<ApiService>()` | Singleton | `ProCienciaWeb.API` |

### 4.3 `Startup.cs` — pipeline (`Configure`)

| Ordem | Middleware / endpoint | Observação |
|-------|----------------------|------------|
| 1 | `UseDeveloperExceptionPage` (Dev) / `UseExceptionHandler("/Error")` (Prod) | Tratamento de erros |
| 2 | `UseHsts` | Apenas fora de Development |
| 3 | `UseHttpsRedirection` | Redireciona HTTP → HTTPS |
| 4 | `UseStaticFiles` | `wwwroot` |
| 5 | `UseRouting` | — |
| 6 | `MapBlazorHub()` | SignalR do Blazor Server |
| 7 | `MapFallbackToPage("/_Host")` | SPA fallback para Blazor |

Não há `MapControllers`, autenticação, CORS explícito nem `MapRazorPages` adicional além do fallback.

### 4.4 Host Blazor — `Pages/_Host.cshtml`

| Atributo | Valor |
|----------|--------|
| `@page` | `/` (Razor Page de hospedagem) |
| `render-mode` | `ServerPrerendered` |
| Componente raiz | `<App />` (`App.razor`) |
| Script | `_framework/blazor.server.js` |

### 4.5 Roteamento Blazor — `App.razor`

- `Router` com `AppAssembly = typeof(Program).Assembly`
- Layout padrão: `MainLayout`
- Rota não encontrada: mensagem em português

---

## 5. Configuração

### 5.1 `appsettings.json`

| Chave | Valor |
|-------|--------|
| `Logging:LogLevel:Default` | Information |
| `AllowedHosts` | * |

**Nota:** URL da API **não** está em configuração — hardcoded em `ApiService.UrlServico`.

### 5.2 `appsettings.Development.json`

| Chave | Valor |
|-------|--------|
| `DetailedErrors` | true |

### 5.3 `Properties/launchSettings.json`

| Perfil | URL(s) | Ambiente |
|--------|--------|----------|
| `ProCienciaWeb` | `https://localhost:5001`, `http://localhost:5000` | Development |
| `IIS Express` | `http://localhost:52350`, SSL `44334` | Development |

---

## 6. Rotas Blazor (`@page`)

| Rota | Arquivo | Função | Menu (`NavMenu`) |
|------|---------|--------|------------------|
| `/` | `Pages/Index.razor` | Página inicial / boas-vindas | Home (link `""`) |
| `/listaprojetos` | `Pages/ListaProjetos.razor` | Listagem, filtro e ações CRUD (parcial) | Projetos |
| `/incluirprojeto` | `Pages/IncluirProjeto.razor` | Formulário de inclusão | Link na lista |
| `/editarprojeto/{projetoId}` | `Pages/EditarProjeto.razor` | Formulário de edição (update não implementado) | Link na lista |
| `/counter` | `Pages/Counter.razor` | Contador de exemplo (template) | Comentado no menu |
| `/error` | `Pages/Error.razor` | Página de erro (pipeline) | — |

### 6.1 Rotas de infraestrutura (não Blazor `@page` de componente)

| Rota | Arquivo | Tipo |
|------|---------|------|
| `/` (host) | `Pages/_Host.cshtml` | Razor Page — shell HTML |
| `/Error` (handler) | Redirecionamento em `Startup` | Exception handler |

### 6.2 Parâmetros de rota

| Página | Parâmetro | Tipo C# | Uso |
|--------|-----------|---------|-----|
| `EditarProjeto.razor` | `projetoId` | `string` | Convertido com `Convert.ToInt32` em `OnInitializedAsync` |

---

## 7. Arquivos `.cs` e `.razor` — responsabilidades

### 7.1 Código C# (`.cs`)

| Arquivo | Responsabilidade |
|---------|----------------|
| `Program.cs` | Bootstrap do host ASP.NET Core |
| `Startup.cs` | DI e pipeline HTTP |
| `API/ApiService.cs` | Cliente HTTP para API Azure (CRUD parcial) |
| `Controller/ProjetoController.cs` | Duplicata parcial de `ApiService` — **não compilado** |
| `Data/WeatherForecast.cs` | Modelo de exemplo (template) |
| `Data/WeatherForecastService.cs` | Serviço de exemplo — registrado em DI, **não usado** nas páginas de negócio |
| `Models/Projeto.cs` | Entidade principal do domínio |
| `Models/Area.cs` | Área de conhecimento |
| `Models/SubArea.cs` | Subárea (vinculada a `Area`) |
| `Models/Instituicao.cs` | Instituição (API expõe; UI não consome) |
| `Pages/IncluirProjeto.cshtml.cs` | PageModel vazio — **não compilado** |

### 7.2 Componentes Razor (`.razor`)

| Arquivo | Responsabilidade | Status |
|---------|------------------|--------|
| `App.razor` | Roteador raiz | Ativo |
| `_Imports.razor` | Usings globais (Models, API, ObservableCollection) | Ativo |
| `Shared/MainLayout.razor` | Layout com sidebar + `@Body` | Ativo |
| `Shared/NavMenu.razor` | Menu: Home, Projetos | Ativo |
| `Shared/SurveyPrompt.razor` | Banner de pesquisa Microsoft | Legado (comentado em `Index`) |
| `Pages/Index.razor` | Home | Ativo |
| `Pages/ListaProjetos.razor` | Lista projetos, filtro por subárea, excluir (vazio) | Ativo |
| `Pages/IncluirProjeto.razor` | Inclusão de projeto | Ativo |
| `Pages/EditarProjeto.razor` | Edição (sem PUT/PATCH na API) | Ativo (parcial) |
| `Pages/Counter.razor` | Template Blazor | Legado |
| `Pages/Error.razor` | Erro genérico | Ativo (infra) |
| `Pages/Component.razor` | Componente vazio | Excluído do build |

### 7.3 Razor Pages / host (`.cshtml`)

| Arquivo | Responsabilidade | Status |
|---------|------------------|--------|
| `Pages/_Host.cshtml` | HTML shell + Blazor Server | Ativo |
| `Pages/IncluirProjeto.cshtml` | Razor Page estática substituída | Excluído |

---

## 8. Modelos de dados (`Models/`)

| Classe | Propriedades principais | Relacionamentos |
|--------|-------------------------|-----------------|
| `Projeto` | `ProjetoId`, `Titulo`, `Resumo`, `Autor`, `Telefone`, `Email`, `AreaId`, `SubAreaId` | `Area`, `SubArea` (navigation virtual) |
| `Area` | `AreaId`, `Nome` | — |
| `SubArea` | `SubAreaId`, `Nome`, `AreaId` | `Area` |
| `Instituicao` | `InstituicaoId`, `Nome`, `Sigla`, `Natureza` | — |

Comentários `[ForeignKey]` presentes mas sem EF Core no projeto — apenas DTOs para JSON.

---

## 9. `ApiService` — métodos e integrações HTTP

**Base URL:** `https://apiprociencia.azurewebsites.net` (constante `UrlServico`)  
**HTTP client:** `public static HttpClient client` (compartilhado, sem `IHttpClientFactory`)

| Método | HTTP | Endpoint | Request | Response | Usado por |
|--------|------|----------|---------|----------|-----------|
| `ObterProjetos()` | GET | `/api/Projetos` | — | `ObservableCollection<Projeto>` | `ListaProjetos.razor` |
| `ObterProjeto(int projetoId)` | GET | `/api/Projetos/{id}` | — | `Projeto` | `EditarProjeto.razor` |
| `IncluirProjeto(Projeto projeto)` | POST | `/api/Projetos/` | JSON body | — (sem tratamento de resposta) | `IncluirProjeto.razor` |
| `ObterAreas()` | GET | `/api/Areas` | — | `ObservableCollection<Area>` | **Nenhuma página** |
| `ObterSubAreas()` | GET | `/api/SubAreas` | — | `ObservableCollection<SubArea>` | `IncluirProjeto.razor`, `EditarProjeto.razor` |
| `ObterInstituicoes()` | GET | `/api/Instituicoes` | — | `ObservableCollection<Instituicao>` | **Nenhuma página** |

### 9.1 Operações ausentes no cliente

| Operação esperada | Status no código |
|-------------------|------------------|
| Atualizar projeto (PUT/PATCH) | Comentado em `EditarProjeto.razor` (`//await servicoProjeto.Update`) |
| Excluir projeto (DELETE) | `ExcluirProjeto` em `ListaProjetos.razor` — método **vazio** |
| Tratamento de erros HTTP | Não implementado |
| Configuração de URL por ambiente | Hardcoded |

### 9.2 Duplicação legada

`Controller/ProjetoController.cs` repete `UrlServico`, `HttpClient` estático e `ObterProjetos()` — pasta inteira excluída do `.csproj`.

---

## 10. Injeção de dependências nas páginas

| Página | `@inject` / serviços |
|--------|----------------------|
| `ListaProjetos.razor` | `ApiService servicoProjeto` |
| `IncluirProjeto.razor` | `ApiService servicoProjeto`, `NavigationManager` |
| `EditarProjeto.razor` | `ApiService servicoProjeto`, `NavigationManager` |
| Demais | Nenhum inject de API |

Padrão: `@inject ApiService` + `OnInitializedAsync` para carregar dados.

---

## 11. Funcionalidades por página (estado atual)

| Funcionalidade | Página | Status |
|----------------|--------|--------|
| Listar projetos | `ListaProjetos` | OK |
| Filtrar por nome de subárea | `ListaProjetos` | OK (client-side, `oninput`) |
| Incluir projeto | `IncluirProjeto` | OK (POST); select de subárea **sem bind** em `SubAreaId` |
| Editar projeto | `EditarProjeto` | Parcial — carrega dados; **não persiste** alterações |
| Excluir projeto | `ListaProjetos` | **Não implementado** (handler vazio) |
| Listar áreas / instituições | — | API disponível; UI não usa |

---

## 12. Arquivos legado e código morto

| Item | Tipo | Recomendação (futura) |
|------|------|------------------------|
| `Pages/Counter.razor` | Template Blazor | Remover ou manter só em dev |
| `Data/WeatherForecast*.cs` | Template Blazor | Remover registro DI e arquivos |
| `Shared/SurveyPrompt.razor` | Template Blazor | Remover se não usado |
| `Pages/Component.razor` | Vazio, excluído do build | Remover do disco |
| `Pages/IncluirProjeto.cshtml` + `.cs` | Abordagem Razor Pages abandonada | Remover |
| `Controller/ProjetoController.cs` | Duplicata de `ApiService` | Remover pasta |
| `Microsoft.VisualStudio.Web.CodeGeneration.Design` | Dev-only scaffolding | Avaliar remoção se não usado |
| Link `/counter` no `NavMenu` | Comentado | — |

---

## 13. Ecossistema relacionado (referência)

Repositórios GitHub do mesmo autor (`user:Elienaldo`), relevantes para integração:

| Repositório | Papel |
|-------------|--------|
| `ServicoProCiencia` | API REST (provável backend em Azure) |
| `AppProCiencia` / `ProCienciaApp` | Apps móveis |
| `ProCienciaWeb` | Este front-end Blazor |

---

## 14. Resumo executivo

- Aplicação **Blazor Server 3.1** com **6 rotas** de componente e fallback `_Host`.
- Domínio centrado em **Projetos**, com lookups **SubArea**, **Area** e **Instituicao** via API REST.
- Cliente HTTP único (`ApiService`), URL fixa, sem testes e com **CRUD incompleto** (sem update/delete no cliente).
- Vários artefatos do **template inicial** permanecem no repositório; parte já excluída da compilação via `.csproj`.

**Próxima fase:** Fase 2 — `docs/architecture.md` (diagramas C4, sequência, riscos arquiteturais).
