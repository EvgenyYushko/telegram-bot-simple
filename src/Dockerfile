# Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Копируем файлы проекта в контейнер
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы и собираем проект
COPY . ./
RUN dotnet publish -c Release -o /publish

# Используем официальный образ .NET Runtime для запуска
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Копируем собранное приложение из предыдущего этапа
COPY --from=build /publish .

# Указываем команду для запуска приложения
ENTRYPOINT ["dotnet", "ConsoleApp1.sln"]
