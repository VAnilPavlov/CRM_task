// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var tokenKey = "accessToken";

function filterTable() {
    var input = document.getElementById("searchInput");
    var filter = input.value.toUpperCase();
    var table = document.getElementById("employeeTable");
    var tr = table.getElementsByTagName("tr");

    for (var i = 0; i < tr.length; i++) {
        var td = tr[i].getElementsByTagName("td")[1]; 
        if (td) {
            var textValue = td.textContent || td.innerText;
            if (textValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

function openModal() {
    document.getElementById("myModal").style.display = "block";
}
 
function closeModal() {
    document.getElementById("myModal").style.display = "none";
}

function openChangeModal(obj) {
    if (selectedRow) {
        if (obj == 1) {
            editEmployee();
        }
        else {
            editTask();
        }
        document.getElementById("myModalTwo").style.display = "block";
    }
    else {
        alert('select row');
    }
}

function closeChangeModal() {
    document.getElementById("myModalTwo").style.display = "none";
}


var selectedRow;

function selectRow(row) {
    if (selectedRow) {
        selectedRow.classList.remove("selected");
    }
    selectedRow = row;
    selectedRow.classList.add("selected");
}

async function deleteSelectedRow(obj) {
    if (selectedRow) {
        var id = selectedRow.cells[0].innerText;
        var path = "/Home/DeleteEmployee"
        if (obj == 1)
            path = "/Home/DeleteTask"
        try {
            let response = await fetch(path, { 
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ Id: id })
            });
            if (response.ok) {
                let result = await response.json();

                if (result.success) {
                    selectedRow.remove();
                    selectedRow = null;
                }
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Error deleting.');
        }
    } else {
        alert("Please select a row to delete.");
    }
}

function editEmployee() {
    var name = selectedRow.cells[1].innerText;
    var title = selectedRow.cells[2].innerText;

    document.getElementById("fullNameForChange").value = name;
    document.getElementById("titleForChange").value = title;
}

function editTask() {
    var title = selectedRow.cells[1].innerText;
    var description = selectedRow.cells[2].innerText;
    var startDate = selectedRow.cells[3].innerText;
    var endDate = selectedRow.cells[4].innerText;

    document.getElementById("titleForChange").value = title;
    document.getElementById("descriptionForChange").value = description;
    document.getElementById("startDateForChange").value = startDate;
    document.getElementById("endDateForChange").value = endDate;

}

async function saveTask() {
    var urlencoded = new URLSearchParams();

    urlencoded.append("Id", selectedRow.cells[0].innerText);
    urlencoded.append("Title", document.getElementById("titleForChange").value);
    urlencoded.append("Description", document.getElementById("descriptionForChange").value);

    var startDate = selectedRow.cells[3].innerText; 
    if (document.getElementById("startDateForChange").value != "") {
        startDate =  document.getElementById("startDateForChange").value;
    }
    var endDate = selectedRow.cells[4].innerText;
    if (document.getElementById("endDateForChange").value != "") {        
        endDate = document.getElementById("endDateForChange").value;
    }

    var requestOptions = {
        method: 'POST',
        body: urlencoded,
        redirect: 'follow'
    };

    fetch("/Home/EditTasks", requestOptions)
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                selectedRow.cells[1].innerText = document.getElementById("titleForChange").value;
                selectedRow.cells[2].innerText = document.getElementById("descriptionForChange").value;
                selectedRow.cells[3].innerText = startDate;
                selectedRow.cells[4].innerText = endDate;

                closeChangeModal()
            } else {
                console.log("Failed to save employee:", result.message);
            }
        })
        .catch(error => console.log('error', error));
}

async function saveEmployee() {
    var id = selectedRow.cells[0].innerText;
    var fullName = document.getElementById("fullNameForChange").value;
    var title = document.getElementById("titleForChange").value;
  
    var urlencoded = new URLSearchParams();
    urlencoded.append("Id", id);
    urlencoded.append("FullName", fullName);
    urlencoded.append("Title", title);

    var requestOptions = {
        method: 'POST',
        body: urlencoded,
        redirect: 'follow'
    };

    fetch("/Home/EditEmployee", requestOptions)
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                selectedRow.cells[1].innerText = fullName;
                selectedRow.cells[2].innerText = title;
                closeChangeModal()
            } else {
                console.log("Failed to save employee:", result.message);
            }
        })
        .catch(error => console.log('error', error));    
}

function report()
{
    location.href = "/Home/Report";
}

function printReport()
{
    var divToPrint = document.getElementById("reportTable");
    newWin = window.open("");
    newWin.document.write(divToPrint.outerHTML);
    newWin.print();
    newWin.close();
}

function redirectOnTasks() {
    const token = sessionStorage.getItem(tokenKey);

    const requestOptions = {
        method: "GET",
        headers: {
            'Authorization': 'Bearer ' + token
        },
        redirect: "follow"
    };

    fetch("/Home/Tasks", requestOptions)
        .then((response) => response.text())
        .then((result) => {
            location.href = "/Home/Tasks";
        })
        .catch((error) => console.error(error));
}

async function addTask() {
    var id = document.getElementById("emp").value;
    var title = document.getElementById("title").value;
    var description = document.getElementById("description").value;
    var startDate = document.getElementById("startDate").value;
    var endDate = document.getElementById("endDate").value;


    var urlencoded = new URLSearchParams();
    urlencoded.append("Title", title);
    urlencoded.append("EmployeeId", id);
    urlencoded.append("Description", description);
    urlencoded.append("StartDate", startDate);
    urlencoded.append("EndDate", endDate);

    var requestOptions = {
        method: 'POST',
        body: urlencoded,
        redirect: 'follow'
    };

    fetch("/Home/Tasks", requestOptions)
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                location.reload()
            } else {
                console.log("Failed to update task:", result.message);
            }
        })
        .catch(error => console.log('error', error));
}

function login()
{
    aler(asdds);   
}

document.getElementById('loginForm').addEventListener('submit', function (event) {
    event.preventDefault(); 

    const formData = new FormData(this);   


    const urlencoded = new URLSearchParams();
    urlencoded.append("Login", formData.get("login"));
    urlencoded.append("Password", formData.get("password"));

    fetch('/Home/Login', {
        method: 'POST',
        body: urlencoded
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                sessionStorage.setItem(tokenKey, data.access_token);
                location.replace("/Home/Index");
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
});