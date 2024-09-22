#!/usr/bin/env bash

# Запуск от имени пользователя "shootsy":

exec sudo -H -u shootsy $0 "$@"

cd ~/ShootsyApp
git pull
docker compose up -d

docker ps