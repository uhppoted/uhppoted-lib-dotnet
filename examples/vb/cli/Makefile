CLI = dotnet run --framework net7.0
CONTROLLER ?= 405419896
CARD ?= 10058400
MISSING_CARD ?= 10058399
ADDRESS ?= 192.168.1.100
EVENT_INDEX ?= 23

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

set-IPv4: build
	$(CLI) set-IPv4 --controller $(CONTROLLER) --address $(ADDRESS)

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
	$(CLI) get-card --controller $(CONTROLLER) --card $(CARD)
	$(CLI) get-card --controller $(CONTROLLER) --card $(MISSING_CARD)

get-card-at-index: build
	$(CLI) get-card-at-index --controller $(CONTROLLER)

put-card: build
	$(CLI) put-card --controller $(CONTROLLER) --card $(CARD)

delete-card: build
	$(CLI) delete-card --controller $(CONTROLLER) --card $(CARD)

delete-all-cards: build
	$(CLI) delete-all-cards --controller $(CONTROLLER)

get-event: build
	$(CLI) get-event --controller $(CONTROLLER) --index $(EVENT_INDEX)

