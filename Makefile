.PHONY: format

build-all:
	cd uhppoted && make build && make test

get-controllers:
	dotnet run --project examples/fs/cli get-controllers
