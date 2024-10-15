.PHONY: format

format:
	dotnet fantomas .

build: format
	dotnet build

run: build
	dotnet run

get-controllers: build
	dotnet run get-controllers

