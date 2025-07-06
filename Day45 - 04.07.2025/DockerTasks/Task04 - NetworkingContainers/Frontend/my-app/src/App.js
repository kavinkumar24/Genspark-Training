import React, { useEffect, useState } from 'react';

function App() {
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchMessage = async () => {
      try {
        const res = await fetch('/api/msg');
        if (!res.ok) throw new Error(`HTTP error! Status: ${res.status}`);
        const data = await res.json();
        setMessage(data.message);
      } catch (err) {
        console.error('Failed to load API:', err.message);
        setError('Failed to connect to backend');
      }
    };

    fetchMessage();
  }, []);

  return (
    <div>
      <h1>{message || error || 'Loading...'}</h1>
    </div>
  );
}

export default App;
