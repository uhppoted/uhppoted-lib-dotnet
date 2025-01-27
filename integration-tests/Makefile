clean:
	dotnet clean

format:
	dotnet fantomas .

build: 
	export VERSION="" && dotnet build
	dotnet fantomas .

test: build
	export VERSION="" && dotnet test -p:TestTfmsInParallel=false

debug: build
	dotnet test -- NUnit.Where="test =~ /TestGetController/" 

