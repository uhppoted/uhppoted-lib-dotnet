CLI = dotnet run --project ../examples/fs/cli

.PHONY: format
.PHONY: build

format:
	dotnet fantomas .

build:
	dotnet build
	dotnet fantomas .

test:
	dotnet test

debug: build
	dotnet run --project ../examples/fs/cli get-door-settings
	dotnet run --project ../examples/cs/cli get-door-settings
	dotnet run --project ../examples/vb/cli get-door-settings

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

set-time: build
	$(CLI) set-time

get-door-settings: build
	$(CLI) get-door-settings

