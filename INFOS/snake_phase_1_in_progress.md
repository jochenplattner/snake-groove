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
```
Assets/
Packages/
ProjectSettings/
```

---

### Assets/

```
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
```

**Назначение слоёв:**
- **Core** — загрузка, инициализация, EventBus, GameManager
- **UI** — экраны, кнопки, пользовательское взаимодействие
- **Gameplay** — мост между Unity и pure C# ядром

---

### Packages/

```
Packages/
  com.jochen.snakecore/
    Runtime/
      GridPosition.cs
      Direction.cs
      Snake.cs
      GameState.cs
      GameLoopService.cs
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
```
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

## 4. Активные задачи (In Progress)

### 🔄 Пункт 3 — Snake Core (pure C#)
В процессе реализации:
- GridPosition
- Direction
- Snake (сегменты, движение)
- GameState
- GameLoopService (tick-based)

Цель: полностью автономное ядро без Unity-зависимостей.

---

## 5. Следующие шаги (To Do)

### ▶️ Пункт 4 — SnakeRunner (Unity bridge)
- Приём ввода (клавиатура)
- Передача тиков в ядро
- Обработка GameOver

### ▶️ Пункт 5 — Визуализация
- Временные спрайты (квадрат / круг)
- Grid-based позиционирование

### ▶️ Пункт 6 — Тесты и CI
- Unit-тесты для ядра
- GitHub Actions (build + test)

---

## 6. Архитектурные принципы проекта

- Clean Architecture
- Разделение логики и представления
- EventBus для коммуникации
- Unity как адаптер, а не источник логики
- Embedded package для ядра

---

## 7. Предложения по улучшению (на будущее)

- Ввести `UIScreenManager` для переключения экранов
- Добавить `SettingsScreen` как второй экран
- Расширить UIScreen анимациями (fade / slide)
- Раннее подключение аудио-слоя (AudioLayerManager)
- Покрыть Snake Core тестами до усложнения логики

---

## 8. Статус Фазы 1

- UI foundation: ✅ завершён
- Snake Core: 🔄 в процессе
- Интеграция и визуализация: ⏳ впереди

📌 Этот файл является **центральной точкой отсчёта Фазы 1** и используется как база для всех дальнейших чатов и решений.

