clean:
	dotnet clean

format:
	dotnet fantomas .

build: 
	dotnet build
	dotnet fantomas .

test: build
	dotnet test -p:TestTfmsInParallel=false

debug: build
	dotnet test -- NUnit.Where="test =~ /TestGetController/" 

