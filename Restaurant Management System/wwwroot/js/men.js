function submitFormAjax(formId, url) {
    var formData = new FormData($(formId)[0]);

    $.ajax({
        type: "POST",
        url: url,
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success!',
                    text: response.message
                }).then(() => {
                    if (response.redirectUrl) {
                        window.location.href = response.redirectUrl;
                    }
                });
            } else {
                let errorText = response.message || response.errors?.join("\n") || "Something went wrong.";
                Swal.fire({
                    icon: 'error',
                    title: 'Oops!',
                    text: errorText
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Server Error',
                text: 'Please try again later.'
            });
        }
    });
}
function previewImage(input, imgSelector) {
    const file = input.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            $(imgSelector).attr('src', e.target.result).removeClass('d-none');
        };
        reader.readAsDataURL(file);
    }
}

