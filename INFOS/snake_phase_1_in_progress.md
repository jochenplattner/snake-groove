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
      GameState.cs
      GameLoopService.cs
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
  - хранит позицию еды на поле.

- `FoodSpawner`
  - создаёт новую еду;
  - учитывает занятые змейкой клетки.

- `GameRules`
  - проверка выхода за границы;
  - проверка столкновения змейки с собой;
  - учитывает возможность прохода через хвост, если хвост освобождается на этом тике.

- `GameState`
  - хранит `GridSize`;
  - хранит `Snake`;
  - хранит текущую `Food`;
  - хранит `Score`;
  - хранит `IsGameOver`;
  - хранит `GameOverReason`.

- `GameLoopService`
  - выполняет один игровой тик через `Tick(Direction? inputDirection = null)`;
  - применяет ввод игрока;
  - вычисляет следующую позицию головы;
  - проверяет выход за границы;
  - определяет, будет ли съедена еда;
  - проверяет self-collision;
  - вызывает `Grow(1)` перед `Move()`, если змейка съедает еду;
  - увеличивает `Score`;
  - спаунит новую еду;
  - возвращает `TickResult`.

- `GameOverReason`
  - `None`;
  - `HitWall`;
  - `HitSelf`.

- `TickResult`
  - `Continue`;
  - `AteFood`;
  - `GameOver`.

**Ключевое архитектурное решение:**
- ядро остаётся полностью независимым от Unity;
- Unity-слой в дальнейшем будет только передавать ввод, вызывать `Tick()` и отображать состояние.

**Коммит:** `Phase1-3: Snake Core`

**Статус:** DONE

---

## 4. Активные задачи (In Progress)

### 🔄 Пункт 4 — SnakeRunner (Unity bridge)

Следующая активная задача: связать Unity и чистое C# ядро.

Цель:
- создать Unity-адаптер, который будет принимать ввод игрока;
- вызывать `GameLoopService.Tick()`;
- передавать состояние ядра дальше в визуальный слой;
- обрабатывать `TickResult.GameOver`.

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

### ▶️ Пункт 6 — Тесты и CI

Цель: закрепить качество ядра перед усложнением проекта.

План unit-тестов:
- змейка двигается на одну клетку за тик;
- разворот на 180 градусов игнорируется;
- еда увеличивает длину змейки;
- съедание еды увеличивает `Score`;
- столкновение со стеной даёт `GameOverReason.HitWall`;
- столкновение с собой даёт `GameOverReason.HitSelf`.

План CI:
- добавить GitHub Actions workflow;
- запускать `dotnet build`;
- запускать `dotnet test`;
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
- Добавить `GameConfig` для скорости тика, размера поля и стартовых параметров
- Добавить seed-based random для воспроизводимых тестов
- Подумать над отдельным `TickResult`/event-механизмом для UI и Audio слоя
- В будущем вынести разные типы еды и эффектов через Strategy / Factory

---

## 8. Статус Фазы 1

- UI foundation: ✅ завершён
- Snake Core: ✅ завершён
- Unity bridge / SnakeRunner: 🔄 следующий активный шаг
- Интеграция с Game-сценой: ⏳ впереди
- Визуализация: ⏳ впереди
- Unit-тесты и CI: ⏳ впереди
- Phase 1 summary: ⏳ впереди

📌 Этот файл является **центральной точкой отсчёта Фазы 1** и используется как база для всех дальнейших чатов и решений.

---

## 9. Следующий практический шаг

Следующий рабочий шаг:

> **Пункт 4 — создать `SnakeRunner.cs` и связать Unity с Snake Core.**

Цель ближайшей итерации:

> **Start → Game.unity → SnakeRunner создаёт GameState → GameLoopService начинает тикать → состояние выводится в Debug.Log.**

После этого проект перейдёт от “ядро готово” к “игра начала жить внутри Unity”.