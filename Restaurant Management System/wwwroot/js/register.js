$("#registerForm").submit(function (e) {
    e.preventDefault();

    var formData = $(this).serialize(); 

    $.ajax({
        type: "POST",
        url: "/Account/Register",
        data: formData, 

        success: function (res) {
            if (res.success) {
                Swal.fire("Success", res.message, "success").then(() => {
                    window.location.href = res.redirectUrl;
                });
            } else {
                Swal.fire("Error", res.message, "error");
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText); 
            Swal.fire("Error", "Something went wrong", "error");
        }
    });
});
