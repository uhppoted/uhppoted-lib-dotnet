PACKAGE = "dist/uhppoted-test.0.8.9.2-alpha.nupkg"

build: 
	dotnet build
	dotnet fantomas .

release: build
	rm -rf dist
	tail -n+5 ../../README.md > README.md
	dotnet pack --include-symbols --configuration Release --output dist

publish-github: 
	# dotnet nuget push dist/uhppoted-test.0.8.9.2-alpha.nupkg  --source "github" --api-key "${GITHUB_API_KEY}" --skip-duplicate
	# dotnet nuget push dist/uhppoted-test.0.8.9.2-alpha.snupkg --source "github" --api-key "${GITHUB_API_KEY}" --skip-duplicate
    dotnet nuget add source --username uhppoted \
                            --password ${{ secrets.GITHUB_TOKEN }} \
                            --store-password-in-clear-text \
                            --name github "https://nuget.pkg.github.com/uhppoted/index.json"
	dotnet nuget push dist/uhppoted-test.0.8.9.2-alpha.nupkg  --source "github" --skip-duplicate

publish-nuget: 
	dotnet nuget push dist/uhppoted-test.0.8.9.2-alpha.nupkg  --source https://api.nuget.org/v3/index.json --api-key ${NUGET_API_KEY} --skip-duplicate
	# dotnet nuget push dist/uhppoted-test.0.8.9.2-alpha.snupkg --source https://api.nuget.org/v3/index.json --api-key ${NUGET_API_KEY}

publish: publish-github
