Start-Process Powershell.exe -Argumentlist "-file dashboard.ps1"
Start-Process Powershell.exe -Argumentlist "-file launch-customers.ps1"
Start-Process Powershell.exe -Argumentlist "-file launch-products.ps1"
Start-Process Powershell.exe -Argumentlist "-file launch-orders.ps1"
Start-Process Powershell.exe -Argumentlist "-file launch-notifications.ps1"
PowerShell -File "launch-dashboard.ps1" -NoExit