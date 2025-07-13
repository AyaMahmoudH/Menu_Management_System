document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('updateForm');

    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();

            const url = form.getAttribute('action');
            const formData = new FormData(form);

            fetch(url, {
                method: 'POST',
                body: new URLSearchParams(formData)
            })
                .then(response => response.json())
                .then(res => {
                    Swal.fire({
                        icon: res.success ? 'success' : 'error',
                        title: res.message,
                        showConfirmButton: false,
                        timer: 2000
                    }).then(() => {
                        if (res.success) {
                            window.location.href = '/Orders';
                        }
                    });
                })
                .catch(() => {
                    Swal.fire("Error", "Something went wrong", "error");
                });
        });
    }
});
