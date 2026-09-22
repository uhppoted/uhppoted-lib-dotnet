# CHANGELOG

## Unreleased


## [0.9.1](https://github.com/uhppoted/uhppoted-lib-dotnet/releases/tag/v0.9.1) - 2026-09-22

### Added
1. `set-firstcard` API function to configure first-card mode for a controller managed door.
2. Added _first card_ privileges to `get-card`, `get-card-at-index` and `put-card`.
3. Added checks and retries for _connect to self_ in UDP and TCP transports.

### Updated
1. Removed support for .NET7 (end of life).
2. Reworked CLI examples to support `-bind`, `--broadcast` and `--listen` command line arguments.
3. Reworked CLI examples to replace internal lookup list with `--dest` command line argument.
4. Migrated from _github.com/uhppoted/uhppoted-lib-dotnet_ to _codeberg.org/uhppoted/uhppoted-lib-dotnet_.


## [0.9.0](https://github.com/uhppoted/uhppoted-lib-dotnet/releases/tag/v0.9.0) - 2026-01-27

### Added
1. Updated to build for .NET10


## [0.8.11](https://github.com/uhppoted/uhppoted-lib-dotnet/releases/tag/v0.8.11) - 2025-07-01

### Added
1. `get-antipassback` and `set-antipassback API functions.


## [0.8.10](https://github.com/uhppoted/uhppoted-lib-dotnet/releases/tag/v0.8.10) - 2025-01-30

1. Initial release.
