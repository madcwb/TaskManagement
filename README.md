# Passo a Passo para Executar o Projeto no Docker

Este projeto foi configurado para ser executado no Docker utilizando o `docker-compose`. A seguir, estão as instruções para configurar e executar o ambiente de desenvolvimento, além de acessar a aplicação ao final do processo.

#### 1. **Instale o Docker e Docker Compose**
   Antes de começar, certifique-se de ter o Docker e o Docker Compose instalados no seu sistema. Você pode baixar o Docker [aqui](https://www.docker.com/products/docker-desktop) para seu sistema operacional.

#### 2. **Clone o Repositório do Projeto**
   Faça o clone do repositório do projeto em seu ambiente local. Se ainda não tiver o código, você pode fazer isso com o seguinte comando:

   ```bash
   git clone https://github.com/madcwb/TaskManagement.git
   ```

   Em seguida, navegue até o diretório do projeto.


#### 3. **Estrutura do `docker-compose.yml`**
   O arquivo `docker-compose.yml` foi configurado para subir tanto o **SQL Server** quanto a aplicação **ASP.NET Core**. Ele utiliza os seguintes serviços:

   - **SQL Server**: Banco de dados onde as informações do projeto serão armazenadas.
   - **Aplicação ASP.NET Core**: A API do projeto.


#### 4. **Comando para Iniciar o Docker Compose**
   Uma vez que o projeto esteja baixado e o Docker instalado, execute o comando abaixo para construir e iniciar os containers:

   ```bash
   docker-compose up --build
   ```

   Isso iniciará dois containers: um para o SQL Server e outro para a aplicação ASP.NET Core.

#### 5. **Acessando a Aplicação**
   Uma vez que os containers estiverem rodando, a aplicação estará disponível na URL abaixo:

   ```
   http://localhost:8080/swagger
   ```

   Você pode abrir esta URL em seu navegador para acessar a documentação da API gerada pelo Swagger. A partir dessa interface, você pode testar todos os endpoints da API.


# Fase 2: Refinamento
Sugestões de perguntas para o PO:

### Funcionalidades:
Quais tipos de relatórios de desempenho adicionais seriam úteis (e.g., tempo médio para conclusão de tarefas, tarefas mais demoradas)?
Existe a necessidade de implementar um sistema de notificações para lembrar os usuários de prazos e tarefas pendentes?
Há interesse em adicionar funcionalidades de colaboração mais avançadas, como atribuição de tarefas a diferentes usuários e discussões em grupo?
É necessário implementar algum tipo de filtro ou busca avançada para as tarefas?

### Escalabilidade:
Qual é a estimativa máxima de usuários e projetos que o sistema precisará suportar?
Quais são os requisitos de performance em termos de tempo de resposta para as operações mais comuns?

### Segurança:
Quais são os requisitos de segurança e privacidade dos dados?
É necessário implementar algum tipo de controle de acesso para limitar o que os usuários podem ver e fazer?

# Fase 3: Final
Possíveis melhorias e pontos de atenção:

### Arquitetura:
Microsserviços: Considerar a divisão do sistema em microsserviços para maior escalabilidade e independência dos componentes.
Event Sourcing: Utilizar o padrão Event Sourcing para rastrear todas as mudanças no sistema, facilitando a auditoria e a reconstrução de estados anteriores.

### Cache: 
Implementar um cache para armazenar dados frequentemente acessados e reduzir a carga no banco de dados.

### Testes:
Testes de performance: Realizar testes de carga e stress para avaliar a capacidade do sistema de lidar com um grande volume de requisições.

### Segurança:
Autenticação e autorização: Implementar mecanismos robustos de autenticação e autorização para proteger os dados dos usuários.
Realizar varreduras de segurança para identificar e corrigir possíveis vulnerabilidades.

### Visão de futuro sobre arquitetura/cloud:
Serverless: Considerar a utilização de funções serverless para executar partes específicas do sistema, reduzindo custos e simplificando a gestão.


