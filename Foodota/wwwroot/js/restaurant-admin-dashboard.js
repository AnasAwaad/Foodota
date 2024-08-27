var restaurantId;

// Handle form submission
function OnSuccessSubmitCreate(restaurantId) {
	var OpeningHours = [];

	$(".js-checkbox-opening-time:checked").each(function () {
		var openingTime = $(this).closest('.row').find('.js-opening-time');
		OpeningHours.push({
			"WeekDayId": $(this).val(),
			"From": openingTime.find('.js-flatpickr-from').val(),
			"To": openingTime.find('.js-flatpickr-to').val(),
			"RestaurantId": restaurantId
		});
	});

	$(".js-checkbox-opening-time").each(function () {
		if (!$(this).is(":checked")) {
			console.log($(this))
			OpeningHours.push({
				"WeekDayId": $(this).val(),
				"From": '',
				"To": '',
				"RestaurantId": restaurantId
			});
		}
	});

	// Send opening hours to the server
	$.ajax({
		url: "/admin/Restaurant/AddOpeningHours",
		contentType: "application/json;charset=utf-8",
		type: "POST",
		dataType: "json",
		data: JSON.stringify({ OpeningHours: OpeningHours }),
		success: function (res) {
			console.log('Success:', res);
			window.location.href = "/Admin/Restaurant/Index";
		},
		error: function (xhr, status, error) {
			console.error('Error submitting opening hours:', status, error);
		}
	});
};

function OnSuccessSubmitUpdate() {
	var OpeningHours = [];
	$(".js-checkbox-opening-time:checked").each(function () {
		var openingTime = $(this).closest('.row').find('.js-opening-time');
		OpeningHours.push({
			"WeekDayId": $(this).val(),
			"From": openingTime.find('.js-flatpickr-from').val(),
			"To": openingTime.find('.js-flatpickr-to').val(),
			"RestaurantId": restaurantId
		});
	});

	// Send opening hours to the server
	$.ajax({
		url: "/Admin/Restaurant/UpdateOpeningHours",
		contentType: "application/json;charset=utf-8",
		type: "POST",
		dataType: "json",
		data: JSON.stringify({ OpeningHours: OpeningHours }),
		success: function (res) {
			console.log('Success:', res);
			window.location.href = "/Admin/Restaurant/Index";
		},
		error: function (xhr, status, error) {
			console.error('Error submitting opening hours:', status, error);
		}
	});

}


$(document).ready(function () {


	// Initialize the weekDays object with correct days
	var weekDays = {
		"Saturday": 0,
		"Sunday": 0,
		"Monday": 0,
		"Tuesday": 0,
		"Wednesday": 0,
		"Thursday": 0,
		"Friday": 0
	};

	// Retrieve restaurantId from hidden input
	restaurantId = $('#Id').val();

	if (restaurantId) {
		// Fetch opening hours from the server
		$.ajax({
			url: "/Admin/Restaurant/GetOpeningHours/" + restaurantId,
			success: function (res) {
				res.openingHours.forEach(function (item) {
					// Set the checkbox and time inputs based on response
					var checkBox = $("#" + item.day + "-day");
					var openingTime = $("#" + item.day + "-opening-time");

					weekDays[item.day] = 1;

					openingTime.find('.js-flatpickr-from').val(item.from);
					openingTime.find('.js-flatpickr-to').val(item.to);
					checkBox.prop('checked', true); // Use prop instead of checked for consistency
				});

				// Update the visibility of time inputs based on weekDays object
				Object.keys(weekDays).forEach(function (day) {
					if (!weekDays[day]) {
						$("#" + day + "-day").prop('checked', false);
					}
				});

				updateVisibility();
			},
			error: function (xhr, status, error) {
				console.error('Error fetching opening hours:', status, error);
			}
		});
	}

});

