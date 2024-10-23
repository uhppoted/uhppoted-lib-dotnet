.PHONY: format

build-all:
	cd uhppoted && make build && make test

get-all-ontrollers:
	dotnet run --project examples/fs/cli get-all-controllers
