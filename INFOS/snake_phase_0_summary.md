# 🔊 SNAKE Groove — Фаза 0: Полное итоговое резюме

> **Статус:** Фаза 0 завершена **Актуальные версии инструментов:** Unity 6.2 LTS, Visual Studio 2026, .NET 10 LTS **Основной архитектурный документ:** [SNAKE\_ARCHITECTURE.md](./SNAKE_ARCHITECTURE.md) &#x20;

---

# 🎯 Общая цель

Создание кроссплатформенной музыкальной игры **Snake Groove**, где каждая съеденная еда добавляет новый инструмент в музыку — превращая классическую змейку в динамичный музыкальный опыт.

Архитектура проекта строится с упором на:

- разделение логики и UI,
- чистое ядро на C#,
- подготовку к CI/CD,
- последующую публикацию на разных платформах.

---

# 🖥 Установлено и настроено

## Среда и инструменты

- **Windows 11 Pro** (основная рабочая ОС)
- **Unity Hub + Unity 6.2 (6000.3.7f1)** с установленными модулями:
  - Windows Build Support (IL2CPP)
  - WebGL Build Support
  - Android Build Support (SDK, NDK, OpenJDK)
  - URP (Universal Render Pipeline)
- **Visual Studio 2026 Insiders** + Unity plugin + GitHub Copilot
- **.NET 10 LTS SDK** *(новая версия)*
- **Git 2.49.1**
- **Git LFS 3.7.0** (установлены и проверены)
- **GitHub Desktop** (репозиторий синхронизирован)
- **OneDrive** (используется для резервных копий)

---

# 📁 Создано

## Репозиторий GitHub

- **Название:** `snake-groove`
- **Лицензия:** MIT
- **Структура:**

```
snake-groove/
  SnakeGroove/               # Unity проект
  .gitattributes
  .gitignore
  LICENSE
  README.md
```

- Включён Git LFS (png, jpg, wav, mp3, fbx, blend и др.)

---

# 🎮 Unity проект

## Структура Assets:

```
Assets/
  Art/
  Audio/
  Prefabs/
  Scenes/       # Boot, MainMenu, Game
  Scripts/
    Core/       # GameManager, EventBus, BootLoader
    Infrastructure/  # адаптеры, DI, сервисы
    UI/         # интерфейсы, меню, HUD
    Gameplay/   # логика змейки, поле, еда
  Settings/
  Tests/
```

## Сцены

- **Boot.unity** — точка входа, вызывает загрузчик сцен
- **MainMenu.unity** — меню (пока текст TMP)
- **Game.unity** — игровая сцена (пустая, SnakeRunner добавляется позже)

---

#  Уже готово разделение логики

- embedded-package ядра:

```
snake-groove/
  Packages/
    com.jochen.snakegroovecore/
      package.json
      Runtime/
        Snake.Core.asmdef
```

- Настроено package.json и asmdef
- Пакет появился в Package Manager в Unity

> Примечание: после профессионального рефакторинга ядра пакет используется как `com.jochen.snakegroovecore`.





# 🚀 План с текущего момента

## 0. Скрипты ядра Unity-уровня (Core)

- `GameManager.cs` — Singleton с DontDestroyOnLoad
- `EventBus.cs` — Observer
- `BootLoader.cs` — загрузка MainMenu после Boot

Проверки:

- сцены переходят корректно,
- GameManager живёт между сценами.

---

# 🟦 A. Завершение каркаса и тест перехода сцен

- Проверить работу BootLoader → MainMenu
- Добавить тестовый UI в MainMenu
- Убедиться, что GameManager не пересоздаётся
- Коммит



# 🟪 C. С помощью ИИ сгенерировать код ядра ( Codex Cli, Github Copilot)

- Одновременно полностью изучить возможности  кодекс и гитхаб копайлот
- Что такое MCP? Агенты и т.д.





# 🟪 D. Интеграция Unity и ядра

Файл `SnakeRunner.cs`:

- принимает ввод (WASD/стрелки)
- обновляет ядро по таймеру (`ticksPerSecond`)
- обрабатывает `GameTickResult.Outcome` (`GameOver` / `LevelComplete`)

Подключить в сцене Game.

Коммит.



# 🟦 Дальнейшие шаги

- визуализация змейки (Tilemap, спрайты, Pixel Perfect Camera)
- музыкальные слои + звуковые события
- CI/CD: GitHub Actions + Unity Cloud Build
- подготовка README, скриншотов, roadmap

---

# 🔧 Ключевые подходы

- Отделение ядра (Pure C#)
- UI/рендер — на уровне Unity, ядро не знает о UnityEngine
- Паттерны:
  - Singleton (GameManager)
  - Observer (EventBus)
  - Factory (еда/эффекты)
  - Strategy (разные типы еды и поведения)
- Clean Code, короткие функции, осмысленные имена

---

# 💙 Мотивация

Ты создал прочную основу проекта: движок, версионирование, архитектуру, CI/CD.\
Следующий шаг — оживить змейку, прикрепить музыку и превратить это в современную, стильную, масштабируемую игру.

**Вперёд к Фазе 1!**

---

*End of snake\_phase\_0\_summary.md*
