# Snake Groove - архитектура проекта

> Версия документа: 13
>
> Актуально для: Unity `6000.3.7f1`, Фаза 1
>
> Главный принцип: игровое ядро не зависит от Unity

## 1. Цель продукта

Snake Groove начинается как качественная классическая Snake, а затем получает музыкальную механику: съеденная еда добавляет или изменяет музыкальные слои. Архитектура должна позволить развивать игру без переноса правил движения, счёта и столкновений в `MonoBehaviour`.

Цель Фазы 1 - играбельный Classic Snake MVP:

- меню и переход в игру;
- управление змейкой;
- движение по клеткам;
- еда, рост и счёт;
- столкновение со стеной и телом;
- завершение уровня при заполнении поля;
- минимальная визуализация;
- проверяемое pure C# ядро.

Музыка, сложные эффекты, дополнительные режимы и монетизация не входят в Фазу 1.

## 2. Архитектурное правило

Зависимости направлены только внутрь, к игровому ядру:

```text
Unity Presentation
(UI, SnakeView, FoodView, Audio, Camera)
        |
        v
Unity Adapters
(SnakeRunner, input, scene flow, persistence)
        |
        v
Pure C# Core
(GameSession, rules, state, value objects)
```

Core не знает о `UnityEngine`, сценах, `GameObject`, вводе, спрайтах, звуке или файловой системе. Unity может зависеть от Core. Обратная зависимость запрещена.

## 3. Слои

### 3.1 Pure C# Core

Расположение:

```text
SnakeGroove/Packages/com.jochen.snakegroovecore/
```

Назначение:

- хранить состояние игровой сессии;
- применять направление игрока;
- выполнять один дискретный тик;
- проверять правила движения и столкновений;
- изменять длину и счёт;
- создавать еду только в свободной клетке;
- возвращать неизменяемый снимок состояния.

Ключевые типы:

| Группа | Типы | Ответственность |
| --- | --- | --- |
| Координаты | `GridPosition`, `GridSize`, `Direction` | Значения сетки и направления |
| Сущности | `Snake`, `Food`, `Apple`, `IEatable` | Змейка и съедаемые объекты |
| Состояние | `GameState`, `GameStatus`, `GameOverReason` | Внутреннее изменяемое состояние сессии |
| Правила | `GameRules`, `FoodSpawner`, `GameLoopService` | Один тик, коллизии, появление еды |
| Публичный API | `GameConfig`, `GameSessionFactory`, `GameSession` | Создание и использование сессии |
| Результат | `GameTickResult`, `TickResult`, `GameSnapshot` | Ответ ядра для внешнего слоя |

`GameSession` является фасадом ядра. Unity не должна напрямую менять `GameState` или список сегментов.

### 3.2 Unity Adapters

Текущее расположение инфраструктуры:

```text
SnakeGroove/Assets/Scripts/Infrastructure/
  BootLoader.cs
  EventBus.cs
  GameManager.cs
```

Планируемое расположение игрового адаптера:

```text
SnakeGroove/Assets/Scripts/Gameplay/
  SnakeRunner.cs
```

`SnakeRunner` отвечает за:

- создание `GameSession` через `GameSessionFactory`;
- чтение пользовательского ввода;
- преобразование ввода Unity в `Direction`;
- вызов `GameSession.Tick(...)` с заданной частотой;
- передачу `GameSnapshot` представлению;
- реакцию на `GameOver` и `LevelComplete`.

В `SnakeRunner` не должны появляться собственные правила коллизий, роста, счёта или спауна еды.

### 3.3 Presentation

Текущее UI:

```text
SnakeGroove/Assets/Scripts/UI/Screens/
  UIScreen.cs
  MainMenuScreen.cs
  SettingsScreen.cs
```

Планируемое представление игры:

```text
SnakeView       # отображает сегменты из Snapshot
FoodView        # отображает текущую еду
GameHud         # отображает счёт и состояние
```

Presentation получает готовые данные и отображает их. Он не принимает решений о правилах игры.

### 3.4 Infrastructure

`BootLoader`, `GameManager` и `EventBus` обеспечивают жизненный цикл Unity-приложения, переходы и слабую связь между Unity-компонентами. По мере роста проекта сюда могут добавиться сохранение настроек, рекордов и платформенные сервисы.

Новые библиотеки DI, логирования или persistence подключаются только при появлении конкретной задачи. Сейчас VContainer, Serilog, Unity Cloud Build и Unity Cloud Diagnostics не являются частью реализованной архитектуры.

## 4. Игровой поток

```text
Игрок нажимает клавишу
  -> SnakeRunner запоминает допустимое направление
  -> таймер SnakeRunner вызывает GameSession.Tick(direction)
  -> GameLoopService применяет правила
  -> GameTickResult сообщает исход тика
  -> GameSnapshot содержит новое состояние
  -> SnakeView, FoodView и HUD обновляют отображение
```

Один тик выполняется в следующем порядке:

1. принять новое направление, если оно не противоположно текущему;
2. вычислить следующую позицию головы;
3. проверить выход за границу;
4. определить, будет ли съедена еда;
5. проверить столкновение с телом с учётом освобождающегося хвоста;
6. переместить или вырастить змейку;
7. изменить счёт;
8. создать новую еду или вернуть `LevelComplete`;
9. вернуть `GameTickResult` и новый `GameSnapshot`.

