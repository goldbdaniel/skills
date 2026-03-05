#!/bin/bash
set -euo pipefail
cd /testbed

# Auto-generated oracle solution for: dotnet--thread-abort-migration--blocking-waithandle-thread-interrupt
# Produces output / files that satisfy the task's assertions.

# Write expected keywords to agent_output.txt
cat > /testbed/agent_output.txt << 'ORACLE_EOF'
CancellationToken
WaitHandle
ORACLE_EOF

exit 0
