#!/bin/bash

set -e 

echo "🚀 Starting deployment..."


RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

log() {
    echo -e "${GREEN}[$(date +'%Y-%m-%d %H:%M:%S')] $1${NC}"
}

warn() {
    echo -e "${YELLOW}[$(date +'%Y-%m-%d %H:%M:%S')] WARNING: $1${NC}"
}

error() {
    echo -e "${RED}[$(date +'%Y-%m-%d %H:%M:%S')] ERROR: $1${NC}"
    exit 1
}

cd ~/ShootsyApp || error "Project directory not found"

log "📥 Pulling latest changes from Git..."
git fetch origin master

if git status -uno | grep -q 'Your branch is behind'; then
    log "New changes found, pulling updates..."
    git pull origin main
else
    log "No new changes, already up to date"
fi

log "🐳 Rebuilding Docker containers..."
docker-compose down

log "Building new images..."
docker-compose build --no-cache

log "Starting services..."
docker-compose up -d

log "⏳ Waiting for services to start..."
sleep 30

log "🔍 Checking services status..."
if docker-compose ps | grep -q "Up"; then
    log "✅ All services are running successfully"
else
    error "Some services failed to start"
fi

log "🧹 Cleaning up Docker system..."
docker image prune -f
docker container prune -f

log "📊 Final status:"
docker-compose ps

log "🎉 Deployment completed successfully!"