#!/bin/sh
set -e

API_URL="${API_URL:-http://localhost:3005}"

cat > /usr/share/nginx/html/config.json <<EOF
{
  "apiUrl": "${API_URL}"
}
EOF

exec nginx -g "daemon off;"
