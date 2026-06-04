# Snake Groove — Фаза 1 (In Progress)

## 1. Общее описание проекта

**Snake Groove** — это модульная игра в жанре Snake, построенная на принципах **Clean Architecture**.

Ключевая идея проекта:
- игровая логика реализована в виде **чистого C# ядра** (без Unity-зависимостей);
- Unity используется как слой представления и адаптации (UI, ввод, визуализация);
- архитектура масштабируема и готова к расширению (музыка, режимы, платформы).

Проект развивается по фазам с фиксацией архитектурных решений в Markdown-документах.

---

## 2. Структура проекта

### Корень

```text
Assets/
Packages/
ProjectSettings/
```

---

### Assets/

```text
Assets/
  Scripts/
    Core/           // Unity-уровень: менеджеры, загрузка, шина событий
    UI/
      UIScreen.cs
      Screens/
        MainMenuScreen.cs
    Gameplay/       // Unity-адаптеры к игровому ядру
  Scenes/
    Boot.unity
    MainMenu.unity
    Game.unity
```

**Назначение слоёв:**
- **Core** — загрузка, инициализация, EventBus, GameManager
- **UI** — экраны, кнопки, пользовательское взаимодействие
- **Gameplay** — мост между Unity и pure C# ядром

---

### Packages/

```text
Packages/
  com.jochen.snakegroovecore/
    Runtime/
      GridPosition.cs
      GridSize.cs
      Direction.cs
      DirectionExtensions.cs
      Snake.cs
      Food.cs
      FoodSpawner.cs
      GameRules.cs
      GameConfig.cs
      GameSession.cs
      GameSessionFactory.cs
      GameSnapshot.cs
      GameStatus.cs
      GameState.cs
      GameLoopService.cs
      GameTickResult.cs
      GameOverReason.cs
      TickResult.cs
```

📌 Ядро игры оформлено как **embedded package**, не зависящий от Unity.

---

## 3. Фаза 1 — выполненные шаги

### ✅ 2.1 Canvas и EventSystem

- Canvas_MainMenu (Overlay)
- Canvas Scaler (1920×1080, Match 0.5)
- Один EventSystem

**Статус:** DONE

---

### ✅ 2.2 UI-иерархия

```text
Canvas_MainMenu
  UI_Root
    Screen_MainMenu
      MenuContainer
        Button_Start
        Button_Settings
        Button_Exit
```

- Используется Layout Element
- Кнопки кликабельны

**Статус:** DONE

---

### ✅ 2.3 UIScreen + MainMenuScreen

**UIScreen.cs**
- Базовый класс экранов
- Методы Show / Hide
- Namespace: `SnakeGroove.UI`

**MainMenuScreen.cs**
- Наследуется от UIScreen
- Обработчики:
  - OnStartClicked
  - OnSettingsClicked
  - OnExitClicked
- Висит на `Screen_MainMenu`
- Namespace: `SnakeGroove.UI.Screens`

Кнопки перепривязаны через **Scene**, не через Assets.

**Статус:** DONE

---

### ✅ 3. Snake Core (pure C#)

Реализовано автономное игровое ядро без зависимости от Unity.

Основные элементы ядра:

- `GridPosition`
  - позиция на сетке;
  - используется для координат головы, тела змейки и еды.

- `GridSize`
  - размер игрового поля;
  - используется для проверки выхода за границы.

- `Direction`
  - направления движения змейки;
  - поддерживается проверка противоположного направления.

- `DirectionExtensions`
  - преобразование направления в смещение по сетке;
  - логика определения противоположных направлений.

- `Snake`
  - хранит список сегментов;
  - голова находится в индексе `0`;
  - умеет двигаться через `Move()`;
  - умеет расти через `Grow(int amount = 1)`;
  - запрещает разворот на 180 градусов через `ChangeDirection()`.

- `Food`
  - хранит позицию еды на поле;
  - содержит `ScoreValue` и `GrowthAmount`.

