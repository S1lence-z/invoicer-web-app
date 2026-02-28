#!/bin/sh
set -e

BASE_PATH="${BASE_PATH:-/}"

# Normalize: ensure leading and trailing /
case "$BASE_PATH" in /*) ;; *) BASE_PATH="/$BASE_PATH" ;; esac
case "$BASE_PATH" in */) ;; *) BASE_PATH="$BASE_PATH/" ;; esac

# Replace base href (handles any previous value for idempotency on restart)
sed -i "s|<base href=\"[^\"]*\" />|<base href=\"${BASE_PATH}\" />|g" /usr/share/nginx/html/index.html

# Write appsettings.json with API_BASE_URL from environment (empty string = use browser base address)
API_BASE_URL="${API_BASE_URL:-}"
cat > /usr/share/nginx/html/appsettings.json <<EOF
{
	"AppSpecificSettings": {
		"ApiBaseUrl": "${API_BASE_URL}"
	}
}
EOF

exec nginx -g 'daemon off;'
