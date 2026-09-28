# Snake Groove - Unity to Codex MCP

> Статус: DONE - подключение настроено и проверено после перезапуска Codex и Unity Editor
>
> Тип задачи: вспомогательная задача 3.4 внутри Фазы 1
>
> Ограничение: MCP не является зависимостью игры и не должен задерживать `SnakeRunner`

## 1. Зачем это нужно

Codex уже может читать и менять файлы проекта. MCP должен добавить контролируемое взаимодействие с открытым Unity Editor, чтобы после изменения кода можно было проверить реальное состояние проекта:

- активную сцену и Hierarchy;
- компоненты на объектах;
- Unity Console и ошибки компиляции;
- окончание import/compile;
- EditMode и PlayMode tests;
- Play Mode;
- Game View и Scene View;
- позднее - воспроизводимые локальные сборки.

MCP не заменяет Git, code review, core-тесты, Unity Test Runner и ручную оценку игрового ощущения.

## 2. Выбранный подход

Выбран Unity CLI из официального репозитория Unity Technologies со встроенным MCP-сервером `unity mcp`.

Почему начинаем с него:

- привязка к конкретному Unity project path;
- настройка Codex;
- доступ к открытому Editor;
- инструменты для сцен, Console, тестов, Play Mode, снимков и builds;
- возможность держать конфигурацию на уровне проекта.

Unity CLI и его MCP-возможности находятся в beta. Базовый proof of concept прошёл успешно. Сторонний Unity MCP остаётся fallback, если официальный вариант окажется нестабильным при дальнейшей работе.

## 3. Текущее состояние

- Unity-проект: `C:\PROJECTS\snake-groove\SnakeGroove`.
- Unity Editor: `6000.3.7f1`.
- Unity CLI: `1.0.0-beta.8`, установлен вместе с Unity Hub.
- Путь CLI: `C:\Program Files\Unity Hub\resources\cli\unity.exe`.
- Unity MCP настроен проектно в `.codex/config.toml` в корне репозитория.
- `com.unity.pipeline` версии `0.8.0-exp.1` добавлен в Unity-проект.
- Skills `unity-cli` и `unity-pipeline` установлены в `.agents/skills` в корне репозитория.
- Нативные инструменты `mcp__unity__*` загружаются в Codex после полного перезапуска приложения.
- Открытый Editor доступен через локальный Pipeline и сообщает состояние `ready`.
- Unity предоставляет 160 команд для работы с Editor.
- Core CI уже работает независимо от Unity.
- `SnakeRunner` ещё не реализован.

## 4. План выполнения задачи 3.4

### 4.1 Установка Unity CLI

Системную установку выполнять только после подтверждения владельца проекта.

После установки проверить:

```powershell
unity --version
unity --help
unity mcp --help
```

Установлена и проверена версия `1.0.0-beta.8`.

### 4.2 Проверка изменений проекта

До применения настройки:

1. посмотреть dry run, если установленная версия CLI его поддерживает;
2. проверить, какие строки будут добавлены в Unity package manifest;
3. проверить, какие файлы появятся в `.codex`;
4. не перезаписывать существующие MCP-настройки;
5. не помещать токены и секреты в репозиторий.

Ожидаемая проектная настройка, которую нужно сверить с `--help` установленной версии:

```powershell
unity mcp configure codex --local --project-path C:\PROJECTS\snake-groove\SnakeGroove --dry-run
unity mcp configure codex --local --project-path C:\PROJECTS\snake-groove\SnakeGroove
unity skill install codex --local
```

Команды в beta могут измениться. Документация не заменяет проверку локального `--help`.

Фактическая настройка использует абсолютный путь к `unity.exe`, потому что каталог Unity CLI пока не добавлен в `PATH` текущего процесса Codex.

### 4.3 Безопасная политика

- сервер работает локально и привязан только к `SnakeGroove`;
- read-only операции разрешаются отдельно от операций записи;
- изменение сцен, компонентов и ассетов требует подтверждения;
- произвольное выполнение C# и builds требуют подтверждения;
- массовые операции начинаются с проверки Git status;
- сцена сохраняется только после визуальной проверки;
- MCP не получает доступ к файлам вне проекта;
- существующие Codex MCP-серверы не удаляются;
- секреты не хранятся в tracked-файлах.

Предпочтительный режим Codex - одобрение операций записи, если установленный MCP-сервер корректно помечает read/write tools.

## 5. Smoke test

Proof of concept выполняется по порядку:

