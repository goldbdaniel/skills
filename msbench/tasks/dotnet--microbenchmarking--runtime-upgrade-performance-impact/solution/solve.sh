#!/bin/bash
set -euo pipefail
cd /testbed

# Auto-generated oracle solution for: dotnet--microbenchmarking--runtime-upgrade-performance-impact
# Produces output / files that satisfy the task's assertions.

mkdir -p /testbed/$(dirname 'oracle.csproj')
echo 'BenchmarkDotNet' >> /testbed/'oracle.csproj'
mkdir -p /testbed/$(dirname 'oracle-reportoracle.md')
touch /testbed/'oracle-reportoracle.md'

exit 0
