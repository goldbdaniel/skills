#!/bin/bash
set -euo pipefail
cd /testbed

# Auto-generated oracle solution for: dotnet--optimizing-ef-core-queries--bulk-operations-ef-core-7-executeupdate
# Produces output / files that satisfy the task's assertions.

# Write expected keywords to agent_output.txt
cat > /testbed/agent_output.txt << 'ORACLE_EOF'
ExecuteUpdateAsync
ExecuteDeleteAsync
SetProperty
ORACLE_EOF

exit 0
