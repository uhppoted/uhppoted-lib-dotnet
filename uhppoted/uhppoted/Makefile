PACKAGE = dist/uhppoted-test.0.8.9.3-alpha.nupkg
SYMBOLS = dist/uhppoted-test.0.8.9.3-alpha.snupkg

build: 
	dotnet build
	dotnet fantomas .

release: build
	rm -rf dist
	tail -n+5 ../../README.md > README.md
	dotnet pack --include-symbols --configuration Release --output dist

publish-github: 
	dotnet nuget push $(PACKAGE) --source "github" --api-key "${GITHUB_API_KEY}" --skip-duplicate
	dotnet nuget push $(SYMBOLS) --source "github" --api-key "${GITHUB_API_KEY}" --skip-duplicate

publish-nuget: 
	dotnet nuget push $(PACKAGE)  --source https://api.nuget.org/v3/index.json --api-key ${NUGET_API_KEY} --skip-duplicate
	# dotnet nuget push $(SYMBOLS) --source https://api.nuget.org/v3/index.json --api-key ${NUGET_API_KEY}

publish: publish-github
