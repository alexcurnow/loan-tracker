#!/bin/bash

# Start only the database for local development
echo "🚀 Starting development database..."
docker compose -f docker-compose.dev.yml up -d

echo ""
echo "✅ Database is running on localhost:5433"
echo ""
echo "📝 Next steps:"
echo "   1. Open the project in Rider"
echo "   2. Press F5 or use 'Run' to start debugging"
echo "   3. App will run at: https://localhost:5001"
echo ""
echo "💡 To stop the database: docker compose -f docker-compose.dev.yml down"
