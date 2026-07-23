# Order System - FIX Protocol

Aplicação desenvolvida para o challenge da Coodesh que implementa a comunicação entre dois serviços utilizando o protocolo FIX 4.4 para processamento de ordens financeiras.

## Tecnologias

### Backend

* .NET 8
* ASP.NET Core Web API
* C#
* QuickFIX/n (FIX 4.4)
* Entity Framework Core (InMemory)
* MediatR
* FluentValidation

### Frontend

* Angular 22
* Angular Material
* TypeScript

### Infraestrutura

* Docker
* Dev Containers

## Arquitetura

O projeto é composto por três aplicações:

* **OrderGenerator**: API responsável por receber requisições HTTP, criar mensagens `NewOrderSingle` e enviá-las ao OrderAccumulator através do protocolo FIX.
* **OrderAccumulator**: Serviço responsável por receber mensagens FIX, calcular a exposição financeira por símbolo e responder com um `ExecutionReport`.
* **Frontend Angular**: Interface para envio de ordens e visualização do resultado do processamento.

Atualmente a persistência utiliza o provedor **Entity Framework Core InMemory** para simplificar a execução do projeto.

## Estrutura do Projeto

```text
src/
├── OrderGenerator.Api
├── OrderGenerator.Application
├── OrderGenerator.Domain
├── OrderGenerator.Infrastructure
├── OrderAccumulator.Api
├── OrderAccumulator.Application
├── OrderAccumulator.Domain
└── OrderAccumulator.Infrastructure

frontend/
└── order-frontend
```

## Funcionalidades

* Comunicação entre aplicações utilizando FIX 4.4
* Envio de mensagens `NewOrderSingle`
* Recebimento de `ExecutionReport`
* Cálculo de exposição financeira por símbolo
* Validação do limite de exposição de R$ 100.000.000
* Aceitação ou rejeição de ordens
* Interface web para envio de ordens

## Como executar

### Pré-requisitos

* Docker
* Visual Studio Code
* Extensão Dev Containers
* .NET SDK 8
* Node.js 24+
* Angular CLI

### 1. Abrir o projeto no Dev Container

Abra o projeto utilizando a extensão **Dev Containers** do VS Code.

### 2. Executar o OrderAccumulator

```bash
cd src/OrderAccumulator.Api
dotnet run
```

### 3. Executar o OrderGenerator

```bash
cd src/OrderGenerator.Api
dotnet run
```

A API estará disponível em:

```
http://localhost:5000
```

### 4. Executar o Frontend

```bash
cd frontend/order-frontend
npm install
ng serve
```

O frontend ficará disponível em:

```
http://localhost:4200
```

---

> This is a challenge by [Coodesh](https://coodesh.com/)
