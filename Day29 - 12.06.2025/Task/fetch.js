
const result = document.getElementById('result');

const BASE_API_URL = 'https://jsonplaceholder.typicode.com/Users'

function getUserWithCallback(){
    result.innerHTML = '';
    function fetchUser(callbackFunction) {
    result.innerHTML = '';
        fetch(BASE_API_URL)
            .then(response => response.json())
            .then(data => {
                console.log("Data fetch - by CallBack")
                callbackFunction(null, data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }

    fetchUser(function(error, data) {
        
        if(error) {
            result.innerHTML = `<p>Error: ${error.message}</p>`;
        }
        else {
            const list = document.createElement('ul');
            data.forEach(user => {
                const listItem = document.createElement('li');
                listItem.textContent = `${user.name} | ${user.email} | ${user.phone}`;
                list.appendChild(listItem);
            });
            result.innerHTML = '';
            result.appendChild(list);
        }
    });
}

function getUserWithPromise(){
    result.innerHTML = '';
    let user = new Promise((resolve, reject) => {
        fetch(BASE_API_URL)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log("Data fetch - by Promise")
                resolve(data);
            })
            .catch(error => reject(error));
    });
    user.then(data => {
        const list = document.createElement('ul');
        data.forEach(user => {
            const listItem = document.createElement('li');
            listItem.textContent = `${user.name} | ${user.email} | ${user.phone}`;
            list.appendChild(listItem);
        });
        result.innerHTML = '';
        result.appendChild(list);
    })
    .catch(error => {
        result.innerHTML = `<p>Error: ${error.message}</p>`;
    });
}

async function getUserWithAsyncAwait() {
        try{
            result.innerHTML = '';
            const response = await fetch(BASE_API_URL);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            console.log("Data fetch - by Async/Await")
            const list = document.createElement('ul');
            data.forEach(user => {
                const listItem = document.createElement('li');
                listItem.textContent = `${user.name} | ${user.email} | ${user.phone}`;
                list.appendChild(listItem);
            });
            result.innerHTML = '';
            result.appendChild(list);
        }
        catch (error) {
            result.innerHTML = `<p>Error: ${error.message}</p>`;
        }
    }