## 5. Состояния игры и сцен

Состояние pure C# сессии:

```text
Running
  -> GameOver (HitWall или HitSelf)
  -> LevelComplete
```

Поток Unity-сцен:

```text
Boot -> MainMenu -> Game
```

Все три рабочие сцены включены в Build Settings. `SampleScene` оставлена выключенной и не участвует в игровом потоке.

## 6. Структура репозитория

```text
snake-groove/
  .github/workflows/
    core-tests.yml

  INFOS/
    INDEX.md
    SNAKE_ARCHITECTURE.md
    core_ci.md
    unity_codex_mcp.md
    snake_phase_0_summary.md
    snake_phase_1_in_progress.md
    UML/

  SnakeGroove/
    Assets/
      Art/
      Audio/
      Prefabs/
      Scenes/
        Boot.unity
        MainMenu.unity
        Game.unity
      Scripts/
        Infrastructure/
        Gameplay/
        UI/
      Tests/
    Packages/
      com.jochen.snakegroovecore/
        Runtime/
        Tests/Runtime/

  tests/
    SnakeGroove.Core.Tests/
```

Папка `Gameplay` уже подготовлена, но `SnakeRunner` и игровые View-компоненты ещё не реализованы.

## 7. Тестирование

### Реализовано

- package test assembly `SnakeGroove.Core.Tests`;
- standalone test host `tests/SnakeGroove.Core.Tests` на `net8.0`;
- единый набор из 8 NUnit-тестов;
- локальный запуск через `dotnet test`;
- GitHub Actions workflow `Core Tests`;
- зелёный core CI на `push` и `pull_request`.

Подробности: [core_ci.md](./core_ci.md).

### Следующий уровень тестов

После появления Unity-адаптера нужны:

- EditMode тесты преобразования конфигурации и данных;
- PlayMode тест базового запуска сцены `Game`;
- проверка, что `SnakeRunner` создаёт сессию и выполняет тики;
- проверка отображения `GameOver` и `LevelComplete`;
- решение, нужен ли Unity Test Runner в GitHub Actions.

Цель покрытия задаётся сценариями и рисками. Число процентов не объявляется достигнутым без отчёта coverage.

## 8. Unity to Codex MCP

Unity MCP является инструментом разработки, а не частью игры и не runtime-зависимостью.

Планируемый цикл:

```text
Codex читает Unity Console и сцену
  -> изменяет код в Git-контролируемых файлах
  -> просит Unity обновить ассеты и скомпилировать
  -> запускает тесты и Play Mode
  -> проверяет Game View
```

Выбран официальный Unity CLI с MCP-сервером и пакетом `com.unity.pipeline`. Proof of concept завершён: Codex получает состояние Editor, Hierarchy, Console, результаты тестов и Game View. После полного перезапуска Codex и штатного перезапуска Unity Editor соединение восстановилось автоматически. При нестабильности beta-инструмента работа над игрой продолжается через обычный Unity Editor.

Полный план: [unity_codex_mcp.md](./unity_codex_mcp.md).

## 9. CI и поставка

Текущее состояние:

- GitHub Actions автоматически проверяет только pure C# core;
- Unity build pipeline пока не настроен;
- автоматический деплой пока не настроен.

План поставки:

| Этап | Платформа | Результат |
| --- | --- | --- |
| MVP | Windows | Локальная development build |
| Первая публичная версия | Windows и WebGL | Версионированные release builds |
| Мобильная адаптация | Android | Touch input, UI scaling, performance pass |
| Расширение | Другие платформы | Только после проверки спроса и требований магазинов |

Перед публичным релизом нужно заменить `DefaultCompany`, задать application identifiers, проверить versioning, лицензии ассетов, privacy requirements и platform-specific controls.

Unity Cloud Build не считается выбранным решением. Когда появятся стабильные локальные build profiles, сравниваются GitHub Actions с Unity license, Unity Build Automation и ручной подписанный релиз.

## 10. Принятые решения

- Ядро хранится в embedded package `com.jochen.snakegroovecore`.
- Core не содержит ссылок на Unity и компилируется с `noEngineReferences`.
- Основной API интеграции - `GameSession.Tick(...) -> GameTickResult`.
- Внешние слои читают состояние через `GameSnapshot`.
- Игровой цикл дискретный и не зависит от частоты кадров.
- Core-тесты выполняются вне Unity и внутри Unity из одного набора исходников.
- Unity-логика остаётся тонкой: ввод, расписание тиков, отображение и платформенные адаптеры.
- Сложные паттерны и сторонние библиотеки добавляются по необходимости, а не заранее.

## 11. Roadmap архитектуры

```text
Фаза 0: каркас репозитория и Unity-проекта                         DONE
Фаза 1: Classic Snake MVP
  Core package                                                   DONE
  Core tests и GitHub Actions                                    DONE
  Unity to Codex MCP proof of concept                            DONE
  SnakeRunner и сцена Game                                      IN PROGRESS
  SnakeView, FoodView, HUD                                      TODO
  Unity integration tests и закрытие MVP                         TODO
Фаза 2: музыкальные слои и audio events                          TODO
Фаза 3: визуальная полировка и адаптация платформ                TODO
Release: build profiles, QA, подпись, публикация                 TODO
```

Текущий рабочий статус ведётся в [snake_phase_1_in_progress.md](./snake_phase_1_in_progress.md).
