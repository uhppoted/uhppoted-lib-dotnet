build: 
	dotnet build
	dotnet fantomas .

release: build
	rm -rf dist
	dotnet pack --configuration Release --output dist

get-all-controllers: build
	dotnet run --project ../../examples/fs/cli get-all-controllers

get-controller: build
	# dotnet run --project ../../examples/fs/cli get-controller --controller 405419896
	dotnet run --project ../../examples/fs/cli get-controller

set-ip: build
	dotnet run --project ../../examples/fs/cli set-IPv4
