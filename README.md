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
dpaLinkTool.exe get equipment
```
Список рабочих центров в формате json будет выведен в STDOUT.

# Список индикаторов

```cmd
dpaLinkTool.exe get indicators
```
Список индикаторов в формате json будет выведен в STDOUT.

# Значения индикаторов

```cmd
dpaLinkTool.exe push indicators --from "20.10.2020 00:00:00" --to "20.10.2020 04:00:00" --cfg "cfg.xml"
```
Запросить значения индикаторов за период [from, to], далее использовать коннекторы, чтобы передать значения индикаторов в другие системы. Перечень индикаторов и коннекторы определяются в файле "cfg.xml":

```xml
<?xml version="1.0" encoding="utf-8"?>
<ArrayOfEquipmentConnectorCfg xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EquipmentConnectorCfg>
    <ID>26</ID>
    <Name>Alpha 700-IST-1</Name>
    <DepartmentName>Цех №1 / Подразделение Alpha</DepartmentName>
    <Indicators>
      <IndicatorConnectorCfg>
        <ID>60407578</ID>
        <Name>Скорость подачи Chanel 1, мм/мин</Name>
        <Device>Chanel 1</Device>
        <DeviceClass>Канал</DeviceClass>
        <Connector>out1</Connector>
      </IndicatorConnectorCfg>
      <IndicatorConnectorCfg>
        <ID>60407579</ID>
        <Name>Коррекция скорости подачи Chanel 1, %</Name>
        <Device>Chanel 1</Device>
        <DeviceClass>Канал</DeviceClass>
        <Connector>insert1</Connector>
      </IndicatorConnectorCfg>
      <IndicatorConnectorCfg>
        <ID>60407580</ID>
        <Name>Коррекция ускоренного хода Chanel 1, %</Name>
        <Device>Chanel 1</Device>
        <DeviceClass>Канал</DeviceClass>
        <Connector>insert1</Connector>
      </IndicatorConnectorCfg>
    </Indicators>
  </EquipmentConnectorCfg>
</ArrayOfEquipmentConnectorCfg>
```

# Генерация файла конфигурации индикторов и коннекторов

```cmd
dpaLinkTool.exe createConnectorsConfig indicators --fileName "cfg.xml"
```

# Пример настройки коннектора CONSOLE

```json
"out1": {
  "type": "console",
  "format": "ID={p0} : {p1} : {p2} = {p4} ({p3})",
  "params": {
    "p0": "equipment.ID",
    "p1": "equipment.Name",
    "p2": "indicator.Name",
    "p3": "value.TimeStamp.LocalDateTime",
    "p4": "value.Value.ToString()"
  }
}
```
# Пример настройки коннектора MSSQL

```json
"insert1": {
  "type": "mssql",
  "connection": "Integrated Security=true;Initial Catalog=IntegrationData;Server=.",
  "command": "INSERT INTO [dbo].[FloatIndicators] ([EquipmentID], [EquipmentName], [IndicatorID], [IndicatorName], [TimeStamp], [Value]) VALUES (@equipmentId, @equipmentName, @indicatorId, @indicatorName, @timeStamp, @value)",
  "params": {
    "@equipmentId": "equipment.ID",
    "@equipmentName": "equipment.Name",
    "@indicatorId": "indicator.ID",
    "@indicatorName": "indicator.Name",
    "@timeStamp": "value.TimeStamp",
    "@value": "value.Value.GetDouble()"
  }
}
```

# Импорт данных за предыдущий час
```powershell
$currentTime = Get-Date
$periodTo = $currentTime.Date.AddHours($currentTime.Hour)
$periodFrom = $periodTo.AddHours(-1)
dpaLinkTool.exe push indicators --from "$periodFrom" --to "$periodTo" --cfg "cfg.xml"
```
