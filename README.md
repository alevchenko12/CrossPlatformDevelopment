# CrossPlatformDevelopment  
Лабораторна робота з дисципліни Розробка кросплатформених додатків  
Левченко Анастасія ІПЗ 32  

## Вимоги
- Встановлений .NET SDK (завантажити можна [тут](https://dotnet.microsoft.com/download))
- MSBuild

## Кроки для завантаження та запуску коду
### Запускаються відразу усі доступні Лаби і Тести до них
Виконайте наступні кроки для завантаження репозиторію та запуску проекту:
1. Завантажте репозиторій у вигляді ZIP файлу з GitHub.
2. Відкрийте термінал Developer PowerShell for VS 2022.
3. Перейдіть до директорії проекту:  
   `cd path/to/extracted-repo`
4. Білд Лабораторної роботи:
   ```bash
   dotnet build Build.proj -t:Build -p:Solution=LAB1
   ```
5. Запуск Лабораторної роботи:  
   ```bash
   dotnet build Build.proj -t:Run -p:Solution=LAB1
   ```
6. Тестування Лабораторної роботи:
   ```bash
   dotnet build Build.proj -t:Test -p:Solution=LAB1
   ```
