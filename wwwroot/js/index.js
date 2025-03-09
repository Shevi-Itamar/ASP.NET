const uri = 'https://localhost:5001';  

document.addEventListener("DOMContentLoaded", function () {
    let token = localStorage.getItem("token");
    if (token) {
        let payload = JSON.parse(atob(token.split('.')[1])); 
        if (payload.Role === "Admin") {
            window.location.href = "../html/admin-dashboard.html";
        } else {
            window.location.href = "../html/cart.html";
        }
    }
});

document.getElementById("login-form").addEventListener("submit", async function(event) {
    event.preventDefault();

    const name = document.getElementById("name").value;
    const password = document.getElementById("password").value;
    const errorMessage = document.getElementById("error-message");

    if (!name || !password) {
        errorMessage.textContent = "נא למלא את כל השדות!";
        return;
    }

    try {
        const url=`${uri}/Login/login`
        
        const response = await fetch(url, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name, password })
        });
        
        if (!response.ok) {
            throw new Error("פרטי ההתחברות שגויים");
        }
        
        const data = await response.json();
        localStorage.setItem("token", data.token);

        let payload = JSON.parse(atob(data.token.split('.')[1])); 
        if (payload.Role === "Admin") {
            window.location.href = "../html/admin-dashboard.html";
        } else {
            window.location.href = "../html/cart.html";
        }
    } catch (error) {
        errorMessage.textContent = error.message;
    }
});
