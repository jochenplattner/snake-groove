# 🎮 Snake Groove — Unified Architecture Specification (v12)

> **Статус:** актуально для Unity 6.2 LTS, Visual Studio 2026, .NET 10 LTS
> **Фаза проекта:** завершена Фаза 0, начинается Фаза 1 (MVP)
> **Связанные документы:** [Фаза 0 — итоговый отчёт](./snake_phase_0_summary.md)
 (v12)

> **Главный архитектурный документ проекта Snake Groove**

---

# 🟦 0. Motivation, Principles & Learning

## 0.1 Motivation During the Process

- Каждое действие связывается с большой картиной развития Snake Groove.
- Проект — тренировочная площадка разработки архитектуры, паттернов, CI/CD.
- Формируем привычку фиксировать маленькие победы.

## 0.2 Motivational Vision

Создать **музыкальную, динамичную и мультяшную версию Snake**, которая:

- Добавляет музыкальный **аудио‑слой** за каждую съеденную еду.
- Имеет мягкую, мультяшную графику.
- Строится по принципам **Clean Architecture**.
- Готова к масштабированию, публикации и монетизации.
- Идеи для расширения: 
   - на двоих играть (vs-, coop-mode) : 
      кто больше очков наберёт, 
	  у каждого свой дисплей с собранными вещами
   - двигающийся экран / сплит скрин и т.д.
   - прыжок через клетку или несколько (потом всё тело за головой прыгает)
   - сжатие тела в точку, остановка, движение дальше в каком-либо направлении  

## 0.3 Principles

- Clean Architecture, SOLID, KISS, DRY, YAGNI.
- Ядро отделено от движка.
- Кроссплатформенность: Unity → Win/WebGL → Mobile.
- CI/CD: GitHub Actions + Unity Cloud Build.
- Расширяемость через `// TODO` метки.
- Устойчивость: Logging, Exception Handling.

---

# 🟦 1. Vision & Core Loop

## 1.1 Key Concept

**Аркадный Snake + музыкальный твист:**

- каждая еда добавляет новый **инструмент**;
- уровни могут иметь **свои эффекты**:
  - Горы → эхо, замедление.
  - Лес → визуальный фильтр.
  - Вода → ускорение, dampened sound.

## 1.2 MVP Phase 1 Goal

**Классический Snake с чистым ядром**:

- движение;
- еда;
- рост;
- столкновения;
- игровое поле;
- простое меню.

---

# 🟦 2. Milestone Roadmap

| Phase | Output            | Platforms       | Key Features                                |
| ----- | ----------------- | --------------- | ------------------------------------------- |
| 0     | Setup             | n/a             | Repo, CI/CD, URP project, architecture, LFS |
| 1     | Classic Snake MVP | Windows + WebGL | Меню, игра, еда, рост, коллизии             |
| 2     | Music Layers      | + Mobile        | Процедурная музыка, аудио‑миксер            |
| 3     | Advanced Gameplay | All             | Диагонали, камера, местности                |
| 4     | Monetization      | All             | IAP, скины, LiveOps                         |

Фазы ведутся в отдельных чатах. Итоги заносятся в этот документ.

---

# 🟦 3. High-Level Architecture

```
┌──────────────────────────────┐
│   📦 Core.GameLogic (.NET 8)  │
│   • Domain Entities           │
│   • Value Objects             │
│   • Services                  │
│   • Interfaces                │
│   • Events                    │
└──────────────▲───────────────┘
               │
┌──────────────┴──────────────┐      
│  Unity Adapter Layer         │
│  • SceneFlowController       │
│  • UnityGameAdapter          │
│  • Input Bridge              │
│  • Repositories              │
└──────────────▲──────────────┘
               │
┌──────────────┴──────────────┐
│     Presentation Layer       │
│  • UI (MVVM)                 │
│  • Rendering                 │
│  • Audio                     │
│  • VFX                       │
└──────────────────────────────┘
```

## 3.1 Core.GameLogic (Pure C#)

