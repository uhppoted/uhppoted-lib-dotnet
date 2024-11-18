CLI = dotnet run --framework net7.0

.PHONY: format

format:
	dotnet fantomas .

build: format
	dotnet build

run: build
	$(CLI)

debug:
	$(CLI) set-IPv4  --controller 303986753 --address 192.168.1.222 --netmask 255.254.253.251 --gateway 192.168.0.0

find-controllers: build
	$(CLI) find-controllers

get-controller: build
	$(CLI) get-controller --controller 405419896

set-ip: build
	$(CLI) set-IPv4  --controller 405419896 --address 192.168.1.100 --netmask 255.255.255.0 --gateway 192.168.1.1

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

set-door-passcodes: build
	$(CLI) set-door-passcodes --controller 405419896

open-door: build
	$(CLI) open-door --controller 405419896

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

delete-card: build
	$(CLI) delete-card

