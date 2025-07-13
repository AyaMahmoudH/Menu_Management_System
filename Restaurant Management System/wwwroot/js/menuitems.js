$(document).ready(function () {
    $('#menuItemsTable').DataTable({ responsive: true });

    $('#menuItemForm').on('submit', function (e) {
        e.preventDefault();
        var formData = new FormData(this);
        var id = $('#menuItemId').val();
        var url = id == 0 ? '/MenuItems/Create' : '/MenuItems/Edit';

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                Swal.fire({
                    icon: res.success ? 'success' : 'error',
                    title: res.success ? 'Done' : 'Error',
                    text: res.message,
                    timer: 2000,
                    showConfirmButton: false
                }).then(() => location.reload());
            },
            error: () => Swal.fire("Error", "Something went wrong!", "error")
        });
    });
});

function editMenuItem(id) {
    $.get('/MenuItems/GetMenuItemById/' + id, function (data) {
        $('#menuItemId').val(data.id);
        $('#name').val(data.name);
        $('#description').val(data.description);
        $('#price').val(data.price);
        $('#imageUrl').val(data.imageUrl);
        $('#categoryId').val(data.categoryId);
        $('#isAvailable').prop('checked', data.isAvailable);
        $('#menuItemModalLabel').text("Edit Menu Item");
        $('#menuItemModal').modal('show');
    });
}

function deleteMenuItem(id) {
    Swal.fire({
        title: "Are you sure?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.post('/MenuItems/Delete', { id }, function (res) {
                Swal.fire(res.message, '', res.success ? "success" : "error")
                    .then(() => location.reload());
            });
        }
    });
}