- `FoodSpawner`
  - создаёт новую еду;
  - учитывает занятые змейкой клетки;
  - поддерживает `TrySpawn(...)`, чтобы заполненное поле стало `LevelComplete`, а не техническим исключением.

- `GameConfig`
  - хранит размер поля, стартовые сегменты змейки, направление, скорость тика и optional seed;
  - валидирует стартовую конфигурацию.

- `GameSessionFactory`
  - создаёт полностью связанную игровую сессию;
  - скрывает ручную сборку `Snake`, `FoodSpawner`, `GameState`, `GameLoopService`.

- `GameSession`
  - публичный facade для Unity-адаптера;
  - отдаёт `Snapshot`;
  - выполняет `Tick(Direction?)`.

- `GameSnapshot`
  - read-only снимок состояния для UI, View и Debug.Log;
  - защищает ядро от случайной мутации Unity-слоем.

- `GameRules`
  - проверка выхода за границы;
  - проверка столкновения змейки с собой;
  - учитывает возможность прохода через хвост, если хвост освобождается на этом тике.

- `GameState`
  - хранит `GridSize`;
  - хранит `Snake`;
  - хранит текущую `Food`;
  - хранит `Score`;
  - хранит `Status`;
  - хранит `GameOverReason`;
  - публичных setters нет: состояние меняется только методами ядра.

- `GameLoopService`
  - выполняет один игровой тик через `Tick(Direction? inputDirection = null)`;
  - применяет ввод игрока;
  - вычисляет следующую позицию головы;
  - проверяет выход за границы;
  - определяет, будет ли съедена еда;
  - проверяет self-collision;
  - применяет `GrowthAmount` и `ScoreValue` съеденной еды;
  - спаунит новую еду через `TrySpawn(...)`;
  - завершает уровень как `LevelComplete`, если свободных клеток не осталось;
  - возвращает `GameTickResult`.

- `GameOverReason`
  - `None`;
  - `HitWall`;
  - `HitSelf`.

- `TickResult`
  - `Continue`;
  - `AteFood`;
  - `GameOver`;
  - `LevelComplete`.

- `GameTickResult`
  - содержит `Outcome`, `ScoreDelta`, `EatenFood`, `SpawnedFood`, `GameOverReason`, `Snapshot`;
  - является основным ответом ядра на один тик.

**Ключевое архитектурное решение:**
- ядро остаётся полностью независимым от Unity;
- Unity-слой в дальнейшем будет только создавать `GameSession`, передавать ввод, вызывать `Tick()` и отображать `GameSnapshot`.

**Коммит:** `Phase1-3: Snake Core`

**Статус:** DONE

---

## 4. Активные задачи (In Progress)

### 🔄 Пункт 4 — SnakeRunner (Unity bridge)

Следующая активная задача: связать Unity и чистое C# ядро.

Цель:
- создать Unity-адаптер, который будет принимать ввод игрока;
- создать `GameSession` через `GameSessionFactory.CreateClassicDefault(...)`;
- вызывать `GameSession.Tick(direction?)`;
- передавать `GameTickResult.Snapshot` дальше в визуальный слой;
- обрабатывать `TickResult.GameOver` и `TickResult.LevelComplete`.

Минимальный план:
- создать `SnakeRunner.cs` в `Assets/Scripts/Gameplay`;
- создать объект `GameController` в сцене `Game.unity`;
- повесить на него `SnakeRunner`;
- в `Update()` читать ввод с клавиатуры;
- по таймеру вызывать тик ядра;
- временно выводить состояние в `Debug.Log`.

**Статус:** IN PROGRESS / NEXT

---

## 5. Следующие шаги (To Do)

### ▶️ Пункт 5 — Визуализация

Цель: увидеть змейку и еду в сцене `Game.unity`.

План:
- создать временный prefab сегмента змейки;
- создать временный prefab еды;
- реализовать перевод `GridPosition -> Vector3`;
- создать простой `SnakeView`;
- создать простой `FoodView`;
- после каждого тика обновлять позиции объектов.

Минимальный критерий готовности:
- змейка визуально двигается по сетке;
- еда отображается;
- после съедания еды длина змейки увеличивается;
- новая еда появляется в свободной клетке.

