$("#loginForm").submit(function (e) {
    e.preventDefault();

    const model = {
        email: $("#Email").val(),
        password: $("#Password").val(),
        rememberMe: $("#RememberMe").prop("checked")
    };

    const token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        type: "POST",
        url: "/Account/Login",
        contentType: "application/json",
        data: JSON.stringify(model),
        headers: {
            "RequestVerificationToken": token
        },
        success: function (res) {
            if (res.success) {
                Swal.fire("Success", res.message, "success").then(() => {
                    window.location.href = res.redirectUrl; // ✅ استخدمي res بدل result
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