CLI = dotnet run --project ../examples/fs/cli --framework net7.0

.PHONY: format
.PHONY: build

format:
	dotnet fantomas .

build:
	dotnet build
	dotnet fantomas .

test:
	dotnet test

integration-tests:
	cd ../integration-tests && make test

debug: build
	dotnet run --project ../examples/fs/cli --framework net7.0 open-door
	dotnet run --project ../examples/cs/cli --framework net7.0 open-door
	dotnet run --project ../examples/vb/cli --framework net7.0 open-door

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

get-door: build
	$(CLI) get-door

set-door: build
	$(CLI) set-door

set-door-passcodes: build
	$(CLI) set-door-passcodes

open-door: build
	$(CLI) open-door


