function initializeClientSelect() {
  $(".client-select").select2({
    placeholder: "Search Client...",
    allowClear: true,
    // ajax: {
    //   url: "/Clients/Search",
    //   dataType: "json",
    //   delay: 250,
    //   data: function (params) {
    //     return { term: params.term };
    //   },
    //   processResults: function (data) {
    //     return {
    //       results: data,
    //     };
    //   },
    // },
  });
}

function handleClientChange(clientSelect) {
  const clientId = clientSelect.val();

  if (clientId) {
    $.get("/Clients/GetById", { id: clientId }, function (data) {
      $("#client-email").val(data.email);
      $("#client-phone").val(data.phone);
      $("#client-address").val(data.address);
    }).fail(function () {
      alert("Failed to load client data.");
    });
  } else {
    $("#client-email").val(null);
    $("#client-phone").val(null);
    $("#client-address").val(null);
  }
}

function setClientSelectOnChange() {
  $(".client-select").on("change", function () {
    handleClientChange($(this));
  });
}

// Items
let itemIndex = $("#invoice-items-body tr").length;

function toCurrencyString(number) {
  return number.toLocaleString("id-ID", {
    style: "currency",
    currency: "IDR",
  });
}

function updateInvoiceTotal() {
  let total = 0;
  $(".invoice-item-row").each(function () {
    const qty = parseFloat($(this).find(".quantity-input").val()) || 0;
    const unitPrice =
      parseFloat($(this)?.find(".product-price").data("unit-price")) || 0;
    const itemTotal = qty * unitPrice;
    $(this).find(".item-total").val(toCurrencyString(itemTotal));
    total += itemTotal;
  });

  $("#invoice-total").text(toCurrencyString(total));
}

function handleDeleteItem(row) {
  let rowNumber = $("#invoice-items-body tr").length;
  if (rowNumber > 1) {
    row.remove();
  } else {
    row.find(".product-select").val(null).trigger("change");
  }
  updateInvoiceTotal();
}

function handleClearItem(row) {
  row.find(".quantity-input").val(1);
  row.find(".item-total").val(toCurrencyString(0));
  row.find(".product-price").val("").data("unit-price", 0);
}

function initializeProductSelect(selectElement) {
  selectElement.select2({
    placeholder: "Search Product...",
    allowClear: true,
    // ajax: {
    //   url: "/Products/Search",
    //   dataType: "json",
    //   delay: 250,
    //   data: function (params) {
    //     return { term: params.term };
    //   },
    //   processResults: function (data) {
    //     return { results: data };
    //   },
    // },
  });

  selectElement.on("change", function () {
    const productId = $(this).val();
    const $select = $(this);
    const row = $(this).closest("tr");

    if (!productId) {
      handleClearItem(row);
      updateInvoiceTotal();
      return;
    }

    let isDuplicate = false;
    $(".product-select")
      .not(this)
      .each(function () {
        if ($(this).val() === productId) {
          isDuplicate = true;
          return false; // to break the loop
        }
      });

    if (isDuplicate) {
      alert("this product has already been selected");
      $(this).val(null).trigger("change"); // Reset selection
      return;
    }

    $.get("/Products/GetById", { id: productId }, function (data) {
      row.find(".quantity-input").val(1);
      row
        .find(".product-price")
        .val(toCurrencyString(data.unitPrice))
        .data("unit-price", data.unitPrice);

      // updateItemTotal(row);
      updateInvoiceTotal();
    }).fail(function () {
      row.find(".product-name, .product-price").val("");
      alert("Failed to load product info.");
    });
  });
}

function addInvoiceItemRow() {
  const template = $("#invoice-item-template").html();
  const rendered = template.replace(/{{index}}/g, itemIndex);
  const row = $(rendered);
  $("#invoice-items-body").append(row);
  initializeProductSelect(row.find(".product-select"));
  itemIndex++;
}

$(document).ready(function () {
  // for client
  initializeClientSelect();
  setClientSelectOnChange();

  handleClientChange($(".client-select"));

  $("#add-item").on("click", function () {
    addInvoiceItemRow();
  });

  // Remove row
  $("#invoice-items-body").on("click", ".remove-item", function () {
    handleDeleteItem($(this).closest("tr"));
  });

  // When quantity is changed
  $("#invoice-items-body").on("input", ".quantity-input", function () {
    updateInvoiceTotal();
  });

  // Initialize product select on first load
  initializeProductSelect($(".product-select"));
  updateInvoiceTotal();
});
