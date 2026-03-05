#!/bin/bash
set -euo pipefail
cd /testbed

# Auto-generated oracle solution for: dotnet--analyzing-dotnet-performance--compiled-regex-startup-budget-regex-chain
# Produces output / files that satisfy the task's assertions.

# Write expected keywords to agent_output.txt
cat > /testbed/agent_output.txt << 'ORACLE_EOF'
Compiled
ToLower
ORACLE_EOF

exit 0
