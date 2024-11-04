.PHONY: format

format:
	dotnet fantomas .

build: format
	dotnet build

run: build
	dotnet run

get-all-controllers: build
	dotnet run get-all-controllers

get-controller: build
	dotnet run get-controller

set-ip: build
	dotnet run set-IPv4

get-listener: build
	dotnet run get-listener --controller 405419896

set-listener: build
	dotnet run set-listener --controller 405419896

get-time: build
	dotnet run get-time --controller 405419896

set-time: build
	dotnet run set-time --controller 405419896
