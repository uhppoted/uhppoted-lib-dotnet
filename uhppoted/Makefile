CLI = dotnet run --project ../examples/fs/cli

.PHONY: format

format:
	dotnet fantomas .

build: format
	dotnet build

test:
	dotnet test

get-controllers: build
	$(CLI) get-all-controllers

get-controller: build
	$(CLI) get-controller

set-ip: build
	$(CLI) set-IPv4
