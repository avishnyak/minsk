#!/bin/bash

# Vars
slndir="$(dirname "${BASH_SOURCE[0]}")/src"

# Restore + Build
dotnet build "$slndir/EV2lang.sln" --nologo || exit

# Test
dotnet test "$slndir/EV2.Tests" --nologo --no-build
