CLI = dotnet run --project ../examples/fs/cli

.PHONY: format

format:
	dotnet fantomas .

build: format
	dotnet build

test:
	dotnet test

get-controllers: build
	$(CLI) get-controllers
