# Tic-Tac-Toe REST API

## Описание проекта

Это REST API для игры в крестики-нолики с размером поля NxN (N ≥ 3) для двух игроков. 
Особенность игры — каждый третий ход с вероятностью 10% ставится знак противника.

---

## Технологии и решения

- **Платформа:** .NET 9
- **База данных:** PostgreSQL
- **API:** REST с передачей и получением данных в формате JSON
- **Версии:** Использован ETag для обеспечения идемпотентности ходов
- **Concurrency:** При одновременных запросах на один ход повторный запрос возвращает 200 OK с тем же ETag
- **Сохранение состояния:** Все игры и их состояния сохраняются в базе данных, что обеспечивает сохранность между рестартами сервиса
- **Валидация:** Проверка корректности запросов и возврат ошибок в формате RFC 7807 для некорректных JSON и бизнес-ошибок
- **Тесты:** Unit и интеграционные тесты с покрытием более 30%
- **Docker:** Приложение собирается в Docker контейнер и запускается через docker-compose, слушает порт 8080

---

## Архитектурные решения

Проект построен с соблюдением принципов чистой архитектуры и SOLID.

### Слои проекта

- **Core:** сущности и бизнес-правила (Entities, Enums, Interfaces, Exceptions, Helpers, etc)
- **Application:** бизнес-логика, use-cases
- **Infrastructure:** реализация хранилища PostgreSQL и сервисов для взаимодействия с данными
- **API:** контроллеры, маппинг, валидация, ответы
---

## API Описание

### Здоровье сервиса

`GET /health`

- Проверка доступности сервиса
- **Ответ:** 200 OK

---

### Создание новой игры

`POST /games`

**Тело запроса:**

```json
{
  "firstPlayer" : "Sarah",
  "secondPlayer" : "Alex",
  "currentTurn" : "Alex"
}
```

**Ответы**

### При успешном создании игры:

- **Ответ:** 201 Created

```json
{
  "id" : "46c5c7fc-e756-4e0d-b15c-38ed754d7f36"
}
```

### При некорректном вводе данных

- **Ответ:** 400 Bad Request
- **Дополнительно:** Сообщение c указанием некорректно введенных данных

---

### Получение данных об игры

`GET /games/{gameId}`

**Параметры запроса**
- **showAsBoard:** отвечает за отображение доски в виде массива

**Ответы**

### При успешном выполнении запроса:

- **Ответ:** 200 Ok

```json
{
  "game": {
    "id": "8f904fab-6f29-4f41-88ad-1a4840b69db3",
    "firstPlayer": "Kasandra",
    "firstPlayerSymbol": "X",
    "secondPlayer": "Sarag",
    "secondPlayerSymbol": "O",
    "currentTurn": "Sarag",
    "boardSize": 4,
    "winLength": 3,
    "whoWon": "Kasandra",
    "createdAt": "2025-07-23T13:35:40.798685",
    "etag": "404F2818",
    "isFinished": true,
    "moves": [
      {
        "id": "56e3bedc-3187-421c-bf21-2a2cd143964a",
        "player": "Kasandra",
        "x": 0,
        "y": 0
      },
      {
        "id": "6ab8d5aa-8c81-4072-bdad-5108515dddc4",
        "player": "Sarag",
        "x": 0,
        "y": 1
      },
      {
        "id": "5c1461cd-9dda-49f3-a969-819bff9fc9e3",
        "player": "Kasandra",
        "x": 1,
        "y": 1
      },
      {
        "id": "5960fcb5-2ad6-443a-a4c9-8a8d210bbd72",
        "player": "Sarag",
        "x": 2,
        "y": 1
      },
      {
        "id": "feff2264-fd8b-49fb-9474-4487ec5667b4",
        "player": "Kasandra",
        "x": 2,
        "y": 2
      },
      {
        "id": "ece44382-0534-4771-aa13-be173c55e9b7",
        "player": "Sarag",
        "x": 3,
        "y": 2
      },
      {
        "id": "0cec5c15-3762-409d-a05b-a2431ec5ff39",
        "player": "Kasandra",
        "x": 3,
        "y": 3
      }
    ],
    "board": [
      ["X", " ", " ", " "],
      ["O", "X", "O", " "],
      [" ", " ", "X", "O"],
      [" ", " ", " ", "X"]
    ]
  }
}
```

### Если игра не была найдена

- **Ответ:** 404 Not Found
- **Дополнительно:** Сообщение c указанием некорректно введенных данных

---

### Создание хода в игре

`POST /games/{gameId}/moves`

**Тело запроса:**

```json
{
  "player" : "Kasandra",
  "row" : "5",
  "column" : "3"
}
```

**Ответы**

### При успешном выполнении запроса:

- **Ответ:** 200 Ok

```json
{
    "moveResult": {
        "value": {
            "id": "72049d88-fdbf-4493-a34b-9e1d3999c8b7",
            "player": "Alex",
            "x": 2,
            "y": 1
        },
        "isWonMove": false,
        "isGameEnd": false
    }
}
```
### Если игра не была найдена

- **Ответ:** 404 Not Found
- **Дополнительно:** Сообщение c указанием некорректно введенных данных

### При некорректном вводе данных

- **Ответ:** 400 Bad Request
- **Дополнительно:** Сообщение c указанием некорректно введенных данных

---

## Запуск и тестирование

### Запуск
```
docker-compose up --build
```
- API будет доступен на: http://localhost:8080


### Тесты
```
dotnet test
```

---

## Переменные окружения

```
GAME_BOARD_SIZE=4
GAME_WIN_LENGTH=3
```

## AppSettings

Конфигурация приложения задаётся через файл appsettings.json

Пример **appsettings.json:**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Database=tictactoe;Username=postgres;Password=postgres"
  }
}
```