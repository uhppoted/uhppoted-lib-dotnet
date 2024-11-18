CLI = dotnet run --framework net7.0
CONTROLLER ?= 405419896

.PHONY: format

format:
	dotnet format

build: format
	dotnet build

run: build
	$(CLI)

find-controllers: build
	$(CLI) find-controllers

get-controller: build
	$(CLI) get-controller --controller $(CONTROLLER)

set-ip: build
	$(CLI) set-IPv4 --controller $(CONTROLLER)

get-listener: build
	$(CLI) get-listener --controller $(CONTROLLER)

set-listener: build
	$(CLI) set-listener --controller $(CONTROLLER)

get-time: build
	$(CLI) get-time --controller $(CONTROLLER)

set-time: build
	$(CLI) set-time --controller $(CONTROLLER)

get-door: build
	$(CLI) get-door --controller $(CONTROLLER)

set-door: build
	$(CLI) set-door --controller $(CONTROLLER)

set-door-passcodes: build
	$(CLI) set-door-passcodes --controller $(CONTROLLER)

open-door: build
	$(CLI) open-door --controller $(CONTROLLER)

get-status:
	$(CLI) get-status --controller $(CONTROLLER)

get-cards: build
	$(CLI) get-cards --controller $(CONTROLLER)

get-card: build
	$(CLI) get-card --controller $(CONTROLLER)

get-card-at-index: build
	$(CLI) get-card-at-index --controller $(CONTROLLER)

put-card: build
	$(CLI) put-card --controller $(CONTROLLER)

delete-card: build
	$(CLI) delete-card --controller $(CONTROLLER)

