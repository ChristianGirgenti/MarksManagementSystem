function filterTable(inputSelector, rowSelector, nameSelector) {
    // Get the input field and table rows
    var input = document.querySelector(inputSelector);
    var rows = document.querySelectorAll(rowSelector);

    // Add event listener to input field
    input.addEventListener("input", function () {
        var filter = input.value.toLowerCase();
        for (var i = 0; i < rows.length; i++) {
            var name = rows[i].querySelector(nameSelector).textContent.toLowerCase();
            if (name.includes(filter)) {
                rows[i].style.display = "";
            } else {
                rows[i].style.display = "none";
            }
        }
    });
}