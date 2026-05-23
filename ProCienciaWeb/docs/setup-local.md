# Setup local — ProCienciaWeb

Guia para clonar, compilar e executar o projeto no **Windows**. Comandos validados neste repositório em **2026-05-23**.

**Referências:** [`inventario-tecnico.md`](./inventario-tecnico.md) · [`PLANO-EXECUCAO-POR-FASES.md`](./PLANO-EXECUCAO-POR-FASES.md)

---

## 1. Pré-requisitos

| Requisito | Versão mínima | Como verificar |
|-----------|---------------|----------------|
| **.NET Core SDK** | **3.1.301** (qualquer 3.1.x) | `dotnet --version` |
| **Git** | 2.x | `git --version` |
| **Navegador** | Chrome, Edge ou Firefox recente | — |
| **IDE (opcional)** | Visual Studio 2019+, VS Code, Cursor | — |

### SDK 3.1

O projeto usa `netcoreapp3.1`. Instale o SDK em:

https://dotnet.microsoft.com/download/dotnet/3.1

Liste SDKs instalados:

```powershell
dotnet --list-sdks
```

Saída esperada (exemplo validado):

```text
3.1.301 [C:\Program Files\dotnet\sdk]
```

Se só houver SDK 5/6/8, o build pode falhar com erro de framework ausente — instale o **SDK 3.1**, não apenas o runtime.

### Dependência externa (runtime)

A aplicação chama a API em produção:

`https://apiprociencia.azurewebsites.net`

