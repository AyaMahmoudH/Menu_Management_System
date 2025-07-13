$(document).ready(function () {
    $.ajax({
        url: '/Admin/GetAllCustomers',
        type: 'GET',
        success: function (data) {
            var rows = '';
            data.forEach(c => {
                rows += `<tr>
                        <td>${c.firstName}</td>
                        <td>${c.lastName}</td>
                        <td>${c.email}</td>
                    </tr>`;
            });
            $('#customersTable tbody').html(rows);
        },
        error: function (xhr) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to load customers.'
            });

            console.log("Status:", xhr.status);
            console.log("Response:", xhr.responseText);
        }
    });
});