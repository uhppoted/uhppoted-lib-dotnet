CLI = dotnet run --framework net7.0

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

get-status:
	$(CLI) get-status

get-cards: build
	$(CLI) get-cards

get-card: build
	$(CLI) get-card

get-card-at-index: build
	$(CLI) get-card-at-index

put-card: build
	$(CLI) put-card

