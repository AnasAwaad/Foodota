

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

function showSuccessMessage() {

}

// Placeholder function for error messages (to be implemented as needed)
function ShowErrorMessage() {
	// Implementation here
	console.log("error")
}



function DisableSubmitButton() {
	$('body :submit').attr('disabled', 'disabled').attr('data-kt-indicator', 'on');
}

function OnBeginModal() {
	DisableSubmitButton();
}
function OnCompleteModal() {
	$('body :submit').removeAttr('disabled').removeAttr('data-kt-indicator');
	$('#Modal').modal('hide');
	$('#datatable').DataTable().draw();
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

	// Handle Modal
	$('body').on('click', '.js-render-modal', function () {
		var modal = $('#Modal');
		var btn = $(this);

		modal.modal('show');


		console.log(btn.data('title'))
		modal.find('.modal-title').html(btn.data('title'));

		$.ajax({
			url: btn.data('url'),
			method: "get",
			success: function (res) {
				$('.modal-body').html(res);
				$.validator.unobtrusive.parse(modal);
				modal.modal('hide');
			}
		})
	});

	$('body').on('click', '.js-close-model', function () {
		$('#Modal').modal('hide');

	});


});



