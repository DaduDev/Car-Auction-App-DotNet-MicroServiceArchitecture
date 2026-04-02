# Car Auction

## 🚗 Car Auction Platform - Microservices Architecture

A full-stack car auction application built with .NET microservices backend and React frontend.

## 🎯 Features

- **Browse Auctions** - View all available car auctions
- **Advanced Search** - Search by make, model, or color
- **Smart Filters** - Filter by Live, Ending Soon, or Finished
- **Authentication** - Secure login with OIDC/OAuth2
- **Create Auctions** - List your car for auction (authenticated)
- **Manage Auctions** - View and delete your listings
- **Real-time Updates** - Event-driven architecture with RabbitMQ
- **Responsive Design** - Works on desktop, tablet, and mobile

## 🏗️ Architecture

### Microservices
- **Frontend Service** - React + Vite + Tailwind CSS
- **Gateway Service** - YARP Reverse Proxy
- **Identity Service** - Duende IdentityServer (OIDC)
- **Auction Service** - CRUD operations for auctions
- **Search Service** - Full-text search with MongoDB
- **Bidding Service** - (Coming soon)

### Infrastructure
- **PostgreSQL** - Auction and Identity data
- **MongoDB** - Search indexes
- **RabbitMQ** - Message bus for events
- **Docker** - Containerization
- **Nginx** - Frontend web server

## 🚀 Quick Start

### Prerequisites
- Docker & Docker Compose
- Node.js 18+ (for local frontend development)

### Run Everything

```bash
cd Carauction
docker-compose up --build
```

### Access the Application

- **Frontend:** http://localhost:3000
- **Identity Server:** http://localhost:5000
- **API Gateway:** http://localhost:5004
- **RabbitMQ Management:** http://localhost:15672 (guest/guest)

### Default Test Users

- Username: `bob` / Password: `Pass123$`
- Username: `alice` / Password: `Pass123$`

Or register a new account through the UI.

## 📚 Documentation

- **[Quick Start Guide](QUICKSTART.md)** - Get up and running fast
- **[Architecture](ARCHITECTURE.md)** - System design and data flow
- **[Frontend Summary](FRONTEND_SUMMARY.md)** - Frontend features and tech
- **[Updates](UPDATES.md)** - Recent changes and fixes
- **[Troubleshooting](TROUBLESHOOTING.md)** - Common issues and solutions

## 🛠️ Technology Stack

### Frontend
- React 18
- Vite
- Tailwind CSS
- React Router v6
- OIDC Authentication
- Axios
- React Hot Toast

### Backend
- .NET 10
- Entity Framework Core
- Duende IdentityServer
- YARP (Reverse Proxy)
- MassTransit + RabbitMQ
- AutoMapper

### Databases
- PostgreSQL 16
- MongoDB

### DevOps
- Docker
- Docker Compose
- Nginx

## 📁 Project Structure

```
Carauction/
├── src/
│   ├── AuctionService/      # Auction CRUD
│   ├── SearchService/       # Search & Filter
│   ├── IdentityService/     # Authentication
│   ├── GatewayService/      # API Gateway
│   ├── BiddingService/      # Bidding (WIP)
│   ├── FrontendService/     # React UI
│   └── Contracts/           # Shared contracts
├── docker-compose.yml       # Docker orchestration
└── Carauction.slnx         # Solution file
```

## 🔧 Development

### Backend Services

```bash
# Run with Docker
cd Carauction
docker-compose up

# Or run individually with .NET
cd src/AuctionService
dotnet run
```

### Frontend

```bash
# Development mode
cd Carauction/src/FrontendService
npm install
npm run dev

# Production build
npm run build
```

## 🧪 Testing

### Test Search Functionality

```bash
# Search by make
curl "http://localhost:5004/api/search?searchTerm=Ford"

# Search by model
curl "http://localhost:5004/api/search?searchTerm=Mustang"

# Filter by status
curl "http://localhost:5004/api/search?filterBy=live"
```

### Test Authentication

1. Go to http://localhost:3000
2. Click "Login"
3. Enter credentials
4. Try creating an auction

## 📊 API Endpoints

### Search Service
- `GET /api/search` - Search auctions
  - Query params: `searchTerm`, `filterBy`, `seller`, `winner`

### Auction Service
- `GET /api/auctions` - Get all auctions
- `GET /api/auctions/{id}` - Get auction by ID
- `POST /api/auctions` - Create auction (auth required)
- `PUT /api/auctions/{id}` - Update auction (auth required)
- `DELETE /api/auctions/{id}` - Delete auction (auth required)

### Identity Service
- `GET /.well-known/openid-configuration` - OIDC discovery
- `POST /connect/token` - Get access token

## 🔐 Security

- **Authentication:** OIDC/OAuth2 with IdentityServer
- **Authorization:** JWT Bearer tokens
- **CORS:** Configured for frontend origin
- **Protected Routes:** Create, Update, Delete require authentication

## 🎨 UI Features

- Modern gradient design
- Smooth animations
- Responsive layout
- Loading states
- Error handling
- Toast notifications
- Empty states
- Mobile menu

## 🔄 Event-Driven Architecture

Services communicate via RabbitMQ:

- `AuctionCreated` → Search Service updates index
- `AuctionUpdated` → Search Service updates index
- `AuctionDeleted` → Search Service removes from index
- `BidPlaced` → Auction Service updates bid (future)

## 🐛 Troubleshooting

See [TROUBLESHOOTING.md](TROUBLESHOOTING.md) for common issues.

Quick fixes:

```bash
# Restart all services
docker-compose restart

# Fresh start
docker-compose down -v
docker-compose up --build

# Check logs
docker-compose logs -f
```

## 📝 Recent Updates

### ✅ Fixed Search Service
- MongoDB text index now includes Model field
- Search works for make, model, and color

### ✅ Built Professional Frontend
- Vite + React 18 (replaced deprecated CRA)
- Full OIDC authentication
- Complete CRUD operations
- Modern Tailwind CSS design
- Responsive mobile layout

## 🚧 Roadmap

- [ ] Bidding Service integration
- [ ] Real-time bid updates (SignalR)
- [ ] Image upload functionality
- [ ] Email notifications
- [ ] Payment integration
- [ ] Admin dashboard
- [ ] Auction analytics

## 📄 License

This is a practice project for learning microservices architecture.

## 🤝 Contributing

This is a personal learning project, but feel free to fork and experiment!

## 📞 Support

If you encounter issues:
1. Check [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
2. Review logs: `docker-compose logs -f`
3. Try fresh start: `docker-compose down -v && docker-compose up --build`

---

**Built with ❤️ using .NET Microservices + React**