- Domain Entities: Snake, Food, Level, AudioLayer, PowerUp, Terrain
- Value Objects: GridPosition, Direction, Score, Velocity
- Services: GameLoopService, CollisionDetectionService
- Audio Domain: AudioLayerManager, MusicCompositionEngine
- Repositories: IScoreRepository, ISettingsRepository
- 100% тестируемый слой

## 3.2 Design Patterns

- Strategy (движение)
- State Machine (экраны)
- Observer/EventBus (UI/Audio)
- Factory Method (еда, усиления)
- Command (input)
- Object Pool
- Repository
- Service Locator (ограниченно)

## 3.3 Dependency Injection

- Constructor Injection
- Zenject/VContainer для Unity
- Декуплинг ядра от платформ

## 3.4 Error Handling & Logging

- Global exception boundary
- Serilog (Console, File, Seq)
- Unity Cloud Diagnostics

---

# 🟦 4. UI / UX Design

### Main Menu

- Мультяшный стиль, лёгкие переливы.
- Кнопки: Start, Settings, Exit.

### Settings

- Размер поля
- Громкость
- Тема оформления

### HUD

- Счёт
- Длина змейки
- Power‑Ups

### Visual Style

- Мягкие анимации
- Яркие цвета
- Pixel Perfect Camera

---

# 🟦 5. Toolchain & Environments

### IDE

- **Visual Studio 2026 (Stable/Insiders)** + GitHub Copilot
- Visual Studio 2022 (fallback)
- VS Code + Codex CLI (новое)
- Rider (опционально)

### Game Engine

- Unity 6.2 LTS (URP 2D)
- WebGL / Windows / Android

### Build Tools

- **.NET 10 LTS SDK** (основной для новых тулзов и сервисов)
- .NET 8 LTS SDK (совместимость с Unity)
- Unity CLI
- GitHub Actions

### SCM

- GitHub + Git LFS

---

# 🟦 6. Phase 0 Summary (Integrated)

> Подробная версия Phase 0: см. [snake_phase_0_summary.md](./snake_phase_0_summary.md)


Phase 0 Summary (Integrated)

### Installed

- Windows 11 Pro
- Unity 6.2 URP 2D Template
- VS 2022 + Copilot
- Git + Git LFS
- GitHub Desktop
- OneDrive (backup)

### Created

- Repo: `snake-groove` (MIT)
- Project structure with Assets/Scenes (Boot/MainMenu/Game)
- Basic Scripts folder structure
- LFS configured

### Prepared & verified

- Scene flow Boot → MainMenu → Game

### Remaining tasks at end of Phase 0

0. Core scripts in Unity A. Scene transitions test B. Embedded package creation C. Pure C# core D. Unity integration layer

Phase 1 начинается отсюда.

---

# 🟦 7. Full Architecture&#x20;

---

# 🎮 Snake Groove - Professional Game Architecture v11

**Аркадный Snake с музыкальным твистом**\
Clean Architecture • SOLID • TDD • CI/CD

---

## 🎯 Industry Best Practices

### 1. 🏗️ Clean Architecture

- **Описание**: Core → Adapters → Presentation
- **Детали**: Domain независим от UI/фреймворков
- **Преимущества**: Тестируемость, переиспользование, масштабируемость

### 2. ✨ SOLID Principles

- **Single Responsibility**: Каждый класс делает одну вещь
- **Open/Closed**: Открыт для расширения, закрыт для изменения
- **Liskov Substitution**: Подтипы взаимозаменяемы
- **Interface Segregation**: Клиенты не зависят от неиспользуемых методов
- **Dependency Inversion**: Зависимость от абстракций, а не конкретики

### 3. 🧪 Test-Driven Development

- **Coverage**: 80%+ для Core.GameLogic
- **Frameworks**: NUnit для unit тестов, Unity Test Framework для интеграции
- **CI/CD**: Автоматические тесты в GitHub Actions

### 4. 🔄 Dependency Injection

- **Паттерн**: Constructor injection
- **Избегаем**: Singleton hell
- **Tools**: Zenject/VContainer для Unity DI

### 5. 📊 Performance First

