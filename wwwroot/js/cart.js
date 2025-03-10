const apiBaseUrl = 'https://localhost:5001/cart';

window.onload = function() {
    if (!localStorage.getItem("token")) {
        window.location.href = "login.html";
    } else {
        showAllJewels();
        loadCart();
    }
};

function logout() {
    localStorage.removeItem("token");
    window.location.href = "../index.html";
}

function showAllJewels() {
    fetch('https://localhost:5001/jewel', {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`
        }
    })
    .then(response => response.json())
    .then(data => {
        console.log("תכשיטים שהתקבלו:", data);

        const jewelsList = document.getElementById("jewels-list").getElementsByTagName("tbody")[0];
        jewelsList.innerHTML = '';

        if (Array.isArray(data) && data.length > 0) {
            data.forEach(jewel => {
                const row = document.createElement("tr");
                row.innerHTML = `
                    <td>${jewel.id}</td>
                    <td>${jewel.name}</td>
                    <td>${jewel.weight} גרם</td>
                    <td><button onclick="addToCart(${jewel.id}, '${jewel.name}', ${jewel.weight})">הוסף לסל</button></td>
                `;
                jewelsList.appendChild(row);
            });
        } else {
            jewelsList.innerHTML = `<tr><td colspan="4">אין תכשיטים במלאי כרגע.</td></tr>`;
        }
    })
    .catch(error => console.error("שגיאה בהבאת התכשיטים:", error));
}

function addToCart(jewelId, name, weight) {
    const userId = getUserIdFromToken(); 

    const body = {
        ID: jewelId
    };

    fetch(`${apiBaseUrl}/${userId}`, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body)
    })
    .then(response => response.text()) 
    .then(data => {
        if (data === "Item added to cart") {
            showMessage(`${name} הוסף/ה לסל!`, true);
            loadCart(); 
            showAllJewels(); 
        } else {
            showMessage("שגיאה בהוספת התכשיט לסל", false);
        }
    })
    .catch(error => {
        console.error("שגיאה בהוספת התכשיט לסל:", error);
        showMessage("שגיאה בהוספת התכשיט לסל", false);
    });
}

function showMessage(message, isSuccess) {
    const messageContainer = document.getElementById("message-container");
    messageContainer.innerText = message;
    messageContainer.style.display = "block";
    messageContainer.style.backgroundColor = isSuccess ? "#4CAF50" : "#F44336";
    messageContainer.style.color = "white";

    setTimeout(() => {
        messageContainer.style.display = "none";
    }, 6000);
}

function loadCart() {
    const userId = getUserIdFromToken(); 
    if (!userId) {
        console.error("לא ניתן למצוא את מזהה המשתמש מה-token.");
        return;
    }

    const url = `https://localhost:5001/cart/${userId}`;

    fetch(url, {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`שגיאה בטעינת הסל: ${response.status} - ${response.statusText}`);
        }
        return response.json();
    })
    .then(cart => {
        
        const cartList = document.getElementById("cart-list");
        cartList.innerHTML = ''; 

        if (Array.isArray(cart) && cart.length > 0) {
            cart.forEach(jewel => {
                const listItem = document.createElement("li");
                listItem.innerHTML = `
                    ${jewel.name} - ${jewel.weight} גרם
                    <button onclick="removeFromCart(${jewel.id})">הסר מהסל</button>
                `;
                cartList.appendChild(listItem);
            });
        } else {
            cartList.innerHTML = `<li>הסל ריק כרגע.</li>`;
        }
    })
    .catch(error => {
        console.error("שגיאה בטעינת הסל:", error.message);
        showMessage("שגיאה בטעינת הסל", false);
    });
}

function removeFromCart(jewelId) {
    const userId = getUserIdFromToken();

    fetch(`${apiBaseUrl}/${userId}/${jewelId}`, {
        method: "DELETE",
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`
        }
    })
    .then(response => response.text()) 
    .then(data => {
        if (data === "Item removed from cart") {
            loadCart(); 
            showMessage("התכשיט הוסר מהסל!", true);
            showAllJewels(); 
        } else {
            showMessage("שגיאה בהסרת התכשיט מהסל", false);
        }
    })
    .catch(error => {
        console.error("שגיאה בהסרת התכשיט מהסל:", error);
        showMessage("שגיאה בהסרת התכשיט מהסל", false);
    });
}

function getUserIdFromToken() {
    const token = localStorage.getItem("token");
    
    if (!token) {
        return null;
    }

    const payload = token.split('.')[1];
    
    const decodedPayload = atob(payload);

    const parsedPayload = JSON.parse(decodedPayload);

    return parsedPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
}