Sem conectividade com essa API, páginas como **Lista de projetos** podem falhar ao carregar dados (ver [Troubleshooting](#6-troubleshooting)).

---

## 2. Clone do repositório

```powershell
git clone https://github.com/Elienaldo/ProCienciaWeb.git
cd ProCienciaWeb
```

Se já tiver o repositório localmente:

```powershell
cd "d:\Projetos\ProCiencia\ProCienciaWeb"
git pull
```

**Branch de documentação (opcional):** `feature/docs-mcp-planejamento`

---

## 3. Restore, build e run

Execute na **raiz da solução** (onde está `ProCienciaWeb.sln`):

### 3.1 Restaurar pacotes

```powershell
cd "d:\Projetos\ProCiencia\ProCienciaWeb"
dotnet restore ProCienciaWeb.sln
```

**Validação (2026-05-23):** concluiu com sucesso — *Todos os projetos estão atualizados para restauração.*

### 3.2 Compilar

```powershell
dotnet build ProCienciaWeb.sln
```

**Validação (2026-05-23):** compilação com sucesso, 0 erros.

> Em builds anteriores foram observados avisos **CS1998** (métodos `async` sem `await`) em `ListaProjetos.razor` e `EditarProjeto.razor` — não impedem a execução.

### 3.3 Executar

```powershell
dotnet run --project ProCienciaWeb\ProCienciaWeb.csproj
```

Atalho equivalente (a partir da pasta do projeto):

```powershell
cd ProCienciaWeb
dotnet run
```

**Validação (2026-05-23):** aplicação iniciou com:

```text
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
Hosting environment: Development
```

Para encerrar: `Ctrl+C` no terminal.

### 3.4 Executar sem recompilar (opcional)

Após um `build` bem-sucedido:

```powershell
dotnet run --project ProCienciaWeb\ProCienciaWeb.csproj --no-build
```

---

## 4. URLs e perfis de execução

Fonte: `ProCienciaWeb/Properties/launchSettings.json`

| Perfil | Comando | URL HTTP | URL HTTPS | Observação |
|--------|---------|----------|-----------|------------|
| **ProCienciaWeb** (padrão `dotnet run`) | `Project` | http://localhost:5000 | https://localhost:5001 | Perfil recomendado |
| **IIS Express** | Visual Studio / `IIS Express` | http://localhost:52350 | https://localhost:44334 | Requer IIS Express instalado |

### Comportamento HTTPS

O `Startup` usa `UseHttpsRedirection()`. Acesso a `http://localhost:5000` pode responder **307** redirecionando para HTTPS — use preferencialmente:

**https://localhost:5001**

### Páginas úteis para teste manual

| URL | Descrição |
|-----|-----------|
| https://localhost:5001/ | Página inicial |
| https://localhost:5001/listaprojetos | Lista de projetos (depende da API) |
| https://localhost:5001/incluirprojeto | Formulário de inclusão |
| https://localhost:5001/editarprojeto/1 | Edição (substitua `1` por id válido) |

---

## 5. Como testar manualmente

### 5.1 App local responde

1. Execute `dotnet run` (seção 3.3).
2. Abra https://localhost:5001 no navegador.
3. Confirme o título **Pró Ciência Web** e o menu **Home** / **Projetos**.

**Validação (2026-05-23):** `GET https://localhost:5001/` → HTTP **200**.

### 5.2 Integração com API Azure

1. No menu, clique em **Projetos** ou acesse https://localhost:5001/listaprojetos .
2. Resultado esperado: tabela com projetos ou mensagem *"Nenhum Projeto Científico foi cadastrado"*.
3. Enquanto carrega, pode aparecer *"Carregando..."*.

Se a API estiver inacessível, a página pode exibir erro (HTTP 500 no servidor ou tela de erro Blazor). Nesse caso, verifique rede, firewall e status da API (seção 6).

### 5.3 Fluxo incluir projeto

1. Em **Projetos**, clique em **Novo Projeto**.
2. Preencha o formulário e **Salvar**.
3. Deve redirecionar para `/listaprojetos`.

> O select de **Área de Conhecimento** pode não gravar `SubAreaId` corretamente (débito conhecido — ver `codebase-overview.md`).

### 5.4 Visual Studio

1. Abra `ProCienciaWeb.sln`.
2. Defina **ProCienciaWeb** como projeto de inicialização.
3. Perfil **ProCienciaWeb** ou **IIS Express** → F5.

---

## 6. Troubleshooting

### 6.1 SDK incorreto ou ausente

**Sintoma:** `error NETSDK1045` ou referência a `netcoreapp3.1` não encontrada.

**Solução:**

```powershell
dotnet --list-sdks
```

Instale [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet/3.1). Reinicie o terminal após instalar.

---

### 6.2 Certificado HTTPS de desenvolvimento

**Sintoma:** navegador bloqueia `https://localhost:5001` (avisos de certificado).

**Solução:**

```powershell
dotnet dev-certs https --trust
```

Reinicie o navegador. No primeiro acesso, aceite o certificado de desenvolvimento se solicitado.

---

### 6.3 Porta 5000 ou 5001 em uso

**Sintoma:** `Failed to bind to address https://127.0.0.1:5001` ou similar.

**Solução:**

- Encerre outra instância do app (`Ctrl+C` no terminal que executou `dotnet run`).
- Ou altere temporariamente em `launchSettings.json` (perfil `ProCienciaWeb` → `applicationUrl`).

Para ver o que usa a porta (PowerShell):

```powershell
netstat -ano | findstr :5001
```

---

### 6.4 API Azure offline ou inacessível

**Sintoma:** `/listaprojetos` com erro, exceção em log, ou página em branco após "Carregando...".

**Causa:** `ApiService` chama `https://apiprociencia.azurewebsites.net` sem fallback local.

**Verificação:**

```powershell
curl.exe -I https://apiprociencia.azurewebsites.net/api/Projetos
```

Se falhar (timeout, SSL, 5xx), o front não lista projetos até a API voltar.

**Mitigações:**

- Verificar VPN/proxy/firewall corporativo.
- Testar a URL no navegador ou Postman.
- Em desenvolvimento futuro: apontar API local via `appsettings.Development.json` (ainda não implementado no código).

**Validação (2026-05-23):** neste ambiente, `GET /listaprojetos` retornou **500** quando a API não respondeu; `GET /` permaneceu **200**.

---

### 6.5 Erro SSL ao restaurar pacotes (npm / corporativo)

**Sintoma:** `UNABLE_TO_VERIFY_LEAF_SIGNATURE` em ferramentas Node (não afeta `dotnet restore` diretamente).

**Contexto:** comum com proxy/antivírus. Para MCPs do Cursor, ver [`FASE-0-GUIA-PENDENCIAS.md`](./FASE-0-GUIA-PENDENCIAS.md).

**Para dotnet:** em geral `dotnet restore` e `dotnet build` funcionam independentemente do npm.

---

### 6.6 Avisos CS1998 no build

**Sintoma:** avisos em `ListaProjetos.razor` / `EditarProjeto.razor` — async sem await.

**Impacto:** não bloqueia build nem run.

**Correção futura:** implementar `ExcluirProjeto` / `AlterarProjeto` ou remover `async` onde não houver await.

---

### 6.7 Blazor circuit disconnected

**Sintoma:** mensagem para recarregar a página após idle ou perda de WebSocket.

**Solução:** recarregar (`F5`). Evitar colocar proxy que bloqueie WebSocket em `/_blazor`.

---

## 7. Variáveis de ambiente

| Variável | Valor (Development) | Onde definido |
|----------|---------------------|---------------|
| `ASPNETCORE_ENVIRONMENT` | `Development` | `launchSettings.json` |
| `DOTNET_CLI_TELEMETRY_OPTOUT` | `1` (opcional) | Desativa telemetria da CLI |

Definir no PowerShell (sessão atual):

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
```

---

## 8. Estrutura de comandos (resumo)

```powershell
# Na raiz do clone
cd "d:\Projetos\ProCiencia\ProCienciaWeb"

dotnet --list-sdks          # conferir 3.1.x
dotnet restore ProCienciaWeb.sln
dotnet build ProCienciaWeb.sln
dotnet run --project ProCienciaWeb\ProCienciaWeb.csproj

# Navegador
# https://localhost:5001
# https://localhost:5001/listaprojetos
```

---

## 9. Validação deste guia

| Passo | Data | Resultado |
|-------|------|-----------|
| `dotnet restore ProCienciaWeb.sln` | 2026-05-23 | OK |
| `dotnet build ProCienciaWeb.sln` | 2026-05-23 | OK (0 erros) |
| `dotnet run` | 2026-05-23 | OK — escuta 5000/5001 |
| `GET https://localhost:5001/` | 2026-05-23 | HTTP 200 |
| `GET http://localhost:5000/` | 2026-05-23 | HTTP 307 (redirect HTTPS) |
| `GET https://localhost:5001/listaprojetos` | 2026-05-23 | HTTP 500 se API indisponível |

**SDK usado na validação:** 3.1.301  
**SO:** Windows 10/11  
**Repositório:** `d:\Projetos\ProCiencia\ProCienciaWeb`

---

## 10. Próximos passos

- **Fase 5:** `docs/PRD.md` — requisitos de produto.
- Melhorias de setup futuras: URL da API em `appsettings.Development.json`, perfil Docker, script `dev.ps1`.
