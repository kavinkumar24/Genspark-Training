 
 
 if (!localStorage.getItem('token')) {
            window.location.href = "login.html";
        }

document.getElementById('logoutBtn').onclick = function() {
    localStorage.removeItem('token');
    window.location.href = "login.html";
};

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5146/fileUploadNotificationHub", {
        withCredentials: true
    })
    .withAutomaticReconnect()
    .build();

connection.on("ReceiveMessage", (user, message, time) => {
    const placeHolder = document.getElementById("msgs");
    const content = document.createElement("p");
    content.innerHTML = `<b>${user}</b> uploaded a new document at ${time}:<br>${message}`;
    placeHolder.append(content);
});

connection.start().catch(err => {
    console.log("SignalR connection Failed");
});