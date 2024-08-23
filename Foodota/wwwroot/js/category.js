
$(function () {
	$('body').on('change','#Image', function (e) {
		var imageURL = URL.createObjectURL(e.target.files[0]);
		$('.image-input-wrapper').css('background-image', 'url(' + imageURL + ')');
		$('.image-input.image-input-empty [data-kt-image-input-action=cancel]').css("display","flex")
	})

	$('body').on('click', '#remove-image', function (e) {
		$('.image-input.image-input-empty [data-kt-image-input-action=cancel]').css("display", "none");
		$('.image-input-wrapper').css('background-image', '');

	})

})

function ShowSuccessMessage() {
	
}

// Handle Datatable
var KTDatatablesServerSide = function () {
	// Shared variables
	var table;
	var dt;

	// Private functions
	var initDatatable = function () {
		dt = $("#datatable").DataTable({
			processing: true,
			serverSide: true,
			
			ajax: {
				url: "/admin/Category/GetCategories",
				type: "POST"
			},
			columns: [
				{
					"name": "Name",
					"render": function (data, type, row) {
						return `
							<div class="d-flex align-items-center text-center">
								<div class="symbol symbol-50px overflow-hidden me-3 js-render-modal" data-title="Update Category" data-url="Category/Update/${row.id}">
											<a href="#">
										<div class="symbol-label h-75">
											<img src="${(row.imagePath === null ? '/assets/images/blank-image.svg' : row.imagePath)}" alt="${row.name}" class="w-100">
										</div>
									</a>
								</div>

								<div class="d-flex flex-column">
									<a href="#" class="text-primary mb-1 js-render-modal" data-title="Update Category" data-url="Category/Update/${row.id}">${row.name}</a>
								</div>
							</div>
										`;
					},
				},
				{
					"className": "text-center",
					"name": "CreatedOn",
					"render": function (data, type, row) {
						return moment(row.createdOn).format("ll");
					},

				},
				{
					"data": "isActive",
					"name": "IsActive",
					"className": "text-center",
					"render": function (data, type, row) {
						return `<span class=" badge badge-${row.isActive ? "success" : "danger"}">
																${row.isActive ? "Available" : "Not Available"}
								</span>`;
					}
				},
				{
					"orderable": false,
					"className": "text-center",
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
									<div href="#" class=" menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4"  data-kt-menu="true">
										<!--begin::Menu item-->
										<div class="menu-item px-3">
											<a class="menu-link px-3 js-render-modal" data-title="Update Category" data-url="Category/Update/${row.id}" data-kt-docs-table-filter="edit_row">
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