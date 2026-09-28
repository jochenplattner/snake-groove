# Snake Groove - Фаза 1: Classic Snake MVP

> Статус фазы: IN PROGRESS
>
> Unity: `6000.3.7f1`
>
> Активная игровая задача: `SnakeRunner`
>
> Завершённая вспомогательная задача: Unity to Codex MCP proof of concept

## 1. Цель Фазы 1

Получить минимальную, но законченную классическую Snake внутри Unity:

- пользователь запускает игру из меню;
- змейка движется по сетке и принимает ввод;
- еда отображается, увеличивает змейку и счёт;
- столкновение со стеной или телом завершает игру;
- заполнение поля завершает уровень;
- правила игры находятся в pure C# ядре;
- Unity отвечает за ввод, время, сцены и изображение;
- основные сценарии ядра защищены автоматическими тестами.

Музыкальная механика Snake Groove начнётся после закрытия этого MVP.

## 2. Фактическая структура

```text
SnakeGroove/
  Assets/
    Scenes/
      Boot.unity
      MainMenu.unity
      Game.unity
    Scripts/
      Infrastructure/
        BootLoader.cs
        EventBus.cs
        GameManager.cs
      Gameplay/                    # подготовлена для Unity-адаптеров
      UI/Screens/
        UIScreen.cs
        MainMenuScreen.cs
        SettingsScreen.cs
  Packages/
    com.jochen.snakegroovecore/
      Runtime/
      Tests/Runtime/

tests/
  SnakeGroove.Core.Tests/

.github/workflows/
  core-tests.yml
```

В Build Settings включены `Boot`, `MainMenu` и `Game`. `SampleScene` выключена.

## 3. Выполнено

### 3.1 Каркас сцен и меню - DONE

- подготовлен поток `Boot -> MainMenu -> Game`;
- `GameManager` живёт между сценами;
- `BootLoader` загружает главное меню;
- создан базовый `UIScreen`;
- создан `MainMenuScreen` с обработчиками Start, Settings и Exit;
- создан `SettingsScreen`;
- меню использует Canvas и EventSystem.

### 3.2 Pure C# ядро - DONE

Embedded package: `com.jochen.snakegroovecore`.

Ядро умеет:

- хранить клеточные координаты и размер поля;
- создавать змейку с валидной начальной конфигурацией;
- запрещать разворот на 180 градусов;
- выполнять один игровой тик;
- определять поедание еды;
- увеличивать длину и счёт;
- создавать новую еду в свободной клетке;
- определять `HitWall` и `HitSelf`;
- возвращать `LevelComplete`, когда свободных клеток не осталось;
- отдавать внешнему слою read-only `GameSnapshot`.

Публичная точка интеграции:

```text
GameSessionFactory
  -> GameSession
  -> GameSession.Tick(Direction?)
  -> GameTickResult
  -> GameSnapshot
```

Unity не должна изменять `GameState` напрямую и не должна повторять правила ядра.

### 3.3 Core-тесты и GitHub Actions - DONE

Реализованы:

- Unity package test assembly `SnakeGroove.Core.Tests`;
- standalone .NET 8 test project `tests/SnakeGroove.Core.Tests`;
- общий набор из 8 NUnit-тестов;
- GitHub Actions workflow `.github/workflows/core-tests.yml`;
- запуск на каждом `push` и `pull_request`;
- сохранение TRX-результата как GitHub artifact.

Проверенные сценарии:

1. обычное движение;
2. запрет разворота на 180 градусов;
3. еда, рост и счёт;
4. `LevelComplete`;
5. столкновение со стеной;
6. столкновение с собой;
7. создание стандартной сессии;
8. отказ при пересечении стартовых сегментов.

Локальный результат: 8 из 8 тестов проходят. GitHub Actions: зелёный.

Подробнее: [core_ci.md](./core_ci.md).

## 4. Текущие задачи

### 4.1 Unity to Codex MCP - DONE, вспомогательная задача 3.4

Цель - дать Codex контролируемый доступ к открытому Unity Editor:

- читать активную сцену, Hierarchy и компоненты;
- читать Console и ошибки компиляции;
- обновлять Asset Database и ждать окончания компиляции;
- запускать EditMode/PlayMode tests;
- управлять Play Mode;
- получать снимок Game View.

Выбран официальный Unity CLI с `unity mcp`. Инструмент не становится runtime-зависимостью игры: если beta-интеграция окажется нестабильной, `SnakeRunner` продолжаем делать обычным способом.

Текущее состояние:

