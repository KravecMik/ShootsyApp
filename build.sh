#!/usr/bin/env bash

# Запуск от имени пользователя "shootsy":

if [ "$(id -u)" -eq 987 ]; then
    exec sudo -H -u shootsy $0 "$@"
fi
cd ~/ShootsyApp
git pull
docker compose up -d

docker ps