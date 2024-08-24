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




"use strict";

// Handle Datatable
var KTDatatablesServerSide = function () {
	// Shared variables
	var table;
	var dt;

	// Private functions
	var initDatatable = function () {
		dt = $("#datatable").DataTable({
			searchDelay: 500,
			processing: true,
			serverSide: true,
			ajax: {
				url: "/admin/Restaurant/GetRestaurants",
				type: "POST"
			},
			columns: [
				{
					"name": "Name",
					"className": "d-flex align-items-center ",
					"render": function (data, type, row) {
						return `
										<div class="symbol symbol-50px overflow-hidden me-3">
													<a href="/Admin/Restaurant/Update/${row.id}">
												<div class="symbol-label h-75">
													<img src="${(row.logoPath === null ? '/assets/images/blank-image.svg' : row.logoPath)}" alt="${row.name}" class="w-100">
												</div>
											</a>
										</div>

										<div class="d-flex flex-column">
											<a href="/Admin/Restaurant/Update/${row.id}" class="text-primary mb-1">${row.name}</a>
										</div>
										`;
					},
					"max-width": "200px"
				},
				{ "data": "description", "name": "Description" },
				{ "data": "address", "name": "Address" },
				{
					"name": "CreatedOn",
					"render": function (data, type, row) {
						return moment(row.createdOn).format("ll");
					}
				},
				{
					"data": "isActive",
					"name": "IsActive",
					"render": function (data, type, row) {
						return `<span class=" badge badge-${row.isActive ? "success" : "danger"}">
																${row.isActive ? "Available" : "Not Available"}
															</span>`;
					}
				},
				{
					"orderable": false,
					"render": function (data, type, row) {
						return `
									<a href="#" class="btn btn-light btn-active-light-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
										Actions
										<span class="svg-icon fs-5 m-0">
											<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
												<g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
													<polygon points="0 0 24 0 24 24 0 24"></polygon>
													<path d="M6.70710678,15.7071068 C6.31658249,16.0976311 5.68341751,16.0976311 5.29289322,15.7071068 C4.90236893,15.3165825 4.90236893,14.6834175 5.29289322,14.2928932 L11.2928932,8.29289322 C11.6714722,7.91431428 12.2810586,7.90106866 12.6757246,8.26284586 L18.6757246,13.7628459 C19.0828436,14.1360383 19.1103465,14.7686056 18.7371541,15.1757246 C18.3639617,15.5828436 17.7313944,15.6103465 17.3242754,15.2371541 L12.0300757,10.3841378 L6.70710678,15.7071068 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000003, 11.999999) rotate(-180.000000) translate(-12.000003, -11.999999)"></path>
												</g>
											</svg>
										</span>
									</a>
									<!--begin::Menu-->
									<div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4" data-kt-menu="true">
										<!--begin::Menu item-->
										<div class="menu-item px-3">
											<a href="/Admin/Restaurant/Update/${row.id}" class="menu-link px-3" data-kt-docs-table-filter="edit_row">
												Edit
											</a>
										</div>
										<!--end::Menu item-->

										<!--begin::Menu item-->
										<div class="menu-item px-3">
											<a href="#" class="menu-link px-3" data-kt-docs-table-filter="delete_row">
												Delete
											</a>
										</div>
										<!--end::Menu item-->
									</div>
									<!--end::Menu-->
									`;
					}

				}
			],

		});

		table = dt.$;

		// Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
		dt.on('draw', function () {
			KTMenu.createInstances();
		});
	}

	// Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
	var handleSearchDatatable = function () {
		const filterSearch = document.getElementById('dataTableSearch');
		filterSearch.addEventListener('keyup', function (e) {
			dt.search(e.target.value).draw();
		});
	}




	// Public methods
	return {
		init: function () {
			initDatatable();
			handleSearchDatatable();
		}
	}
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
	KTDatatablesServerSide.init();

});
