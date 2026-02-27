#!/bin/sh
set -e

HTML_DIR="/usr/share/nginx/html"

# 1. Update ApiBaseUrl if API_BASE_URL env var is set
if [ -n "$API_BASE_URL" ]; then
    sed -i "s|\"ApiBaseUrl\": \"[^\"]*\"|\"ApiBaseUrl\": \"${API_BASE_URL}\"|g" "${HTML_DIR}/appsettings.json"
    echo "ApiBaseUrl set to: ${API_BASE_URL}"
fi

# 2. Update <base href> if APP_BASE_PATH env var is set
APP_BASE_PATH="${APP_BASE_PATH:-/}"
sed -i "s|<base href=\"/\"|<base href=\"${APP_BASE_PATH}\"|g" "${HTML_DIR}/index.html"

# 3. Remove the Development appsettings override
rm -f "${HTML_DIR}/appsettings.Development.json"

echo "Frontend configured: APP_BASE_PATH=${APP_BASE_PATH}"
