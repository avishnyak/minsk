#!/bin/bash

# Vars
slndir="$(dirname "${BASH_SOURCE[0]}")/src"

# Restore + Build
dotnet build "$slndir/ev2c" --nologo || exit

# Run
dotnet run -p "$slndir/ev2c" --no-build -- "$@"
