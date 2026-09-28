# Snake Groove - Core CI

> Статус: настроено и проверено. Локальный запуск и GitHub Actions проходят, 8 из 8 тестов зелёные.

## Назначение

Core CI быстро проверяет игровое ядро без запуска Unity Editor и без Unity license. Он защищает правила движения, роста, столкновений и завершения игры при каждом `push` и `pull request`.

## Где находятся части системы

```text
SnakeGroove/Packages/com.jochen.snakegroovecore/
  Runtime/                         # исходный код ядра
  Tests/Runtime/                   # единственный набор NUnit-тестов

tests/SnakeGroove.Core.Tests/
  SnakeGroove.Core.Tests.csproj    # standalone .NET 8 test host

.github/workflows/
  core-tests.yml                   # GitHub Actions workflow
```

Тесты не дублируются. Один и тот же исходный код из package-папок компилируется двумя способами:

- Unity использует `SnakeGroove.Core.asmdef` и `SnakeGroove.Core.Tests.asmdef`;
- .NET 8 test project подключает runtime и test `.cs`-файлы через `<Compile Include=...>`.

## Что проверяют 8 тестов

1. Обычный тик возвращает `Continue` и корректный снимок.
2. Команда разворота на 180 градусов игнорируется.
3. Еда увеличивает длину и счёт и возвращает сведения о событии.
4. Заполнение последней свободной клетки завершает уровень.
5. Удар о стену даёт `GameOverReason.HitWall`.
6. Удар в себя даёт `GameOverReason.HitSelf`.
7. `GameSessionFactory` создаёт запущенную сессию с корректным снимком.
8. Пересекающиеся стартовые сегменты отклоняются валидацией.

## Локальный запуск

Из корня репозитория:

```powershell
dotnet test tests/SnakeGroove.Core.Tests/SnakeGroove.Core.Tests.csproj
```

Проект использует `net8.0`, NUnit и `Microsoft.NET.Test.Sdk`. Unity для этой команды не нужен.

## Что делает GitHub Actions

Workflow `Core Tests` выполняет:

1. получение кода репозитория;
2. установку .NET 8 SDK;
3. восстановление пакетов;
4. запуск тестов в конфигурации Release;
5. сохранение `core-tests.trx` как артефакта `core-test-results`, даже если тесты упали.

## Где смотреть результат на GitHub

1. Открыть репозиторий.
2. Перейти во вкладку **Actions**.
3. Выбрать workflow **Core Tests**.
4. Открыть нужный запуск.
5. В job **Pure C# Core Tests** посмотреть шаг **Test**.

Зелёная галочка означает, что сборка тестового проекта и все core-тесты прошли. При ошибке подробности находятся в логе шага **Test** и в артефакте `core-test-results`.

## Что этот CI пока не проверяет

- компиляцию всего Unity-проекта;
- состояние сцен, объектов и компонентов;
- Play Mode;
- визуализацию, ввод, звук и сборки платформ.

Эти проверки остаются за Unity Test Runner и ручным Play Mode. После появления `SnakeRunner` и Unity-представления нужно добавить EditMode/PlayMode тесты. Их автоматизацию в CI следует оценивать отдельно, чтобы Unity license и тяжёлая сборка не замедляли быстрый core pipeline.

## Связанные документы

- [Текущая Фаза 1](./snake_phase_1_in_progress.md)
- [Архитектура](./SNAKE_ARCHITECTURE.md)
- [Unity to Codex MCP](./unity_codex_mcp.md)
