.PHONY: format

format:
	# dotnet format -v diag
	dotnet format

build: format
	dotnet build

run: build
	dotnet run

get-all-controllers: build
	dotnet run get-all-controllers

