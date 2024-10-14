.PHONY: format

format:
	dotnet fantomas .

build: format
	dotnet build

