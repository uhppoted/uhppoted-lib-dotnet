clean:
	dotnet clean
	rm -rf bin
	rm -rf obj
	rm -rf dist

build: 
	export VERSION="" && dotnet build
	dotnet fantomas .

test: 
	export VERSION="" && dotnet test

release-debug: build test
	rm -rf dist
	echo "# uhppoted" > README.md
	tail -n+6 ../../README.md >> README.md
	dotnet build --configuration Release
	nuget pack uhppoted-net7.nuspec -Symbols -SymbolPackageFormat snupkg -OutputDirectory dist

release: build test
	rm -rf dist
	echo "# uhppoted" > README.md
	tail -n+6 ../../README.md >> README.md
	dotnet pack --include-symbols --configuration Release --output dist

publish-github: 
	dotnet nuget push dist --no-symbols --source "github" --api-key "${GITHUB_API_KEY}" --skip-duplicate

publish-nuget: 
	dotnet nuget push dist --source https://api.nuget.org/v3/index.json --api-key ${NUGET_API_KEY} --skip-duplicate

publish: publish-github publish-nuget
