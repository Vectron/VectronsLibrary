nuget pack VectronsLibrary.TextBlockLogger.csproj -Build -Symbols -Properties Configuration=Release
dotnet nuget push VectronsLibrary.TextBlockLogger.1.0.0.nupkg -k oy2d6c7k6co2p67cozdqhyx4epcrhmduxoxnwgtd3kcw7m -s https://api.nuget.org/v3/index.json
pause