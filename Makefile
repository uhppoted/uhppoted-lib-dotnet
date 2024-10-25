.PHONY: format

build-all:
	cd uhppoted        && make build && make test
	cd examples/fs/cli && make build
	cd examples/cs/cli && make build
	cd examples/vb/cli && make build

get-all-controllers:
	dotnet run --project examples/fs/cli get-all-controllers

get-controller:
	dotnet run --project examples/fs/cli get-controller
