document.getElementById('loginform').addEventListener('submit', async function(event) {
   event.preventDefault();
   const email = document.getElementById('email').value;
   const password = document.getElementById('password').value;
   const role = document.getElementById('role').value;

   const response = await fetch("http://localhost:5230/api/v1/Authentication/login", {
       method: "POST",
       headers: {
           "Content-Type": "application/json"
       },
       body: JSON.stringify({ email, password, role })
   });

   const data = await response.json();

   const token = data.token || (data.data && data.data.token);

   if(response.ok && token) {
       localStorage.setItem('token', token);
       window.location.assign('home.html');
   } else {
       document.querySelector('.error-message').textContent = data.message || 'Login failed: Token not received.';
   }
});