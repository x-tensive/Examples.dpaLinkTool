# Examples.dpaLinkTool

dpaLinkTool - это пример вызова REAST API DPA и применение коннекторов для передачи данных из DPA в другие системы.

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
    "type": connectorType,
    "params": {
      "paramName1": "paramExpression1",
      "paramName2": "paramExpression3",
      "paramName3": "paramExpression4"
    }
  } 
}
```
