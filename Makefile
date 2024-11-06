CLI = dotnet run --project ./examples/fs/cli --framework net7.0

.PHONY: format
.PHONY: integration-tests

build-all:
	cd uhppoted        && make build && make test
	cd examples/fs/cli && make build
	cd examples/cs/cli && make build
	cd examples/vb/cli && make build

integration-tests: 
	cd integration-tests && make test

get-controllers:
	$(CLI) get-all-controllers

get-controller:
	$(CLI) get-controller

set-ipv4:
	$(CLI) set-IPv4

get-listener:
	$(CLI) get-listener

set-listener:
	$(CLI) set-listener

get-time:
	$(CLI) get-time

set-time:
	$(CLI) set-time

get-door-settings:
	$(CLI) get-door-settings

set-door-settings:
	$(CLI) set-door-settings
