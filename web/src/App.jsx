import { useEffect, useMemo, useState } from 'react';

const API_BASE = (import.meta.env.VITE_API_URL || '').replace(/\/$/, '');
const MENU_ENDPOINT = API_BASE ? `${API_BASE}/api/menu` : '/menu.json';
const currency = (price) => `Rs. ${Number(price).toLocaleString('en-LK')}`;
const categories = ['All', 'Local Favourites', 'Rice & Bowls', 'Sweet Treats', 'Drinks'];

export default function App() {
  const [menu, setMenu] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [search, setSearch] = useState('');
  const [category, setCategory] = useState('All');
  const [selected, setSelected] = useState(null);
  const [cart, setCart] = useState([]);
  const [sort, setSort] = useState('featured');
  useEffect(() => {
    const controller = new AbortController();
    async function load() {
      try {
        const response = await fetch(MENU_ENDPOINT, { signal: controller.signal });
        if (!response.ok) throw new Error(`Server returned HTTP ${response.status}`);
        const json = await response.json();
        if (!Array.isArray(json)) throw new Error('API response is not a menu array');
        setMenu(json);
      } catch (e) {
        if (e.name !== 'AbortError') setError(e.message || 'Unable to load menu');
      } finally { if (!controller.signal.aborted) setLoading(false); }
    }
    load();
    return () => controller.abort();
  }, []);
  const filtered = useMemo(() => {
    const q = search.trim().toLowerCase();
    const result = menu.filter(item => (category === 'All' || item.category === category) &&
      `${item.name} ${item.description} ${item.category}`.toLowerCase().includes(q));
    if (sort === 'low') return result.sort((a,b) => a.price - b.price);
    if (sort === 'high') return result.sort((a,b) => b.price - a.price);
    return result.sort((a,b) => Number(Boolean(b.featured)) - Number(Boolean(a.featured)));
  }, [menu, search, category, sort]);
  const addToCart = (item) => setCart(previous => [...previous, item]);
  return (
    <div className="site">
      <header className="nav container">
        <a className="brand" href="#home" aria-label="CampusEats home"><span className="brand-icon">✳</span> Campus<span>Eats</span><small>STUDENT FOOD HUB</small></a>
        <nav aria-label="Main navigation"><a href="#menu">Explore menu</a><a href="#about">How it works</a></nav>
        <div className="cart-pill" aria-label={`${cart.length} items in order`}>🛍️ <span>{cart.length} items</span></div>
      </header>
      <main id="home">
        <section className="hero container">
          <div className="eyebrow"><span className="dot"/> MADE FOR CAMPUS LIFE <span className="line"/></div>
          <div className="hero-grid">
            <div><h1>Good food.<br/><em>Better breaks.</em></h1><p>From homestyle Sri Lankan favourites to quick bites between lectures. Discover something delicious, right here on campus.</p><a href="#menu" className="primary">Explore the menu <span aria-hidden="true">↗</span></a><div className="hero-meta"><span>✦ Local favourites</span><span>✦ Student-friendly prices</span></div></div>
            <div className="feature-art" aria-hidden="true"><div className="art-orbit"/><div className="art-dish">🍛</div><div className="floating-tag">TODAY'S PICK <strong>Classic Kottu</strong></div><div className="mini-tag">Freshly made ✨</div></div>
          </div>
        </section>
        <section id="menu" className="menu-section"><div className="container">
          <div className="section-top"><div><div className="eyebrow muted">OUR MENU · CAMPUS FAVOURITES</div><h2>What's cooking?</h2><p>Choose your next delicious study break.</p></div><div className="source-chip">{API_BASE ? '● Connected to cloud API' : '● Sample menu mode'}</div></div>
          <div className="tools"><label className="search">⌕ <input value={search} onChange={e => setSearch(e.target.value)} placeholder="Search dishes, flavours, ingredients…" aria-label="Search menu" /></label><select aria-label="Sort menu" value={sort} onChange={e => setSort(e.target.value)}><option value="featured">Recommended first</option><option value="low">Price: low to high</option><option value="high">Price: high to low</option></select></div>
          <div className="filters" aria-label="Menu categories">{categories.map(x => <button className={category===x?'active':''} key={x} onClick={()=>setCategory(x)}>{x}</button>)}</div>
          {loading && <p role="status" className="feedback">Loading your menu…</p>}
          {error && <div className="error" role="alert">We couldn't load the menu: {error}. {API_BASE && 'Check the VITE_API_URL, API logs and CORS configuration.'}</div>}
          {!loading && !error && filtered.length===0 && <p className="feedback">No dishes found. Try a different search or category.</p>}
          <div className="cards">{filtered.map(item => <article key={item.id} className="food-card"><div className="food-art"><span className="food-emoji" aria-hidden="true">{item.emoji || '🍽️'}</span>{item.featured && <div className="badge">POPULAR PICK</div>}</div><div className="food-body"><div className="cat">{item.category}</div><h3>{item.name}</h3><p>{item.description}</p><div className="food-footer"><strong>{currency(item.price)}</strong><button onClick={()=>{addToCart(item);setSelected(item)}} aria-label={`Add ${item.name} to order`}>Add +</button></div></div></article>)}</div>
          <p className="catalog-info">Showing {filtered.length} of {menu.length} dishes · Prices in LKR</p>
        </div></section>
        <section id="about" className="about container"><div><div className="eyebrow muted">WHY CAMPUSEATS?</div><h2>Fuel your next big idea.</h2></div><div className="steps"><div><span>01</span><h3>Browse the menu</h3><p>Explore our student favourites and discover what looks good.</p></div><div><span>02</span><h3>Choose your meal</h3><p>Use the search and category filters to find exactly what you want.</p></div><div><span>03</span><h3>Enjoy your break</h3><p>Sample ordering interface built for your SE3090 cloud deployment lab.</p></div></div></section>
      </main>
      <footer><div className="container footer-inner"><strong>✳ CampusEats</strong><span>Built for SE3090 · Lab 11 · IT24103606</span></div></footer>
      {selected && <div className="overlay" onClick={()=>setSelected(null)} role="presentation"><div className="dialog" onClick={e=>e.stopPropagation()} role="dialog" aria-modal="true" aria-label="Order update"><button onClick={()=>setSelected(null)} aria-label="Close" className="close">×</button><div className="success-symbol">✓</div><h2>Added to your selection!</h2><p>{selected.name} has been added to your demo order.</p><div className="order-note">{cart.length} item{cart.length===1?'':'s'} selected · Demo only — no checkout or payment</div><button className="primary" onClick={()=>setSelected(null)}>Keep browsing →</button></div></div>}
    </div>
  );
}
