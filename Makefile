.PHONY: format

build-all:
	cd uhppoted && make build

get-controllers:
	dotnet run --project examples/fs/cli get-controllers
