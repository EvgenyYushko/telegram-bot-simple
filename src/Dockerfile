# Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Копируем файлы проекта в контейнер
COPY *.sln ./
COPY *.csproj .

# Проверяем содержимое рабочего каталога
RUN ls -la

# Восстанавливаем зависимости
RUN dotnet restore ConsoleApp1.sln

# Копируем остальные файлы и собираем проект
COPY . ./

# Проверяем содержимое рабочего каталога
RUN ls -la

# Сборка и публикация
RUN dotnet publish ConsoleApp2.csproj  -c Release -o /publish

# Используем официальный образ .NET Runtime для запуска
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

# Копируем собранное приложение из предыдущего этапа
COPY --from=build /publish .

# Проверяем содержимое рабочего каталога
RUN ls -la

# Указываем команду для запуска приложения
ENTRYPOINT ["/bin/bash", "-c", "exec dotnet ConsoleApp2.dll"]
№ ENTRYPOINT ["dotnet", "ConsoleApp2.dll"]
