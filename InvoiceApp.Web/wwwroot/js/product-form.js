function formatThousands(number) {
    const parts = String(number).split('.');
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join('.');
}

function getRawPrice(input) {
  let value = input.value.replace(/,/g, '');
  value = value.replace(/[^0-9.]/g, "");

  if (value.includes('.')) {
    const parts = value.split(".");
    parts[1] = parts[1].slice(0, 2);
    value = parts[0] + "." + parts[1];
  }

  return value;
}

// without JQuery, so no document ready
// make sure to load the script after the element or use defer
// to make sure the dom has already loaded 
// or use document.addEventListener("DOMContentLoaded", ...)
const formattedInput = document.getElementById("PriceFormatted");
const hiddenInput = document.getElementById("Price");

formattedInput.addEventListener("input", function () {
  let raw = getRawPrice(this);
  hiddenInput.value = parseFloat(raw) || 0;

  formattedInput.value = "IDR " + formatThousands(raw);
});