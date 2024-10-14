.PHONY: format

format:
	dotnet fantomas .

build: format
	dotnet build

get-controllers: build
	dotnet run --project examples/cli get-controllers
