# Snake Groove — Фаза 1

## Что готово по шагу 2 (UI foundation)

### 2.1 Canvas-база (готово ✅)
- В сцене **MainMenu** создан **Canvas_MainMenu**.
- Настройки Canvas:
  - Render Mode: **Screen Space — Overlay**
  - **Canvas Scaler**: Scale With Screen Size, Reference Resolution **1920×1080**, Match **0.5**
  - **Graphic Raycaster** присутствует
- В сцене есть **ровно один EventSystem**.

### 2.2 Структура UI-иерархии (готово ✅)
- Собрана чистая и расширяемая иерархия UI:
  - Canvas_MainMenu
    - UI_Root
      - Screen_MainMenu (stretch на весь экран)
        - MenuContainer (центр)
          - Button_Start
          - Button_Settings
          - Button_Exit
- На кнопках добавлены **Layout Element** (Preferred Width/Height), чтобы все кнопки были одинакового размера.
- Кнопки уже кликабельны и вызывают метод (OnClick) — базовая связка UI → код работает.

✅ Итог: UI-слой теперь не «накинул кнопки», а построен как система, на которую дальше легко навешивать экраны (Settings/HUD/Pause) и анимации.

---

## Что делаем дальше (ближайшие шаги)

### 2.3 Экранный слой (UIScreen + MainMenuScreen) — следующий шаг ▶️
Цель: сделать «студийную» архитектуру UI — экраны как сущности, а не разрозненные MonoBehaviour.
- Создать базовый класс **UIScreen** (Show/Hide)
- Создать **MainMenuScreen** (обработчики Start/Settings/Exit)
- Перепривязать кнопки на методы MainMenuScreen
- (Опционально) убрать/заменить текущий **MainMenuUI**, чтобы не было двух параллельных подходов

После 2.3 переходим к:
- **Пункт 3:** Snake Core (pure C# ядро)
- **Пункт 4:** SnakeRunner (Unity-интеграция)

---

## Namespaces — мы это раньше явно не фиксировали
Да, до этого мы «implicit» следовали структуре папок, но **явно namespaces не закрепляли**. Сейчас закрепим, чтобы проект рос без хаоса.

### Почему namespaces важны
- Чётко разделяют слои (Core / UI / Gameplay / Infrastructure)
- Упрощают поиск классов и автокомплит
- Уменьшают конфликты имён (особенно когда подключим embedded package)
- Помогают держать архитектуру (Clean Architecture) в порядке

---

## Предлагаемая схема namespaces для Unity-проекта
> Базовый префикс: **SnakeGroove**

### 1) Scripts/Core (Unity bootstrap / app-level core)
**Namespace: `SnakeGroove.Core`**
Сюда относятся вещи, которые связывают сцены и жизненный цикл приложения:
- `GameManager` → `SnakeGroove.Core`
  - Долгоживущий singleton/entry point (DontDestroyOnLoad)
  - Хранит ссылки на глобальные сервисы/контейнер (позже DI)
- `BootLoader` → `SnakeGroove.Core`
  - Логика старта и перехода Boot → MainMenu
  - Позже здесь может жить SceneFlowController
- `EventBus` → `SnakeGroove.Core`
  - Простая шина событий (UI/Audio/Game) на уровне Unity
  - Важно: ядро (pure C#) не должно зависеть от Unity EventBus

### 2) Scripts/UI
**Namespace: `SnakeGroove.UI`** (и подпакеты)
- `UIScreen` → `SnakeGroove.UI`
- Экраны:
  - `MainMenuScreen` → `SnakeGroove.UI.Screens`
  - `SettingsScreen` → `SnakeGroove.UI.Screens` (позже)
  - `HudScreen` → `SnakeGroove.UI.Screens` (позже)
- UI-компоненты (кнопки/виджеты): `SnakeGroove.UI.Components`

> Что делать с текущим `MainMenuUI`:
- Если он остаётся как временный MonoBehaviour, лучше дать ему имя и неймспейс по роли:
  - либо переименовать в `MainMenuScreen` и перенести в `SnakeGroove.UI.Screens`
  - либо оставить как `MainMenuUI` в `SnakeGroove.UI` и потом удалить

### 3) Scripts/Gameplay
**Namespace: `SnakeGroove.Gameplay`**
Unity-специфичная игровая часть (рендер/объекты/инпут-бридж), но НЕ pure C# домен:
- `SnakeRunner` → `SnakeGroove.Gameplay` (будет в шаге 4)
- спавнеры, визуализация, контроллеры

### 4) Scripts/Infrastructure
**Namespace: `SnakeGroove.Infrastructure`**
Адаптеры, репозитории, сохранения, настройки, интеграции:
- `SettingsRepository`, `ScoreRepository`, сохранение/загрузка и т.д.

---

## Namespaces для embedded package (pure C# ядро)
Чтобы не путаться с Unity-слоем, ядро лучше держать отдельно по имени.
Вариант (рекомендованный):
- `SnakeGroove.SnakeCore` или `SnakeGroove.GameLogic`
  - `SnakeGroove.GameLogic.Domain`
  - `SnakeGroove.GameLogic.Services`
  - `SnakeGroove.GameLogic.ValueObjects`

Главный принцип:
- **Unity-проект** = `SnakeGroove.*`
- **Pure C# ядро** = отдельный корневой namespace, чтобы сразу видно было границу слоя

---

