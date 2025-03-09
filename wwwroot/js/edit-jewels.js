const uri = 'https://localhost:5001/jewel';  

window.onload = function() {
    showAllJewels();
};

// הצגת כל התכשיטים
function showAllJewels() {
    fetch(uri, {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`
        }
    })
    .then(response => response.json())
    .then(data => {
        const jewelsList = document.getElementById("jewels-list");
        jewelsList.innerHTML = '';  
        if (data.length > 0) {
            data.forEach(jewel => {
                const row = document.createElement("tr");
                row.innerHTML = `
                    <td>${jewel.id}</td>
                    <td>${jewel.name}</td>
                    <td>${jewel.weight} גרם</td>
                `;
                jewelsList.appendChild(row);
            });
        } else {
            jewelsList.innerHTML = `<tr><td colspan="3">אין תכשיטים במלאי כרגע.</td></tr>`;
        }
    })
    .catch(error => showMessage("❌ שגיאה בהצגת התכשיטים: " + error, false));
}

// הצגת הודעות למשתמש
function showMessage(message, isSuccess) {
    const messageContainer = document.getElementById("message-container");
    messageContainer.innerText = message;
    messageContainer.style.display = "block";
    messageContainer.style.padding = "10px";
    messageContainer.style.borderRadius = "5px";
    messageContainer.style.textAlign = "center";
    messageContainer.style.fontSize = "16px";
    messageContainer.style.marginTop = "10px";
    messageContainer.style.backgroundColor = isSuccess ? "#4CAF50" : "#F44336";
    messageContainer.style.color = "white";
    setTimeout(() => { messageContainer.style.display = "none"; }, 4000);
}

// הצגת טופס יצירת תכשיט
function showCreateForm() {
    hideAllForms();
    document.getElementById("create-form").classList.remove("hidden");
}

// הצגת טופס עדכון תכשיט
function showUpdateForm() {
    hideAllForms();
    document.getElementById("update-form").classList.remove("hidden");
}

// הצגת טופס מחיקת תכשיט
function showDeleteForm() {
    hideAllForms();
    document.getElementById("delete-form").classList.remove("hidden");
}

// הסתרת כל הטפסים
function hideAllForms() {
    document.getElementById("create-form").classList.add("hidden");
    document.getElementById("update-form").classList.add("hidden");
    document.getElementById("delete-form").classList.add("hidden");
}

// יצירת תכשיט חדש
function createJewel(event) {
    event.preventDefault();
    const name = document.getElementById("create-name").value;
    const weight = document.getElementById("create-weight").value;

    fetch(uri, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ name: name, weight: weight })
    })
    .then(response => {
        if (response.ok) {
            showMessage("✅ התכשיט נוצר בהצלחה!", true);
            showAllJewels();  // עדכון התכשיטים
            hideAllForms();  // סגירת הטופס
        } else {
            showMessage("❌ שגיאה ביצירת התכשיט", false);
        }
    })
    .catch(error => showMessage("❌ שגיאה ביצירת התכשיט: " + error, false));
}

// עדכון תכשיט
function updateJewel(event) {
    event.preventDefault();
    const id = document.getElementById("update-id").value;
    const name = document.getElementById("update-name").value;
    const weight = document.getElementById("update-weight").value;

    fetch(`${uri}/${id}`, {
        method: "PUT",
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ id:id,name: name, weight: weight })
    })
    .then(response => {
        if (response.ok) {
            showMessage("✅ התכשיט עודכן בהצלחה!", true);
            showAllJewels();  // עדכון התכשיטים
            hideAllForms();  // סגירת הטופס
        } else {
            showMessage("❌ שגיאה בעדכון התכשיט", false);
        }
    })
    .catch(error => showMessage("❌ שגיאה בעדכון התכשיט: " + error, false));
}

// מחיקת תכשיט
function deleteJewel(event) {
    event.preventDefault();
    const id = document.getElementById("delete-id").value;

    fetch(`${uri}/${id}`, {
        method: "DELETE",
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`
        }
    })
    .then(response => {
        if (response.ok) {
            showMessage("✅ התכשיט נמחק!", true);
            showAllJewels(); // עדכון התכשיטים
            hideAllForms();
        } else {
            showMessage("❌ שגיאה במחיקה", false);
        }
    })
    .catch(error => showMessage("❌ שגיאה במחיקה: " + error, false));
}

// פונקציה יצירת כפתור Logout
function logout() {
    localStorage.removeItem("token");
    window.location.href = "../index.html";
}

const logoutButton = document.createElement("button");
logoutButton.innerText = "🚪 Logout";
logoutButton.style.position = "absolute";
logoutButton.style.top = "10px";
logoutButton.style.right = "10px";
logoutButton.style.padding = "10px 15px";
logoutButton.style.backgroundColor = "#F44336";
logoutButton.style.color = "white";
logoutButton.style.border = "none";
logoutButton.style.borderRadius = "5px";
logoutButton.style.cursor = "pointer";
logoutButton.onclick = logout;
document.body.appendChild(logoutButton);
