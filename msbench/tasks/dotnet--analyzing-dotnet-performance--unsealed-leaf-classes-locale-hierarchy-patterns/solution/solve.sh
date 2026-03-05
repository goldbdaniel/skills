#!/bin/bash
set -euo pipefail
cd /testbed

# Auto-generated oracle solution for: dotnet--analyzing-dotnet-performance--unsealed-leaf-classes-locale-hierarchy-patterns
# Produces output / files that satisfy the task's assertions.

# Write expected keywords to agent_output.txt
cat > /testbed/agent_output.txt << 'ORACLE_EOF'
sealed
ORACLE_EOF

exit 0
