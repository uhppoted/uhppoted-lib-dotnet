build: 
	dotnet build
	dotnet fantomas .

release: build
	rm -rf dist
	dotnet pack --configuration Release --output dist

