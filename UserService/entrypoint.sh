#!/bin/bash
set -e

# Выполнение миграций
dotnet ef database update

# Запуск приложения
exec dotnet UserService.dll
