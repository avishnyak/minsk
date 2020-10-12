#!/bin/bash

# Vars
slndir="$(dirname "${BASH_SOURCE[0]}")/src"

# Restore + Build
dotnet build "$slndir/ev2i" --nologo || exit

# Run
dotnet run -p "$slndir/ev2i" --no-build
