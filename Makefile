CLI = dotnet run --project ./examples/fs/cli --framework net7.0 --controller $(CONTROLLER)

CONTROLLER ?= 405419896

.PHONY: format
.PHONY: integration-tests

build-all:
	cd uhppoted        && make build && make test
	cd examples/fs/cli && make build
	cd examples/cs/cli && make build
	cd examples/vb/cli && make build

integration-tests: 
	cd integration-tests && make test

release:
	cd uhppoted && make release

find-controllers:
	$(CLI) find-controllers

get-controller:
	$(CLI) get-controller --controller $(CONTROLLER)

set-ipv4:
	$(CLI) set-IPv4 --controller $(CONTROLLER)

get-listener:
	$(CLI) get-listener --controller $(CONTROLLER)

set-listener:
	$(CLI) set-listener --controller $(CONTROLLER)

get-time:
	$(CLI) get-time --controller $(CONTROLLER) --controller $(CONTROLLER)

set-time:
	$(CLI) set-time --controller $(CONTROLLER)

get-door:
	$(CLI) get-door --controller $(CONTROLLER)

set-door:
	$(CLI) set-door --controller $(CONTROLLER)

set-door-passcodes:
	$(CLI) set-door-passcodes --controller $(CONTROLLER)

open-door:
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

delete-all-cards: build
	$(CLI) delete-all-cards --controller $(CONTROLLER) --controller $(CONTROLLER)