1. Codex видит запущенный Unity Editor и правильный project path.
2. Codex читает версию Unity и имя активной сцены.
3. Codex читает Hierarchy активной сцены.
4. Codex читает Unity Console.
5. Codex запрашивает refresh/recompile и дожидается результата.
6. Codex запускает EditMode package tests и получает структурированный итог.
7. Codex включает Play Mode, получает Game View screenshot и останавливает Play Mode.
8. В отдельной тестовой сцене создаётся временный `GameObject`.
9. Изменение отменяется через Undo и не загрязняет рабочие сцены.
10. Соединение восстанавливается после script recompile, domain reload и перезапуска Editor.

Рабочие сцены не используются для первого теста операций записи.

### Результат от 28.09.2026

- Editor `6000.3.7f1` найден по правильному project path, состояние `ready`.
- Прочитана активная сцена `MainMenu.unity`, её Hierarchy и компоненты.
- Unity Console: 0 ошибок, 0 предупреждений, компиляция успешна.
- Unity Test Runner нашёл и успешно выполнил 8 из 8 EditMode-тестов.
- Play Mode включён и остановлен через Pipeline.
- Получен Game View screenshot `1280x720` с главным меню.
- Временный `GameObject` создан, отменён через Unity Undo и не остался в сцене.
- Временный screenshot-ассет удалён; рабочие сцены не содержат итоговых изменений.
- После переходов Play Mode соединение восстановилось автоматически.
- После полного перезапуска Codex нативные Unity MCP tools загрузились в новую сессию.
- После штатного перезапуска Unity Editor MCP автоматически восстановил соединение.
- Повторная проверка прочитала `MainMenu.unity`, Hierarchy и Console: сцена чистая, ошибок и предупреждений нет.
- Команда принудительной проверки компиляции вернула `up_to_date`; соединение осталось рабочим.

## 6. Критерий решения

### Proof of concept успешен

Продолжаем использовать MCP, если:

- Editor однозначно привязан к нужному проекту;
- Console, Hierarchy и active scene читаются стабильно;
- tests и Play Mode запускаются;
- screenshot возвращается;
- write-операции требуют ожидаемого подтверждения;
- после reload соединение восстанавливается;
- в Git не появляется неожиданных изменений.

### Включаем fallback

Возвращаемся к обычному Unity workflow, если:

- установка или package ломает совместимость проекта;
- соединение регулярно теряется;
- tools не различают чтение и запись;
- невозможно надёжно выбрать нужный Editor;
- proof of concept начинает задерживать `SnakeRunner`.

Fallback означает Unity Editor + Test Runner + ручной Play Mode. Это нормальный рабочий путь, а не блокировка проекта.

## 7. Рабочий цикл после подключения

```text
Codex читает сцену и Console через MCP
  -> меняет C#-файлы
  -> Unity обновляет ассеты и компилирует
  -> Codex читает ошибки
  -> запускает тесты
  -> запускает Play Mode
  -> проверяет Game View
  -> изменения сцены сохраняются после проверки
```

Для `SnakeRunner` это позволит проверить `GameController`, компонент, Console и фактические тики `GameSession`.

Для визуализации это позволит проверить prefab, позиции сегментов, камеру и Game View без ручного чтения YAML сцены.

## 8. Критерий готовности задачи 3.4

Задача закрыта, когда:

- выбран MCP-вариант или явно зафиксирован fallback;
- успешный вариант привязан к `SnakeGroove`;
- read-only smoke tests пройдены;
- EditMode tests запускаются;
- Play Mode и screenshot работают;
- тестовая write/Undo операция не оставляет изменений;
- правила подтверждения зафиксированы;
- процедура повторяется после перезапуска Unity и Codex.

Все критерии выполнены 28.09.2026. Задача 3.4 завершена; официальный Unity MCP остаётся рабочим инструментом разработки с обычным Unity workflow в качестве fallback.

## 9. Место в roadmap

```text
Core + Core CI
  -> Unity to Codex MCP proof of concept
  -> SnakeRunner
  -> SnakeView и FoodView
  -> Unity integration tests
  -> build profiles и платформенные сборки
```

MCP помогает разрабатывать и проверять Unity-слой, но pure C# core CI продолжает работать без Unity.

## 10. Источники

- [OpenAI Codex MCP](https://developers.openai.com/codex/extend/mcp)
- [Unity Technologies skills and Unity CLI](https://github.com/Unity-Technologies/skills)
- [Unity CLI MCP reference](https://github.com/Unity-Technologies/skills/blob/main/skills/unity-cli/references/integration-advanced.md)
