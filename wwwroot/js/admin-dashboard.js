const uri = 'https://localhost:5001';
document.addEventListener("DOMContentLoaded", getAllUsers);

function logout() {
    localStorage.removeItem("token");
    window.location.href = "../index.html";
}

function toggleAddUserModal() {
    document.getElementById("addUserModal").style.display = "block";
}

async function addUser() {
    const token = localStorage.getItem("token");
    if (!token) return;

    const user = {
        name: document.getElementById("newUserName").value,
        email: document.getElementById("newUserEmail").value,
        phone: document.getElementById("newUserPhone").value,
        address: document.getElementById("newUserAddress").value,
        password: document.getElementById("newUserPassword").value,
        permission: document.getElementById("newUserPermission").value
    };

    if (user.name && user.email && user.password)
        await fetch(`${uri}/User`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json',
                       'Authorization': `Bearer ${token}` },
            body: JSON.stringify(user)
        });

    closeModal("addUserModal");
    getAllUsers();
}

function editUser(id, name, password, email, phone, address, permission) {
    document.getElementById("editUserId").value = id;
    document.getElementById("editUserName").value = name;
    document.getElementById("editUserPassword").value = password;
    document.getElementById("editUserPhone").value = phone || "";
    document.getElementById("editUserAddress").value = address || "";
    document.getElementById("editUserEmail").value = email;
    document.getElementById("editUserPermission").value = permission;

    document.getElementById("editUserModal").style.display = "block";
}

async function updateUser() {
    const token = localStorage.getItem("token");
    if (!token) return;

    const user = {
        id: document.getElementById("editUserId").value,
        name: document.getElementById("editUserName").value,
        password: document.getElementById("editUserPassword").value,
        phone: document.getElementById("editUserPhone").value,
        address: document.getElementById("editUserAddress").value,
        email: document.getElementById("editUserEmail").value,
        permission: document.getElementById("editUserPermission").value
    };

    await fetch(`${uri}/User/${user.id}`, {
        method: 'PUT',
        headers: { 
            'Content-Type': 'application/json', 
            'Authorization': `Bearer ${token}` 
        },
        body: JSON.stringify(user)
    });

    closeModal("editUserModal");
    getAllUsers();
}

async function deleteUser(id) {
    const token = localStorage.getItem("token");
    if (!token) return;

    try {
        const response = await fetch(`${uri}/User/${id}`, {
            method: 'DELETE',
            headers: { 
                'Content-Type': 'application/json', 
                'Authorization': `Bearer ${token}` 
            }
        });

        if (!response.ok) {
            throw new Error('Failed to delete user');
        }

        getAllUsers(); 

    } catch (error) {
        throw new Error(error);
    }
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    modal.style.display = 'none';
}

