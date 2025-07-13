const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ReceiveUserNotification", function (message) {
    Swal.fire({
        title: ' New User Registered',
        text: message,
        icon: 'info',
        confirmButtonText: 'OK'
    });
});

connection.on("ReceiveNewItemNotification", function (message) {
    Swal.fire({
        title: ' New Menu Item Added',
        text: message,
        icon: 'success',
        confirmButtonText: 'Yummy!'
    });
});

connection.start()
    .then(() => {
        console.log(" SignalR Connected.");
    })
    .catch(err => {
        console.error(" SignalR Error:", err.toString());
    });
