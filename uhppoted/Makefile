CLI = dotnet run --project ../examples/fs/cli

.PHONY: format

format:
	dotnet fantomas .

build: format
	dotnet build

test:
	dotnet test

debug: build
#	dotnet run --project ../examples/fs/cli get-controller
#	dotnet run --project ../examples/cs/cli get-controller
	dotnet run --project ../examples/vb/cli get-controller

get-controllers: build
	$(CLI) get-all-controllers

get-controller: build
	$(CLI) get-controller

set-ip: build
	$(CLI) set-IPv4

get-listener: build
	$(CLI) get-listener

set-listener: build
	$(CLI) set-listener

get-time: build
	$(CLI) get-time
