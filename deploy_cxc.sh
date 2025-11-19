#!/bin/bash

echo "=== CXC: Deploy iniciado ==="

cd /workspace/CXC || exit 1

echo "→ Construyendo imagen..."
docker-compose build || exit 1

echo "→ Levantando contenedor..."
docker-compose up -d --force-recreate || exit 1

echo "=== CXC desplegado correctamente ==="
