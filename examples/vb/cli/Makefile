.PHONY: format

format:
	dotnet format

build: format
	dotnet build

run: build
	dotnet run

get-controllers: build
	dotnet run get-all-controllers

get-controller: build
	dotnet run get-controller

set-ip: build
	dotnet run set-IPv4

get-listener: build
	dotnet run get-listener
