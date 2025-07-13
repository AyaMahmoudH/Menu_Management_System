let itemIndex = 1;

document.getElementById("addItem").addEventListener("click", function () {
    const orderItems = document.getElementById("orderItems");
    const html = `
        <div class="order-item mb-2">
            <select name="OrderDetails[${itemIndex}].MenuItemId" class="form-control d-inline w-25">
                ${menuOptionsHtml}
            </select>
            <input name="OrderDetails[${itemIndex}].Quantity" type="number" class="form-control d-inline w-25 mx-2" placeholder="Qty" />
            <input name="OrderDetails[${itemIndex}].SpecialRequest" type="text" class="form-control d-inline w-25" placeholder="Special Request" />
        </div>`;
    orderItems.insertAdjacentHTML("beforeend", html);
    itemIndex++;
});

document.getElementById("orderForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const form = this;
    const formData = new FormData(form);
    const data = new URLSearchParams(formData);

    fetch(form.action, {
        method: "POST",
        body: data
    }).then(res => res.json())
        .then(res => {
            if (res.success) {
                Swal.fire("Success", res.message, "success").then(() => {
                    window.location.href = "/Orders";
                });
            } else {
                Swal.fire("Error", "Failed to place order.", "error");
            }
        }).catch(() => {
            Swal.fire("Error", "Unexpected error occurred.", "error");
        });
});
