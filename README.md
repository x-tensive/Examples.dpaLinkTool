# Examples.dpaLinkTool

dpaLinkTool - это пример вызова REST API DPA и применение коннекторов для передачи данных из DPA в другие системы.

# appsettings.json

Базовый адрес DPA и учетная запись для вызовов REST API:
```json
"dpa": {
  "baseUrl": "http://dpadev.intranet.x-tensive.com",
  "userName": "admin",
  "password": "admin"
}
```

Настройка коннекторов:
```json
"connectors": {
  "connectorName": {
    "type": "connectorType",
    "params": {
      "paramName1": "paramExpression1",
      "paramName2": "paramExpression3",
      "paramName3": "paramExpression4"
    }
  } 
}
```

# Список рабочих центров

```cmd
dpaLinkTool get equipment
```
Список рабочих центров в формате json будет выведен в STDOUT.

# Список индикаторов

```cmd
dpaLinkTool get indicators
```
Список индикаторов в формате json будет выведен в STDOUT.

# Значения индикаторов

```cmd
dpaLinkTool.exe push indicators --from "20.10.2020 00:00:00" --to "20.10.2020 04:00:00" --cfg "cfg.xml"
```
Запросить значения индикаторов за период [from, to], далее использовать коннекторы, чтобы передать значения индикаторов в другиес системы. Перечень индикаторов и коннекторы определяются в файле "cfg.xml"
