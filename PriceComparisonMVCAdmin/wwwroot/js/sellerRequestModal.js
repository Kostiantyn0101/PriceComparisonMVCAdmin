document.addEventListener("DOMContentLoaded", () => {
    const modalEl = document.getElementById("sellerInfoModal");
    const contentDiv = document.getElementById("sellerInfoContent");
    const sellerModal = new bootstrap.Modal(modalEl);

    document.querySelectorAll(".btn-view-seller-request").forEach(button => {
        button.addEventListener("click", async () => {
            const sellerId = button.getAttribute("data-id");
            contentDiv.innerHTML = "<p class='text-muted'>Завантаження...</p>";

            try {
                const response = await fetch(`/SellerRequest/GetSellerRequestInfoPartial/${sellerId}`);
                const html = await response.text();
                contentDiv.innerHTML = html;

                sellerModal.show();

                initModalButtons();
            } catch {
                contentDiv.innerHTML = `<p class="text-danger">Помилка завантаження інформації продавця.</p>`;
            }
        });
    });

    function initModalButtons() {
        const approveBtn = document.querySelector(".btn-approve");
        const rejectForm = document.querySelector("#rejectForm");
        const rejectModalEl = document.querySelector("#rejectModal");

        if (approveBtn) {
            approveBtn.addEventListener("click", async () => {
                const id = approveBtn.getAttribute("data-id");
                const payload = {
                    Id: parseInt(id),
                    IsApproved: true,
                    RefusalReason: null
                };
                await processRequest(payload);
            });
        }

        if (rejectForm) {
            rejectForm.addEventListener("submit", async (e) => {
                e.preventDefault();
                const id = document.getElementById("rejectId").value;
                const reason = document.getElementById("refusalReason").value;

                const payload = {
                    Id: parseInt(id),
                    IsApproved: false,
                    RefusalReason: reason
                };

                await processRequest(payload);

                const rejectModalInstance = bootstrap.Modal.getInstance(rejectModalEl);
                if (rejectModalInstance) rejectModalInstance.hide();
            });
        }

        if (rejectModalEl) {
            rejectModalEl.addEventListener("show.bs.modal", e => {
                const button = e.relatedTarget;
                const id = button?.getAttribute("data-id");
                if (id) document.getElementById("rejectId").value = id;
            });
        }
    }

    async function processRequest(data) {
        try {
            const res = await fetch("/SellerRequest/Approve", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            });

            if (res.ok) {
                const updatedPartial = await fetch(`/SellerRequest/GetSellerRequestInfoPartial/${data.Id}`);
                const html = await updatedPartial.text();
                document.getElementById("sellerInfoContent").innerHTML = html;

                initModalButtons();
            } else {
                alert("Помилка при обробці заявки.");
            }
        } catch (error) {
            alert("Щось пішло не так.");
            console.error(error);
        }
    }
});
