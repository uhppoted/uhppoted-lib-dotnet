CLI = dotnet run --framework net7.0

.PHONY: format

format:
	dotnet fantomas .

build: format
	dotnet build

run: build
	$(CLI)

get-controllers: build
	$(CLI) get-all-controllers

get-controller: build
	$(CLI) get-controller

set-ip: build
	$(CLI) set-IPv4

get-listener: build
	$(CLI) get-listener --controller 405419896

set-listener: build
	$(CLI) set-listener --controller 405419896

get-time: build
	$(CLI) get-time --controller 405419896

set-time: build
	$(CLI) set-time --controller 405419896

get-door: build
	$(CLI) get-door --controller 405419896

set-door: build
	$(CLI) set-door --controller 405419896
