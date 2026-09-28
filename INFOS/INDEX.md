# Snake Groove - индекс документации

Это главная точка входа в документацию проекта. Здесь зафиксировано, какой файл за что отвечает и в каком порядке их читать.

## Текущий снимок проекта

- Unity: `6000.3.7f1`.
- Текущая фаза: Фаза 1 - Classic Snake MVP.
- Чистое C# ядро: готово как embedded package `com.jochen.snakegroovecore`.
- Core-тесты: 8 тестов, локальный запуск и GitHub Actions проходят.
- Unity MCP: настроен и проверен после полного перезапуска Codex и Unity Editor.
- Активная игровая задача: реализовать `SnakeRunner` и подключить ядро к сцене `Game`.

## Порядок чтения

1. [snake_phase_1_in_progress.md](./snake_phase_1_in_progress.md) - где проект находится сейчас и что делать дальше.
2. [SNAKE_ARCHITECTURE.md](./SNAKE_ARCHITECTURE.md) - действующая архитектура и границы слоёв.
3. [unity_codex_mcp.md](./unity_codex_mcp.md) - отдельный план подключения Unity Editor к Codex.
4. [core_ci.md](./core_ci.md) - устройство pure C# тестового проекта и GitHub Actions.
5. [snake_phase_0_summary.md](./snake_phase_0_summary.md) - закрытый исторический отчёт о Фазе 0.

## Что является источником правды

| Вопрос | Документ |
| --- | --- |
| Архитектурные правила и зависимости | `SNAKE_ARCHITECTURE.md` |
| Текущий статус и очередь работ | `snake_phase_1_in_progress.md` |
| Настройка Unity MCP | `unity_codex_mcp.md` |
| Запуск и диагностика core CI | `core_ci.md` |
| История завершённой Фазы 0 | `snake_phase_0_summary.md` |

При расхождении старой формулировки с текущим статусом приоритет имеет `snake_phase_1_in_progress.md`, а для устройства кода - `SNAKE_ARCHITECTURE.md`.

## UML-атлас

- [UML/index.html](./UML/index.html) - главная страница диаграмм.
- [UML/core-package-class-diagram.html](./UML/core-package-class-diagram.html) - классы пакета ядра.
- [UML/unity-to-core-sequence-diagram.html](./UML/unity-to-core-sequence-diagram.html) - последовательность `Unity -> GameSession -> GameTickResult`.
- [UML/scene-flow-state-diagram.html](./UML/scene-flow-state-diagram.html) - переходы между сценами.
- [UML/clean-architecture-component-diagram.html](./UML/clean-architecture-component-diagram.html) - компоненты Clean Architecture.

HTML-диаграммы используются как визуальный атлас. При изменении публичного API ядра, порядка сцен или границ слоёв их нужно обновлять вместе с Markdown-документацией.

## Ближайший маршрут

```text
Core + 8 тестов + зелёный Core CI
  -> Unity to Codex MCP proof of concept: DONE
  -> SnakeRunner
  -> SnakeView и FoodView
  -> Unity EditMode/PlayMode проверки
  -> Classic Snake MVP
  -> музыка и полировка
  -> сборки Windows, WebGL и Android
```

## Состав папки

```text
INFOS/
  INDEX.md
  SNAKE_ARCHITECTURE.md
  core_ci.md
  unity_codex_mcp.md
  snake_phase_0_summary.md
  snake_phase_1_in_progress.md
  UML/
    index.html
    core-package-class-diagram.html
    unity-to-core-sequence-diagram.html
    scene-flow-state-diagram.html
    clean-architecture-component-diagram.html
```
