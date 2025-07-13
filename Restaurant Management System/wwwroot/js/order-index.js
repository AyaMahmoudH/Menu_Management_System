$(document).ready(function () {
    $('#ordersTable').DataTable();

    $('.update-status').click(function () {
        var id = $(this).data('id');
        var status = $(this).data('status');

        Swal.fire({
            title: "Are you sure?",
            text: "Change order status to " + status + "?",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Yes"
        }).then((result) => {
            if (result.isConfirmed) {
                $.post('/Orders/UpdateStatus', { id: id, status: status }, function (res) {
                    if (res.success) {
                        Swal.fire("Updated!", res.message, "success").then(() => {
                            location.reload();
                        });
                    } else {
                        Swal.fire("Error", res.message, "error");
                    }
                });
            }
        });
    });
});
