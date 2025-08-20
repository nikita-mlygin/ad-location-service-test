# AdLocationService

## Описание

Сервис для хранения и поиска рекламных площадок по локациям.  
используется структура данных **trie** для быстрых поисков.  
поддерживаются два метода api: загрузка данных из файла и поиск площадок по локации.  

## Требования

- .net 9.0 sdk  
- linux/windows/mac  

## Запуск

```bash
git clone https://github.com/<your-username>/ad-location-service.git
cd ad-location-service
dotnet run --project src/AdLocationService.Api
```

По умолчанию сервис будет доступен по адресу:

- [http://localhost:5193](http://localhost:5193)
- [https://localhost:7175](https://localhost:7175)

## Api

### Загрузка рекламных площадок

```txt
post /trie/upload-file
content-type: multipart/form-data

form-data:
file=<путь_к_файлу.txt>
```

Пример файла:

```txt
Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
Крутая реклама:/ru/svrd
```

### Поиск рекламных площадок

```txt
get /trie/search?path=/ru/svrd/revda
accept: application/json
```

Пример ответа:

```json
{
  "path": "/ru/svrd/revda",
  "platforms": [
    "Яндекс.Директ",
    "Ревдинский рабочий",
    "Крутая реклама"
  ]
}
```

## Тесты

```bash
dotnet test
```

## Нагрузочное тестирование

Для jmeter подготовлен файл `HTTP Request Defaults.jmx`
пример запуска (после старта сервиса):

```bash
jmeter -n -t "HTTP Request Defaults.jmx" -l results.jtl -e -o ./report
```

## Логирование

Используется **Serilog**:

- вывод в консоль
- вывод в файл `logs/log-<дата>.txt` (ротация раз в день, хранится 7 дней)
