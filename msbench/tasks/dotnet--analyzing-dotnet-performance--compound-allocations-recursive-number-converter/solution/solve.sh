#!/bin/bash
set -euo pipefail
cd /testbed

# Auto-generated oracle solution for: dotnet--analyzing-dotnet-performance--compound-allocations-recursive-number-converter
# Produces output / files that satisfy the task's assertions.

# Write expected keywords to agent_output.txt
cat > /testbed/agent_output.txt << 'ORACLE_EOF'
ToLower
allocation
ORACLE_EOF

exit 0
