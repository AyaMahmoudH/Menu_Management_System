function deleteMenuItem(id) {
    Swal.fire({
        title: "Are you sure?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            const form = document.querySelector(`#delete-form-${id}`);
            const tokenInput = form.querySelector('input[name="__RequestVerificationToken"]');
            const token = tokenInput ? tokenInput.value : '';

            $.ajax({
                url: '/MenuItems/Delete',
                type: 'POST',
                data: {
                    id: id,
                    __RequestVerificationToken: token
                },
                success: function (res) {
                    if (res.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Deleted!',
                            text: res.message,
                            timer: 2000,
                            showConfirmButton: false
                        }).then(() => {
                            if (res.redirectUrl) {
                                window.location.href = res.redirectUrl;
                            } else {
                                location.reload(); 
                            }
                        });
                    } else {
                        Swal.fire("Error", res.message || "Something went wrong!", "error");
                    }
                },
                error: function () {
                    Swal.fire("Error", "Server error, please try again.", "error");
                }
            });
        }
    });
}
