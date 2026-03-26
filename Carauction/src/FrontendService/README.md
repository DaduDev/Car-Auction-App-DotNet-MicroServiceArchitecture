# Frontend Service

React-based frontend for the Car Auction microservices application.

## Development

```bash
npm install
npm start
```

The app will run on http://localhost:3000 and proxy API requests to the gateway service at http://localhost:5004

## Docker

Build and run with Docker:

```bash
docker build -t frontend-svc .
docker run -p 3000:80 frontend-svc
```

Or use docker-compose from the root:

```bash
docker-compose up frontend-svc
```

## Features

- Search auctions by make, model, or color
- Filter by live, ending soon, or finished auctions
- View auction details including current bid, seller, and end time
- Responsive grid layout
