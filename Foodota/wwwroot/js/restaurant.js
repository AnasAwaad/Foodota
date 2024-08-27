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

	const numberOfMaxChars = 20;

	/* Start slice item description */
	$('.item-desc').each(function () {
		let $this = $(this);
		let content = $this.text().trim();
		
		if (content.length > numberOfMaxChars) {
			let displayText = content.slice(0, numberOfMaxChars);
			let moreText = content.slice(numberOfMaxChars);

			$this.html(`
            ${displayText}<span class="read-more-btn">...Read more</span>
            <span class="js-more-text d-none">${moreText}</span>
            <span class="read-less-btn d-none">...Read less</span>
        `);
		}
	});

	/* Toggle read more/less functionality */
	$('body').on('click', '.read-more-btn, .read-less-btn', function () {
		let $this = $(this);
		$this.toggleClass('d-none');
		$this.siblings('.js-more-text, .read-less-btn, .read-more-btn').toggleClass('d-none');
		$('.menu-item > div').toggleClass('flex-xl-column')

	});
	
	
	/* End slice item description */


	// start render menu items
	$('.js-categories').each(function () {
		var category = $(this);
		category.on('click', function () {
			var selectedCategoryName = category.text(); // Get the category name
			$.ajax({
				url: "/Restaurant/GetCategoryItems",
				contentType: "application/json;charset=utf-8",
				type: "post",
				data: JSON.stringify({
					categoryId: category.data('categoryid'),
					restaurantId: category.data('restaurantid')
				}),
				success: function (res) {
					$('.js-render-menu-items').html(res);

					// Create a temporary container to count the items
					var tempContainer = $('<div>').html(res);
					var itemCount = tempContainer.find('.menu-item').length;

					// Update the category title with the item count
					$('.js-category-title').text(selectedCategoryName + " (" + itemCount + ")");
				}
			})
		});
	});

	// end render menu items

});




/*start scrollable tabs in restaurant details*/
const tabs = document.querySelectorAll(".scrollable-tabs-container a");
const rightArrow = document.querySelector(".scrollable-tabs-container .right-arrow svg");
const leftArrow = document.querySelector(".scrollable-tabs-container .left-arrow svg");
const tabsList = document.querySelector(".scrollable-tabs-container ul");
const leftArrowContainer = document.querySelector(".scrollable-tabs-container .left-arrow");
const rightArrowContainer = document.querySelector(".scrollable-tabs-container .right-arrow ");

const removeAllActiveClasses = () => {
	tabs.forEach((tab) => {
		tab.classList.remove("active");
	});
};

const manageIcons = () => {
	if (tabsList.scrollLeft > 20) {
		leftArrowContainer.classList.add("active");
	} else {
		leftArrowContainer.classList.remove("active");
	}
	let maxScrollValue = tabsList.scrollWidth - tabsList.clientWidth;

	
	if (tabsList.scrollLeft >= maxScrollValue) {
		rightArrowContainer.classList.remove("active");
	} else {
		rightArrowContainer.classList.add("active");
	}
};

tabs.forEach((tab) => {
	tab.addEventListener("click", function (e) {
		removeAllActiveClasses();
		tab.classList.add("active");
	});
});


rightArrow.addEventListener("click", function () {
	tabsList.scrollLeft += 200;
	manageIcons();
});

leftArrow.addEventListener("click", function () {
	tabsList.scrollLeft -= 200;
	manageIcons();
});

tabsList.addEventListener("scroll", manageIcons);

let dragging = false;

const drag = (e) => {
	if (!dragging)
		return;
	tabsList.classList.add("dragging");
	tabsList.scrollLeft -= e.movementX;
}


tabsList.addEventListener("mousedown", () => {
	dragging = true;
});

tabsList.addEventListener("mousemove", drag);

document.addEventListener("mouseup", () => {
	dragging = false;
	tabsList.classList.remove("dragging");
});

/*end scrollable tabs in restaurant details*/