- **Object Pooling**: Для Food, VFX, AudioSources
- **ECS Patterns**: Для будущего масштабирования
- **Target**: 60 FPS на мобильных устройствах

### 6. 📝 Clean Code

- **Приоритет**: Читаемость > краткость
- **Documentation**: Self-documenting code, XML docs для public API
- **Standards**: Соблюдение C# coding conventions

### 7. 🔐 Error Resilience

- **Defensive Programming**: Try-catch boundaries
- **Validation**: Null checks, parameter validation
- **Logging**: Serilog для структурированного логирования

### 8. 🚀 CI/CD Automation

- **Pipeline**: Push → Build → Test → Deploy
- **Tools**: GitHub Actions, Unity Cloud Build
- **Automation**: Автоматические билды и тесты

### 9. 📈 Data-Driven Design

- **Configuration**: ScriptableObjects для настроек
- **Benefits**: Дизайнеры настраивают без кода
- **Flexibility**: Быстрые итерации и A/B тестирование

---

# 🏛️ Architecture Layers

---

# 🟦 8. AI Development Workflow

## 8.1 Goals

Использовать ИИ для:

- генерации архитектуры;
- написания кода;
- написания тестов;
- code-review;
- создания Pull Requests;
- анализа репозитория;
- работы в фоне (background agents).

## 8.2 Tools

### Codex CLI

- Локальный агент разработки.
- Может читать файлы, анализировать проект, генерировать код.
- Способен создавать PR через GitHub API.
- Можно запускать "как процесс" для авто‑рефакторинга.

### GitHub Copilot

- Работа непосредственно в IDE.
- Подсказки, автодополнения, объяснения.
- Хорош для итеративного мелкого кода.

### Background AI Agents

```
Agent.ReadRepo → Agent.GenerateCode → Agent.Test → Agent.Commit → Agent.PR
```

- могут работать как CI/CD боты;
- проверять архитектурные несоответствия;
- запускаться в GitHub Actions.

## 8.3 Suggested Workflow

1. **Codex CLI** создаёт структуру и код.
2. Ты проверяешь и корректируешь.
3. Codex пишет unit‑тесты.
4. Codex формирует Pull Request.
5. Copilot в IDE помогает улучшать локальные мелкие детали.
6. Background Agent смотрит PR и делает code‑review.

## 8.4 GitHub Integration

- Подключение через **PAT Token**.
- Доступ к дереву файлов.
- Создание / обновление PR.
- Inline‑review.
- Авто‑commit от имени бота.

---

## 9. Coding Style & .editorconfig

Для проекта Snake Groove используется единый стиль кода, зафиксированный в файле `.editorconfig` в корне репозитория.

### 9.1 Базовые правила

- Отступы: **4 пробела**, без табов.
- Скобки: открывающая `{` **на новой строке** (`csharp_new_line_before_open_brace = all`).
- Концовка строки: **CRLF**.
- Кодировка: **UTF-8**.
- Автоматически:
  - обрезаются пробелы в конце строк;
  - добавляется финальная пустая строка в файл;
  - сортируются и группируются `using` (System — первым).

### 9.2 Именование

- Публичные типы и члены: **PascalCase**  
  `GameManager`, `SnakeRunner`, `SnakeGameService`.
- Приватные поля: **_camelCase**  
  `_snakeSegments`, `_inputService`, `_random`.
- Интерфейсы: **IPascalCase**  
  `IGridPos`, `ISnakeGameService`.
- Константы: **PascalCase**  
  `MaxSnakeLength`, `DefaultSpeed`.

### 9.3 Инструменты

- Visual Studio 2026 использует **Code Cleanup on Save**, который применяет правила `.editorconfig`.
- GitHub Copilot / Codex CLI генерируют код в соответствии с этим стилем.
- Любой участник команды при клонировании репозитория автоматически получает одинаковый стиль без ручной настройки IDE.

---

# 🟦 10. Final Notes

- Документ является *главным источником правды*.
- Обновляется при завершении каждой фазы.
- Используется для контекста ИИ‑агентов.

---

**End of Unified Architecture v12**

