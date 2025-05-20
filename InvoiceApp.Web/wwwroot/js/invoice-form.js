$(function () {
    let itemIndex = $("#itemRows tr").length;

    // Add new row in editable mode
    $("#addItemBtn").on("click", function () {
        const newRow = `
            <tr data-index="${itemIndex}">
                <td>
                    <span class="display d-none"></span>
                    <input name="Items[${itemIndex}].ProductId" class="form-control edit-input" />
                </td>
                <td>
                    <span class="display d-none"></span>
                    <input name="Items[${itemIndex}].ProductName" class="form-control edit-input" />
                </td>
                <td>
                    <span class="display d-none"></span>
                    <input name="Items[${itemIndex}].Quantity" type="number" class="form-control edit-input" />
                </td>
                <td>
                    <span class="display d-none"></span>
                    <input name="Items[${itemIndex}].UnitPrice" type="number" step="0.01" class="form-control edit-input" />
                </td>
                <td>
                    <button type="button" class="btn btn-sm btn-secondary edit-row d-none">Edit</button>
                    <button type="button" class="btn btn-sm btn-success save-row">Save</button>
                    <button type="button" class="btn btn-sm btn-danger remove-item">Delete</button>
                </td>
            </tr>`;
        $("#itemRows").append(newRow);
        itemIndex++;
    });

    // Remove item
    $("#itemRows").on("click", ".remove-item", function () {
        $(this).closest("tr").remove();
    });

    // Switch to edit mode
    $("#itemRows").on("click", ".edit-row", function () {
        const row = $(this).closest("tr");
        row.find(".display").addClass("d-none");
        row.find(".edit-input").removeClass("d-none");
        row.find(".edit-row").addClass("d-none");
        row.find(".save-row").removeClass("d-none");
    });

    // Save changes and switch to read-only
    $("#itemRows").on("click", ".save-row", function () {
        const row = $(this).closest("tr");

        row.find("td").each(function () {
            const input = $(this).find("input");
            const span = $(this).find(".display");

            if (input.length && span.length) {
                span.text(input.val());
            }
        });

        row.find(".edit-input").addClass("d-none");
        row.find(".display").removeClass("d-none");
        row.find(".edit-row").removeClass("d-none");
        row.find(".save-row").addClass("d-none");
    });
});

