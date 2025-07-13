$("#registerForm").submit(function (e) {
    e.preventDefault();

    const model = {
        firstName: $("#FirstName").val(),
        lastName: $("#LastName").val(),
        email: $("#Email").val(),
        password: $("#Password").val(),
        confirmPassword: $("#ConfirmPassword").val()
    };

    const token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        type: "POST",
        url: "/Account/Register",
        contentType: "application/json",
        data: JSON.stringify(model),
        headers: {
            "RequestVerificationToken": token
        },
        success: function (res) {
            if (res.success) {
                Swal.fire("Success", res.message, "success").then(() => {
                    window.location.href = "/";
                });
            } else {
                Swal.fire("Error", res.message, "error");
            }
        },
        error: function (xhr) {
            Swal.fire("Error", "Something went wrong", "error");
        }
    });
});
