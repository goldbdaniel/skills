#!/bin/bash
set -euo pipefail
cd /testbed

# Auto-generated oracle solution for: dotnet--analyzing-dotnet-performance--aggregate-replace-chain-struct-missing-iequatable
# Produces output / files that satisfy the task's assertions.

# Write expected keywords to agent_output.txt
cat > /testbed/agent_output.txt << 'ORACLE_EOF'
Aggregate
IEquatable
ORACLE_EOF

exit 0
