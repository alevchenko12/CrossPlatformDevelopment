# CrossPlatformDevelopment  
Лабораторна робота з дисципліни Розробка кросплатформених додатків  
Левченко Анастасія ІПЗ 32  

## Вимоги
- Встановлений .NET SDK (завантажити можна [тут](https://dotnet.microsoft.com/download))

## LAB #1-3 INSTRUCTIONS

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
Де `LAB1` може бути замінена на `LAB2`, щоб запустити лабораторну №2, `LAB3` - лабораторна №3, тощо

## LAB #4 INSTRUCTIONS
## Команди для запуску у віртуальних середовищах Windows, Ubuntu
```bash
dotnet run run lab1 -input LAB1\INPUT.TXT --output LAB1\OUTPUT.TXT
```
Де lab1 може бути замінена на номер бажаної лаби (1-3)
Також шлях для файлу Output може бути змінений як завгодно 

### На жаль, для встановлення Mac OS потрібно мати пристрій від Apple
[Відповідне посилання з доказом:](https://forums.virtualbox.org/viewtopic.php?f=6&t=92649)
![image](https://github.com/user-attachments/assets/455c9876-82d3-4182-a14e-33d40122b244)

## LAB5 INSTRUCTIONS
# Команди для запуску у віртуальних середовищах Windows, Ubuntu
1. Запуск віртуальної машини
```bash
vagrant up
```
2. Підключення до Ubuntu
```bash
vagrant ssh ubuntu
```
3. Виведення списку директорій
```bash
ls
```
4. Перехід в директорію з лабами
```bash
cd project
```
5. Перехід в папку з LAB5
```bash
cd LAB5
```
6. Запуск веб сайту
```bash
dotnet run --urls "https://0.0.0.0:7180"
```
