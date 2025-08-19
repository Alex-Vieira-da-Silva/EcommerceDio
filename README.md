# 📦 Plataforma de E-commerce com Arquitetura de Microserviços

Este projeto tem como objetivo o desenvolvimento de uma aplicação escalável e modular para gerenciamento de estoque e vendas em uma plataforma de e-commerce, utilizando arquitetura de microserviços.

## 🧠 Visão Geral da Arquitetura

A solução é composta por dois microserviços principais, integrados por um API Gateway e comunicando-se de forma assíncrona via RabbitMQ. A autenticação é realizada por meio de tokens JWT, garantindo segurança e controle de acesso.

### Componentes Principais

| Componente                    | Responsabilidades                                                                |
|-------------------------------|----------------------------------------------------------------------------------|
| **Client**                    | Interface de interação com o sistema (usuário ou sistema externo).               |
| **API Gateway**               | Autenticação via JWT e roteamento de requisições para os microserviços.          |
| **Microserviço de Vendas**    | Criação e consulta de pedidos, validação de estoque, envio de notificações.      |
| **Microserviço de Estoque**   | Cadastro, atualização e validação de produtos, persistência em banco relacional. |
| **RabbitMQ**                  | Broker de mensagens para comunicação assíncrona entre os serviços.               |
| **Banco de Dados Relacional** | Armazenamento estruturado de dados de produtos e estoque.                        |

### Fluxo de Comunicação

1. O cliente realiza uma requisição via API Gateway.
2. O Gateway autentica a requisição utilizando JWT.
3. A requisição é roteada para o microserviço correspondente (vendas ou estoque).
4. O microserviço de vendas consulta o estoque para validar disponibilidade.
5. Após a confirmação, o pedido é criado e uma notificação é enviada via RabbitMQ.
6. O microserviço de estoque atualiza os dados conforme necessário.
7. Todas as informações são persistidas em um banco de dados relacional.

## 🛠️ Tecnologias Utilizadas

- **.NET Core** – Framework principal para desenvolvimento dos microserviços.
- **C#** – Linguagem de programação.
- **Entity Framework** – ORM para abstração de acesso ao banco de dados.
- **RESTful API** – Padrão de comunicação entre serviços.
- **RabbitMQ** – Mensageria para comunicação assíncrona.
- **JWT (JSON Web Token)** – Autenticação segura.
- **Banco de Dados Relacional** – Persistência de dados estruturados.


## 📌 Objetivos Estratégicos

- Garantir escalabilidade horizontal e independência entre serviços.
- Promover segurança e integridade dos dados.
- Facilitar manutenção e evolução contínua da plataforma.
- Adotar boas práticas de arquitetura distribuída e mensageria.



