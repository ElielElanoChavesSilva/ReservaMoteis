# 📘 Reserva Moteis – Projeto Fullstack

API em **.NET 6** com **Clean Architecture**, **SQLite**, **Redis Cache** e Frontend em **Angular**.

## 📌 Sumário

* [Visão Geral](#visão-geral)
* [Arquitetura](#arquitetura)
* [Tecnologias](#tecnologias)
* [Setup do Projeto](#setup-do-projeto)
* [Executando com Docker](#executando-com-docker)
* [Credenciais de Teste](#credenciais-de-teste)
* [Estrutura do Projeto Backend](#estrutura-do-projeto-backend)

---

## 🚀 Visão Geral

Este projeto é um sistema para gerenciamento de motéis, contendo:

* Cadastro e edição de motéis
* Cadastro de suítes
* Sistema de reservas
* Autenticação e autorização por perfis (**Admin/User**)
* Cache de dados usando Redis
* Frontend totalmente integrado feito em Angular

O objetivo é demonstrar domínio de backend com .NET 6 e frontend com Angular, utilizando arquitetura limpa e boas práticas.

---

## 🏛️ Arquitetura

A aplicação segue **Clean Architecture**, separando regras de negócio da infraestrutura.

### Camadas principais:
* API
* Application
* Domain
* Infrastructure
### Serviços utilizados:

* **SQLite** (Banco principal – arquivo `main.db`)
* **Redis** (Cache para otimizar consultas)
* **Docker** (Orquestração dos serviços)
* **Angular 16+** (Frontend SPA)
* **JWT Authentication** (Autenticação)

---

## 🛠️ Tecnologias

### Backend

* .NET 6
* Entity Framework Core (SQLite)
* Clean Architecture
* Redis Cache
* Swagger
* JWT Authentication
* Xunit
  
### Frontend

* Angular
* Angular Material
* Interceptors de autenticação

### DevOps

* Docker & Docker Compose

## 🔄 Integração Contínua (GitHub Actions)

Este projeto utiliza **GitHub Actions** para Integração Contínua (CI).

A cada `push` na branch `main`, o pipeline executa automaticamente:

- Restore das dependências do backend (.NET)
- Build do projeto
- Execução de testes automatizados

O workflow está definido em:
.github/workflows/ci.yml

---

## ⚙️ Setup do Projeto

### 🧑‍💻 1. Clone o repositório

```bash
git clone https://github.com/ElielElanoChavesSilva/ReservaMoteis.git
cd ReservaMoteis
```

### 🐳 2. Execute tudo com Docker

A forma mais rápida de iniciar o projeto é utilizando o **Docker**, pois ele sobe automaticamente:

* API (.NET 6)
* Redis
* Frontend Angular
* SQLite (arquivo `main.db` em volume compartilhado)

Para iniciar todos os serviços:

```bash
docker-compose up -d --build
```

Para derrubar tudo:

```bash
docker-compose down
```

### 🛠️ 3. Configure a API

```bash
cd .\Backend\ReservaMoteisAPI

dotnet restore
```

Por fim, execute:
```bash
dotnet run --project ../ReservaMoteisAPI
```


* O arquivo `appsettings.Development.json` já contém configurações padrão para SQLite e Redis.
* Certifique-se de que o Redis esteja rodando localmente na porta **6379** e a API na **44310**.

### 3. Configure o Frontend

* Certifique-se de usar no mínimo a versão 20.X do Node.js

```bash
cd /Frontend/
npm install
```

### 4. Execute o frontend

```bash
ng serve
```

---

## 🔐 Credenciais de Teste

### **Admin**

* **Email:** [eliel@gmail.com]
* **Senha:** eliel

### **Usuário comum**

* **Email:** [silva@gmail.com]
* **Senha:** silva

---

## 📂 Estrutura do Projeto Backend

```bash
src/Backend/
│── BookMotelsAPI       
│── BookMotelsApplication
│── BookMotelsDomain      
│── BookMotelsInfrastructure 


