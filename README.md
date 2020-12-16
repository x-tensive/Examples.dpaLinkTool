# Examples.dpaLinkTool

This project contains an example code of how to call DPA REST API and how to utilize "connectors" to push data from DPA to external systems.

# appsettings.json

Base url and account to access REST API:
```json
"dpa": {
  "baseUrl": "http://dpadev.intranet.x-tensive.com",
  "userName": "admin",
  "password": "admin"
}
```

Connectors configuration:
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

# Get a list of equipment

```powershell
dpaLinkTool.exe get equipment
```
The json result is sent to STDOUT.

# Write data to CNC

```powershell
dpaLinkTool.exe set value --equipmentId 112233 --url "/Channel/Parameter/R[123]" --value "3.14"
```
Set value 3.14 to R register №123 for Equipment with id 112233

```powershell
dpaLinkTool.exe set value --equipmentId 112233 --url "Some_URL"
```
Set empty string value to Some_URL for Equipment with id 112233

# Get a list of indicators

```powershell
dpaLinkTool.exe get indicators
```
The json result is sent to STDOUT.

# Get indicator values

```powershell
dpaLinkTool.exe push indicators --from "20.10.2020 00:00:00" --to "20.10.2020 04:00:00" --cfg "cfg.xml"
```
Receives indicator values for the specified period [from, to], then utilizes connectors to transfer data to external system. The list of indicators and applied connectors are defined in "cfg.xml":

```xml
<?xml version="1.0" encoding="utf-8"?>
<ArrayOfEquipmentConnectorCfg xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EquipmentConnectorCfg>
    <ID>26</ID>
    <Name>Alpha 700-IST-1</Name>
    <DepartmentName>Area №1 / Alpha location</DepartmentName>
    <Indicators>
      <IndicatorConnectorCfg>
        <ID>60407578</ID>
        <Name>Feedrate Channel 1, mm/min</Name>
        <Device>Channel 1</Device>
        <DeviceClass>Channel</DeviceClass>
        <Connector>out1</Connector>
      </IndicatorConnectorCfg>
      <IndicatorConnectorCfg>
        <ID>60407579</ID>
        <Name>Feedrate override Chanel 1, %</Name>
        <Device>Channel 1</Device>
        <DeviceClass>Channel</DeviceClass>
        <Connector>insert1</Connector>
      </IndicatorConnectorCfg>
      <IndicatorConnectorCfg>
        <ID>60407580</ID>
        <Name>Rapid traverse override Channel 1, %</Name>
        <Device>Channel 1</Device>
        <DeviceClass>Channel</DeviceClass>
        <Connector>insert1</Connector>
      </IndicatorConnectorCfg>
    </Indicators>
  </EquipmentConnectorCfg>
</ArrayOfEquipmentConnectorCfg>
```

# To generate a configuration file with a list of indicators and applied connectors

```powershell
dpaLinkTool.exe createConnectorsConfig indicators --fileName "cfg.xml"
```

# An example of CONSOLE connector configuration

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

# An example of MSSQL connector configuration

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

# An example of INFLUXDB connector configuration

```json
"influxdb1": {
  "type": "influxdb",
  "url": "http://192.168.0.249:8086",
  "token": "O-YmomQnsZhGf_Fa56e88871sPssp-v3jwFG8jG94kPhtXkcQlRndDaQ0CZfGkpsdpBf6QbI6DQ==",
  "org": "X-tensive",
  "bucket": "default",
  "params": {
    "equipment": "equipment.Name",
    "indicator": "indicator.Name"
  }
}
```

# To upload the data of the previous hour

```powershell
$currentTime = Get-Date
$periodTo = $currentTime.Date.AddHours($currentTime.Hour)
$periodFrom = $periodTo.AddHours(-1)
$periodToStr = $periodTo.ToString()
$periodFromStr = $periodFrom.ToString()
dpaLinkTool.exe push indicators --from "$periodFromStr" --to "$periodToStr" --cfg "cfg.xml"
```
