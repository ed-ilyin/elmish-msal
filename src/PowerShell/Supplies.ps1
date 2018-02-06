$ht = @{}

Get-Content ./input/0010148470.txt `
    | ConvertFrom-Csv -Delimiter "`t" `
    | ForEach-Object { $ht.($_.EAN) = $_.Description }
    # | Where-Object EAN -ne '' `
    # | Select-Object @{Name = 'id'; Expression = {$_.EAN}}, Description `
    # | ConvertTo-Json `
    # | Set-Content 'supplies.json'

$ht | ConvertTo-Json | Set-Content 'supplies.json'