async function getAllUsers() {
    const token = localStorage.getItem("token");
    if (!token) return;

    try {
        const response = await fetch(`${uri}/User`, {
            headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error("שגיאה בשליפת משתמשים");

        const users = await response.json();
        const usersList = document.getElementById("users-list");
        const usersTable = document.getElementById("users-table");
        usersTable.style.display = "table";
        usersList.innerHTML = '';

        users.forEach(async user => {
            const row = document.createElement("tr");
            const cartHasItems = await checkCart(user.id);
            row.innerHTML = `
                <td>${user.id}</td>
                <td>${user.name}</td>
                <td>${user.password}</td>
                <td>${user.email}</td>
                <td>${user.phone || '-'}</td>
                <td>${user.address || '-'}</td>
                <td>${user.permission}</td>
                <td>
                    <button onclick="editUser(${user.id}, '${user.name}','${user.password}', '${user.email}', '${user.phone}', '${user.address}', '${user.permission}')">✏️</button>
                    <button onclick="deleteUser(${user.id})" style="background: red;">🗑️</button>
                    <button onclick="openCart(${user.id})">🛒</button>
                    <button onclick="openAddJewelsModal(${user.id})">🔹</button> <!-- כפתור להוספת תכשיט לעגלה -->
                </td>
            `;
            usersList.appendChild(row);
        });

    } catch (error) {
        alert(error.message);
    }
}

async function openCart(userId) {
    const token = localStorage.getItem("token");
    if (!token) return;

    const response = await fetch(`${uri}/Cart/${userId}`, {
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` }
    });

    const cart = await response.json();
    const cartItemsList = document.getElementById("cartItems");
    cartItemsList.innerHTML = ''; 

    cart.forEach(item => {
        const listItem = document.createElement("li");
        listItem.innerHTML = `
            ${item.name} - ${item.weight}
            <button onclick="deleteItemFromCart(${userId}, ${item.id})" style="background: red;">🗑️ מחק</button>
        `;
        cartItemsList.appendChild(listItem);
    });

    document.getElementById("cartModal").style.display = 'block';
}

async function checkCart(userId) {
    const token = localStorage.getItem("token");
    if (!token) return false;

    const response = await fetch(`${uri}/Cart/${userId}`, {
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` }
    });

    return response.ok;
}


async function openAddJewelsModal(userId) {
    const token = localStorage.getItem("token");
    if (!token) return;

    const response = await fetch(`${uri}/Jewel`, {
        headers: { 'Content-Type': 'application/json',
             'Authorization': `Bearer ${token}` }
    });

    if (!response.ok) {
        alert('שגיאה בהבאת התכשיטים');
        return;
    }

    const jewels = await response.json();
    const jewelsList = document.getElementById("jewelsList");
    jewelsList.innerHTML = ''; 

    jewels.forEach(jewel => {
        const listItem = document.createElement("li");
        listItem.innerHTML = `
            <input type="checkbox" id="jewel-${jewel.id}" />
            <label for="jewel-${jewel.id}">${jewel.name} - ₪${jewel.weight}</label>
        `;
        jewelsList.appendChild(listItem);
    });

    document.getElementById("addJewelsModal").style.display = 'block';
    document.getElementById("userIdForAddingJewels").value = userId; 
}

async function addJewelsToCart() {
    const token = localStorage.getItem("token");
    if (!token) return;

    const userId = document.getElementById("userIdForAddingJewels").value;
    const jewels = [];

    document.querySelectorAll("#jewelsList input[type='checkbox']:checked").forEach((checkbox) => {
        const jewelId = checkbox.id.split('-')[1];
        jewels.push(jewelId);
    });

    if (jewels.length > 0) {
        for (let jewelId of jewels) {
            const data = {
                ID: jewelId 
            };

            await fetch(`https://localhost:5001/Cart/${userId}`, {
                method: 'POST',
                headers: { 
                    'Content-Type': 'application/json', 
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(data) 
            }).then(response => {
                if (!response.ok) {
                    throw new Error('Failed to add jewel to cart');
                }
            }).catch(error => {
                throw new Error(error)
            });
        }

        closeModal("addJewelsModal");
        openCart(userId); 
    } else {
        alert('לא נבחרו תכשיטים להוספה');
    }
}



    
document.addEventListener('DOMContentLoaded', function () {
    const cartButton = document.getElementById("cartButton");
    const modal = document.getElementById("jewelModal");
    const closeModalButton = document.getElementById("closeModal");
    const addToCartButton = document.getElementById("addToCartButton");
    const jewelList = document.getElementById("jewelList");
    const userId = document.getElementById("userId").value; 

    cartButton.addEventListener("click", function () {
        modal.style.display = "block"; 
        fetch('/api/jewels') 
            .then(response => response.json())
            .then(jewels => {
                jewelList.innerHTML = '';
                jewels.forEach(jewel => {
                    const li = document.createElement("li");
                    li.innerHTML = `<input type="checkbox" id="jewel-${jewel.id}" value="${jewel.id}" /> ${jewel.name}`;
                    jewelList.appendChild(li);
                });
            });
    });

    closeModalButton.addEventListener("click", function () {
        modal.style.display = "none";
    });

    addToCartButton.addEventListener("click", function () {
        const selectedJewels = [];
        const checkboxes = jewelList.querySelectorAll('input[type="checkbox"]:checked');
        checkboxes.forEach(checkbox => {
            selectedJewels.push(checkbox.value);
        });

        if (selectedJewels.length > 0) {
            fetch(`/api/users/${userId}/cart`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    jewelIds: selectedJewels
                })
            })
            .then(response => response.json())
            .then(data => {
                alert('Items added to cart');
                modal.style.display = "none";
            })
            .catch(error => {
                alert('Failed to add items to cart');
            });
        } else {
            alert('Please select at least one jewel');
        }
    });
});
async function deleteItemFromCart(userId, itemId) {
    const token = localStorage.getItem("token");
    if (!token) return;

    try {
        const response = await fetch(`${uri}/Cart/${userId}/${itemId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error('Failed to delete item from cart');
        }

        openCart(userId);

    } catch (error) {
        throw new Error(error);
    }
}
