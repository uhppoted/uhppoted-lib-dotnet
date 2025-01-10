build: 
	dotnet build
	dotnet fantomas .

release: build
	rm -rf dist
	dotnet pack --configuration Release --output dist

publish-github: 
	dotnet nuget push dist/uhppoted-dotnet.0.8.9.2-alpha.nupkg  --source "github" --api-key "${GITHUB_API_KEY}"
	dotnet nuget push dist/uhppoted-dotnet.0.8.9.2-alpha.snupkg --source "github" --api-key "${GITHUB_API_KEY}"

publish-nuget: 
	dotnet nuget push dist/uhppoted-dotnet.0.8.9.2-alpha.nupkg  --source https://api.nuget.org/v3/index.json --api-key ${NUGET_API_KEY}
	dotnet nuget push dist/uhppoted-dotnet.0.8.9.2-alpha.snupkg --source https://api.nuget.org/v3/index.json --api-key ${NUGET_API_KEY}

publish: release
