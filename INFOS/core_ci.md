# Snake Groove — Core CI

## Что проверяет CI

GitHub Actions запускает только pure C# тесты ядра, без Unity Editor.

Workflow:

```text
.github/workflows/core-tests.yml
```

Тестовый проект:

```text
tests/SnakeGroove.Core.Tests/SnakeGroove.Core.Tests.csproj
```

Этот проект:
- компилирует runtime-файлы из `SnakeGroove/Packages/com.jochen.snakegroovecore/Runtime`;
- переиспользует package tests из `SnakeGroove/Packages/com.jochen.snakegroovecore/Tests/Runtime`;
- запускается обычной командой `dotnet test`;
- не требует Unity license и Unity Test Runner.

## Локальный запуск

```powershell
dotnet test tests/SnakeGroove.Core.Tests/SnakeGroove.Core.Tests.csproj
```

## Где смотреть в GitHub

1. Открой репозиторий на GitHub.
2. Перейди во вкладку **Actions**.
3. Выбери workflow **Core Tests**.
4. Открой последний run.
5. В job **Pure C# Core Tests** смотри шаг **Test**.

Если всё хорошо, run будет зелёным, а в артефактах будет `core-test-results` с `.trx` результатом тестов.

## Что остаётся отдельно

Unity package tests по-прежнему можно запускать в Unity:

```text
Window -> General -> Test Runner -> EditMode -> Run All
```

Unity Test Runner в CI можно добавить позже, когда будет смысл поднимать Unity Editor в GitHub Actions.
