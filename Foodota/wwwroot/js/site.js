

// Function to update the visibility of time inputs
function updateVisibility() {
	$(".js-checkbox-opening-time").each(function () {
		var openingTime = $(this).closest('.row').find(".js-opening-time");
		if (this.checked) {
			openingTime.removeClass("d-none");
			openingTime.next().addClass("d-none");
		} else {
			openingTime.addClass("d-none");
			openingTime.next().removeClass("d-none");

		}
	});
}


// Placeholder function for error messages (to be implemented as needed)
function ShowErrorMessage() {
	// Implementation here
	console.log("error")
}



$(function () {
	// Attach event handler to checkboxes

	$(document).on('change', '.js-checkbox-opening-time', function () {
		updateVisibility();
	});


	// Initialize flatpickr for time inputs
	$(".js-flatpickr-from").flatpickr({
		enableTime: true,
		noCalendar: true,
		dateFormat: "H:i",
		defaultDate: "09:00"
	});

	$(".js-flatpickr-to").flatpickr({
		enableTime: true,
		noCalendar: true,
		dateFormat: "H:i",
		defaultDate: "23:00"
	});
});