---

### 🔄 Пункт 6 — Тесты и CI

Цель: закрепить качество ядра перед усложнением проекта.

**Статус:** PARTIALLY DONE

Уже реализовано:
- package test assembly `SnakeGroove.Core.Tests`;
- standalone .NET test project `tests/SnakeGroove.Core.Tests`;
- `GameLoopServiceTests`;
- `GameSessionFactoryTests`;
- тест обычного движения;
- тест поедания еды, роста и счёта;
- тест `LevelComplete`;
- тест `HitWall`;
- тест запрета разворота на 180 градусов;
- тест self-collision → `GameOverReason.HitSelf`;
- тест валидации пересекающихся стартовых сегментов;
- GitHub Actions workflow `.github/workflows/core-tests.yml` для pure C# core-тестов;
- локальная проверка `dotnet test tests/SnakeGroove.Core.Tests/SnakeGroove.Core.Tests.csproj` проходит: 8/8 tests passed.

Осталось:
- прогнать package tests через Unity Test Runner / EditMode;
- проверить первый GitHub Actions run после push / pull request;
- позже решить, добавлять ли Unity Test Runner в CI.

План CI:
- на первом этапе запускать `dotnet test` для standalone pure C# test project;
- при возможности добавить запуск Unity Test Runner / EditMode package tests;
- запускать проверки на push / pull request.

---

### ▶️ Пункт 7 — Закрытие Фазы 1

Цель: зафиксировать завершение Classic Snake MVP.

План:
- обновить `snake_phase_1_in_progress.md`;
- создать `snake_phase_1_summary.md`;
- описать, что сделано:
  - UI foundation;
  - Snake Core;
  - SnakeRunner;
  - визуализация;
  - тесты;
  - CI;
- отдельно указать, что музыка, настройки и расширенные режимы переносятся в следующие фазы;
- сделать финальный коммит Фазы 1;
- по желанию создать tag `phase-1` или `v0.1`.

---

## 6. Архитектурные принципы проекта

- Clean Architecture
- Разделение логики и представления
- EventBus для коммуникации на Unity-уровне
- Unity как адаптер, а не источник игровой логики
- Embedded package для ядра
- Pure C# core без `UnityEngine`
- Движение игры через tick-based loop
- Минимизация логики внутри MonoBehaviour

---

## 7. Предложения по улучшению (на будущее)

- Ввести `UIScreenManager` для переключения экранов
- Добавить `SettingsScreen` как второй экран
- Расширить UIScreen анимациями (fade / slide)
- Раннее подключение аудио-слоя (`AudioLayerManager`)
- Покрыть Snake Core тестами до усложнения логики
- Расширить package tests новыми сценариями для еды и seed-based random
- Добавить CI-запуск Unity Test Runner / package tests
- Подумать над event-механизмом поверх `GameTickResult` для UI и Audio слоя
- В будущем вынести разные типы еды и эффектов через Strategy / Factory

---

## 8. Статус Фазы 1

- UI foundation: ✅ завершён
- Snake Core: ✅ завершён
- Unity bridge / SnakeRunner: 🔄 следующий активный шаг
- Интеграция с Game-сценой: ⏳ впереди
- Визуализация: ⏳ впереди
- Unit-тесты: ✅ базовый набор готов
- CI: 🔄 core workflow настроен, ждёт проверки на GitHub
- Phase 1 summary: ⏳ впереди

📌 Этот файл является **центральной точкой отсчёта Фазы 1** и используется как база для всех дальнейших чатов и решений.

---

## 9. Следующий практический шаг

Следующий рабочий шаг:

> **Пункт 4 — создать `SnakeRunner.cs` и связать Unity с Snake Core.**

Цель ближайшей итерации:

> **Start → Game.unity → SnakeRunner создаёт GameSession через GameSessionFactory → GameSession.Tick() возвращает GameTickResult → Snapshot выводится в Debug.Log / визуальный слой.**

После этого проект перейдёт от “ядро готово” к “игра начала жить внутри Unity”.