- Unity CLI `1.0.0-beta.8` установлен вместе с Unity Hub;
- проектный Unity MCP настроен для Codex;
- `com.unity.pipeline` `0.8.0-exp.1` подключён;
- Editor, Hierarchy, Console, Play Mode и Game View доступны через Pipeline;
- все 8 EditMode-тестов проходят через новое подключение;
- write/Undo smoke test не оставил изменений в сцене;
- после полного перезапуска Codex нативные Unity MCP tools загрузились в новую сессию;
- после штатного перезапуска Unity Editor соединение восстановилось автоматически;
- повторная проверка `MainMenu.unity`, Hierarchy и Console прошла без ошибок и изменений сцены.

План и критерии готовности: [unity_codex_mcp.md](./unity_codex_mcp.md).

### 4.2 SnakeRunner - IN PROGRESS, игровая задача 4

Цель - связать сцену `Game` с готовым ядром.

Минимальная реализация:

1. создать `Assets/Scripts/Gameplay/SnakeRunner.cs`;
2. создать `GameSession` через `GameSessionFactory.CreateClassicDefault(...)`;
3. читать стрелки и WASD через установленный Input System;
4. сохранять последнее запрошенное направление;
5. вызывать `GameSession.Tick(...)` по независимому от FPS таймеру;
6. публиковать или передавать `GameTickResult` представлению;
7. временно выводить в Console позицию головы, счёт и исход тика;
8. останавливать тики при `GameOver` или `LevelComplete`;
9. подключить компонент к объекту `GameController` в `Game.unity`.

Критерий готовности:

- проект компилируется без ошибок;
- в Play Mode змейка логически двигается;
- направление меняется корректно;
- Console показывает изменение snapshot;
- исходы `GameOver` и `LevelComplete` обрабатываются;
- правила игры не продублированы в Unity-коде.

## 5. Следующие задачи

### 5.1 Визуализация - TODO, задача 5

- реализовать перевод `GridPosition -> Vector3`;
- создать простой prefab сегмента;
- создать простой prefab еды;
- реализовать `SnakeView` и `FoodView`;
- обновлять View из `GameSnapshot`;
- добавить камеру и читаемое игровое поле;
- проверить рост, новую еду и завершение игры визуально.

### 5.2 Unity-проверки - TODO, оставшаяся часть задачи 6

Общий статус задачи «Тесты и CI»: PARTIALLY DONE.

Package tests уже проходят в Unity Test Runner / EditMode: 8 из 8. Для будущего Unity-слоя остаётся:

- добавить EditMode тесты адаптеров;
- добавить минимальный PlayMode smoke test сцены `Game`;
- решить, оправдан ли Unity Test Runner в GitHub Actions;
- сохранить быстрый core workflow отдельным от тяжёлого Unity pipeline.

### 5.3 Закрытие Фазы 1 - TODO, задача 7

Фаза закрывается, когда:

- меню запускает игровую сцену;
- `SnakeRunner` управляет core-сессией;
- змейка и еда видимы;
- счёт и рост работают;
- `HitWall`, `HitSelf` и `LevelComplete` видны пользователю;
- core CI остаётся зелёным;
- Unity tests и ручной smoke test проходят;
- создаётся `snake_phase_1_summary.md`;
- при необходимости создаётся tag `v0.1`.

## 6. Что не входит в Фазу 1

- музыкальные слои и синхронизация с ритмом;
- сложные типы еды и power-ups;
- сохранение прогресса;
- онлайн-функции;
- multiplayer;
- магазин и монетизация;
- финальная графика;
- публикация в stores.

Эти задачи не должны усложнять `SnakeRunner` и первую визуализацию.

## 7. План после MVP

```text
Фаза 2
  -> события еды для Audio layer
  -> музыкальные дорожки и AudioMixer
  -> настройки громкости

Фаза 3
  -> визуальный стиль, анимации, VFX
  -> touch input и адаптивный UI
  -> производительность и accessibility

Release
  -> Windows build
  -> WebGL build
  -> Android build
  -> platform QA
  -> versioning, signing, store metadata и публикация
```

## 8. Следующий практический маршрут

```text
1. Реализовать SnakeRunner.
2. Использовать Unity MCP для проверки сцены, Console и Play Mode.
3. Убедиться, что сессия тикает в Play Mode.
4. Реализовать SnakeView и FoodView.
5. Добавить Unity integration tests.
6. Закрыть Фазу 1.
```

Архитектурные границы описаны в [SNAKE_ARCHITECTURE.md](./SNAKE_ARCHITECTURE.md).
