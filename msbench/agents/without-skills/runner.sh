#!/bin/bash
set -euo pipefail

METADATA_PATH="${METADATA_PATH:-/drop/metadata.json}"

# No skill directories — baseline Copilot CLI run
echo "{\"skill_injected\": false}" > /agent/skill_metadata.json

# Launch via the standard CES agent entry point
cd "$AGENT_DIR"
set +u
. entry.sh
