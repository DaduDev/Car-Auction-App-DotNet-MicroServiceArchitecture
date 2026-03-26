import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [auctions, setAuctions] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState(false);
  const [filterBy, setFilterBy] = useState('live');

  useEffect(() => {
    fetchAuctions();
  }, [filterBy]);

  const fetchAuctions = async () => {
    setLoading(true);
    try {
      const response = await axios.get('/api/search', {
        params: { filterBy }
      });
      setAuctions(response.data.results || []);
    } catch (error) {
      console.error('Error fetching auctions:', error);
    }
    setLoading(false);
  };

  const handleSearch = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const response = await axios.get('/api/search', {
        params: { searchTerm, filterBy }
      });
      setAuctions(response.data.results || []);
    } catch (error) {
      console.error('Error searching:', error);
    }
    setLoading(false);
  };

  return (
    <div className="App">
      <header className="header">
        <h1>🚗 Car Auction</h1>
      </header>

      <div className="container">
        <div className="search-section">
          <form onSubmit={handleSearch} className="search-form">
            <input
              type="text"
              placeholder="Search by make, model, or color..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="search-input"
            />
            <button type="submit" className="search-btn">Search</button>
          </form>

          <div className="filters">
            <button 
              className={filterBy === 'live' ? 'filter-btn active' : 'filter-btn'}
              onClick={() => setFilterBy('live')}
            >
              Live Auctions
            </button>
            <button 
              className={filterBy === 'endingSoon' ? 'filter-btn active' : 'filter-btn'}
              onClick={() => setFilterBy('endingSoon')}
            >
              Ending Soon
            </button>
            <button 
              className={filterBy === 'finished' ? 'filter-btn active' : 'filter-btn'}
              onClick={() => setFilterBy('finished')}
            >
              Finished
            </button>
          </div>
        </div>

        {loading ? (
          <div className="loading">Loading...</div>
        ) : (
          <div className="auctions-grid">
            {auctions.length === 0 ? (
              <div className="no-results">No auctions found</div>
            ) : (
              auctions.map((auction) => (
                <div key={auction.id} className="auction-card">
                  <img 
                    src={auction.imageUrl || 'https://via.placeholder.com/300x200?text=No+Image'} 
                    alt={`${auction.make} ${auction.model}`}
                    className="auction-image"
                  />
                  <div className="auction-details">
                    <h3>{auction.year} {auction.make} {auction.model}</h3>
                    <p className="color">Color: {auction.color}</p>
                    <p className="mileage">Mileage: {auction.mileage.toLocaleString()} km</p>
                    <div className="auction-footer">
                      <div className="bid-info">
                        <span className="label">Current Bid:</span>
                        <span className="amount">${auction.currentHighBid || 0}</span>
                      </div>
                      <div className="status">
                        <span className={`status-badge ${auction.status.toLowerCase()}`}>
                          {auction.status}
                        </span>
                      </div>
                    </div>
                    <p className="seller">Seller: {auction.seller}</p>
                    <p className="auction-end">
                      Ends: {new Date(auction.auctionEnd).toLocaleString()}
                    </p>
                  </div>
                </div>
              ))
            )}
          </div>
        )}
      </div>
    </div>
  );
}

export default App;
