# Fase 0 — Guia das ações pendentes

Checklist para concluir a ativação dos MCPs após a configuração inicial.

---

## Diagnóstico (2026-05-22)

| Problema | Causa | Correção aplicada / necessária |
|----------|-------|--------------------------------|
| Playwright e Azure MCP falham | Certificado SSL do npm (`UNABLE_TO_VERIFY_LEAF_SIGNATURE`) | `npm config set strict-ssl false` |
| Playwright exige Node ≥ 18 | `npx` usava Node **v16.20.2** em `C:\Program Files\nodejs` | MCPs atualizados para Node **v22** do Cursor |
| GitHub MCP com erro | Variável `GITHUB_PAT` **não definida** | Criar PAT e definir variável (passo 2) |
| MCPs não recarregados | Cursor aberto antes das alterações | Reiniciar Cursor (passo 1) |

---

## Passo 1 — Reiniciar o Cursor

1. Salve todos os arquivos abertos.
2. Feche o Cursor completamente (File → Exit ou Alt+F4).
3. Abra o Cursor de novo na pasta `d:\Projetos\ProCiencia\ProCienciaWeb`.
4. Vá em **Settings → Tools & Integrations → MCP**.
5. Confira o **ponto verde** ao lado de cada servidor:
   - `github`
   - `playwright`
   - `Azure MCP Server`

**Atalho:** `Ctrl+Shift+P` → digite `MCP: View Server Status`.

Se algum servidor continuar vermelho, veja os passos 2 e 3 abaixo.

---

## Passo 2 — Configurar `GITHUB_PAT` (GitHub MCP)

### 2.1 Criar o token

1. Acesse: https://github.com/settings/personal-access-tokens/new
2. **Fine-grained token** (recomendado) ou **Classic token**:
   - Nome: `cursor-mcp-prociencia`
   - Expiração: 90 dias (ou conforme política da org)
   - Repositório: `ProCienciaWeb` (ou todos que usar)
   - Permissões mínimas sugeridas:
     - **Contents:** Read
     - **Issues:** Read and write (se for usar issues via MCP)
     - **Pull requests:** Read and write (se for usar PRs via MCP)
3. Gere o token e **copie** (só aparece uma vez).

Documentação oficial: https://github.com/github/github-mcp-server/blob/main/docs/installation-guides/install-cursor.md

### 2.2 Definir a variável no Windows

**Opção A — PowerShell (permanente para seu usuário):**

```powershell
# Substitua ghp_xxxxxxxx pelo token real
[Environment]::SetEnvironmentVariable('GITHUB_PAT', 'ghp_xxxxxxxx', 'User')
```

**Opção B — Interface gráfica:**

1. `Win + R` → `sysdm.cpl` → Enter
2. Aba **Avançado** → **Variáveis de Ambiente**
3. Em **Variáveis do usuário** → **Novo**
4. Nome: `GITHUB_PAT` | Valor: seu token
5. OK em todas as janelas

### 2.3 Validar

Abra um **novo** PowerShell (importante: nova sessão):

```powershell
[Environment]::GetEnvironmentVariable('GITHUB_PAT', 'User')
# Deve exibir o token (ou pelo menos não vazio)
```

Reinicie o Cursor novamente após definir a variável.

### 2.4 Testar no chat

No Composer/Agent, peça:

> Liste meus repositórios do GitHub

Se o MCP estiver OK, o agente usará ferramentas do servidor `github`.

---

## Passo 3 — SSL do npm (Playwright + Azure)

### O que já foi feito

Foi executado:

```powershell
npm config set strict-ssl false
```

Isso permite que `npx` baixe pacotes mesmo com certificado interceptado (proxy/antivírus corporativo).

### Validar

```powershell
npm config get strict-ssl
# Esperado: false

npx -y @playwright/mcp@latest --version
# Esperado: Version 0.0.x
```

### Alternativa mais segura (quando tiver certificado da empresa)

Se a TI fornecer um arquivo `.pem` da CA corporativa:

```powershell
npm config set strict-ssl true
npm config set cafile "C:\caminho\para\corporate-ca.pem"
```

---

## Passo 4 — Node.js (recomendação de longo prazo)

Hoje existem **duas** instalações de Node:

| Caminho | Versão | Uso |
|---------|--------|-----|
| Cursor (`...\cursor\...\helpers\node.exe`) | 22.22.0 | MCPs (config atual) |
| `C:\Program Files\nodejs\node.exe` | 16.20.2 | `npm` / `npx` no terminal |

**Recomendado:** instalar [Node.js 22 LTS](https://nodejs.org/) e substituir a v16 em `Program Files\nodejs`. Depois disso, os MCPs podem voltar a usar `"command": "npx"` simples.

---

## Passo 5 — Context7 (opcional)

Não é obrigatório para a Fase 0. Se quiser documentação .NET atualizada nas Fases 4–6:

1. Registre-se em https://context7.com
2. Adicione o servidor conforme a documentação do provedor em `~/.cursor/mcp.json`

---

## Checklist final

- [ ] Cursor reiniciado
- [ ] `GITHUB_PAT` definido e Cursor reiniciado de novo
- [ ] `npm config get strict-ssl` → `false` (ou `cafile` configurado)
- [ ] MCP `github` com ponto verde
- [ ] MCP `playwright` com ponto verde
- [ ] MCP `Azure MCP Server` com ponto verde

Quando todos estiverem verdes, a Fase 0 está **100% operacional** e você pode iniciar a **Fase 1**.
