.PHONY: format

format:
	dotnet fantomas .

build: 
	dotnet build
	dotnet fantomas .

test: build
	dotnet test

debug: build
	dotnet test -- NUnit.Where="test =~ /TestGetDoorSettings/" 

