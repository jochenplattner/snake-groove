# SnakeGroove.Core.Tests

Standalone .NET test project for the pure C# core.

It compiles the runtime sources from `SnakeGroove/Packages/com.jochen.snakegroovecore/Runtime` and reuses the package tests from `SnakeGroove/Packages/com.jochen.snakegroovecore/Tests/Runtime`.

This keeps one test suite for both:
- Unity Test Runner / EditMode;
- GitHub Actions without Unity.

Run locally:

```powershell
dotnet test tests/SnakeGroove.Core.Tests/SnakeGroove.Core.Tests.csproj
```
