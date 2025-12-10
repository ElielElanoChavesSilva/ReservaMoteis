# 📘 Book Motels – Projeto Fullstack  
API em **.NET 6** com **Clean Architecture**, **SQLite**, **Redis Cache** e Frontend em **Angular**.

## 📌 Sumário
- [Visão Geral](#visão-geral)  
- [Arquitetura](#arquitetura)  
- [Tecnologias](#tecnologias)  
- [Setup do Projeto](#setup-do-projeto)  
- [Variáveis de Ambiente](#variáveis-de-ambiente)  
- [Executando com Docker](#executando-com-docker)  
- [Endpoints Principais](#endpoints-principais)  
- [Credenciais de Teste](#credenciais-de-teste)  
- [Estrutura do Projeto](#estrutura-do-projeto)  
- [Contato](#contato)  

---

## 🚀 Visão Geral

Este projeto é um sistema para gerenciamento de motéis, contendo:

- Cadastro e edição de motéis  
- Cadastro e gerenciamento de suítes  
- Sistema de reservas  
- Autenticação e autorização por perfis (**Admin/User**)  
- Cache de dados usando Redis  
- Frontend totalmente integrado feito em Angular  

O objetivo é demonstrar domínio de backend com .NET 6 e frontend com Angular, utilizando arquitetura limpa e boas práticas.

---


## 🏛️ Arquitetura

A aplicação segue **Clean Architecture**, separando regras de negócio da infraestrutura.


### Camadas principais:
### Serviços utilizados:

- **SQLite** (Banco principal – arquivo `main.db`)  
- **Redis** (Cache para otimizar consultas)  
- **Docker** (Orquestração dos serviços)  
- **Angular 16+** (Frontend SPA)  
- **JWT Authentication** (Autenticação)

---

## 🛠️ Tecnologias

### Backend
- .NET 6  
- Entity Framework Core (SQLite)  
- Clean Architecture  
- Redis Cache  
- Swagger  
- JWT Authentication  

### Frontend
- Angular  
- Angular Material  
- Interceptors de autenticação  

### DevOps
- Docker & Docker Compose

---

## ⚙️ Setup do Projeto

### 1. Clone o repositório  
```bash
git clone https://github.com/ElielElanoChavesSilva/ReservaMoteis.git
cd ReservaMoteis
