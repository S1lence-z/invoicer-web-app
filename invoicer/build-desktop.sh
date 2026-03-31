#!/bin/bash
set -e

# Build script for the Invoicer Desktop app
# Usage: ./build-desktop.sh [runtime-id]
# Example: ./build-desktop.sh win-x64
#          ./build-desktop.sh linux-x64
#          ./build-desktop.sh osx-arm64

RID=${1:-linux-x64}
OUTPUT_DIR="publish-desktop"

echo "Building Invoicer Desktop for $RID..."

# Clean previous output
rm -rf "$OUTPUT_DIR" publish-frontend

# Step 1: Publish the Frontend (Blazor WASM)
echo "Publishing Frontend..."
dotnet publish Frontend/Frontend.csproj -c Release -o publish-frontend --nologo -v q

# Step 2: Publish the Desktop app (self-contained, NOT single-file)
echo "Publishing Desktop..."
dotnet publish Desktop/Desktop.csproj \
  -c Release \
  --self-contained \
  -r "$RID" \
  -p:DebugType=none \
  -o "$OUTPUT_DIR" \
  --nologo -v q

# Step 3: Copy Frontend wwwroot into the publish output
echo "Copying Frontend assets..."
mkdir -p "$OUTPUT_DIR/wwwroot"
cp -r publish-frontend/wwwroot/* "$OUTPUT_DIR/wwwroot/"

echo ""
echo "Build complete! Output in: $OUTPUT_DIR/"
if [[ "$RID" == win-* ]]; then
  echo "Run: $OUTPUT_DIR/Desktop.exe"
else
  echo "Run: ./$OUTPUT_DIR/Desktop"
fi
