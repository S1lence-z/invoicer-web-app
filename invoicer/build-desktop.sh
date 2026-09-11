#!/bin/bash
set -e

# Build script for the Invoicer Desktop app
# Usage: ./build-desktop.sh [runtime-id]
# Example: ./build-desktop.sh win-x64
#          ./build-desktop.sh linux-x64
#
# The Desktop project references the Frontend project, so the Blazor WASM assets are
# published into publish-desktop/wwwroot automatically.

RID=${1:-linux-x64}
OUTPUT_DIR="publish-desktop"

echo "Building Invoicer Desktop for $RID..."
rm -rf "$OUTPUT_DIR"

dotnet publish Desktop/Desktop.csproj \
  -c Release \
  -r "$RID" \
  --self-contained \
  -p:DebugType=none \
  -o "$OUTPUT_DIR" \
  --nologo -v q

echo ""
echo "Build complete! Output in: $OUTPUT_DIR/"
if [[ "$RID" == win-* ]]; then
  echo "Run: $OUTPUT_DIR/Invoicer.exe"
else
  echo "Run: ./$OUTPUT_DIR/Invoicer"
fi
