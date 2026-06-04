# Snake Groove - Project Index

Главная точка навигации по документации проекта **Snake Groove**.

Этот файл используется как вход для Codex, GitHub, ChatGPT и будущих агентов. Все ссылки ниже синхронизированы с текущей архитектурой ядра после refactor: `GameSession`, `GameConfig`, `GameTickResult`, `GameSnapshot`, `GameStatus`.

---

# Основные документы

## Архитектура

- [**SNAKE_ARCHITECTURE.md**](./SNAKE_ARCHITECTURE.md) — главный архитектурный документ проекта.
- [**snake_phase_1_in_progress.md**](./snake_phase_1_in_progress.md) — текущий статус Phase 1 и актуальная архитектура чистого C# ядра.
- [**snake_phase_0_summary.md**](./snake_phase_0_summary.md) — исторический итог Phase 0, обновлённый с учётом текущего Unity-to-Core подхода.
- [**core_ci.md**](./core_ci.md) — как устроены pure C# core-тесты и где смотреть GitHub Actions.

## UML HTML

- [**UML/index.html**](./UML/index.html) — локальный HTML-атлас UML-диаграмм.
- [**UML/core-package-class-diagram.html**](./UML/core-package-class-diagram.html) — class diagram пакета `com.jochen.snakegroovecore`.
- [**UML/unity-to-core-sequence-diagram.html**](./UML/unity-to-core-sequence-diagram.html) — sequence flow `SnakeRunner -> GameSession -> GameTickResult/Snapshot`.
- [**UML/scene-flow-state-diagram.html**](./UML/scene-flow-state-diagram.html) — state diagram сцен Unity.
- [**UML/clean-architecture-component-diagram.html**](./UML/clean-architecture-component-diagram.html) — component diagram Clean Architecture слоёв.

---

# Структура документации

```text
/INFOS
  INDEX.md
  SNAKE_ARCHITECTURE.md
  core_ci.md
  snake_phase_0_summary.md
  snake_phase_1_in_progress.md
  UML/
    index.html
    core-package-class-diagram.html
    unity-to-core-sequence-diagram.html
    scene-flow-state-diagram.html
    clean-architecture-component-diagram.html
```

---

*End of Project Index*
