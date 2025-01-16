clean:
	dotnet clean

build: 
	dotnet build
	dotnet fantomas .

test: 
	dotnet test

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
