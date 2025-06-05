 
 
 document.getElementById('loginform').addEventListener('submit', async function(event) {
    event.preventDefault();
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const response = await fetch("http://localhost:5146/api/Authentication/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, password })
    });
    if(response.ok) {
        const data = await response.json();
        localStorage.setItem('token', data.token);
        window.location.href = "home.html";
    } else {
        const errorData = await response.json();
        document.querySelector('.error-message').textContent = errorData.message || 'Login failed';
    }
});