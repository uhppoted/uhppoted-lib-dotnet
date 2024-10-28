.PHONY: format

format:
	dotnet fantomas .

build: format
	dotnet build

test: build
	dotnet test --logger:console;verbosity=detailed

