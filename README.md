# ShootsyApp - Найди себе коллегу по жизни йобана! 🎯

**Date-приложение для IT-специалистов от IT-специалистов**

> Open-source проект для знакомств в IT-сообществе. Без монетизации, только развитие и практика! 

## 🌟 О проекте

ShootsyApp - это open-source dating приложение, созданное IT-специалистами для IT-сообщеста. Мы верим, что лучшие проекты рождаются из страсти к технологиям и желания помогать друг другу.

**Наша философия:**
- 🚫 Без монетизации и донатов
- 💝 Создано сообществом для сообщества  
- 🛠️ Платформа для практики и развития
- 🔓 Открытый API и код

## 👥 Для кого этот проект?

### 🧪 Тестировщики
- **Manual QA**: Практика тестирования REST API, Kafka
- **Automation QA**: Написание автотестов на реальном проекте
- **Тест-аналитики**: Разработка тестовой документации

### 💻 Разработчики
- **Backend**: Практика с ASP.NET Core, Entity Framework, Kafka
- **Frontend**: Разработка UI/UX (ждем ваших предложений!)
- **DevOps**: Настройка CI/CD, Docker

### 🎨 Другие специалисты
- **Аналитики**: Проектирование функциональности
- **Дизайнеры**: Создание UI/UX интерфейсов
- **Product Manager'ы**: Развитие продукта

## 🚀 Технологии

**Backend:**
- ASP.NET Core 8.0
- Entity Framework Core
- PostgreSQL / MongoDB
- JWT Authentication
- xUnit / NUnit

**Инфраструктура:**
- Docker
- MinIO (S3-совместимое хранилище)
- Kafka

# Ссылки на развернутый стенд: 
* Ссылка на Swagger: http://45.130.147.122:5000/swagger/index.html
* Ссылка на Kafka UI: http://45.130.147.122:8080/ui/clusters/local/all-topics
* Ссылка на MongoUI: http://45.130.147.122:8081/db/shootsy/

## Строки подключения к БД:
* MongoDB: "mongodb://admin:admin@mongoDb:27017/shootsy?authSource=admin"
* PostgresDB: "Host=shootsy-pg-15;Port=5432;Database=testdb;Username=postgres;Password=postgres"

## Kafka
* "BootstrapServers": "kafka:9092"
* "SecurityProtocol": "PLAINTEXT"

### Креды к Minio сможете узнать в файле проекта `appsettings.json`

Контакты для связи:
* Telegram: [@kravec_tv](https://t.me/kravectv)
* Email: kravec.mik@ya.ru

# Документация API

### Контроллеры
- [UsersController](Shootsy/Docs/UsersController/UsersController.md) - Управление пользователями
- [MessagesController](Shootsy/Docs/MessagesController/MessagesController.md) - Отправка сообщений
- [FilesController](Shootsy/Docs/FilesController/FilesController.md) - Управление файлами
- [PostsController](Shootsy/Docs/PostsController/PostsController.md) - Публикации, комментарии и лайки

## Аутентификация: 
Производится через добавление хэдера "Session" в запросы

## Контроль доступа к операциям изменения и удаления

Доступ к операциям обновления и удаления данных осуществляется путем:
1. Получения идентификатора текущего пользователя из сессии
2. Сравнения его с идентификатором владельца целевой записи
3. Разрешения операции только при совпадении идентификаторов

При попытке доступа к чужим данным возвращается статус **403 Forbidden**.

## Технологии
- **База данных:** MongoDB, PostgreSQL
- **Хранилище файлов:** MinIO (S3-совместимое)
- **Брокер сообщений:** Kafka
- **Аутентификация:** Session header

## Перечисления:

### CityEnum

|ID	|Значение	|
|:---------:|:---------:|
|1	|Новосибирск|	
|2	|Барнаул|	
|3	|Москва|	

### GenderEnum

|ID	|Значение	|
|:---------:|:---------:|
|1	|Мужской|	
|2	|Женский|	

### ProfessionEnum

| ID | Значение | Описание |
|----|-----------|-----------|
| 1 | QA | Quality Assurance Engineer |
| 2 | AQA | Automated Quality Assurance |
| 3 | ManualQA | Manual Quality Assurance |
| 4 | SDET | Software Development Engineer in Test |
| 5 | QALead | Quality Assurance Lead |
| 10 | FrontendDeveloper | Frontend Developer |
| 11 | BackendDeveloper | Backend Developer |
| 12 | FullStackDeveloper | Full Stack Developer |
| 13 | WebDeveloper | Web Developer |
| 14 | MobileDeveloper | Mobile Developer |
| 15 | iOSDeveloper | iOS Developer |
| 16 | AndroidDeveloper | Android Developer |
| 17 | GameDeveloper | Game Developer |
| 20 | DevOps | DevOps Engineer |
| 21 | SystemAdministrator | System Administrator |
| 22 | NetworkEngineer | Network Engineer |
| 23 | SecurityEngineer | Security Engineer |
| 24 | SiteReliabilityEngineer | Site Reliability Engineer |
| 25 | CloudEngineer | Cloud Engineer |
| 30 | DataScientist | Data Scientist |
| 31 | DataAnalyst | Data Analyst |
| 32 | DataEngineer | Data Engineer |
| 33 | MachineLearningEngineer | Machine Learning Engineer |
| 34 | BIAnalyst | Business Intelligence Analyst |
| 40 | ProjectManager | Project Manager |
| 41 | ProductManager | Product Manager |
| 42 | ProductOwner | Product Owner |
| 43 | ScrumMaster | Scrum Master |
| 44 | TeamLead | Team Lead |
| 45 | CTO | Chief Technology Officer |
| 46 | HeadOfDevelopment | Head of Development |
| 50 | UXUIDesigner | UX/UI Designer |
| 51 | UIDesigner | UI Designer |
| 52 | UXDesigner | UX Designer |
| 53 | GraphicDesigner | Graphic Designer |
| 54 | ProductDesigner | Product Designer |
| 60 | HR | Human Resources |
| 61 | HRBP | HR Business Partner |
| 62 | ITRecruiter | IT Recruiter |
| 63 | TalentAcquisition | Talent Acquisition Specialist |
| 64 | TechnicalRecruiter | Technical Recruiter |
| 70 | BusinessAnalyst | Business Analyst |
| 71 | SystemAnalyst | System Analyst |
| 72 | ProductAnalyst | Product Analyst |
| 73 | TechnicalAnalyst | Technical Analyst |
| 80 | SolutionArchitect | Solution Architect |
| 81 | SystemArchitect | System Architect |
| 82 | EnterpriseArchitect | Enterprise Architect |
| 83 | TechnicalArchitect | Technical Architect |
| 90 | TechSupport | Technical Support |
| 91 | ITHelpdesk | IT Helpdesk |
| 92 | SupportEngineer | Support Engineer |
| 93 | CustomerSupport | Customer Support |
| 100 | DatabaseAdministrator | Database Administrator |
| 101 | DBA | Database Administrator (DBA) |
| 102 | DatabaseDeveloper | Database Developer |
| 110 | SoftwareEngineer | Software Engineer |
| 111 | ResearchEngineer | Research Engineer |
| 112 | PerformanceEngineer | Performance Engineer |
| 113 | IntegrationEngineer | Integration Engineer |
| 120 | TechnicalWriter | Technical Writer |
| 121 | ContentManager | Content Manager |
| 122 | SEOSpecialist | SEO Specialist |
| 123 | ITMarketer | IT Marketer |
| 999 | Other | Другая профессия |
