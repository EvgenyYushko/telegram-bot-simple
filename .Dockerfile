name: .NET

on: 
  push:
    branches:
      - master

jobs:
  build:
    runs-on: windows-latest

    steps:
    - name: Checkout code
      uses: actions/checkout@v2

    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '5.0.x'

    - name: Restore dependencies
      run: dotnet restore ConsoleApp1.sln
         
    - name: Build solution
      run: dotnet build ConsoleApp1.sln.sln --configuration Release --no-restore


    # Вывод содержимого файла в консоль
    #- name: Print test results file
    #  run: cat ./SimpleCalculator.Tests/TestResults/test_results.trx 
