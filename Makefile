CLI = dotnet run --project ./examples/fs/cli --framework net7.0 --controller $(CONTROLLER)

CONTROLLER ?= 405419896
CARD ?= 10058400
MISSING_CARD ?= 10058399
ADDRESS ?= 192.168.1.100
NETMASK ?= 255.255.255.0
GATEWAY ?= 192.168.1.1
LISTENER ?= 192.168.1.100:60001
INTERVAL ?= 30
EVENT_INDEX ?= 23
RECORD_SPECIAL_EVENTS ?= true
TIME_PROFILE ?= 29

clean:
	cd uhppoted && make clean
	cd integration-tests && make clean
	cd examples/fs/cli && make clean
	cd examples/cs/cli && make clean
	cd examples/vb/cli && make clean

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

set-IPv4:
	$(CLI) set-IPv4 --controller $(CONTROLLER) --address $(ADDRESS) --netmask $(NETMASK) --gateway $(GATEWAY)

get-listener:
	$(CLI) get-listener --controller $(CONTROLLER)

set-listener:
	$(CLI) set-listener --controller $(CONTROLLER) --listener $(LISTENER) --interval $(INTERVAL)

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
	$(CLI) get-card --controller $(CONTROLLER) --card $(CARD)
	$(CLI) get-card --controller $(CONTROLLER) --card $(MISSING_CARD)

get-card-at-index: build
	$(CLI) get-card-at-index --controller $(CONTROLLER)

put-card: build
	$(CLI) put-card --controller $(CONTROLLER) --card $(CARD)

delete-card: build
	$(CLI) delete-card --controller $(CONTROLLER) --card $(CARD)

delete-all-cards: build
	$(CLI) delete-all-cards --controller $(CONTROLLER) --controller $(CONTROLLER)

get-event: build
	$(CLI) get-event --controller $(CONTROLLER) --index $(EVENT_INDEX)

get-event-index: build
	$(CLI) get-event-index --controller $(CONTROLLER)

set-event-index: build
	$(CLI) set-event-index --controller $(CONTROLLER) --index $(EVENT_INDEX)

record-special-events:build
	$(CLI) record-special-events --controller $(CONTROLLER) --enable $(RECORD_SPECIAL_EVENTS)

get-time-profile:build
	$(CLI) get-time-profile --controller $(CONTROLLER) --profile $(TIME_PROFILE)

